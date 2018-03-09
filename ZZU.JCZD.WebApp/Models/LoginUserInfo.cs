using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZZU.JCZD.WebApp.Models
{
    /// <summary>
    /// 登陆用户信息
    /// </summary>
    public class LoginUserInfo
    {
        public string UserName { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int CompId { get; set; }
        public string CompName { get; set; }
        public int UserInfoId { get; set; }


    }
}