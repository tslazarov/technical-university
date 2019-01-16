using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Users
{
    public class UserDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsFriend { get; set; }
        public bool IsPending { get; set; }
        public Guid CurrentUserId { get; set; }
        public bool IsRatedAsDriver { get; set; }
        public int DriverRating { get; set; }
        public bool IsRatedAsPassenger { get; set; }
        public int PassengerRating { get; set; }
        public double DriverRatingDisplayed { get;set; }
        public double PassengerRatingDisplayed { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
    }
}
