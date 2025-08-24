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
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Log;

namespace ManageSystem.Services.Members
{
    /// <summary>
    /// 操作类 ，数据库表名：MemberAttestation 
    /// </summary>
    public partial class MemberAttestationService : BaseService<MemberAttestation>, IMemberAttestationService
    {
        public readonly IMemberService MemberService;
        public readonly IActionLogService ActionLogService;

        public MemberAttestationService(IRepository<MemberAttestation> repository,
            IMemberService _memberService,
            IActionLogService _actionLogService
            ) : base(repository)
        {
            this.MemberService = _memberService;
            this.ActionLogService = _actionLogService;
        }

        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool FinishCheck(long id, Account account)
        {
            if (id <= 0) throw new Exception("保存数据不能为空");

            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    var entity = this.QueryEntity(id);
                    if (entity == null || entity.Id <= 0) throw new Exception("申请不存在");

                    if (entity.Status != (int)MemberAttestationStatus.WaitCheck)
                        throw new Exception("只有等待审核的才能操作");

                    //1、修改用户主表
                    var member = this.MemberService.QueryEntity(entity.MemberId);
                    member.HospitalId = entity.HospitalId;
                    member.AreaId = entity.AreaId;
                    member.ProvinceId = entity.AreaId;
                    member.DoctorTitleId = entity.DoctorTitleId;
                    member.HospitalDepartmentId = entity.HospitalDepartmentId;
                    member.Type = (int)MemberType.Authentication;
                    this.MemberService.Update(member);

                    //2、修改申请表
                    entity.Status = (int)MemberAttestationStatus.Finish;
                    this.Update(entity);

                    //3、添加操作日志
                    this.ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, account.Id, account.Name + "（" + account.LoginId + "）", "用户认证申请审核通过，申请人姓名：" + entity.MemberName, entity.SerializeObject());

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
        /// 分页查询
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="state"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<MemberAttestation> QueryPage(string memberName, int state, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(memberName))
                query = query.Where(m => m.MemberName.Contains(memberName.Trim()));

            if (state > 0)
                query = query.Where(m => m.Status == state);

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<MemberAttestation>(query, pageIndex, pageSize);

        }
    }
}
