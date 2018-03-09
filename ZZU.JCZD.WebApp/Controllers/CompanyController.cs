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
    public class CompanyController : BaseController
    {
        JCZDEntities db = DBContextBll.GetDbContext();
       
        public ActionResult Index()
        {

            return View();
        }
        #region 获取公司列表
        public JsonResult GetCompanys()
        {
            var companyList = db.Company.Where<Company>(c => true).ToList();
            return Json(companyList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 添加公司
        public ContentResult AddCompany(string companyname, string companyaddress, string companyemail, string companyurl, string detail)
        {
            Company company = new Company()
            {
                CompName = companyname,
                Address = companyaddress,
                CompEmail = companyemail,
                CompUrl = companyurl,
                Detail = detail
            };
            //保证公司名字唯一
            if (db.Company.Where<Company>(c => c.CompName == companyname) != null)
            {
                if (db.Company.Add(company) != null)
                {
                    db.SaveChanges();
                    return Content("ok");

                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("no");
            }

        }
        #endregion

        #region 修改公司信息
        public ContentResult EditCompany(string companyname, string companyaddress, string companyemail, string companyurl, string detail, string id)
        {
            int cid;
            //确保参数是int类型
            if (int.TryParse(id, out cid))
            {
                Company comp = db.Company.Where<Company>(c => c.Id == cid).FirstOrDefault();
                //确保修改的数据存在
                if (comp != null)
                {
                    comp.CompName = companyname;
                    comp.Address = companyaddress;
                    comp.CompEmail = companyemail;
                    comp.CompUrl = companyurl;
                    comp.Detail = detail;
                    if (db.SaveChanges() > 0)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("noedit");
                    }
                }
                else { return Content("no"); }
            }
            else
            {
                return Content("no");
            }

        } 
        #endregion

        #region 删除公司
        public ContentResult DeleteCompanys(string ids)
        {
            string[] idArr = ids.Split(',');
            int id;
            Company comp;
            for (int i = 0; i < idArr.Length; i++)
            {
                id = int.Parse(idArr[i]);
                comp = db.Company.Where<Company>(c => c.Id == id).FirstOrDefault<Company>();
                db.Company.Remove(comp);
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
