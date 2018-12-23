using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Auth
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Have to supply an e-mail address")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Have to supply an e-mail address")]
        public string LastName { get; set; }

        public string Provider { get; set; }

    }
}
