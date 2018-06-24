using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZhengWei.ExtractOil.WebApp.Controllers
{
    public class HomeController : Controller//BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HomePage()
        {
            return View();
        }

    }
}
