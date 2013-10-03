using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Model;

namespace TellToAsk.Areas.LoggedUser.Models
{
    
    public class QuestionModel
    {
        public int QuestionId { get; set; }

        [AllowHtml]
        [Required]
        [StringLength(500)]
        public string Text { get; set; }

        [Range(1, int.MaxValue, ErrorMessage="Category is requerd.")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }

        public int? TargetedGender { get; set; }

        [Range(0, 100)]
        public int? TargetedMinAge { get; set; }

        [Range(0, 100)]
        public int? TargetedMaxAge { get; set; }

        public static Expression<Func<Question, QuestionModel>> FromQuestion
        {
            get
            {
                return x => new QuestionModel()
                {
                    QuestionId = x.QuestionId,
                    Text = x.Text,
                    Category = new List<Category>() { x.Category }.AsQueryable().Select(CategoryModel.FromCategory).FirstOrDefault(),
                    TargetedGender =  (int)x.TargetedGender,
                    TargetedMaxAge =  x.TargetedMaxAge,
                    TargetedMinAge =  x.TargetedMinAge,
                    CategoryId =  x.CategoryId,


                };
            }
        }
    }
}