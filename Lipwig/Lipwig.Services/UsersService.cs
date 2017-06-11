using Bytes2you.Validation;
using Lipwig.Data.Contracts;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using Lipwig.Services.Utils;
using System;
using System.Linq;

namespace Lipwig.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILipwigData data;

        public UsersService(ILipwigData data)
        {
            Guard.WhenArgument<ILipwigData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public User GetUserById(Guid id)
        {
            return this.data.UsersRepository.GetById(id);
        }

        public User GetUserByEmail(string email)
        {
            return this.data.UsersRepository.All().Where(u => u.Email == email).FirstOrDefault();
        }

        public User Login(string email, string password)
        {
            var user = this.GetUserByEmail(email);

            if (user != null)
            {
                var hashedPassword = PasswordHelper.CreatePasswordHash(password, user.Salt);

                if (user.HashedPassword != hashedPassword)
                {
                    return null;
                }
            }

            return user;
        }

        public void Register(User user, string password)
        {
            var salt = PasswordHelper.CreateSalt(10);
            var hashedPassword = PasswordHelper.CreatePasswordHash(password, salt);

            user.Salt = salt;
            user.HashedPassword = hashedPassword;

            this.data.UsersRepository.Add(user);
            this.data.SaveChanges();
        }

        public void SaveIncome(string email, Income income)
        {
            var user = this.GetUserByEmail(email);
            user.Incomes.Add(income);
            user.Balance += income.Amount;

            this.data.SaveChanges();
        }

        public void SaveExpense(string email, Expense expense)
        {
            var user = this.GetUserByEmail(email);
            user.Expenses.Add(expense);
            user.Balance -= expense.Amount;

            this.data.SaveChanges();
        }
    }
}
