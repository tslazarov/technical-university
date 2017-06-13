using Lipwig.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lipwig.Models;
using Lipwig.Data.Contracts;
using Bytes2you.Validation;

namespace Lipwig.Services
{
    public class IncomesService : IIncomesService
    {
        private readonly ILipwigData data;

        public IncomesService(ILipwigData data)
        {
            Guard.WhenArgument<ILipwigData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public Income GetIncome(Guid id)
        {
            return this.data.IncomesRepository.GetById(id);
        }

        public void DeleteIncome(Income income)
        {
            this.data.IncomesRepository.Delete(income);
            this.data.SaveChanges();
        }

        public void UpdateIncome(Income income)
        {
            this.data.IncomesRepository.Update(income);
            this.data.SaveChanges();
        }
    }
}
