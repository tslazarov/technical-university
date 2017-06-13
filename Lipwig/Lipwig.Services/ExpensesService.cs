using Lipwig.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lipwig.Models;
using Bytes2you.Validation;
using Lipwig.Data.Contracts;

namespace Lipwig.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly ILipwigData data;

        public ExpensesService(ILipwigData data)
        {
            Guard.WhenArgument<ILipwigData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public Expense GetExpense(Guid id)
        {
            return this.data.ExpensesRepository.GetById(id);
        }

        public void DeleteExpense(Expense expense)
        {
            this.data.ExpensesRepository.Delete(expense);
            this.data.SaveChanges();
        }

        public void UpdateExpense(Expense expense)
        {
            this.data.ExpensesRepository.Update(expense);
            this.data.SaveChanges();
        }
    }
}
