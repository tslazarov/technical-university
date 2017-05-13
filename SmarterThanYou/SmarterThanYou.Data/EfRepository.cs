using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SmarterThanYou.Data
{
    public class EfRepository<T> : IEfRepository<T>
           where T : class
    {
        private readonly ISmarterThanYouContext dbContext;
        private readonly IDbSet<T> dbSet;

        public EfRepository(ISmarterThanYouContext dbContext)
        {
            Guard.WhenArgument<ISmarterThanYouContext>(dbContext, "Database context cannot be null.")
                .IsNull()
                .Throw();

            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.dbSet;
        }

        public T GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public void Add(T entity)
        {
            DbEntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Added;
        }

        public void Delete(T entity)
        {
            DbEntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Deleted;
        }

        public void Update(T entity)
        {
            DbEntityEntry entry = this.AttachEntry(entity);
            entry.State = EntityState.Modified;
        }

        private DbEntityEntry AttachEntry(T entity)
        {
            DbEntityEntry entry = this.dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            return entry;
        }
    }
}
