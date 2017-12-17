using Lipwig.Models;
using System;
using System.Data.Entity.Migrations;

namespace Lipwig.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LipwigContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LipwigContext context)
        {
            context.Currencies.AddOrUpdate(
                  c => c.Name,
                  new Currency { Id = Guid.NewGuid(), Name = "EUR", Value = 1M },
                  new Currency { Id = Guid.NewGuid(), Name = "USD", Value = 1.12M },
                  new Currency { Id = Guid.NewGuid(), Name = "BGN", Value = 1.95M },
                  new Currency { Id = Guid.NewGuid(), Name = "GBP", Value = 0.87M },
                  new Currency { Id = Guid.NewGuid(), Name = "RUB", Value = 63.88M },
                  new Currency { Id = Guid.NewGuid(), Name = "CNY", Value = 7.61M },
                  new Currency { Id = Guid.NewGuid(), Name = "INR", Value = 71.95M }
                );
        }
    }
}
