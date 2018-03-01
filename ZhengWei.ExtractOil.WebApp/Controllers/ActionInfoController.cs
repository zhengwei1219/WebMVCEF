using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhengWei.ExtractOil.BLL;
using ZhengWei.ExtractOil.IBLL;

namespace ZhengWei.ExtractOil.WebApp.Controllers
{
    public class ActionInfoController : Controller
    {
        IActionInfoService actionInfoService = new ActionInfoService();
        //
        // GET: /ActionInfo/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetActionInfoList()
        {
            int pageIndex = int.Parse(Request["page"]);
            int pageSize = int.Parse(Request["rows"]);
            int totalCount = 0;
            var ActionInfoList = actionInfoService.LoadPageEntities(pageIndex, pageSize,out totalCount, s => s.DelFlag == "False",a=>a.ActionInfoName,true).ToList();
            var rows = (from a in ActionInfoList select new { ID = a.ID, ActionInfoName = a.ActionInfoName, Remark = a.Remark, Url = a.Url, HttpMethod = a.HttpMethod, SubTime=a.SubTime }).ToList();

            return Json(new { rows=rows,totalCount=totalCount},JsonRequestBehavior.AllowGet);
        }

    }
}
