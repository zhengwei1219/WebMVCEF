using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;
using TokenTest.CommonCS;

namespace TokenTest.Models
{
    public class LoginsModel
    {
        private readonly object LOCK = new object();
        [Required(ErrorMessage = "请输入账户号码/手机号")]
        [RegularExpression(@"^1[34578][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入账户密码")]
        [DataType(DataType.Password, ErrorMessage = "密码格式不正确")]
        public string UserPwd { get; set; }

        public bool remember { get; set; }

        public string LoginAction()
        {
            lock (LOCK)
            {
                string userRole = string.Empty;
                //数据库操作代码
                int UserId = 0;
                if (UserName == "18137070152" && UserPwd == "18137070152")
                {
                    UserId = 1;
                    userRole = "HomeCare.Administrator";
                }
                else if (UserName == "18911695087" && UserPwd == "18911695087")
                {
                    UserId = 2;
                    userRole = "HomeCare.Vip";
                }
                else
                {
                    UserId = 3;
                    userRole = "HomeCare.User";
                }
                if (UserId != 0)
                {
                    if (remember)
                    {
                        CommonMethod.setCookie("HX_userName", UserName, 7);
                        CommonMethod.setCookie("HX_userPwd", UserPwd, 7);
                    }
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                           1,
                           UserName + "_" + UserId,
                           DateTime.Now,
                           DateTime.Now.AddMinutes(30),
                           false,
                           userRole,
                           "/"
                           );
                    //.ASPXAUTH
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                    return "HomeCare.Administrator";
                }
                else
                {
                    return "账户密码不存在";
                }
            }
        }
    }
}