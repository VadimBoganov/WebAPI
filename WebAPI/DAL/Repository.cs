using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EmployeeContext _context;

        public IEnumerable<T> Data { get; }

        public Repository(EmployeeContext context)
        {
            _context = context;
            Data = _context.Set<T>();
        }

        public void Add(T obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetEmployeeByName(string name)
        {
            var employees = _context.Employees.FromSqlRaw("EXECUTE dbo.GetEmployeeByName {0}", name).ToList();
            return employees as IEnumerable<T>;
        }
    }
}
