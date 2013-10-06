using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using TellToAsk.Model;

namespace TellToAsk.Areas.LoggedUser.Models
{
    
   public class AnswerModel
    {
        public int AnswerId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionText { get; set; }
        public int QuestionId { get; set; }
        public bool IsVoted { get; set; }

        [DisplayFormat(DataFormatString = "{0:dddd dd MMMM HH:mm:ss}")]
        public DateTime DateAnswered { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 20, ErrorMessage="The commet should be at least 20 symbols")]
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Comment { get; set; }

        public static Expression<Func<Answer, AnswerModel>> FromAnswer
        {
            get
            {
                return x => new AnswerModel()
                {
                    AnswerId = x.AnswerId,
                    Comment = x.Comment,
                    QuestionId = x.Question.QuestionId,
                    QuestionTitle = x.Question.Text,
                    QuestionText = x.Question.Title,
                    DateAnswered = x.DateAnswered,
                    IsVoted = x.IsVoted

                };
            }
        }
    }
}
