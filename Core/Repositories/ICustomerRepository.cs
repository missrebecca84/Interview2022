using Microgroove.Core.Entities;

namespace Microgroove.Core.Repositories
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
