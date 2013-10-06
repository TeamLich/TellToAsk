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
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;

namespace TellToAsk.Areas.LoggedUser.Controllers
{
    //[Authorize(Roles="User")]
    [Authorize]
    public class LoggedUserController : BaseController
    {
        private const int PointsForAnswer = 10;
        private const int PointsForUsefullAnswer = 5;
        private const int PointsForUselessAnswer = -5;

          public LoggedUserController(IUowData data)
            : base(data)
            {
            }

        public ActionResult MyQuestions()
        {
            return View();
        }

        public JsonResult GetMyQuestionsSimple([DataSourceRequest]DataSourceRequest request)
        {
            var userName = this.User.Identity.Name;

            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);

            var questions =
                this.Data.Questions.All()
                .Where(u => u.Creator.Id == user.Id).OrderBy(q => q.Category.Questions.Count)
                .Select(QuestionModel.FromQuestion);
            return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // not in use
        public JsonResult GetMyQuestions([DataSourceRequest]DataSourceRequest request, int? id)
        {
            var userName = this.User.Identity.Name;

            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == userName);

            if (id != null)
            {

                var questions =
                    this.Data.Questions.All()
                    .Where(q => q.Creator.Id == user.Id && q.CategoryId == id).Select(QuestionModel.FromQuestion);

                return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var categories = user.Categories.OrderBy(c => c.Name).AsQueryable().Select(CategoryModel.FromCategory);
                return Json(categories.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }


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

            // just in case of evil hacker
            var currUserId = this.User.Identity.GetUserId();
            if (currUserId != question.Creator.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (question != null)
            {

                var model = new QuestionModel()
                {
                    QuestionId = question.QuestionId,
                    QuestionText = question.Text,
                    QuestionTitle = question.Title,
                    CategoryId = question.CategoryId,
                    TargetedGender = question.TargetedGender != null ? (int)question.TargetedGender : -1,
                    TargetedGenderValue = question.TargetedGender != null ? question.TargetedGender.ToString() : null,
                    TargetedMaxAge = question.TargetedMaxAge,
                    TargetedMinAge = question.TargetedMinAge,
                };

                if (question.TargetedGender != null)
                {
                    model.TargetedGender = (int)question.TargetedGender; 
                }

               return View(model);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public JsonResult GetQuestionAnswers([DataSourceRequest]DataSourceRequest request, int? id)
        {
            //// just in case of evil hacker
            //var question = this.Data.Questions.All().FirstOrDefault(q => q.QuestionId == id);

            //var currUserId = this.User.Identity.GetUserId();
            //if (currUserId != question.Creator.Id)
            //{
            //    return null;
            //}

            var answers = this.Data.Answers.All()
                .Where(q => q.Question.QuestionId == id && q.IsReported == false)
                .Select(AnswerModel.FromAnswer);
            
            return Json(answers.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetQuestionById(int id)
        {
            //// just in case of evil hacker
            //var questionFull = this.Data.Questions.All().FirstOrDefault(q => q.QuestionId == id);
            //var currUserId = this.User.Identity.GetUserId();
            //if (currUserId != questionFull.Creator.Id)
            //{
            //    return null;
            //}

            var question = this.Data.Questions.All().Select(QuestionModel.FromQuestion).FirstOrDefault(q => q.QuestionId == id);
            return Json(question, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAnswerById(int id)
        {
            var question = this.Data.Answers.All().Select(AnswerModel.FromAnswer).FirstOrDefault(q => q.AnswerId == id);
            return Json(question, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TakeQuestion()
        {
            // needed for check id in js
            return View(new AnswerModel());
        }

        [ValidateAntiForgeryToken]
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
       
        public ActionResult AskQuestion()
        {
            this.LoadDropDownModelsInViewBag();

            return View();
        }

        private void LoadDropDownModelsInViewBag()
        {
            var list = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "---Select category---", Value = "-1" } };


            var categories = this.PopulateSuitableCategories().Select(c => new SelectListItem() { Value = c.CategoryId.ToString(), Text = c.Name }).ToList();

            list.AddRange(categories);

            ViewBag.Categories = list;


            var genders = this.PopulateGendersList();

            var listGenders = new List<SelectListItem>() { new SelectListItem() { Selected = true, Text = "---Select gender---", Value = "-1" } };

            listGenders.AddRange(genders);

            ViewBag.genders = listGenders;
        }

        [ValidateAntiForgeryToken]
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
            this.LoadDropDownModelsInViewBag();

                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(questionModel);


                ViewBag.JsonModel = json;
            return View("AskQuestion", questionModel);
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
            var currenUserId = this.User.Identity.GetUserId();
            var answer = this.Data.Answers.All().FirstOrDefault(a => a.AnswerId == id);

            // check just in case of evil hacker
            if (DoesUserHavePermissionsToVote(currenUserId, answer))
            {
                answer.IsReported = true;
                this.Data.SaveChanges();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        public ActionResult VoteForAnswer(int id, bool isPositiveVote)
        {
            var currenUserId = this.User.Identity.GetUserId();
            var answer = this.Data.Answers.All().FirstOrDefault(a => a.AnswerId == id);

            // check just in case of evil hacker
            if (DoesUserHavePermissionsToVote(currenUserId, answer))
            {
                answer.IsVoted = true;

                int points;

                if (isPositiveVote)
                {
                    points = PointsForUsefullAnswer;
                }
                else
                {
                    points = PointsForUselessAnswer;
                }
                answer.User.Points += points;
                this.Data.SaveChanges();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        private static bool DoesUserHavePermissionsToVote(string currenUserId, Answer answer)
        {
            var id = answer.Question.Creator.Id;
            return currenUserId == id;
        }
	}
}