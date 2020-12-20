using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface ITopicServices
    {
        Task<bool> SendTopic(string message);
    }
}
