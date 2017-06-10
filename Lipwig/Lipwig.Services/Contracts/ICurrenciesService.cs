using Lipwig.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.Services.Contracts
{
    public interface ICurrenciesService
    {
        IEnumerable<Currency> GetCurrencies();
    }
}
