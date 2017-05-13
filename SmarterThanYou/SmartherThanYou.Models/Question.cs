using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartherThanYou.Models
{
    public class Question
    {
        private ICollection<Answer> answers;

        public Question()
        {
            this.answers = new HashSet<Answer>();
        }

        public Question(int? categoryId, 
            Category category, 
            string quest)
        {
            this.CategoryId = categoryId;
            this.Category = category;
            this.Quest = quest;
            this.answers = new HashSet<Answer>();
        }

        [Key]
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Quest { get; set; }

        public virtual ICollection<Answer> Answers
        {
            get
            {
                return this.answers;
            }
            set
            {
                this.answers = value;
            }
        }

        public int? AnswerId { get; set; }

        public virtual Answer Answer { get; set; }
    }
}
