using AutoMapper;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Orders;
using ManageSystem.Core.Domain.Organism;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Core.Domain.CRProjects;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Web.Models.Articles;
using ManageSystem.Web.Models.Medicine;
using ManageSystem.Web.Models.Meetings;
using ManageSystem.Web.Models.Members;
using ManageSystem.Web.Models.Orders;
using ManageSystem.Web.Models.Organism;
using ManageSystem.Web.Models.Products;
using ManageSystem.Web.Models.Project;
using ManageSystem.Web.Models.Researches;
using ManageSystem.Core.Domain.CRs;

namespace ManageSystem.Web.Extensions
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


        #region CR复敏项目

        public static CRProjectModel ToModel(this  CRProject entity)
        {
            return entity.MapTo<CRProject, CRProjectModel>();
        }
        
        public static CRProject ToEntity(this CRProjectModel model)
        {
            return model.MapTo<CRProjectModel, CRProject>();
        }

        public static CRProject ToEntity(this CRProjectModel model, CRProject destination)
        {
            return model.MapTo(destination);
        }
        #endregion
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

    }
}