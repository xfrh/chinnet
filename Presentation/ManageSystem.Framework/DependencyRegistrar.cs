using Autofac;
using ManageSystem.Core.Configuration;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Integration.Mvc;
using ManageSystem.Core.Data;
using ManageSystem.Data;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Log;

using ManageSystem.Services.Security;
using ManageSystem.Services.Users;
using System.Web;
using System.Reflection;
using ManageSystem.Framework.UI;
using ManageSystem.Services.Media;
using ManageSystem.Services.Configuration;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Services.Installation;
using Autofac.Core;
using Autofac.Builder;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Weixin;

using ManageSystem.Services.Members;
using ManageSystem.Services.Tasks;
using ManageSystem.Services.HttpContext;
using ManageSystem.Services.Meetings;
using ManageSystem.Services.Researches;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Products;
using ManageSystem.Services.Orders;
using ManageSystem.Services.Integrals;
using ManageSystem.Services.Organism;
using ManageSystem.Services.Datas;
using ManageSystem.Services.Messages;
using ManageSystem.Services.Documents;
using ManageSystem.Services.ScoringModule;
using ManageSystem.Services.Survey;
using ManageSystem.Services.Teams;
using ManageSystem.Services.Chart;
using ManageSystem.Services.CRE;
using ManageSystem.Services.CRs;
using ManageSystem.Services.MicdataDistribution;
using ManageSystem.Services.DataInput;
using ManageSystem.Services.SHChart;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.Ctr;

namespace ManageSystem.Framework
{

    /// <summary>
    /// 依赖注册
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {

        /// <summary>
        ///注册服务和接口
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, ManageSystemConfig config)
        {
            //HTPP相关注册
            builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).As<HttpContextBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();

            //对所有的控制进行依赖注入
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //注入操作数据库接口
            builder.RegisterType<ManageSystemContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //对所有的ManageSystem.Services 进行依赖注入
            builder.RegisterType<ActionLogService>().As<IActionLogService>().InstancePerLifetimeScope();
            builder.RegisterType<SystemLogService>().As<ISystemLogService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRegistrationService>().As<IUserRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserinfoService>().As<IUserinfoService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<PictureService>().As<IPictureService>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadService>().As<IDownloadService>().InstancePerLifetimeScope();
            builder.RegisterType<FunctionService>().As<IFunctionService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleFunctionService>().As<IRoleFunctionService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleTypeService>().As<IArticleTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleService>().As<IArticleService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleService>().As<IArticleService>().InstancePerLifetimeScope();
            builder.RegisterType<WeixinMenuService>().As<IWeixinMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<SystemConfigService>().As<ISystemConfigService>().InstancePerLifetimeScope();
            builder.RegisterType<WeixinUserService>().As<IWeixinUserService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleAttachmentService>().As<IArticleAttachmentService>().InstancePerLifetimeScope();
            builder.RegisterType<ArticleViewService>().As<IArticleViewService>().InstancePerLifetimeScope();
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerLifetimeScope();
            builder.RegisterType<AutoCodeService>().As<IAutoCodeService>().InstancePerLifetimeScope();
            builder.RegisterType<ValidateCodeService>().As<IValidateCodeService>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultSessionManager>().As<ISessionManager>().InstancePerLifetimeScope();
            builder.RegisterType<MeetingTypeService>().As<IMeetingTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<MeetingService>().As<IMeetingService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchService>().As<IResearchService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchTypeService>().As<IResearchTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<AreaService>().As<IAreaService>().InstancePerLifetimeScope();
            builder.RegisterType<MeetingApplyService>().As<IMeetingApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<MeetingViewService>().As<IMeetingViewService>().InstancePerLifetimeScope();
            builder.RegisterType<MeetingCollectService>().As<IMeetingCollectService>().InstancePerLifetimeScope();
            builder.RegisterType<MeetingCommentService>().As<IMeetingCommentService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchViewService>().As<IResearchViewService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchCollectService>().As<IResearchCollectService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchApplyService>().As<IResearchApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<ResearchCommentService>().As<IResearchCommentService>().InstancePerLifetimeScope();
            builder.RegisterType<HospitalService>().As<IHospitalService>().InstancePerLifetimeScope();
            builder.RegisterType<BacteriaTypeService>().As<IBacteriaTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<SpecimenService>().As<ISpecimenService>().InstancePerLifetimeScope();
            builder.RegisterType<HospitalDepartmentService>().As<IHospitalDepartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDataService>().As<IMedicalDataService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDataItemService>().As<IMedicalDataItemService>().InstancePerLifetimeScope();
            builder.RegisterType<DoctorTitleService>().As<IDoctorTitleService>().InstancePerLifetimeScope();
            builder.RegisterType<MemberAddressService>().As<IMemberAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductCategoryService>().As<IProductCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductBrandService>().As<IProductBrandService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductImageService>().As<IProductImageService>().InstancePerLifetimeScope();
            builder.RegisterType<MemberCartService>().As<IMemberCartService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderItemService>().As<IOrderItemService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderLogService>().As<IOrderLogService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderAddressService>().As<IOrderAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<MemberIntegralLogService>().As<IMemberIntegralLogService>().InstancePerLifetimeScope();
            builder.RegisterType<IntegralSettingService>().As<IIntegralSettingService>().InstancePerLifetimeScope();
            builder.RegisterType<MemberAttestationService>().As<IMemberAttestationService>().InstancePerLifetimeScope();
            builder.RegisterType<FeedbackService>().As<IFeedbackService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDataItemValidateService>().As<IMedicalDataItemValidateService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDepartmentTypeService>().As<IMedicalDepartmentTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDataWardTypeService>().As<IMedicalDataWardTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalSpecTypeService>().As<IMedicalSpecTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalOrganismTypeService>().As<IMedicalOrganismTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalOrganismService>().As<IMedicalOrganismService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDataProjectService>().As<IMedicalDataProjectService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalAntibioticService>().As<IMedicalAntibioticService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalAntibioticRuleService>().As<IMedicalAntibioticRuleService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalAntibioticResultService>().As<IMedicalAntibioticResultService>().InstancePerLifetimeScope();
            builder.RegisterType<BacteriaDetailedDataService>().As<IBacteriaDetailedDataService>().InstancePerLifetimeScope();
            builder.RegisterType<DataAntibioticDrugFastService>().As<IDataAntibioticDrugFastService>().InstancePerLifetimeScope();
            builder.RegisterType<DataGermYearService>().As<IDataGermYearService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDataDisposeLogService>().As<IMedicalDataDisposeLogService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageEmailService>().As<IMessageEmailService>().InstancePerLifetimeScope();

