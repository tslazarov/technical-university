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

        private IIncomesFactory incomesFactory;
        private IModelFactory modelFactory;

        public IncomeAddEditViewModel(IIncomesFactory incomesFactory,
            IModelFactory modelFactory)
        {
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

        public SimpleIncome SimpleIncome
        {
            get
            {
                return this.simpleIncome;
            }
            set
            {
               this.SetProperty(ref this.simpleIncome, value);
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
               this.SetProperty(ref this.date, value);
            }
        }

        public PaymentType PaymentType { get; set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand EditCommand { get; private set; }

        public event Action SuccessfulIncomeRequested = delegate { };

        public async void PopulateEditView(Guid id)
        {
            try
            {
                this.Income = JsonConvert.DeserializeObject<Lipwig.Models.Income>(await HttpRequestHelper.GetIncomeRemote(id));
            }
            catch
            {
            }

            if(this.Income != null)
            {
                this.Date = this.Income.Date;
                this.PaymentType = this.Income.PaymentType;
                this.SimpleIncome.Amount = this.Income.LocalizedAmount.ToString();
                this.SimpleIncome.Side = this.Income.Side;
                this.SimpleIncome.Description = this.Income.Description;
            }

            this.IsSaveMode = false;
            this.IsEditMode = true;
        }

        private async void Edit()
        {
            try
            {
                var amount = decimal.Parse(this.SimpleIncome.Amount);

                var difference = this.Income.LocalizedAmount - amount;

                this.Income.Date = this.Date;
                this.Income.Amount = amount / ViewBag.CurrencyValue;
                this.Income.Side = this.SimpleIncome.Side;
                this.Income.Description = this.SimpleIncome.Description;
                this.Income.PaymentType = this.PaymentType;

                var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

                if (user != null)
                {
                    user.Balance -= difference / ViewBag.CurrencyValue;
                    ViewBag.Balance = user.LocalizedBalance;

                    var responseUser = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateUserRemote(user));

                    if (responseUser.Status == "0")
                    {
                        throw new Exception();
                    }
                }

                var responseExpense = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.UpdateIncomeRemote(this.Income));

                if (responseExpense.Status == "0")
                {
                    throw new Exception();
                }

                this.Message = Constants.SuccessfulIncomeUpdate;
                this.MessageColor = Constants.MessagePositiveColor;

                this.SuccessfulIncomeRequested();
            }
            catch
            {
                this.Message = Constants.UnsuccessfulIncomeUpdate;
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
                var amount = decimal.Parse(this.SimpleIncome.Amount);
                var income = this.incomesFactory.Create(Guid.NewGuid(), this.Date, amount, this.SimpleIncome.Side, this.SimpleIncome.Description, this.PaymentType);

                var responseExpense = JsonConvert.DeserializeObject<GeneralResponse>(await HttpRequestHelper.CreateIncomeRemote(ViewBag.Email, income));

                if (responseExpense.Status == "0")
                {
                    throw new Exception();
                }

                var user = JsonConvert.DeserializeObject<User>(await HttpRequestHelper.GetUserByEmailRemote(ViewBag.Email));

                ViewBag.Balance = user.LocalizedBalance;

                this.Message = Constants.SuccessfulfulIncomeCreation;
                this.MessageColor = Constants.MessagePositiveColor;

                this.SuccessfulIncomeRequested();

                this.SimpleIncome = this.modelFactory.CreateSimpleIncome();
                this.Date = DateTime.Today;
            }
            catch
            {
                this.Message = Constants.UnsuccessfulIncomeCreation;
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
