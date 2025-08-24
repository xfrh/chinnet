using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ManageSystem.Core.Utility
{
    /// <summary>
    ///  object 的扩展函数
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        /// 将对象序列化成json数据
        /// </summary>
        /// <param name="obj">需要被序列化的对象</param>
        /// <returns></returns>
        public static string SerializeObject(this object obj)
        {
           return  JsonConvert.SerializeObject(obj);
        }

   

    }
}
