using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
