using System.Collections.Generic;

namespace WebAPI.DAL
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Data { get; }
        void Add(T obj);
    }
}
