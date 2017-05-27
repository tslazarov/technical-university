using SmartherThanYou.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarterThanYou.Services.Contracts
{
    public interface IAnswersService
    {
        void CreateAnswer(Answer answer);

        Answer GetAnswer(string member);
    }
}
