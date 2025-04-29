using AIDocIntelligence.WebApi.Services;
using Azure;
using Azure.AI.OpenAI;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var keyVaultUrl = builder.Configuration["KeyVaultUrl"];

var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
KeyVaultSecret secret = await client.GetSecretAsync("AzureStorageConnectionString");
var blobConnectionString = secret.Value;

var blobContainerName = builder.Configuration["AzureStorage:BlobContainerName"];

// Azure Blob Storage
builder.Services.AddSingleton(serviceProvider =>
{
    var blobServiceClient = new BlobServiceClient(blobConnectionString);

    return blobServiceClient.GetBlobContainerClient(blobContainerName);
});

// Azure Table
builder.Services.AddSingleton(new TableServiceClient(blobConnectionString));

builder.Services.AddSingleton<Func<string, TableClient>>(serviceProvider =>
{
    var tableServiceClient = serviceProvider.GetRequiredService<TableServiceClient>();
    
    return tableName => tableServiceClient.GetTableClient(tableName);
});

// Azure Open AI
// var openAiConfig = builder.Configuration.GetSection("OpenAI");

// builder.Services.AddSingleton(serviceProvider =>
// {
//     var apiKey = openAiConfig["ApiKey"];
//     var endpoint = openAiConfig["Endpoint"];

//     return new AzureOpenAIClient(
//        new Uri(endpoint!),
//        new AzureKeyCredential(apiKey!));
// });

// builder.Services.AddHttpClient<OpenAIService>((serviceProvider, client) =>
// {
//     var apiKey = openAiConfig["ApiKey"];
//     var endpoint = openAiConfig["Endpoint"];

//     client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
//     client.BaseAddress = new Uri(endpoint!);
// });

// builder.Services.AddSingleton<OpenAIService>();

// Azure Cosmos DB
// builder.Services.AddSingleton((serviceProvider) =>
// {
//     var configuration = serviceProvider.GetRequiredService<IConfiguration>();
//     var cosmosDbSection = configuration.GetSection("CosmosDb");

//     var accountEndpoint = cosmosDbSection["AccountEndpoint"];
//     var accountKey = cosmosDbSection["AccountKey"];

//     return new CosmosClient(accountEndpoint, accountKey);
// });

// builder.Services.AddSingleton(serviceProvider =>
// {
//     var configuration = serviceProvider.GetRequiredService<IConfiguration>();
//     var cosmosDbSection = configuration.GetSection("CosmosDb");

//     var databaseName = cosmosDbSection["DatabaseName"];
//     var containerName = cosmosDbSection["ContainerName"];
    
//     var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();

//     var container = cosmosClient.GetContainer(databaseName, containerName);

//     return container;
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
