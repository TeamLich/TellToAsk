using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
    public class QuestionEditModel
    {
        public static Expression<Func<Question, QuestionEditModel>> FromEditQuestion
        {
            get
            {
                return question => new QuestionEditModel
                {
                    Id = question.QuestionId,
                    Title = question.Title,
                    Content = question.Text,
                    IsApproved = question.IsApproved,
                    Category = question.CategoryId
                };
            }
        }
        public int Id { get; set; }

        [AllowHtml]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public bool IsApproved { get; set; }

        public int Category { get; set; }
    }
}