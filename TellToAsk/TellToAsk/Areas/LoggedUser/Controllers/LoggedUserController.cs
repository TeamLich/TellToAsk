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

        //
        // GET: /LoggedUser/LoggedUser/

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

            var questions = this.Data.Questions.All()
                .Where( q => user.Categories.Contains(q.Category)).Select(QuestionModel.FromQuestion);
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
            if (answerModel.Comment.Length < 100)
            {
                // validate
            }

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
	}
}