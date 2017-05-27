using SmartherThanYou.Models;

namespace SmarterThanYou.Services.Contracts
{
    public interface IQuestionsService
    {
        void CreateQuestion(Question question);

        Question GetQuestion();

        Question GetQuestion(Category category);

        int GetQuestionsCount();
    }
}
