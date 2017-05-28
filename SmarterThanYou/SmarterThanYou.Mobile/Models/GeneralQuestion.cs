using System.Collections.Generic;

namespace SmarterThanYou.Mobile.Models
{
    public class GeneralQuestion
    {
        public Answer Answer { get; set; }

        public List<Answer> Answers { get; set; }

        public Category Category { get; set; }

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Quest { get; set; }

        public int AnswerId { get; set; }
    }
}
