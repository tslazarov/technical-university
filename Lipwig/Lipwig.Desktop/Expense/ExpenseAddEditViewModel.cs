using Lipwig.Data.Factories;
using Lipwig.Desktop.Factories;
using Lipwig.Desktop.Models;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lipwig.Desktop.Expense
{
    public class ExpenseAddEditViewModel : BindableBase
    {
        private string message;
        private string messageColor;
        private SimpleExpense expense;
        private DateTime date;

        private IUsersService usersService;
        private IExpensesFactory expensesFactory;
        private IModelFactory modelFactory;

        public ExpenseAddEditViewModel(IUsersService usersService,
            IExpensesFactory expensesFactory,
            IModelFactory modelFactory)
        {
            this.usersService = usersService;
            this.expensesFactory = expensesFactory;
            this.modelFactory = modelFactory;

            this.SaveCommand = new RelayCommand(Save);
            this.Expense = this.modelFactory.CreateSimpleExpense();
            this.Date = DateTime.Today;
        }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                SetProperty(ref this.message, value);
            }
        }

        public string MessageColor
        {
            get
            {
                return this.messageColor;
            }
            set
            {
                SetProperty(ref this.messageColor, value);
            }
        }

        public SimpleExpense Expense
        {
            get
            {
                return this.expense;
            }
            set
            {
                SetProperty(ref this.expense, value);
            }
        }

        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                SetProperty(ref this.date, value);
            }
        }

        public PaymentType PaymentType { get; set; }

        public CategoryType CategoryType { get; set; }

        public RelayCommand SaveCommand { get; private set; }

        public event Action SuccessfulExpenseRequested = delegate { };

        private void Save()
        {
            try
            {
                var amount = decimal.Parse(this.Expense.Amount);
                var expense = this.expensesFactory.Create(Guid.NewGuid(), this.Date, amount, this.Expense.Side, this.Expense.Description, this.PaymentType, this.CategoryType);

                this.usersService.SaveExpense(Constants.Email, expense);
                var user = this.usersService.GetUserByEmail(Constants.Email);

                Constants.Balance = user.LocalizedBalance;

                this.Message = "Expense save was successful";
                this.MessageColor = "#2CB144";

                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });

                this.SuccessfulExpenseRequested();

                this.Expense = this.modelFactory.CreateSimpleExpense();
                this.Date = DateTime.Today;
            }
            catch
            {
                this.Message = "Expense save was unsuccessful";
                this.MessageColor = "#FFD50000";

                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }
    }
}
