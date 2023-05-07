using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Infrastructure.Interfaces
{
    public interface IBaseRepository<T>
    {
        bool Create(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        bool Delete(int id);
    }
}
