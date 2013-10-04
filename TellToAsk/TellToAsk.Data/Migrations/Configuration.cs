namespace TellToAsk.Data.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TellToAsk.Model;

    public sealed class Configuration : DbMigrationsConfiguration<TellToAsk.Data.TellToAskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TellToAsk.Data.TellToAskContext";
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TellToAsk.Data.TellToAskContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (context.Roles.FirstOrDefault() == null)
            {
                //context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX UX_TagName ON Tags (Name)");
                context.Roles.AddOrUpdate(r => r.Name,
                    new Role("User"),
                    new Role("Admin"));


                context.Categories.AddOrUpdate(
                 c => c.Name,
                 new Category { Name = "Others", AgeRating = AgeRating.Everyone },
                 new Category { Name = "Fun", AgeRating = AgeRating.Everyone },
                 new Category { Name = "Sport", AgeRating = AgeRating.Everyone },
                 new Category { Name = "Health", AgeRating = AgeRating.Everyone },
                 new Category { Name = "In the Kitchen", AgeRating = AgeRating.Everyone },
                 new Category { Name = "In the Name of the Law", AgeRating = AgeRating.Mature },
                 new Category { Name = "Love is all Around", AgeRating = AgeRating.Mature },
                 new Category { Name = "Communication Skills", AgeRating = AgeRating.Teen },
                 new Category { Name = "18+ - For Adults Only", AgeRating = AgeRating.Adult },
                 new Category { Name = "Geeks Zone - Dreams in Code", AgeRating = AgeRating.Children }
                );
            }
        }
    }
}
