using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TellToAsk.Model;

namespace TellToAsk.Areas.LoggedUser.Models
{
   public class CategoryModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

       
        public static Expression<Func<Category, CategoryModel>> FromCategory
        {
            get
            {
                return x => new CategoryModel()
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                   
                };
            }
        }
    }
}
