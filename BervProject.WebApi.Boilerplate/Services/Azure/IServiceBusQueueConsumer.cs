using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public interface IServiceBusQueueConsumer
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
    }
}
