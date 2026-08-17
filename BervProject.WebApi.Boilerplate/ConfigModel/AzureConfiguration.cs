namespace BervProject.WebApi.Boilerplate.ConfigModel;

/// <summary>
/// Azure Config
/// </summary>
public class AzureConfiguration
{
    /// <summary>
    /// Service Bus
    /// </summary>
    public AzureServiceBus ServiceBus { get; set; }
    /// <summary>
    /// Storage
    /// </summary>
    public AzureStorage Storage { get; set; }
}

/// <summary>
/// Azure Storage
/// </summary>
public class AzureStorage
{
    /// <summary>
    /// Queue
    /// </summary>
    public StorageQueue Queue { get; set; }
    /// <summary>
    /// Blob
    /// </summary>
    public BlobStorage Blob { get; set; }
}

/// <summary>
/// Blob
/// </summary>
public class BlobStorage
{
    /// <summary>
    /// Container Name
    /// </summary>
    public string ContainerName { get; set; }
}

/// <summary>
/// Queue
/// </summary>
public class StorageQueue
{
    /// <summary>
    /// Queue Name
    /// </summary>
    public string QueueName { get; set; }
}

/// <summary>
/// Service Bus
/// </summary>
public class AzureServiceBus
{
    /// <summary>
    /// Queue Name
    /// </summary>
    public string QueueName { get; set; }
    /// <summary>
    /// Topic Name
    /// </summary>
    public string TopicName { get; set; }
}