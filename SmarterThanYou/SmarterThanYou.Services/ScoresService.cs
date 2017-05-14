using SmarterThanYou.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartherThanYou.Models;
using SmarterThanYou.Data.Contracts;
using Bytes2you.Validation;

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
