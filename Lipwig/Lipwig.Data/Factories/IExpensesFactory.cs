using Lipwig.Models;
using System;

namespace Lipwig.Data.Factories
{
    public interface IExpensesFactory
    {
        Expense Create(Guid id,
            DateTime date,
            decimal amount,
            string side,
            string description,
            PaymentType paymentType,
            CategoryType categoryType);
    }
}
