using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microgroove.Infrastructure.Data
{
    public class MicrogrooveContext : DbContext
    {
        public MicrogrooveContext(DbContextOptions<MicrogrooveContext> options) : base(options) { }
        public DbSet<Microgroove.Core.Entities.Customer> Customers { get; set; }
        
    }
}
