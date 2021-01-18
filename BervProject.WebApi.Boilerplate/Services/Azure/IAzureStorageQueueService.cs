namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public interface IAzureStorageQueueService
    {
        bool SendMessage(string message);
        string ReceiveMessage();
    }
}
