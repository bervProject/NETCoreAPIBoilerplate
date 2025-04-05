namespace BervProject.WebApi.Boilerplate.ConfigModel
{
    public class AzureConfiguration
    {
        public AzureServiceBus ServiceBus { get; set; }
        public AzureStorage Storage { get; set; }
    }

    public class AzureStorage
    {
        public StorageQueue Queue { get; set; }
        public BlobStorage Blob { get; set; }
    }

    public class BlobStorage
    {
        public string ContainerName { get; set; }
    }

    public class StorageQueue
    {
        public string QueueName { get; set; }
    }

    public class AzureServiceBus
    {
        public string QueueName { get; set; }
        public string TopicName { get; set; }
    }
}
