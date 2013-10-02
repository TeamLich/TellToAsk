using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TellToAsk.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TellToAsk.Data
{
    public interface IUowData : IDisposable
    {
        IRepository<Category> Categories { get; }

        IRepository<Question> Questions { get; }
        
        IRepository<Answer> Answers { get; }

        IRepository<ApplicationUser> Users { get; }

        IRepository<Role> Roles { get; }

        int SaveChanges();
    }
}