using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommute.Data.Contracts
{
    public interface IEfRepository<T> where T : class
    {
        T GetById(object id);

        IQueryable<T> All();

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
