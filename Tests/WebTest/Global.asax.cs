using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ManageSystem.Data;
using ManageSystem.Core.Data;

namespace WebTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {



            //ContainerBuilder builder = new ContainerBuilder();
            //builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<NewsItemService>().As<INewsItemService>().InstancePerDependency();

            #region 使用 Autofac依赖注入

            ContainerBuilder builder = new ContainerBuilder();

            //对所有的控制进行依赖注入
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            ////对所有的ManageSystem.Services 进行依赖注入
            //builder.RegisterType<NewsItemService>().As<INewsItemService>().InstancePerDependency();
            //builder.RegisterType<NewsCommentService>().As<INewsCommentService>().InstancePerDependency();

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //注入操作数据库接口
            builder.Register<IDbContext>(c => new ManageSystemContext()).InstancePerLifetimeScope();


            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

            #endregion

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }
    }
}
