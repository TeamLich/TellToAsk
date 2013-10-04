using System;
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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TellToAsk.Areas.Administration.Controllers
{
    [Authorize(Roles = "User")]
    public class UsersController : BaseController
    {
         
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
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = this.Data.Users.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
         
            return View(user);
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

        public ActionResult AddRole(string id, string roleId)
        {
            ApplicationUser user = this.Data.Users.All().FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var roleExisting = user.Roles.FirstOrDefault(x => x.RoleId == roleId);
            if (roleExisting == null)
            {
                var role = this.Data.Roles.All().FirstOrDefault(r => r.Id == roleId);
                var userRole = new UserRole()
                {
                    RoleId = roleId,
                    UserId = user.Id,
                    User = user,
                    Role = role
                };
                user.Roles.Add(userRole);
                this.Data.SaveChanges();
            }

            return PartialView("_Roles", user);
        }

        public ActionResult EditRoles(string id)
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

            ViewBag.Roles = this.Data.Roles.All().ToList()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id });
            return PartialView("_EditRoles", user);
        }

        public ActionResult RemoveRole(string id, string roleId)
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
            var role = user.Roles.FirstOrDefault(x => x.RoleId == roleId);
            if (role != null)
            {
                user.Roles.Remove(role);
                this.Data.SaveChanges();
            }

            return PartialView("_Roles", user);
        }
        
        public ActionResult BanUser(string id)
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
            user.Management.DisableSignIn = true;
            this.Data.SaveChanges();
            return View();
        }

        public ActionResult UnbanUser(string id)
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
            user.Management.DisableSignIn = false;
            this.Data.SaveChanges();
            return View();
        }
    }
}
