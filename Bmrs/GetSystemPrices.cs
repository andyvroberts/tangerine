using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace Bmrs
{
    using Bmrs.Models;
    using Bmrs.Common;
    using Bmrs.Services;

    public class GetSystemPrices
    {
        [FunctionName("GetSystemPrices")]
        /// <summary>
        /// Lookup the BMRS API retrieval date from configuration, add 1 day, process the request/responses and save the 
        ///   new date back to configuration.
        /// The table binding allows both read and write to the referenced entity (table row).
        /// The formatted output record will be written to table storage as a single entity.
        /// </summary>
        public static async Task GetSystemPriceWithVolume([TimerTrigger("0 2 * * *", RunOnStartup = true)] TimerInfo myTimer,
        [Table("AcquisitionConfig", Constants.ConfigPK, Constants.ConfigRK, Connection = "EnergyDataStorage")] ConfigTable cd,
        [Table("SystemPrice", Connection = "EnergyDataStorage")] TableClient pt,
        ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var nextDate = cd.Latest.AddDays(1);
            PriceTable payload = new();

            if (nextDate.Date < DateTime.Now.Date)
            {
                string urlDate = $"{nextDate.Year}-{nextDate.Month:00}-{nextDate.Day:00}";
                string apiKey = Environment.GetEnvironmentVariable("BmrsApiKey");
                log.LogInformation($"Settlement Date = {urlDate}");

                var prices = await ImbalanceRequests.BmrsPriceRequestAsync(apiKey, urlDate);
                var volumes = await ImbalanceRequests.BmrsVolumeRequestAsync(apiKey, urlDate);

                payload = Mapping.SeriesToEntity(prices, volumes, urlDate);
            }
            // store the data then update the configuration date.
            await pt.UpsertEntityAsync(payload);
            cd.Latest = nextDate;
            log.LogInformation($"Updated Binding Date for completion of = {nextDate}");
        }
    }
}
