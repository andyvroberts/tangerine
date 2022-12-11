using System;
using Azure.Data.Tables;
using Bmrs.Models;

namespace Bmrs.Common
{
    public static class TableUtils
    {
        public static ConfigTable GetNextDate(TableClient confTable)
        {
            ConfigTable configDate = new();

            configDate = confTable.GetEntity<ConfigTable>(Constants.ConfigPartitionKey, Constants.ConfigRowKeyPrice);

            var nextDate = configDate.Latest.AddDays(1);

            if (nextDate < DateOnly.FromDateTime(DateTime.Now))
            {
                configDate.Latest = nextDate;
            }

            configDate.Latest = new DateOnly(2014, 12, 31);


            return configDate;
        }
    }

}