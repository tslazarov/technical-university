using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using SmarterThanYou.Services.Contracts;
using SmartherThanYou.Models;
using System;
using System.Linq;

namespace SmarterThanYou.Services
{
    public class QuestionsService : IQuestionsService
    {
        private ÌSmarterThanYouData data;
        private Random random;

        public QuestionsService(ÌSmarterThanYouData data)
        {
            Guard.WhenArgument<ÌSmarterThanYouData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
            this.random = new Random();
        }


        public Question GetQuestion()
        {
            return this.data.QuestionsRepository.All()
                .Skip(this.GetRandomNumber() - 1)
                .Take(1)
                .FirstOrDefault();
        }

        public Question GetQuestion(Category category)
        {
            return this.data.QuestionsRepository.All()
                .Where(q => q.Category.Name == category.Name)
                .Skip(this.GetRandomNumber() - 1)
                .Take(1)
                .FirstOrDefault();
        }

        public int GetQuestionsCount()
        {
            return this.data.QuestionsRepository.All().Count();
        }

        private int GetRandomNumber()
        {
            var upperBound = this.GetQuestionsCount();

            if(upperBound > 1)
            {
                return random.Next(1, upperBound);
            }

            return 1;
        }
    }
}
