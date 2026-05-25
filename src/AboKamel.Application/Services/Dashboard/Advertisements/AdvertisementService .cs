using AboKamel.Application.Contracts.Dashboard.Advertisements;
using AboKamel.Application.Dtos.Dashboard.Advertisements;
using AboKamel.Application.Dtos.Mobile.Advertisements;
using AboKamel.Domain.Entities.Advertisements;
using Capsula.Application.Contracts.Images;
using Microsoft.EntityFrameworkCore;
using Services.Core.Results;
using Services.Infrastructure.DbContexts;

namespace AboKamel.Application.Services.Dashboard.Advertisements;

public class AdvertisementService : IAdvertisementService
{
    private readonly CapsulaDbContext _context;
    private readonly IImageService _imageService;

    public AdvertisementService(CapsulaDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task<ResultAbstract<int>> CreateAsync(CreateAdvertisementRequest request)
    {
        var advertisement = new Advertisement
        {
            Name = request.Name,
            Description = request.Description,
        };

        if (request.Images != null && request.Images.Any())
        {
            foreach (var image in request.Images)
            {
                _imageService.ValidateImage(image);

                var relativePath = _imageService.GetImageRelativePath(image);

                advertisement.Images.Add(new AdvertisementImage
                {
                    Url = relativePath
                });

                await _imageService.SaveImageAsync(relativePath, image);
            }
        }

        _context.Advertisements.Add(advertisement);
        await _context.SaveChangesAsync();

        return Result.Success(advertisement.Id);
    }

    public async Task UpdateAsync(int id, UpdateAdvertisementRequest request)
    {
        var advertisement = await _context.Advertisements
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (advertisement == null)
            throw new Exception("Advertisement not found");

        advertisement.Name = request.Name;
        advertisement.Description = request.Description;

        if (request.NewImages != null && request.NewImages.Any())
        {
            foreach (var image in request.NewImages)
            {
                _imageService.ValidateImage(image);

                var relativePath = _imageService.GetImageRelativePath(image);

                advertisement.Images.Add(new AdvertisementImage
                {
                    Url = relativePath
                });

                await _imageService.SaveImageAsync(relativePath, image);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var advertisement = await _context.Advertisements
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (advertisement == null)
            throw new Exception("Advertisement not found");

        foreach (var image in advertisement.Images)
        {
            _imageService.DeleteImage(image.Url);
        }

        _context.Advertisements.Remove(advertisement);
        await _context.SaveChangesAsync();
    }

    public async Task<ResultAbstract<List<AdvertisementDto>>> SearchByNameAsync(string name)
    {
        var ads = await _context.Advertisements
            .Include(a => a.Images)
            .Where(a => a.Name.Contains(name))
            .ToListAsync();

        return Result.Success(ads.Select(a => new AdvertisementDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            ImageUrls = a.Images
                .Select(i => _imageService.GenerateUrl(i.Url)!)
                .ToList()
        }).ToList());
    }
}