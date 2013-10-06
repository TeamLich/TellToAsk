using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TellToAsk.Model
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public string Comment { get; set; }

        public virtual Question Question { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool IsReported { get; set; }
        public bool IsRead { get; set; }
        public bool IsVoted { get; set; }

        public DateTime DateAnswered { get; set; }

    }
}
