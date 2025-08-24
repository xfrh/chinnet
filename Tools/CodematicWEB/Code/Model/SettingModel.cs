using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodematicWEB.Code.Model
{
    public class SettingModel
    {
        /// <summary>
        /// 设置状态 ， true 表示完成，false 未完成
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 数据库版本
        /// </summary>
        public string DatabaseVersion { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 被生成项目的解决方案名称所，例：TestProject
        /// </summary>
        public string SolutionName { get; set; }


        /// <summary>
        /// Form表单Id，Form表单的Id，用于前台JS验证
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// 控件Id前缀，控件的id前缀，如果是母版页这里需要设置，例如：ctl00$ContentPlaceHolder1$
        /// </summary>
        public string ControlId { get; set; }

    }
}