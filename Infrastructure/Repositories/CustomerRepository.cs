using Microgroove.Core.Entities;
using Microgroove.Core.Repositories;
using Microgroove.Infrastructure.Data;

namespace Microgroove.Infrastructure.Repositories;

public class CustomerRepository : MicrogrooveRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(MicrogrooveContext dataContext) : base(dataContext)
    {
    }

    public async Task<Guid> SaveCustomer(Customer customer)
    {
        await Add(customer);
        return customer.CustomerId;
    }

    public async Task<IEnumerable<Customer>> GetCustomerByAgeAsync(int age)
    {
        var ageInDaysStart = age * 365;
        var ageInDaysEnd = (age + 1) * 365;
        return await GetWhere(a => DateTime.Now.Date.Subtract(a.DateOfBirth.Date).Days >= ageInDaysStart && DateTime.Now.Date.Subtract(a.DateOfBirth.Date).Days < ageInDaysEnd);
    }
}
