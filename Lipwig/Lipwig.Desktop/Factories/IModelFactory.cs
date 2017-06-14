using Lipwig.Desktop.Models;

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
