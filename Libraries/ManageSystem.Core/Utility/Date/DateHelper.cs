using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Utility
{
    public class DateHelper
    {

        /// <summary>
        /// 获取系统默认的日期
        /// </summary>
        /// <returns></returns>
        public static DateTime DefaultValue()
        {
            return DateTime.Parse("1900-01-01 00:00");
        }

        /// <summary>
        /// 检查值是否是正常的日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime(object value)
        {
            try
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return false;

                DateTime time = Convert.ToDateTime(value);
                if (time.Year < 1901) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查传入的时间是在当前时间的多少分或小时或月
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string BeforeTime(DateTime time)
        {
            DateTime now = DateTime.Now;

            if (now.Year > time.Year) return now.Year - time.Year + "年以前";
            if (now.Month > time.Month) return now.Month - time.Month + "月以前";
            if (now.Hour > time.Hour) return now.Hour - time.Hour + "小时以前";
            if (now.Minute > time.Minute) return now.Minute - time.Minute + "分钟以前";

            return "1分钟以前";
        }

        /// <summary>
        /// 根据日期转成中文周
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetWeekName(DateTime time)
        {
            var weekdays = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

            return weekdays[(int)time.DayOfWeek];
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        public static long Timestamp
        {
            get
            {
                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
                return Convert.ToInt64((DateTime.Now - start).TotalSeconds);
            }
        }

        public static long UtcTimestamp
        {
            get
            {
                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return Convert.ToInt64((DateTimeOffset.UtcNow - start).TotalSeconds);
            }
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(long timestamp)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            return start.AddSeconds(timestamp);
        }
    }
}
