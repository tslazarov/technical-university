using System;
using System.Collections.Generic;

namespace MyCommute.Models
{
    public partial class User : IDataItem
    {
        public User()
        {
            Cars = new HashSet<Car>();
            RidesUsers = new HashSet<RidesUser>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public string Image { get; set; }
        public bool IsExternal { get; set; }
        public string ProviderName { get; set; }

        public ICollection<Car> Cars { get; set; }
        public ICollection<RidesUser> RidesUsers { get; set; }
    }
}
