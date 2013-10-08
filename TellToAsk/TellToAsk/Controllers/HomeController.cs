using System;
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

            if (this.HttpContext.Cache["HomePageTellToAskQuestionsData"] == null)
            {
                IEnumerable<dynamic> questionsData = new List<dynamic>();
                IEnumerable<dynamic> answersData = new List<dynamic>();

                var questionsCount = this.Data.Questions.All().Count();

                var regUsersCount = this.Data.Users.All().Count();

                

                if (questionsCount > 0)
                {
                    var quest = this.Data.Categories.All().ToList().Select(x => new { category = this.Server.HtmlEncode(x.Name), value = x.Questions.Count * 100 / questionsCount }).ToList();

                    questionsData = quest.Where(q => q.value != 0);
                }

                var answersCount = this.Data.Answers.All().Count();
                if (answersCount > 0)
                {
                    var ans = this.Data.Categories.All().ToList().Select(x =>
                     new
                     {
                         category = this.Server.HtmlEncode(x.Name),
                         value = this.Data.Answers.All().Where(a => a.Question.CategoryId == x.CategoryId).Count() * 100 / answersCount
                     }).ToList();

                    answersData = ans.Where(a => a.value != 0);
                }

                this.HttpContext.Cache.Add("HomePageTellToAskRegUsersData", regUsersCount, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
                this.HttpContext.Cache.Add("HomePageTellToAskAnswersData", answersData, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
                this.HttpContext.Cache.Add("HomePageTellToAskQuestionsData", questionsData, null, DateTime.Now.AddMinutes(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Default, null);
            }

             ViewBag.AnswersData = this.HttpContext.Cache["HomePageTellToAskAnswersData"];
             ViewBag.QuestionsData = this.HttpContext.Cache["HomePageTellToAskQuestionsData"];
             ViewBag.RegisteredUsers = this.HttpContext.Cache["HomePageTellToAskRegUsersData"];
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
    }
}