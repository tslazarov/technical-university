using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyCommute.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommute.Data
{
    public class EfRepository<T> : IEfRepository<T> where T : class
    {
        private readonly DbContext context;
        private readonly DbSet<T> set;

        public EfRepository(DbContext context)
        {
            this.context = context;
            this.set = this.context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set;
        }

        public T GetById(object id)
        {
            return this.set.Find(id);
        }

        public void Create(T entity)
        {
            EntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Added;
        }

        public void Delete(T entity)
        {
            EntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Deleted;
        }

        public void Update(T entity)
        {
            EntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Modified;
        }

        private EntityEntry AttachEntry(T entity)
        {
            EntityEntry entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            return entry;
        }
    }
}