using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TellToAsk.Areas.Administration.Models
{
    public class AnswerView
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Reported { get; set; }
    }
}