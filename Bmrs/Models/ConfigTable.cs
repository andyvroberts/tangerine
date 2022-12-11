using System;
using Azure;
using Azure.Data.Tables;

namespace Bmrs.Models
{
    public class ConfigTable : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public DateOnly Latest { get; set; }
        public int Completed { get; set; }
    }
}