using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;


namespace ManageSystem.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    ///容器管理器
    /// </summary>
    public class ContainerManager
    {
        private readonly IContainer _container;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">Conainer</param>
        public ContainerManager(IContainer container)
        {
            this._container = container;
        }

        /// <summary>
        ///得到一个容器
        /// </summary>
        public virtual IContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        ///解决
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">范围;通过空自动解决当前的范围</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
                
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 解决
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">范围;通过空自动解决当前的范围</param>
        /// <returns>Resolved service</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// 解决所有
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">范围;通过空自动解决当前的范围e</param>
        /// <returns>解决服务</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// 解决未注册的服务
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="scope">范围;通过空自动解决当前的范围</param>
        /// <returns>Resolved service</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// 解决未注册的服务
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">范围;通过空自动解决当前的范围</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new ManageSystemException("未知的依赖");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (ManageSystemException)
                {

                }
            }
            throw new ManageSystemException("没有contructor发现的所有依赖项satisfiedNo contructor发现都满意的依赖关系");
        }

        /// <summary>
        /// 试图解决私营化程度
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围;通过空自动解决当前的范围</param>
        /// <param name="instance">解决服务</param>
        /// <returns>值指示是否服务已成功解决</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// 检查是否有服务注册(可以解决)
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围;通过空自动解决当前的范围</param>
        /// <returns>结果</returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// 解决可选
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围;通过空自动解决当前的范围</param>
        /// <returns>解决服务</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// 得到当前的范围
        /// </summary>
        /// <returns>范围</returns>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                //这样的一生范围返回时,你应该确保它会处理曾经在安排任务(例如)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {

                //这样的一生范围返回时,你应该确保它会处理曾经在安排任务(例如)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}
