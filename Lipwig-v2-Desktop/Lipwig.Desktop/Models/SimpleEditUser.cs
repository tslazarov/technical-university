namespace Lipwig.Desktop.Models
{
    public class SimpleEditUser : BindableBase
    {
        private string email;
        private string firstName;
        private string lastName;


        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
               this.SetProperty(ref this.email, value);
            }
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
               this.SetProperty(ref this.firstName, value);
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
               this.SetProperty(ref this.lastName, value);
            }
        }
    }
}
