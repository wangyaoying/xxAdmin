using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ZZU.JCZD.Model;

namespace ZZU.JCZD.DAL
{
   /// <summary>
    ///  通过工厂保证db在线程内唯一
   /// </summary>
    public class DBContexFactory
    {
        public static DbContext CreateDbContext()
        {
            DbContext dbcontext = (DbContext)CallContext.GetData("dbContext");
            if (dbcontext == null)
            {
                dbcontext = new JCZDEntities();
                CallContext.SetData("dbContext", dbcontext);
            }
            return dbcontext;
        }
    }
}
