using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Users
{
    public class UsersViewModel
    {
        public string Name { get; set; }
        public string OrderBy { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }

    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double DriverRating { get; set; }
        public double PassengerRating { get; set; }
    }
}
