using SmartherThanYou.Models;

namespace SmarterThanYou.Data.Factories
{
    public interface IScoresFactory
    {
        Score CreateScore(int? userId, User user, long points);
    }
}
