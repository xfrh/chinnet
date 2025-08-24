using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodematicWEB.Code.Model
{
    /// <summary>
    /// 表实体类
    /// </summary>
    public class TableInfoModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 数据库表名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// Entity类生成路径
        /// </summary>
        public string EntityClassPath { get; set; }

        /// <summary>
        /// Entity类命名空间
        /// </summary>
        public string EntityNameSpace { get; set; }

        /// <summary>
        /// Entity名
        /// </summary>
        public string EntityClassName { get; set; }

        /// <summary>
        /// Data类生成路径
        /// </summary>
        public string DataClassPath { get; set; }

        /// <summary>
        /// Data类命名空间
        /// </summary>
        public string DataNameSpace { get; set; }

        /// <summary>
        /// Data类名
        /// </summary>
        public string DataClassName { get; set; }

        /// <summary>
        /// Services类生成路径
        /// </summary>
        public string ServicesClassPath { get; set; }

        /// <summary>
        /// Services类命名空间
        /// </summary>
        public string ServicesNameSpace { get; set; }

        /// <summary>
        /// Services类名
        /// </summary>
        public string ServicesClassName { get; set; }

        /// <summary>
        /// Controllers类地址
        /// </summary>
        public string ControllerClassPath { get; set; }

        /// <summary>
        /// Controllers类命名空间
        /// </summary>
        public string ControllerNameSpace { get; set; }

        /// <summary>
        /// Controllers类名
        /// </summary>
        public string ControllerClassName { get; set; }

        /// <summary>
        ///Model类生成路径
        /// </summary>
        public string ModelClassPath { get; set; }

        /// <summary>
        /// Model类命名空间
        /// </summary>
        public string ModelNameSpace { get; set; }

        /// <summary>
        /// Model类名
        /// </summary>
        public string ModelClassName { get; set; }

        /// <summary>
        ///Validator验证类地址
        /// </summary>
        public string ValidatorClassPath { get; set; }

        /// <summary>
        ///Validator类命名空间
        /// </summary>
        public string ValidatorNameSpace { get; set; }

        /// <summary>
        ///Validator类名
        /// </summary>
        public string ValidatorClassName { get; set; }

        /// <summary>
        /// 页面路径
        /// </summary>
        public string PagePath { get; set; }

        /// <summary>
        ///是否添加AutoMapper映射
        /// </summary>
        public bool AddAutoMapper { get; set; }

        /// <summary>
        ///是否注册Autofac
        /// </summary>
        public bool AddAutofac { get; set; }

        /// <summary>
        /// 该表中字段集合
        /// </summary>
        public List<ColumnInfo> ColumnInfoList { get; set; }

    }
}