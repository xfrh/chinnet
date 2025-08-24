using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Extensions
{
    /// <summary>
    /// MvcPager 分页控件的扩展
    /// </summary>
    public static class MvcPagerExtensions
    {
        /// <summary>
        /// 分页每页的数量
        /// </summary>
        public static int PageSize = 10;

        /// <summary>
        /// 获取分页相关参数，用于web分页
        /// </summary>
        /// <returns></returns>
        public static PagerOptions GetPagerOptions()
        {
            return new PagerOptions
            {
                ContainerTagName = "ul",
                CssClass = "pagination",
                CurrentPagerItemTemplate = "<li class=\"active\"><a href=\"#\">{0}</a></li>",
                DisabledPagerItemTemplate = "<li class=\"disabled\"><a>{0}</a></li>",
                PagerItemTemplate = "<li>{0}</li>",
                Id = "bootstrappager"
            };
        }
    }
}