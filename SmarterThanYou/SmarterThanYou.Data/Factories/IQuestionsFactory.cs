using SmartherThanYou.Models;

namespace SmarterThanYou.Data.Factories
{
    public interface IQuestionsFactory
    {
        Question CreateQuestion(int? categoryId,
            Category category,
            string quest);
    }
}
