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
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUowData data)
            : base(data)
        {
        }
        // GET: /Administration/Categories/
        public ActionResult Index()
        {
            return View(this.Data.Categories.All().Select(CategoryModel.FromCategory).ToList());
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var categories = this.Data.Categories.All().Select(CategoryModel.FromCategory);

            return Json(categories.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // GET: /Administration/Categories/Create
        public ActionResult Create()
        {
            var list = this.PopulateAgeRatings();
            ViewBag.AgeRatings = list;
            return View();
        }

        // POST: /Administration/Categories/Create
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                this.Data.Categories.Add(category);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: /Administration/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = this.Data.Categories.GetById((int)id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var list = this.PopulateAgeRatings();
            ViewBag.AgeRatings = list;

            return View(category);
        }

        // POST: /Administration/Categories/Edit/5
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                this.Data.Categories.Update(category);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: /Administration/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = this.Data.Categories.GetById((int)id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: /Administration/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.Data.Categories.Delete(id);
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
