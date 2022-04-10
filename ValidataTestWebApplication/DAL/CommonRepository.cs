using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using ValidataTestWebApplication.Models;

namespace ValidataTestWebApplication.DAL
{
    public class CommonRepository<T> : IRepository<T> where T : class
    {
        CustomerDbContext _context = null; 
        DbSet<T> _dbSet = null;

        public CommonRepository()
        {
            this._context = new CustomerDbContext();
            _dbSet = _context.Set<T>();
        }

        public CommonRepository(CustomerDbContext _context)
        {
            this._context = _context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public Task<T> GetAsync(int id)
        {
            return _dbSet.FindAsync(id);
        }

        public void Create(T obj)
        {
            _dbSet.Add(obj);
        }

        public void Update(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(T obj)
        {
            if (_context.Entry(obj).State == EntityState.Detached)
            {
                _dbSet.Attach(obj);
            }
            _dbSet.Remove(obj);
        }
    }
}