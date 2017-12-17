using Lipwig.Models;
using System.Collections.Generic;

namespace Lipwig.Services.Contracts
{
    public interface ICurrenciesService
    {
        IEnumerable<Currency> GetCurrencies();
    }
}
