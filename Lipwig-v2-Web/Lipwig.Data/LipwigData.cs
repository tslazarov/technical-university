using Bytes2you.Validation;
using Lipwig.Data.Contracts;
using Lipwig.Models;

namespace Lipwig.Data
{
    public class LipwigData : ILipwigData
    {
        private readonly ILipwigContext dbContext;
        private readonly IEfRepository<User> usersRepository;
        private readonly IEfRepository<Income> incomesRepository;
        private readonly IEfRepository<Expense> expensesRepository;
        private readonly IEfRepository<Currency> currenciesRepository;

        public LipwigData(ILipwigContext dbContext,
            IEfRepository<User> usersRepository,
            IEfRepository<Income> incomesRepository,
            IEfRepository<Expense> expensesRepository,
            IEfRepository<Currency> currenciesRepository)
        {
            Guard.WhenArgument<ILipwigContext>(dbContext, "Database context cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<User>>(usersRepository, "Users repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Income>>(incomesRepository, "Incomes repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Expense>>(expensesRepository, "Expenses repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Currency>>(currenciesRepository, "Currencies repository cannot be null.")
                .IsNull()
                .Throw();

            this.dbContext = dbContext;
            this.usersRepository = usersRepository;
            this.incomesRepository = incomesRepository;
            this.expensesRepository = expensesRepository;
            this.currenciesRepository = currenciesRepository;
        }

        public IEfRepository<User> UsersRepository
        {
            get
            {
                return this.usersRepository;
            }
        }

        public IEfRepository<Income> IncomesRepository
        {
            get
            {
                return this.incomesRepository;
            }
        }

        public IEfRepository<Expense> ExpensesRepository
        {
            get
            {
                return this.expensesRepository;
            }
        }

        public IEfRepository<Currency> CurrenciesRepository
        {
            get
            {
                return this.currenciesRepository;
            }
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }
    }
}
