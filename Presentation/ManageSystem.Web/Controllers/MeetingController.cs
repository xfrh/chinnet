using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Upload;
using ManageSystem.Services.Meetings;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Meetings;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class MeetingController : WebBaseController
    {
        private readonly IMeetingTypeService MeetingTypeService;
        private readonly IMeetingService MeetingService;
        private readonly IAreaService AreaService;
        private readonly IMemberService MemberService;
        private readonly IMeetingApplyService MeetingApplyService;
        private readonly IMeetingViewService MeetingViewService;
        private readonly IMeetingCollectService MeetingCollectService;
        private readonly IMeetingCommentService MeetingCommentService;
        private readonly ISatelliteService SatelliteService;


        public MeetingController(
            IMeetingTypeService _meetingTypeService,
                IMeetingService _meetingService,
                IAreaService _areaService,
                IMemberService _memberService,
                 IMeetingApplyService _meetingApplyService,
                 IMeetingViewService _meetingViewService,
                 IMeetingCollectService _meetingCollectService,
                 IMeetingCommentService _meetingCommentService,
                 ISatelliteService _satelliteService
        )
        {
            this.MeetingTypeService = _meetingTypeService;
            this.MeetingService = _meetingService;
            this.AreaService = _areaService;
            this.MemberService = _memberService;
            this.MeetingApplyService = _meetingApplyService;
            this.MeetingViewService = _meetingViewService;
            this.MeetingCollectService = _meetingCollectService;
            this.MeetingCommentService = _meetingCommentService;
            SatelliteService = _satelliteService;
        }

        #region 会议首页数据

        [CheckRole(false)]
        public ActionResult Index(IndexSearchModel model)
        {
            var result = this.SetIndexData(model);

            return View(result);
        }

        private IndexModel SetIndexData(IndexSearchModel searchModel)
        {

            IndexModel model = new IndexModel();
            model.SearchModel = searchModel ?? new IndexSearchModel();

            model.SearchMeetingTypeList = this.MeetingTypeService.Query(m => m.MeetingTypeId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //信息动态的类型（顶级）
            model.SearchAreaList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //区域列表（顶级，省份）
            model.SearchDayList = this.GetSearchDayList();  //时间查询条件
            model.SearchPriceList = this.GetSearchPrcieList();   //收费查询条件

            //分页数据
            model.PageList = this.MeetingService.Query(searchModel.Type, searchModel.Area, searchModel.Day, searchModel.Price).Where(x=>string.IsNullOrEmpty(x.Describe)).ToPagedList(searchModel.PageIndex, 10); //获取查询数据

            List<long> areaIdArray = model.PageList.Select(m => m.AreaId).ToList();
            List<long> memberIdArray = model.PageList.Select(m => m.MemberId).ToList();
            var areaList = this.AreaService.Query(m => areaIdArray.Contains(m.Id));
            var memberList = this.MemberService.Query(m => memberIdArray.Contains(m.Id));

            foreach (var item in model.PageList)
            {
                var memberTemp = memberList.Where(m => m.Id == item.MemberId).FirstOrDefault();
                item.MemberName = (memberTemp == null || memberTemp.Id <= 0) ? item.MemberName : memberTemp.NickName;

                try
                {
                    var areaTemp = areaList.Where(m => m.Id == item.AreaId).FirstOrDefault();
                    item.AreaName = (areaTemp == null || areaTemp.Id <= 0) ? item.AreaName : areaTemp.Name;
                }
                catch {
                    item.AreaName = string.Empty;
                }
                
            }

            return model;
        }


        [CheckRole(false)]
        public ActionResult SatelliteIndex1(string city)
        {

            var result = this.SetSatelliteIndexData(new IndexSearchModel(), city);

            return View(result);
        }

        private IndexModel SetSatelliteIndexData(IndexSearchModel searchModel, string city)
        {
            string name = SatelliteService.Query().Single(x => x.RealmName == city).Id.ToString();
            IndexModel model = new IndexModel();
            model.SearchModel = searchModel ?? new IndexSearchModel();
            model.SearchMeetingTypeList = this.MeetingTypeService.Query(m => m.MeetingTypeId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //信息动态的类型（顶级）
            model.SearchAreaList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //区域列表（顶级，省份）
            model.SearchDayList = this.GetSearchDayList();  //时间查询条件
            model.SearchPriceList = this.GetSearchPrcieList();   //收费查询条件

            int count = this.MeetingTypeService.Query().Where(x => !string.IsNullOrEmpty(x.Describe)).Where(x => x.Describe.Contains(name)).Count();
            if (count > 0)
            {

                long typeid = this.MeetingTypeService.Query().Single(x => !string.IsNullOrEmpty(x.Describe) && x.Describe.Contains(name)).Id;
                //分页数据
                model.PageList = this.MeetingService.Query(searchModel.Type, searchModel.Area, searchModel.Day, searchModel.Price).Where(x => x.MeetingTypeId == typeid).ToPagedList(searchModel.PageIndex, 10); //获取查询数据

                List<long> areaIdArray = model.PageList.Select(m => m.AreaId).ToList();
                List<long> memberIdArray = model.PageList.Select(m => m.MemberId).ToList();
                var areaList = this.AreaService.Query(m => areaIdArray.Contains(m.Id));
                var memberList = this.MemberService.Query(m => memberIdArray.Contains(m.Id));

                foreach (var item in model.PageList)
                {
                    var memberTemp = memberList.Where(m => m.Id == item.MemberId).FirstOrDefault();
                    item.MemberName = (memberTemp == null || memberTemp.Id <= 0) ? item.MemberName : memberTemp.NickName;

                    var areaTemp = areaList.Where(m => m.Id == item.AreaId).FirstOrDefault();
                    item.AreaName = (areaTemp == null || areaTemp.Id <= 0) ? item.AreaName : areaTemp.Name;
                }

            }
            else
            {
                model.PageList = null;
            }


            return model;
        }


        private List<IndexSearchDay> GetSearchDayList()
        {
            List<IndexSearchDay> list = new List<IndexSearchDay>();
            list.Add(new IndexSearchDay() { Text = "今天", Value = 1 });
            list.Add(new IndexSearchDay() { Text = "近一周", Value = 2 });
            list.Add(new IndexSearchDay() { Text = "近一月", Value = 3 });
            //list.Add(new IndexSearchDay() { Text = "周末", Value = "s" });

            return list;
        }
        private List<IndexSearchPrice> GetSearchPrcieList()
        {
            List<IndexSearchPrice> list = new List<IndexSearchPrice>();
            list.Add(new IndexSearchPrice() { Text = "收费", Value = 1 });
            list.Add(new IndexSearchPrice() { Text = "免费", Value = 2 });

            return list;
        }


        #endregion

        #region 发布会议

        public ActionResult Create()
        {
            return View(this.SetCreateData());
        }

        private MeetingModel SetCreateData()
        {
            MeetingModel model = new MeetingModel();

            model.StartTime = DateTime.Now;
            model.Price = 0;
            model.PersonMaxCount = 0;
            model.Agreement = true;
            model.MeetingTypeParentList = new List<SelectListItem>();
            model.MeetingTypeParentList = this.MeetingTypeService.Query(m => m.Mark > 0 && m.MeetingTypeId == 0).OrderBy(m => m.Sort).Select(x =>
            {
                return new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                };
            }).ToList();

            model.MeetingTypeParentList.Insert(0, new SelectListItem()
            {
                Text = "请选择",
                Value = "0"
            });

            model.MeetingTypeList = new List<SelectListItem>();
            model.MeetingTypeList.Insert(0, new SelectListItem()
            {
                Text = "请选择",
                Value = "0"
            });

            return model;
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MeetingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();

                    var loginUser = base.LoginUserinfo;
                    entity.MemberId = loginUser.Id;
                    entity.MemberName = loginUser.NickName;
                    entity.AreaFullName = this.AreaService.GetFullName(entity.AreaId);
                    entity.Status = (int)MeetingStatusEnum.Finish;

                    var areaEntity = this.AreaService.QueryEntity(entity.AreaId);
                    if (areaEntity != null && areaEntity.Id > 0)
                    {
                        entity.AreaName = areaEntity.Name;
                        string[] tempArray = areaEntity.Position.Split(',');
                        entity.AreaProvinceId = long.Parse(tempArray[1]);
                    }

                    this.MeetingService.Insert(entity);

                    string logContent = "创建信息动态，会议主题：" + model.Name;
                    base.InsetActionLog(ActionType.Create, logContent, model.SerializeObject());
                    base.SuccessNotification("您的会议申请提交成功！");

                    return this.RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);
                base.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
            }

            return View(this.SetCreateData());
        }

        /// <summary>
        /// 获取会议类型的2级下拉数据
        /// </summary>
        /// <param name="parentId">上级id</param>
        /// <returns></returns>
        public ContentResult GetMettingTypeList(long parentId)
        {
            if (parentId <= 0) return this.Content("");

            var list = this.MeetingTypeService.Query(m => m.Mark > 0 && m.MeetingTypeId == parentId).OrderBy(m => m.Sort).Select(x =>
          {
              return new
              {
                  Text = x.Name,
                  Value = x.Id.ToString()
              };
          }).ToList();

            return this.Content(list.SerializeObject());
        }

        /// <summary>
        /// 上传会议的图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UploadFile()
        {
            UploadParameter par = new UploadParameter();
            UploadResult model = new UploadifyUpload().Upload(new UploadParameter()
            {
                Extension = "jpg,jpeg,png,bmp",
                FilePath = "/Content/Upload/Meetings/",
                HttpFile = this.HttpContext.Request.Files,
                MaxLength = 2 * 1024, //10M
                NewFileName = Guid.NewGuid() + "_" + new Random().Next(10000, 99999),
                Type = UploadTypeEnum.Image
            });

            return this.Content(model.SerializeObject());
        }

        #endregion

        #region 会议详细页面  
        [CheckRole(false)]
        public ActionResult Detail(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("会议不存在");

                MeetingModel model = this.MeetingService.QueryEntity(id).ToModel();
                if (model == null || model.Id <= 0) throw new Exception("会议不存在");

                if (model.Status != (int)MeetingStatusEnum.Finish) throw new Exception("会议不可用");

                var memberEntity = this.MemberService.QueryEntity(model.MemberId);
                if (memberEntity != null && memberEntity.Id > 0)
                    model.MemberNickName = memberEntity.NickName;

                var memberModel = base.LoginUserinfo;

                MeetingDetailModel resultModel = new MeetingDetailModel();
                resultModel.MeetingEntity = model; //会议数据

                resultModel.MeetingEntity.CollectCount = MeetingCollectService.Count(r => r.MeetingId == model.Id && r.Mark > 0);

                if (memberModel == null)
                {
                    resultModel.MemberEntity = null;    //当前登录用户
                    resultModel.IsCollect = this.MeetingCollectService.MemberCollectStatus(0, model.Id);     //是否已经收藏
                    resultModel.IsApply = this.MeetingApplyService.MemberApplyStatus(0, model.Id);// 是否已经申请
                }
                else
                {
                    resultModel.MemberEntity = base.LoginUserinfo;    //当前登录用户
                    resultModel.IsCollect = this.MeetingCollectService.MemberCollectStatus(memberModel.Id, model.Id);     //是否已经收藏
                    resultModel.IsApply = this.MeetingApplyService.MemberApplyStatus(memberModel.Id, model.Id);// 是否已经申请
                }

                try
                {
                    //会议对应的申请数据
                    resultModel.ApplyMemberList = new List<MemberModel>();
                    IEnumerable<long> memberIds = this.MeetingApplyService.Query(m => m.MeetingId == model.Id && m.Mark > 0).Select(m => m.MemberId);
                    if (memberIds != null &&  memberIds.Any())
                    {
                        resultModel.ApplyMemberList = this.MemberService.Query(m => memberIds.Contains(m.Id) && m.Mark > 0).Select(m => m.ToModel()).ToList();
                    }
                }
                catch { }
                
                return View(resultModel);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("Index");
            }

        }

        /// <summary>
        /// 获取会议详细页面的用户申请数据
        /// </summary>
        /// <param name="mettingId">会议id</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult GetMettingApplyMember(long meetingId)
        {
            if (meetingId <= 0) return this.Content("");

            var applyList = this.MeetingApplyService.Query(m => m.MeetingId == meetingId && m.Mark > 0);
            if (applyList == null || !applyList.Any()) return this.Content("");

            IEnumerable<long> memberIds = applyList.Select(m => m.MemberId);
            var memberList = this.MemberService.Query(m => memberIds.Contains(m.Id) && m.Mark > 0);
            if (memberList == null || !memberList.Any()) return this.Content("");

            List<ApplyMemberModel> list = new List<ApplyMemberModel>();
            DateTime now = DateTime.Now;

            foreach (var item in applyList)
            {
                var memberTemp = memberList.Where(m => m.Id == item.MemberId).FirstOrDefault();
                if (memberTemp == null || memberTemp.Id <= 0) continue;

                string timeString = (now.Year == item.InsertTime.Year && now.Month == item.InsertTime.Month &&
                    now.Hour == item.InsertTime.Hour && now.Minute > item.InsertTime.Minute) ?
                    now.Minute - item.InsertTime.Minute + "分钟前" : item.InsertTime.ToString("MM月dd日");

                list.Add(new ApplyMemberModel()
                {
                    HeadImage = MemberExtensions.GetHeadImage(memberTemp.HeadImage),
                    MemberId = memberTemp.Id,
                    MemberNickName = memberTemp.NickName,
                    TimeValue = timeString
                });
            }

            return this.Content(list.SerializeObject());
        }

        /// <summary>
        /// 用户提交申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult MemberApply(MemberApplyModel model)
        {
            if (model == null) return this.Content(JsonHelper.GetBaseMessage(false, "数据错误"));

            if (model.MeetingId <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "会议不存在"));
            if (string.IsNullOrEmpty(model.Name) || !(new Regex(@"^\S{2,}$").IsMatch(model.Name)))
                return this.Content(JsonHelper.GetBaseMessage(false, "姓名格式不正确"));
            if (!RegexHelper.IsPhone(model.Phone))
                return this.Content(JsonHelper.GetBaseMessage(false, "手机号码格式不正确"));
            if (!RegexHelper.IsEmail(model.Email))
                return this.Content(JsonHelper.GetBaseMessage(false, "邮箱格式不正确"));

            try
            {
                MeetingApply applyModel = this.MeetingApplyService.Insert(model.MeetingId, model.Name, model.Phone, model.Email, base.LoginUserinfo);
                if (applyModel != null && applyModel.Id > 0)
                {
                    string message = applyModel.PayAmount > 0 ? @"/Pay/PayIndex?sn=" + applyModel.OrderSN : "";
                    return this.Content(JsonHelper.GetBaseMessage(true, message));
                }

                return this.Content(JsonHelper.GetBaseMessage(false, "申请失败，请重试"));

            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        /// <summary>
        /// 添加信息动态查看记录
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult AddMettingView(long meetingId)
        {
            if (meetingId <= 0) return this.Content("");
            var member = base.LoginUserinfo;
            if (member == null)
            {
                this.MeetingViewService.Insert(0, "匿名", meetingId);
            }
            {
                this.MeetingViewService.Insert(member.Id, member.Name, meetingId);
            }

            return this.Content("");
        }

        /// <summary>
        /// 收藏信息动态查看记录
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddMettingCollect(long meetingId)
        {
            if (meetingId <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "会议不存在"));
            var member = base.LoginUserinfo;

            string error = "";
            if (this.MeetingCollectService.Insert(member.Id, member.Name, meetingId, ref error))
            {
                return Content(JsonHelper.GetBaseMessage(true, Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    count = MeetingCollectService.Count(r => r.MeetingId == meetingId && r.Mark > 0)
                })));
            }
            return this.Content(JsonHelper.GetBaseMessage(string.IsNullOrWhiteSpace(error), error));
        }

        /// <summary>
        /// 收藏信息动态查看记录
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CancelMettingCollect(long meetingId)
        {
            if (meetingId <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "会议不存在"));
            var member = base.LoginUserinfo;

            string error = "";
            if (this.MeetingCollectService.Cancel(member.Id, meetingId, ref error))
            {
                return Content(JsonHelper.GetBaseMessage(true, Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    count = MeetingCollectService.Count(r => r.MeetingId == meetingId && r.Mark > 0)
                })));
            }
            return this.Content(JsonHelper.GetBaseMessage(string.IsNullOrWhiteSpace(error), error));
        }

        /// <summary>
        /// 获取信息动态的评论
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        public ContentResult GetMeetingComment(long meetingId)
        {
            if (meetingId <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "会议不存在"));

            var list = this.MeetingCommentService.Query(m => m.MeetingId == meetingId && m.Mark > 0 && m.ParentId == 0).OrderByDescending(m => m.InsertTime).Select(x => x.ToModel()).ToList();

            foreach (var item in list)
            {
                MemberModel member = this.MemberService.QueryEntity(item.MemberId).ToModel();
                member = member ?? new MemberModel();

                item.MemberHeadImage = MemberExtensions.GetHeadImage(member.HeadImage);
                item.MemberNickName = member.NickName;
                item.InsertTimeString = item.InsertTime.ToString("yyyy-MM-dd HH:mm");

                SetMettingComment(item);
            }

            return this.Content(JsonHelper.GetBaseMessage(true, list.SerializeObject()));

        }


        /// <summary>
        /// 通过递归获取评论的数据
        /// </summary>
        /// <param name="model"></param>
        [NonAction]
        private void SetMettingComment(MeetingCommentModel model)
        {
            var itemList = this.MeetingCommentService.Query(m => m.ParentId == model.Id && m.Mark > 0).OrderByDescending(m => m.InsertTime).ToList();
            if (itemList == null || !itemList.Any()) return;

            model.ItemCommentList = itemList.Select(x => x.ToModel()).ToList();

            foreach (var item in model.ItemCommentList)
            {
                MemberModel member = this.MemberService.QueryEntity(item.MemberId).ToModel();
                member = member ?? new MemberModel();

                item.MemberHeadImage = MemberExtensions.GetHeadImage(member.HeadImage);
                item.MemberNickName = member.NickName;
                item.InsertTimeString = item.InsertTime.ToString("yyyy-MM-dd HH:mm");

                SetMettingComment(item);
            }
        }


        /// <summary>
        /// 用户提交评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddComment(MeetingCommentModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Content)) return this.Content(JsonHelper.GetBaseMessage(false, "请输入评论内容！"));
            var member = base.LoginUserinfo;

            string error = "";
            model.Type = (int)MeetingCommentTypeEnum.Member;
            model.MemberId = member.Id;
            model.MemberName = member.Name;

            this.MeetingCommentService.Insert(model.ToEntity(), ref error);

            if (!string.IsNullOrWhiteSpace(error))
                return this.Content(JsonHelper.GetBaseMessage(false, error));

            //成功
            var data = new
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                User = member.NickName,
                Image = MemberExtensions.GetHeadImage(member.HeadImage)
            };

            return this.Content(JsonHelper.GetBaseMessage(string.IsNullOrWhiteSpace(error), data.SerializeObject()));
        }


        #endregion

        #region 右边栏
        public ActionResult _RightSidebar()
        {
            //热门活动
            List<MeetingModel> list = this.MeetingService.QueryHotMeeting().Select(m => m.ToModel()).ToList();

            return View(list);
        }


        #endregion

    }
}

