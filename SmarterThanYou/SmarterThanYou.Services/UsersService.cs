using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using SmarterThanYou.Services.Contracts;
using SmartherThanYou.Models;
using System.Collections.Generic;
using System.Linq;

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
            this.data.SaveChanges();
        }

        public User GetUser(int id)
        {
            return this.data.UsersRepository.GetById(id);
        }

        public User GetUserByUsername(string username)
        {
            return this.data.UsersRepository.All().Where(u => u.Username == username).FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return this.data.UsersRepository.All();
        }
    }
}
