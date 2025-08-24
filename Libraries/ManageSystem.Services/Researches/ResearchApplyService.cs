using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Data;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Infrastructure;
using System.Data.Common;
using ManageSystem.Services.SystemSet;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.Log;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Research;

namespace ManageSystem.Services.Researches
{
    /// <summary>
    /// 操作类 ，数据库表名：ResearchApply 
    /// </summary>
    public partial class ResearchApplyService :  BaseService<ResearchApply>, IResearchApplyService
    {
        private readonly IResearchService ResearchService;
        private readonly IAutoCodeService AutoCodeService;
        private readonly IActionLogService ActionLogService;

        public ResearchApplyService(IRepository<ResearchApply> repository,
             IResearchService researchService,
              IAutoCodeService autoCodeService,
               IActionLogService actionLogService
            ) : base(repository)
		{
            this.ResearchService = researchService;
            this.AutoCodeService = autoCodeService;
            this.ActionLogService = actionLogService;
        }


        /// <summary>
        /// 用户申请参与科研合作
        /// </summary>
        /// <param name="researchId">科研id</param>
        /// <param name="userName">用户姓名</param>
        /// <param name="userPhone">用户手机</param>
        /// <param name="userEmail">用户邮箱</param>
        /// <param name="member">当前登录用户</param>
        /// <returns></returns>
        public ResearchApply Insert(long researchId, string userName, string userPhone, string userEmail, Member member)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查科研数据
                    Research researchEntity = this.ResearchService.QueryEntity(researchId);
                    if (researchEntity == null || researchEntity.Id <= 0) throw new Exception("科研不存在");
                    if (researchEntity.Status < (int)ResearchStatusEnum.Finish) throw new Exception("科研不可用");
     
                    //检查是否重复申请， 状态是“已取消”的排除掉
                    int applyCount =  this.Count(m => m.ResearchId == researchEntity.Id && m.MemberId == member.Id && m.Status != (int)ResearchApplyStatusEnum.Cancel && m.Mark>0);
                    if(applyCount>0) throw new Exception("您已经报过名了");

                    //申请表里面插入数据
                    ResearchApply model = new ResearchApply();
                    model.Count = 1;
                    model.Email = userEmail;
                    model.ResearchId = researchEntity.Id;
                    model.ResearchName = researchEntity.Name;
                    model.MemberId = member.Id;
                    model.MemberName = member.Name;
                    model.Name = userName;
                    model.Phone = userPhone;
                    model.Remark = "";
                    model.Email = userEmail;
                    model.Status = (int)ResearchApplyStatusEnum.Wait;
                    model.StatusTime = DateTime.Now;
                    this.Insert(model);

                    //更新会议主表
                    researchEntity.ApplyCount += 1;
                    this.ResearchService.Update(researchEntity);

                    //添加操作日志
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Web, member.Id, member.Name + "（" + member.LoginId + "）", "用户申请科研合作成功，会议名称：" + researchEntity.Name, model.SerializeObject());
                 
                    tran.Commit();

                    return model;
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
        /// <param name="name">会议主题</param>
        /// <param name="meetingType">所属分类</param>
        /// <param name="type">会议类型</param>
        /// <param name="memberName">发布人</param>
        /// <param name="contact">联系方式</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<ResearchApply> QueryPage(long researchId, string researchName, string name, string phone, string email, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (researchId > 0)
                query = query.Where(m => m.ResearchId == researchId);
            
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.Phone.Contains(phone.Trim()));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(m => m.Email.Contains(email.Trim()));

            if (!string.IsNullOrWhiteSpace(researchName))
            {
                List<long> ids = this.ResearchService.Query(m => m.Name.Contains(researchName)).Select(m => m.Id).ToList();
                if (ids != null && ids.Any())
                    query = query.Where(m => ids.Contains(m.ResearchId));
            }

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<ResearchApply>(query, pageIndex, pageSize);
        }
        
        /// <summary>
        /// 检查用户的申请状态
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <param name="researchId">科研id</param>
        /// <returns>true 已经申请  false  未申请</returns>
        public bool MemberApplyStatus(long memberId,long researchId)
        {
            if (memberId <= 0) return false;
            return (this.Count(m => m.MemberId == memberId  && m.ResearchId== researchId  && m.Mark > 0) > 0);
        }

        /// <summary>
        /// 根据会员编号获取所对应的科研合作数据  （会员中心）
        /// </summary>
        /// <param name="memberId">会员编号</param>>
        /// <returns></returns>
        public IQueryable<ResearchApply> Query(long memberId)
        {
            if (memberId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId).OrderByDescending(m => m.InsertTime);

            return query;
        }

        /// <summary>
        /// 用户取消申请
        /// </summary>
        /// <param name="memberId">会员编号</param>
        /// <param name="applyId">科研合作申请的Id</param>
        /// <param name="memberName">操作人姓名，格式：姓名（登录帐号）</param>
        /// <returns></returns>
        public bool Cancel(long memberId, long applyId, string memberName)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查科研合作数据
                    ResearchApply meetingEntity = this.QueryEntity(m => m.Id == applyId);
                    if (meetingEntity == null || meetingEntity.Id <= 0) throw new Exception("数据不存在");
                    if (meetingEntity.MemberId != memberId) throw new Exception("您无权操作");

                    Research meeting = this.ResearchService.QueryEntity(meetingEntity.ResearchId);
                    if (meeting == null || meeting.Id <= 0) throw new Exception("数据不存在");

                    //取消申请数据
                    meetingEntity.Status = (int)ResearchApplyStatusEnum.Cancel;
                    meeting.StartTime = DateTime.Now;
                    this.Update(meetingEntity);

                    //更新科研合作主表
                    meeting.ApplyCount -= 1;
                    this.ResearchService.Update(meeting);

                    //添加操作日志
                    this.ActionLogService.Insert(ActionType.Delete, ActionSource.Web, memberId, memberName, "取消科研合作报名申请，科研名称：" + meeting.Name, meetingEntity.SerializeObject());

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
        /// 根据会员编号获取所对应的科研合作数据  （会员中心）
        /// </summary>
        /// <param name="researchId">信息动态编号</param>
        /// <param name="searchKey">查询关键字</param>
        /// <param name="status">报名申请状态</param>
        /// <returns></returns>
        public IQueryable<ResearchApply> QueryByMeetingId(long researchId, String searchKey, int status)
        {
            if (researchId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.ResearchId == researchId);

            if (!string.IsNullOrWhiteSpace(searchKey))
                query = query.Where(m => m.Name.Contains(searchKey) || m.Email.Contains(searchKey) || m.Phone.Contains(searchKey));

            if (status > 0)
                query = query.Where(m => m.Status == status);

            query = query.OrderByDescending(m => m.InsertTime);

            return query;
        }

    }

}
