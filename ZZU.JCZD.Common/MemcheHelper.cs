using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZU.JCZD.Common
{
    public static class MemcheHelper
    {
        static readonly MemcachedClient mc=null;
        static MemcheHelper()
        {
            //最好放在配置文件中
            string[] serverlist = { "127.0.0.1:11211" };

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);
            pool.InitConnections = 10;
            pool.MinConnections = 10;
            pool.MaxConnections = 10;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient();
            mc.EnableCompression = false;
        }
        /// <summary>
        /// 这是添加的方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set(string key,object value)
        {
            return mc.Set(key, value);
        }
        public static bool Set(string key, object value,DateTime time)//指定过期时间，最大30天
        {
            return mc.Set(key, value,time);
           
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return mc.Get(key);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
               return mc.Delete(key);
            }
            return false;
        }
    }
}
