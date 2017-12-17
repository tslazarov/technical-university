using Lipwig.Data.Factories;
using Lipwig.Desktop.Factories;
using Lipwig.Desktop.Models;
using Lipwig.HttpUtilities;
using Lipwig.Models;
using Lipwig.Utilities;
using Newtonsoft.Json;
using System;
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

        private IExpensesFactory expensesFactory;
        private IModelFactory modelFactory;

        public ExpenseAddEditViewModel(IExpensesFactory expensesFactory,
            IModelFactory modelFactory)
        {

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
               this.SetProperty(ref this.message, value);
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
               this.SetProperty(ref this.messageColor, value);
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
               this.SetProperty(ref isEditMode, value);
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
               this.SetProperty(ref isSaveMode, value);
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
               this.SetProperty(ref this.simpleExpense, value);
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
               this.SetProperty(ref this.date, value);
            }
        }

        public PaymentType PaymentType { get; set; }

        public CategoryType CategoryType { get; set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand EditCommand { get; private set; }

        public event Action SuccessfulExpenseRequested = delegate { };

        public async void PopulateEditView(Guid id)
        {
            try
            {
                this.Expense = JsonConvert.DeserializeObject<Lipwig.Models.Expense>(await HttpRequestHelper.GetExpenseRemote(id));
            }
            catch
            {
            }

            if(this.Expense != null)
            {
                this.Date = this.Expense.Date;
                this.CategoryType = this.Expense.CategoryType;
                this.PaymentType = this.Expense.PaymentType;
                this.SimpleExpense.Amount = this.Expense.LocalizedAmount.ToString();
                this.SimpleExpense.Side = this.Expense.Side;
                this.SimpleExpense.Description = this.Expense.Description;
            }

            this.IsSaveMode = false;
            this.IsEditMode = true;
        }

        private async void Edit()
        {
            try
            {
                var amount = decimal.Parse(this.SimpleExpense.Amount);

                var difference = this.Expense.LocalizedAmount - amount;

                this.Expense.Date = this.Date;
                this.Expense.Amount = amount / ViewBag.CurrencyValue;
                this.Expense.Side = this.SimpleExpense.Side;
                this.Expense.Description = this.SimpleExpense.Description;
                this.Expense.CategoryType = this.CategoryType;
                this.Expense.PaymentType = this.PaymentType;

                var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

                if(user != null)
                {
                    user.Balance += difference / ViewBag.CurrencyValue;
                    ViewBag.Balance = user.LocalizedBalance;

                    var responseUser = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateUserRemote(user));

                    if(responseUser.Status == "0")
                    {
                        throw new Exception();
                    }
                }
                
                var responseExpense = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateExpenseRemote(this.Expense));

                if (responseExpense.Status == "0")
                {
                    throw new Exception();
                }

                this.Message = Constants.SuccessfulExpenseUpdate;
                this.MessageColor = Constants.MessagePositiveColor;

                this.SuccessfulExpenseRequested();
            }
            catch
            {
                this.Message = Constants.UnsuccessfulExpenseUpdate;
                this.MessageColor = Constants.MessageNegativeColor;
            }
            finally
            {
                await Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }

        private async void Save()
        {
            try
            {
                var amount = decimal.Parse(this.SimpleExpense.Amount);
                var expense = this.expensesFactory.Create(Guid.NewGuid(), this.Date, amount, this.SimpleExpense.Side, this.SimpleExpense.Description, this.PaymentType, this.CategoryType);

                var responseExpense = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.CreateExpenseRemote(ViewBag.Email, expense));

                if(responseExpense.Status == "0")
                {
                    throw new Exception();
                }

                var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

                ViewBag.Balance = user.LocalizedBalance;

                this.Message = Constants.SuccessfulfulExpenseCreation;
                this.MessageColor = Constants.MessagePositiveColor;

                this.SuccessfulExpenseRequested();

                this.SimpleExpense = this.modelFactory.CreateSimpleExpense();
                this.Date = DateTime.Today;
            }
            catch
            {
                this.Message = Constants.UnsuccessfulExpenseCreation;
                this.MessageColor = Constants.MessageNegativeColor;
            }
            finally
            {
                await Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }
    }
}
