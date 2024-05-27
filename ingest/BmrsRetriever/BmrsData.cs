using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace BmrsRetriever
{
    public class BmrsData
    {
        [Function("SystemPrices")]
        public static async Task<IActionResult> SystemPrices(
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

            if (apiKey == null)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            var prices = await ImbalanceRequests.BmrsPriceRequestAsync(apiKey, urlDate);    
            var returns = JsonSerializer.Serialize(prices.Select(Mapper.FromXmlToImbalancePrice));

            return returns.Length > 0 ? 
                new OkObjectResult(returns) :
                new NotFoundObjectResult(urlDate);
        }

        [Function("SystemVolumes")]
        public static async Task<IActionResult> SystemVolumes(
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

            if (apiKey == null)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);


            var volumes = await ImbalanceRequests.BmrsVolumeRequestAsync(apiKey, urlDate);    
            var returns = JsonSerializer.Serialize(volumes.Select(Mapper.FromXmlToImbalanceVolume));

            return returns.Length > 0 ? 
                new OkObjectResult(returns) :
                new NotFoundObjectResult(urlDate);
        }

    }
}