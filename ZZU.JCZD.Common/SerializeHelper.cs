
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ZZU.JCZD.Common
{
    /// <summary>
    /// 使用json.net进行序列化和反序列化
    /// </summary>
   public class SerializeHelper
    {
       /// <summary>
       /// 对数据进行序列化
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
       public static string  SerializeToString(object value)
       {
          return JsonConvert.SerializeObject(value);
       }
       /// <summary>
       /// 反序列化操作
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="str"></param>
       /// <returns></returns>
       public static T DeserializeToObject<T>(string str)
       {
          return JsonConvert.DeserializeObject<T>(str);
       }
    }
}
