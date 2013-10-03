using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Model;

namespace TellToAsk.Areas.LoggedUser.Models
{
   
    public class DetailsModel
    {
        public IEnumerable<AnswerModel> Answers { get; set; }

        public int QuestionId { get; set; }

        
        public string QuestionText { get; set; }


    }
}