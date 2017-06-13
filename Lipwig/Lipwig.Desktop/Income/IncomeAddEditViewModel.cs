using Lipwig.Data.Factories;
using Lipwig.Desktop.Factories;
using Lipwig.Desktop.Models;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lipwig.Desktop.Income
{
    public class IncomeAddEditViewModel : BindableBase
    {
        private string message;
        private string messageColor;
        private bool isEditMode;
        private bool isSaveMode;
        private SimpleIncome simpleIncome;
        private DateTime date;

        private IUsersService usersService;
        private IIncomesService incomesService;
        private IIncomesFactory incomesFactory;
        private IModelFactory modelFactory;

        public IncomeAddEditViewModel(IUsersService usersService,
            IIncomesService incomesService,
            IIncomesFactory incomesFactory,
            IModelFactory modelFactory)
        {
            this.usersService = usersService;
            this.incomesService = incomesService;
            this.incomesFactory = incomesFactory;
            this.modelFactory = modelFactory;

            this.SimpleIncome = this.modelFactory.CreateSimpleIncome();
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

        public SimpleIncome SimpleIncome
        {
            get
            {
                return this.simpleIncome;
            }
            set
            {
                SetProperty(ref this.simpleIncome, value);
            }
        }

        private Lipwig.Models.Income Income { get; set; }


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

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand EditCommand { get; private set; }

        public event Action SuccessfulIncomeRequested = delegate { };

        public void PopulateEditView(Guid id)
        {
            this.Income = this.incomesService.GetIncome(id);

            this.Date = this.Income.Date;
            this.PaymentType = this.Income.PaymentType;
            this.SimpleIncome.Amount = this.Income.LocalizedAmount.ToString();
            this.SimpleIncome.Side = this.Income.Side;
            this.SimpleIncome.Description = this.Income.Description;

            this.IsSaveMode = false;
            this.IsEditMode = true;
        }

        private void Edit()
        {
            try
            {
                var amount = decimal.Parse(this.SimpleIncome.Amount);

                var difference = this.Income.LocalizedAmount - amount;

                this.Income.Date = this.Date;
                this.Income.Amount = amount / Constants.CurrencyValue;
                this.Income.Side = this.SimpleIncome.Side;
                this.Income.Description = this.SimpleIncome.Description;
                this.Income.PaymentType = this.PaymentType;

                var user = this.usersService.GetUserByEmail(Constants.Email);

                if (user != null)
                {
                    user.Balance -= difference / Constants.CurrencyValue;
                    Constants.Balance = user.LocalizedBalance;

                    this.usersService.UpdateUser(user);
                }

                this.incomesService.UpdateIncome(this.Income);

                this.Message = "Income update was successful";
                this.MessageColor = "#2CB144";

                this.SuccessfulIncomeRequested();
            }
            catch
            {
                this.Message = "Income update was unsuccessful";
                this.MessageColor = "#FFD50000";
            }
            finally
            {
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
                var amount = decimal.Parse(this.SimpleIncome.Amount);
                var income = this.incomesFactory.Create(Guid.NewGuid(), this.Date, amount, this.SimpleIncome.Side, this.SimpleIncome.Description, this.PaymentType);

                this.usersService.SaveIncome(Constants.Email, income);
                var user = this.usersService.GetUserByEmail(Constants.Email);

                Constants.Balance = user.LocalizedBalance;

                this.Message = "Income save was successful";
                this.MessageColor = "#2CB144";

                this.SuccessfulIncomeRequested();

                this.SimpleIncome = this.modelFactory.CreateSimpleIncome();
                this.Date = DateTime.Today;
            }
            catch
            {
                this.Message = "Income save was unsuccessful";
                this.MessageColor = "#FFD50000";
            }
            finally
            {
                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });
            }
        }
    }
}
