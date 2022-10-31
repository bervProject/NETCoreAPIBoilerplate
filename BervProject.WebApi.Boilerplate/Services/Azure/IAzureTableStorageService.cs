namespace BervProject.WebApi.Boilerplate.Services;

using System.Threading.Tasks;

public interface IAzureTableStorageService<T>
{
    Task CreateTableAsync();
    Task UpsertAsync(T data);
    Task<T> GetAsync(string partitionKey, string rowKey);   
}