using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface IServiceBusTopicSubscription
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseSubscriptionClientAsync();
    }
}
