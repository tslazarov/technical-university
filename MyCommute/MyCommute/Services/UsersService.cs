using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCommute.Data.Contracts;
using MyCommute.Models;
using MyCommute.Utilities;

namespace MyCommute.Services
{
    public class UsersService : IUsersService
    {
        private readonly IManager usersManager;
        private readonly IPasswordHelper passwordHelper;

        public UsersService(IUsersManager usersManager, IPasswordHelper passwordHelper)
        {
            this.usersManager = usersManager as IManager;
            this.passwordHelper = passwordHelper;
        }

        public Task<User> AddExternalUser(string email, string firstName, string lastName, string provider = null)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                ProviderName = provider,
                IsExternal = true,
                FirstName = firstName,
                LastName = lastName
            };

            this.usersManager.CreateItem(user);
            this.usersManager.SaveChanges();

            return Task.FromResult(user);
        }

        public Task<User> AddLocalUser(string email, string password, string provider = null)
        {
            var salt = this.passwordHelper.CreateSalt(10);
            var hashedPassword = this.passwordHelper.CreatePasswordHash(password, salt);

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                HashedPassword = hashedPassword,
                Salt = salt,
                ProviderName = provider,
                IsExternal = false,
            };

            this.usersManager.CreateItem(user);
            this.usersManager.SaveChanges();

            return Task.FromResult(user);
        }

        public Task<User> GetUserByIdentifier(string email, string provider = null)
        {
            var user = (this.usersManager.GetItems() as IEnumerable<User>).Where(i => i.Email == email && i.ProviderName == provider).FirstOrDefault();

            if (user != null)
            {
                return Task.FromResult(user);
            }
            return Task.FromResult<User>(null);
        }

        public void UpdateImage(User user, string path)
        {
            user.Image = path;

            this.usersManager.UpdateItem(user);
            this.usersManager.SaveChanges();
        }

        public Task<User> UpdateLocalUser(string email, string firstName, string lastName, string provider = null)
        {
            var user = this.GetUserByIdentifier(email, provider).Result;

            if(user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;

                this.usersManager.UpdateItem(user);
                this.usersManager.SaveChanges();

                return Task.FromResult(user);
            }

            return Task.FromResult<User>(null);
        }

        public Task<bool> ValidateCredentials(out User user, string email, string password, string provider = null)
        { 
            user = (this.usersManager.GetItems() as IEnumerable<User>).Where(i => i.Email == email && i.ProviderName == provider).FirstOrDefault();

            if (user != null)
            {
                string hashedPassword = this.passwordHelper.CreatePasswordHash(password, user.Salt);

                if (user.HashedPassword == hashedPassword)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(false);
        }
    }
}
