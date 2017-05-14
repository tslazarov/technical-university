using SmartherThanYou.Models;
using System.Collections.Generic;

namespace SmarterThanYou.Services.Contracts
{
    public interface IUsersService
    {
        void CreateUser(User user);

        User GetUser(int id);

        IEnumerable<User> GetUsers();
    }
}
