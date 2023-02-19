using ENTITIES.Entities;
using ENTITIES.Enums;
using Microsoft.EntityFrameworkCore;
using REPOSITORIES.Abstract;
using REPOSITORIES.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace REPOSITORIES.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        private readonly BlogDbContext _db;

        public GenericRepository(BlogDbContext db)
        {
            _db = db;
        }


        public bool Activate(int id)
        {
            T item = GetById(id);
            item.Status = Status.Active;
            return Save();
        }


        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().FirstOrDefault(exp);
        }

        public T GetById(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _db.Set<T>().Where(exp).ToList();
        }
        public List<T> GetAll(string[] includes)
        {
            var query = _db.Set<T>().AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return query.ToList();
        }
        public List<T> GetActive(string[] includes)
        {
            var query = _db.Set<T>().AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return query.Where(x => x.Status == Status.Active).ToList();
        }
        public bool Add(T item)
        {
            _db.Set<T>().Add(item);
            return Save();
        }
       

        public bool SetPassive(int id)
        {
            T item = GetById(id);
            item.Status = Status.Passive;
            return Save();
        }
        public bool Remove(T item)
        {
            _db.Set<T>().Remove(item);
            return Save();
        }

        public bool SetPassive(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var items = GetDefault(exp);
                    int count = 0;

                    foreach (var item in items)
                    {
                        item.Status = Status.Passive;
                        bool result = Update(item);
                        if (result) count++;
                    }

                    if (items.Count == count) ts.Complete();
                    else return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }
        


        public bool Update(T item)
        {
            try
            {
                _db.Set<T>().Update(item);
                return Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update(List<T> items)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    int count = 0;

                    foreach (var item in items)
                    {
                        bool result = Update(item);
                        if (result) count++;
                    }

                    if (items.Count == count) ts.Complete();
                    else return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //public IQueryable<T> GetAll()
        //{
        //    return _db.AsNoTracking().AsQueryable();
        //}












    }
}
