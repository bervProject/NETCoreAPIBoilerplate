using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public interface ITopicServices
    {
        Task<bool> SendTopic(string message);
    }
}
