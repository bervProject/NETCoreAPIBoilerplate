
using Azure.Data.Tables;

namespace BervProject.WebApi.Boilerplate.Services.Azure;

using ConfigModel;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

/// <inheritdoc />
public class AzureTableStorageService<T> : IAzureTableStorageService<T>
    where T : class, ITableEntity, new()
{
    private readonly ILogger<AzureTableStorageService<T>> _logger;
    private readonly TableServiceClient _tableServiceClient;
    private readonly string _tableName;

    /// <summary>
    /// Default Constructor with dependency injections
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="tableServiceClient"></param>
    public AzureTableStorageService(ILogger<AzureTableStorageService<T>> logger,
        TableServiceClient tableServiceClient)
    {
        _logger = logger;
        _tableServiceClient = tableServiceClient;
        _tableName = typeof(T).ShortDisplayName();
    }

    /// <inheritdoc />
    public async Task CreateTableAsync()
    {
        _logger.LogInformation($"Creating table: {_tableName}");
        await _tableServiceClient.CreateTableIfNotExistsAsync(_tableName);
        _logger.LogInformation($"{_tableName} created");
    }

    /// <inheritdoc />
    public async Task UpsertAsync(T data)
    {
        var tableClient = _tableServiceClient.GetTableClient(_tableName);
        var response = await tableClient.UpsertEntityAsync<T>(data);
        if (response.IsError)
        {
            _logger.LogError(response.ReasonPhrase);
        }
    }

    /// <inheritdoc />
    public async Task<T> GetAsync(string partitionKey, string rowKey)
    {
        var tableClient = _tableServiceClient.GetTableClient(_tableName);
        var response = await tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        return response.Value;
    }
}