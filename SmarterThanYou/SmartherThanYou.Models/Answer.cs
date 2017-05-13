using System.ComponentModel.DataAnnotations;

namespace SmartherThanYou.Models
{
    public class Answer
    {
        public Answer()
        {

        }

        public Answer(string member)
        {
            this.Member = member;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Member { get; set; }
    }
}
