using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Members;
using System.Data.Common;
using ManageSystem.Core.Infrastructure;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using System.Linq.Expressions;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.Members
{
    /// <summary>
    /// 操作类 ，数据库表名：MemberAddress 
    /// </summary>
    public partial class MemberAddressService : BaseService<MemberAddress>, IMemberAddressService
    {
        private readonly IActionLogService ActionLogService;
        private readonly IMemberService MemberService;

        public MemberAddressService(IRepository<MemberAddress> repository,
               IActionLogService actionLogService,
                  IMemberService memberService
            ) : base(repository)
        {
            this.ActionLogService = actionLogService;
            this.MemberService = memberService;
        }


        /// <summary>
        ///保存 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="account"></param>
        public void Insert(MemberAddress entity, Account account, ActionSource source)
        {
            if (entity == null) throw new Exception("保存数据不能为空");

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    if (entity.IsMain)
                    {
                        //新增的收货地址是默认的，修改数据库中其他的收货地址默认状态
                        this.Update(m => m.MemberId == entity.MemberId, p => new MemberAddress { IsMain = false, UpdateTime = DateTime.Now });
                    }

                    //插入数据
                    this.Insert(entity);
                    if (entity.Id <= 0) throw new Exception("保存收货地址失败");

                    //添加操作日志
                    this.ActionLogService.Insert(ActionType.Create, source, account.Id, account.Name + "（" + account.LoginId + "）", "添加会员地址成功，收货人姓名：" + entity.Name, entity.SerializeObject());

                    tran.Commit();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<MemberAddress> QueryPage(string loginId, string name, string phone, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Equals(name.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.Phone.Equals(phone.Trim()));

            if (!string.IsNullOrWhiteSpace(loginId))
            {
                var member = this.MemberService.QueryEntity(m => m.Mark > 0 && m.LoginId.Equals(loginId.Trim()));
                if (member == null || member.Id <= 0)
                    query = query.Where(m => m.MemberId == -1);
                else
                    query = query.Where(m => m.MemberId == member.Id);
            }
         
            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<MemberAddress>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 保存编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="account"></param>
        public void Update(MemberAddress entity, Account account, ActionSource source)
        {
            if (entity == null) throw new Exception("保存数据不能为空");

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    if (entity.IsMain)
                    {
                        //新增的收货地址是默认的，修改数据库中其他的收货地址默认状态
                        this.Update(m => m.MemberId == entity.MemberId, p => new MemberAddress { IsMain = false, UpdateTime = DateTime.Now });
                    }

                    //插入数据
                    this.Update(entity);
                    if (entity.Id <= 0) throw new Exception("保存收货地址失败");

                    //添加操作日志
                    this.ActionLogService.Insert(ActionType.Edit, source, account.Id, account.Name + "（" + account.LoginId + "）", "修改会员地址成功，收货人姓名：" + entity.Name, entity.SerializeObject());

                    tran.Commit();
                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
