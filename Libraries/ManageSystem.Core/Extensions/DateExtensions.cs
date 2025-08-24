using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Extensions
{
    /// <summary>
    /// 日期扩展函数
    /// </summary>
 public  static  class DateExtensions
    {

        /// <summary>
        /// 获取日期格式的值，如果日期为非法的则返回默认值（1900-01-01）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime DefaultValue(this DateTime time)
        {
            DateTime defalutValue = DateTime.Parse("1900-01-01");
       
            try
            {
                DateTime value = Convert.ToDateTime(time);
              
                if (value >= defalutValue) return value;
            }
            catch (Exception)
            {
            }

            return defalutValue;
        }

        /// <summary>
        /// 获取系统默认的时间，1900-01-01
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime DefaultValue(this DateTime? time)
        {
            return Convert.ToDateTime(time).DefaultValue();
        }

        /// <summary>
        /// 检查日期是否是正常的。不能为空且大于1900-01-01 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsNormal(this DateTime? time)
        {
            try
            {
                DateTime value = Convert.ToDateTime(time);
                DateTime defalutValue = DateTime.Parse("1900-01-01");

                if (value > defalutValue) return true;
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
