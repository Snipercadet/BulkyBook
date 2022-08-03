using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BulkDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(BulkDbContext db)
        {
            _db = db;
            //_db.Products.Include(u => u.Category.Name).Include(u => u.CoverType);
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
           dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter =null, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter!=null)
            {
                query = query.Where(filter);
            }
            if (IncludeProperties != null)
            {
                foreach(var includeProp in IncludeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(includeProp);
                }
            }
            return query.ToList();
        }


        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
                if(IncludeProperties != null)
                {
                    foreach (var includeProp in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query.Include(includeProp);
                    }
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
