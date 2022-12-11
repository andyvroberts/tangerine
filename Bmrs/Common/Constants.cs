using System;

namespace Bmrs.Common
{
    public static class Constants
    {
        public const string ConfigPartitionKey = "BMRS";
        public const string ConfigRowKeyPrice = "B1770";
        public const string ConfigRowKeyVolume = "B1780";
        public const string SystemPrice = "https://api.bmreports.com/BMRS/B1770/v1?APIKey=<KEY>&SettlementDate=<SD>&Period=<SP>&ServiceType=xml";
        public const string SystemVolume = "https://api.bmreports.com/BMRS/B1780/v1?APIKey=<KEY>&SettlementDate=<SD>&Period=<SP>&ServiceType=xml";
    }
}