            builder.RegisterType<HospitalWardLocationService>().As<IHospitalWardLocationService>().InstancePerLifetimeScope();

            //资料下载
            builder.RegisterType<DocumentDownloadLogService>().As<IDocumentDownloadLogService>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentService>().As<IDocumentService>().InstancePerLifetimeScope();

            builder.RegisterType<DataQualityScoreService>().As<IDataQualityScoreService>().InstancePerLifetimeScope();
            builder.RegisterType<DataQualityScoreRecordService>().As<IDataQualityScoreRecordService>().InstancePerLifetimeScope();
            builder.RegisterType<DataQualityScoreDetailService>().As<IDataQualityScoreDetailService>().InstancePerLifetimeScope();
            builder.RegisterType<DataQualityReevaluationApplyService>().As<IDataQualityReevaluationApplyService>().InstancePerLifetimeScope();
            builder.RegisterType<DataQualityJuryService>().As<IDataQualityJuryService>().InstancePerLifetimeScope();
            builder.RegisterType<DataQualityHospitalService>().As<IDataQualityHospitalService>().InstancePerLifetimeScope();
            builder.RegisterType<DataQualityHospitalPageInitService>().As<IDataQualityHospitalPageInitService>().InstancePerLifetimeScope();
            builder.RegisterType<DataQualityActionLogService>().As<IDataQualityActionLogService>().InstancePerLifetimeScope();

            // 调研
            builder.RegisterType<SurveySurveyService>().As<ISurveySurveyService>().InstancePerLifetimeScope();
            builder.RegisterType<SurveySubjectService>().As<ISurveySubjectService>().InstancePerLifetimeScope();
            builder.RegisterType<SurveySubjectOptionService>().As<ISurveySubjectOptionService>().InstancePerLifetimeScope();
            builder.RegisterType<SurveyRecordService>().As<ISurveyRecordService>().InstancePerLifetimeScope();

            // CRE成员单位
            builder.RegisterType<TeamCREContentService>().As<ITeamCREContentService>().InstancePerLifetimeScope();

