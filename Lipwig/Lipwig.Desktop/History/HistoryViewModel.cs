using Lipwig.Models.Contracts;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lipwig.Desktop.History
{
    public class HistoryViewModel : BindableBase
    {
        private ObservableCollection<ITransaction> transactions;

        private IUsersService usersService;
        private IExpensesService expensesService;
        private IIncomesService incomesService;

        public HistoryViewModel(IUsersService usersService,
            IExpensesService expensesService,
            IIncomesService incomesService)
        {
            this.expensesService = expensesService;
            this.incomesService = incomesService;
            this.usersService = usersService;

            this.PopulateTransactions();

            this.DeleteTransactionCommand = new RelayCommand<object>(DeleteTransaction);
            this.EditTransactionCommand = new RelayCommand<object>(EditTransaction);
        }

        public ObservableCollection<ITransaction> Transactions
        {
            get
            {
                return this.transactions;
            }
            set
            {
                this.transactions = value;
            }
        }

        public RelayCommand<object> DeleteTransactionCommand { get; private set; }

        public RelayCommand<object> EditTransactionCommand { get; private set; }

        public event Action SuccessfulDeleteRequested = delegate { };

        public event Action<Guid> SuccessfulIncomeEditRequested = delegate { };

        public event Action<Guid> SuccessfulExpenseEditRequested = delegate { };

        private void PopulateTransactions()
        {
            var user = this.usersService.GetUserByEmail(ViewBag.Email);

            if(user != null)
            {
                var transactions = new List<ITransaction>(user.Expenses)
                    .Concat(new List<ITransaction>(user.Incomes))
                    .OrderByDescending(t => t.Date);

                this.Transactions = new ObservableCollection<ITransaction>(transactions);
            }
        }

        private void EditTransaction(object parameter)
        {
            var transaction = (ITransaction)parameter;

            if (transaction.IsExpense)
            {
                this.SuccessfulExpenseEditRequested(transaction.Id);
            }
            else
            {
                this.SuccessfulIncomeEditRequested(transaction.Id);
            }
        }

        private void DeleteTransaction(object parameter)
        {
            var transaction = (ITransaction)parameter;

            var user = this.usersService.GetUserByEmail(ViewBag.Email);

            if (user != null)
            {
                if (transaction.IsExpense)
                {
                    var expense = this.expensesService.GetExpense(transaction.Id);

                    user.Expenses.Remove(expense);
                    user.Balance += expense.Amount;

                    ViewBag.Balance = user.LocalizedBalance;

                    this.usersService.UpdateUser(user);
                    this.expensesService.DeleteExpense(expense);
                }
                else
                {
                    var income = this.incomesService.GetIncome(transaction.Id);

                    user.Incomes.Remove(income);
                    user.Balance -= income.Amount;

                    ViewBag.Balance = user.LocalizedBalance;

                    this.usersService.UpdateUser(user);
                    this.incomesService.DeleteIncome(income);
                }

                this.SuccessfulDeleteRequested();
                this.Transactions.Remove(transaction);
            }
        }
    }
}
