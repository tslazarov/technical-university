﻿namespace Lipwig.Web.Models
{
    public class UserUpdatePasswordViewModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}