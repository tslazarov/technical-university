using System.Data.Entity.Migrations;

namespace SmarterThanYou.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SmarterThanYouContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SmarterThanYouContext context)
        {
        }
    }
}
