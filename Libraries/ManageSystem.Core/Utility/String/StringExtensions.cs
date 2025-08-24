using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ManageSystem.Core.Utility
{
    public static class StringExtensions
    {
        /// <summary>
        /// 剪切字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="showDot"></param>
        /// <returns></returns>
        public static string CutString(this String value, int length, bool showDot = true)
        {
            try
            {
                if (length <= 0) return "";
                if (value.Length <= length) return value;

                return value.Substring(0, length) + (showDot ? "..." : "");
            }
            catch (Exception)
            {

                return value;
            }

        }
        /// <summary>
        /// 判断是不是数值(可以是带符号的整数或小数)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 当前值是否是int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(this String value)
        {
            try
            {
                int temp = Convert.ToInt32(value);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// 当前值是否是int32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool IsInt(this String value, out int defaultValue)
        {
            try
            {
                int temp = Convert.ToInt32(value);
                defaultValue = temp;
                return true;
            }
            catch (Exception)
            {
                defaultValue = 0;
            }
            return false;
        }


        /// <summary>
        /// 获取int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetInt(this String value, int defaultValue = 0)
        {
            if (!value.IsInt()) return defaultValue;

            return int.Parse(value);
        }

        public static Boolean ToBoolean(this string value)
        {
            if (bool.TryParse(value, out bool defaultValue))
            {
                return defaultValue;
            }
            return defaultValue;
        }

        /// <summary>
        /// 当前值是否是long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLong(this String value)
        {
            try
            {
                long temp = Convert.ToInt64(value);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// 当前值是否是Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal(this String value)
        {
            try
            {
                decimal temp = Convert.ToDecimal(value);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }


        /// <summary>
        /// 获取Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal GetDecimal(this String value, decimal defaultValue = 0)
        {
            if (!value.IsDecimal()) return defaultValue;

            return decimal.Parse(value);
        }

        /// <summary>
        /// 获取Float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static float GetFloat(this String value, float defaultValue = 0)
        {
            if (!value.IsDecimal()) return defaultValue;

            return float.Parse(value);
        }

        /// <summary>
        /// 当前值是否是Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFloat(this String value)
        {
            try
            {
                float temp = float.Parse(value);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// 判断是否Double
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDouble(this String _value, out double value)
        {
            return double.TryParse(_value, out value);
        }

        /// <summary>
        /// 获取long
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static long GetLong(this String value, long defaultValue = 0)
        {
            if (!value.IsLong()) return defaultValue;

            return long.Parse(value);
        }

        /// <summary>
        /// 当前值是否是DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime(this String value)
        {
            try
            {
                //特殊情况： "20180101";
                if (value.IsLong() && value.Length == 8)
                {
                    string temp = value.Substring(0, 4) + "-" + value.Substring(4, 2) + "-" + value.Substring(6, 2);
                    Convert.ToDateTime(temp);
                    return true;
                }
                else
                {
                    Convert.ToDateTime(value);
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// 当前值是否是DateTime
        /// 如果是则返回日期
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsDateTime(this String value, out DateTime dateTime)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                    return false;
                }
                //特殊情况： "20180101";
                if (value.IsLong() && value.Length == 8)
                {
                    string temp = value.Substring(0, 4) + "-" + value.Substring(4, 2) + "-" + value.Substring(6, 2);
                    dateTime = Convert.ToDateTime(temp);
                    return true;
                }
                else
                {
                    dateTime = Convert.ToDateTime(value);
                    return true;
                }
            }
            catch (Exception)
            {
                dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            }
            return false;
        }

        /// <summary>
        /// 获取DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>如果不是日期格式，返回 1900-01-01</returns>
        public static DateTime GetDateTime(this String value)
        {
            if (!value.IsDateTime())
                return DateTime.Parse("1900-01-01");

            //特殊情况： "20180101";
            if (value.IsLong() && value.Length == 8)
            {
                string temp = value.Substring(0, 4) + "-" + value.Substring(4, 2) + "-" + value.Substring(6, 2);
                return DateTime.Parse(temp);
            }

            return DateTime.Parse(value);
        }

        /// <summary>
        /// 获取DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime GetDateTime(this String value, DateTime defaultValue)
        {
            if (!value.IsDateTime()) return defaultValue;

            return DateTime.Parse(value);
        }

        /// <summary>
        /// 将json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string jsonValue)
        {
            return JsonConvert.DeserializeObject<T>(jsonValue);
        }


        /// <summary>
        /// 将填写的值中符号替换成英文的符号，比如大于、小于、等于等等
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceSymbol(this string value)
        {
            return value.Replace("≧", ">=").Replace("≥", ">=").Replace("≦", "<=").Replace("≤", "<=").Replace("＞", ">")
                .Replace("＜", "<").Replace("＝", "=").Replace("．", ".").Replace("。", ".").Replace(" ", "");
        }

        /// <summary>
        /// 将规则的符号替换成空，比如大于、小于、等于等等
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DeleteSymbol(this string value)
        {
            return value
                .Replace("≧", "")
                .Replace(">=", "")
                .Replace("≥", "")
                .Replace(">=", "")
                .Replace("≦", "")
                .Replace("<=", "")
                .Replace("≥", "")
                .Replace(">=", "")
                .Replace("≦", "")
                .Replace("<=", "")
                .Replace("≤", "")
                .Replace("<=", "")
                .Replace("＞", "")
                .Replace(">", "")
                .Replace("＜", "")
                .Replace("<", "")
                .Replace("＝", "")
                .Replace("=", "")
                .Replace("．", "")
                .Replace("。", ".")
                .Replace(" ", "");
        }

    }
}
