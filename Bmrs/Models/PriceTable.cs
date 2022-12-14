using System;
using Azure;
using Azure.Data.Tables;

namespace Bmrs.Models
{
    public class PriceTable : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public DateTime SettlementDate { get; set; }
        public string EnergyData { get; set; }  // complex type (dict of dicts)
    }
}