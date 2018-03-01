using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ZhengWei.ExtractOil.WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //初始化log4net
            log4net.Config.XmlConfigurator.Configure();
            //扫描异常队列，写入日志
            GetExceptionInfo();
        }

        private void GetExceptionInfo()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(a => { 
               while(true)
               {
                   if (MyExceptionAttribute.ExceptionQueue.Count > 0)
                   {
                       Exception ex = MyExceptionAttribute.ExceptionQueue.Dequeue();
                       if (ex != null)
                       {
                           //写入日志
                           LogService.ErrorLog(ex.Message);
                       }
                       else
                       {
                           System.Threading.Thread.Sleep(9000);
                       }
                   }
                   else {
                       System.Threading.Thread.Sleep(9000);
                   }
               }
            });
        }
    }
}