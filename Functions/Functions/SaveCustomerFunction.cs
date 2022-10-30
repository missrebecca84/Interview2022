using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Microgroove.Functions.Functions
{
    public class SaveCustomerFunction
    {
        private readonly ILogger _logger;

        public SaveCustomerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveCustomerFunction>();
        }

        [Function("SaveCustomerFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/customers")] 
        HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
