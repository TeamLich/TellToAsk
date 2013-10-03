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

namespace TellToAsk.Areas.Administration
{
    public class AnswersController : BaseController
    {

        public AnswersController(IUowData data)
            : base(data)
        {
        }
        // GET: /Administration/Aswers/
        public ActionResult Index()
        {
            var answers = this.Data.Answers.All().Select(AnswerModel.FromAnswer);
            return View(answers.ToList());
        }
              
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var answers = this.Data.Answers.All().Select(AnswerModel.FromAnswer);

            return Json(answers.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        
        // GET: /Administration/Aswers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = this.Data.Answers.GetById((int)id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: /Administration/Aswers/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // 
        // Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Answer answer)
        {
            if (ModelState.IsValid)
            {
                this.Data.Answers.Update(answer);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answer);
        }

        // GET: /Administration/Aswers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = this.Data.Answers.GetById((int)id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: /Administration/Aswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.Data.Answers.Delete(id);
            this.Data.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            this.Data.Dispose();
            base.Dispose(disposing);
        }
    }
}
