using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TellToAsk.Model
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public AgeRating AgeRating { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
