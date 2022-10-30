﻿using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MicrogrooveRepository<T> : AsyncRepository<T> where T : class
{
    protected new MicrogrooveContext Context;

    /// <summary>
    /// Establish a respository to an entity within the Microgroove DataContext
    /// </summary>
    /// <param name="dataContext">Microgroove Data Context</param>
    public MicrogrooveRepository(MicrogrooveContext dataContext) : base(dataContext)
    {
        Context = dataContext;
    }
}

