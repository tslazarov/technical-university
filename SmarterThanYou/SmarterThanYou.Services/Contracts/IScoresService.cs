using SmartherThanYou.Models;
using System.Collections.Generic;

namespace SmarterThanYou.Services.Contracts
{
    public interface IScoresService
    {
        void CreateScore(Score score);

        void UpdateScore(Score score);

        IEnumerable<Score> GetTopScores(int count);

        Score GetScoreByUsername(string username);
    }
}
