using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        List<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
