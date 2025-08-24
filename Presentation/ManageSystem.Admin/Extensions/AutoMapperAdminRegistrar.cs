using AutoMapper;
using ManageSystem.Admin.Models.Log;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Admin.Models.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Core.Domain.Weixin;
using ManageSystem.Admin.Models.Weixin;
using ManageSystem.Admin.Models.Configuration;
using ManageSystem.Core.Domain.Configuration;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Admin.Models.Members;
using ManageSystem.Core.Domain.Tasks;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Admin.Models.Products;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Admin.Models.Orders;
using ManageSystem.Core.Domain.Orders;
using ManageSystem.Core.Domain.Integrals;
using ManageSystem.Admin.Models.Integrals;
using ManageSystem.Admin.Models.Organism;
using ManageSystem.Core.Domain.Organism;
using ManageSystem.Core.Domain.CRProjects;
using ManageSystem.Admin.Models.Project;
using ManageSystem.Admin.Models.Survey;
using ManageSystem.Core.Domain.Survey;
using ManageSystem.Core.Domain.CRs;
using ManageSystem.Admin.Models.CRs;
using ManageSystem.Core.Domain.Teams;
using ManageSystem.Admin.Models.Team;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Admin.Models.Chart;
using ManageSystem.Admin.Models.Satellite;
using ManageSystem.Core.Domain.Sate;

