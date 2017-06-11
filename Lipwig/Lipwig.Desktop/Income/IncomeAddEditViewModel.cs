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
        private SimpleIncome income;
        private DateTime date;

        private IUsersService usersService;
        private IIncomesFactory incomesFactory;
        private IModelFactory modelFactory;

        public IncomeAddEditViewModel(IUsersService usersService,
            IIncomesFactory incomesFactory,
            IModelFactory modelFactory)
        {
            this.usersService = usersService;
            this.incomesFactory = incomesFactory;
            this.modelFactory = modelFactory;

            this.SaveCommand = new RelayCommand(Save);
            this.Income = this.modelFactory.CreateSimpleIncome();
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

        public SimpleIncome Income
        {
            get
            {
                return this.income;
            }
            set
            {
                SetProperty(ref this.income, value);
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

        public RelayCommand SaveCommand { get; private set; }

        public event Action SuccessfulIncomeRequested = delegate { };

        private void Save()
        {
            try
            {
                var amount = decimal.Parse(this.Income.Amount);
                var income = this.incomesFactory.Create(Guid.NewGuid(), this.Date, amount, this.Income.Side, this.Income.Description, this.PaymentType);

                this.usersService.SaveIncome(Constants.Email, income);
                var user = this.usersService.GetUserByEmail(Constants.Email);

                Constants.Balance = user.LocalizedBalance;

                this.Message = "Income save was successful";
                this.MessageColor = "#2CB144";

                Task.Factory.StartNew(() => Thread.Sleep(2 * 1000))
                    .ContinueWith((t) =>
                    {
                        this.Message = string.Empty;
                    });

                this.SuccessfulIncomeRequested();

                this.Income = this.modelFactory.CreateSimpleIncome();
                this.Date = DateTime.Today;
            }
            catch
            {
                this.Message = "Income save was unsuccessful";
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
