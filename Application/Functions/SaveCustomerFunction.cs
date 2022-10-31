using Core.Domain.Services;
using Core.Domain.Validation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DomainModels = Core.Domain.Models;

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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers")]
        HttpRequestData req)
        {
            _logger.LogInformation("Entering {Method}", nameof(SaveCustomerFunction));
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            try
            {
                Guid returnValue = Guid.Empty;

                var serialized = req.ReadAsString();
                var requestWrapper = ValidationWrapper<DomainModels.Customer>.BuildValidationWrapper(serialized);
                if (requestWrapper.IsValid)
                {
                    returnValue = await _customerService.SaveCustomerAsync(requestWrapper.Value).ConfigureAwait(false);
                    response.WriteString(returnValue.ToString());
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    var errorList = requestWrapper.ValidationResults.Select(a => a.ErrorMessage).ToList();
                    response.WriteString(string.Join(", ", errorList));
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during {Method}", nameof(SaveCustomerFunction));
                response.StatusCode = HttpStatusCode.BadRequest;
                response.WriteString(ex.Message);
            }
            _logger.LogInformation("Exiting {Method}", nameof(SaveCustomerFunction));
            return response;
        }
    }
}
