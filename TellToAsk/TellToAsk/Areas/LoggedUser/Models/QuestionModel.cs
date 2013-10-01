using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TellToAsk.Model;

namespace TellToAsk.Areas.LoggedUser.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }

        public string Text { get; set; }

        public CategoryModel Category { get; set; }

        public static Expression<Func<Question, QuestionModel>> FromQuestion
        {
            get
            {
                return x => new QuestionModel()
                {
                    QuestionId = x.QuestionId,
                    Text = x.Text,
                    Category = new List<Category>() { x.Category }.AsQueryable().Select(CategoryModel.FromCategory).FirstOrDefault(),

                };
            }
        }
    }
}