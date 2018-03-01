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
    public class RoleInfoController : Controller
    {
        //
        // GET: /RoleInfo/
        IRoleInfoService roleInfoService = new RoleInfoService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRoleInfoList()
        {
            int pageIndex = int.Parse(Request["page"]);
            int pageSize = int.Parse(Request["rows"]);
            int totalCount;
            //short delFlag = (short)DeleteEnumType.Normal;
           var roleList =  roleInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, s => s.DelFlag == "false",s=>s.ID,true);
           var rows =from r in roleList select new { ID = r.ID, RoleName = r.RoleName, Sort = r.Sort, Remark = r.Remark, SubTime = r.SubTime };
           return Json(new { rows=rows,totalCount = totalCount},JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddRoleInfo(RoleInfo roleInfo)
        {
            roleInfo.ModifiedOn = DateTime.Now;
            roleInfo.SubTime = DateTime.Now;
            roleInfo.DelFlag = "false";
            roleInfoService.AddEntity(roleInfo);
            return Content("ok");
        }

        public ActionResult DeleteRoleInfo()
        {
            string strId = Request["strId"];
            string[] strIds = strId.Split(',');
            List<int> intIds = new List<int>();
            foreach (var item in strIds)
            {
                intIds.Add(int.Parse(item));
            }
            roleInfoService.DeleteEntitys(intIds);
            return Content("ok");
        }

    }
}
