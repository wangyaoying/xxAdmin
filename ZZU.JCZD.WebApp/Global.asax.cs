using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZZU.JCZD.WebApp.Models;

namespace ZZU.JCZD.WebApp
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();//读取配置文件的log4net信息
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region 记录异常信息
            //开启一个线程记录异常
            string filePath = Server.MapPath("/log/");
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    //Model中定义的MyHandleErrerAttribute特性
                    if (MyHandleErrerAttribute.errorQueue.Count > 0)
                    {
                        Exception ex = MyHandleErrerAttribute.errorQueue.Dequeue();
                        if (ex != null)
                        {
                            //string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                            //File.AppendAllText(filePath + fileName+".txt", ex.ToString(), System.Text.Encoding.UTF8);
                            //ex写到日志文件中
                            ILog logger = LogManager.GetLogger("errorMsg");
                            logger.Error(ex.ToString());
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
            },filePath); 
            #endregion


        }
    }
}