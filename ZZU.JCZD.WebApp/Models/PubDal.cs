using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZZU.JCZD.BLL;
using ZZU.JCZD.Model;
namespace ZZU.JCZD.WebApp.Models
{
    public class PubDal
    {
        JCZDEntities db = DBContextBll.GetDbContext();
        /// <summary>
        /// 根据角色id返回对应的模块列表
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public  DataTable GetActionsByRole(int roleid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(long));
            dt.Columns.Add("actionName", typeof(string));
            dt.Columns.Add("controller", typeof(string));
            dt.Columns.Add("action", typeof(string));
            var actionList = from ra in db.Role_ActionInfo
                             where ra.Role_Id == roleid
                             join a in db.ActionInfo on ra.ActionInfo_Id  equals a.Id
                             select new
                             {
                                 Id = a.Id,
                                 actionName = a.ActionName,
                                 controler = a.Controller,
                                 action = a.Action
                             };
            if (actionList.Count() > 0)
            {
                foreach (var a in actionList)
                {
                    dt.Rows.Add(new object[] { a.Id, a.actionName, a.controler, a.action });
                }
            }
            return dt;

        }
        ///// <summary>
        ///// 根据登录信息获取登录人下属用户
        ///// </summary>
        ///// <param name="userInfo"></param>
        ///// <param name="platformInfo"></param>
        ///// <returns></returns>
        //public  List<long> Get_UserIDS(object userInfo, object platformInfo)
        //{
        //    List<long> userIDList = new List<long>();
        //    if (platformInfo == null)
        //    {
        //        if (userInfo == null)
        //        {
        //            //没有登录
        //            return userIDList;
        //        }
        //        else
        //        {
        //            //普通用户登录
        //            userIDList.Add(((T_User)userInfo).id);
        //        }
        //    }
        //    else
        //    {
        //       long platform_id = ((T_PlatForm)platformInfo).id;
        //        //获取对应平台id下的所有用户
        //        var userList = db.T_User.Where(u => u.platform_id == platform_id).ToList();
        //        if (userList.Count <= 0) return userIDList;
        //        foreach (T_User item in userList)
        //        {
        //            userIDList.Add(item.id);
        //        }
        //    }
        //    return userIDList;
        //}

        

        ///// <summary>
        ///// 获取平台id
        ///// </summary>
        ///// <param name="userInfo"></param>
        ///// <param name="platformInfo"></param>
        ///// <returns></returns>
        //public static long Get_PlatForm_id(object userInfo, object platformInfo)
        //{
        //    long platform_id = -1;
        //    //校验session
        //    if (platformInfo == null)
        //    {
        //        if (userInfo == null)
        //        {
        //            //没有登录
        //            return -1;
        //        }
        //        else
        //        {
        //            //普通用户登录
        //            platform_id = (long)((T_User)userInfo).platform_id;
        //        }
        //    }
        //    else
        //    {
        //        platform_id = ((T_PlatForm)platformInfo).id;
        //    }
        //    return platform_id;
        //}
        ///// <summary>
        ///// 获取平台信息
        ///// </summary>
        ///// <param name="userInfo"></param>
        ///// <param name="platformInfo"></param>
        ///// <returns></returns>
        //public T_PlatForm GetPlatInfo(object userInfo, object platformInfo) {
        //    T_PlatForm pf =null;
        //    //校验session
        //    if (platformInfo == null)
        //    {
        //        if (userInfo == null)
        //        {
        //            //没有登录
        //            return null;
        //        }
        //        else
        //        {
        //            //普通用户登录
        //            pf =db.T_PlatForm.Where(p=>p.id==((T_User)userInfo).platform_id).FirstOrDefault();
        //        }
        //    }
        //    else
        //    {
        //        pf = (T_PlatForm)platformInfo;
        //    }
        //    return pf;
        //}
        ///// <summary>
        ///// 根据设备信息获取对应的xml文件
        ///// </summary>
        ///// <param name="dti"></param>
        ///// <returns></returns>
        //public  string Getpath(DevTotalInfo dti) {
        //    try
        //    {
        //        return System.AppDomain.CurrentDomain.BaseDirectory + "Content\\xml\\" + dti.platform_name + "\\" + dti.p_type_id + ".xml";
        //    } catch {
        //        return "";
        //    }
        //}
        //public static string GetShortTimeString(DateTime dt) {
        //    if (dt != null) {
        //        return dt.Year + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
        //    }
        //    return "";
        //}

        //#region 其他control简单dal
        /// <summary>
        /// 根据模块名称检测模块是否存在
        /// </summary>
        /// <param name="module_name"></param>
        /// <returns></returns>
        public bool ActionNameExist(string actionName)
        {
            var actionInfo = db.ActionInfo.Where(a => a.ActionName == actionName).ToList().FirstOrDefault();
            if (actionInfo == null) return false;
            else return true;
        }
        ///// <summary>
        ///// 检测用户分类名称是否存在
        ///// </summary>
        ///// <param name="uc_name"></param>
        ///// <param name="platform_id"></param>
        ///// <returns></returns>
        //public bool UC_Name_Exist(string uc_name,long platform_id) {
        //    var ucInfo = db.T_UserCate.Where(uc => uc.platform_id == platform_id && uc.user_type == uc_name).FirstOrDefault();
        //    if (ucInfo == null) return false;
        //    return true;
        //}
        ///// <summary>
        ///// 修改时检测
        ///// </summary>
        ///// <param name="uc_name"></param>
        ///// <param name="id"></param>
        ///// <param name="platform_id"></param>
        ///// <returns></returns>
        //public bool UC_NameID_Exist(string uc_name, long id, long platform_id) {
        //    var ucInfo = db.T_UserCate.Where(uc => uc.platform_id == platform_id && uc.user_type == uc_name&&uc.id!=id).FirstOrDefault();
        //    if (ucInfo == null) return false;
        //    return true;
        //}
        ///// <summary>
        ///// 检测用户分类名称是否存在
        ///// </summary>
        ///// <param name="dg_name"></param>
        ///// <param name="platform_id"></param>
        ///// <returns></returns>
        //public bool DG_Name_Exist(string dg_name, long platform_id)
        //{
        //    var dgInfo = db.T_DevGroup.Where(dg => dg.platform_id == platform_id && dg.group_name == dg_name).FirstOrDefault();
        //    if (dgInfo == null) return false;
        //    return true;
        //}
        ///// <summary>
        ///// 修改时检测
        ///// </summary>
        ///// <param name="uc_name"></param>
        ///// <param name="id"></param>
        ///// <param name="platform_id"></param>
        ///// <returns></returns>
        //public bool DG_NameID_Exist(string dg_name, long id, long platform_id)
        //{
        //    var dgInfo = db.T_DevGroup.Where(dg => dg.platform_id == platform_id && dg.group_name == dg_name&&dg.id!=id).FirstOrDefault();
        //    if (dgInfo == null) return false;
        //    return true;
        //}
        //#endregion
    }
}