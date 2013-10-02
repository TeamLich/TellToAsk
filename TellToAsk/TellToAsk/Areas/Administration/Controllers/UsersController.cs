﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Controllers;
using TellToAsk.Data;
using TellToAsk.Areas.Administration.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net;
using TellToAsk.Model;

namespace TellToAsk.Areas.Administration.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController()
            : this(new UowData())
        {
        }

        public UsersController(IUowData data)
            : base(data)
        {
        }
        //
        // GET: /Administration/Users/
        private static List<SelectListItem> genders = new List<SelectListItem>()
            { 
                new SelectListItem { Text = Gender.Male.ToString(), Value = "0" },
                new SelectListItem { Text = Gender.Female.ToString(), Value = "1" }
            };
        public ActionResult Index()
        {
            var users = this.Data.Users.All().Select(UserModel.FromUsers);
            return View(users.ToList());
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var questions = this.Data.Users.All().Select(UserModel.FromUsers);

            return Json(questions.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Administration/Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Administration/Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = this.Data.Users.All().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Genders = genders;

            return View(user);
        }

        //
        // POST: /Administration/Users/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                this.Data.Users.Update(user);
                this.Data.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Genders = genders;

            return View(user);
        }

        //
        // GET: /Administration/Users/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Administration/Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}