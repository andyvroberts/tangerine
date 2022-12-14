using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;

namespace Bmrs.Services
{
    using Bmrs.Models;
    using Bmrs.Common;

    public static class Mapping
    {
        /// <summary>
        /// perform the HTTP request from the BMRS API, retrieve one day of imbalance price records.
        /// Note: Historically there used to be 2 prices (buy & sell), although today only buy prices are created
        ///   by the balancing mechanism.  This converged the price for MWh irrespective of surpluss or shortfall.
        /// <summary>
        public static PriceTable SeriesToEntity(
            IEnumerable<ImbalancePriceItem> prices, IEnumerable<ImbalanceVolumeItem> volumes, string settDate)
        {
            PriceTable dailyData = new();
            Dictionary<string, ListDictionary> energyDict = new();
            dailyData.PartitionKey = Constants.PricePK;
            dailyData.RowKey = settDate;

            // create a loop for a possible maximum 51 settlement periods in a day.
            for (int i = 1; i <= 51; i++)
            {
                ListDictionary EnergyValues = new();

                // Find the price records for this settlement period.
                // There may be up to 2 price records (a buy and/or a sell)
                var periodPrices = prices
                    .Where(x => x.SettlementPeriod == i)
                    .Select(n => new
                    {
                        Series = n.TimeSeriesId,
                        Price = n.ImbalancePriceAmountGbp
                    })
                    .ToList();

                // Find the volume for this settlement period
                var periodVolume = volumes
                    .Where(y => y.SettlementPeriod == i)
                    .Select(m => m.ImbalanceQuantityMaw)
                    .FirstOrDefault();

                // Add the settlement period energy values to the List Dict.
                foreach (var p1 in periodPrices)
                {
                    // add the prices.
                    if (p1.Series.EndsWith('1'))
                    {
                        EnergyValues.Add("BUY", p1.Price);
                    }
                    else
                    {
                        EnergyValues.Add("SELL", p1.Price);
                    }
                }
                // then add the volume.
                EnergyValues.Add("VOL", periodVolume);

                // Save the Settlement period energy values in the outer Dict.
                if (periodPrices.Count() > 0 | periodVolume > 0)
                {
                    energyDict.Add(i.ToString(), EnergyValues);
                }
            }

            dailyData.EnergyData = JsonSerializer.Serialize(energyDict);
            return dailyData;
        }

    }

}