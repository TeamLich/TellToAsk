using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TellToAsk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.QuestionsData = new dynamic[] 
           {
                new {category="Asia",value=53.8,color="#9de219"},
                new {category="Europe",value=16.1,color="#90cc38"},
                new {category="LatinAmerica",value=11.3,color="#068c35"},
                new {category="Africa",value=9.6,color="#006634"},
                new {category="MiddleEast",value=5.2,color="#004d38"},
                new {category="NorthAmerica",value=3.6,color="#033939"}
           };

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