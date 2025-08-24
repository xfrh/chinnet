using System;
using System.Collections.Generic;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;

namespace ManageSystem.Admin.Models.SystemInfo
{
    public partial class SystemInfoModel : BaseEntityModel
    {
        public SystemInfoModel()
        {
            this.ServerVariables = new List<ServerVariableModel>();
            this.LoadedAssemblies = new List<LoadedAssembly>();
        }

        
        [HtmlDisplayAttribute("ASP.NET版本", "ASP.NET版本")]
        public string AspNetInfo { get; set; }

        [HtmlDisplayAttribute("完全信任级别", "完全信任级别")]
        public string IsFullTrust { get; set; }

        [HtmlDisplayAttribute("软件系统版本", "软件系统版本")]
        public string NopVersion { get; set; }

        [HtmlDisplayAttribute("操作系统", "操作系统")]
        public string OperatingSystem { get; set; }

        [HtmlDisplayAttribute("服务器本地时间", "服务器本地时间")]
        public DateTime ServerLocalTime { get; set; }

        [HtmlDisplayAttribute("服务器时区", "服务器时区")]
        public string ServerTimeZone { get; set; }

        [HtmlDisplayAttribute("格林威治标准时间(GMT/UTC)", "格林威治标准时间(GMT/UTC)")]
        public DateTime UtcTime { get; set; }

        [HtmlDisplayAttribute("地址", "HTTP_HOST用于多店解决方案，用来决定哪个商城是当前商城")]
        public string HttpHost { get; set; }

        [HtmlDisplayAttribute("服务器变量", "服务器变量的列表")]
        public IList<ServerVariableModel> ServerVariables { get; set; }

        [HtmlDisplayAttribute("已加载的程序集", "已加载的程序集")]
        public IList<LoadedAssembly> LoadedAssemblies { get; set; }

        public partial class ServerVariableModel : BaseEntityModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public partial class LoadedAssembly : BaseEntityModel
        {
            public string FullName { get; set; }
            public string Location { get; set; }
        }
    }
}