using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Models.Profile
{
    public class ProfileDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsExternal { get; set; }
        public ProfilePersonalInformationViewModel PersonalInformationViewModel { get; set; }
        public ProfileCarInformationViewModel CarInformationViewModel { get; set; }
    }
}
