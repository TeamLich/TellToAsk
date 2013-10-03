using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Model;
using TellToAsk.Data;
using TellToAsk.Controllers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using TellToAsk.Areas.Administration.Models;

namespace TellToAsk.Areas.Administration.Controllers
{
    public class QuestionsController : BaseController
    {

       public QuestionsController(IUowData data)
            : base(data)
        {
        }

        // GET: /Administration/Questions/
        public ActionResult Index()
        {
            var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);
            return View(questions.ToList());
        }

        // GET: /Administration/Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = this.Data.Questions.GetById((int)id);
            if (question == null)
            {
                return HttpNotFound();
            }
           
            return View(question);
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var questions = this.Data.Questions.All().Select(QuestionModel.FromQuestion);

            return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // GET: /Administration/Questions/Create
        public ActionResult Create()
        {
            ViewBag.Categories = this.Data.Categories.All().ToList()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
            return View();
        }

        // POST: /Administration/Questions/Create
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            

            var user = this.Data.Users.All().FirstOrDefault( u => u.UserName == this.User.Identity.Name);

            question.Creator = user;

            if (ModelState.IsValid)
            {
                this.Data.Questions.Add(question);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = this.Data.Categories.All().ToList()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
            return View(question);
        }

        // GET: /Administration/Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = this.Data.Questions.GetById((int)id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = this.Data.Categories.All()
                .ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
            return View(question);
        }

        // POST: /Administration/Questions/Edit/5
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                this.Data.Questions.Update(question);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = this.Data.Categories.All()
               .ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() });
            return View(question);
        }

        // GET: /Administration/Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = this.Data.Questions.GetById((int)id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Administration/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {          
            this.Data.Questions.Delete(id);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }
        // comment
        protected override void Dispose(bool disposing)
        {
            this.Data.Dispose();
            base.Dispose(disposing);
        }
    }
}
