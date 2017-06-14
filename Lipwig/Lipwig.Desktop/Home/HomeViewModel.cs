using Lipwig.Desktop.Models;
using Lipwig.Models;
using Lipwig.Models.Contracts;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lipwig.Desktop.Home
{
    public class HomeViewModel : BindableBase
    {
        private ObservableCollection<CartesianData> cartesianData;
        private ObservableCollection<ITransaction> transactions;
        private User user;

        private IUsersService usersService;

        public HomeViewModel(IUsersService usersService)
        {
            this.usersService = usersService;
            this.user = this.usersService.GetUserByEmail(ViewBag.Email);

            this.DailyCartesianChart(DateTime.Today.AddDays(-6), DateTime.Today);
            this.PopulateTransactions(DateTime.Today.AddDays(-6), DateTime.Today);
        }

        public ObservableCollection<CartesianData> CartesianData
        {
            get
            {
                return this.cartesianData;
            }
            set
            {
               this.SetProperty(ref this.cartesianData, value);
            }
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

        private void DailyCartesianChart(DateTime startDate, DateTime endDate)
        {
            var expenses = this.user.Expenses.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
                .GroupBy(e => e.Date.Date)
                .Select(g => new
                {
                    Key = g.Key,
                    Value = g.Sum(e => e.LocalizedAmount)
                })
                .ToList();

            var incomes = this.user.Incomes.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
                .GroupBy(e => e.Date.Date)
                .Select(g => new
                {
                    Key = g.Key,
                    Value = g.Sum(e => e.LocalizedAmount)
                })
                .ToList();

            var collection = new ObservableCollection<CartesianData>();

            for (var day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                var expensesMatch = expenses.FirstOrDefault(d => d.Key.Date == day);
                var incomesMatch = incomes.FirstOrDefault(d => d.Key.Date == day);

                decimal value = 0M;
                value -= expensesMatch != null ? expensesMatch.Value : 0M;
                value += incomesMatch != null ? incomesMatch.Value : 0M;

                var format = "{0:dd/MM}";

                collection.Add(new CartesianData()
                {
                    Date = string.Format(format, day),
                    Value = value
                });
            }

            var oldValue = collection[collection.Count - 1].Value;

            collection[collection.Count - 1].Value = this.user.LocalizedBalance;

            for (int i = collection.Count - 1; i > 1; i--)
            {
                var tempValue = collection[i - 1].Value;
                collection[i - 1].Value = collection[i].Value - oldValue;
                oldValue = tempValue; 
            }

            this.CartesianData = collection;
        }

        private void PopulateTransactions(DateTime startDate, DateTime endDate)
        {
            var incomes = this.user.Incomes.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date);
            var expenses = this.user.Expenses.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date);

            var transactions = new List<ITransaction>(expenses)
                .Concat(new List<ITransaction>(incomes))
                .OrderByDescending(t => t.Date);

            this.Transactions = new ObservableCollection<ITransaction>(transactions);
        }
    }
}
