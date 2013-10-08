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
    public class TakeQuestionModel
    {
        public int QuestionId { get; set; }

     
       
        public string QuestionTitle { get; set; }

       
        public string QuestionText { get; set; }

        
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public Gender? TargetedGender { get; set; }

     
        public string TargetedGenderValue { get; set; }

      
        public int? TargetedMinAge { get; set; }

       
        public int? TargetedMaxAge { get; set; }


       [DisplayFormat(DataFormatString = "{0:dddd dd MMMM HH:mm:ss}")]
        public DateTime DateAsked { get; set; }
        public static Expression<Func<Question, TakeQuestionModel>> FromQuestion
        {
            get
            {
                return x => new TakeQuestionModel()
                {
                    QuestionId = x.QuestionId,
                    QuestionTitle = x.Title,
                    QuestionText = x.Text,
                    TargetedGender = x.TargetedGender,
                    TargetedMaxAge = x.TargetedMaxAge,
                    TargetedMinAge = x.TargetedMinAge,
                    DateAsked = x.DateAsked,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name
                };
            }
        }
    }
}