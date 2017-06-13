using Lipwig.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Services.Contracts
{
    public interface IIncomesService
    {
        Income GetIncome(Guid id);

        void DeleteIncome(Income income);

        void UpdateIncome(Income income);
    }
}
