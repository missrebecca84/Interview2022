using Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Data
{
    public class MicrogrooveContext : DbContext
    {
        public MicrogrooveContext(DbContextOptions<MicrogrooveContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }

    }
}
