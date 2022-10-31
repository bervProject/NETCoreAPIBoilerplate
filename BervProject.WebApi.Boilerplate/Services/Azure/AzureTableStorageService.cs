
using Azure.Data.Tables;
using BervProject.WebApi.Boilerplate.ConfigModel;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public class AzureTableStorageService<T> : IAzureTableStorageService<T>
    where T : class, ITableEntity, new()
    {
        private readonly ILogger<AzureTableStorageService<T>> _logger;
        private readonly TableServiceClient _tableServiceClient;
        private readonly string _tableName;
        public AzureTableStorageService(ILogger<AzureTableStorageService<T>> logger,
                AzureConfiguration azureConfiguration,
                TableServiceClient tableServiceClient)
        {
            _logger = logger;
            _tableServiceClient = tableServiceClient;
            _tableName = typeof(T).ShortDisplayName();
        }

        public async Task CreateTableAsync()
        {
            _logger.LogInformation($"Creating table: {_tableName}");
            await _tableServiceClient.CreateTableIfNotExistsAsync(_tableName);
            _logger.LogInformation($"{_tableName} created");
        }

        public async Task UpsertAsync(T data)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            var response = await tableClient.UpsertEntityAsync<T>(data);
            if (response.IsError)
            {
                _logger.LogError(response.ReasonPhrase);
            }
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            var response = await tableClient.GetEntityAsync<T>(partitionKey, rowKey);
            return response.Value;
        }
    }
}

