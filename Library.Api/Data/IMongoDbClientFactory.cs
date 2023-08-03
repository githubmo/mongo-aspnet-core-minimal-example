using MongoDB.Driver;

namespace Library.Api.Data;

public interface IMongoDbClientFactory
{
    IMongoClient CreateClientAsync();

    IMongoDatabase GetDatabase(string name);

}
