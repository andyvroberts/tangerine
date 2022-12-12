using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Bmrs
{
    using Bmrs.Models;
    using Bmrs.Common;

    public class GetSystemPrices
    {
        [FunctionName("GetSystemPrices")]
        [return: Queue("imbalance", Connection = "EnergyDataStorage")]
        /// <summary>
        /// Lookup the BMRS API retrieval date from configuration, add 1 day, process the request/responses and save the 
        /// new date back to configuration.
        /// The table binding allows both read and write to the referenced entity (table row).
        /// The queue binding allows pushing to the queue when issuing the function return statement.
        /// </summary>
        public String GetSystemPriceWithVolume([TimerTrigger("0 2 * * *", RunOnStartup = true)] TimerInfo myTimer,
        [Table("AcquisitionConfig", Constants.ConfigPK, Constants.ConfigRK, Connection = "EnergyDataStorage")] ConfigTable configDate,
        ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var nextDate = configDate.Latest.AddDays(1);

            if (nextDate < DateTime.Now)
            {
                string urlDate = $"{nextDate.Year}-{nextDate.Month:00}-{nextDate.Day:00}";
                string apiKey = Environment.GetEnvironmentVariable("BmrsApiKey");
            }

            configDate.Latest = nextDate;

            log.LogInformation($"Updated Binding Date = {nextDate}");

            return nextDate.ToString();
        }

    }
}
