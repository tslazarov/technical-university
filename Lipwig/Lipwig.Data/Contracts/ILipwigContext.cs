using Lipwig.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Lipwig.Data.Contracts
{
    public interface ILipwigContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<Income> Incomes { get; set; }

        IDbSet<Expense> Expenses { get; set; }

        IDbSet<Currency> Currencies { get; set; }

        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();
    }
}
