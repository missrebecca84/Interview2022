using Azure;
using Core.Domain.Exceptions;
using Core.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/{id:guid}")]
            HttpRequestData req,
            Guid id)
        {
            _logger.LogInformation("Entering {Method}", nameof(GetByIdFunction));

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            try
            {
                var returnValue = await _customerService.GetCustomerByIdAsync(id).ConfigureAwait(false);

                var returnString = JsonConvert.SerializeObject(returnValue);
                response.WriteString(returnString);

            }
            catch (CustomerNotFoundException ex)
            {
                _logger.LogError(ex, "Error during {Method}", nameof(GetByIdFunction));
                response.StatusCode = HttpStatusCode.NotFound;
                response.WriteString(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during {Method}", nameof(GetByIdFunction));
                response.StatusCode = HttpStatusCode.BadRequest;
                response.WriteString(ex.Message);
            }

            _logger.LogInformation("Exiting {Method}", nameof(GetByIdFunction));
            return response;
        }
    }
}
