using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZZU.JCZD.BLL;
using ZZU.JCZD.Common;
using ZZU.JCZD.Model;

namespace ZZU.JCZD.WebApp.Controllers
{
    public class UserInfoController : BaseController// : BaseController//
    {
        //
        // GET: /UserInfo/
        UserInfoService userBll = new BLL.UserInfoService();
        JCZDEntities db = DBContextBll.GetDbContext();

        public ActionResult Index()
        {
            //从memcache中取出用户名
            //ViewBag.loginUser=MemcheHelper.Get("loginuser");
            ///////////////////这里是修改的部分///////////////////////ViewBag.compId = compId;           
            List<Role> roleList = (from r in db.Role where r.IsAdmin == false select r).ToList();
            ViewBag.CompList = (from c in db.Company select c).ToList();
            ViewBag.RoleList = roleList;
            return View();
        }

        #region 展示用户
        public JsonResult GetUserInfo()
        {
            //查询用户信息和公司名、权限名
            IQueryable<UserInfoSet> list = userBll.LoadEntities(user => user.DelFlag == false && user.CompId == compId );
            
            //IQueryable<UserInfoSet> list = userBll.LoadEntities(user => user.DelFlag == false);

            var list2 = from user in list
                        join comp in db.Company on user.CompId equals comp.Id
                        join role in db.Role on user.RoleId equals role.Id
                        where role.IsAdmin==false
                        select new
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            RealName = user.RealName,
                            Phone = user.Phone,
                            RoleId = user.RoleId,
                            Role = role.RoleName,
                            CompId = user.CompId,
                            Comp = comp.CompName,
                            Detail = user.Detail,
                            UserPass = user.UserPass
                        };



            return Json(list2.ToArray(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 添加用户
        public ContentResult AddUser()
        {
            string name = Request.Form["name"];
            string pass = Request.Form["pass"];
            string detail = (Request.Form["detail"] == "" ? "暂无描述" : Request.Form["detail"]);
            string phone = Request.Form["phone"];
            int comid = int.Parse(Request.Form["compid"]);
            int roleid = int.Parse(Request.Form["roleid"]);
            string image = "";//暂时的
            string realName = Request.Form["realname"];
            if (db.UserInfoSet.Where<UserInfoSet>(u => (u.UserName == name)).FirstOrDefault() != null)
            {
                return Content("ex");
            }
            UserInfoSet user = new UserInfoSet()
            {
                UserName = name,
                UserPass = pass,
                Detail = detail,
                Phone = phone,
                CompId = comid,
                RoleId = roleid,
                Image = image,
                RealName = realName,
                SubTime = DateTime.Now,
                ModifTime = DateTime.Now,
                DelFlag = false

            };
            if (userBll.AddEntity(user) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 修改用户
        public ContentResult EditUser(string roleid, string username, string realname, string phone, string detail, string id, string userpass)
        {
            int uid = int.Parse(id);
            UserInfoSet user = userBll.LoadEntities(u => u.Id == uid).FirstOrDefault<UserInfoSet>();
            user.UserName = username;
            user.RealName = realname;
            user.Phone = phone;
            user.Detail = detail;
            user.RoleId = int.Parse(roleid);
            if (!userpass.Equals("在这里输入修改密码"))
            {
                user.UserPass = userpass;
            }
            if (userBll.EditEntity(user) == true)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }


        }
        #endregion
        #region 逻辑删除用户
        public ContentResult DeleteUsers(string ids)
        {
            string[] idArr = ids.Split(',');
            int id;
            UserInfoSet user;
            for (int i = 0; i < idArr.Length; i++)
            {
                id = int.Parse(idArr[i]);
                user = db.UserInfoSet.Where<UserInfoSet>(u => u.Id == id).FirstOrDefault<UserInfoSet>();
                user.DelFlag = true;
            }
            if (db.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
            /////////////////////////

        }
        #endregion
    }
}
