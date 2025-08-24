using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Domain.Members;
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

namespace ManageSystem.Services.Meetings
{
    /// <summary>
    /// 操作类 ，数据库表名：MeetingApply 
    /// </summary>
    public partial class MeetingApplyService : BaseService<MeetingApply>, IMeetingApplyService
    {
        private readonly IMeetingService MeetingService;
        private readonly IAutoCodeService AutoCodeService;
        private readonly IActionLogService ActionLogService;


        public MeetingApplyService(IRepository<MeetingApply> repository,
             IMeetingService meetingService,
              IAutoCodeService autoCodeService,
               IActionLogService actionLogService
            ) : base(repository)
        {
            this.MeetingService = meetingService;
            this.AutoCodeService = autoCodeService;
            this.ActionLogService = actionLogService;
        }


        /// <summary>
        /// 用户申请参与会议
        /// </summary>
        /// <param name="meetingId">会议id</param>
        /// <param name="userName">用户姓名</param>
        /// <param name="userPhone">用户手机</param>
        /// <param name="userEmail">用户邮箱</param>
        /// <param name="member">当前登录用户</param>
        /// <returns></returns>
        public MeetingApply Insert(long meetingId, string userName, string userPhone, string userEmail, Member member)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查信息动态数据
                    Meeting meetingEntity = this.MeetingService.QueryEntity(meetingId);
                    if (meetingEntity == null || meetingEntity.Id <= 0) throw new Exception("会议不存在");
                    if (meetingEntity.Status < (int)MeetingStatusEnum.Finish) throw new Exception("会议不可用");
                    if (meetingEntity.EndTime < DateTime.Now) throw new Exception("会议已经结束");

                    //检查是否重复申请， 状态是“已取消”的排除掉
                    int applyCount = this.Count(m => m.MeetingId == meetingEntity.Id && m.MemberId == member.Id && m.Status != (int)MeetingApplyStatusEnum.Cancel && m.Mark > 0);
                    if (applyCount > 0) throw new Exception("您已经报过名了");

                    //申请表里面插入数据
                    MeetingApply model = new MeetingApply();
                    model.CallBackSN = "";
                    model.Count = 1;
                    model.Email = userEmail;
                    model.MeetingId = meetingEntity.Id;
                    model.MeetingName = meetingEntity.Name;
                    model.MemberId = member.Id;
                    model.MemberName = member.Name;
                    model.OrderSN = this.AutoCodeService.GetCode(AutoCodeType.MeetingApply);
                    model.Price = meetingEntity.Price;
                    model.PayAmount = model.Price * model.Count;
                    model.PayMethod = 0;
                    model.PayTime = DateHelper.DefaultValue();
                    model.Name = userName;
                    model.Phone = userPhone;
                    model.ReceivedAmount = 0;
                    model.Remark = "";
                    model.Email = userEmail;
                    model.Status = meetingEntity.Type == (int)MeetingTypeEnum.Free ? (int)MeetingApplyStatusEnum.Finish : (int)MeetingApplyStatusEnum.Wait;
                    model.StatusTime = DateTime.Now;
                    this.Insert(model);

                    //更新会议主表
                    meetingEntity.ApplyCount += 1;
                    this.MeetingService.Update(meetingEntity);

                    //添加操作日志
                    this.ActionLogService.Insert(ActionType.Create, ActionSource.Web, member.Id, member.Name + "（" + member.LoginId + "）", "用户申请信息动态成功，会议名称：" + meetingEntity.Name, model.SerializeObject());

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
        /// 检查用户的申请状态
        /// </summary>
        /// <param name="memberId">会议id</param>
        /// <param name="meetingId">科研id</param>
        /// <returns>true 已经申请  false  未申请</returns>
        public bool MemberApplyStatus(long memberId, long meetingId)
        {
            if (memberId <= 0) return false;
            return (this.Count(m => m.MemberId == memberId && m.MeetingId == meetingId && m.Mark > 0) > 0);
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
        public IPagedList<MeetingApply> QueryPage(long meetingId, string meetingName, string name, string phone, string email, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (meetingId > 0)
                query = query.Where(m => m.MeetingId == meetingId);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.Phone.Contains(phone.Trim()));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(m => m.Email.Contains(email.Trim()));

            if (!string.IsNullOrWhiteSpace(meetingName))
            {
                List<long> ids = this.MeetingService.Query(m => m.Name.Contains(meetingName)).Select(m => m.Id).ToList();
                if (ids != null && ids.Any())
                    query = query.Where(m => ids.Contains(m.MeetingId));
            }

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<MeetingApply>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据会员编号获取所对应的信息动态数据 （会员中心）
        /// </summary>
        /// <param name="memberId">会员编号</param>>
        /// <returns></returns>
        public IQueryable<MeetingApply> Query(long memberId)
        {
            if (memberId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId).OrderByDescending(m => m.InsertTime);

            return query;
        }

        /// <summary>
        /// 根据会员编号获取所对应的信息动态数据  （会员中心）
        /// </summary>
        /// <param name="meetingId">信息动态编号</param>
        /// <param name="searchKey">查询关键字</param>
        /// <param name="status">报名申请状态</param>
        /// <returns></returns>
        public IQueryable<MeetingApply> QueryByMeetingId(long meetingId, String searchKey, int status)
        {
            if (meetingId <= 0) return null;

            var query = this._repository.Table.Where(m => m.Mark > 0 && m.MeetingId == meetingId);

            if (!string.IsNullOrWhiteSpace(searchKey))
                query = query.Where(m => m.Name.Contains(searchKey) || m.Email.Contains(searchKey) || m.Phone.Contains(searchKey));

            if (status > 0)
                query = query.Where(m => m.Status == status);

            query = query.OrderByDescending(m => m.InsertTime);

            return query;
        }

        /// <summary>
        /// 用户取消申请
        /// </summary>
        /// <param name="memberId">会员编号</param>
        /// <param name="meetingApplyId">信息动态申请的Id</param>
        /// <param name="memberName">操作人姓名，格式：姓名（登录帐号）</param>
        /// <returns></returns>
        public bool Cancel(long memberId, long meetingApplyId, string memberName)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    //检查信息动态数据
                    MeetingApply meetingEntity = this.QueryEntity(m => m.Id == meetingApplyId);
                    if (meetingEntity == null || meetingEntity.Id <= 0) throw new Exception("数据不存在");
                    if (meetingEntity.MemberId != memberId) throw new Exception("您无权操作");

                    Meeting meeting = this.MeetingService.QueryEntity(meetingEntity.MeetingId);
                    if (meeting == null || meeting.Id <= 0) throw new Exception("数据不存在");

                    //取消申请数据
                    meetingEntity.Status = (int)MeetingApplyStatusEnum.Cancel;
                    meeting.StartTime = DateTime.Now;
                    this.Update(meetingEntity);

                    //更新会议主表
                    meeting.ApplyCount -= 1;
                    this.MeetingService.Update(meeting);

                    //添加操作日志
                    this.ActionLogService.Insert(ActionType.Delete, ActionSource.Web, memberId, memberName, "【手动】取消信息动态报名申请，会议名称：" + meeting.Name, meetingEntity.SerializeObject());

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
    }
}
