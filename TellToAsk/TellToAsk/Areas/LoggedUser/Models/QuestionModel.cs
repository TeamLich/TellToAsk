using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TellToAsk.Model;

namespace TellToAsk.Areas.LoggedUser.Models
{

    public class QuestionModel
    {
        public int QuestionId { get; set; }

        [Display(Name = "Title")]
        [AllowHtml]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string QuestionTitle { get; set; }

        [Display(Name = "Text")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string QuestionText { get; set; }

        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }

        [Display(Name = "Targeted Gender")]
        public int? TargetedGender { get; set; }

        [DisplayFormat(NullDisplayText = "Not specified")]
        public string TargetedGenderValue { get; set; }

        [Display(Name = "Targeted Min Age")]
        [DisplayFormat(NullDisplayText = "Not specified")]
        [Range(1, 100, ErrorMessage = "Must be a positive number int the range of 1 to 100.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Must be a positive number int the range of 1 to 100.")]
        public int? TargetedMinAge { get; set; }

        [Display(Name="Targeted Max Age")]
        [DisplayFormat(NullDisplayText = "Not specified")]
        [Range(1, 100, ErrorMessage = "Must be a positive number int the range of 1 to 100.")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Must be a positive number int the range of 1 to 100.")]
        public int? TargetedMaxAge { get; set; }


        [DisplayFormat(DataFormatString = "{0:dddd dd MMMM HH:mm:ss}")]
        public DateTime DateAsked { get; set; }
        public static Expression<Func<Question, QuestionModel>> FromQuestion
        {
            get
            {
                return x => new QuestionModel()
                {
                    QuestionId = x.QuestionId,
                    QuestionTitle = x.Title,
                    QuestionText = x.Text,
                    Category = new List<Category>() { x.Category }.AsQueryable().Select(CategoryModel.FromCategory).FirstOrDefault(),
                    TargetedGender = (int)x.TargetedGender,
                    TargetedMaxAge = x.TargetedMaxAge,
                    TargetedMinAge = x.TargetedMinAge,
                    CategoryId = x.CategoryId,
                    DateAsked = x.DateAsked


                };
            }
        }
    }
}