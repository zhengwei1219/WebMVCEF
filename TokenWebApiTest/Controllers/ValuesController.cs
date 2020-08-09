using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TokenWebApiTest.Models;
using TokenWebApiTest.App_Start;

namespace TokenWebApiTest.Controllers
{
    public class MangerApiController : ApiController
    {
        [ApiActionAuth]
        [HttpGet]
        public string GetStr()
        {
            int UserId = SysHelper.CurrentPrincipal.UserId;
            string UserName = SysHelper.CurrentPrincipal.UserName;
            string UserPwd = SysHelper.CurrentPrincipal.UserPwd;
            string UserRole = SysHelper.CurrentPrincipal.UserRole;
            return "当前登录的第三方用户信息如下,UserId:" + UserId + ",UserName:" + UserName + ",UserPwd:" + UserPwd + ",UserRole:" + UserRole;
        }
    }
}
