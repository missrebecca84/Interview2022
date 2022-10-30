using Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Services;

public interface ICustomerService
{
    Task<Customer> GetCustomerByIdAsync(Guid id);
}
