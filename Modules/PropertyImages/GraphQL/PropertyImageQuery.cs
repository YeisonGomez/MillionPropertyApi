using MillionPropertyApi.Modules.PropertyImages.DTOs;
using MillionPropertyApi.Modules.PropertyImages.Interfaces;

namespace MillionPropertyApi.Modules.PropertyImages.GraphQL;

[ExtendObjectType("Query")]
public class PropertyImageQuery
{
    public async Task<List<PropertyImageDto>> GetPropertyImages([Service] IPropertyImageService propertyImageService)
    {
        var images = await propertyImageService.GetAllAsync();
        return images.Select(img => new PropertyImageDto
        {
            IdPropertyImage = img.IdPropertyImage,
            IdProperty = img.IdProperty,
            File = img.File,
            Enabled = img.Enabled
        }).ToList();
    }

    public async Task<PropertyImageDto?> GetPropertyImage(string id, [Service] IPropertyImageService propertyImageService)
    {
        var image = await propertyImageService.GetByIdAsync(id);
        if (image == null)
        {
            return null;
        }

        return new PropertyImageDto
        {
            IdPropertyImage = image.IdPropertyImage,
            IdProperty = image.IdProperty,
            File = image.File,
            Enabled = image.Enabled
        };
    }

    public async Task<List<PropertyImageDto>> GetImagesByProperty(string propertyId, [Service] IPropertyImageService propertyImageService)
    {
        var images = await propertyImageService.GetByPropertyIdAsync(propertyId);
        return images.Select(img => new PropertyImageDto
        {
            IdPropertyImage = img.IdPropertyImage,
            IdProperty = img.IdProperty,
            File = img.File,
            Enabled = img.Enabled
        }).ToList();
    }
}

