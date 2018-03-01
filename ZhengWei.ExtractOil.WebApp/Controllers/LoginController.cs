using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhengWei.ExtractOil.BLL;

namespace ZhengWei.ExtractOil.WebApp.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        IBLL.IUserInfoService userInfoService = new UserInfoService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserLogin()
        {

            string validateCode = Session["validateCode"] == null ? string.Empty : Session["validateCode"].ToString();
            if (string.IsNullOrEmpty(validateCode))
            {
                return Content("验证码错误！");
            }
            string requestCode = Request["vCode"];
            if (!requestCode.Equals(validateCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Content("验证码错误！");
            }
            string userName = Request["LoginCode"];
            string passWord = Request["LoginPwd"];
            var userInfo = userInfoService.LoadEntities(u=>u.Name==userName && u.Pwd== passWord).FirstOrDefault();
            if (userInfo == null)
            {
                return Content("用户名密码错误！");
            }
            else
            {
                Session["userInfo"] = userInfo;
                return Content("ok");
            }
        }
        //展示验证码
        public ActionResult ShowValidateCode()
        {
            Common.ValidateCode validatteCode = new Common.ValidateCode();
            string code = validatteCode.CreateValidateCode(4);
            Session["validateCode"] = code;
            byte[] buffer =validatteCode.CreateValidateGraphic(code);
            return File(buffer,"image/jpg");
        }

    }
}
