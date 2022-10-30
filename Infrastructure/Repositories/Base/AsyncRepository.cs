using Microgroove.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Microgroove.Infrastructure.Repositories;
public abstract class AsyncRepository<T> : IAsyncRepository<T> where T : class
{
    protected DbContext Context;

    /// <summary>
    /// Establish a repository to an entity with a generic data context
    /// </summary>
    /// <param name="dataContext"></param>
    public AsyncRepository(DbContext dataContext)
    {
        Context = dataContext;
    }

    public Task<T> GetById(Guid id) => Context.Set<T>().FindAsync(id).AsTask();

    public async Task Add(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).ToListAsync();
    }
}
