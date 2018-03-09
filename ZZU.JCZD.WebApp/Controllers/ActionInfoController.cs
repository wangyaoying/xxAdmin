using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZZU.JCZD.BLL;
using ZZU.JCZD.Model;
using ZZU.JCZD.WebApp.Models;

namespace ZZU.JCZD.WebApp.Controllers
{
    public class ActionInfoController : BaseController
    {
        //从父类中获取loginUser
        LoginUserInfo loginUser = LoginUser;
        int roleId = LoginUser.RoleId;
        JCZDEntities db = DBContextBll.GetDbContext();
        // GET: Permission
        public ActionResult Index(string Id)
        {
            //string Id = "2";
            //只用网站管理员和公司管理员才能访问权限页面
            int rId;
            if (int.TryParse(Id, out rId))
            {
                bool isAdmin = db.Role.Where<Role>(r => r.Id == roleId).FirstOrDefault<Role>().IsAdmin;
                if (loginUser == null || !isAdmin)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            List<ActionInfo> actionInfoList = new List<ActionInfo>();
            //获取所有的权限           
            actionInfoList = db.ActionInfo.ToList();//-----(网址管理员获取所有的action)

            //获取当前角色的权限
            List<ActionInfo> roleActionInfoList = (from ra in db.Role_ActionInfo
                                                   where ra.Role_Id == rId
                                                   join a in db.ActionInfo on ra.ActionInfo_Id equals a.Id
                                                   select a).ToList();


            ViewBag.actionInfoList = actionInfoList;
            ViewBag.roleActionList = roleActionInfoList;

            //生成父模块表
            List<ActionInfo> parActionInfoList = actionInfoList.Where(a => a.ControllerName == "1").ToList();
            ViewBag.parActionInfoList = parActionInfoList;
            List<Role_ActionInfo> raList = new List<Role_ActionInfo>();

            //通过roleId获取roleAction列表
            raList = db.Role_ActionInfo.Where<Role_ActionInfo>(ra => ra.Role_Id == rId).ToList<Role_ActionInfo>();
            ViewBag.role_id = rId;//
            ViewBag.raList = raList;

            return View();
        }

        public ActionResult Edit(string idstr, string role_id)
        {
            object o = new object();
            lock (o)
            {
                int r_id;
                if (int.TryParse(role_id, out r_id))
                {
                    if (idstr != null)
                    {
                        

                        var raList = db.Role_ActionInfo.Where(ra => ra.Role_Id == r_id);
                        foreach (var item in raList)
                        {
                            db.Role_ActionInfo.Remove(item);
                        }
                        db.SaveChanges();
                        //删除成功了，现在加上新权限
                        if (idstr != "")
                        {
                            if (idstr.Contains(","))
                            {
                                idstr = idstr.Substring(0, idstr.Length - 1);
                                string[] ids = idstr.Split(',');
                                List<Role_ActionInfo> ral_add = new List<Role_ActionInfo>();
                                foreach (var item in ids)
                                {
                                    Role_ActionInfo ra = new Role_ActionInfo();
                                    ra.ActionInfo_Id = Convert.ToInt32(item);
                                    ra.Role_Id = r_id;
                                    ral_add.Add(ra);
                                }
                                //去重之后，添加到数据库
                                var ral_add_distinct = ral_add.Where((x, i) => ral_add.FindIndex(z => z.Role_Id == x.Role_Id && z.ActionInfo_Id == x.ActionInfo_Id) == i);
                                foreach (var item in ral_add_distinct)
                                {
                                    db.Role_ActionInfo.Add(item);
                                }
                                if (db.SaveChanges() > 0)
                                {
                                    return Json(new { msg = "OK", content = "修改成功!" });
                                }
                                else
                                {
                                    return Json(new { msg = "OK", content = "修改失败!" });
                                }
                            }
                        }

                        //先删除所有的前权限

                    }
                    else
                    {
                        return Json(new { msg = "Fail", content = "参数不合法!" });
                    }
                }
                else
                {
                    return Json(new { msg = "Fail", content = "角色id不合法!" });
                }

                return null;

            }
        }
    }
}






//[AuthorizeFilter]
//[HttpPost]
