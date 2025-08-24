using AutoMapper;
using ManageSystem.Admin.Models.Log;
using ManageSystem.Admin.Models.Setting;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Admin.Models.Weixin;
using ManageSystem.Core.Domain.Weixin;
using ManageSystem.Core.Domain.Configuration;
using ManageSystem.Admin.Models.Configuration;


using ManageSystem.Admin.Models.Members;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Tasks;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Admin.Models.Products;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Admin.Models.Orders;
using ManageSystem.Core.Domain.Orders;
using ManageSystem.Admin.Models.Integrals;
using ManageSystem.Core.Domain.Integrals;
using ManageSystem.Admin.Models.Organism;
using ManageSystem.Core.Domain.Organism;
using ManageSystem.Admin.Models.Project;
using ManageSystem.Core.Domain.CRProjects;
using ManageSystem.Admin.Models.Survey;
using ManageSystem.Core.Domain.Survey;
using ManageSystem.Admin.Models.CRE;
using ManageSystem.Core.Domain.Cre;
using ManageSystem.Admin.Models.MicdataDistribution;
using ManageSystem.Core.Domain.MicdataDistribution;
using ManageSystem.Admin.Models.CRs;
using ManageSystem.Core.Domain.CRs;
using ManageSystem.Admin.Models.Team;
using ManageSystem.Core.Domain.Teams;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Admin.Models.Chart;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Admin.Models.Satellite;

namespace ManageSystem.Admin.Extensions
{
    public static class MappingExtensions
    {

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        #region 系统日志

        public static SystemLogModel ToModel(this SystemLog entity)
        {
            return entity.MapTo<SystemLog, SystemLogModel>();
        }

        public static SystemLog ToEntity(this SystemLogModel model)
        {
            return model.MapTo<SystemLogModel, SystemLog>();
        }

