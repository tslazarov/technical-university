using SmartherThanYou.Models;
using System.Collections.Generic;

namespace SmarterThanYou.Services.Contracts
{
    public interface IScoresService
    {
        IEnumerable<Score> GetTopScores(int count);
    }
}
