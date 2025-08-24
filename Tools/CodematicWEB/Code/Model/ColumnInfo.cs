using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodematicWEB.Code.Model
{
    /// <summary>
    /// 字段信息
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string  ShowName { get; set; }

        /// <summary>
        /// 数据类型 C#
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPK { get; set; }

        /// <summary>
        /// 是否需要输入验证
        /// </summary>
        public bool IsValidator { get; set; }
        
        /// <summary>
        /// 非空验证提示内容
        /// </summary>
        public string ValidateEmpty { get; set; }

        /// <summary>
        ///其他验证（日期、整数等等），根据数据类型判定
        /// </summary>
        public string ValidateOther { get; set; }

        /// <summary>
        /// 其他验证值，比如长度
        /// </summary>
        public string ValidateOtherValue{ get; set; }

    }
}