using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;

namespace Bmrs
{
    using Bmrs.Models;
    using Bmrs.Common;

    public class GetSystemPrices
    {
        [FunctionName("GetSystemPrices")]
        [return: Queue("imbalances", Connection = "energyDataStorage")]

        public void GetSystemPricesWithVolumes([TimerTrigger("0 2 * * *", RunOnStartup = true)] TimerInfo myTimer,
        [Table("AcquisitionConfig", Connection = "energyDataStorage")] TableClient confTable,
        ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = TableUtils.GetNextDate(confTable);

            // continue only if configuration exists.
            if (config != null)
            {
                // TODO
            }

            // save amended config.
            confTable.UpdateEntity<ConfigTable>(config, config.ETag, TableUpdateMode.Replace);
        }

    }
}
