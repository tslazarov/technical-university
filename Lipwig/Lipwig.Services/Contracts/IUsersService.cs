using Lipwig.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Services.Contracts
{
    public interface IUsersService
    {
        User GetUserById(Guid id);

        User GetUserByEmail(string email);

        User Login(string email, string password);

        void Register(User user, string password);
    }
}
