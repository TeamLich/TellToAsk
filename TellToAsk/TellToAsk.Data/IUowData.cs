using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TellToAsk.Model;

namespace TwiterApp.Data
{
    public interface IUowData : IDisposable
    {
        IRepository<Category> Categories { get; }

        IRepository<Question> Questions { get; }
        
        IRepository<Answer> Answers { get; }

        IRepository<ApplicationUser> Users { get; }

        int SaveChanges();
    }
}