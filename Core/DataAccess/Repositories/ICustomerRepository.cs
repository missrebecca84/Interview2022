using Core.DataAccess.Entities;
using Core.DataAccess.Repositories.Base;

namespace Core.DataAccess.Repositories
{
    public interface ICustomerRepository : IAsyncRepository<Customer>
    {
        /// <summary>
        /// Saves customer and returns Guid id of the new entity
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<Guid> SaveCustomer(Customer customer);

        /// <summary>
        /// Get a list of customers of a certain age
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAgeException"></exception>
        Task<IEnumerable<Customer>> GetCustomersByAgeAsync(int age);
    }
}
