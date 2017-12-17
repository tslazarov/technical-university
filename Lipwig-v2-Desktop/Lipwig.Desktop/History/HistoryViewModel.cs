using Lipwig.HttpUtilities;
using Lipwig.Models;
using Lipwig.Models.Contracts;
using Lipwig.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lipwig.Desktop.History
{
    public class HistoryViewModel : BindableBase
    {
        private ObservableCollection<ITransaction> transactions;

        public HistoryViewModel()
        {
            this.PopulateView();

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
                this.SetProperty(ref this.transactions, value);
            }
        }

        public RelayCommand<object> DeleteTransactionCommand { get; private set; }

        public RelayCommand<object> EditTransactionCommand { get; private set; }

        public event Action SuccessfulDeleteRequested = delegate { };

        public event Action<Guid> SuccessfulIncomeEditRequested = delegate { };

        public event Action<Guid> SuccessfulExpenseEditRequested = delegate { };

        private async void PopulateView()
        {
            try
            {
                var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

                if (user != null)
                {
                    this.PopulateTransactions(user);
                }
            }
            catch
            {
            }
        }

        private void PopulateTransactions(User user)
        {
            var transactions = new List<ITransaction>(user.Expenses)
                      .Concat(new List<ITransaction>(user.Incomes))
                      .OrderByDescending(t => t.Date);

            this.Transactions = new ObservableCollection<ITransaction>(transactions);
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

        private async void DeleteTransaction(object parameter)
        {
            var transaction = (ITransaction)parameter;

            try
            {
                var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

                if (user != null)
                {
                    if (transaction.IsExpense)
                    {
                        var expense = JsonConvert.DeserializeObject<Lipwig.Models.Expense>(await HttpRequestHelper.GetExpenseRemote(transaction.Id));

                        user.Expenses.Remove(expense);
                        user.Balance += expense.Amount;

                        ViewBag.Balance = user.LocalizedBalance;


                        var responseUser = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateUserRemote(user));

                        if (responseUser.Status == "0")
                        {
                            throw new Exception();
                        }

                        var responseExpense = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.DeleteExpenseRemote(expense));

                        if (responseExpense.Status == "0")
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        var income = JsonConvert.DeserializeObject<Lipwig.Models.Income>(await HttpRequestHelper.GetIncomeRemote(transaction.Id));

                        user.Incomes.Remove(income);
                        user.Balance -= income.Amount;

                        ViewBag.Balance = user.LocalizedBalance;


                        var responseUser = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateUserRemote(user));

                        if (responseUser.Status == "0")
                        {
                            throw new Exception();
                        }

                        var responseIncome = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.DeleteIncomeRemote(income));

                        if (responseIncome.Status == "0")
                        {
                            throw new Exception();
                        }
                    }

                    this.SuccessfulDeleteRequested();
                    this.Transactions.Remove(transaction);
                }
            }
            catch
            {
            }
        }
    }
}
