using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Areas.LoggedUser.Models;
using TellToAsk.Data;
using TellToAsk.Model;

namespace TellToAsk.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IUowData data)
        {
            this.Data = data;
        }

        public IUowData Data { get; set; }

        protected IList<SelectListItem> PopulateGendersList()
        {
            IList<SelectListItem> genList = new List<SelectListItem>();
            foreach (Gender gen in Enum.GetValues(typeof(Gender)))
            {
                SelectListItem item = new SelectListItem()
                {
                    Selected = false,
                    Text = gen.ToString(),
                    Value = ((int)gen).ToString(),
                };

                genList.Add(item);
            }

            return genList;
        }

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

        public JsonResult GetCategories([DataSourceRequest]DataSourceRequest request)
        {
            var categories = this.Data.Categories.All().Select(CategoryModel.FromCategory);
            var x = categories.ToList().Count;
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        protected IList<CategoryModel> PopulateSuitableCategories()
        {
            var userName = this.User.Identity.Name;
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);
            List<CategoryModel> suitableCategories = new List<CategoryModel>();
            if (user != null)
            {
                var cats = this.Data.Categories.All().Select(CategoryModel.FromCategory);
                foreach (var cat in cats)
                {
                    if (cat != null)
                    {
                        if (DateTime.Now.AddYears((-1) * (int)cat.AgeRating) >= user.BirthDate.Value)
                        {
                            suitableCategories.Add(cat);
                        }
                    }
                }
                var y = suitableCategories.Count();
            }
            else
            {
                suitableCategories = this.Data.Categories.All().Select(CategoryModel.FromCategory).ToList();
            }

            return suitableCategories;
        }

        public JsonResult GetUserCategories([DataSourceRequest]DataSourceRequest request)
        {
            var suitableCategories = PopulateSuitableCategories();

            return Json(suitableCategories, JsonRequestBehavior.AllowGet);
        }
	}
}