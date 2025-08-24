using Autofac;
using Autofac.Integration.Mvc;
using ManageSystem.Core.Data;
using ManageSystem.Core.Infrastructure.DependencyManagement;
using ManageSystem.Data;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Log;
using ManageSystem.Services.Security;
using ManageSystem.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Weixin;
using ManageSystem.Services.SystemSet;

using ManageSystem.Services.Members;
using ManageSystem.Services.CRProjects;
using ManageSystem.Services.Survey;
using ManageSystem.Services.Teams;
using ManageSystem.Services.Medicine;

namespace ManageSystem.Admin.Extensions
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

            //自动生成代码标识位
            //问卷调查
            builder.RegisterType<SurveyRecordService>().As<ISurveyRecordService>().InstancePerLifetimeScope();
            builder.RegisterType<SurveySubjectService>().As<ISurveySubjectService>().InstancePerLifetimeScope();
            builder.RegisterType<SurveySubjectOptionService>().As<ISurveySubjectOptionService>().InstancePerLifetimeScope();
            builder.RegisterType<SurveySurveyService>().As<ISurveySurveyService>().InstancePerLifetimeScope();

            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<CRProjectService>().As<ICRProjectService>().InstancePerLifetimeScope();
            builder.RegisterType<CRProjectItemService>().As<ICRProjectItemService>().InstancePerLifetimeScope();
            builder.RegisterType<CRProjectLogService>().As<ICRProjectLogService>().InstancePerLifetimeScope();
            builder.RegisterType<SystemLogService>().As<ISystemLogService>().InstancePerLifetimeScope();

            builder.RegisterType<TeamCREContentService>().As<ITeamCREContentService>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectHospitalService>().As<IProjectHospitalService>().InstancePerLifetimeScope();
        }
    }
}