        public static SystemLog ToEntity(this SystemLogModel model, SystemLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 用户信息

        public static UserinfoModel ToModel(this Userinfo entity)
        {
            return entity.MapTo<Userinfo, UserinfoModel>();
        }

        public static Userinfo ToEntity(this UserinfoModel model)
        {
            return model.MapTo<UserinfoModel, Userinfo>();
        }

        public static Userinfo ToEntity(this UserinfoModel model, Userinfo destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 操作日志

        public static ActionLogModel ToModel(this ActionLog entity)
        {
            return entity.MapTo<ActionLog, ActionLogModel>();
        }

        public static ActionLog ToEntity(this ActionLogModel model)
        {
            return model.MapTo<ActionLogModel, ActionLog>();
        }

        public static ActionLog ToEntity(this ActionLogModel model, ActionLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 系统功能

        public static FunctionModel ToModel(this Function entity)
        {
            return entity.MapTo<Function, FunctionModel>();
        }

        public static Function ToEntity(this FunctionModel model)
        {
            return model.MapTo<FunctionModel, Function>();
        }

        public static Function ToEntity(this FunctionModel model, Function destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 角色管理

        public static RoleModel ToModel(this Role entity)
        {
            return entity.MapTo<Role, RoleModel>();
        }

        public static Role ToEntity(this RoleModel model)
        {
            return model.MapTo<RoleModel, Role>();
        }

        public static Role ToEntity(this RoleModel model, Role destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 角色和功能关联

        public static RoleFunctionModel ToModel(this RoleFunction entity)
        {
            return entity.MapTo<RoleFunction, RoleFunctionModel>();
        }

        public static RoleFunction ToEntity(this RoleFunctionModel model)
        {
            return model.MapTo<RoleFunctionModel, RoleFunction>();
        }

        public static RoleFunction ToEntity(this RoleFunctionModel model, RoleFunction destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 用户和角色关联

        public static UserRoleModel ToModel(this UserRole entity)
        {
            return entity.MapTo<UserRole, UserRoleModel>();
        }

        public static UserRole ToEntity(this UserRoleModel model)
        {
            return model.MapTo<UserRoleModel, UserRole>();
        }

        public static UserRole ToEntity(this UserRoleModel model, UserRole destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 文章分类

        public static ArticleTypeModel ToModel(this ArticleType entity)
        {
            return entity.MapTo<ArticleType, ArticleTypeModel>();
        }

        public static ArticleType ToEntity(this ArticleTypeModel model)
        {
            return model.MapTo<ArticleTypeModel, ArticleType>();
        }

        public static ArticleType ToEntity(this ArticleTypeModel model, ArticleType destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 文章

        public static ArticleModel ToModel(this Article entity)
        {
            return entity.MapTo<Article, ArticleModel>();
        }

        public static Article ToEntity(this ArticleModel model)
        {
            return model.MapTo<ArticleModel, Article>();
        }

        public static Article ToEntity(this ArticleModel model, Article destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 微信菜单

        public static WeixinMenuModel ToModel(this WeixinMenu entity)
        {
            return entity.MapTo<WeixinMenu, WeixinMenuModel>();
        }

        public static WeixinMenu ToEntity(this WeixinMenuModel model)
        {
            return model.MapTo<WeixinMenuModel, WeixinMenu>();
        }

        public static WeixinMenu ToEntity(this WeixinMenuModel model, WeixinMenu destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 系统配置

        public static SettingModel ToModel(this Setting entity)
        {
            return entity.MapTo<Setting, SettingModel>();
        }

        public static Setting ToEntity(this SettingModel model)
        {
            return model.MapTo<SettingModel, Setting>();
        }

        public static Setting ToEntity(this SettingModel model, Setting destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 关注用户

        public static WeixinUserModel ToModel(this WeixinUser entity)
        {
            return entity.MapTo<WeixinUser, WeixinUserModel>();
        }

        public static WeixinUser ToEntity(this WeixinUserModel model)
        {
            return model.MapTo<WeixinUserModel, WeixinUser>();
        }

        public static WeixinUser ToEntity(this WeixinUserModel model, WeixinUser destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 文章附件

        public static ArticleAttachmentModel ToModel(this ArticleAttachment entity)
        {
            return entity.MapTo<ArticleAttachment, ArticleAttachmentModel>();
        }

        public static ArticleAttachment ToEntity(this ArticleAttachmentModel model)
        {
            return model.MapTo<ArticleAttachmentModel, ArticleAttachment>();
        }

        public static ArticleAttachment ToEntity(this ArticleAttachmentModel model, ArticleAttachment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 文章查看管理

        public static ArticleViewModel ToModel(this ArticleView entity)
        {
            return entity.MapTo<ArticleView, ArticleViewModel>();
        }

        public static ArticleView ToEntity(this ArticleViewModel model)
        {
            return model.MapTo<ArticleViewModel, ArticleView>();
        }

        public static ArticleView ToEntity(this ArticleViewModel model, ArticleView destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 会员管理

        public static MemberModel ToModel(this Member entity)
        {
            return entity.MapTo<Member, MemberModel>();
        }

        public static Member ToEntity(this MemberModel model)
        {
            return model.MapTo<MemberModel, Member>();
        }

        public static Member ToEntity(this MemberModel model, Member destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 计划任务

        public static ScheduleTaskModel ToModel(this ScheduleTask entity)
        {
            return entity.MapTo<ScheduleTask, ScheduleTaskModel>();
        }

        public static ScheduleTask ToEntity(this ScheduleTaskModel model)
        {
            return model.MapTo<ScheduleTaskModel, ScheduleTask>();
        }

        public static ScheduleTask ToEntity(this ScheduleTaskModel model, ScheduleTask destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 信息动态类型

        public static MeetingTypeModel ToModel(this MeetingType entity)
        {
            return entity.MapTo<MeetingType, MeetingTypeModel>();
        }

        public static MeetingType ToEntity(this MeetingTypeModel model)
        {
            return model.MapTo<MeetingTypeModel, MeetingType>();
        }

        public static MeetingType ToEntity(this MeetingTypeModel model, MeetingType destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 信息动态

        public static MeetingModel ToModel(this Meeting entity)
        {
            return entity.MapTo<Meeting, MeetingModel>();
        }

        public static Meeting ToEntity(this MeetingModel model)
        {
            return model.MapTo<MeetingModel, Meeting>();
        }

        public static Meeting ToEntity(this MeetingModel model, Meeting destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 科研合作

        public static ResearchModel ToModel(this Research entity)
        {
            return entity.MapTo<Research, ResearchModel>();
        }

        public static Research ToEntity(this ResearchModel model)
        {
            return model.MapTo<ResearchModel, Research>();
        }

        public static Research ToEntity(this ResearchModel model, Research destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 科研合作类型

        public static ResearchTypeModel ToModel(this ResearchType entity)
        {
            return entity.MapTo<ResearchType, ResearchTypeModel>();
        }

        public static ResearchType ToEntity(this ResearchTypeModel model)
        {
            return model.MapTo<ResearchTypeModel, ResearchType>();
        }

        public static ResearchType ToEntity(this ResearchTypeModel model, ResearchType destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 科研合作申请

        public static ResearchApplyModel ToModel(this ResearchApply entity)
        {
            return entity.MapTo<ResearchApply, ResearchApplyModel>();
        }

        public static ResearchApply ToEntity(this ResearchApplyModel model)
        {
            return model.MapTo<ResearchApplyModel, ResearchApply>();
        }

        public static ResearchApply ToEntity(this ResearchApplyModel model, ResearchApply destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 科研合作查看记录

        public static ResearchViewModel ToModel(this ResearchView entity)
        {
            return entity.MapTo<ResearchView, ResearchViewModel>();
        }

        public static ResearchView ToEntity(this ResearchViewModel model)
        {
            return model.MapTo<ResearchViewModel, ResearchView>();
        }

        public static ResearchView ToEntity(this ResearchViewModel model, ResearchView destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 科研合作收藏记录

        public static ResearchCollectModel ToModel(this ResearchCollect entity)
        {
            return entity.MapTo<ResearchCollect, ResearchCollectModel>();
        }

        public static ResearchCollect ToEntity(this ResearchCollectModel model)
        {
            return model.MapTo<ResearchCollectModel, ResearchCollect>();
        }

        public static ResearchCollect ToEntity(this ResearchCollectModel model, ResearchCollect destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 科研合作用户评论

        public static ResearchCommentModel ToModel(this ResearchComment entity)
        {
            return entity.MapTo<ResearchComment, ResearchCommentModel>();
        }

        public static ResearchComment ToEntity(this ResearchCommentModel model)
        {
            return model.MapTo<ResearchCommentModel, ResearchComment>();
        }

        public static ResearchComment ToEntity(this ResearchCommentModel model, ResearchComment destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 信息动态申请

        public static MeetingApplyModel ToModel(this MeetingApply entity)
        {
            return entity.MapTo<MeetingApply, MeetingApplyModel>();
        }

        public static MeetingApply ToEntity(this MeetingApplyModel model)
        {
            return model.MapTo<MeetingApplyModel, MeetingApply>();
        }

        public static MeetingApply ToEntity(this MeetingApplyModel model, MeetingApply destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 信息动态查看记录

        public static MeetingViewModel ToModel(this MeetingView entity)
        {
            return entity.MapTo<MeetingView, MeetingViewModel>();
        }

        public static MeetingView ToEntity(this MeetingViewModel model)
        {
            return model.MapTo<MeetingViewModel, MeetingView>();
        }

        public static MeetingView ToEntity(this MeetingViewModel model, MeetingView destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 信息动态收藏记录

        public static MeetingCollectModel ToModel(this MeetingCollect entity)
        {
            return entity.MapTo<MeetingCollect, MeetingCollectModel>();
        }

        public static MeetingCollect ToEntity(this MeetingCollectModel model)
        {
            return model.MapTo<MeetingCollectModel, MeetingCollect>();
        }

        public static MeetingCollect ToEntity(this MeetingCollectModel model, MeetingCollect destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 信息动态用户评论

        public static MeetingCommentModel ToModel(this MeetingComment entity)
        {
            return entity.MapTo<MeetingComment, MeetingCommentModel>();
        }

        public static MeetingComment ToEntity(this MeetingCommentModel model)
        {
            return model.MapTo<MeetingCommentModel, MeetingComment>();
        }

        public static MeetingComment ToEntity(this MeetingCommentModel model, MeetingComment destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医院

        public static HospitalModel ToModel(this Hospital entity)
        {
            return entity.MapTo<Hospital, HospitalModel>();
        }

        public static Hospital ToEntity(this HospitalModel model)
        {
            return model.MapTo<HospitalModel, Hospital>();
        }

        public static Hospital ToEntity(this HospitalModel model, Hospital destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 细菌类型

        public static BacteriaTypeModel ToModel(this BacteriaType entity)
        {
            return entity.MapTo<BacteriaType, BacteriaTypeModel>();
        }

        public static BacteriaType ToEntity(this BacteriaTypeModel model)
        {
            return model.MapTo<BacteriaTypeModel, BacteriaType>();
        }

        public static BacteriaType ToEntity(this BacteriaTypeModel model, BacteriaType destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 标本

        public static SpecimenModel ToModel(this Specimen entity)
        {
            return entity.MapTo<Specimen, SpecimenModel>();
        }

        public static Specimen ToEntity(this SpecimenModel model)
        {
            return model.MapTo<SpecimenModel, Specimen>();
        }

        public static Specimen ToEntity(this SpecimenModel model, Specimen destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医院科室

        public static HospitalDepartmentModel ToModel(this HospitalDepartment entity)
        {
            return entity.MapTo<HospitalDepartment, HospitalDepartmentModel>();
        }

        public static HospitalDepartment ToEntity(this HospitalDepartmentModel model)
        {
            return model.MapTo<HospitalDepartmentModel, HospitalDepartment>();
        }

        public static HospitalDepartment ToEntity(this HospitalDepartmentModel model, HospitalDepartment destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医学数据

        public static MedicalDataModel ToModel(this MedicalData entity)
        {
            return entity.MapTo<MedicalData, MedicalDataModel>();
        }

        public static MedicalData ToEntity(this MedicalDataModel model)
        {
            return model.MapTo<MedicalDataModel, MedicalData>();
        }

        public static MedicalData ToEntity(this MedicalDataModel model, MedicalData destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 医学数据类型

        public static MedicalDataProjectModel ToModel(this MedicalDataProject entity)
        {
            return entity.MapTo<MedicalDataProject, MedicalDataProjectModel>();
        }

        public static MedicalDataProject ToEntity(this MedicalDataProjectModel model)
        {
            return model.MapTo<MedicalDataProjectModel, MedicalDataProject>();
        }

        public static MedicalDataProject ToEntity(this MedicalDataProjectModel model, MedicalDataProject destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医学数据项

        public static MedicalDataItemModel ToModel(this MedicalDataItem entity)
        {
            return entity.MapTo<MedicalDataItem, MedicalDataItemModel>();
        }

        public static MedicalDataItem ToEntity(this MedicalDataItemModel model)
        {
            return model.MapTo<MedicalDataItemModel, MedicalDataItem>();
        }

        public static MedicalDataItem ToEntity(this MedicalDataItemModel model, MedicalDataItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 医生职称

        public static DoctorTitleModel ToModel(this DoctorTitle entity)
        {
            return entity.MapTo<DoctorTitle, DoctorTitleModel>();
        }

        public static DoctorTitle ToEntity(this DoctorTitleModel model)
        {
            return model.MapTo<DoctorTitleModel, DoctorTitle>();
        }

        public static DoctorTitle ToEntity(this DoctorTitleModel model, DoctorTitle destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 会员收货地址

        public static MemberAddressModel ToModel(this MemberAddress entity)
        {
            return entity.MapTo<MemberAddress, MemberAddressModel>();
        }

        public static MemberAddress ToEntity(this MemberAddressModel model)
        {
            return model.MapTo<MemberAddressModel, MemberAddress>();
        }

        public static MemberAddress ToEntity(this MemberAddressModel model, MemberAddress destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 商品分类

        public static ProductCategoryModel ToModel(this ProductCategory entity)
        {
            return entity.MapTo<ProductCategory, ProductCategoryModel>();
        }

        public static ProductCategory ToEntity(this ProductCategoryModel model)
        {
            return model.MapTo<ProductCategoryModel, ProductCategory>();
        }

        public static ProductCategory ToEntity(this ProductCategoryModel model, ProductCategory destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 商品品牌

        public static ProductBrandModel ToModel(this ProductBrand entity)
        {
            return entity.MapTo<ProductBrand, ProductBrandModel>();
        }

        public static ProductBrand ToEntity(this ProductBrandModel model)
        {
            return model.MapTo<ProductBrandModel, ProductBrand>();
        }

        public static ProductBrand ToEntity(this ProductBrandModel model, ProductBrand destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 商品图片

        public static ProductImageModel ToModel(this ProductImage entity)
        {
            return entity.MapTo<ProductImage, ProductImageModel>();
        }

        public static ProductImage ToEntity(this ProductImageModel model)
        {
            return model.MapTo<ProductImageModel, ProductImage>();
        }

        public static ProductImage ToEntity(this ProductImageModel model, ProductImage destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 商品

        public static ProductModel ToModel(this Product entity)
        {
            return entity.MapTo<Product, ProductModel>();
        }

        public static Product ToEntity(this ProductModel model)
        {
            return model.MapTo<ProductModel, Product>();
        }

        public static Product ToEntity(this ProductModel model, Product destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 用户购物车

        public static MemberCartModel ToModel(this MemberCart entity)
        {
            return entity.MapTo<MemberCart, MemberCartModel>();
        }

        public static MemberCart ToEntity(this MemberCartModel model)
        {
            return model.MapTo<MemberCartModel, MemberCart>();
        }

        public static MemberCart ToEntity(this MemberCartModel model, MemberCart destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 订单

        public static OrderModel ToModel(this Order entity)
        {
            return entity.MapTo<Order, OrderModel>();
        }

        public static Order ToEntity(this OrderModel model)
        {
            return model.MapTo<OrderModel, Order>();
        }

        public static Order ToEntity(this OrderModel model, Order destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 订单明细

        public static OrderItemModel ToModel(this OrderItem entity)
        {
            return entity.MapTo<OrderItem, OrderItemModel>();
        }

        public static OrderItem ToEntity(this OrderItemModel model)
        {
            return model.MapTo<OrderItemModel, OrderItem>();
        }

        public static OrderItem ToEntity(this OrderItemModel model, OrderItem destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 订单日志

        public static OrderLogModel ToModel(this OrderLog entity)
        {
            return entity.MapTo<OrderLog, OrderLogModel>();
        }

        public static OrderLog ToEntity(this OrderLogModel model)
        {
            return model.MapTo<OrderLogModel, OrderLog>();
        }

        public static OrderLog ToEntity(this OrderLogModel model, OrderLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 订单收货信息

        public static OrderAddressModel ToModel(this OrderAddress entity)
        {
            return entity.MapTo<OrderAddress, OrderAddressModel>();
        }

        public static OrderAddress ToEntity(this OrderAddressModel model)
        {
            return model.MapTo<OrderAddressModel, OrderAddress>();
        }

        public static OrderAddress ToEntity(this OrderAddressModel model, OrderAddress destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 用户积分使用记录

        public static MemberIntegralLogModel ToModel(this MemberIntegralLog entity)
        {
            return entity.MapTo<MemberIntegralLog, MemberIntegralLogModel>();
        }

        public static MemberIntegralLog ToEntity(this MemberIntegralLogModel model)
        {
            return model.MapTo<MemberIntegralLogModel, MemberIntegralLog>();
        }

        public static MemberIntegralLog ToEntity(this MemberIntegralLogModel model, MemberIntegralLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 积分制度设置

        public static IntegralSettingModel ToModel(this IntegralSetting entity)
        {
            return entity.MapTo<IntegralSetting, IntegralSettingModel>();
        }

        public static IntegralSetting ToEntity(this IntegralSettingModel model)
        {
            return model.MapTo<IntegralSettingModel, IntegralSetting>();
        }

        public static IntegralSetting ToEntity(this IntegralSettingModel model, IntegralSetting destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 会认证申请信息

        public static MemberAttestationModel ToModel(this MemberAttestation entity)
        {
            return entity.MapTo<MemberAttestation, MemberAttestationModel>();
        }

        public static MemberAttestation ToEntity(this MemberAttestationModel model)
        {
            return model.MapTo<MemberAttestationModel, MemberAttestation>();
        }

        public static MemberAttestation ToEntity(this MemberAttestationModel model, MemberAttestation destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 建议反馈

        public static FeedbackModel ToModel(this Feedback entity)
        {
            return entity.MapTo<Feedback, FeedbackModel>();
        }

        public static Feedback ToEntity(this FeedbackModel model)
        {
            return model.MapTo<FeedbackModel, Feedback>();
        }

        public static Feedback ToEntity(this FeedbackModel model, Feedback destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医学数据细菌

        public static MedicalOrganismModel ToModel(this MedicalOrganism entity)
        {
            return entity.MapTo<MedicalOrganism, MedicalOrganismModel>();
        }

        public static MedicalOrganism ToEntity(this MedicalOrganismModel model)
        {
            return model.MapTo<MedicalOrganismModel, MedicalOrganism>();
        }

        public static MedicalOrganism ToEntity(this MedicalOrganismModel model, MedicalOrganism destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医学数据抗生素

        public static MedicalAntibioticModel ToModel(this MedicalAntibiotic entity)
        {
            return entity.MapTo<MedicalAntibiotic, MedicalAntibioticModel>();
        }

        public static MedicalAntibiotic ToEntity(this MedicalAntibioticModel model)
        {
            return model.MapTo<MedicalAntibioticModel, MedicalAntibiotic>();
        }

        public static MedicalAntibiotic ToEntity(this MedicalAntibioticModel model, MedicalAntibiotic destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医学数据 细菌与抗生素规则

        public static MedicalAntibioticRuleModel ToModel(this MedicalAntibioticRule entity)
        {
            return entity.MapTo<MedicalAntibioticRule, MedicalAntibioticRuleModel>();
        }

        public static MedicalAntibioticRule ToEntity(this MedicalAntibioticRuleModel model)
        {
            return model.MapTo<MedicalAntibioticRuleModel, MedicalAntibioticRule>();
        }

        public static MedicalAntibioticRule ToEntity(this MedicalAntibioticRuleModel model, MedicalAntibioticRule destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region 医学信息 医院科室配置
        public static HospitalWardLocationModel ToModel(this HospitalWardLocation entity)
        {
            return entity.MapTo<HospitalWardLocation, HospitalWardLocationModel>();
        }
        public static HospitalWardLocation ToEntity(this HospitalWardLocationModel model)
        {
            return model.MapTo<HospitalWardLocationModel, HospitalWardLocation>();
        }
        #endregion
        #region 细菌

        public static BacteriaDetailedDataModel ToModel(this BacteriaDetailedData entity)
        {
            return entity.MapTo<BacteriaDetailedData, BacteriaDetailedDataModel>();
        }

        public static BacteriaDetailedData ToEntity(this BacteriaDetailedDataModel model)
        {
            return model.MapTo<BacteriaDetailedDataModel, BacteriaDetailedData>();
        }

        public static BacteriaDetailedData ToEntity(this BacteriaDetailedDataModel model, BacteriaDetailedData destination)
        {
            return model.MapTo(destination);
        }

        #endregion


        #region 项目

        public static ProjectDataModel ToModel(this CRProject entity)
        {
            return entity.MapTo<CRProject, ProjectDataModel>();
        }

        public static CRProject ToEntity(this ProjectDataModel model)
        {
            return model.MapTo<ProjectDataModel, CRProject>();
        }

        public static CRProject ToEntity(this ProjectDataModel model, CRProject destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region 项目日志

        public static CRProjectLogModel ToModel(this CRProjectLog entity)
        {
            return entity.MapTo<CRProjectLog, CRProjectLogModel>();
        }

        public static CRProjectLog ToEntity(this CRProjectLogModel model)
        {
            return model.MapTo<CRProjectLogModel, CRProjectLog>();
        }

        public static CRProjectLog ToEntity(this CRProjectLogModel model, CRProjectLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        //自动生成代码标识位



        //问卷调查
        #region 问卷调查

        public static SurveySurveyModel ToModel(this Survey_Survey entity)
        {
            return entity.MapTo<Survey_Survey, SurveySurveyModel>();
        }

        public static Survey_Survey ToEntity(this SurveySurveyModel model)
        {
            return model.MapTo<SurveySurveyModel, Survey_Survey>();
        }

        public static Survey_Survey ToEntity(this SurveySurveyModel model, Survey_Survey destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        //问卷调查
        #region 问卷调查

        public static SurveyRecordModel ToModel(this Survey_Record entity)
        {
            return entity.MapTo<Survey_Record, SurveyRecordModel>();
        }

        public static Survey_Record ToEntity(this SurveyRecordModel model)
        {
            return model.MapTo<SurveyRecordModel, Survey_Record>();
        }

        public static Survey_Record ToEntity(this SurveyRecordModel model, Survey_Record destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        //问卷调查
        #region 问卷调查

        public static SurveySubjectModel ToModel(this Survey_Subject entity)
        {
            return entity.MapTo<Survey_Subject, SurveySubjectModel>();
        }

        public static Survey_Subject ToEntity(this SurveySubjectModel model)
        {
            return model.MapTo<SurveySubjectModel, Survey_Subject>();
        }

        public static Survey_Subject ToEntity(this SurveySubjectModel model, Survey_Subject destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region CRE

        public static ToconfigureModel ToModel(this Toconfigure entity)
        {
            return entity.MapTo<Toconfigure, ToconfigureModel>();
        }

        public static Toconfigure ToEntity(this ToconfigureModel model)
        {
            return model.MapTo<ToconfigureModel, Toconfigure>();
        }

        public static Toconfigure ToEntity(this ToconfigureModel model, Toconfigure destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        public static ddGerm ToEntity(this ddGermModel model)
        {
            return model.MapTo<ddGermModel, ddGerm>();
        }
        public static ddGermModel ToModel(this ddGerm entity)
        {
            return entity.MapTo<ddGerm, ddGermModel>();
        }   
        public static ddGerm ToEntity(this ddGermModel model, ddGerm destination)
        {
            return model.MapTo(destination);
        }

        #region CR

        public static CRDataModel ToModel(this CRData entity)
        {
            return entity.MapTo<CRData, CRDataModel>();
        }

        public static CRData ToEntity(this CRDataModel model)
        {
            return model.MapTo<CRDataModel, CRData>();
        }

        public static CRData ToEntity(this CRDataModel model, CRData destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        public static TeamModel ToModel(this Team entity)
        {
            return entity.MapTo<Team, TeamModel>();
        }

        public static Team ToEntity(this TeamModel model)
        {
            return model.MapTo<TeamModel, Team>();
        }

        public static Team ToEntity(this TeamModel model, Team destination)
        {
            return model.MapTo(destination);
        }

        #region 卫星网

        public static SatelliteMenuRole ToEntity(this SatelliteMenuRoleModel model)
        {
            return model.MapTo<SatelliteMenuRoleModel, SatelliteMenuRole>();
        }

        public static SatelliteMenuRole ToEntity(this SatelliteMenuRoleModel model, SatelliteMenuRole entity)
        {
            return model.MapTo(entity);
        }
        public static SatelliteMenu ToEntity(this SatelliteMenuModel model )
        {
            return model.MapTo<SatelliteMenuModel, SatelliteMenu>();
        }

        public static SatelliteMenu ToEntity(this SatelliteMenuModel model, SatelliteMenu entity)
        {
            return model.MapTo(entity);
        }

        public static SatelliteUser ToEntity(this SatelliteUserModel model)
        {
            return model.MapTo<SatelliteUserModel, SatelliteUser>();
        }

        public static SatelliteUser ToEntity(this SatelliteUserModel model, SatelliteUser entity)
        {
            return model.MapTo(entity);
        }

        public static SatelliteMenuModel ToModel(this SatelliteMenu entity)
        {
            return entity.MapTo<SatelliteMenu, SatelliteMenuModel>();
        }
        public static SatelliteMenuRoleModel ToModel(this SatelliteMenuRole entity)
        {
            return entity.MapTo<SatelliteMenuRole, SatelliteMenuRoleModel>();
        }
        public static SatelliteUserModel ToModel(this SatelliteUser entity)
        {
            return entity.MapTo<SatelliteUser, SatelliteUserModel>();
        }

        #endregion
        //public static SHBarChartModel ToModel(this Chart_SHBarChart entity)
        //{
        //    return entity.MapTo<Chart_SHBarChart, SHBarChartModel>();
        //}

        //public static Chart_SHBarChart ToEntity(this SHBarChartModel model)
        //{
        //    return model.MapTo<SHBarChartModel, Chart_SHBarChart>();
        //}

        //public static Chart_SHBarChart ToEntity(this SHBarChartModel model, Chart_SHBarChart destination)
        //{
        //    return model.MapTo(destination);
        //}




















































    }
}