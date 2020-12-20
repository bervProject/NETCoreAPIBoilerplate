using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface IServiceBusConsumer
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
    }
}
