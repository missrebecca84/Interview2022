using AutoMapper;
using Core.DataAccess.Repositories;
using Core.Domain.Exceptions;
using Core.Domain.Services;
using Microsoft.Extensions.Logging;
using DomainModels = Core.Domain.Models;
using Entities = Core.DataAccess.Entities;

namespace Infrastructure.Domain.Services;

/// <summary>
/// Domain services related to customers
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ILogger logger, IMapper mapper, ICustomerRepository repository)
    {
        _logger = logger;
        _mapper = mapper;
        _customerRepository = repository;

    }

    /// <inheritdoc/>
    public async Task<DomainModels.Customer> GetCustomerByIdAsync(Guid id)
    {
        _logger.LogInformation("Entering CustomerService {Method}", nameof(GetCustomerByIdAsync));
        var returnValue = new DomainModels.Customer();
        try
        {
            var entityFound = await _customerRepository.GetById(id).ConfigureAwait(false);
            if (entityFound == null)
                throw new CustomerNotFoundException();
            returnValue = _mapper.Map(entityFound, returnValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CustomerService {Method}", nameof(GetCustomerByIdAsync));
            throw;
        }
        _logger.LogInformation("Exiting CustomerService {Method}", nameof(GetCustomerByIdAsync));
        return returnValue;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DomainModels.Customer>> GetCustomersByAgeAsync(int age)
    {
        _logger.LogInformation("Entering CustomerService {Method}", nameof(GetCustomersByAgeAsync));
        var returnValue = new List<DomainModels.Customer>();
        try
        {
            if(age == 0)
                throw new InvalidAgeException();
            var entitiesFound = await _customerRepository.GetCustomersByAgeAsync(age).ConfigureAwait(false);
            _mapper.Map(entitiesFound, returnValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CustomerService {Method}", nameof(GetCustomersByAgeAsync));
            throw;
        }
        _logger.LogInformation("Exiting CustomerService {Method}", nameof(GetCustomersByAgeAsync));
        return returnValue;
    }

    /// <inheritdoc/>
    public async Task<Guid> SaveCustomerAsync(DomainModels.Customer customer)
    {
        _logger.LogInformation("Entering CustomerService {Method}", nameof(SaveCustomerAsync));
        Guid returnValue = Guid.Empty;
        try
        {
            if(customer == null)
                throw new ArgumentNullException(nameof(customer));
            var entityCustomer = _mapper.Map<Entities.Customer>(customer);
            returnValue = await _customerRepository.SaveCustomer(entityCustomer).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CustomerService {Method}", nameof(SaveCustomerAsync));
            throw;
        }
        _logger.LogInformation("Exiting CustomerService {Method}", nameof(SaveCustomerAsync));
        return returnValue;
    }
}
