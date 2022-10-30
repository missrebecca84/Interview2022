using Core.DataAccess.Entities;
using Core.DataAccess.Repositories.Base;

namespace Core.DataAccess.Repositories
{
    public interface ICustomerRepository : IAsyncRepository<Customer>
    {
        /// <summary>
        /// Get a list of customers of a certain age
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        Task<IEnumerable<Customer>> GetCustomerByAgeAsync(int age);
    }
}
