using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZZU.JCZD.Model;
using ZZU.JCZD.WebApp.Models;

namespace ZZU.JCZD.WebApp.Controllers
{
    public class BaseController : Controller
    {
        
        // GET: /Base/
        public static LoginUserInfo LoginUser { get; set; }
        public static int compId ;
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            /////////把用户和公司Id传过去
          
           
            bool isSucess = false;
            //////1.通过cookies判断登陆状态,Memche中有登陆用户则通过
            if (Request.Cookies["loginPerson"] != null)
            {
                string loginPerson = Request.Cookies["loginPerson"].Value;
                //根据该值查Memcache.
                object obj = Common.MemcheHelper.Get(loginPerson);
                if (obj != null)
                {
                    LoginUserInfo userInfo = Common.SerializeHelper.DeserializeToObject<LoginUserInfo>(obj.ToString());
                    LoginUser = userInfo;
                    compId = userInfo.CompId;
                    isSucess = true;
                    ViewBag.compId = compId;
                    ViewBag.loginUser = LoginUser;
                    Common.MemcheHelper.Set(loginPerson, obj, DateTime.Now.AddDays(1));//模拟出滑动过期时间.
                }
            }


            ////2.通过Memche中的权限列表判断是否有权限
            if (filterContext.HttpContext.Request.Cookies["allowActions"] != null)
            {
                string allowActionsKey = filterContext.HttpContext.Request.Cookies["allowActions"].Value;
                object obj = Common.MemcheHelper.Get(allowActionsKey);
                if (obj != null)
                {
                    DataTable dt = Common.SerializeHelper.DeserializeToObject<DataTable>(obj.ToString());
                    if (dt == null)//没有任何权限
                    {
                        isSucess = false;
                    }
                    else
                    {
                        ViewBag.allactionList = dt;
                        Common.MemcheHelper.Set(allowActionsKey, obj, DateTime.Now.AddDays(1));//模拟出滑动过期时间.
                        filterContextInfo fcinfo = new filterContextInfo(filterContext);
                        //通过controller和action判断是否角色有该权限，有的话isfind为true
                        bool isfind = false;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["controller"].ToString() == fcinfo.controllerName && row["action"].ToString()
                                == fcinfo.actionName)
                            {
                                isfind = true;
                                break;
                            }
                        }
                        if (isfind)//如果满足
                        {
                            isSucess = true;
                            //用户具备权限  不拦截
                        }
                        else
                        {
                            isSucess = false;
                        }
                    }
                }
            }
            else//dt为null
            {
                isSucess = false;
            }
                        
            if (!isSucess)
            {
                filterContext.Result = Redirect("/Login/Index");//注意.
            }
        }

    }
      public class filterContextInfo
    {
        public filterContextInfo(ActionExecutingContext filterContext)
        {
            #region 获取链接中的字符
            // 获取域名
            domainName = filterContext.HttpContext.Request.Url.Authority;

            //获取模块名称
            //  module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();

            //获取 controllerName 名称
            controllerName = filterContext.RouteData.Values["controller"].ToString();

            //获取ACTION 名称
            actionName = filterContext.RouteData.Values["action"].ToString();

            #endregion
        }
        /// <summary>
        /// 获取域名
        /// </summary>
        public string domainName { get; set; }
        /// <summary>
        /// 获取模块名称
        /// </summary>
        public string module { get; set; }
        /// <summary>
        /// 获取 controllerName 名称
        /// </summary>
        public string controllerName { get; set; }
        /// <summary>
        /// 获取ACTION 名称
        /// </summary>
        public string actionName { get; set; }

    }
}

