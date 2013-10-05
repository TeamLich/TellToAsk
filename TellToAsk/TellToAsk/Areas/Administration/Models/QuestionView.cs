using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TellToAsk.Areas.Administration.Models
{
    public class QuestionView
    {
        public int QuestionId { get; set; }

        [AllowHtml]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Title { get; set; }

        public string Approved { get; set; }
    }
}