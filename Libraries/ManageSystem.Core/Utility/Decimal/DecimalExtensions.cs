using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Utility
{
    public static class DecimalExtensions
    {

        /// <summary>
        /// 获取字符串格式
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="len">保留小数的位数</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ToString3(this Decimal value, int len = 2, int defaultValue = 0)
        {
            return Math.Round(value, 1, MidpointRounding.AwayFromZero).ToString();
        }

        /// <summary>
        /// 获取四舍五入指定长度的数值
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="len">保留小数的位数</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static Decimal GetDecimal2(this Decimal value, int len = 2, int defaultValue = 0)
        {
            return Math.Round(value, len, MidpointRounding.AwayFromZero);
        }


    }
}