            // 图表管理
            builder.RegisterType<DataSegmentService>().As<IDataSegmentService>().InstancePerLifetimeScope();
            builder.RegisterType<HeatmapService>().As<IHeatmapService>().InstancePerLifetimeScope();
            builder.RegisterType<HeatmapItemService>().As<IHeatmapItemService>().InstancePerLifetimeScope();
            builder.RegisterType<BarChartService>().As<IBarChartService>().InstancePerLifetimeScope();
            builder.RegisterType<BarChartWithItemDataService>().As<IBarChartWithItemDataService>().InstancePerLifetimeScope();
            builder.RegisterType<TrendChartService>().As<ITrendChartService>().InstancePerLifetimeScope();
            builder.RegisterType<TrendChartWithItemDataService>().As<ITrendChartWithItemDataService>().InstancePerLifetimeScope();
            builder.RegisterType<MedicalDataHandleLockService>().As<IMedicalDataHandleLockService>().InstancePerLifetimeScope();

            //CRE手机端
            builder.RegisterType<CreDataService>().As<ICreDataService>().InstancePerLifetimeScope();
            builder.RegisterType<CreUploatDataService>().As<ICreUploatDataService>().InstancePerLifetimeScope();
            //CRE后台
            builder.RegisterType<CreBackstageService>().As<ICreBackstageService>().InstancePerLifetimeScope();
            //其他服务接口
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<ICacheManager>().InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            }

            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();
            builder.RegisterType<SqlFileInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            builder.RegisterType<SettingService>().As<ISettingService>().WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static")).InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());
            //CR服务接口
            builder.RegisterType<CRService>().As<ICRService>().InstancePerLifetimeScope();
            builder.RegisterType<CRItemService>().As<ICRItemService>().InstancePerLifetimeScope();
            builder.RegisterType<CRLogService>().As<ICRLogService>().InstancePerLifetimeScope();

            //MIC数据分布服务接口
            builder.RegisterType<ddYearService>().As<IddYearService>().InstancePerLifetimeScope();
            builder.RegisterType<ddGermService>().As<IddGermService>().InstancePerLifetimeScope();
            builder.RegisterType<ddDocumentService>().As<IddDocumentService>().InstancePerLifetimeScope();
            builder.RegisterType<ddDocumentItemService>().As<IddDocumentItemService>().InstancePerLifetimeScope();
            builder.RegisterType<ddCategoryValueService>().As<IddCategoryValueService>().InstancePerLifetimeScope();
            builder.RegisterType<ddCategoryService>().As<IddCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ddAntibioticService>().As<IddAntibioticService>().InstancePerLifetimeScope();

            //OL项目数据上传
            builder.RegisterType<OLTemplateService>().As<IOLTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<OLProjectService>().As<IOLProjectService>().InstancePerLifetimeScope();
            builder.RegisterType<OLFieldmodelService>().As<IOLFieldmodelService>().InstancePerLifetimeScope();
            builder.RegisterType<OLDataInputService>().As<IOLDataInputService>().InstancePerLifetimeScope();
            builder.RegisterType<OLMemberService>().As<IOLMemberService>().InstancePerLifetimeScope();

            //成员单位接口
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();

            //上海细菌真菌监测
            builder.RegisterType<SHDataSegmentService>().As<ISHDataSegmentService>().InstancePerLifetimeScope();
            builder.RegisterType<SHBarChartService>().As<ISHBarChartService>().InstancePerLifetimeScope();
            builder.RegisterType<SHBarChartWithItemDataService>().As<ISHBarChartWithItemDataService>().InstancePerLifetimeScope();

            //MIC访问记录
            builder.RegisterType<MICPermissionapplicationSevers>().As<IMICPermissionapplication>().InstancePerLifetimeScope();

            //卫星网
            builder.RegisterType<SatelliteService>().As<ISatelliteService>().InstancePerLifetimeScope();
            builder.RegisterType<SatelliteUserService>().As<ISatelliteUserService>().InstancePerLifetimeScope();
            builder.RegisterType<SatelliteRoleService>().As<ISatelliteRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<SatelliteMenuRoleService>().As<ISatelliteMenuRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<SatelliteMenuService>().As<ISatelliteMenuService>().InstancePerLifetimeScope();

            //临床上报
            builder.RegisterType<ClinicalTrialReportService>().As<IClinicalTrialReportService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// 这种依赖性注册的顺序执行
        /// </summary>
        public int Order
        {
            get { return 0; }
        }

    }

    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }

        }

        //static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        //{
        //    return RegistrationBuilder
        //        .ForDelegate((c, p) =>
        //        {
        //            //uncomment the code below if you want load settings per store only when you have two stores installed.
        //            //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
        //            //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

        //            //although it's better to connect to your database and execute the following SQL:
        //            //DELETE FROM [Setting] WHERE [StoreId] > 0
        //            return c.Resolve<ISettingService>().LoadSetting<TSettings>();
        //        })
        //        .InstancePerLifetimeScope()
        //        .CreateRegistration();
        //}

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }
}
