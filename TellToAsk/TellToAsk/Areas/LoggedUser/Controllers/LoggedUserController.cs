using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Areas.LoggedUser.Models;
using TellToAsk.Controllers;
using TellToAsk.Data;
using Kendo.Mvc.Extensions;

namespace TellToAsk.Areas.LoggedUser.Controllers
{
    public class LoggedUserController : BaseController
    {

          public LoggedUserController()
            : this(new UowData())
        {
        }

          public LoggedUserController(IUowData data)
            : base(data)
        {
        }

        //
        // GET: /LoggedUser/LoggedUser/

        public ActionResult MyQuestions()
        {
            var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);
            return View(questions);
        }

        public JsonResult GetMyQuestions([DataSourceRequest]DataSourceRequest request)
        {
          var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);
          var list = questions.ToList();
          return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TakeQuestion()
        {
            var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);
            return View(questions);
        }
	}
}