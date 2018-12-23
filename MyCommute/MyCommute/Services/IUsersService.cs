using MyCommute.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommute.Services
{
    public interface IUsersService
    {
        Task<bool> ValidateCredentials(out User user, string email, string password, string provider);
        Task<User> GetUserByIdentifier(string email, string provider);
        Task<User> AddLocalUser(string email, string password, string provider);
        Task<User> UpdateLocalUser(string email, string firstName, string lastName, string provider);
        Task<User> AddExternalUser(string email, string firstName, string lastName, string provider);
    }
}
