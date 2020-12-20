using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface IQueueServices
    {
        Task<bool> SendMessage(string message);
    }
}
