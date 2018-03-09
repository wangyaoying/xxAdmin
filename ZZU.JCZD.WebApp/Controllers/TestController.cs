using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZZU.JCZD.WebApp.Models;

namespace ZZU.JCZD.WebApp.Controllers
{
    //[MyHandleErrerAttribute]
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Show()
        {
            int a = 5;
            int b = 0;
            int c = a / b;
            return Content(c.ToString());
        }

    }
}
