using Bytes2you.Validation;
using Lipwig.Data.Contracts;
using Lipwig.Models;
using Lipwig.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

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
