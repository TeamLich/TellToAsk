using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Models
{
   public class CategoryModel
    {
        public int CategoryId { get; set; }

        [AllowHtml]
        public string Name { get; set; }

        public AgeRating AgeRating { get; set; }
       
        public static Expression<Func<Category, CategoryModel>> FromCategory
        {
            get
            {
                return x => new CategoryModel()
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    AgeRating = x.AgeRating,
                };
            }
        }
    }
}
