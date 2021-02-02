using System.Collections.Generic;

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
    }
}
