using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TokenTest.Controllers
{
    public class BaseController : Controller
    {
        #region 退出登录
        /// <summary>
        /// 退出登录
        /// </summary>
        public void ClearLogin()
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                   1,
                   "",
                   DateTime.Now,
                   DateTime.Now.AddMinutes(-30),
                   false,
                   "",
                   "/"
                   );
            //.ASPXAUTH
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

        }
        #endregion

        #region 自定义过滤器
        /// <summary>
        /// 自定义过滤器
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[cookieName];
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                return;
            }
            if (authTicket != null && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string UserName = authTicket.Name;

                base.OnActionExecuting(filterContext);
            }
            else
            {
                Content("<script >top.location.href='/Home/Login';</script >", "text/html");
                //filterContext.HttpContext.Response.Redirect("/Home/Logins");
            }
        }
        #endregion

        #region 读取错误信息
        /// <summary>
        /// 读取错误信息
        /// </summary>
        /// <returns></returns>
        public string GetError()
        {
            var errors = ModelState.Values;
            foreach (var item in errors)
            {
                foreach (var item2 in item.Errors)
                {
                    if (!string.IsNullOrEmpty(item2.ErrorMessage))
                    {
                        return item2.ErrorMessage;
                    }
                }
            }
            return "";
        }
        #endregion

    }
}