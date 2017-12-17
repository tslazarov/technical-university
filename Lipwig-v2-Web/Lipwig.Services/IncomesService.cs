using Bytes2you.Validation;
using Lipwig.Data.Contracts;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using System;

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
