using SmarterThanYou.Data.Contracts;
using SmartherThanYou.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SmarterThanYou.Data
{
    public class SmarterThanYouContext : DbContext, ISmarterThanYouContext
    {
        public SmarterThanYouContext() : base("SmarterThanYou")
        {
        }

        public virtual IDbSet<Answer> Answers { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Question> Questions { get; set; }

        public virtual IDbSet<Score> Scores { get; set; }

        public virtual IDbSet<User> Users { get; set; }


        public static DbContext Create()
        {
            return new SmarterThanYouContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        IDbSet<T> ISmarterThanYouContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}
