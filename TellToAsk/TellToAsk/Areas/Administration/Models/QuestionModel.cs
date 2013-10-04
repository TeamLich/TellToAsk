using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
    public class QuestionModel
    {
        public static Expression<Func<Question, QuestionModel>> FromQuestion
        {
            get
            {
                return question => new QuestionModel
                {
                    QuestionId = question.QuestionId,
                    Text = question.Text,
                    AnswersCount = question.Answers.Count,
                    Creator = question.Creator.UserName,
                    Approved = question.IsApproved == false ? "not approved" : "approved",
                    Category = question.Category.Name,
                    TargetedGender = question.TargetedGender == null ? "" : (question.TargetedGender == 0 ? "Male" : "Female"),
                    TargetedMinAge = question.TargetedMinAge == null ? 0 : question.TargetedMinAge.Value,
                    TargetedMaxAge = question.TargetedMaxAge == null ? 0 : question.TargetedMaxAge.Value
                };
            }
        }

        public int QuestionId { get; set; }

        public string Text { get; set; }

        public int AnswersCount { get; set; }

        public string Creator { get; set; }

        public string Approved { get; set; }

        public string Category { get; set; }

        public string TargetedGender { get; set; }

        public int TargetedMinAge { get; set; }

        public int TargetedMaxAge { get; set; }
    }
}