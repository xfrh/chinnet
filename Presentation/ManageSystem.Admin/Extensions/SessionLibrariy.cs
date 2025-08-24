using ManageSystem.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Extensions
{
    /// <summary>
    /// 所有有关session的声明
    /// </summary>
    public class SessionLibrariy
    {
        /// <summary>
        /// 后台的菜单缓存，存储的是HTML代码
        /// </summary>
        public static string MenuHtml
        {
            get
            {
                var httpSession = EngineContext.Current.Resolve<HttpSessionStateBase>();
                var value = httpSession["MenuHtml"];
                if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return "";

                return value.ToString();
            }
            set
            {
                var httpSession = EngineContext.Current.Resolve<HttpSessionStateBase>();
                httpSession["MenuHtml"] = value;
            }
        }

    }
}