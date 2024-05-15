using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace BmrsRetriever
{
    public class BmrsData
    {
        [Function("SystemPrices")]
        public static async Task SystemPrices(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "systemprice/{year}/{month}")] HttpRequestData req,
            string year,
            string month,
            FunctionContext ctx)
        {
            var _logger = ctx.GetLogger(nameof(SystemPrices));
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string urlDate = $"{year}-{month:00}-01";
            string? apiKey = Environment.GetEnvironmentVariable("BmrsApiKey");
            _logger.LogInformation("Settlement Date = {}", urlDate);

            if (apiKey != null)
            {
                var prices = await ImbalanceRequests.BmrsPriceRequestAsync(apiKey, urlDate);
                
                var periodPrices = prices
                    .Where(x => x.SettlementPeriod <= 51)
                    .Select(n => new
                    {
                        Series = n.TimeSeriesId,
                        Price = n.ImbalancePriceAmountGbp
                    })
                    .ToList();

                _logger.LogInformation("Found prices\n {}", periodPrices);
            }
            else 
                _logger.LogInformation("No prices found.");

        }

    }
}