using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZU.JCZD.DAL;
using ZZU.JCZD.Model;

namespace ZZU.JCZD.BLL
{
    /// <summary>
    /// view层调用该方法获取JCModelContainer，实现view和Dal层的解耦
    /// </summary>
    public  class DBContextBll
    {
        public static JCZDEntities GetDbContext()
        {
            return DBContexFactory.CreateDbContext() as JCZDEntities;
        }
          
    }
}
