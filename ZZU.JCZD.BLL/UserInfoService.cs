using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZU.JCZD.BLL;
using ZZU.JCZD.DAL;
using ZZU.JCZD.Model;

namespace ZZU.JCZD.BLL
{
    public class UserInfoService : BaseService<UserInfoSet>
    {
        //这是写自己的方法
        public override void SetCurrentDal()
        {
            currentDal = new UserInfoDal();
        }
    }
}
