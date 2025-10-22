using MongoDB.Driver;
using MillionPropertyApi.Modules.Properties.Models;
using MillionPropertyApi.Modules.Properties.DTOs;
using MillionPropertyApi.Modules.Properties.Interfaces;
using MillionPropertyApi.Shared.Services;

namespace MillionPropertyApi.Modules.Properties.Services;

public class PropertyService : IPropertyService
{
    private readonly IMongoCollection<Property> _collection;

    public PropertyService(IDatabaseService databaseService)
    {
        _collection = databaseService.GetCollection<Property>("properties");
    }

    public async Task<(IEnumerable<Property> properties, int totalCount)> GetAllAsync(PropertyFilterDto? filter = null)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        if (!string.IsNullOrWhiteSpace(filter?.Query))
        {
            var queryRegex = new MongoDB.Bson.BsonRegularExpression(filter.Query, "i");
            var nameFilter = filterBuilder.Regex(p => p.Name, queryRegex);
            var addressFilter = filterBuilder.Regex(p => p.Address, queryRegex);
            
            filters.Add(filterBuilder.Or(nameFilter, addressFilter));
        }

        if (filter?.MinPrice.HasValue == true)
        {
            filters.Add(filterBuilder.Gte(p => p.Price, filter.MinPrice.Value));
        }

        if (filter?.MaxPrice.HasValue == true)
        {
            filters.Add(filterBuilder.Lte(p => p.Price, filter.MaxPrice.Value));
        }

        var combinedFilter = filters.Count > 0 
            ? filterBuilder.And(filters) 
            : filterBuilder.Empty;

        var totalCount = await _collection.CountDocumentsAsync(combinedFilter);

        var page = filter?.Page ?? 1;
        var pageSize = filter?.PageSize ?? 10;
        var skip = (page - 1) * pageSize;

        var properties = await _collection
            .Find(combinedFilter)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();

        return (properties, (int)totalCount);
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        return await _collection.Find(property => property.IdProperty == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Property>> GetByOwnerIdAsync(string ownerId)
    {
        return await _collection.Find(property => property.IdOwner == ownerId).ToListAsync();
    }

    public async Task<Property> CreateAsync(Property property)
    {
        await _collection.InsertOneAsync(property);
        return property;
    }
}
