using MongoDB.Driver;
using MillionPropertyApi.Modules.Owners.Models;
using MillionPropertyApi.Modules.Owners.Interfaces;
using MillionPropertyApi.Shared.Services;

namespace MillionPropertyApi.Modules.Owners.Services;

public class OwnerService : IOwnerService
{
    private readonly IMongoCollection<Owner> _collection;

    public OwnerService(IDatabaseService databaseService)
    {
        _collection = databaseService.GetCollection<Owner>("owners");
    }

    public async Task<IEnumerable<Owner>> GetAllAsync()
    {
        return await _collection.Find(owner => true).ToListAsync();
    }

    public async Task<Owner?> GetByIdAsync(string id)
    {
        return await _collection.Find(owner => owner.IdOwner == id).FirstOrDefaultAsync();
    }

    public async Task<Owner> CreateAsync(Owner owner)
    {
        await _collection.InsertOneAsync(owner);
        return owner;
    }
}
