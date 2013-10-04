using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
    public class AnswerModel : AnswerView
    {
        public static Expression<Func<Answer, AnswerModel>> FromAnswer
        {
            get
            {
                return answer => new AnswerModel
                {
                    Id = answer.AnswerId,
                    Text = answer.Comment,  
                    Question = answer.Question.Text,
                    Creator = answer.User.UserName,
                    Reported = answer.IsReported == false ? "" : "Reported",
                };
            }
        }

        public string Question { get; set; }

        public string Creator { get; set; }

    }
}