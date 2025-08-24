using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using ManageSystem.Core.Configuration;


namespace ManageSystem.Core.Infrastructure
{
    /// <summary>
    /// 提供了访问单例实例引擎。
    /// </summary>
    public class EngineContext
    {
        #region Methods

        /// <summary>
        ///初始化静态工厂的实例。
        /// </summary>
        /// <param name="forceRecreate">创建一个新工厂实例,尽管工厂已经被初始化。</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new MSEngine();

                //web config 的配置
                ManageSystemConfig MSConfig = ConfigurationManager.GetSection("ManageSystemConfig") as ManageSystemConfig;
                Singleton<IEngine>.Instance.Initialize(MSConfig);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// 设置静态引擎实例提供的引擎。使用这个方法来提供自己的引擎实现。
        /// </summary>
        /// <param name="engine">要使用的引擎。</param>
        /// <remarks>只有使用这种方法如果你知道你在做什么。</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 得到了单引擎用来访问服务。
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }


        #endregion
    }
}
