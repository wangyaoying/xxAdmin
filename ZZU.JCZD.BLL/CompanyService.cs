using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZU.JCZD.Dal;
using ZZU.JCZD.Model;

namespace ZZU.JCZD.BLL
{
    /// <summary>
    /// 公司的业务交互类
    /// </summary>
    public class CompanyService:BaseService<Company>
    {
        /// <summary>
        /// 公司的业务操作类
        /// </summary>

        public override void SetCurrentDal()
        {
            currentDal = new CompanyDal();
        }
    }
}
