using System.Collections.Generic;
using System.Net;
using Core.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Application.Functions
{
    public class SaveCustomerFunction
    {
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public SaveCustomerFunction(ILogger logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
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
