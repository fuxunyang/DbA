using System;
using System.Collections;

namespace DbA.Common
{
    /// <summary>
    /// 数据库访问器工厂
    /// </summary>
    public sealed class DAFactory
    {
        /// <summary>
        /// 数据访问器列表
        /// </summary>
        private static Hashtable daList = new Hashtable();
        /// <summary>
        /// 获取一个数据访问器实例（单例）
        /// </summary>
        /// <param name="cn">连接字符串</param>
        /// <typeparam name="T">数据库访问器类型</typeparam>
        /// <returns>数据访问器实例</returns>
        /// <remarks>连接字符串不同会产生不同的实例</remarks>
        public static T GetInstance<T>(string cn = "") where T : DbAccessor
        {
            var daType = typeof(T);
            var key = daType.Name;//访问器键名
            object[] args = { };
            if (!String.IsNullOrEmpty(cn))
            {
                key = cn;
                args = new object[] { cn };
            }
            if (!daList.ContainsKey(key))
            {
                var constructors = daType.GetConstructors();
                foreach (var c in constructors)
                {
                    var ps = c.GetParameters();
                    if (ps.Length.Equals(args.Length))
                    {
                        var da = c.Invoke(args) as T;
                        daList[key] = da;
                        return da; ;
                    }
                }
            }

            return daList[key] as T;
        }
    }
}