using MillionPropertyApi.Modules.Properties.DTOs;
using MillionPropertyApi.Modules.Properties.Interfaces;
using MillionPropertyApi.Modules.Owners.Interfaces;
using MillionPropertyApi.Modules.Owners.DTOs;

namespace MillionPropertyApi.Modules.Properties.GraphQL;

[ExtendObjectType("Query")]
public class PropertyQuery
{
    public async Task<PaginatedPropertiesDto> GetProperties(
        [Service] IPropertyService propertyService,
        [Service] IPropertyImageService propertyImageService,
        string? query = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? page = 1,
        int? pageSize = 10)
    {
        var filter = new PropertyFilterDto
        {
            Query = query,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Page = page,
            PageSize = pageSize
        };

        var (properties, totalCount) = await propertyService.GetAllAsync(filter);
        var propertyDtos = new List<PropertyDto>();

        foreach (var property in properties)
        {
            var firstImage = await propertyImageService.GetFirstImageAsync(property.IdProperty!);
            propertyDtos.Add(new PropertyDto
            {
                IdProperty = property.IdProperty,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                IdOwner = property.IdOwner,
                FirstImage = firstImage
            });
        }

        var currentPage = page ?? 1;
        var currentPageSize = pageSize ?? 10;
        var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

        return new PaginatedPropertiesDto
        {
            Items = propertyDtos,
            TotalCount = totalCount,
            Page = currentPage,
            PageSize = currentPageSize,
            TotalPages = totalPages,
            HasNextPage = currentPage < totalPages,
            HasPreviousPage = currentPage > 1
        };
    }

    public async Task<PropertyDto?> GetProperty(
        string id, 
        [Service] IPropertyService propertyService,
        [Service] IPropertyImageService propertyImageService,
        [Service] IOwnerService ownerService)
    {
        var property = await propertyService.GetByIdAsync(id);
        if (property == null)
        {
            return null;
        }

        var firstImage = await propertyImageService.GetFirstImageAsync(property.IdProperty!);
        var allImages = await propertyImageService.GetAllImagesAsync(property.IdProperty!);
        
        var owner = await ownerService.GetByIdAsync(property.IdOwner);
        OwnerDto? ownerDto = null;
        if (owner != null)
        {
            ownerDto = new OwnerDto
            {
                IdOwner = owner.IdOwner,
                Name = owner.Name,
                Address = owner.Address,
                Photo = owner.Photo,
                Birthday = owner.Birthday
            };
        }

        return new PropertyDto
        {
            IdProperty = property.IdProperty,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            IdOwner = property.IdOwner,
            Owner = ownerDto,
            FirstImage = firstImage,
            Images = allImages
        };
    }

    public async Task<List<PropertyDto>> GetPropertiesByOwner(
        string ownerId, 
        [Service] IPropertyService propertyService,
        [Service] IPropertyImageService propertyImageService)
    {
        var properties = await propertyService.GetByOwnerIdAsync(ownerId);
        var propertyDtos = new List<PropertyDto>();

        foreach (var property in properties)
        {
            var firstImage = await propertyImageService.GetFirstImageAsync(property.IdProperty!);
            propertyDtos.Add(new PropertyDto
            {
                IdProperty = property.IdProperty,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                IdOwner = property.IdOwner,
                FirstImage = firstImage
            });
        }

        return propertyDtos;
    }
}
