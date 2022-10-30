using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Application.Functions
{
    public class GetCustomersByAgeFunction
    {
        private readonly ILogger _logger;

        public GetCustomersByAgeFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetCustomersByAgeFunction>();
        }

        [Function("GetCustomersByAgeFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/customers/{age}")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
