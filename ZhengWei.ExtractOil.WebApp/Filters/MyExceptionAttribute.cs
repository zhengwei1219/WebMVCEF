using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZhengWei.ExtractOil.WebApp
{
    public class MyExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null)
            {
                ExceptionQueue.Enqueue(filterContext.Exception);
                filterContext.HttpContext.Response.Redirect("/ErrorPage.html");
            }
           // base.OnException(filterContext);
        }

        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();
    }
}