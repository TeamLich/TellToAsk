using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
    public class UserModel
    {
        public static Expression<Func<ApplicationUser, UserModel>> FromUsers
        {
            get
            {
                return user => new UserModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender == 0 ? "Male" : "Female",
                    Points = user.Points,
                    Roles = user.Roles.Select(x => x.Role.Name)
                };
            }
        }
        public string UserId { get; set; }

        public string Username { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public int Points { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}