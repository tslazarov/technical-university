using SmartherThanYou.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SmarterThanYou.Data.Contracts
{
    public interface ISmarterThanYouContext
    {
        IDbSet<Answer> Answers { get; set; }

        IDbSet<Category> Categories { get; set; }

        IDbSet<Question> Questions { get; set; }

        IDbSet<Score> Scores { get; set; }

        IDbSet<User> Users { get; set; }

        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();
    }
}
