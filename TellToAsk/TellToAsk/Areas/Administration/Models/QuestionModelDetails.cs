using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
    public class QuestionModelDetails : QuestionModel
    {
        public IEnumerable<AnswerView> Answers { get; set; }

        public static Expression<Func<Question, QuestionModelDetails>> FromQuestionDetails
        {
            get
            {
                return question => new QuestionModelDetails
                {
                    QuestionId = question.QuestionId,
                    Title = question.Title,
                    Text = question.Text,
                    AnswersCount = question.Answers.Count,
                    Creator = question.Creator.UserName,
                    Approved = question.IsApproved == false ? "not approved" : "approved",
                    Category = question.Category.Name,
                    TargetedGender = question.TargetedGender == null ? "" : (question.TargetedGender == 0 ? "Male" : "Female"),
                    TargetedMinAge = question.TargetedMinAge,
                    TargetedMaxAge = question.TargetedMaxAge,
                    Answers = question.Answers.Select(a => new AnswerView() 
                        {
                            Id = a.AnswerId,
                            Text = a.Comment,
                            Reported = a.IsReported == false ? "" : "Reported"
                        })
                };
            }
        }
    }
}