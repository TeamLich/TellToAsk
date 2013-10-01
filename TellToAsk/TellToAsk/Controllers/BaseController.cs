using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TellToAsk.Data;

namespace TellToAsk.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
            : this(new UowData())
        {
        }

        public BaseController(IUowData data)
        {
            this.Data = data;
        }

        public IUowData Data { get; set; }
	}
}