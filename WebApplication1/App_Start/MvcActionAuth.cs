using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TokenTest.App_Start
{
    public class MvcActionAuth : AuthorizeAttribute
    {
        public string[] AuthorizeRoleAry = new string[] { "HomeCare.User", "HomeCare.Vip", "HomeCare.Administrator" };//本系统允许的角色 普通用户 会员 超级管理员
        /// <summary>
        /// 自定义 MVC 控制前访问权限验证
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string Role = string.Empty;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            //数据库验证当前Controller及Action允许访问的权限
            //简单模拟 读取数据库
            if (controllerName == "Manger" && actionName == "Index")
            {
                //得到允许访问 Manger/Index 的权限值
                Role = "HomeCare.Administrator,HomeCare.Vip";
            }

            //
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
            string[] roles = authTicket.UserData.Split(',');
            string NowRole = string.Empty;
            foreach (var item in roles)
            {
                NowRole += item;
            }
            if (!Role.Contains(NowRole)) //没有权限访问当前控制器 ACtion
            {
                System.Web.HttpContext.Current.Response.Redirect("/Home/Login");
            }
            base.OnAuthorization(filterContext);
        }
    }
}