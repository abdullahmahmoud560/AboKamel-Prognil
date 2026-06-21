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
        
        // Print first row (headers) and first 5 data rows to see columns
        Console.WriteLine("=== Excel Headers ===");
        var headerRow = worksheet.Row(1);
        for (int i = 1; i <= 20; i++)
        {
            Console.WriteLine($"Column {i}: {headerRow.Cell(i).GetValue<string>()}");
        }
        Console.WriteLine("\n=== First 5 Data Rows ===");
        for (int r = 2; r <= Math.Min(6, worksheet.RowsUsed().Count()); r++)
        {
            var row = worksheet.Row(r);
            Console.Write($"Row {r}: ");
            for (int c = 1; c <= 20; c++)
            {
                Console.Write($"[{c}:{row.Cell(c).GetValue<string>()}] ");
            }
            Console.WriteLine();
        }
        
        var rows = worksheet.RowsUsed().Skip(1).ToList();

        var allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp", ".avif" };
        var allImages = Directory.GetFiles(imagesRoot, "*.*", SearchOption.AllDirectories)
            .Where(file => allowedExtensions.Contains(Path.GetExtension(file)))
            .ToList();

        Console.WriteLine($"Total images loaded from folder: {allImages.Count}");

        var existingProductNames = new HashSet<string>(await context.Products.Select(p => p.Name).ToListAsync(), StringComparer.OrdinalIgnoreCase);
        var categoriesCache = await context.Categories.ToDictionaryAsync(c => c.Name, c => c, StringComparer.OrdinalIgnoreCase);
        var sellingUnitsCache = await context.SellingUnits.ToDictionaryAsync(s => s.Name, s => s, StringComparer.OrdinalIgnoreCase);
        var brandsList = await context.Brands.ToListAsync();
        var brandsCache = brandsList.GroupBy(b => b.Name, StringComparer.OrdinalIgnoreCase).ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        var defaultBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "Default Brand");
        if (defaultBrand is null)
        {
            defaultBrand = new Brand { Name = "Default Brand", Slug = "default-brand", ImagePath = "" };
            context.Brands.Add(defaultBrand);
            await context.SaveChangesAsync();
            brandsCache[defaultBrand.Name] = defaultBrand;
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

                // --- GET BRAND FROM EXCEL FIRST ---
                // Check cells 1, 2, 3 for brand name (we can see which one after printing headers!)
                var brandNameFromExcel = string.Empty;
                for (int c = 1; c <= 5; c++)
                {
                    var possibleBrand = row.Cell(c).GetValue<string>()?.Trim();
                    if (!string.IsNullOrWhiteSpace(possibleBrand))
                    {
                        // Try exact match first
                        if (brandsCache.TryGetValue(possibleBrand, out _))
                        {
                            brandNameFromExcel = possibleBrand;
                            break;
                        }
                        // If not, try case-insensitive match with the cache keys
                        var matchKey = brandsCache.Keys.FirstOrDefault(k => string.Equals(k, possibleBrand, StringComparison.OrdinalIgnoreCase));
                        if (matchKey != null)
                        {
                            brandNameFromExcel = matchKey;
                            break;
                        }
                    }
                }

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

                Brand? productBrand = defaultBrand;
                Category? productCategory = category;
                
                // --- MAIN: Get Brand from Excel ---
                if (!string.IsNullOrWhiteSpace(brandNameFromExcel) && brandsCache.TryGetValue(brandNameFromExcel, out var brandFromExcel))
                {
                    productBrand = brandFromExcel;
                    Console.WriteLine($"✅ SUCCESS: Product '{productName}' assigned to Brand '{productBrand.Name}' (ID: {productBrand.Id})");
                }
                else
                {
                    Console.WriteLine($"⚠️ WARNING: No Brand found in Excel for Product '{productName}' - using Default Brand!");
                }
                
                if (matchedImage is not null)
                {
                    var relativePath = Path.GetRelativePath(Path.Combine(env.ContentRootPath, "wwwroot"), matchedImage);
                    imageUrl = "/" + relativePath.Replace("\\", "/");
                    Console.WriteLine($"MATCH FOUND: {productName} -> Path: {imageUrl}");

                    // Extract directory parts to get Category and Brand
                    var directoryParts = Path.GetDirectoryName(matchedImage)
                        .Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                    
                    // Find the index where the "products" directory is
                    int productsIndex = Array.FindIndex(directoryParts, p => p.Equals("products", StringComparison.OrdinalIgnoreCase));
                    
                    if (productsIndex != -1 && productsIndex + 1 < directoryParts.Length)
                    {
                        // The first directory after "products" is our Category!
                        var categoryNameFromPath = directoryParts[productsIndex + 1];
                        
                        // Check if this category exists or create it
                        if (!categoriesCache.TryGetValue(categoryNameFromPath, out var foundCategory))
                        {
                            foundCategory = new Category 
                            { 
                                Name = categoryNameFromPath, 
                                Slug = categoryNameFromPath.Replace(" ", "-").ToLower(), 
                                ImagePath = "" 
                            };
                            context.Categories.Add(foundCategory);
                            await context.SaveChangesAsync();
                            categoriesCache[categoryNameFromPath] = foundCategory;
                        }
                        
                        productCategory = foundCategory;
                        
                        // SECOND: If NO Brand from Excel, try to find from folders
                        if (string.IsNullOrWhiteSpace(brandNameFromExcel))
                        {
                            // Find Brand - look from the end backwards
                            for (int i = directoryParts.Length - 1; i > productsIndex + 1; i--)
                            {
                                var possibleBrandName = directoryParts[i];
                                if (brandsCache.TryGetValue(possibleBrandName, out var foundBrand))
                                {
                                    productBrand = foundBrand;
                                    Console.WriteLine($"USING FOLDER BRAND for {productName}: {productBrand.Name}");
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Product Name: {productName} -> NO MATCH FOUND");
                }

                var product = new Product
                {
                    Name = productName,
                    Description = fullDescription,
                    ImagePath = imageUrl,
                    MinimumQuantityPerOrder = minQty,
                    MaximumQuantityPerOrder = maxQty,
                    Brand = productBrand,
                    Category = productCategory
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
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        text = text.Trim().ToLower();
        text = Regex.Replace(text, @"[\d\.]+", "");
        return text.Replace("أ", "ا").Replace("إ", "ا").Replace("آ", "ا").Replace("ة", "ه").Replace("ى", "ي")
                   .Replace("-", " ").Replace("_", " ").Replace(".", "").Replace("قطعة", "").Replace("علبة", "").Trim();
    }
}