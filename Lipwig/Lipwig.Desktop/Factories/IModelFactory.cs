using Lipwig.Desktop.Authentication;
using Lipwig.Desktop.Models;
using Lipwig.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Desktop.Factories
{
    public interface IModelFactory
    {
        SimpleRegistrationUser CreateSimpleRegistrationUser();

        SimpleEditUser CreateSimpleEditUser();

        SimpleIncome CreateSimpleIncome();

        SimpleExpense CreateSimpleExpense();
    }
}
