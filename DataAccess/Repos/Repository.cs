using Business;
using Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repos
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public T GetById(int id) => _context.Set<T>().Find(id);
        public List<T> GetAll() => _context.Set<T>().ToList();

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
