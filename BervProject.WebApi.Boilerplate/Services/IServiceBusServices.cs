using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface IServiceBusServices
    {
        Task<bool> SendMessage(string message);
    }
}
