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
               this.SetProperty(ref this.side, value);
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
               this.SetProperty(ref this.description, value);
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
               this.SetProperty(ref this.amount, value);
            }
        }
    }
}
