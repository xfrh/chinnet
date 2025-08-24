using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Utility
{
    public static class DateExtensions
    {

        /// <summary>
        ///获取指定格式的时间字符串，如果时间不合法（为空、小于等于等于1900等）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="type">获取的指定格式 ： L 表示是yyyy-MM-dd HH:mm    其他 yyyy-MM-dd</param>
        /// <returns></returns>
        public static string GetNormalString(this DateTime dateTime, string type = "L")
        {
            try
            {
                if (dateTime == null || string.IsNullOrWhiteSpace(dateTime.ToString())) return "";

                if (dateTime.Year < 1901) return "";

                if (type.ToUpper().Equals("L")) return dateTime.ToString("yyyy-MM-dd HH:mm");
                return dateTime.ToString("yyyy-MM-dd");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime dateTime)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
    }
}
