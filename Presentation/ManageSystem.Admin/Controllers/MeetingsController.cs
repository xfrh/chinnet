using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Admin.Extensions;
using ManageSystem.Framework.Controllers;
using ManageSystem.Framework.Kendoui;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Core;
using ManageSystem.Services.Meetings;
using ManageSystem.Admin.Models.Meetings;
using ManageSystem.Core.Domain.Meetings;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Utility;
using ManageSystem.Services.SystemSet;
using System.Text.RegularExpressions;
using ManageSystem.Services.Satellites;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Admin.Controllers
{

    public class MeetingsController : AdminBaseController
    {
        private readonly IMeetingTypeService MeetingTypeService;
        private readonly IMeetingService MeetingService;
        private readonly IMeetingApplyService MeetingApplyService;
        private readonly IMeetingViewService MeetingViewService;
        private readonly IMeetingCollectService MeetingCollectService;
        private readonly IMeetingCommentService MeetingCommentService;
        private readonly IAreaService AreaService;
        private readonly ISatelliteService SatelliteService;
        private readonly IUserRoleService UserRoleService;
        private readonly IRoleService RoleService;
        public MeetingsController(
                     IMeetingTypeService _meetingTypeService,
                     IMeetingService _meetingService,
                     IMeetingApplyService _meetingApplyService,
                     IMeetingViewService _meetingViewService,
                     IMeetingCollectService _meetingCollectService,
                     IMeetingCommentService _meetingCommentService,
                     IAreaService _areaService,
                     ISatelliteService _satelliteService,
                      IUserRoleService _userRoleService,
            IRoleService _roleService

        )
        {
            this.MeetingTypeService = _meetingTypeService;
            this.MeetingService = _meetingService;
            this.MeetingApplyService = _meetingApplyService;
            this.MeetingCollectService = _meetingCollectService;
            this.MeetingViewService = _meetingViewService;
            this.MeetingCommentService = _meetingCommentService;
            this.AreaService = _areaService;
            SatelliteService = _satelliteService;
            UserRoleService = _userRoleService;
            RoleService = _roleService;
        }

        #region 信息动态类型

        /// <summary>
        /// 信息动态类型 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingTypeList()
        {
            return View();
        }

        /// <summary>
        /// 信息动态类型 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingTypeList(DataSourceRequest command, MeetingTypeModel model)
        {
            var user = base.LoginUserinfo;
            Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(user.Id.ToString())));
            bool isSa = false;
            if (!string.IsNullOrEmpty(role.SatelliteId))
            {
                isSa = true;
            }
            //获得数据
            var list = isSa ? this.MeetingTypeService.QueryPageSa(model.Name, user.Describe, command.Page - 1, command.PageSize) : this.MeetingTypeService.QueryPage(model.Name, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Name = x.GetFormattedBreadCrumb(this.MeetingTypeService),
                        Sort = x.Sort,
                        Describe = string.IsNullOrEmpty(x.Describe) ? "" : x.Describe.Split('^')[0]
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 信息动态类型 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingTypeModel SetMeetingTypeCreateData()
        {
            MeetingTypeModel model = new MeetingTypeModel();

            //设置上级功能的下拉列表
            PrepareAllMeetingTypeModel(model);
            model.Sort = 100;

            return model;
        }

        /// <summary>
        /// 信息动态类型 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingTypeCreate()
        {

            return View(this.SetMeetingTypeCreateData());

        }

        /// <summary>
        /// 信息动态类型 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingTypeCreate(MeetingTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = base.LoginUserinfo;
                Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(user.Id.ToString())));
                if (!string.IsNullOrEmpty(role.SatelliteId))
                {
                    model.Describe += ("^" + user.Describe);
                }
                else
                {
                    model.Describe += "^";
                }
                var entity = model.ToEntity();
                this.MeetingTypeService.Insert(entity);

                string logContent = "添加信息动态类型，会议名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingTypeList");
            }

            return View(this.SetMeetingTypeCreateData());
        }


        /// <summary>
        /// 信息动态类型 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingTypeModel SetMeetingTypeEditData(long id)
        {
            var entity = this.MeetingTypeService.QueryEntity(id);

            var model = entity.ToModel();

            PrepareAllMeetingTypeModel(model);

            return model;
        }

        /// <summary>
        /// 信息动态类型 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingTypeEdit(long id)
        {
            return this.View(this.SetMeetingTypeEditData(id));
        }

        /// <summary>
        /// 信息动态类型 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingTypeEdit(MeetingTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MeetingTypeService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.MeetingTypeService.Update(entity);

                string logContent = "修改信息动态类型，会议名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingTypeList");
            }
            return this.View(this.SetMeetingTypeEditData(model.Id));
        }


        /// <summary>
        /// 信息动态类型 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingTypeView(long id)
        {
            return this.View(this.SetMeetingTypeEditData(id));
        }


        /// <summary>
        /// 信息动态类型 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingTypeDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MeetingTypeList");
            }

            this.MeetingTypeService.Delete(selectedIds);

            string logContent = "【手动】删除信息动态类型，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MeetingTypeList");
        }


        /// <summary>
        /// 加载所有的功能集合
        /// </summary>
        /// <param name="model"></param>
        [NonAction]
        protected virtual void PrepareAllMeetingTypeModel(MeetingTypeModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.MeetingTypeList.Add(new SelectListItem
            {
                Text = "顶级功能",
                Value = "0"
            });

            var functionList = this.MeetingTypeService.Query(null, 0, false, p => p.Sort);
            foreach (var c in functionList)
            {
                model.MeetingTypeList.Add(new SelectListItem
                {
                    Text = c.GetFormattedBreadCrumb(functionList),
                    Value = c.Id.ToString(),
                    Selected = c.Id == model.Id
                });
            }
        }

        #endregion

        #region 信息动态

        /// <summary>
        /// 信息动态 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingList()
        {
            return View(this.SetMeetingList());
        }

        /// <summary>
        /// 信息动态 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingList(DataSourceRequest command, MeetingSeachModel model)
        {
            var user = base.LoginUserinfo;
            Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(user.Id.ToString())));
            bool isSa = false;
            if (!string.IsNullOrEmpty(role.SatelliteId))
            {
                isSa = true;
            }

            //获得数据
            var list = isSa ? this.MeetingService.QueryPageSa(model.Name, user.Describe, model.MeetingTypeId, model.Type, model.MemberName, model.Contact, model.StartTime, model.EndTime, command.Page - 1, command.PageSize) : this.MeetingService.QueryPage(model.Name, model.MeetingTypeId, model.Type, model.MemberName, model.Contact, model.StartTime, model.EndTime, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Type = ((MeetingTypeEnum)x.Type).GetDescription(),
                        StartTime = x.StartTime.GetNormalString("L"),
                        EndTime = x.EndTime.GetNormalString("L"),
                        Contact = x.Contact,
                        MemberName = x.Author,
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Area = x.AreaName,
                        MeetingType = this.MeetingTypeService.GetTypeName(x.MeetingTypeId),
                        ViewCount = x.ViewCount,
                        ApplyCount = x.ApplyCount,
                        CommentCount = x.CommentCount,
                        CollectCount = x.CollectCount
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <returns></returns>
        public MeetingSeachModel SetMeetingList()
        {
            MeetingSeachModel model = new MeetingSeachModel();
            model.MeetingTypeList = this.MeetingTypeService.Query().OrderBy(m => m.Sort).
            Select(x =>
            { return new SelectListItem { Text = x.Name, Value = x.Id.ToString() }; }
            ).ToList();

            model.MeetingTypeList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "" });

            model.TypeEnumList = MeetingTypeEnum.Charge.ToSelectList().ToList();

            return model;
        }

        /// <summary>
        /// 信息动态 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingModel SetMeetingCreateData()
        {
            MeetingModel model = new MeetingModel();
            //设置上级功能的下拉列表
            model.TypeEnumList = MeetingTypeEnum.Free.ToSelectList(true, false).ToList();
            model.MemberName = base.LoginUserinfo.Name;

            model.MeetingTypeList = this.MeetingTypeService.Query(null, 0, false, p => p.Sort).Select(x =>
            {
                return
                        new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        };
            }).ToList();

            model.StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
            model.EndTime = Convert.ToDateTime(DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd 00:00"));

            return model;
        }

        /// <summary>
        /// 信息动态 创建
        /// </summary>
        /// <returns></returns>

        public ActionResult MeetingCreate()
        {
            return View(this.SetMeetingCreateData());
        }

        /// <summary>
        /// 信息动态 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MeetingCreate(MeetingModel model, HttpPostedFileBase coverImage)
        {
            var user = base.LoginUserinfo;
            Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(user.Id.ToString())));
            if (!string.IsNullOrEmpty(role.SatelliteId))
            {
                model.Describe += ("^" + user.Describe);
            }
            else
            {
                model.Describe += "^";
            }

            model.CoverImage = this.UploadCoverImage(coverImage);

            var member = base.LoginUserinfo;
            try
            {
                var entity = model.ToEntity();
                entity.Status = (int)MeetingStatusEnum.Finish;
                entity.MemberId = member.Id;
                entity.MemberName = member.Name;
                entity.StartTime = DateTime.Parse("1900-01-01");
                entity.EndTime = DateTime.Parse("1900-01-01");
                entity.MemberId = member.Id;
                entity.MemberName = member.Name;
                entity.Author = !string.IsNullOrWhiteSpace(model.Author) ? model.Author : member.Name;

                this.MeetingService.Insert(entity);

                string logContent = "添加信息动态，会议主题：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingCreate");
            }
            catch (Exception ex)
            {

            }

            return View(this.SetMeetingCreateData());
        }


        /// <summary>
        /// 信息动态 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingModel SetMeetingEditData(long id)
        {
            var entity = this.MeetingService.QueryEntity(id);

            var model = entity.ToModel();
            model.MeetingTypeList = this.MeetingTypeService.Query(null, 0, false, p => p.Sort).Select(x =>
            {
                return
                        new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        };
            }).ToList();


            model.TypeEnumList = ((MeetingTypeEnum)model.Type).ToSelectList(true, false).ToList();

            return model;
        }

        /// <summary>
        /// 信息动态 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingEdit(long id)
        {
            return this.View(this.SetMeetingEditData(id));
        }

        /// <summary>
        /// 信息动态 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MeetingEdit(MeetingModel model, HttpPostedFileBase coverImage)
        {
            var image = this.UploadCoverImage(coverImage);
            if (!string.IsNullOrWhiteSpace(image))
                model.CoverImage = image;

            model.MemberName = model.MemberId.ToString();
            if (model.StartTime > model.EndTime)
                ModelState.AddModelError("EndTime", "开始时间不能大于结束时间");

            ModelState.Remove("PersonMaxCount");

            if (ModelState.IsValid)
            {
                var entity = this.MeetingService.QueryEntity(model.Id);
                if (string.IsNullOrWhiteSpace(model.CoverImage))
                    model.CoverImage = entity.CoverImage;

                base.SetDefaultValue(model, entity);
                entity.Name = model.Name;
                entity.MeetingTypeId = model.MeetingTypeId;
                entity.CoverImage = model.CoverImage;
                entity.Content = model.Content;
                entity.Author = model.Author;

                this.MeetingService.Update(entity);

                string logContent = "修改信息动态，会议主题：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingList");
            }
            return this.View(this.SetMeetingEditData(model.Id));
        }


        /// <summary>
        /// 信息动态 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingView(long id)
        {
            return this.View(this.SetMeetingEditData(id));
        }


        /// <summary>
        /// 信息动态 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MeetingList");
            }

            this.MeetingService.Delete(selectedIds);

            string logContent = "【手动】删除信息动态，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MeetingList");
        }



        /// <summary>
        /// 上传文章封面图片
        /// </summary>
        /// <param name="coverImage"></param>
        /// <returns></returns>
        private string UploadCoverImage(HttpPostedFileBase coverImage)
        {
            string result = "";

            //上传封面图片
            if (coverImage != null)
            {
                if (coverImage.ContentLength <= 0)
                {
                    ModelState.AddModelError("CoverImage", "封面图片格式或其他原因不正确");
                    return "";
                }

                string path = "/Content/Upload/Meetings";
                UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(coverImage, path, 0, UpLoadType.Image, "");
                if (uploadResult == null || !uploadResult.State)
                {
                    ModelState.AddModelError("CoverImage", "封面图片格式或其他原因不正确");
                }
                else
                {
                    result = path + "/" + uploadResult.Name;
                }
            }

            return result;
        }

        #endregion

        #region 信息动态报名

        /// <summary>
        /// 信息动态报名 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingApplyList()
        {
            return View();
        }

        /// <summary>
        /// 信息动态报名 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingApplyList(DataSourceRequest command, MeetingApplyModel model)
        {
            //获得数据
            var list = this.MeetingApplyService.QueryPage(model.MeetingId, model.MeetingName, model.Name, model.Phone, model.Email, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> meetingIds = data.Select(m => m.MeetingId).ToList();
            var meetingList = this.MeetingService.Query(m => meetingIds.Contains(m.Id));

            if (meetingList != null && meetingList.Any())
            {
                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.MeetingId).FirstOrDefault();
                    if (meetingTemp != null && meetingTemp.Id > 0)
                    {
                        item.MeetingName = meetingTemp.Name;
                        item.MeetingTypeName = ((MeetingTypeEnum)meetingTemp.Type).GetDescription();
                    }
                }
            }

            //封装返回的数据
            var gridModel = new DataSourceResult
            {
                Data = data.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        MeetingId = x.MeetingId.ToString(),
                        MeetingName = x.MeetingName,
                        MeetingTypeName = x.MeetingTypeName,
                        MemberId = x.MemberId.ToString(),
                        Name = x.Name,
                        Email = x.Email,
                        Phone = x.Phone,
                        StatusName = ((MeetingApplyStatusEnum)x.Status).GetDescription()
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 信息动态报名 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingApplyModel SetMeetingApplyCreateData()
        {
            MeetingApplyModel model = new MeetingApplyModel();

            return model;
        }

        /// <summary>
        /// 信息动态报名 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingApplyCreate()
        {
            return View(this.SetMeetingApplyCreateData());

        }

        /// <summary>
        /// 信息动态报名 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingApplyCreate(MeetingApplyModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MeetingApplyService.Insert(entity);

                string logContent = "添加信息动态申请 ";
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingApplyList");
            }

            return View(this.SetMeetingApplyCreateData());
        }


        /// <summary>
        /// 信息动态报名 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingApplyModel SetMeetingApplyEditData(long id)
        {
            var entity = this.MeetingApplyService.QueryEntity(id);

            var model = entity.ToModel();
            model.MeetingApplyStatusList = ((MeetingApplyStatusEnum)model.Status).ToSelectList(false, true).ToList();

            return model;
        }

        /// <summary>
        /// 信息动态报名 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingApplyEdit(long id)
        {
            return this.View(this.SetMeetingApplyEditData(id));
        }

        /// <summary>
        /// 信息动态报名 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MeetingApplyEdit(MeetingApplyModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || !(new Regex(@"^\S{2,}$").IsMatch(model.Name)))
                this.ModelState.AddModelError("Name", "姓名格式不正确，必须大于等于2位");

            if (!RegexHelper.IsPhone(model.Phone))
                this.ModelState.AddModelError("Phone", "手机号码格式不正确");

            if (!RegexHelper.IsEmail(model.Email))
                this.ModelState.AddModelError("Email", "邮箱格式不正确");

            if (ModelState.IsValid)
            {
                var entity = this.MeetingApplyService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);

                entity.Name = model.Name;
                entity.Status = model.Status;
                entity.Phone = model.Phone;
                entity.Email = model.Email;
                entity.Remark = model.Remark;
                if (model.Status != entity.Status)
                    entity.StatusTime = DateTime.Now;

                this.MeetingApplyService.Update(entity);

                string logContent = "修改信息动态报名，申请人姓名：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingApplyList");
            }
            return this.View(this.SetMeetingApplyEditData(model.Id));
        }


        /// <summary>
        /// 信息动态报名 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingApplyView(long id)
        {
            return this.View(this.SetMeetingApplyEditData(id));
        }


        /// <summary>
        /// 信息动态报名 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingApplyDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MeetingApplyList");
            }

            this.MeetingApplyService.Delete(selectedIds);

            string logContent = "【手动】删除信息动态报名，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MeetingApplyList");
        }


        #endregion

        #region 信息动态查看记录

        /// <summary>
        /// 信息动态查看记录 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingViewList()
        {
            return View();
        }

        /// <summary>
        /// 信息动态查看记录 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingViewList(DataSourceRequest command, MeetingViewModel model)
        {
            //获得数据
            var list = this.MeetingViewService.QueryPage(model.MeetingId, model.MeetingName, model.MemberName, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> meetingIds = data.Select(m => m.MeetingId).ToList();
            var meetingList = this.MeetingService.Query(m => meetingIds.Contains(m.Id));

            if (meetingList != null && meetingList.Any())
            {
                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.MeetingId).FirstOrDefault();
                    if (meetingTemp != null && meetingTemp.Id > 0)
                        item.MeetingName = meetingTemp.Name;
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        MeetingId = x.MeetingId.ToString(),
                        MeetingName = x.MeetingName,
                        MemberId = x.MemberId.ToString(),
                        MemberName = x.MemberName,
                        Ip = x.Ip,
                        BrowserName = x.BrowserName
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 信息动态查看记录 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingViewModel SetMeetingViewEditData(long id)
        {
            var entity = this.MeetingViewService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 信息动态查看记录 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingViewView(long id)
        {
            return this.View(this.SetMeetingViewEditData(id));
        }

        /// <summary>
        /// 信息动态查看记录 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingViewDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MeetingViewList");
            }

            this.MeetingViewService.Delete(selectedIds);

            string logContent = "【手动】删除信息动态查看记录，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MeetingViewList");
        }


        #endregion

        #region 信息动态收藏记录

        /// <summary>
        /// 信息动态收藏记录 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingCollectList()
        {
            return View();
        }

        /// <summary>
        /// 信息动态收藏记录 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingCollectList(DataSourceRequest command, MeetingCollectModel model)
        {
            //获得数据
            var list = this.MeetingCollectService.QueryPage(model.MeetingId, model.MeetingName, model.MemberName, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> meetingIds = data.Select(m => m.MeetingId).ToList();
            var meetingList = this.MeetingService.Query(m => meetingIds.Contains(m.Id));

            if (meetingList != null && meetingList.Any())
            {
                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.MeetingId).FirstOrDefault();
                    if (meetingTemp != null && meetingTemp.Id > 0)
                        item.MeetingName = meetingTemp.Name;
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        MeetingId = x.MeetingId.ToString(),
                        MeetingName = x.MeetingName,
                        MemberId = x.MemberId.ToString(),
                        MemberName = x.MemberName,
                        Ip = x.Ip,
                        BrowserName = x.BrowserName
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 信息动态收藏记录 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingCollectModel SetMeetingCollectEditData(long id)
        {
            var entity = this.MeetingCollectService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 信息动态收藏记录 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingCollectView(long id)
        {
            return this.View(this.SetMeetingCollectEditData(id));
        }

        /// <summary>
        /// 信息动态收藏记录 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingCollectDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MeetingCollectList");
            }

            this.MeetingCollectService.Delete(selectedIds);

            string logContent = "【手动】删除信息动态收藏记录，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MeetingCollectList");
        }


        #endregion

        #region 信息动态评论记录

        /// <summary>
        /// 信息动态评论记录 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingCommentList()
        {
            return View();
        }

        /// <summary>
        /// 信息动态评论记录 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingCommentList(DataSourceRequest command, MeetingCommentModel model)
        {

            //获得数据
            var list = this.MeetingCommentService.QueryPage(model.MeetingId, model.MeetingName, model.MemberName, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> meetingIds = data.Select(m => m.MeetingId).ToList();
            var meetingList = this.MeetingService.Query(m => meetingIds.Contains(m.Id));

            if (meetingList != null && meetingList.Any())
            {
                foreach (var item in data)
                {
                    var meetingTemp = meetingList.Where(m => m.Id == item.MeetingId).FirstOrDefault();
                    if (meetingTemp != null && meetingTemp.Id > 0)
                        item.MeetingName = meetingTemp.Name;
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        MeetingId = x.MeetingId.ToString(),
                        MeetingName = x.MeetingName,
                        MemberId = x.MemberId.ToString(),
                        MemberName = x.MemberName,
                        Content = x.Content,
                        TypeName = ((MeetingCommentTypeEnum)x.Type).GetDescription()
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 信息动态评论记录 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingCommentModel SetMeetingCommentCreateData()
        {
            MeetingCommentModel model = new MeetingCommentModel();

            return model;
        }

        /// <summary>
        /// 信息动态评论记录 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingCommentCreate()
        {
            return View(this.SetMeetingCommentCreateData());

        }

        /// <summary>
        /// 信息动态评论记录 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingCommentCreate(MeetingCommentModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.MeetingCommentService.Insert(entity);

                string logContent = "添加信息动态收藏记录 ";
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingCommentList");
            }

            return View(this.SetMeetingCommentCreateData());
        }


        /// <summary>
        /// 信息动态评论记录 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public MeetingCommentModel SetMeetingCommentEditData(long id)
        {
            var entity = this.MeetingCommentService.QueryEntity(id);

            var model = entity.ToModel();
            model.TypeName = ((MeetingCommentTypeEnum)model.Type).GetDescription();

            return model;
        }

        /// <summary>
        /// 信息动态评论记录 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingCommentEdit(long id)
        {
            return this.View(this.SetMeetingCommentEditData(id));
        }

        /// <summary>
        /// 信息动态评论记录 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingCommentEdit(MeetingCommentModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.MeetingCommentService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);

                entity.Content = model.Content;

                this.MeetingCommentService.Update(entity);

                string logContent = "修改信息动态评论记录，评论内容：" + entity.Content;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("MeetingCommentList");
            }
            return this.View(this.SetMeetingCommentEditData(model.Id));
        }


        /// <summary>
        /// 信息动态评论记录 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult MeetingCommentView(long id)
        {
            return this.View(this.SetMeetingCommentEditData(id));
        }


        /// <summary>
        /// 信息动态评论记录 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MeetingCommentDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("MeetingCommentList");
            }

            this.MeetingCommentService.Delete(selectedIds);

            string logContent = "【手动】删除信息动态评论记录，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("MeetingCommentList");
        }


        #endregion

    }
}
