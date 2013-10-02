using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TellToAsk.Model
{
    public class Question
    {
        public int QuestionId { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Text { get; set; }

        public bool IsApproved { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public Gender? TargetedGender { get; set; }

        
        public int? TargetedMinAge { get; set; }

        public int? TargetedMaxAge { get; set; }

        [Required]
        [Display(Name="Category")]
        public  int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public Question()
        {
           
            this.Answers = new HashSet<Answer>();
        }
    }
}
