using SmartherThanYou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarterThanYou.Data.Factories
{
    public interface IAnswersFactory
    {
        Answer CreateAnswer(string member);
    }
}
