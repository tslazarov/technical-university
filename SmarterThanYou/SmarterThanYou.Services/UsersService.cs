using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using SmarterThanYou.Services.Contracts;
using System.Collections.Generic;
using SmartherThanYou.Models;

namespace SmarterThanYou.Services
{
    public class UsersService : IUsersService
    {
        private readonly ÌSmarterThanYouData data;

        public UsersService(ÌSmarterThanYouData data)
        {
            Guard.WhenArgument<ÌSmarterThanYouData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public void CreateUser(User user)
        {
            this.data.UsersRepository.Add(user);
        }

        public User GetUser(int id)
        {
            return this.data.UsersRepository.GetById(id);
        }

        public IEnumerable<User> GetUsers()
        {
            return this.data.UsersRepository.All();
        }
    }
}
