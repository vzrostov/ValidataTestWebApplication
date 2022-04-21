using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ValidataTest.Core.DAL
{
    internal class CommonRepository<T> : IRepository<T> where T : class
    {
        ICustomerDbContext _customerDbContext = null; 
        DbSet<T> _dbSet = null;
        public bool IsForTesting { internal set; get; } = false;

        public CommonRepository()
        {
            this._customerDbContext = new CustomerDbContext();
            _dbSet = _customerDbContext.DBContext.Set<T>();
        }

        public CommonRepository(ICustomerDbContext _context)
        {
            _customerDbContext = _context;
            _dbSet = _context.DBContext.Set<T>();
        }

        public CommonRepository(ICustomerDbContext _context, DbSet<T> dbSet)
        {
            _customerDbContext = _context;
            _dbSet = dbSet;
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

        public Task<T> GetAsync(int id, string includeProperties = "")
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                _dbSet.Include(includeProperty);
            }
            return _dbSet.FindAsync(id);
        }

        public void Create(T obj)
        {
            _dbSet.Add(obj);
        }

        public void Update(T obj)
        {
            _dbSet.Attach(obj);
            if(!IsForTesting)
                _customerDbContext.DBContext.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(T obj)
        {
            if (!IsForTesting)
                if (_customerDbContext.DBContext.Entry(obj).State == EntityState.Detached)
                {
                    _dbSet.Attach(obj);
                }
            _dbSet.Remove(obj);
        }
    }
}