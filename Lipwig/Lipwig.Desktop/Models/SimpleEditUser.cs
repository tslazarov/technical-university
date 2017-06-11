using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                SetProperty(ref this.email, value);
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
                SetProperty(ref this.firstName, value);
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
                SetProperty(ref this.lastName, value);
            }
        }
    }
}
