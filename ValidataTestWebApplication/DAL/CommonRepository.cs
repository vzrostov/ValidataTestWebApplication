﻿using System;
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
        ICustomerDbContext _customerDbContext = null; 
        DbSet<T> _dbSet = null;

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
            //_dbSet = _context.Context.Set<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                //query = query.Where((c) => { return c.CustomerId == 19 });
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                //return query.OrderBy((x => x.));
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

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Create(T obj)
        {
            _dbSet.Add(obj);
        }

        public void Update(T obj)
        {
            _dbSet.Attach(obj);
            _customerDbContext.DBContext.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(T obj)
        {
            if (_customerDbContext.DBContext.Entry(obj).State == EntityState.Detached)
            {
                _dbSet.Attach(obj);
            }
            _dbSet.Remove(obj);
        }
    }
}