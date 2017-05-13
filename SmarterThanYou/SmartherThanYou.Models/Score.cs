using System.ComponentModel.DataAnnotations;

namespace SmartherThanYou.Models
{
    public class Score
    {
        public Score()
        {

        }

        public Score(int? userId, User user, long points)
        {
            this.UserId = userId;
            this.User = user;
            this.Points = points;
        }

        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        [Required]
        public long Points { get; set; }
    }
}
