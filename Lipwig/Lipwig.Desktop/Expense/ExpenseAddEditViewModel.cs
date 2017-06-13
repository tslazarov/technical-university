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
        private bool isEditMode;
        private bool isSaveMode;
        private SimpleExpense simpleExpense;
        private DateTime date;

        private IUsersService usersService;
        private IExpensesService expensesService;
        private IExpensesFactory expensesFactory;
        private IModelFactory modelFactory;

        public ExpenseAddEditViewModel(IUsersService usersService,
            IExpensesService expensesService,
            IExpensesFactory expensesFactory,
            IModelFactory modelFactory)
        {
            this.usersService = usersService;
            this.expensesService = expensesService;
            this.expensesFactory = expensesFactory;
            this.modelFactory = modelFactory;

            this.SimpleExpense = this.modelFactory.CreateSimpleExpense();
            this.Date = DateTime.Today;
            this.IsEditMode = false;
            this.IsSaveMode = true;

            this.SaveCommand = new RelayCommand(Save);
            this.EditCommand = new RelayCommand(Edit);
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

        public bool IsEditMode
        {
            get
            {
                return this.isEditMode;
            }
            set
            {
                SetProperty(ref isEditMode, value);
            }
        }

        public bool IsSaveMode
        {
            get
            {
                return this.isSaveMode;
            }
            set
            {
                SetProperty(ref isSaveMode, value);
            }
        }

        public SimpleExpense SimpleExpense
        {
            get
            {
                return this.simpleExpense;
            }
            set
            {
                SetProperty(ref this.simpleExpense, value);
            }
        }

        private Lipwig.Models.Expense Expense { get; set; }

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

        public RelayCommand EditCommand { get; private set; }

        public event Action SuccessfulExpenseRequested = delegate { };

        public void PopulateEditView(Guid id)
        {
            this.Expense = this.expensesService.GetExpense(id);

            this.Date = this.Expense.Date;
            this.CategoryType = this.Expense.CategoryType;
            this.PaymentType = this.Expense.PaymentType;
            this.SimpleExpense.Amount = this.Expense.LocalizedAmount.ToString();
            this.SimpleExpense.Side = this.Expense.Side;
            this.SimpleExpense.Description = this.Expense.Description;

            this.IsSaveMode = false;
            this.IsEditMode = true;
        }

        private void Edit()
        {
            try
            {
                var amount = decimal.Parse(this.SimpleExpense.Amount);

                var difference = this.Expense.LocalizedAmount - amount;

                this.Expense.Date = this.Date;
                this.Expense.Amount = amount / Constants.CurrencyValue;
                this.Expense.Side = this.SimpleExpense.Side;
                this.Expense.Description = this.SimpleExpense.Description;
                this.Expense.CategoryType = this.CategoryType;
                this.Expense.PaymentType = this.PaymentType;

                var user = this.usersService.GetUserByEmail(Constants.Email);

                if(user != null)
                {
                    user.Balance += difference / Constants.CurrencyValue;
                    Constants.Balance = user.LocalizedBalance;

                    this.usersService.UpdateUser(user);
                }
                
                this.expensesService.UpdateExpense(this.Expense);

                this.Message = "Expense update was successful";
                this.MessageColor = "#2CB144";

                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });

                this.SuccessfulExpenseRequested();
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

        private void Save()
        {
            try
            {
                var amount = decimal.Parse(this.SimpleExpense.Amount);
                var expense = this.expensesFactory.Create(Guid.NewGuid(), this.Date, amount, this.SimpleExpense.Side, this.SimpleExpense.Description, this.PaymentType, this.CategoryType);

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

                this.SimpleExpense = this.modelFactory.CreateSimpleExpense();
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
