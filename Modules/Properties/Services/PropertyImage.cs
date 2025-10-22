using MongoDB.Driver;
using MillionPropertyApi.Modules.PropertyImages.Models;
using MillionPropertyApi.Modules.Properties.Interfaces;
using MillionPropertyApi.Shared.Services;

namespace MillionPropertyApi.Modules.Properties.Services;

public class PropertyImageService : IPropertyImageService
{
    private readonly IMongoCollection<PropertyImage> _collection;

    public PropertyImageService(IDatabaseService databaseService)
    {
        _collection = databaseService.GetCollection<PropertyImage>("propertyImages");
    }

    public async Task<string?> GetFirstImageAsync(string propertyId)
    {
        var firstImage = await _collection
            .Find(img => img.IdProperty == propertyId && img.Enabled)
            .FirstOrDefaultAsync();
        
        return firstImage?.File;
    }

    public async Task<List<string>> GetAllImagesAsync(string propertyId)
    {
        var images = await _collection
            .Find(img => img.IdProperty == propertyId && img.Enabled)
            .ToListAsync();
        
        return images.Select(img => img.File).ToList();
    }
}

