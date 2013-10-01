using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TellToAsk.Areas.LoggedUser.Controllers
{
    public class LoggedUserController : Controller
    {
        //
        // GET: /LoggedUser/LoggedUser/
        public ActionResult Index()
        {
            return View();
        }
	}
}