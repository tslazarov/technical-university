using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using SmartherThanYou.Models;

namespace SmarterThanYou.Data
{
    public class SmarterThanYouData : ÌSmarterThanYouData
    {
        private readonly ISmarterThanYouContext dbContext;
        private readonly IEfRepository<Answer> answersRepository;
        private readonly IEfRepository<Category> categoriesRepository;
        private readonly IEfRepository<Question> questionsRepository;
        private readonly IEfRepository<Score> scoresRepository;
        private readonly IEfRepository<User> usersRepository;

        public SmarterThanYouData(ISmarterThanYouContext dbContext,
            IEfRepository<Answer> answersRepository,
            IEfRepository<Category> categoriesRepository,
            IEfRepository<Question> questionsRepository,
            IEfRepository<Score> scoresRepository,
            IEfRepository<User> usersRepository)
        {
            Guard.WhenArgument<ISmarterThanYouContext>(dbContext, "Database context cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Answer>>(answersRepository, "Answers repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Category>>(categoriesRepository, "Categories repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Question>>(questionsRepository, "Questions repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Score>>(scoresRepository, "Scores repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<User>>(usersRepository, "Users repository cannot be null.")
                .IsNull()
                .Throw();

            this.dbContext = dbContext;
            this.answersRepository = answersRepository;
            this.categoriesRepository = categoriesRepository;
            this.questionsRepository = questionsRepository;
            this.scoresRepository = scoresRepository;
            this.usersRepository = usersRepository;
        }

        public IEfRepository<Answer> AnswersRepository
        {
            get
            {
                return this.answersRepository;
            }
        }

        public IEfRepository<Category> CategoriesRepository
        {
            get
            {
                return this.categoriesRepository;
            }
        }

        public IEfRepository<Question> QuestionsRepository
        {
            get
            {
                return this.questionsRepository;
            }
        }

        public IEfRepository<Score> ScoresRepository
        {
            get
            {
                return this.scoresRepository;
            }
        }

        public IEfRepository<User> UsersRepository
        {
            get
            {
                return this.usersRepository;
            }
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }
    }
}
