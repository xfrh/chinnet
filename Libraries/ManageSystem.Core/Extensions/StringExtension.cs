using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Extensions
{
    public static class StringExtension
    {

        /// <summary>
        /// 如果值是null，设置值为双引号（“”），否则不改变值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void GetDefaultValue(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                value = "";
        }

        /// <summary>
        /// 检查值是否是正常的日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime2(this string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value)) return false;

                DateTime time = Convert.ToDateTime(value);
                if (time.Year < 1950) return false;

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}