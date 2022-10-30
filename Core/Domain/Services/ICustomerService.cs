using Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Services;

public interface ICustomerService
{
    /// <summary>
    /// Retrieve the customer identified by the given Guid
    /// </summary>
    /// <param name="id">Guid of Customer</param>
    /// <returns>Customer</returns>
    /// <exception cref="CustomerNotFoundException">Custom Exception Returned if entity is not found in the database</exception>
    Task<Customer> GetCustomerByIdAsync(Guid id);

    /// <summary>
    /// Get a list of customers who have the age requested
    /// </summary>
    /// <param name="age">Age</param>
    /// <returns>List of Customers</returns>
    Task<IEnumerable<Customer>> GetCustomersByAgeAsync(int age);

    /// <summary>
    /// Save the customer
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <returns>Guid Identifer of the Customer Saved</returns>
    Task<Guid> SaveCustomerAsync(Customer customer);
}
