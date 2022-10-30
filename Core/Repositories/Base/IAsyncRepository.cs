using System.Linq.Expressions;

namespace Microgroove.Core.Repositories;
public interface IAsyncRepository<T> where T : class
{
    /// <summary>
    /// Get an entity by the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetById(Guid id);
    /// <summary>
    /// Save an entity to the data context
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Add(T entity);

    /// <summary>
    /// Get a list of entities based on a condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
}
