using MongoDB.Driver;
using MillionPropertyApi.Modules.PropertyTraces.Models;
using MillionPropertyApi.Modules.PropertyTraces.Interfaces;
using MillionPropertyApi.Shared.Services;

namespace MillionPropertyApi.Modules.PropertyTraces.Services;

public class PropertyTraceService : IPropertyTraceService
{
    private readonly IMongoCollection<PropertyTrace> _collection;

    public PropertyTraceService(IDatabaseService databaseService)
    {
        _collection = databaseService.GetCollection<PropertyTrace>("propertyTraces");
    }

    public async Task<IEnumerable<PropertyTrace>> GetAllAsync()
    {
        return await _collection.Find(trace => true).ToListAsync();
    }

    public async Task<PropertyTrace?> GetByIdAsync(string id)
    {
        return await _collection.Find(trace => trace.IdPropertyTrace == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(string propertyId)
    {
        return await _collection.Find(trace => trace.IdProperty == propertyId).ToListAsync();
    }

    public async Task<PropertyTrace> CreateAsync(PropertyTrace propertyTrace)
    {
        await _collection.InsertOneAsync(propertyTrace);
        return propertyTrace;
    }
}
