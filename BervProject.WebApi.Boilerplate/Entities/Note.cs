
using System;
using Azure;
using Azure.Data.Tables;

namespace BervProject.WebApi.Boilerplate.Entities
{
    public class Note : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}

