using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using SmarterThanYou.Services.Contracts;
using SmartherThanYou.Models;
using System.Linq;

namespace SmarterThanYou.Services
{
    public class AnswersService : IAnswersService
    {
        private ÌSmarterThanYouData data;

        public AnswersService(ÌSmarterThanYouData data)
        {
            Guard.WhenArgument<ÌSmarterThanYouData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }
        public void CreateAnswer(Answer answer)
        {
            this.data.AnswersRepository.Add(answer);
            this.data.SaveChanges();
        }

        public Answer GetAnswer(string member)
        {
            return this.data.AnswersRepository.All().Where(a => a.Member == member).FirstOrDefault();
        }
    }
}
