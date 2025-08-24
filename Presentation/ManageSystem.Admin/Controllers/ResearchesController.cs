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
using ManageSystem.Services.Researches;
using ManageSystem.Admin.Models.Researches;
using ManageSystem.Core.Domain.Researches;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Members;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Extensions;
using System.Text.RegularExpressions;

namespace ManageSystem.Admin.Controllers
{

    public class ResearchesController : AdminBaseController
    {

        private readonly IResearchTypeService ResearchTypeService;
        private readonly IResearchService ResearchService;
        private readonly IAreaService AreaService;
        private readonly IMemberService MemberService;
        private readonly IResearchViewService ResearchViewService;
        private readonly IResearchCollectService ResearchCollectService;
        private readonly IResearchApplyService ResearchApplyService;
        private readonly IResearchCommentService ResearchCommentService;

        public ResearchesController(
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

        #region 科研合作类型

        /// <summary>
        /// 科研合作类型 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchTypeList()
        {
            return View();
        }

        /// <summary>
        /// 科研合作类型 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchTypeList(DataSourceRequest command, ResearchTypeModel model)
        {

            //获得数据
            var list = this.ResearchTypeService.QueryPage(command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Sort = x.Sort,
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm")
                    };
                }),
                Total = list.TotalCount
            };


            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 科研合作类型 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchTypeModel SetResearchTypeCreateData()
        {
            ResearchTypeModel model = new ResearchTypeModel();

            return model;
        }

        /// <summary>
        /// 科研合作类型 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchTypeCreate()
        {
            return View(this.SetResearchTypeCreateData());

        }

        /// <summary>
        /// 科研合作类型 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchTypeCreate(ResearchTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.ResearchTypeService.Insert(entity);

