namespace BervProject.WebApi.Boilerplate.Entities;

using System;
using Azure;
using Azure.Data.Tables;

/// <summary>
/// Note
/// </summary>
public class Note : ITableEntity
{
    /// <summary>
    /// Partition Key
    /// </summary>
    public string PartitionKey { get; set; }
    /// <summary>
    /// Row key
    /// </summary>
    public string RowKey { get; set; }
    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }
    /// <summary>
    /// ETag
    /// </summary>
    public ETag ETag { get; set; }
}