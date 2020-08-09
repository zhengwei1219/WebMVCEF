using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TokenTest.App_Start;

namespace TokenTest.Controllers
{
    public class MangerController : Controller
    {
        // GET: Manger
        [MvcActionAuth]
        public ActionResult Index()
        {
            return View();
        }
    }
}