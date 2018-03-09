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
    public class RoleController : BaseController
    {                     
        // GET: /Role/
        JCZDEntities db = DBContextBll.GetDbContext();       
       
        public ActionResult Index()
        {
            ViewBag.CompList = db.Company.Where<Company>(c => true).ToList();
            ////////////////这里是修改的///////////////////////ViewBag.compId = compId;
            return View();
        }
        #region 获取角色列表
        public JsonResult GetRole()
        {
            
            if (compId != 1)
            {//用户公司查看自己公司的角色
                var roleList = db.Role.Where<Role>(r => r.CompId == compId).ToList();
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            else
            {//本公司查看所有的角色
                var roleList = db.Role.Where<Role>(r =>true).ToList();
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
        } 
        #endregion
        #region 添加角色
        public ContentResult AddRole(string rolename, string detail, string companyId)
        {
            if (db.Role.Where<Role>(r =>r.RoleName==rolename).FirstOrDefault()==null)
            {
                Role r = new Role()
                {
                    RoleName = rolename,
                    Detail = detail,
                    DelFlag = false,
                    CompId=int.Parse(companyId)
                };
                if (db.Role.Add(r) != null)
                {
                    db.SaveChanges();
                    return Content("ok");
                }
                else {
                    return Content("no");
                }
            }
            else
            {
                return Content("ex");
            }           
        } 
        #endregion
        #region 修改角色信息
        public ContentResult EditRole(string rolename, string detail, string id)
        {
            int rid = int.Parse(id);
            Role role = db.Role.Where<Role>(r => r.Id == rid).FirstOrDefault<Role>();
            role.RoleName = rolename;
            role.Detail = detail;
            
            if (db.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        } 
        #endregion

        #region 删除角色信息
        public ContentResult DeleteRoles(string ids)
        {
            string[] idArr = ids.Split(',');
            int id;
            Role role;
            for (int i = 0; i < idArr.Length; i++)
            {
                id = int.Parse(idArr[i]);
                role = db.Role.Where<Role>(r => r.Id == id).FirstOrDefault<Role>();
                db.Role.Remove(role);
            }
            if (db.SaveChanges() > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }  
        #endregion
    }
}
