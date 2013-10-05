using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
    public class UserDetailsModel : UserModel
    {
        public static Expression<Func<ApplicationUser, UserDetailsModel>> FromUsersDetails
        {
            get
            {
                return user => new UserDetailsModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender == 0 ? "Male" : "Female",
                    Points = user.Points,
                    Roles = user.Roles.Select(x => x.Role.Name),
                    Questions = user.MyQuestions.Select(q => new QuestionView() 
                    {
                        QuestionId = q.QuestionId,
                        Title = q.Title,
                        Approved = q.IsApproved == false ? "not approved" : "approved"
                    }),
                    Answers = user.Answers.Select(a => new AnswerView()
                    {
                        Id = a.AnswerId,
                        Text = a.Comment,
                        Reported = a.IsReported == false ? "" : "Reported"
                    })
                };
            }
        }
        public IEnumerable<QuestionView> Questions { get; set; }

        public IEnumerable<AnswerView> Answers { get; set; }
    }
}