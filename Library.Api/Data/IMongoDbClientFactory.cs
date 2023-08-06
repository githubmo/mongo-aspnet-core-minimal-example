using MongoDB.Driver;

namespace Library.Api.Data;

public interface IMongoDbClientFactory
{
    IMongoClient CreateClient();

    IMongoDatabase GetDatabase(string name);

}
