using Infrastructure.DataAccess.Data;

namespace Infrastructure.DataAccess.Repositories.Base;

public class BusinessRepository<T> : AsyncRepository<T> where T : class
{
    protected new BusinessContext Context;

    /// <summary>
    /// Establish a respository to an entity within the Business DataContext
    /// </summary>
    /// <param name="dataContext">Business Data Context</param>
    public BusinessRepository(BusinessContext dataContext) : base(dataContext)
    {
        Context = dataContext;
    }
}

