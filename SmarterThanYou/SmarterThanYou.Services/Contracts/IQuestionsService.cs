using SmartherThanYou.Models;

namespace SmarterThanYou.Services.Contracts
{
    public interface IQuestionsService
    {
        Question GetQuestion();

        Question GetQuestion(Category category);

        int GetQuestionsCount();
    }
}
