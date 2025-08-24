using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using ManageSystem.Core.Configuration;

namespace ManageSystem.Core.Infrastructure
{
    /// <summary>
    /// 提供有关类型的信息在当前的web应用程序。选择这个类可以看看bin文件夹中的所有组件。
    /// </summary>
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Fields

        private bool _ensureBinFolderAssembliesLoaded = true;
        private bool _binFolderAssembliesLoaded;

        #endregion

        #region Properties

        /// <summary>
        /// 返回或者设置web应用程序的组件在本文件夹中是否应该具体分析检查得到加载应用程序负载。这是需要在插件的情况下需要加载后的AppDomain,应用程序被重新加载。
        /// </summary>
        public bool EnsureBinFolderAssembliesLoaded
        {
            get { return _ensureBinFolderAssembliesLoaded; }
            set { _ensureBinFolderAssembliesLoaded = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 物理磁盘的路径\ Bin目录
        /// </summary>
        /// <returns>物理路径。如。“c:\ inetpub \ wwwroot \ bin”</returns>
        public virtual string GetBinDirectory()
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HttpRuntime.BinDirectory;
            }

            // 不主持。例如,在单元测试中运行
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public override IList<Assembly> GetAssemblies()
        {
            if (this.EnsureBinFolderAssembliesLoaded && !_binFolderAssembliesLoaded)
            {
                _binFolderAssembliesLoaded = true;
                string binPath = GetBinDirectory();
                //binPath = _webHelper.MapPath("~/bin");
                LoadMatchingAssemblies(binPath);
            }

            return base.GetAssemblies();
        }

        #endregion
    }
}
