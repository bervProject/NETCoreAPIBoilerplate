namespace BervProject.WebApi.Boilerplate.ConfigModel
{
    public class AzureConfiguration
    {
        public AzureServiceBus ServiceBus { get; set; }
        public AzureQueueStorage QueueStorage { get; set; }
    }

    public class AzureQueueStorage
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }

    public class AzureServiceBus
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public string TopicName { get; set; }
    }
}
