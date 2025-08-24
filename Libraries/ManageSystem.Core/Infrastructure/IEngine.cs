using System;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Infrastructure.DependencyManagement;

namespace ManageSystem.Core.Infrastructure
{
    /// <summary>
    /// 实现了这个接口的类可以作为门户的各种服务组合Nop引擎。
    /// 编辑功能,模块通过这个接口和实现访问大多数Nop功能
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// 容器管理器
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// 在系统环境初始化组件和插件。
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(ManageSystemConfig config);

        /// <summary>
        /// 解决依赖关系
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        ///  解决依赖关系
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 解决依赖关系
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();
    }
}
