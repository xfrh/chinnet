using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Services.Log;
using ManageSystem.Services.Meetings;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    /// <summary>
    /// 信息动态
    /// </summary>
    public class InformationController : MobileBaseController
    {
        /// <summary>
        /// 动态业务层
        /// </summary>
        private readonly IMeetingService MeetingService;
        /// <summary>
        /// 动态信息业务层
        /// </summary>
        private readonly IMeetingViewService MeetingViewService;
        /// <summary>
        /// 区域业务层
        /// </summary>
        private readonly IAreaService AreaService;
        /// <summary>
        /// 用户业务层
        /// </summary>
        private readonly IMemberService MemberService;
        /// <summary>
        /// 加解密服务
        /// </summary>
        private readonly IEncryptionService EncryptionService;
        /// <summary>
        /// 系统日志业务层
        /// </summary>
        private readonly ISystemLogService SystemLogService;
        /// <summary>
        /// 操作日志业务层
        /// </summary>
        private readonly IActionLogService ActionLogService;

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="_meetingService"></param>
        /// <param name="_meetingViewService"></param>
        /// <param name="_areaService"></param>
        /// <param name="_memberService"></param>
        /// <param name="_encryptionService"></param>
        /// <param name="_systemLogService"></param>
        /// <param name="_actionLogService"></param>
        public InformationController(IMeetingService _meetingService, IMeetingViewService _meetingViewService, IAreaService _areaService, IMemberService _memberService, IEncryptionService _encryptionService, ISystemLogService _systemLogService, IActionLogService _actionLogService)
        {
            MeetingService = _meetingService;
            MeetingViewService = _meetingViewService;
            AreaService = _areaService;
            MemberService = _memberService;
            EncryptionService = _encryptionService;
            SystemLogService = _systemLogService;
            ActionLogService = _actionLogService;
        }
        
        
        #region 动态信息列表
        /// <summary>
        /// 动态信息列表页
        /// </summary>
        /// <returns></returns>
        public ViewResult List()
        {
            return View();
        }

        /// <summary>
        /// 分页获取动态信息
        /// </summary>
        /// <param name="page">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(int page = 1, int pageSize = 15)
        {

            //分页数据
            var pageList = MeetingService.Query(0, 0, 0, 0).ToPagedList(page, pageSize); //获取查询数据

            List<long> areaIdArray = pageList.Select(m => m.AreaId).ToList();
            List<long> memberIdArray = pageList.Select(m => m.MemberId).ToList();
            var areaList = this.AreaService.Query(m => areaIdArray.Contains(m.Id));
            var memberList = this.MemberService.Query(m => memberIdArray.Contains(m.Id));
            List<dynamic> data = new List<dynamic>();
            foreach (var item in pageList)
            {
                var memberTemp = memberList.Where(m => m.Id == item.MemberId).FirstOrDefault();
                item.MemberName = (memberTemp == null || memberTemp.Id <= 0) ? item.MemberName : memberTemp.NickName;

                var areaTemp = areaList.Where(m => m.Id == item.AreaId).FirstOrDefault();
                item.AreaName = (areaTemp == null || areaTemp.Id <= 0) ? item.AreaName : areaTemp.Name;
                data.Add(new
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    CoverImage = MeetingServiceExtensions.GetCoverImage(item.CoverImage),
                    Date = item.InsertTime.ToString("yyyy/MM/dd"),
                    item.CommentCount,
                    item.ViewCount,
                    linkUrl = Url.Action("Detail", new { id = item.Id }).ToLowerInvariant()
                });
            }

            if (pageList.Any())
            {
                return Json(new
                {
                    status = true,
                    data = data,
                    nextPage = page + 1,
                    existNextPage = pageList.TotalPageCount > page
                });
            }

            return Json(new
            {
                status = false,
                data = data,
                nextPage = page + 1,
                existNextPage = pageList.TotalPageCount > page
            });
        }
        #endregion

        #region 动态信息详情
        /// <summary>
        /// 动态信息详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Detail(long id)
        {
            if (id <= 0) throw new Exception("会议不存在");

            var entity = this.MeetingService.QueryEntity(id);
            if (entity == null || entity.Id <= 0) throw new Exception("会议不存在");

            if (entity.Status != (int)MeetingStatusEnum.Finish) throw new Exception("会议不可用");

            var memberEntity = this.MemberService.QueryEntity(entity.MemberId);

            ViewData["Id"] = entity.Id;
            ViewData["InfoTitle"] = entity.Name;
            ViewData["MemberNickName"] = memberEntity?.NickName;
            ViewData["Date"] = entity.InsertTime.ToString("yyyy-MM-dd HH:mm");
            ViewData["Content"] = !string.IsNullOrWhiteSpace(entity.Content) ? entity.Content : "";
            ViewData["ViewCount"] = entity.ViewCount;

            return View();
        }

        /// <summary>
        /// 添加查看记录
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMettingView(long meetingId, string token)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = MemberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            MeetingViewService.Insert(member.Id, member.Name, meetingId);

            return Json(new { status = true });
        }
        #endregion
    }
}