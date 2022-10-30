using Core.DataAccess.Entities;
using Core.DataAccess.Repositories;
using Infrastructure.DataAccess.Data;
using Infrastructure.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataAccess.Repositories;

public class CustomerRepository : MicrogrooveRepository<Customer>, ICustomerRepository
{
    ILogger _logger;
    public CustomerRepository(MicrogrooveContext dataContext, ILogger logger) : base(dataContext)
    {
        _logger = logger;
    }

    public async Task<Guid> SaveCustomer(Customer customer)
    {
        Guid id;
        _logger.LogInformation("Entering CustomerRepository {Method}", nameof(SaveCustomer));
        try
        {
            if (customer == null)
                throw new ArgumentNullException();

            await Add(customer);
            id = customer.CustomerId.GetValueOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CustomerRepository {Method}", nameof(SaveCustomer));
            throw;
        }

        _logger.LogInformation("Exiting CustomerRepository {Method}", nameof(SaveCustomer));
        return id;
    }

    public async Task<IEnumerable<Customer>> GetCustomerByAgeAsync(int age)
    {
        IEnumerable<Customer> returnList;
        _logger.LogInformation("Entering CustomerRepository {Method}", nameof(GetCustomerByAgeAsync));

        try
        {
            if (age == 0)
                throw new ArgumentException();

            var ageInDaysStart = age * 365;
            var ageInDaysEnd = (age + 1) * 365;
            returnList = await GetWhere(a => DateTime.Now.Date.Subtract(a.DateOfBirth.Date).Days >= ageInDaysStart
                                        && DateTime.Now.Date.Subtract(a.DateOfBirth.Date).Days < ageInDaysEnd);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CustomerRepository {Method}", nameof(GetCustomerByAgeAsync));
            throw;
        }
        _logger.LogInformation("Exiting CustomerRepository {Method}", nameof(GetCustomerByAgeAsync));
        return returnList;
    }
}
