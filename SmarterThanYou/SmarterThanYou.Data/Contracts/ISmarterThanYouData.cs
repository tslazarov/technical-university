using SmartherThanYou.Models;

namespace SmarterThanYou.Data.Contracts
{
    public interface ÌSmarterThanYouData
    {
        IEfRepository<Answer> AnswersRepository { get; }

        IEfRepository<Category> CategoriesRepository { get; }

        IEfRepository<Question> QuestionsRepository { get; }

        IEfRepository<Score> ScoresRepository { get; }

        IEfRepository<User> UsersRepository { get; }

        void SaveChanges();

    }
}
