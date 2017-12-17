using Lipwig.Models;
using System;

namespace Lipwig.Services.Contracts
{
    public interface IIncomesService
    {
        Income GetIncome(Guid id);

        void DeleteIncome(Income income);

        void UpdateIncome(Income income);
    }
}
