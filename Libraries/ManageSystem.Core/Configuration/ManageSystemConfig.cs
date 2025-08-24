using System;
using System.Configuration;
using System.Xml;
using ManageSystem.Core.Infrastructure;

namespace ManageSystem.Core.Configuration
{
    /// <summary>
    /// 描述一个 ManageSystemConfig
    /// </summary>
    public partial class ManageSystemConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// 创建一个配置节处理程序。
        /// </summary>
        /// <param name="parent">父对象。</param>
        /// <param name="configContext">配置上下文对象。</param>
        /// <param name="section">node XML节.</param>
        /// <returns>处理程序对象创建的部分。</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new ManageSystemConfig();

            var startupNode = section.SelectSingleNode("Startup");
            if (startupNode != null && startupNode.Attributes != null)
            {
                var attribute = startupNode.Attributes["IgnoreStartupTasks"];
                if (attribute != null)
                    config.IgnoreStartupTasks = Convert.ToBoolean(attribute.Value);
            }

            var redisCachingNode = section.SelectSingleNode("RedisCaching");
            if (redisCachingNode != null && redisCachingNode.Attributes != null)
            {
                var enabledAttribute = redisCachingNode.Attributes["Enabled"];
                if (enabledAttribute != null)
                    config.RedisCachingEnabled = Convert.ToBoolean(enabledAttribute.Value);

                var connectionStringAttribute = redisCachingNode.Attributes["ConnectionString"];
                if (connectionStringAttribute != null)
                    config.RedisCachingConnectionString = connectionStringAttribute.Value;

                var redisKeyPrefix = redisCachingNode.Attributes["KeyPrefix"];
                if (redisKeyPrefix != null)
                    config.RedisKeyPrefix = redisKeyPrefix.Value;
           
            }

            var userAgentStringsNode = section.SelectSingleNode("UserAgentStrings");
            if (userAgentStringsNode != null && userAgentStringsNode.Attributes != null)
            {
                var attribute = userAgentStringsNode.Attributes["databasePath"];
                if (attribute != null)
                    config.UserAgentStringsPath = attribute.Value;
            }

            var supportPreviousNopcommerceVersionsNode = section.SelectSingleNode("SupportPreviousNopcommerceVersions");
            if (supportPreviousNopcommerceVersionsNode != null && supportPreviousNopcommerceVersionsNode.Attributes != null)
            {
                var attribute = supportPreviousNopcommerceVersionsNode.Attributes["Enabled"];
                if (attribute != null)
                    config.SupportPreviousNopcommerceVersions = Convert.ToBoolean(attribute.Value);
            }

            var webFarmsNode = section.SelectSingleNode("WebFarms");
            if (webFarmsNode != null && webFarmsNode.Attributes != null)
            {
                var multipleInstancesEnabledAttribute = webFarmsNode.Attributes["MultipleInstancesEnabled"];
                if (multipleInstancesEnabledAttribute != null)
                    config.MultipleInstancesEnabled = Convert.ToBoolean(multipleInstancesEnabledAttribute.Value);

                var runOnAzureWebsitesAttribute = webFarmsNode.Attributes["RunOnAzureWebsites"];
                if (runOnAzureWebsitesAttribute != null)
                    config.RunOnAzureWebsites = Convert.ToBoolean(runOnAzureWebsitesAttribute.Value);
            }

            var installationNode = section.SelectSingleNode("Installation");
            if (installationNode != null && installationNode.Attributes != null)
            {
                var disableSampleDataDuringInstallationAttribute = installationNode.Attributes["DisableSampleDataDuringInstallation"];
                if (disableSampleDataDuringInstallationAttribute != null)
                    config.DisableSampleDataDuringInstallation = Convert.ToBoolean(disableSampleDataDuringInstallationAttribute.Value);

                var useFastInstallationServiceAttribute = installationNode.Attributes["UseFastInstallationService"];
                if (useFastInstallationServiceAttribute != null)
                    config.UseFastInstallationService = Convert.ToBoolean(useFastInstallationServiceAttribute.Value);

                var pluginsIgnoredDuringInstallationAttribute = installationNode.Attributes["PluginsIgnoredDuringInstallation"];
                if (pluginsIgnoredDuringInstallationAttribute != null)
                    config.PluginsIgnoredDuringInstallation = pluginsIgnoredDuringInstallationAttribute.Value;
            }
            
            return config;
        }

        /// <summary>
        /// 表明我们是否应该忽略启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; private set; }

        /// <summary>
        /// 路径数据库用户代理字符串
        /// </summary>
        public string UserAgentStringsPath { get; private set; }

        /// <summary>
        /// 表明我们是否应该使用复述,服务器缓存(而不是默认的内存缓存)
        /// </summary>
        public bool RedisCachingEnabled { get; private set; }
        /// <summary>
        /// 复述,连接字符串。复述时使用缓存启用
        /// </summary>
        public string RedisCachingConnectionString { get; private set; }

        /// <summary>
        ///  配置文件和缓存的默认前缀，例如：如果key的名称是web.admin.path ，那么在数据库和缓存中的完整名称就应该是：test.web.admin.path  ，防止一个服务器上多个相同网站导致redis的key重复等问题 
        /// </summary>
        public string RedisKeyPrefix { get; private set; }

        /// <summary>
        /// 表明我们是否应该支持先前的nopCommerce版本(它可以稍微提高性能)
        /// </summary>
        public bool SupportPreviousNopcommerceVersions { get; private set; }

        /// <summary>
        /// 值指示该网站是否运行在多个实例(例如web农场,与多个实例Windows Azure,等等)。
        /// 不启用它如果你在Azure上运行,但仅使用一个实例呢
        /// </summary>
        public bool MultipleInstancesEnabled { get; private set; }

        /// <summary>
        /// 值指示该网站是否运行在Windows Azure网站
        /// </summary>
        public bool RunOnAzureWebsites { get; private set; }

        /// <summary>
        /// 一个值指示是否一个店主可以在安装过程中安装示例数据
        /// </summary>
        public bool DisableSampleDataDuringInstallation { get; private set; }
        /// <summary>
        /// 默认情况下这个设置应该被设置为“False”(仅供高级用户)
        /// </summary>
        public bool UseFastInstallationService { get; private set; }
        /// <summary>
        /// 插件忽略nopCommerce安装期间的列表
        /// </summary>
        public string PluginsIgnoredDuringInstallation { get; private set; }

  
    }




}
