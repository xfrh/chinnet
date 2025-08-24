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
    public class AutoMapperRegistrar
    {
        /// <summary>
        /// 注册映射
        /// </summary>
        public static void Register()
        {
            Mapper.CreateMap<Member, MemberModel>();
            Mapper.CreateMap<MemberModel, Member>();
            Mapper.CreateMap<ArticleType, ArticleTypeModel>();
            Mapper.CreateMap<ArticleTypeModel, ArticleType>();
            Mapper.CreateMap<Article, ArticleModel>();
            Mapper.CreateMap<ArticleModel, Article>();
            Mapper.CreateMap<MeetingType, MeetingTypeModel>();
            Mapper.CreateMap<MeetingTypeModel, MeetingType>();
            Mapper.CreateMap<Meeting, MeetingModel>();
            Mapper.CreateMap<MeetingModel, Meeting>();
            Mapper.CreateMap<MeetingApply, MeetingApplyModel>();
            Mapper.CreateMap<MeetingApplyModel, MeetingApply>();
            Mapper.CreateMap<MeetingComment, MeetingCommentModel>();
            Mapper.CreateMap<MeetingCommentModel, MeetingComment>();
            Mapper.CreateMap<Research, ResearchModel>();
            Mapper.CreateMap<ResearchModel, Research>();
            Mapper.CreateMap<ResearchType, ResearchTypeModel>();
            Mapper.CreateMap<ResearchTypeModel, ResearchType>();
            Mapper.CreateMap<ResearchComment, ResearchCommentModel>();
            Mapper.CreateMap<ResearchCommentModel, ResearchComment>();
            Mapper.CreateMap<ResearchApply, ResearchApplyModel>();
            Mapper.CreateMap<ResearchApplyModel, ResearchApply>();
            Mapper.CreateMap<MedicalData, MedicalDataModel>();
            Mapper.CreateMap<MedicalDataModel, MedicalData>();
            Mapper.CreateMap<MedicalDataItem, MedicalDataItemModel>();
            Mapper.CreateMap<MedicalDataItemModel, MedicalDataItem>();
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
            Mapper.CreateMap<MemberAddress, MemberAddressModel>();
            Mapper.CreateMap<MemberAddressModel, MemberAddress>();

            Mapper.CreateMap<Order, OrderModel>();
            Mapper.CreateMap<OrderModel, Order>();
            Mapper.CreateMap<OrderItem, OrderItemModel>();
            Mapper.CreateMap<OrderItemModel, OrderItem>();
            Mapper.CreateMap<OrderLog, OrderLogModel>();
            Mapper.CreateMap<OrderLogModel, OrderLog>();
            Mapper.CreateMap<OrderAddress, OrderAddressModel>();
            Mapper.CreateMap<OrderAddressModel, OrderAddress>();
            Mapper.CreateMap<MemberAttestation, MemberAttestationModel>();
            Mapper.CreateMap<MemberAttestationModel, MemberAttestation>();
            Mapper.CreateMap<Feedback, FeedbackModel>();
            Mapper.CreateMap<FeedbackModel, Feedback>();
            Mapper.CreateMap<BacteriaDetailedDataModel, BacteriaDetailedData>();
            Mapper.CreateMap<BacteriaDetailedData, BacteriaDetailedDataModel>();


            Mapper.CreateMap<CRProject, CRProjectModel>();
            Mapper.CreateMap<CRProjectModel, CRProject>();

            Mapper.CreateMap<MedicalDataProject, MedicalDataProjectModel>();
            Mapper.CreateMap<MedicalDataProjectModel, MedicalDataProject>();

            Mapper.CreateMap<CRData, CRDataModel>();
            Mapper.CreateMap<CRDataModel, CRData>();



            //自动生成代码标识位



        }
    }
}