using System;
using TellToAsk.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Data;

namespace TellToAsk.Controllers
{
    public class HomeController : BaseController
    {

          public HomeController()
            : this(new UowData())
        {
        }

          public HomeController(IUowData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
           
             var questionsCount =  this.Data.Questions.All().Count();

            ViewBag.RegisteredUsers = this.Data.Users.All().Count();

            if (questionsCount > 0)
            {
                ViewBag.QuestionsData = this.Data.Categories.All().Select(x => new { category = x.Name, value = x.Questions.Count * 100 / questionsCount }).ToList();
            }

            var answersCount = this.Data.Answers.All().Count();
            if (answersCount > 0)
            {
                ViewBag.AnswersData = this.Data.Categories.All().ToList().Select(x =>
                 new { category = x.Name, value = this.Data.Answers.All().Where(a => a.Question.CategoryId == x.CategoryId).Count() * 100 / answersCount }).ToList();

            }
         
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}