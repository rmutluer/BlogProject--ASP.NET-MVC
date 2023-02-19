using BUSINESS.Abstract;
using ENTITIES.Entities;
using REPOSITORIES.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BUSINESS.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class, IBaseEntity
    {
        private readonly IGenericRepository<T> _repository;

        public GenericManager(IGenericRepository<T> repository)
        {
            _repository = repository;
        }
        public bool Activate(int id)
        {
            if (id <= 0 || GetById(id) == null)
            {
                return false;
            }
            return _repository.Activate(id);
        }

        public bool Add(T item)
        {
            try
            {
                if (item == null)
                {
                    return false;
                }
                return _repository.Add(item);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetByDefault(exp);
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetDefault(exp);
        }

        public bool SetPassive(int id)
        {
            if (id <= 0 || GetById(id) == null)
            {
                return false;
            }
            return _repository.SetPassive(id);
        }

        public bool Remove(T item)
        {
           
            return _repository.Remove(item);
        }

        public bool SetPassive(Expression<Func<T, bool>> exp)
        {
            return _repository.SetPassive(exp);
        }

        public bool Update(T item)
        {
            if (item == null)
            {
                return false;
            }
            return _repository.Update(item);
        }

        public bool Update(List<T> items)
        {
            if (items == null)
            {
                return false;
            }
            return _repository.Update(items);
        }
        public List<T> GetActive(string[] includes)
        {
            return _repository.GetActive(includes);
        }
        public List<T> GetAll(string[] includes)
        {
            return _repository.GetAll(includes);
        }

    }
}
