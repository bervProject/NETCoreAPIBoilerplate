using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public interface IAzureQueueServices
    {
        Task<bool> SendMessage(string message);
    }
}
