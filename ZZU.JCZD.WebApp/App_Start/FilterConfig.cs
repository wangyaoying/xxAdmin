using System.Web;
using System.Web.Mvc;
using ZZU.JCZD.WebApp.Models;

namespace ZZU.JCZD.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MyHandleErrerAttribute());
        }
    }
}