using AboKamel.Domain.Entities.SellingUnits;
using Capsula.Domain.Entities.Brands;
using Capsula.Domain.Entities.Categories;
using Capsula.Domain.Entities.Products;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using System.Text.RegularExpressions;

namespace AboKamel.Api.SeedData;

public static class ProductSeeder
{
    public static async Task SeedProductsAsync(CapsulaDbContext context, IWebHostEnvironment env)
    {
        if (await context.Products.AnyAsync())
            return;

        var excelPath = Path.Combine(env.ContentRootPath, "SeedData", "products.xlsx");
        var imagesRoot = Path.Combine(env.ContentRootPath, "wwwroot", "images", "products");

        if (!File.Exists(excelPath) || !Directory.Exists(imagesRoot))
            return;

        using var workbook = new XLWorkbook(excelPath);
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RowsUsed().Skip(1).Take(10).ToList();

        var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp", ".avif" };
        var allImages = Directory.GetFiles(imagesRoot, "*.*", SearchOption.AllDirectories)
            .Where(file => allowedExtensions.Contains(Path.GetExtension(file)))
            .ToList();

        Console.WriteLine($"Total images loaded from folder: {allImages.Count}");

        var existingProductNames = new HashSet<string>(await context.Products.Select(p => p.Name).ToListAsync(), StringComparer.OrdinalIgnoreCase);
        var categoriesCache = await context.Categories.ToDictionaryAsync(c => c.Name, c => c, StringComparer.OrdinalIgnoreCase);
        var sellingUnitsCache = await context.SellingUnits.ToDictionaryAsync(s => s.Name, s => s, StringComparer.OrdinalIgnoreCase);

        var defaultBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Default Brand");
        if (defaultBrand is null)
        {
            defaultBrand = new Brand { Name = "Default Brand", Slug = "default-brand", ImagePath = "" };
            context.Brands.Add(defaultBrand);
            await context.SaveChangesAsync();
        }

        var newProducts = new List<Product>();
        var newProductSellingUnits = new List<ProductSellingUnit>();

        foreach (var row in rows)
        {
            try
            {
                var fullDescription = row.Cell(4).GetValue<string>().Trim();
                var unitName = row.Cell(5).GetValue<string>().Trim();
                var productName = row.Cell(6).GetValue<string>().Trim();
                var categoryName = row.Cell(7).GetValue<string>().Trim();
                var priceText = row.Cell(8).GetValue<string>().Trim();
                var minQtyText = row.Cell(9).GetValue<string>().Trim();
                var maxQtyText = row.Cell(10).GetValue<string>().Trim();

                if (string.IsNullOrWhiteSpace(productName))
                    continue;

                if (existingProductNames.Contains(productName))
                    continue;

                decimal.TryParse(priceText, out decimal price);
                int.TryParse(minQtyText, out int minQty);
                int.TryParse(maxQtyText, out int maxQty);

                minQty = minQty <= 0 ? 1 : minQty;
                maxQty = maxQty <= 0 ? 10 : maxQty;

                if (!categoriesCache.TryGetValue(categoryName, out var category))
                {
                    category = new Category
                    {
                        Name = categoryName,
                        Slug = categoryName.Replace(" ", "-").ToLower(),
                        ImagePath = ""
                    };
                    context.Categories.Add(category);
                    categoriesCache[categoryName] = category;
                }

                if (!sellingUnitsCache.TryGetValue(unitName, out var sellingUnit))
                {
                    sellingUnit = new SellingUnit { Name = unitName };
                    context.SellingUnits.Add(sellingUnit);
                    sellingUnitsCache[unitName] = sellingUnit;
                }

                string? imageUrl = null;

                var cleanProduct = CleanTextForMatching(productName);
                var productWords = cleanProduct.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Where(w => w.Length > 2).ToList();

                var matchedImage = allImages.FirstOrDefault(img =>
                {
                    var imageNameOnly = Path.GetFileNameWithoutExtension(img);
                    var cleanImage = CleanTextForMatching(imageNameOnly);

                    if (string.IsNullOrEmpty(cleanImage))
                        return false;

                    if (cleanImage == cleanProduct || cleanImage.Contains(cleanProduct) || cleanProduct.Contains(cleanImage))
                        return true;

                    if (productWords.Any())
                    {
                        var matchesCount = productWords.Count(word => cleanImage.Contains(word));

                        if (matchesCount >= 3)
                            return true;

                        if (matchesCount == 2)
                        {
                            var matchedWords = productWords.Where(word => cleanImage.Contains(word)).ToList();
                            if (matchedWords.Count == 2)
                            {
                                return cleanImage.Contains(matchedWords[0]) && cleanImage.Contains(matchedWords[1]);
                            }
                        }
                    }

                    return false;
                });

                if (matchedImage is not null)
                {
                    var relativePath = Path.GetRelativePath(Path.Combine(env.ContentRootPath, "wwwroot"), matchedImage);
                    imageUrl = "/" + relativePath.Replace("\\", "/");
                    Console.WriteLine($"MATCH FOUND: {productName} -> Path: {imageUrl}");
                }
                else
                {
                    Console.WriteLine($"Product Name: {productName} | NO MATCH FOUND");
                }

                var product = new Product
                {
                    Name = productName,
                    Description = fullDescription,
                    ImagePath = imageUrl,
                    MinimumQuantityPerOrder = minQty,
                    MaximumQuantityPerOrder = maxQty,
                    Brand = defaultBrand,
                    Category = category
                };

                newProducts.Add(product);
                existingProductNames.Add(productName);

                var productSellingUnit = new ProductSellingUnit
                {
                    Product = product,
                    SellingUnit = sellingUnit,
                    Price = price,
                    Quantity = 1
                };

                newProductSellingUnits.Add(productSellingUnit);
            }
            catch
            {
                continue;
            }
        }

        if (newProducts.Any())
        {
            context.Products.AddRange(newProducts);
            context.ProductSellingUnits.AddRange(newProductSellingUnits);
            await context.SaveChangesAsync();
            Console.WriteLine($"Successfully seeded {newProducts.Count} products with their images!");
        }
    }

    private static string CleanTextForMatching(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text.Trim().ToLower();
        text = Regex.Replace(text, @"[\d\.]+", "");

        return text
            .Replace("أ", "ا").Replace("إ", "ا").Replace("آ", "ا")
            .Replace("ة", "ه").Replace("ى", "ي")
            .Replace("-", " ").Replace("_", " ")
            .Replace("(", "").Replace(")", "")
            .Replace("[", "").Replace("]", "")
            .Replace(".", "").Replace(",", "").Replace("،", "")
            .Replace("قطعة", "").Replace("قطعه", "")
            .Replace("علبة", "").Replace("علبه", "").Replace("علب", "")
            .Replace("كرتونة", "").Replace("كرتونه", "")
            .Replace("كجم", "").Replace("كيلو", "").Replace("جرام", "").Replace("جم", "")
            .Replace("ج", "").Replace("مل", "").Replace("لتر", "")
            .Replace("بسكويت", "").Replace("ويفر", "")
            .Replace("مشروب", "").Replace("غازي", "").Replace("غازى", "")
            .Trim();
    }
}