using Bytes2you.Validation;
using SmarterThanYou.Data.Contracts;
using SmarterThanYou.Services.Contracts;
using SmartherThanYou.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmarterThanYou.Services
{
    public class ScoresService : IScoresService
    {
        private readonly ÌSmarterThanYouData data;

        public ScoresService(ÌSmarterThanYouData data)
        {
            Guard.WhenArgument<ÌSmarterThanYouData>(data, "Data cannot be null.")
                .IsNull()
                .Throw();

            this.data = data;
        }

        public IEnumerable<Score> GetTopScores(int count)
        {
            return this.data.ScoresRepository.All()
                .OrderByDescending(s => s.Points)
                .Skip(0)
                .Take(count);
        }
    }
}
