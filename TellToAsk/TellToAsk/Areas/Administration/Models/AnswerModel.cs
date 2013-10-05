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
                    Question = answer.Question.Title,
                    Creator = answer.User.UserName,
                    IsReported = answer.IsReported,
                    Reported = answer.IsReported == false ? "" : "Reported",
                    AnsweredDate = answer.DateAnswered
                };
            }
        }

        public bool IsReported { get; set; }

        public string Question { get; set; }

        public string Creator { get; set; }

        public DateTime AnsweredDate { get; set; }

    }
}