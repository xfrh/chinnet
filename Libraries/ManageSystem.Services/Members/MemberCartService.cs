using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core;
using ManageSystem.Data;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Services.Log;
using ManageSystem.Services.Products;
using ManageSystem.Core.Domain.Products;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.Members
{
	/// <summary>
	/// 操作类 ，数据库表名：MemberCart 
	/// </summary>
	public partial class MemberCartService :  BaseService<MemberCart>, IMemberCartService
	{
        private readonly IActionLogService ActionLogService;
        private readonly IProductService ProductService;

        public MemberCartService(IRepository<MemberCart> repository,
                       IActionLogService actionLogService,
                            IProductService _productService) : base(repository)
		{
            this.ActionLogService = actionLogService;
            this.ProductService = _productService;
        }

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="entity">购物车数据实体</param>
        /// <param name="account">操作用户</param>
        /// <param name="source">日志数据来源</param>
        /// <returns></returns>
        public bool AddCart(MemberCart entity, Account account, ActionSource source)
        {
            if (entity == null) throw new Exception("保存数据不能为空");

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //数据检测
                    if (entity.ProductId <= 0) throw new Exception("商品不存在");
                    var product = this.ProductService.QueryEntity(entity.ProductId);

                    if (product == null || product.Id <= 0) throw new Exception("商品不存在");
                    if (product.Status == (int)ProductStatusEnum.Soldout) throw new Exception("商品已下架");

                    //删除该商品之前的数据
                    this.Delete(m => m.MemberId == entity.MemberId && m.ProductId == entity.ProductId);

                    //插入数据
                    entity.ProductId = product.Id;
                    entity.ProductName = product.Name;
                    entity.MarketPrice = product.MarketPrice;
                    this.Insert(entity);
                    if (entity.Id <= 0) throw new Exception("添加购物车失败");

                    //添加操作日志
                    this.ActionLogService.Insert(ActionType.Create, source, account.Id, account.Name + "（" + account.LoginId + "）", "添加购物车，商品id：" + entity.ProductId, entity.SerializeObject());

                    tran.Commit();
                }

                con.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据用户id获取对应的购物车数据
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        public List<MemberCart> QueryByMember(long memberId)
        {
            return this.Query(m => m.Mark > 0 && m.MemberId == memberId).ToList();
        }
    }
}
