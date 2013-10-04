﻿using System;
using TellToAsk.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using TellToAsk.Areas.LoggedUser.Models;

namespace TellToAsk.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IUowData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
           
            var questionsCount = this.Data.Questions.All().Count();

            ViewBag.RegisteredUsers = this.Data.Users.All().Count();

            if (questionsCount > 0)
            {
                var quest = this.Data.Categories.All().ToList().Select(x => new { category = this.Server.HtmlEncode(x.Name), value = x.Questions.Count * 100 / questionsCount }).ToList();

                ViewBag.QuestionsData = quest.Where(q => q.value != 0);
            }

            var answersCount = this.Data.Answers.All().Count();
            if (answersCount > 0)
            {
                var ans = this.Data.Categories.All().ToList().Select(x =>
                 new { category = this.Server.HtmlEncode(x.Name), 
                       value = this.Data.Answers.All().Where(a => a.Question.CategoryId == x.CategoryId).Count() * 100 / answersCount }).ToList();

                ViewBag.AnswersData = ans.Where(a => a.value != 0);
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "This time is personal.";

            return View();
        }

        public JsonResult GetCategories([DataSourceRequest]DataSourceRequest request)
        {
            var categories = this.Data.Categories.All().Select(CategoryModel.FromCategory);
            var x = categories.ToList().Count;
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
    }
}