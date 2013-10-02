using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TellToAsk.Model
{
    public class ApplicationUser : User
    {
        public DateTime? BirthDate { get; set; }

        public Gender Gender { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Question> MyQuestions { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public ApplicationUser()
        {
            this.MyQuestions = new HashSet<Question>();
            this.Answers = new HashSet<Answer>();
            this.Categories = new HashSet<Category>();
        }

    }
}
