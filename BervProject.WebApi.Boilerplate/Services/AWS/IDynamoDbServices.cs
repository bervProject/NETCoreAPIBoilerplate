using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.AWS
{
    public interface IDynamoDbServices
    {
        Task CreateObject();
    }
}
