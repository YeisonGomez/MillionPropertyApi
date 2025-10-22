using MongoDB.Driver;

namespace MillionPropertyApi.Shared.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IMongoDatabase _database;

    public DatabaseService(IMongoDatabase database)
    {
        _database = database;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    public IMongoDatabase Database => _database;
}
