using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure;

/// <summary>
/// Azure Table Storage Service
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAzureTableStorageService<T>
{
    /// <summary>
    /// Create Table
    /// </summary>
    /// <returns></returns>
    Task CreateTableAsync();
    /// <summary>
    /// Upsert
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task UpsertAsync(T data);
    /// <summary>
    /// Get Data
    /// </summary>
    /// <param name="partitionKey"></param>
    /// <param name="rowKey"></param>
    /// <returns></returns>
    Task<T> GetAsync(string partitionKey, string rowKey);   
}