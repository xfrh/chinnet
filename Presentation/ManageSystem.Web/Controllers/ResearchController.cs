using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Research;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Upload;
using ManageSystem.Services.Members;
using ManageSystem.Services.Researches;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Members;
using ManageSystem.Web.Models.Researches;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class ResearchController : WebBaseController
    {
        private readonly IResearchTypeService ResearchTypeService;
        private readonly IResearchService ResearchService;
        private readonly IAreaService AreaService;
        private readonly IMemberService MemberService;
        private readonly IResearchViewService ResearchViewService;
        private readonly IResearchCollectService ResearchCollectService;
        private readonly IResearchApplyService ResearchApplyService;
        private readonly IResearchCommentService ResearchCommentService;

        public ResearchController(
            IResearchTypeService _researchTypeService,
                IResearchService _researchService,
                IAreaService _areaService,
                IMemberService _memberService,
               IResearchViewService _researchViewService,
                 IResearchCollectService _researchCollectService,
                   IResearchApplyService _researchApplyService,
                       IResearchCommentService _researchCommentService

        )
        {
            this.ResearchTypeService = _researchTypeService;
            this.ResearchService = _researchService;
            this.AreaService = _areaService;
            this.MemberService = _memberService;
            this.ResearchViewService = _researchViewService;
            this.ResearchCollectService = _researchCollectService;
            this.ResearchApplyService = _researchApplyService;
            this.ResearchCommentService = _researchCommentService;
        }

        #region 会议首页数据

        public ActionResult Index(IndexSearchModel model)
        {
            var result = this.SetIndexData(model);
            // result.SearchModel.Order = "time";

            return View(result);
        }

        private IndexModel SetIndexData(IndexSearchModel searchModel)
        {
            IndexModel model = new IndexModel();
            model.SearchModel = searchModel ?? new IndexSearchModel();
            model.SearchResearchTypeList = this.ResearchTypeService.Query(m => m.ResearchTypeId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //科研合作的类型（顶级）
            model.SearchAreaList = this.AreaService.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m => m.Sort).ToList();    //区域列表（顶级，省份）
            model.SearchDayList = this.GetSearchDayList();  //时间查询条件
            model.SearchOrderList = this.GetSearchOrderList(); //排序条件
            //分页数据
            model.PageList = this.ResearchService.Query(searchModel.Type, searchModel.Area, searchModel.Day, searchModel.Order).ToPagedList(searchModel.PageIndex, 12); //获取查询数据

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

        private List<IndexSearchOrder> GetSearchOrderList()
        {
            List<IndexSearchOrder> list = new List<IndexSearchOrder>();
            list.Add(new IndexSearchOrder() { Text = "按时间排序", Value = 0 });
            list.Add(new IndexSearchOrder() { Text = "按热度排序", Value = 1 });

            return list;
        }

        #endregion

        #region 发布科研页面

        public ActionResult Create()
        {
            return View(this.SetCreateData());
        }

        private ResearchModel SetCreateData()
        {
            ResearchModel model = new ResearchModel();

            model.StartTime = DateTime.Now;
            model.ResearchTypeList = new List<SelectListItem>();
            model.ResearchTypeList = this.ResearchTypeService.Query(m => m.Mark > 0 && m.ResearchTypeId == 0).OrderBy(m => m.Sort).Select(x =>
            {
                return new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                };
            }).ToList();
            model.ResearchTypeList.Insert(0, new SelectListItem()
            {
                Text = "请选择",
                Value = "0"
            });
            return model;
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResearchModel model)
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
                    entity.Status = (int)ResearchStatusEnum.Finish;
                    entity.Grade = 3;

                    var areaEntity = this.AreaService.QueryEntity(entity.AreaId);
                    if (areaEntity != null && areaEntity.Id > 0)
                    {
                        entity.AreaName = areaEntity.Name;
                        string[] tempArray = areaEntity.Position.Split(',');
                        entity.AreaProvinceId = long.Parse(tempArray[1]);
                    }

                    this.ResearchService.Insert(entity);

                    string logContent = "创建科研合作，科研主题：" + model.Name;
                    base.InsetActionLog(ActionType.Create, logContent, model.SerializeObject());
                    base.SuccessNotification("您的申请提交成功！");

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
                FilePath = "/Content/Upload/Research/",
                HttpFile = this.HttpContext.Request.Files,
                MaxLength = 2 * 1024, //10M
                NewFileName = Guid.NewGuid() + "_" + new Random().Next(10000, 99999),
                Type = UploadTypeEnum.Image
            });

            return this.Content(model.SerializeObject());
        }

        #endregion

        #region 科研详细页面

        public ActionResult Detail(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("科研不存在");

                ResearchModel model = this.ResearchService.QueryEntity(id).ToModel();
                if (model == null || model.Id <= 0) throw new Exception("科研不存在");

                if (model.Status != (int)ResearchStatusEnum.Finish) throw new Exception("科研不可用");

                var memberModel = base.LoginUserinfo;

                var memberEntity = this.MemberService.QueryEntity(model.MemberId);

                ResearchDetailModel resultModel = new ResearchDetailModel();
                resultModel.ResearchEntity = model; //会议数据
                resultModel.MemberEntity = base.LoginUserinfo;    //当前登录用户
                resultModel.IsCollect = this.ResearchCollectService.MemberCollectStatus(memberModel.Id, model.Id);     //是否已经收藏
                resultModel.IsApply = this.ResearchApplyService.MemberApplyStatus(memberModel.Id, model.Id);// 是否已经申请

                return View(resultModel);
            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex.Message);

                return this.RedirectToAction("Index");
            }

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

            if (model.ResearchId <= 0)
                return this.Content(JsonHelper.GetBaseMessage(false, "科研不存在"));
            if (string.IsNullOrEmpty(model.Name) || !(new Regex(@"^\S{2,}$").IsMatch(model.Name)))
                return this.Content(JsonHelper.GetBaseMessage(false, "姓名格式不正确"));
            if (!RegexHelper.IsPhone(model.Phone))
                return this.Content(JsonHelper.GetBaseMessage(false, "手机号码格式不正确"));
            if (!RegexHelper.IsEmail(model.Email))
                return this.Content(JsonHelper.GetBaseMessage(false, "邮箱格式不正确"));

            try
            {
                ResearchApply applyModel = this.ResearchApplyService.Insert(model.ResearchId, model.Name, model.Phone, model.Email, base.LoginUserinfo);
                if (applyModel != null && applyModel.Id > 0)
                {
                    return this.Content(JsonHelper.GetBaseMessage(true, ""));
                }

                return this.Content(JsonHelper.GetBaseMessage(false, "申请失败，请重试"));

            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
            }
        }

        /// <summary>
        /// 添加科研查看记录
        /// </summary>
        /// <param name="researchId"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddResearchView(long researchId)
        {
            if (researchId <= 0) return this.Content("");
            var member = base.LoginUserinfo;

            this.ResearchViewService.Insert(member.Id, member.Name, researchId);

            return this.Content("");
        }

        /// <summary>
        /// 收藏科研合作查看记录
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddResearchCollect(long researchId)
        {
            if (researchId <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "科研不存在"));
            var member = base.LoginUserinfo;

            string error = "";
            this.ResearchCollectService.Insert(member.Id, member.Name, researchId, ref error);

            return this.Content(JsonHelper.GetBaseMessage(string.IsNullOrWhiteSpace(error), error));
        }


        /// <summary>
        /// 获取科研合作的评论
        /// </summary>
        /// <param name="researchId"></param>
        /// <returns></returns>
        public ContentResult GetResearchComment(long researchId)
        {
            if (researchId <= 0) return this.Content(JsonHelper.GetBaseMessage(false, "科研不存在"));

            var list = this.ResearchCommentService.Query(m => m.ResearchId == researchId && m.Mark > 0 && m.ParentId == 0).OrderByDescending(m => m.InsertTime).Select(x => x.ToModel()).ToList();

            foreach (var item in list)
            {
                MemberModel member = this.MemberService.QueryEntity(item.MemberId).ToModel();
                member = member ?? new MemberModel();

                item.MemberHeadImage = MemberExtensions.GetHeadImage(member.HeadImage);
                item.MemberNickName = member.NickName;
                item.InsertTimeString = item.InsertTime.ToString("yyyy-MM-dd HH:mm");

                SetResearchComment(item);
            }

            return this.Content(JsonHelper.GetBaseMessage(true, list.SerializeObject()));

        }


        /// <summary>
        /// 通过递归获取评论的数据
        /// </summary>
        /// <param name="model"></param>
        [NonAction]
        private void SetResearchComment(ResearchCommentModel model)
        {
            var itemList = this.ResearchCommentService.Query(m => m.ParentId == model.Id && m.Mark > 0).OrderByDescending(m => m.InsertTime).ToList();
            if (itemList == null || !itemList.Any()) return;

            model.ItemCommentList = itemList.Select(x => x.ToModel()).ToList();

            foreach (var item in model.ItemCommentList)
            {
                MemberModel member = this.MemberService.QueryEntity(item.MemberId).ToModel();
                member = member ?? new MemberModel();

                item.MemberHeadImage = MemberExtensions.GetHeadImage(member.HeadImage);
                item.MemberNickName = member.NickName;
                item.InsertTimeString = item.InsertTime.ToString("yyyy-MM-dd HH:mm");

                SetResearchComment(item);
            }

        }


        /// <summary>
        /// 用户提交评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult AddComment(ResearchCommentModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Content)) return this.Content(JsonHelper.GetBaseMessage(false, "请输入评论内容！"));
            var member = base.LoginUserinfo;

            string error = "";
            model.Type = (int)ResearchCommentTypeEnum.Member;
            model.MemberId = member.Id;
            model.MemberName = member.Name;

            this.ResearchCommentService.Insert(model.ToEntity(), ref error);

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

    }

}