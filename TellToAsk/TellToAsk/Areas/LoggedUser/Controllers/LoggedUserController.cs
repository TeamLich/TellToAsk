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
using TellToAsk.Model;

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

        public ActionResult MyQuestions()
        {
            return View();
        }

        public JsonResult GetMyQuestions([DataSourceRequest]DataSourceRequest request)
        {

            var userName = this.User.Identity.Name;

            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);

            var questions = this.Data.Questions.All().Where(u => u.Creator.Id == user.Id).Select(QuestionModel.FromQuestion);
            return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTargetedQuestions([DataSourceRequest]DataSourceRequest request)
        {
             var userName = this.User.Identity.Name;

            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);

            var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);
                //.Where( q => user.Categories.Contains(q.Category)).Select(QuestionModel.FromQuestion);
            return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public JsonResult URIDecode(string data)
        {
            string question = this.Server.UrlDecode(data);
            return Json(question, JsonRequestBehavior.AllowGet);
        }

        public ActionResult QuestionAnswers(int? id)
        {
            var question = this.Data.Questions.All().FirstOrDefault(q => q.QuestionId == id);
            
            if (question != null)
            {
               var answers = question.Answers.AsQueryable().Select(AnswerModel.FromAnswer);

               var model = new DetailsModel() { Answers = answers, QuestionId = question.QuestionId, QuestionText = question.Text };

               return View(model);
            }
            return View();
        }

        public JsonResult GetQuestionAnswers([DataSourceRequest]DataSourceRequest request, int? id)
        {
           // var id = Convert.ToInt32(this.Request.Params["id"]);

            var question = this.Data.Questions.All().FirstOrDefault(q => q.QuestionId == id);

            if (question != null)
            {
                var answers = question.Answers.AsQueryable().Select(AnswerModel.FromAnswer);
                return Json(answers.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
               
                
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TakeQuestion()
        {
            var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);
            return View(questions);
        }

        public ActionResult AnswerToQuestion(AnswerModel answerModel)
        {
            if (ModelState.IsValid)
            {
                var userName = this.User.Identity.Name;

                var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);
                var question = this.Data.Questions.All().FirstOrDefault(q => q.QuestionId == answerModel.QuestionId);

                var newAnswer = new Answer()
                {
                    IsReported = false,
                    Comment = answerModel.Comment,
                    User = user,
                    Question = question
                };

                this.Data.Answers.Add(newAnswer);

                this.Data.SaveChanges();

                return View("MyQuestions");
            }

            return PartialView("_AnswerQuestion", answerModel);
        }

        public ActionResult AskQuestion()
        {
            var list = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text= "---Select category---", Value= "-1"} };

            var categories = this.Data.Categories.All().OrderBy(c => c.Name).ToList().Select( c => new SelectListItem () { Value = c.CategoryId.ToString(), Text= c.Name } ).ToList();

            list.AddRange(categories);

            ViewBag.Categories = list;

            return View();
        }

        public ActionResult CreateQuestion(Question question)
        {

            if (question != null && question.TargetedMinAge != null && question.TargetedMaxAge != null)
            {
                // validate
            }

            var userName = this.User.Identity.Name;

            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);
            question.Creator = user;
            if (ModelState.IsValid)
            {
                this.Data.Questions.Add(question);
                this.Data.SaveChanges();
                return RedirectToAction("MyQuestions");
            }

            return View(question);
        }

        public ActionResult RenderTargetGroupForm()
        {
            return PartialView("_TargetGroupForm");
        }
	}
}