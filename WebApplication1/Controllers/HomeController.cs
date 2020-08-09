using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TokenTest.CommonCS;
using TokenTest.Models;

namespace TokenTest.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Login()
        {
            ClearLogin();
            string HX_userName = CommonMethod.getCookie("HX_userName");
            string HX_userPwd = CommonMethod.getCookie("HX_userPwd");
            ViewBag.HX_userName = HX_userName;
            ViewBag.HX_userPwd = HX_userPwd;
            return View();
        }

        [HttpPost]
        public object UserLogin(LoginsModel LoginMol)
        {
            if (ModelState.IsValid)//是否通过Model验证
            {
                return LoginMol.LoginAction();
            }
            else
            {
                return GetError();
            }
        }
    }
}