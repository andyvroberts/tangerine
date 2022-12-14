using System;

namespace Bmrs.Common
{
    public static class Constants
    {
        public const string ConfigPK = "BMRS";
        public const string ConfigRK = "SYSTEMPRICE";
        public const string PricePK = "SYSTEMPRICE";
        public const string SystemPrice = "https://api.bmreports.com/BMRS/B1770/v1?APIKey=<KEY>&SettlementDate=<SD>&Period=<SP>&ServiceType=xml";
        public const string SystemVolume = "https://api.bmreports.com/BMRS/B1780/v1?APIKey=<KEY>&SettlementDate=<SD>&Period=<SP>&ServiceType=xml";
    }
}