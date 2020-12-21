using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public interface IServiceBusTopicSubscription
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseSubscriptionClientAsync();
    }
}
