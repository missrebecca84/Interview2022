using Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Data
{
    public class BusinessContext : DbContext
    {
        private static bool _created = false;
        public BusinessContext(DbContextOptions<BusinessContext> options) : base(options)
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }
        public virtual DbSet<Customer> Customers { get; set; }

    }
}
