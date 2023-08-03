using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Library.Api.Data;

public class MongoDbClientFactory : IMongoDbClientFactory
{
    private readonly string _connectionString;
    private IMongoClient? _client;

    public MongoDbClientFactory(string connectionString)
    {
        _connectionString = connectionString;
    }


    public IMongoClient CreateClientAsync()
    {
        if (_client is null)
        {
            _client = new MongoClient(_connectionString);
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            // convention must be registered before initialising collection
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true); 
        }
        return _client;
    }

    public IMongoDatabase GetDatabase(string name)
    {
        var client = CreateClientAsync();
        return client.GetDatabase(name);
    }
}