                string logContent = "添加科研合作类型 ";
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchTypeCreate");
            }

            return View(this.SetResearchTypeCreateData());
        }


        /// <summary>
        /// 科研合作类型 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchTypeModel SetResearchTypeEditData(long id)
        {
            var entity = this.ResearchTypeService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 科研合作类型 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchTypeEdit(long id)
        {
            return this.View(this.SetResearchTypeEditData(id));
        }

        /// <summary>
        /// 科研合作类型 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchTypeEdit(ResearchTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.ResearchTypeService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.ResearchTypeService.Update(entity);

                string logContent = "修改科研合作类型 ";
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchTypeList");
            }
            return this.View(this.SetResearchTypeEditData(model.Id));
        }


        /// <summary>
        /// 科研合作类型 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchTypeView(long id)
        {
            return this.View(this.SetResearchTypeEditData(id));
        }


        /// <summary>
        /// 科研合作类型 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchTypeDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ResearchTypeList");
            }

            this.ResearchTypeService.Delete(selectedIds);

            string logContent = "【手动】删除科研合作类型，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ResearchTypeList");
        }


        #endregion

        #region 科研合作

        /// <summary>
        /// 科研合作列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchList()
        {
            return View(this.SetMeetingList());
        }

        /// <summary>
        /// 科研合作 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchList(DataSourceRequest command, ResearchSeachModel model)
        {

            //获得数据
            var list = this.ResearchService.QueryPage(model.Name, model.ResearchTypeId, model.MemberName, model.StartTime, model.EndTime, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        StartTime = x.StartTime.GetNormalString("L"),
                        MemberName = x.MemberName,
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Area = x.AreaName,
                        ResearchType = this.ResearchTypeService.GetTypeName(x.ResearchTypeId),
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
        public ResearchSeachModel SetMeetingList()
        {
            ResearchSeachModel model = new ResearchSeachModel();
            model.ResearchTypeList = this.ResearchTypeService.Query().OrderBy(m => m.Sort).
            Select(x =>
            { return new SelectListItem { Text = x.Name, Value = x.Id.ToString() }; }
            ).ToList();

            model.ResearchTypeList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "" });

            return model;
        }

        /// <summary>
        /// 科研合作 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchModel SetResearchCreateData()
        {
            ResearchModel model = new ResearchModel(); ;

            model.ResearchTypeList = this.ResearchTypeService.Query(null, 0, false, p => p.Sort).Select(x =>
            {
                return
                        new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        };
            }).ToList();

            model.StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00"));

            return model;
        }

        /// <summary>
        /// 科研合作类型 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCreate()
        {
            return View(this.SetResearchCreateData());
        }

        /// <summary>
        /// 科研合作 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResearchCreate(ResearchModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.ResearchService.Insert(entity);

                string logContent = "添加科研合作，科研名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchCreate");
            }

            return View(this.SetResearchCreateData());
        }


        /// <summary>
        /// 科研合作 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchModel SetResearchEditData(long id)
        {
            var entity = this.ResearchService.QueryEntity(id);

            var model = entity.ToModel();
            model.ResearchTypeList = this.ResearchTypeService.Query(null, 0, false, p => p.Sort).Select(x =>
            {
                return
                        new SelectListItem()
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        };
            }).ToList();

            return model;
        }

        /// <summary>
        /// 科研合作 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchEdit(long id)
        {
            return this.View(this.SetResearchEditData(id));
        }

        /// <summary>
        /// 科研合作 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResearchEdit(ResearchModel model, HttpPostedFileBase coverImage)
        {
            var image = this.UploadCoverImage(coverImage);
            if (!string.IsNullOrWhiteSpace(image))
                model.CoverImage = image;

            model.MemberName = model.MemberId.ToString();

            if (ModelState.IsValid)
            {
                var entity = this.ResearchService.QueryEntity(model.Id);
                if (string.IsNullOrWhiteSpace(model.CoverImage))
                    model.CoverImage = entity.CoverImage;

                base.SetDefaultValue(model, entity);
                entity.Name = model.Name;
                entity.Code = model.Code;
                entity.Author = model.Author;
                entity.Require = model.Require;
                entity.Remark = model.Remark;
                entity.StartTime = model.StartTime;
                entity.ResearchTypeId = model.ResearchTypeId;
                entity.CoverImage = model.CoverImage;
                entity.Content = model.Content;

                this.ResearchService.Update(entity);

                string logContent = "修改科研合作，科研主题：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchList");
            }
            return this.View(this.SetResearchEditData(model.Id));
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

                string path = "/Content/Upload/Research";
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

        /// <summary>
        /// 科研合作 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchView(long id)
        {
            return this.View(this.SetResearchEditData(id));
        }


        /// <summary>
        /// 科研合作 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ResearchList");
            }

            this.ResearchService.Delete(selectedIds);

            string logContent = "【手动】删除科研合作，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ResearchList");
        }


        #endregion

        #region 科研报名记录

        /// <summary>
        /// 科研合作报名 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchApplyList()
        {
            return View();
        }

        /// <summary>
        /// 科研合作报名 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchApplyList(DataSourceRequest command, ResearchApplyModel model)
        {
            //获得数据
            var list = this.ResearchApplyService.QueryPage(model.ResearchId, model.ResearchName, model.Name, model.Phone, model.Email, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> ResearchIds = data.Select(m => m.ResearchId).ToList();
            var ResearchList = this.ResearchService.Query(m => ResearchIds.Contains(m.Id));

            if (ResearchList != null && ResearchList.Any())
            {
                foreach (var item in data)
                {
                    var ResearchTemp = ResearchList.Where(m => m.Id == item.ResearchId).FirstOrDefault();
                    if (ResearchTemp != null && ResearchTemp.Id > 0)
                    {
                        item.ResearchName = ResearchTemp.Name;
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
                        ResearchId = x.ResearchId.ToString(),
                        ResearchName = x.ResearchName,
                        ResearchTypeName = x.ResearchTypeName,
                        MemberId = x.MemberId.ToString(),
                        Name = x.Name,
                        Email = x.Email,
                        Phone = x.Phone,
                        StatusName = ((ResearchApplyStatusEnum)x.Status).GetDescription()
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 科研合作报名 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchApplyModel SetResearchApplyCreateData()
        {
            ResearchApplyModel model = new ResearchApplyModel();

            return model;
        }

        /// <summary>
        /// 科研合作报名 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchApplyCreate()
        {
            return View(this.SetResearchApplyCreateData());

        }

        /// <summary>
        /// 科研合作报名 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchApplyCreate(ResearchApplyModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.ResearchApplyService.Insert(entity);

                string logContent = "添加科研合作申请 ";
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchApplyCreate");
            }

            return View(this.SetResearchApplyCreateData());
        }


        /// <summary>
        /// 科研合作报名 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchApplyModel SetResearchApplyEditData(long id)
        {
            var entity = this.ResearchApplyService.QueryEntity(id);

            var model = entity.ToModel();
            model.ResearchApplyStatusList = ((ResearchApplyStatusEnum)model.Status).ToSelectList(false, true).ToList();

            return model;
        }

        /// <summary>
        /// 科研合作报名 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchApplyEdit(long id)
        {
            return this.View(this.SetResearchApplyEditData(id));
        }

        /// <summary>
        /// 科研合作报名 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResearchApplyEdit(ResearchApplyModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || !(new Regex(@"^\S{2,}$").IsMatch(model.Name)))
                this.ModelState.AddModelError("Name", "姓名格式不正确，必须大于等于2位");

            if (!RegexHelper.IsPhone(model.Phone))
                this.ModelState.AddModelError("Phone", "手机号码格式不正确");

            if (!RegexHelper.IsEmail(model.Email))
                this.ModelState.AddModelError("Email", "邮箱格式不正确");

            if (ModelState.IsValid)
            {
                var entity = this.ResearchApplyService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);

                entity.Name = model.Name;
                entity.Status = model.Status;
                entity.Phone = model.Phone;
                entity.Email = model.Email;
                entity.Remark = model.Remark;
                if (model.Status != entity.Status)
                    entity.StatusTime = DateTime.Now;

                this.ResearchApplyService.Update(entity);

                string logContent = "修改科研合作报名，申请人姓名：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchApplyList");
            }
            return this.View(this.SetResearchApplyEditData(model.Id));
        }


        /// <summary>
        /// 科研合作报名 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchApplyView(long id)
        {
            return this.View(this.SetResearchApplyEditData(id));
        }


        /// <summary>
        /// 科研合作报名 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchApplyDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ResearchApplyList");
            }

            this.ResearchApplyService.Delete(selectedIds);

            string logContent = "【手动】删除科研合作报名，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ResearchApplyList");
        }


        #endregion

        #region 科研合作查看记录

        /// <summary>
        /// 科研合作查看记录 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchViewList()
        {
            return View();
        }

        /// <summary>
        /// 科研合作查看记录 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchViewList(DataSourceRequest command, ResearchViewModel model)
        {
            //获得数据
            var list = this.ResearchViewService.QueryPage(model.ResearchId, model.ResearchName, model.MemberName, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> ResearchIds = data.Select(m => m.ResearchId).ToList();
            var ResearchList = this.ResearchService.Query(m => ResearchIds.Contains(m.Id));

            if (ResearchList != null && ResearchList.Any())
            {
                foreach (var item in data)
                {
                    var ResearchTemp = ResearchList.Where(m => m.Id == item.ResearchId).FirstOrDefault();
                    if (ResearchTemp != null && ResearchTemp.Id > 0)
                        item.ResearchName = ResearchTemp.Name;
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
                        ResearchId = x.ResearchId.ToString(),
                        ResearchName = x.ResearchName,
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
        /// 科研合作查看记录 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchViewModel SetResearchViewEditData(long id)
        {
            var entity = this.ResearchViewService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 科研合作查看记录 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchViewView(long id)
        {
            return this.View(this.SetResearchViewEditData(id));
        }

        /// <summary>
        /// 科研合作查看记录 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchViewDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ResearchViewList");
            }

            this.ResearchViewService.Delete(selectedIds);

            string logContent = "【手动】删除科研合作查看记录，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ResearchViewList");
        }


        #endregion

        #region 科研合作收藏记录

        /// <summary>
        /// 科研合作收藏记录 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCollectList()
        {
            return View();
        }

        /// <summary>
        /// 科研合作收藏记录 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchCollectList(DataSourceRequest command, ResearchCollectModel model)
        {
            //获得数据
            var list = this.ResearchCollectService.QueryPage(model.ResearchId, model.ResearchName, model.MemberName, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> ResearchIds = data.Select(m => m.ResearchId).ToList();
            var ResearchList = this.ResearchService.Query(m => ResearchIds.Contains(m.Id));

            if (ResearchList != null && ResearchList.Any())
            {
                foreach (var item in data)
                {
                    var ResearchTemp = ResearchList.Where(m => m.Id == item.ResearchId).FirstOrDefault();
                    if (ResearchTemp != null && ResearchTemp.Id > 0)
                        item.ResearchName = ResearchTemp.Name;
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
                        ResearchId = x.ResearchId.ToString(),
                        ResearchName = x.ResearchName,
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
        /// 科研合作收藏记录 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchCollectModel SetResearchCollectEditData(long id)
        {
            var entity = this.ResearchCollectService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 科研合作收藏记录 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCollectView(long id)
        {
            return this.View(this.SetResearchCollectEditData(id));
        }

        /// <summary>
        /// 科研合作收藏记录 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchCollectDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ResearchCollectList");
            }

            this.ResearchCollectService.Delete(selectedIds);

            string logContent = "【手动】删除科研合作收藏记录，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ResearchCollectList");
        }


        #endregion

        #region 科研合作评论记录

        /// <summary>
        /// 科研合作评论记录 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCommentList()
        {
            return View();
        }

        /// <summary>
        /// 科研合作评论记录 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchCommentList(DataSourceRequest command, ResearchCommentModel model)
        {

            //获得数据
            var list = this.ResearchCommentService.QueryPage(model.ResearchId, model.ResearchName, model.MemberName, command.Page - 1, command.PageSize);

            //填充一些数据
            var data = list.Select(x => x.ToModel()).ToList();
            IList<long> ResearchIds = data.Select(m => m.ResearchId).ToList();
            var ResearchList = this.ResearchService.Query(m => ResearchIds.Contains(m.Id));

            if (ResearchList != null && ResearchList.Any())
            {
                foreach (var item in data)
                {
                    var ResearchTemp = ResearchList.Where(m => m.Id == item.ResearchId).FirstOrDefault();
                    if (ResearchTemp != null && ResearchTemp.Id > 0)
                        item.ResearchName = ResearchTemp.Name;
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
                        ResearchId = x.ResearchId.ToString(),
                        ResearchName = x.ResearchName,
                        MemberId = x.MemberId.ToString(),
                        MemberName = x.MemberName,
                        Content = x.Content,
                        TypeName = ((ResearchCommentTypeEnum)x.Type).GetDescription()
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 科研合作评论记录 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchCommentModel SetResearchCommentCreateData()
        {
            ResearchCommentModel model = new ResearchCommentModel();

            return model;
        }

        /// <summary>
        /// 科研合作评论记录 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCommentCreate()
        {
            return View(this.SetResearchCommentCreateData());

        }

        /// <summary>
        /// 科研合作评论记录 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchCommentCreate(ResearchCommentModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.ResearchCommentService.Insert(entity);

                string logContent = "添加科研合作收藏记录 ";
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchCommentCreate");
            }

            return View(this.SetResearchCommentCreateData());
        }


        /// <summary>
        /// 科研合作评论记录 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ResearchCommentModel SetResearchCommentEditData(long id)
        {
            var entity = this.ResearchCommentService.QueryEntity(id);

            var model = entity.ToModel();
            model.TypeName = ((ResearchCommentTypeEnum)model.Type).GetDescription();

            return model;
        }

        /// <summary>
        /// 科研合作评论记录 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCommentEdit(long id)
        {
            return this.View(this.SetResearchCommentEditData(id));
        }

        /// <summary>
        /// 科研合作评论记录 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchCommentEdit(ResearchCommentModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.ResearchCommentService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);

                entity.Content = model.Content;

                this.ResearchCommentService.Update(entity);

                string logContent = "修改科研合作评论记录，评论内容：" + entity.Content;
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ResearchCommentList");
            }
            return this.View(this.SetResearchCommentEditData(model.Id));
        }


        /// <summary>
        /// 科研合作评论记录 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchCommentView(long id)
        {
            return this.View(this.SetResearchCommentEditData(id));
        }


        /// <summary>
        /// 科研合作评论记录 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResearchCommentDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ResearchCommentList");
            }

            this.ResearchCommentService.Delete(selectedIds);

            string logContent = "【手动】删除科研合作评论记录，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ResearchCommentList");
        }


        #endregion
    }

}
