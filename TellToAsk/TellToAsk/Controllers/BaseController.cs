using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Data;
using TellToAsk.Model;

namespace TellToAsk.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
            : this(new UowData())
        {
        }

        public BaseController(IUowData data)
        {
            this.Data = data;
        }

        public IUowData Data { get; set; }

        protected IList<SelectListItem> PopulateAgeRatings()
        {
            var list = new List<SelectListItem>();

            foreach (AgeRating rating in Enum.GetValues(typeof(AgeRating)))
            {
                list.Add(new SelectListItem()
                {
                    Value = ((int)rating).ToString(),
                    Text = rating.ToString()
                });
            }

            return list;
        }
	}
}