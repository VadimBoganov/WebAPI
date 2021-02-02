using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Data { get; }
        void Add(T obj);
        IEnumerable<T> GetEmployeeByName(string name);
    }
}
