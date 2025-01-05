namespace Movies.Api.Services;

using Microsoft.Azure.Cosmos;

public interface ICosmosDbService
{
    Task<IEnumerable<T>> GetItemsAsync<T>(string query);
    Task<T> GetItemAsync<T>(string id, int partitionKey);
    Task AddItemAsync<T>(T item);
    Task UpdateItemAsync<T>(string id, T item);
    Task DeleteItemAsync<T>(string id, string partitionKey);
    Task DeleteItemAsync<T>(string id, int partitionKey);
}

public class CosmosDbService : ICosmosDbService
{
    private readonly CosmosClient _cosmosClient;

    public CosmosDbService(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public async Task AddItemAsync<T>(T item)
    {
        await _cosmosClient.GetContainer("MoviesDb", "Movies").CreateItemAsync(item);
    }

    public async Task DeleteItemAsync<T>(string id, string partitionKey)
    {
        await _cosmosClient.GetContainer("MoviesDb", "Movies").DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
    }

    public async Task DeleteItemAsync<T>(string id, int partitionKey)
    {
        await _cosmosClient.GetContainer("MoviesDb", "Movies").DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
    }

    public async Task<T> GetItemAsync<T>(string id, int partitionKey)
    {
        return await _cosmosClient.GetContainer("MoviesDb", "Movies").ReadItemAsync<T>(id, new PartitionKey(partitionKey));
    }

    public async Task<IEnumerable<T>> GetItemsAsync<T>(string query)
    {
        // Query Cosmos DB
        var container = _cosmosClient.GetContainer("MoviesDb", "Movies");
        var iterator = container.GetItemQueryIterator<T>(new QueryDefinition(query));
        var results = new List<T>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }

    public async Task UpdateItemAsync<T>(string id, T item)
    {
        await _cosmosClient.GetContainer("MoviesDb", "Movies").ReplaceItemAsync(item, id);
    }
}
