using Lipwig.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop.Models
{
    public class UserPostLogin
    {
        public string Email { get; set; }

        public Currency Currency { get; set; }

        public decimal LocalizedBalance { get; set; }
    }
}
