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
                 new Category { Name = "Others" },
                 new Category { Name = "Fun" },
                 new Category { Name = "Sport" },
                 new Category { Name = "Health" },
                 new Category { Name = "In the Kitchen" },
                 new Category { Name = "In the Name of the Law" },
                 new Category { Name = "Love is all Around " },
                 new Category { Name = "Communication Skills" },
                 new Category { Name = "18+ - For Adults Only" },
                 new Category { Name = "Geeks Zone - Dreams in Code" }
                );
            }

        }
    }
}
