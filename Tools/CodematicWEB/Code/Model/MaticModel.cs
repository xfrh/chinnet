using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodematicWEB.Code.Model
{
    /// <summary>
    /// 选择生成的表数据
    /// </summary>
    public class MaticModel
    {
        /// <summary>
        /// 数据库中表的名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 文件存储目录，页面和js文件的目录，相对路径
        /// </summary>
        public string FloderPath { get; set; }

        /// <summary>
        /// 是否需要前台和后台对输入内容验证，验证类型：非空验证、整数验证等等，根据字段的数据类型验证
        /// </summary>
        public bool IsValidate { get; set; }

        /// <summary>
        /// 用于显示的名称
        /// </summary>
        public string TableTitle { get; set; }

        /// <summary>
        /// 实体命名空间名称，需要完成名称
        /// </summary>
        public string ModelNameSpace { get; set; }

        /// <summary>
        /// 数据库操作类命名空间名称，需要完成名称
        /// </summary>
        public string DalNameSpace { get; set; }

        /// <summary>
        /// 业务处理类命名空间名称，需要完成名称
        /// </summary>
        public string BllNameSpace { get; set; }

        /// <summary>
        /// 页面命名空间名称，需要完成名称
        /// </summary>
        public string PageNameSpace { get; set; }

        /// <summary>
        /// 该表的字段集合
        /// </summary>
        public List<ColumnInfo> FieldList { get; set; }

        /// <summary>
        /// 类名称，默认表名+所属类库的名称，
        /// 比如：表名（Test），实体层：Test，数据库操作层是：TestSerivce，业务逻辑层是：TestManage
        /// </summary>
        public string ClassName { get; set; }

    }
}