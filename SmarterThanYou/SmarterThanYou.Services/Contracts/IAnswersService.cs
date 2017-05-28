using SmartherThanYou.Models;

namespace SmarterThanYou.Services.Contracts
{
    public interface IAnswersService
    {
        void CreateAnswer(Answer answer);

        Answer GetAnswer(string member);
    }
}
