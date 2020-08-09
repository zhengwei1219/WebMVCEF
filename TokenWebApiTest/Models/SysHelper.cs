using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace TokenWebApiTest.Models
{
    public class SysHelper
    {
        public static UserPrincipal CurrentPrincipal
        {
            get
            {
                return HttpContext.Current.User as UserPrincipal;
            }
        }
    }

    public class UserPrincipal : ClientUserData, IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public string[] Roles { get; set; }

        public UserPrincipal(ClientUserData clientUserData)
        {
            this.Identity = new GenericIdentity(string.Format("{0}", clientUserData.UserId));
            this.UserId = clientUserData.UserId;
            this.UserName = clientUserData.UserName;
            this.UserPwd = clientUserData.UserPwd;
            this.UserRole = clientUserData.UserRole;
        }

        public bool IsInRole(string role)
        {
            if (Roles.Any(r => r == role))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ClientUserData
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string UserRole { get; set; }
    }
}