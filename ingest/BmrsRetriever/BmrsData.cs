using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace BmrsRetriever
{
    public class BmrsData
    {
        [Function("SystemPrices")]
        public static async Task SystemPrices(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "bmrs/price/{year}/{month}/{day}")] HttpRequestData req,
            string year,
            string month,
            string day,
            FunctionContext ctx)
        {
            var _logger = ctx.GetLogger(nameof(SystemPrices));
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string urlDate = $"{year}-{month:00}-{day:00}";
            string? apiKey = Environment.GetEnvironmentVariable("BmrsApiKey");
            _logger.LogInformation("Settlement Date = {}", urlDate);

            if (apiKey != null)
            {
                var prices = await ImbalanceRequests.BmrsPriceRequestAsync(apiKey, urlDate);
                
                var returns = JsonSerializer.Serialize(prices.Select(Mapper.FromXmlToImbalancePrice));

                _logger.LogInformation("Found prices\n {}", returns);
            }
            else 
                _logger.LogInformation("No prices found.");

        }

        [Function("SystemVolumes")]
        public static async Task SystemVolumes(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "bmrs/volume/{year}/{month}/{day}")] HttpRequestData req,
            string year,
            string month,
            string day,
            FunctionContext ctx)
        {
            var _logger = ctx.GetLogger(nameof(SystemPrices));
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string urlDate = $"{year}-{month:00}-{day:00}";
            string? apiKey = Environment.GetEnvironmentVariable("BmrsApiKey");
            _logger.LogInformation("Settlement Date = {}", urlDate);

            if (apiKey != null)
            {
                var volumes = await ImbalanceRequests.BmrsVolumeRequestAsync(apiKey, urlDate);
                
                var returns = JsonSerializer.Serialize(volumes.Select(Mapper.FromXmlToImbalanceVolume));

                _logger.LogInformation("Found volumes\n {}", returns);
            }
            else 
                _logger.LogInformation("No volumes found.");

        }

    }
}