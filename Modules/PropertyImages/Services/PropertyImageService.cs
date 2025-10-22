using MongoDB.Driver;
using MillionPropertyApi.Modules.PropertyImages.Models;
using MillionPropertyApi.Modules.PropertyImages.Interfaces;
using MillionPropertyApi.Shared.Services;

namespace MillionPropertyApi.Modules.PropertyImages.Services;

public class PropertyImageService : IPropertyImageService
{
    private readonly IMongoCollection<PropertyImage> _collection;

    public PropertyImageService(IDatabaseService databaseService)
    {
        _collection = databaseService.GetCollection<PropertyImage>("propertyImages");
    }

    public async Task<IEnumerable<PropertyImage>> GetAllAsync()
    {
        return await _collection.Find(image => true).ToListAsync();
    }

    public async Task<PropertyImage?> GetByIdAsync(string id)
    {
        return await _collection.Find(image => image.IdPropertyImage == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(string propertyId)
    {
        return await _collection.Find(image => image.IdProperty == propertyId).ToListAsync();
    }

    public async Task<PropertyImage> CreateAsync(PropertyImage propertyImage)
    {
        await _collection.InsertOneAsync(propertyImage);
        return propertyImage;
    }
}
