using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZhengWei.ExtractOil.BLL;
using ZhengWei.ExtractOil.IBLL;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.WebApp.Controllers
{
    public class ReplyController : Controller
    {
        //
        // GET: /Reply/
        ITopicsService topicsService = new TopicsService();
        IReplysService replysService = new ReplysService();
        public ActionResult Index()
        {
            return View();
        }

        
        

    }
}
