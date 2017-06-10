using Lipwig.Models;
using System;

namespace Lipwig.Data.Factories
{
    public interface IIncomesFactory
    {
        Income CreateIncome(Guid id,
            DateTime date,
            decimal amount,
            string side,
            string description,
            PaymentType paymentType);
    }
}
