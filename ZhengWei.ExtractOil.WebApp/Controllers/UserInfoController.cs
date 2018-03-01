using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhengWei.ExtractOil.BLL;
using ZhengWei.ExtractOil.IBLL;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.WebApp.Controllers
{
    public class UserInfoController : BaseController//Controller
    {
        //
        // GET: /UserInfo/
        IUserInfoService userInfoService = new UserInfoService();
        IRoleInfoService roleInfoService = new RoleInfoService();
        IActionInfoService actionInfoService = new ActionInfoService();
        public ActionResult Index()
        {
            LogService.OperationLog("操作用户信息");
            return View();
        }
        //获取用户信息
        public ActionResult GetUserInfoList()
        {
            int pageIndex = int.Parse(Request["page"]);
            int pageSize = int.Parse(Request["rows"]);
            int totalCount=0;
            string name = Request["name"];
            string remark = Request["remark"];
            UserInfoSearch userInfoSearch = new UserInfoSearch() {PageIndex=pageIndex,PageSize = pageSize,Name=name,Remark=remark,TotalCount=totalCount};
            var userInfoList = userInfoService.LoadSearchPageEntities(userInfoSearch);
            var temp = from a in userInfoList select new {ID = a.ID,UName = a.Name,UPwd=a.Pwd,Remark = a.Remark,SubTime = a.SubTime };
            return Json(new { rows = temp, total = userInfoSearch.TotalCount },JsonRequestBehavior.AllowGet);          
        }
        //显示用户信息
        public ActionResult ShowEditInfo()
        { 
         int id = int.Parse(Request["id"]);
         var temp = (from a in userInfoService.LoadEntities(u => u.ID == id) select new { UName = a.Name, UPwd = a.Pwd, Remark = a.Remark, Sort = a.Sort, SubTime = a.SubTime, DelFlag = a.DelFlag, ID=a.ID }).OrderBy(S=>S.Sort).FirstOrDefault();
         return Json(temp, JsonRequestBehavior.AllowGet);
        }
        //编辑用户信息
        public ActionResult EditUserInfo()
        {
            string name = Request["UName"];
            string password = Request["UPwd"];
            string remark = Request["Remark"];
            string sort = Request["Sort"];
            int id = int.Parse(Request["ID"]);
            UserInfo user = userInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            user.Name = name;
            user.Pwd = password;
            user.Remark = remark;
            user.Sort = sort;
            user.SubTime = DateTime.Now.ToShortDateString();

            return userInfoService.UpdateEntity(user) ? Content("ok") : Content("no");
        }
        //增加用户信息
        public ActionResult AddUserInfo() 
        {           
            string name = Request["UName"];
            string password = Request["UPwd"];
            string remark = Request["Remark"];
            string sort = Request["Sort"];
            UserInfo u = new UserInfo() { Name = name, Pwd = password,Remark= remark,Sort=sort,SubTime= DateTime.Now.ToShortDateString() };
            return userInfoService.AddEntity(u) != null?Content("ok"):Content("no");
        }
        //删除用户信息
        public ActionResult DeleteUserInfo()
        {
            int id = int.Parse(Request["strId"]);
            UserInfo u = userInfoService.LoadEntities(s=>s.ID == id).FirstOrDefault();
            if(u != null)
            {
                if (userInfoService.DeleteEntity(u))
                    return Content("ok");
                else
                    return Content("no");
            }else
            {
                return Content("no");
            }
            
        }
        //显示用户角色信息
        public ActionResult ShowUserRoleInfo()
        {
            //1、获取当前用户信息和存在的角色信息
           int userId = int.Parse(Request["id"]);
           UserInfo userInfo = userInfoService.LoadEntities(s=>s.ID == userId).FirstOrDefault();
           ViewBag.UserInfo = userInfo;
           ViewBag.ExitUserRole = userInfo.RoleInfo.ToList();

            //2、获取所有的角色信息          
           List<RoleInfo> allRole = roleInfoService.LoadEntities(s=>s.DelFlag=="False").ToList();
           ViewBag.AllRole = allRole;
           return View();
        }
        //保存用户角色信息
        public ActionResult SaveUserRoleInfo()
        {
            int userId = int.Parse(Request["userId"]);
            UserInfo userInfo = userInfoService.LoadEntities(s=>s.ID==userId).FirstOrDefault();
            string[] Ids = Request.Form.AllKeys;
            List<int> roleIds = new List<int>();
            for (int i = 0; i < Ids.Length; i++)
            {
                if (Ids[i].StartsWith("abc"))
                {                    
                    roleIds.Add(int.Parse(Ids[i].Replace("abc","")));
                }
            }
            bool success =userInfoService.SaveUserRoleInfo(userInfo, roleIds);
            if (success)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        //展示要为用户分配的权限
        public ActionResult ShowUserActionInfo()
        {
            return View();
        }


    }
}
