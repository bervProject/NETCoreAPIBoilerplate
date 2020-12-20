using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface IServiceBusQueueConsumer
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
    }
}
