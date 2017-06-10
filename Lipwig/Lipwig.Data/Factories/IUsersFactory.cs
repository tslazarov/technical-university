using Lipwig.Models;
using System;

namespace Lipwig.Data.Factories
{
    public interface IUsersFactory
    {
        User Create(Guid id,
            string email,
            string firstName,
            string lastName,
            decimal balance,
            Currency currency);
    }
}
