using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HourlyEmployee>();
            builder.Entity<FixedEmployee>();

            base.OnModelCreating(builder);
        }
    }
}
