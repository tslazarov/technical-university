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
    public class CurrenciesService : ICurrenciesService
    {
        private readonly ILipwigData data;

        public CurrenciesService(ILipwigData data)
        {
            Guard.WhenArgument<ILipwigData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return this.data.CurrenciesRepository.All().ToList();
        }
    }
}
