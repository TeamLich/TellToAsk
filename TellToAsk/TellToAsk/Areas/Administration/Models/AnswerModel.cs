using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
    public class AnswerModel
    {
        public static Expression<Func<Answer, AnswerModel>> FromAnswer
        {
            get
            {
                return answer => new AnswerModel
                {
                    AnswerId = answer.AnswerId,
                    Comment = answer.Comment,  
                    Question = answer.Question.Text,
                    Creator = answer.User.UserName,
                    Reported = answer.IsReported == false ? "" : "Reported",
                };
            }
        }

        public int AnswerId { get; set; }

        public string Comment { get; set; }

        public string Question { get; set; }

        public string Creator { get; set; }

        public string Reported { get; set; }
    }
}