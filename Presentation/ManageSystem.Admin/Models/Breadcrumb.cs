using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models
{
    /// <summary>
    /// 后台面包屑导航实体类
    /// </summary>
    public class Breadcrumb
    {
        /// <summary>
        /// 所属菜单名称（顶级菜单）
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 二级菜单名称
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// 当前页面名称
        /// </summary>
        public string LocationName { get; set; }

    }
}