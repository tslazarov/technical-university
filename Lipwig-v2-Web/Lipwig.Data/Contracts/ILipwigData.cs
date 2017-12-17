using Lipwig.Models;

namespace Lipwig.Data.Contracts
{
    public interface ILipwigData
    {
        IEfRepository<User> UsersRepository { get; }

        IEfRepository<Income> IncomesRepository { get; }

        IEfRepository<Expense> ExpensesRepository { get; }

        IEfRepository<Currency> CurrenciesRepository { get; }

        void SaveChanges();
    }
}
