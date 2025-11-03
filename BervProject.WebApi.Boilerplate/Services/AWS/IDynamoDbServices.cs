namespace BervProject.WebApi.Boilerplate.Services.AWS;

using System.Threading.Tasks;

/// <summary>
/// Dynamo DB Service Interface
/// </summary>
public interface IDynamoDbServices
{
    /// <summary>
    /// Create an object
    /// </summary>
    /// <returns></returns>
    Task CreateObject();
}