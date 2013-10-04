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
using System.Web.Routing;
using System.Net;

namespace TellToAsk.Areas.LoggedUser.Controllers
{
    [Authorize(Roles="User")]
    public class LoggedUserController : BaseController
    {
        private const int PointsForAnswer = 10;

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
                

                var model = new QuestionModel()
                {
                    QuestionId = question.QuestionId,
                    QuestionText = question.Text,
                    CategoryId = question.CategoryId,
                    TargetedMaxAge = question.TargetedMaxAge,
                    
                    TargetedMinAge = question.TargetedMinAge,
                };

                if (question.TargetedGender != null)
                {
                    model.TargetedGender = (int)question.TargetedGender; 
                }

               return View(model);
            }
            return View();
        }

        public JsonResult GetQuestionAnswers([DataSourceRequest]DataSourceRequest request, int? id)
        {
            var question = this.Data.Questions.All().FirstOrDefault(q => q.QuestionId == id);

            if (question != null)
            {
                var answers = question.Answers.AsQueryable().Select(AnswerModel.FromAnswer);
                return Json(answers.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuestionById(int id)
        {
            var question = this.Data.Questions.All().Select(QuestionModel.FromQuestion).FirstOrDefault(q => q.QuestionId == id);
            return Json(question, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TakeQuestion()
        {
            //var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);
            return View(new AnswerModel());
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
                    Question = question,
                    DateAnswered = DateTime.Now,
                    
                };

                this.Data.Answers.Add(newAnswer);

                this.Data.SaveChanges();
                user.Points =+ PointsForAnswer;
                this.Data.SaveChanges();
                return View("TakeQuestion", new AnswerModel());
            }

            return View("TakeQuestion", answerModel);
        }

       
        public ActionResult AskQuestion(QuestionModel questionModel)
        {
            ValidateNewQuestiionInput(questionModel);

            var list = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text= "---Select category---", Value= "-1"} };


            var categories = this.Data.Categories.All().OrderBy(c => c.Name).ToList().Select( c => new SelectListItem () { Value = c.CategoryId.ToString(), Text= c.Name } ).ToList();

            list.AddRange(categories);
            
            ViewBag.Categories = list;


            var genders = this.PopulateGendersList();

            var list1 = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "---Select gender---", Value = "-1" } };

            list1.AddRange(genders);

            ViewBag.genders = list1;

            return View(questionModel);
        }

        public ActionResult CreateQuestion(QuestionModel questionModel)
        {

            ValidateNewQuestiionInput(questionModel);

           
            if (ModelState.IsValid)
            {
                var userName = this.User.Identity.Name;
                var currentUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);


                var newQuestion = new Question()
                {
                    Text = questionModel.QuestionText,
                    CategoryId = questionModel.CategoryId,
                    Title = questionModel.QuestionTitle,
                    TargetedMaxAge = questionModel.TargetedMaxAge,
                    TargetedMinAge = questionModel.TargetedMinAge,
                    Creator = currentUser,
                    DateAsked = DateTime.Now
                };


                if (questionModel.TargetedGender < 0)
                {
                    newQuestion.TargetedGender = null;
                }
                else
                {
                    newQuestion.TargetedGender = (Gender)questionModel.TargetedGender;   
                }

                var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);
                newQuestion.Creator = user;
                
                this.Data.Questions.Add(newQuestion);
                this.Data.SaveChanges();
                return RedirectToAction("MyQuestions");
            }

          
            return RedirectToAction("AskQuestion", new RouteValueDictionary(
             new 
             { 
                 controller = "LoggedUser", 
                 action = "AskQuestion", 
                 QuestionId = questionModel.QuestionId, 
                 CategoryId = questionModel.CategoryId,
                 TargetedGender = questionModel.TargetedGender,
                 TargetedMaxAge = questionModel.TargetedMaxAge,
                 TargetedMinAge = questionModel.TargetedMinAge,
                 Text = questionModel.QuestionText,
             }));
        }

        private void ValidateNewQuestiionInput(QuestionModel questionModel)
        {
            if (questionModel.TargetedMinAge != null && questionModel.TargetedMaxAge != null)
            {
                if (questionModel.TargetedMinAge > questionModel.TargetedMaxAge)
                {
                    ModelState.AddModelError("TargetedMinAge", "Can not be lower than Max Age");
                }
            }
        }

        public ActionResult RenderTargetGroupForm()
        {
            var genders = this.PopulateGendersList();

            var list1 = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "---Select gender---", Value = "-1" } };

            list1.AddRange(genders);

            ViewBag.genders = list1;

            var list = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "---Select category---", Value = "-1" } };


            var categories = this.Data.Categories.All().OrderBy(c => c.Name).ToList().Select(c => new SelectListItem() { Value = c.CategoryId.ToString(), Text = c.Name }).ToList();

            list.AddRange(categories);

            ViewBag.Categories = list;

            return PartialView("_TargetGroupForm");
        }

        public ActionResult MarkAnswerAsSpam(int id)
        {
            var answer = this.Data.Answers.All().FirstOrDefault(a => a.AnswerId == id);

            try
            {

            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return Json(answer, JsonRequestBehavior.AllowGet);
        }
	}
}