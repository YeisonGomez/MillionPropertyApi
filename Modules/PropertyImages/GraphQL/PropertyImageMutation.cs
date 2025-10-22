using MillionPropertyApi.Modules.PropertyImages.DTOs;
using MillionPropertyApi.Modules.PropertyImages.Interfaces;
using MillionPropertyApi.Modules.PropertyImages.Models;

namespace MillionPropertyApi.Modules.PropertyImages.GraphQL;

[ExtendObjectType("Mutation")]
public class PropertyImageMutation
{
    public async Task<PropertyImageDto> CreatePropertyImage(CreatePropertyImageDto input, [Service] IPropertyImageService propertyImageService)
    {
        var image = new PropertyImage
        {
            IdProperty = input.IdProperty,
            File = input.File,
            Enabled = input.Enabled
        };

        var createdImage = await propertyImageService.CreateAsync(image);

        return new PropertyImageDto
        {
            IdPropertyImage = createdImage.IdPropertyImage,
            IdProperty = createdImage.IdProperty,
            File = createdImage.File,
            Enabled = createdImage.Enabled
        };
    }
}
