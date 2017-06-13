using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop.Models
{
    public class SimpleIncome : BindableBase
    {
        private string side;
        private string description;
        private string amount;

        public string Side
        {
            get
            {
                return this.side;
            }
            set
            {
                SetProperty(ref this.side, value);
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                SetProperty(ref this.description, value);
            }
        }

        public string Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                SetProperty(ref this.amount, value);
            }
        }
    }
}
