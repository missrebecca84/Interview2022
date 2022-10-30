using System.Collections.Generic;
using System.Net;
using Core.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Application.Functions
{
    public class GetByIdFunction
    {
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public GetByIdFunction(ILogger logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [Function("GetByIdFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/customers/{id}")]
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
