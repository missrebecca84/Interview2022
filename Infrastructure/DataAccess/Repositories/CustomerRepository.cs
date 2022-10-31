using Core.DataAccess.Entities;
using Core.DataAccess.Repositories;
using Core.Domain.Exceptions;
using Infrastructure.DataAccess.Data;
using Infrastructure.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataAccess.Repositories;

public class CustomerRepository : BusinessRepository<Customer>, ICustomerRepository
{
    ILogger _logger;
    public CustomerRepository(BusinessContext dataContext, ILogger logger) : base(dataContext)
    {
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Guid> SaveCustomer(Customer customer)
    {
        Guid id;
        _logger.LogInformation("Entering CustomerRepository {Method}", nameof(SaveCustomer));
        try
        {
            if (customer == null)
                throw new ArgumentNullException();

            await Add(customer);
            id = customer.Id.GetValueOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CustomerRepository {Method}", nameof(SaveCustomer));
            throw;
        }

        _logger.LogInformation("Exiting CustomerRepository {Method}", nameof(SaveCustomer));
        return id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="age"></param>
    /// <returns></returns>
    /// <exception cref="InvalidAgeException"></exception>
    public async Task<IEnumerable<Customer>> GetCustomersByAgeAsync(int age)
    {
        IEnumerable<Customer> returnList = new List<Customer>();
        _logger.LogInformation("Entering CustomerRepository {Method}", nameof(GetCustomersByAgeAsync));

        try
        {
            if (age == 0)
                throw new InvalidAgeException();

            var ageInDaysStart = (age + 1) * -365;
            var ageInDaysEnd =  age * -365;
            var startDate = DateTime.Now.Date.AddDays(ageInDaysStart);
            var endDate = DateTime.Now.Date.AddDays(ageInDaysEnd);
            returnList = await GetWhere(a => a.DateOfBirth >= startDate
                                        && a.DateOfBirth < endDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CustomerRepository {Method}", nameof(GetCustomersByAgeAsync));
            throw;
        }
        _logger.LogInformation("Exiting CustomerRepository {Method}", nameof(GetCustomersByAgeAsync));
        return returnList;
    }
}
