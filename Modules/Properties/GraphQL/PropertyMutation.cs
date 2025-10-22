using MillionPropertyApi.Modules.Properties.DTOs;
using MillionPropertyApi.Modules.Properties.Models;
using MillionPropertyApi.Modules.Properties.Interfaces;

namespace MillionPropertyApi.Modules.Properties.GraphQL;

[ExtendObjectType("Mutation")]
public class PropertyMutation
{
    public async Task<PropertyDto> CreateProperty(
        CreatePropertyDto input,
        [Service] IPropertyService propertyService,
        [Service] IPropertyImageService propertyImageService)
    {
        var property = new Property
        {
            Name = input.Name,
            Address = input.Address,
            Price = input.Price,
            CodeInternal = input.CodeInternal,
            Year = input.Year,
            IdOwner = input.IdOwner
        };

        var createdProperty = await propertyService.CreateAsync(property);
        var firstImage = await propertyImageService.GetFirstImageAsync(createdProperty.IdProperty!);

        return new PropertyDto
        {
            IdProperty = createdProperty.IdProperty,
            Name = createdProperty.Name,
            Address = createdProperty.Address,
            Price = createdProperty.Price,
            CodeInternal = createdProperty.CodeInternal,
            Year = createdProperty.Year,
            IdOwner = createdProperty.IdOwner,
            FirstImage = firstImage
        };
    }
}
