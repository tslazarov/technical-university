﻿using Lipwig.Models;
using System;

namespace Lipwig.Services.Contracts
{
    public interface IUsersService
    {
        User GetUserById(Guid id);

        User GetUserByEmail(string email);

        User Login(string email, string password);

        void Register(User user, string password);

        void SaveIncome(string email, Income income);

        void SaveExpense(string email, Expense expense);

        void UpdateUser(User user);

        bool UpdateUserPassword(string email, string oldPassword, string newPassword);
    }
}
