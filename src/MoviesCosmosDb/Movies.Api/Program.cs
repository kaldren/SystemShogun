using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Movies.Api.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    var keyVaultEndpoint = builder.Configuration["KeyVaultUrl"];
    var keyVaultClient = new SecretClient(new Uri(keyVaultEndpoint!), new DefaultAzureCredential());
    var cosmosDbConnectionString = (await keyVaultClient.GetSecretAsync("MoviesDbConnection")).Value.Value;
    builder.Configuration["ConnectionStrings:MoviesDbConnection"] = cosmosDbConnectionString;
}

builder.Services.AddSingleton(new CosmosClient(
    connectionString: builder.Configuration.GetConnectionString("MoviesDbConnection")
));

builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>(s =>
{
    var cosmosClient = s.GetService<CosmosClient>();

    // Re-create the database
    var database = cosmosClient.CreateDatabaseIfNotExistsAsync("MoviesDb").Result;
    database.Database.CreateContainerIfNotExistsAsync("Movies", "/releaseYear").Wait();

    var container = cosmosClient.GetContainer("MoviesDb", "Movies");
    var iterator = container.GetItemQueryIterator<Movie>(new QueryDefinition("SELECT * FROM c"));
    while (iterator.HasMoreResults)
    {
        var response = iterator.ReadNextAsync().Result;
        foreach (var item in response)
        {
            container.DeleteItemAsync<Movie>(item.id, new PartitionKey(item.releaseYear)).Wait();
        }
    }

    var movies = File.ReadAllText("movies.json");
    var movieList = JsonSerializer.Deserialize<List<Movie>>(movies)!;

    foreach (var movie in movieList)
    {
        container.CreateItemAsync(movie, new PartitionKey(movie.releaseYear)).Wait();
    }

    Console.WriteLine("Database and container created. Movies data inserted.");

    return new CosmosDbService(cosmosClient);
});


// Inject ICosmosDbService below

var app = builder.Build();

app.UseHttpsRedirection();

// Get all movies by querying Cosmos DB
app.MapGet("/movies", async (ICosmosDbService cosmosDbService) =>
{
    var movies = await cosmosDbService.GetItemsAsync<Movie>("SELECT * FROM c");
    return movies;
});

// Get a movie by ID and releaseYear query
app.MapGet("/movies/{id}/{releaseYear}", async (ICosmosDbService cosmosDbService, string id, int releaseYear) =>
{
    var movie = await cosmosDbService.GetItemAsync<Movie>(id, releaseYear);
    return movie;
});

// Delete a movie by ID and partition key
app.MapDelete("/movies/{id}/{partitionKey}", async (ICosmosDbService cosmosDbService, string id, string partitionKey) =>
{
    await cosmosDbService.DeleteItemAsync<Movie>(id, int.Parse(partitionKey));
    return Results.NoContent();
});

app.Run();

// Movies record
public record Movie
{
    public string id { get; init; } // Movie ID
    public string title { get; init; } // Movie title
    public string genre { get; init; } // Movie genre
    public int releaseYear { get; init; } // Release year
    public double? rating { get; init; } // Optional movie rating
    public List<string>? cast { get; init; } // Optional list of actors
    public string? director { get; init; } // Optional director's name
    public int? durationMinutes { get; init; } // Optional movie duration in minutes
    public string? language { get; init; } // Optional language
    public SpecialFeatures? specialFeatures { get; init; } // Optional special features
    public List<string>? awards { get; init; } // Optional awards
    public List<Review>? reviews { get; init; } // Optional reviews
    public double? boxOfficeMillionUSD { get; init; } // Optional box office earnings
    public string? trailerLink { get; init; } // Optional trailer link
}

public record SpecialFeatures
{
    public bool? has4k { get; init; } // Optional flag for 4K availability
    public bool? hasSubtitles { get; init; } // Optional flag for subtitles
    public List<string>? audio { get; init; } // Optional list of audio features
    public string? commentary { get; init; } // Optional commentary description
}

public record Review
{
    public string user { get; init; } // Reviewer's username
    public string comment { get; init; } // Review comment
    public int rating { get; init; } // Review rating
}
