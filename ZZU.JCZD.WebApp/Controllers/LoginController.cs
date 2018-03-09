using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZZU.JCZD.BLL;
using ZZU.JCZD.Common;
using ZZU.JCZD.Model;
using ZZU.JCZD.WebApp.Models;

namespace ZZU.JCZD.WebApp.Controllers
{
    public class LoginController : Controller
    {
        JCZDEntities db = DBContextBll.GetDbContext();
        UserInfoService userBll = new UserInfoService();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ajax登陆
        /// </summary>
        /// <returns></returns>
        public ContentResult LoginIndex(string username, string userpass)
        {
            //string username = Request.Form["username"];
            //string userpass = Request.Form["userpass"];           
            IQueryable<UserInfoSet> users = userBll.LoadEntities(u => (u.UserName == username && u.UserPass == userpass));

            if (users.Count() > 0)
            {
                LoginUserInfo loginUser = (from user in users
                                           join comp in db.Company on user.CompId equals comp.Id
                                           join role in db.Role on user.RoleId equals role.Id
                                           select new LoginUserInfo
                                           {
                                               UserInfoId = user.Id,
                                               UserName = user.UserName,
                                               RoleId = user.RoleId,
                                               RoleName = role.RoleName,
                                               CompId = user.CompId,
                                               CompName = comp.CompName,
                                           }).FirstOrDefault<LoginUserInfo>();
                
                //把用户存在memcache中    
                int roleId=loginUser.RoleId;
                 PubDal pd = new PubDal();
                DataTable allowActionList=pd.GetActionsByRole(roleId);
                string loginPerson = Guid.NewGuid().ToString();
                string allowActions = Guid.NewGuid().ToString();

                MemcheHelper.Set(loginPerson, Common.SerializeHelper.SerializeToString(loginUser)
                    , DateTime.Now.AddDays(1));//将登录用户信息存储到Memcache中。
                MemcheHelper.Set(allowActions, Common.SerializeHelper.SerializeToString(allowActionList), DateTime.Now.AddDays(1));
                Response.Cookies["loginPerson"].Value = loginPerson;//将Memcache的key(登陆用户)以Cookie的形式返回给浏览器。
                Response.Cookies["allowActions"].Value = allowActions;//将Memcache的key（权限列表）以Cookie的形式返回给浏览器。
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        public ActionResult LoginOut()
        {
            Response.Cookies["loginPerson"].Value = null;
            Response.Cookies["allowActions"].Value = null;
            return RedirectToAction("Index");
        }
    }
}

