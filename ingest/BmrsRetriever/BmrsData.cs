using System.Net;
using System.Xml.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace BmrsRetriever
{
    public class BmrsData
    {
        [Function("SystemPrices")]
        public HttpResponseData SystemPrices(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "systemprice/{year}/{month}")] HttpRequestData req,
            int year, 
            int month,
            FunctionContext ctx)
        {
            var _logger = ctx.GetLogger(nameof(SystemPrices));
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

    }
}
