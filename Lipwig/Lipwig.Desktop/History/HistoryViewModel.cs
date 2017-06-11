using Lipwig.Models.Contracts;
using Lipwig.Services.Contracts;
using Lipwig.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop.History
{
    public class HistoryViewModel : BindableBase
    {
        private IUsersService usersService;
        private ObservableCollection<ITransaction> transactions;

        public HistoryViewModel(IUsersService usersService)
        {
            this.usersService = usersService;
            this.PopulateTransactions();
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

        private void PopulateTransactions()
        {
            var user = this.usersService.GetUserByEmail(Constants.Email);

            if(user != null)
            {
                var transactions = new List<ITransaction>(user.Expenses)
                    .Concat(new List<ITransaction>(user.Incomes))
                    .OrderByDescending(t => t.Date);

                this.Transactions = new ObservableCollection<ITransaction>(transactions);
            }
        }
    }
}
