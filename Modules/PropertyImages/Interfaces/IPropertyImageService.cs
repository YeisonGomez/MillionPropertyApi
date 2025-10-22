using MillionPropertyApi.Modules.PropertyImages.Models;

namespace MillionPropertyApi.Modules.PropertyImages.Interfaces;

public interface IPropertyImageService
{
    Task<IEnumerable<PropertyImage>> GetAllAsync();
    Task<PropertyImage?> GetByIdAsync(string id);
    Task<IEnumerable<PropertyImage>> GetByPropertyIdAsync(string propertyId);
    Task<PropertyImage> CreateAsync(PropertyImage propertyImage);
}
