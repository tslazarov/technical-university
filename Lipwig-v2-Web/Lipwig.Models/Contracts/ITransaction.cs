using System;

namespace Lipwig.Models.Contracts
{
    public interface ITransaction
    {
        Guid Id { get; set; }

        DateTime Date { get; set; }
   
        decimal Amount { get; set; }

        decimal LocalizedAmount { get; }

        string Side { get; set; }

        string Description { get; set; }

        PaymentType PaymentType { get; set; }

        string CategoryName { get; }

        bool IsExpense { get; }

        bool IsIncome { get; }
    }
}