namespace ManageSystem.Admin.Extensions
{
    /// <summary>
    ///   AutoMapper  映射 
    /// </summary>
    public class AutoMapperAdminRegistrar
    {
        /// <summary>
        /// 注册映射
        /// </summary>
        public static void Register()
        {

            Mapper.CreateMap<BacteriaDetailedDataModel, BacteriaDetailedData>();
            Mapper.CreateMap<BacteriaDetailedData, BacteriaDetailedDataModel>();
            Mapper.CreateMap<SystemLog, SystemLogModel>();
            Mapper.CreateMap<SystemLogModel, SystemLog>();
            Mapper.CreateMap<Userinfo, UserinfoModel>();
            Mapper.CreateMap<UserinfoModel, Userinfo>();
            Mapper.CreateMap<ActionLog, ActionLogModel>();
            Mapper.CreateMap<ActionLogModel, ActionLog>();
            Mapper.CreateMap<Function, FunctionModel>();
            Mapper.CreateMap<FunctionModel, Function>();
            Mapper.CreateMap<Role, RoleModel>();
            Mapper.CreateMap<RoleModel, Role>();
            Mapper.CreateMap<RoleFunction, RoleFunctionModel>();
            Mapper.CreateMap<RoleFunctionModel, RoleFunction>();
            Mapper.CreateMap<UserRole, UserRoleModel>();
            Mapper.CreateMap<UserRoleModel, UserRole>();
            Mapper.CreateMap<Userinfo, UserinfoModel>();
            Mapper.CreateMap<UserinfoModel, Userinfo>();
            Mapper.CreateMap<Userinfo, UserinfoModel>();
            Mapper.CreateMap<UserinfoModel, Userinfo>();
            Mapper.CreateMap<ArticleType, ArticleTypeModel>();
            Mapper.CreateMap<ArticleTypeModel, ArticleType>();
            Mapper.CreateMap<Article, ArticleModel>();
            Mapper.CreateMap<ArticleModel, Article>();
            Mapper.CreateMap<WeixinMenu, WeixinMenuModel>();
            Mapper.CreateMap<WeixinMenuModel, WeixinMenu>();
            Mapper.CreateMap<Setting, SettingModel>();
            Mapper.CreateMap<SettingModel, Setting>();
            Mapper.CreateMap<WeixinUser, WeixinUserModel>();
            Mapper.CreateMap<WeixinUserModel, WeixinUser>();
            Mapper.CreateMap<ArticleAttachment, ArticleAttachmentModel>();
            Mapper.CreateMap<ArticleAttachmentModel, ArticleAttachment>();
            Mapper.CreateMap<ArticleApply, ArticleApplyModel>();
            Mapper.CreateMap<ArticleApplyModel, ArticleApply>();
            Mapper.CreateMap<ArticleView, ArticleViewModel>();
            Mapper.CreateMap<ArticleViewModel, ArticleView>();
            Mapper.CreateMap<Member, MemberModel>();
            Mapper.CreateMap<MemberModel, Member>();
            Mapper.CreateMap<ScheduleTaskModel, ScheduleTask>();
            Mapper.CreateMap<ScheduleTask, ScheduleTaskModel>();
            Mapper.CreateMap<MeetingType, MeetingTypeModel>();
            Mapper.CreateMap<MeetingTypeModel, MeetingType>();
            Mapper.CreateMap<Meeting, MeetingModel>();
            Mapper.CreateMap<MeetingModel, Meeting>();
            Mapper.CreateMap<MeetingApply, MeetingApplyModel>();
            Mapper.CreateMap<MeetingApplyModel, MeetingApply>();
            Mapper.CreateMap<MeetingView, MeetingViewModel>();
            Mapper.CreateMap<MeetingViewModel, MeetingView>();
            Mapper.CreateMap<MeetingCollect, MeetingCollectModel>();
            Mapper.CreateMap<MeetingCollectModel, MeetingCollect>();
            Mapper.CreateMap<MeetingComment, MeetingCommentModel>();
            Mapper.CreateMap<MeetingCommentModel, MeetingComment>();
            Mapper.CreateMap<Research, ResearchModel>();
            Mapper.CreateMap<ResearchModel, Research>();
            Mapper.CreateMap<ResearchType, ResearchTypeModel>();
            Mapper.CreateMap<ResearchTypeModel, ResearchType>();
            Mapper.CreateMap<ResearchApply, ResearchApplyModel>();
            Mapper.CreateMap<ResearchApplyModel, ResearchApply>();
            Mapper.CreateMap<ResearchView, ResearchViewModel>();
            Mapper.CreateMap<ResearchViewModel, ResearchView>();
            Mapper.CreateMap<ResearchCollect, ResearchCollectModel>();
            Mapper.CreateMap<ResearchCollectModel, ResearchCollect>();
            Mapper.CreateMap<ResearchComment, ResearchCommentModel>();
            Mapper.CreateMap<ResearchCommentModel, ResearchComment>();
            Mapper.CreateMap<Hospital, HospitalModel>();
            Mapper.CreateMap<HospitalModel, Hospital>();
            Mapper.CreateMap<BacteriaType, BacteriaTypeModel>();
            Mapper.CreateMap<BacteriaTypeModel, BacteriaType>();
            Mapper.CreateMap<Specimen, SpecimenModel>();
            Mapper.CreateMap<SpecimenModel, Specimen>();
            Mapper.CreateMap<HospitalDepartment, HospitalDepartmentModel>();
            Mapper.CreateMap<HospitalDepartmentModel, HospitalDepartment>();
            Mapper.CreateMap<MedicalData, MedicalDataModel>();
            Mapper.CreateMap<MedicalDataModel, MedicalData>();
            Mapper.CreateMap<MedicalDataItem, MedicalDataItemModel>();
            Mapper.CreateMap<MedicalDataItemModel, MedicalDataItem>();
            Mapper.CreateMap<DoctorTitle, DoctorTitleModel>();
            Mapper.CreateMap<DoctorTitleModel, DoctorTitle>();
            Mapper.CreateMap<MemberAddress, MemberAddressModel>();
            Mapper.CreateMap<MemberAddressModel, MemberAddress>();
            Mapper.CreateMap<ProductCategory, ProductCategoryModel>();
            Mapper.CreateMap<ProductCategoryModel, ProductCategory>();
            Mapper.CreateMap<ProductBrand, ProductBrandModel>();
            Mapper.CreateMap<ProductBrandModel, ProductBrand>();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<ProductModel, Product>();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<ProductModel, Product>();
            Mapper.CreateMap<ProductImage, ProductImageModel>();
            Mapper.CreateMap<ProductImageModel, ProductImage>();
            Mapper.CreateMap<MemberCart, MemberCartModel>();
            Mapper.CreateMap<MemberCartModel, MemberCart>();
            Mapper.CreateMap<Order, OrderModel>();
            Mapper.CreateMap<OrderModel, Order>();
            Mapper.CreateMap<OrderItem, OrderItemModel>();
            Mapper.CreateMap<OrderItemModel, OrderItem>();
            Mapper.CreateMap<OrderLog, OrderLogModel>();
            Mapper.CreateMap<OrderLogModel, OrderLog>();
            Mapper.CreateMap<OrderAddress, OrderAddressModel>();
            Mapper.CreateMap<OrderAddressModel, OrderAddress>();
            Mapper.CreateMap<MemberIntegralLog, MemberIntegralLogModel>();
            Mapper.CreateMap<MemberIntegralLogModel, MemberIntegralLog>();
            Mapper.CreateMap<IntegralSetting, IntegralSettingModel>();
            Mapper.CreateMap<IntegralSettingModel, IntegralSetting>();
            Mapper.CreateMap<MemberAttestation, MemberAttestationModel>();
            Mapper.CreateMap<MemberAttestationModel, MemberAttestation>();
            Mapper.CreateMap<Feedback, FeedbackModel>();
            Mapper.CreateMap<FeedbackModel, Feedback>();
            Mapper.CreateMap<MedicalOrganism, MedicalOrganismModel>();
            Mapper.CreateMap<MedicalOrganismModel, MedicalOrganism>();
            Mapper.CreateMap<MedicalAntibiotic, MedicalAntibioticModel>();
            Mapper.CreateMap<MedicalAntibioticModel, MedicalAntibiotic>();
            Mapper.CreateMap<MedicalAntibioticRule, MedicalAntibioticRuleModel>();
            Mapper.CreateMap<MedicalAntibioticRuleModel, MedicalAntibioticRule>();
            Mapper.CreateMap<MedicalDataItem, MedicalDataItemModel>();
            Mapper.CreateMap<MedicalDataItemModel, MedicalDataItem>();

            Mapper.CreateMap<CRProject, ProjectDataModel>();
            Mapper.CreateMap<ProjectDataModel, CRProject>();

            Mapper.CreateMap<CRProjectLog, CRProjectLogModel>();
            Mapper.CreateMap<CRProjectLogModel, CRProjectLog>();

            //自动生成代码标识位

            //多中心研究医院
            Mapper.CreateMap<ProjectHospital, ProjectHospitalModel>();
            Mapper.CreateMap<ProjectHospitalModel, ProjectHospital>();

            //问卷调查
            Mapper.CreateMap<Survey_Survey, SurveySurveyModel>();
            Mapper.CreateMap<SurveySurveyModel, Survey_Survey>();

            Mapper.CreateMap<Survey_Record, SurveyRecordModel>();
            Mapper.CreateMap<SurveyRecordModel, Survey_Record>();

            Mapper.CreateMap<Survey_Subject, SurveySubjectModel>();
            Mapper.CreateMap<SurveySubjectModel, Survey_Subject>();

            Mapper.CreateMap<MedicalDataProject, MedicalDataProjectModel>();
            Mapper.CreateMap<MedicalDataProjectModel, MedicalDataProject>();

            // 科室配置
            Mapper.CreateMap<HospitalWardLocation, HospitalWardLocationModel>()
                .ForMember(src => src.HospitalID, tag => tag.MapFrom(x => x.HospitalId))
                .ForMember(src => src.Department, tag => tag.MapFrom(x => x.Department_EN))
                .ForMember(src => src.LocationType, tag => tag.MapFrom(x => x.Location_Type));

            Mapper.CreateMap<HospitalWardLocationModel, HospitalWardLocation>()
                .ForMember(src => src.HospitalId, tag => tag.MapFrom(x => x.HospitalID))
                .ForMember(src => src.Department_EN, tag => tag.MapFrom(x => x.Department))
                .ForMember(src => src.Location_Type, tag => tag.MapFrom(x => x.LocationType));


            //C替加环素配置
            Mapper.CreateMap<CRData, CRDataModel>();
            Mapper.CreateMap<CRDataModel, CRData>();

            //Mapper.CreateMap<MedicalDataProject, MedicalDataProjectModel>();
            //成员单位
            Mapper.CreateMap<TeamModel, Team> ();
            Mapper.CreateMap<Team, TeamModel>();
            Mapper.CreateMap<SatelliteUserModel, SatelliteUser>();
            Mapper.CreateMap<SatelliteUser, SatelliteUserModel>();
            Mapper.CreateMap<SatelliteMenuModel, SatelliteMenu>();
            Mapper.CreateMap<SatelliteMenu, SatelliteMenuModel>();
            Mapper.CreateMap<SatelliteMenuRoleModel, SatelliteMenuRole>();
            Mapper.CreateMap<SatelliteMenuRole, SatelliteMenuRoleModel>();

        }
    }
}