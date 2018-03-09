using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZZU.JCZD.WebApp.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        filterContextInfo fcinfo;
        // OnActionExecuted 在执行操作方法后由 ASP.NET MVC 框架调用。
        // OnActionExecuting 在执行操作方法之前由 ASP.NET MVC 框架调用。
        // OnResultExecuted 在执行操作结果后由 ASP.NET MVC 框架调用。
        // OnResultExecuting 在执行操作结果之前由 ASP.NET MVC 框架调用。

        /// <summary>
        /// 在执行操作方法之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //得到用户权限表
            if (filterContext.HttpContext.Request.Cookies["allowActions"] != null)
            {
                string allowActionsKey = filterContext.HttpContext.Request.Cookies["allowActions"].Value;
                object obj = Common.MemcheHelper.Get(allowActionsKey);
                if (obj != null)
                {
                    DataTable dt = Common.SerializeHelper.DeserializeToObject<DataTable>(obj.ToString());
                    if (dt == null)
                    {
                        //filterContext.Result = new ContentResult { Content = @"抱歉,您没有登录不能使用该功能。" };
                        //直接跳转到登录页面
                        //filterContext.Result = new RedirectResult(@"/T_User/Login");
                        //filterContext.Result = new ContentResult
                        //{ Content = @"很抱歉,您没有登录不能使用该功能。<a href='/T_User/Login'>进入登录页面</a>" };
                        filterContext.Result = new RedirectResult("/Login/Index");
                    }
                    else
                    {
                        Common.MemcheHelper.Set(allowActionsKey, obj, DateTime.Now.AddDays(1));//模拟出滑动过期时间.
                        fcinfo = new filterContextInfo(filterContext);
                        //得到controler和action的名字
                        //判断用户是否具备该权限
                        /*
                         row["userNmae"] = e.userName;
                                    row["userId"] = Convert.ToInt32(e.userId);
                                    row["userRole"] = e.userRole;
                                    row["userModuleName"] = e.userModuleName;
                                    row["userControl"] = e.userControl;
                                    row["userAction"] = e.userAction;
                                    row["userActionName"] = e.userActionName;
                         */
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


                        //islogin = false;
                        if (isfind)//如果满足
                        {
                            //用户具备权限  不拦截
                        }
                        else
                        {
                            // filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" + fcinfo.actionName + "," + fcinfo.controllerName + "," + fcinfo.domainName + "," + fcinfo.module };// 直接返回 return Content("抱歉,你不具有当前操作的权限！")
                            //filterContext.Result = new ContentResult { Content = @"很抱歉,你不具有当前操作的权限。<a href='/home/index'>返回首页</a>" };
                            filterContext.Result = new RedirectResult("/Login/Index");
                        }
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }




            //fcinfo = new filterContextInfo(filterContext);
            //得到controler和action的名字
            //判断用户是否具备该权限
            /*
             row["userNmae"] = e.userName;
                        row["userId"] = Convert.ToInt32(e.userId);
                        row["userRole"] = e.userRole;
                        row["userModuleName"] = e.userModuleName;
                        row["userControl"] = e.userControl;
                        row["userAction"] = e.userAction;
                        row["userActionName"] = e.userActionName;
             */
            //    bool isfind = false;
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        if (row["controller"].ToString() == fcinfo.controllerName && row["action"].ToString()
            //            == fcinfo.actionName)
            //        {
            //            isfind = true;
            //            break;
            //        }
            //    }


            //    //islogin = false;
            //    if (isfind)//如果满足
            //    {
            //        //用户具备权限  不拦截
            //    }
            //    else
            //    {
            //        // filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" + fcinfo.actionName + "," + fcinfo.controllerName + "," + fcinfo.domainName + "," + fcinfo.module };// 直接返回 return Content("抱歉,你不具有当前操作的权限！")
            //        //filterContext.Result = new ContentResult { Content = @"很抱歉,你不具有当前操作的权限。<a href='/home/index'>返回首页</a>" };
            //        filterContext.Result = new RedirectResult("/Login/LoginIndex");
            //    }
            //}



        }
        /// <summary>
        /// 在执行操作方法后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        ///  OnResultExecuted 在执行操作结果后由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        /// <summary>
        /// OnResultExecuting 在执行操作结果之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
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