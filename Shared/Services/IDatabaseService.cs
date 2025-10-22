using MongoDB.Driver;

namespace MillionPropertyApi.Shared.Services;

public interface IDatabaseService
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
    IMongoDatabase Database { get; }
}
