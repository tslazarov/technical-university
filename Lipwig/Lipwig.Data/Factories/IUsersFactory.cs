using Lipwig.Models;
using System;

namespace Lipwig.Data.Factories
{
    public interface IUsersFactory
    {
        User CreateUser(Guid id,
            string email,
            string firstName,
            string lastName,
            string salt,
            string hashedPassword,
            decimal totalAmount,
            Currency currency);
    }
}
