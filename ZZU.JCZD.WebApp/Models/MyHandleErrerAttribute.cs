using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZZU.JCZD.WebApp.Models
{
    public class MyHandleErrerAttribute:HandleErrorAttribute
    {
        //静态的，所有人出现异常都会用
        public static Queue<Exception> errorQueue = new Queue<Exception>();

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;

            errorQueue.Enqueue(ex);
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}