using Lipwig.Models;
using System;

namespace Lipwig.Services.Contracts
{
    public interface IExpensesService
    {
        Expense GetExpense(Guid id);

        void DeleteExpense(Expense expense);

        void UpdateExpense(Expense expense);
    }
}
