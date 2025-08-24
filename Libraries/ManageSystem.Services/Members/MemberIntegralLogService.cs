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
using System.Data.Common;
using ManageSystem.Core.Infrastructure;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.Orders;
using ManageSystem.Services.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Extensions;

namespace ManageSystem.Services.Members
{
	/// <summary>
	/// 操作类 ，数据库表名：MemberIntegralLog 
	/// </summary>
	public partial class MemberIntegralLogService :  BaseService<MemberIntegralLog>, IMemberIntegralLogService
	{
        private readonly IMemberService MemberService;
        private readonly IActionLogService ActionLogService;
        public MemberIntegralLogService(IRepository<MemberIntegralLog> repository,
             IMemberService _memberService,
                       IActionLogService _actionLogService) : base(repository)
		{
            this.MemberService = _memberService;
            this.ActionLogService = _actionLogService;

        }

        /// <summary>
        /// 获取指定用户的积分使用记录
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IQueryable<MemberIntegralLog> Query(long memberId)
        {
            if (memberId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId);

            query = query.OrderByDescending(m => m.InsertTime);

            return query;
        }

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<MemberIntegralLog> QueryPage(string loginId, string name, string remark, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(loginId))
            {
                var member = this.MemberService.QueryEntity(m => m.Mark > 0 && m.LoginId.Equals(loginId.Trim()));
                if (member == null || member.Id <= 0)
                    query = query.Where(m => m.MemberId == -1);
                else
                    query = query.Where(m => m.MemberId == member.Id);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var memberList = this.MemberService.Query(m => m.Mark > 0 && m.Name.Equals(name.Trim()));
                if (memberList == null || !memberList.Any())
                    query = query.Where(m => m.MemberId == -1);
                else
                {
                    var memberIds = memberList.Select(m => m.Id).ToList();
                    query = query.Where(m =>memberIds.Contains(m.MemberId));
                }
            }

            if(!string.IsNullOrWhiteSpace(remark))
                query = query.Where(m => m.Remark.Contains(remark));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<MemberIntegralLog>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actionSource"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool Update(MemberIntegralLog entityTemp, ActionSource actionSource, Account account)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    decimal itemAmount1 = 0; //明细修改前的金额
                    decimal itemAmount2 = 0; //明细修改后的金额
                    decimal memberAmount1 = 0; //会员主表修改前的金额
                    decimal memberAmount2 = 0; //会员主表修改后的金额

                    //1、修改信息
                    var entity = this.QueryEntity(entityTemp.Id);
                    itemAmount1 = entity.Value;
                    entity.Value = entityTemp.Value;
                    entity.Remark = entityTemp.Remark;
                    entity.Describe = string.IsNullOrWhiteSpace(entityTemp.Describe) ? "" : entityTemp.Describe;
                    entity.Source = actionSource.GetDescription();
                    this.Update(entity);

                    itemAmount2 = entity.Value;
       
                    //2、修改用户主表积分
                    var member = this.MemberService.QueryEntity(entity.MemberId);
                    if (member == null || member.Id <= 0)
                        throw new Exception("会员数据不正确");

                    memberAmount1 = member.IntegralAmount;
                    //差值大于0表示本次操作是减少了，如果小于0表示本次操作是增加了
                    member.IntegralAmount = this.Query(m => m.Mark > 0 && m.MemberId == entity.MemberId).Sum(m => m.Value);  //重新计算用户的总积分
                    this.MemberService.Update(member);
                    memberAmount2 = member.IntegralAmount;

                    //3、添加系统日志
                    string log = string.Format("修改用户积分记录，明细id：{0}，明细修改前{1}，明细修改后{2}。会员总额修改前{3}，会员总额修改后{4}", 
                                                      entity.Id,  itemAmount1, itemAmount2, memberAmount1, memberAmount2);
                    this.ActionLogService.Insert(ActionType.Edit, actionSource, account.Id, account.Name + "（" + account.LoginId + "）", log, entity.SerializeObject());

                    tran.Commit();
                }
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        ///添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actionSource"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool Insert(MemberIntegralLog entity, ActionSource actionSource, Account account)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    decimal memberAmount1 = 0; //会员主表修改前的金额
                    decimal memberAmount2 = 0; //会员主表修改后的金额

                    //1、修改信息
                    entity.Source = actionSource.GetDescription();
                    entity.Describe = string.IsNullOrWhiteSpace(entity.Describe) ? "" : entity.Describe;
                    this.Insert(entity);

                    //2、修改用户主表积分
                    var member = this.MemberService.QueryEntity(entity.MemberId);
                    if (member == null || member.Id <= 0)
                        throw new Exception("会员数据不正确");

                    memberAmount1 = member.IntegralAmount;
                    //差值大于0表示本次操作是减少了，如果小于0表示本次操作是增加了
                    member.IntegralAmount = this.Query(m => m.Mark > 0 && m.MemberId == entity.MemberId).Sum(m => m.Value);  //重新计算用户的总积分
                    this.MemberService.Update(member);
                    memberAmount2 = member.IntegralAmount;

                    //3、添加系统日志
                    string log = string.Format("添加用户积分记录，明细id：{0}。会员总额修改前{1}，会员总额修改后{2}",
                                                      entity.Id, memberAmount1, memberAmount2);
                    this.ActionLogService.Insert(ActionType.Create, actionSource, account.Id, account.Name + "（" + account.LoginId + "）", log, entity.SerializeObject());

                    tran.Commit();
                }
                con.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
