using Lipwig.Data.Contracts;
using Lipwig.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Lipwig.Data
{
    public class LipwigContext : DbContext, ILipwigContext
    {
        public LipwigContext() : base("Lipwig")
        {
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Income> Incomes { get; set; }

        public virtual IDbSet<Expense> Expenses { get; set; }

        public virtual IDbSet<Currency> Currencies { get; set; }

        public static DbContext Create()
        {
            return new LipwigContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        IDbSet<T> ILipwigContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}
