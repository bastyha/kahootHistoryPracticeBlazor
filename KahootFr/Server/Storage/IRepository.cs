using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KahootFr.Server.Storage
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Remove(T entity);
        IEnumerable<T> GetAll();
    }
}
