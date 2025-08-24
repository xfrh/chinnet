using Autofac;
using Autofac.Integration.Mvc;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Data;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Infrastructure.DependencyManagement;
using ManageSystem.Data;
using ManageSystem.Services;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Log;
using ManageSystem.Services.CRProjects;
using ManageSystem.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ManageSystem.Services.Medicine;

namespace ManageSystem.Web
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
            builder.RegisterType<CRProjectService>().As<ICRProjectService>().InstancePerLifetimeScope();

            builder.RegisterType<CRProjectItemService>().As<ICRProjectItemService>().InstancePerLifetimeScope();
            builder.RegisterType<CRProjectLogService>().As<ICRProjectLogService>().InstancePerLifetimeScope();

            builder.RegisterType<ProjectHospitalService>().As<IProjectHospitalService>().InstancePerLifetimeScope();
        }
    }
}