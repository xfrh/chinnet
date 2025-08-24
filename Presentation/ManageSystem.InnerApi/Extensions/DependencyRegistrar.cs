using Autofac;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Infrastructure.DependencyManagement;
using ManageSystem.Services.Authentication;

namespace ManageSystem.InnerApi.Extensions
{

    /// <summary>
    /// 依赖注册
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 20; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, ManageSystemConfig config)
        {
            builder.RegisterType<FormsMemberAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            //自动生成代码标识位

        }
    }
}