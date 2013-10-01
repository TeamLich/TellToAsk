using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TellToAsk.Model;

namespace TellToAsk.Data
{
    public class TellToAskContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Question> Questions { get; set; }

        public IDbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(m => m.MyQuestions);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(m => m.AnsweredQuestions)
                .WithMany(p => p.Users)
                .Map(m =>
                {
                    m.ToTable("QuestionsUsers");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("QuetionId");
                }
                );
               
        }
    }
}
