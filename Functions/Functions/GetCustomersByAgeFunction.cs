using Core.Domain.Exceptions;
using Core.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.Functions
{
    public class GetCustomersByAgeFunction
    {
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public GetCustomersByAgeFunction(ILogger logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [Function("GetCustomersByAgeFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/{age:int}")] HttpRequestData req, int age)
        {
            _logger.LogInformation("Entering {Method}", nameof(GetCustomersByAgeFunction));
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            try
            {
                if (age == 0)
                    throw new InvalidAgeException();

                var returnValue = await _customerService.GetCustomersByAgeAsync(age).ConfigureAwait(false);
                if (returnValue.Any())
                {
                    var returnString = JsonConvert.SerializeObject(returnValue);
                    response.WriteString(returnString);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during {Method}", nameof(GetCustomersByAgeFunction));
                response.StatusCode = HttpStatusCode.BadRequest;
                response.WriteString(ex.Message);
            }
            _logger.LogInformation("Exiting {Method}", nameof(GetCustomersByAgeFunction));
            return response;
        }
    }
}
