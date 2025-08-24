using ManageSystem.Admin.Models.SHChart;
using ManageSystem.Core.Domain.SHChart;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.DynamicLinq;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Services.SHChart;
using ManageSystem.Services.Members;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class MgtSHChartController : AdminBaseController
    {
        private readonly IMemberService _memberService;
        private readonly ISHDataSegmentService _SHdataSegmentService;
        private readonly ISHBarChartService _barSHChartService;
        private readonly ISHBarChartWithItemDataService _barSHChartWithItemDataService;


        #region 构造器
        public MgtSHChartController(IMemberService memberService, ISHDataSegmentService dataSegmentService, ISHBarChartService barChartService, ISHBarChartWithItemDataService barChartWithItemDataService)
        {
            this._memberService = memberService;
            this._SHdataSegmentService = dataSegmentService;
            this._barSHChartService = barChartService;
            this._barSHChartWithItemDataService = barChartWithItemDataService;
        }
        #endregion




        #region 数据段管理

        #region 判断数据段重名
        /// <summary>
        /// 判断数据段是否重名
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public JsonResult DataSegment_OnCheck(SHDataSegmentModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                Expression<Func<Chart_SHDataSegment, bool>> predicate = r => r.Name == model.Name && r.Mark > 0 && r.ProjectType == model.ProjectType;
                if (model.Id > 0)
                {
                    predicate = predicate.And(r => r.Id != model.Id);
                }
                return Json(_SHdataSegmentService.Count(predicate) == 0);
            }

            return Json(false);
        }
        #endregion

        #region 新增提交
        /// <summary>
        /// 新增数据段
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult DataSegment_OnCreateSubmit(SHDataSegmentModel model)
        {
            if (ModelState.IsValid)
            {
                Chart_SHDataSegment entity = new Chart_SHDataSegment
                {
                    Id = CommonHelper.GuidToLongID,
                    Name = model.Name,
                    Sort = model.Sort,
                    ProjectType = model.ProjectType,
                    InsertTime = DateTime.Now,
                    Describe = null,
                    Mark = 1,
                    Version = 1,
                    Data_type=model.Data_type
                    
                };
                entity.UpdateTime = entity.InsertTime;

                _SHdataSegmentService.Insert(entity);
                string logContent = $"数据段添加成功，名称：【{model.Name}】";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Create, logContent);
            }
            else
            {
                base.ErrorNotification($"信息填写不正确");
            }

            switch (model.ProjectType)
            {              
                case SHDataSegmentEnum.SHBarChart:
                    return RedirectToAction("SHBarChartList");
                default:
                    return RedirectToAction("SHBarChartList");
            }
        }
        #endregion

        #region 编辑提交
        /// <summary>
        /// 提交修改数据段
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult DataSegment_OnEditSubmit(SHDataSegmentModel model)
        {

            if (ModelState.IsValid)
            {
                Chart_SHDataSegment entity = _SHdataSegmentService.QueryEntity(model.Id);
                if (entity != null && entity.Id > 0 && entity.Mark > 0 && entity.ProjectType == model.ProjectType)
                {
                    // 判断重名
                    if (_SHdataSegmentService.Count(r => r.ProjectType == model.ProjectType && r.Mark > 0 && r.Id != entity.Id && r.Name == model.Name) == 0)
                    {
                        entity.Name = model.Name;
                        entity.Sort = model.Sort;
                        _SHdataSegmentService.Update(entity);
                        string logContent = $"数据段修改成功，名称：【{model.Name}】";
                        base.SuccessNotification(logContent);
                        base.InsetActionLog(ActionType.Edit, logContent);
                    }
                    else
                    {
                        base.ErrorNotification($"此数据段名称已存在");
                    }
                }
                else
                {
                    base.ErrorNotification($"提交参数不正确，请刷新后重试！");
                }
            }
            else
            {
                base.ErrorNotification($"信息填写不正确");
            }

            switch (model.ProjectType)
            {               
                case SHDataSegmentEnum.SHBarChart:
                    return RedirectToAction("SHBarChartList");
                default:
                    return Redirect("/");
            }
        }
        #endregion

        #region 删除数据段
        /// <summary>
        /// 删除数据段
        /// </summary>
        /// <param name="id">数据段id</param>
        /// <param name="type">数据段所属项目类型</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult DataSegment_OnDelete(long id, SHDataSegmentEnum type)
        {
            switch (type)
            {              
                case SHDataSegmentEnum.SHBarChart:
                    {
                        if (_SHdataSegmentService.OnDeleteByBarSHChartWithTransaction(id))
                        {
                            string logContent = $"【手动】已成功删除数据段";
                            base.SuccessNotification(logContent);
                            InsetActionLog(ActionType.Delete, logContent);
                        }
                        else
                        {
                            ErrorNotification("删除失败");
                        }
                        return RedirectToAction("SHBarChartList");
                    }
                default:
                    return Redirect("/");
            }
        }
        #endregion

        #region 设置数据段显示/隐藏
        /// <summary>
        /// 设置数据段显示/隐藏
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult DataSegment_SettingDisplay(SHDataSegmentModel model)
        {
            try
            {
                Chart_SHDataSegment entity = _SHdataSegmentService.QueryEntity(model.Id);
                if (entity != null && entity.Id > 0 && entity.Mark > 0 && entity.ProjectType == model.ProjectType)
                {
                    entity.Display = model.Display;
                    _SHdataSegmentService.Update(entity);

                    string logContent = $"设置数据段【{entity.Name}】为{(entity.Display ? "显示" : "隐藏")}状态";
                    base.SuccessNotification(logContent);
                    base.InsetActionLog(ActionType.Edit, logContent);
                }
                else
                {
                    base.ErrorNotification($"提交参数不正确，请刷新后重试！");
                }
            }
            catch (Exception)
            {
                base.ErrorNotification($"数据段状态变更失败");
            }

            switch (model.ProjectType)
            {               
                case SHDataSegmentEnum.SHBarChart:
                    return RedirectToAction("SHBarChartList");
                default:
                    return Redirect("/");
            }
        }
        #endregion

        #endregion


        #region 列表
        /// <summary>
        /// 柱状图列表
        /// </summary>
        /// <param name="page">分页索引</param>
        /// <returns></returns>
        public ActionResult SHBarChartList(int page = 1)
        {
            page = page > 1 ? page : 1;
            Expression<Func<Chart_SHDataSegment, bool>> predicate = r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart;
            IEnumerable<SHBarChartListDTO> dataSource = _SHdataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new SHBarChartListDTO
            {
                Id = item.Id,
                Name = item.Name,
                ProjectType = item.ProjectType,
                Sort = item.Sort,
                Display = item.Display,
                DataItems = _barSHChartService.Query(r => r.DataSegmentId == item.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ThenBy(r => r.Id).Select(r => new SHBarChartItemDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Title = r.Title,
                    SubTitle = r.SubTitle,
                    Sort = r.Sort,
                    Display = r.Display,
                    Default = r.Default
                }),
                Data_type=item.Data_type
            });
            IPagedList<SHBarChartListDTO> model = new PagedList<SHBarChartListDTO>(dataSource, page, 5);
            return View(model);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增柱状图
        /// </summary>
        /// <param name="id">数据段id</param>
        /// <returns></returns>
        public ActionResult SHBarChartCreate(long id)
        {
            SHBarChartModel model = new SHBarChartModel();
            model = InitSHBarChartCreate(id);
            return View(model);
            
        }

        /// <summary>
        /// 提交新增柱状图
        /// </summary>
        /// <param name="model">请求的数据</param>
        /// <param name="BarColor">柱状图颜色</param>
        /// <param name="ChartType">图表类型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHBarChartCreate(SHBarChartModel model, string BarColor, SHChartTypeEnum ChartType = SHChartTypeEnum.Bar)
        {
            try
            {
                model.Name = (model.Name ?? "").Trim();

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    ModelState.AddModelError("Name", "报表名称不能为空");
                }

                // 判断报表名称是否存在
                if (!string.IsNullOrWhiteSpace(model.Name) && _barSHChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Name == model.Name && r.Mark > 0) > 0)
                {
                    ModelState.AddModelError("Name", "此报表名称已经存在");
                }

                if (model.DataItemType == ChartSHDataItemType.MultipleData)
                {
                    if (model.Antibiotics == null || model.Antibiotics.Count == 0)
                    {
                        ModelState.AddModelError("Antibiotics", "请添加更多列");
                    }
                }

                if (model.ItemModel == null || model.ItemModel.Count == 0)
                {
                    ModelState.AddModelError("ItemModel", "请填写报表数据项");
                }

                if (ModelState.IsValid)
                {
                    Chart_SHBarChart mainEntity = new Chart_SHBarChart
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataSegmentId = model.DataSegmentId,
                        Name = (model.Name ?? "").Trim(),
                        Title = (model.Title ?? "").Trim(),
                        SubTitle = (model.SubTitle ?? "").Trim(),
                        Sort = model.Sort,
                        Display = model.Display,
                        Unit = (model.Unit ?? "%").Trim(),
                        MobileDisplayScale = model.MobileDisplayScale,
                        DataItemType = model.DataItemType,
                        Antibiotics = model.DataItemType == ChartSHDataItemType.MultipleData ? model.Antibiotics.SerializeObject() : null,
                        Mark = 1,
                        Version = 1,
                        Describe = null,
                        InsertTime = DateTime.Now,
                        DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0)
                    };
                    mainEntity.UpdateTime = mainEntity.InsertTime;

                    // 关联数据项集合
                    List<Chart_SHBarChartWithItemData> itemEntities = new List<Chart_SHBarChartWithItemData>();

                    switch (model.DataItemType)
                    {
                        case ChartSHDataItemType.SingleData:
                            {
                                foreach (SHBarChartDataItemModel item in model.ItemModel)
                                {
                                    if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                    {
                                        itemEntities.Add(new Chart_SHBarChartWithItemData
                                        {
                                            Id = CommonHelper.GuidToLongID,
                                            SHBarChartId = mainEntity.Id,
                                            ChartType = ChartType,
                                            ChartColor = BarColor,
                                            Name = item.Name,
                                            Value = item.Value,
                                            Sort = item.Sort,
                                            Display = item.Display,
                                            Mark = 1,
                                            Version = 1,
                                            Describe = null,
                                            DeleteTime = mainEntity.DeleteTime,
                                            InsertTime = mainEntity.InsertTime,
                                            UpdateTime = mainEntity.UpdateTime
                                        });
                                    }
                                }
                            }
                            break;
                        case ChartSHDataItemType.MultipleData:
                            {
                                foreach (SHBarChartDataItemModel item in model.ItemModel)
                                {
                                    if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                    {
                                        item.Antibiotics = item.Antibiotics.Where(r => model.Antibiotics.Select(x => x.Guid).Contains(r.Guid)).ToList();
                                        item.Antibiotics.ForEach(x => { x.Name = model.Antibiotics.Where(r => r.Guid == x.Guid).FirstOrDefault().Name; });

                                        item.Value = item.Antibiotics.SerializeObject();

                                        itemEntities.Add(new Chart_SHBarChartWithItemData
                                        {
                                            Id = CommonHelper.GuidToLongID,
                                            SHBarChartId = mainEntity.Id,
                                            ChartType = item.ChartType,
                                            ChartColor = item.BarColor,
                                            Name = item.Name,
                                            Value = item.Value,
                                            Sort = item.Sort,
                                            Display = item.Display,
                                            Mark = 1,
                                            Version = 1,
                                            Describe = null,
                                            DeleteTime = mainEntity.DeleteTime,
                                            InsertTime = mainEntity.InsertTime,
                                            UpdateTime = mainEntity.UpdateTime
                                        });
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    if (itemEntities.Count > 0)
                    {
                        if (/*_barSHChartService.UpdateSort(model.Sort) > 0 &&*/ _barSHChartService.OnCreateBarChartByTransaction(mainEntity, itemEntities))
                        {
                            string logContent = $"柱状图报表添加成功，报表名称：【{model.Name}】";
                            base.SuccessNotification(logContent);
                            base.InsetActionLog(ActionType.Create, logContent);
                            return RedirectToAction("SHBarChartCreate", new { id = mainEntity.DataSegmentId });
                        }
                        else
                        {
                            base.ErrorNotification("保存柱状图报表失败");
                        }
                    }
                    else
                    {
                        base.ErrorNotification("请完善报表相关数据项");
                    }
                }
                else
                {
                    string errorMessage = ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        errorMessage = "请完善报表相关数据项";
                    }
                    base.ErrorNotification(errorMessage);
                }
            }
            catch (Exception)
            {
                base.ErrorNotification("报表相关数据项提交失败");
            }


            model.DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == model.DataSegmentId
            }).ToList();

            return View(model);
        }

        public ActionResult SHBarChartCreateTable(long id)
        {
            SHBarChartTableModel model = new SHBarChartTableModel();
            model = InitSHTableCreate(id);
            return View(model);

        }

        /// <summary>
        /// 提交新增柱状图
        /// </summary>
        /// <param name="model">请求的数据</param>
        /// <param name="BarColor">柱状图颜色</param>
        /// <param name="ChartType">图表类型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHBarChartCreateTable(SHBarChartTableModel model)
        {          
            try
            {
                model.Name = (model.Name ?? "").Trim();

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    ModelState.AddModelError("Name", "报表名称不能为空");
                }

                // 判断报表名称是否存在
                if (!string.IsNullOrWhiteSpace(model.Name) && _barSHChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Name == model.Name && r.Mark > 0) > 0)
                {
                    ModelState.AddModelError("Name", "此报表名称已经存在");
                }
                string[] MICNames = model.MICName.Split(',');
                string[] MIC_Ranges = model.MIC_Range.Split(',');
                string[] MIC50s = model.MIC50.Split(',');
                string[] MIC90s = model.MIC90.Split(',');
                string[] Ss = model.S.Split(',');
                string[] SDDs = model.SDD.Split(',');
                string[] Is = model.I.Split(',');
                string[] Rs = model.R.Split(',');
                List<SHBarChartDataItemTableModel> list = new List<SHBarChartDataItemTableModel>();
               
                for (int i = 0; i < MICNames.Length-1; i++)
                {
                    SHBarChartDataItemTableModel SHmodel = new SHBarChartDataItemTableModel();
                    SHmodel.Name = MICNames[i].ToString();
                    SHmodel.MIC_Range = MIC_Ranges[i].ToString();
                    SHmodel.MIC50 = MIC50s[i].ToString();
                    SHmodel.MIC90 = MIC90s[i].ToString();
                    SHmodel.S = Ss[i].ToString();
                    SHmodel.SDD = SDDs[i].ToString();
                    SHmodel.I = Is[i].ToString();
                    SHmodel.R = Rs[i].ToString();
                    list.Add(SHmodel);
                }
                if (ModelState.IsValid)
                {
                    Chart_SHBarChart mainEntity = new Chart_SHBarChart
                    {
                        Id = CommonHelper.GuidToLongID,
                        DataSegmentId = model.DataSegmentId,
                        Name = (model.Name ?? "").Trim(),
                        Title = (model.Title ?? "").Trim(),
                        SubTitle = null,
                        Sort = model.Sort,
                        Display = model.Display,
                        Unit = null,
                        MobileDisplayScale = model.MobileDisplayScale,
                        DataItemType = model.DataItemType,
                        Antibiotics = list.SerializeObject(),
                        Mark = 1,
                        Version = 1,
                        Describe = null,
                        InsertTime = DateTime.Now,
                        DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0)
                    };
                    mainEntity.UpdateTime = mainEntity.InsertTime;                                     
                        if (_barSHChartService.OnCreateTableTransaction(mainEntity))
                        {
                            string logContent = $"表格报表添加成功，报表名称：【{model.Name}】";
                            base.SuccessNotification(logContent);
                            base.InsetActionLog(ActionType.Create, logContent);
                            return RedirectToAction("SHBarChartCreateTable", new { id = mainEntity.DataSegmentId });
                        }
                        else
                        {
                            base.ErrorNotification("保存表格图报表失败");
                        }                   
                }
                else
                {
                    string errorMessage = ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        errorMessage = "请完善报表相关数据项";
                    }
                    base.ErrorNotification(errorMessage);
                }
            }
            catch (Exception)
            {
                //throw;
                base.ErrorNotification("报表相关数据项提交失败");
            }


            model.DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == model.DataSegmentId
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// 初始化新增表格
        /// </summary>
        /// <param name="dsid">数据段id</param>
        /// <returns></returns>
        private SHBarChartTableModel InitSHTableCreate(long dsid)
        {
            SHBarChartTableModel model = new SHBarChartTableModel();
            model.Id = CommonHelper.GuidToLongID;
            model.DataSegmentId = dsid;
            model.DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == dsid
            }).ToList();
            model.Name = null;
            model.Title = null;
            model.SubTitle = null;
            model.Sort = _barSHChartService.NextSort(dsid);
            model.Display = true;
            model.Unit = "%";
            model.MobileDisplayScale = 100;
            model.DataItemType = ChartSHDataItemType.SingleData;
            return model;
        }


        /// <summary>
        /// 初始化新增柱状图
        /// </summary>
        /// <param name="dsid">数据段id</param>
        /// <returns></returns>
        private SHBarChartModel InitSHBarChartCreate(long dsid)
        {
            SHBarChartModel model = new SHBarChartModel();           
                model.Id = CommonHelper.GuidToLongID;
                model.DataSegmentId = dsid;
                model.DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == dsid
                }).ToList();
                model.Name = null;
                model.Title = null;
                model.SubTitle = null;
                model.Sort = _barSHChartService.NextSort(dsid);
                model.Display = true;
                model.Unit = "%";
                model.MobileDisplayScale = 100;            
                model.DataItemType = ChartSHDataItemType.SingleData;    
                return model;           
        }

        #endregion

        #region 编辑
        /// <summary>
        /// 编辑柱状图
        /// </summary>
        /// <param name="id">柱状图报表id</param>
        /// <returns></returns>
        public ActionResult SHBarChartEdit(long id)
        {
            var mainEntity = _barSHChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("SHBarChartList");
            }
            SHBarChartModel model = InitSHBarChartEdit(mainEntity);
            return View(model);
        }

        /// <summary>
        /// 提交编辑柱状图
        /// </summary>
        /// <param name="model">请求的数据</param>
        /// <param name="BarColor">柱状图颜色</param>
        /// <param name="ChartType">图表类型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHBarChartEdit(SHBarChartModel model, string BarColor, SHChartTypeEnum ChartType = SHChartTypeEnum.Bar)
        {
            model.Name = (model.Name ?? "").Trim();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "报表名称不能为空");
            }

            // 判断报表名称是否存在
            if (!string.IsNullOrWhiteSpace(model.Name) && _barSHChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Id != model.Id && r.Name == model.Name && r.Mark > 0) > 0)
            {
                ModelState.AddModelError("Name", "此报表名称已经存在");
            }

            Chart_SHBarChart mainEntity = _barSHChartService.QueryEntity(model.Id);
            if (mainEntity == null || mainEntity.Id <= 0 || mainEntity.Mark <= 0)
            {
                ModelState.AddModelError("Name", "此报表不存在或已经被删除");
            }

            if (ModelState.IsValid)
            {
                mainEntity.Name = model.Name;
                mainEntity.Title = model.Title;
                mainEntity.SubTitle = model.SubTitle;
                mainEntity.Sort = model.Sort;
                mainEntity.Display = model.Display;
                mainEntity.Unit = model.Unit;
                mainEntity.MobileDisplayScale = model.MobileDisplayScale;

                List<Chart_SHBarChartWithItemData> itemEntities = new List<Chart_SHBarChartWithItemData>();

                switch (model.DataItemType)
                {
                    case ChartSHDataItemType.SingleData:
                        {
                            foreach (SHBarChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    itemEntities.Add(new Chart_SHBarChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        SHBarChartId = mainEntity.Id,
                                        ChartType = ChartType,
                                        ChartColor = BarColor,
                                        Name = item.Name,
                                        Value = item.Value,
                                        Sort = item.Sort,
                                        Display = item.Display,
                                        Mark = 1,
                                        Version = 1,
                                        Describe = null,
                                        DeleteTime = mainEntity.DeleteTime,
                                        InsertTime = mainEntity.InsertTime,
                                        UpdateTime = mainEntity.UpdateTime
                                    });
                                }
                            }
                        }
                        break;
                    case ChartSHDataItemType.MultipleData:
                        {
                            mainEntity.Antibiotics = model.Antibiotics.SerializeObject();
                            foreach (SHBarChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    item.Antibiotics = item.Antibiotics.Where(r => model.Antibiotics.Select(x => x.Guid).Contains(r.Guid)).ToList();
                                    item.Antibiotics.ForEach(x => { x.Name = model.Antibiotics.Where(r => r.Guid == x.Guid).FirstOrDefault().Name; });
                                    item.Value = item.Antibiotics.SerializeObject();

                                    itemEntities.Add(new Chart_SHBarChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        SHBarChartId = mainEntity.Id,
                                        ChartType = item.ChartType,
                                        ChartColor = item.BarColor,
                                        Name = item.Name,
                                        Value = item.Value,
                                        Sort = item.Sort,
                                        Display = item.Display,
                                        Mark = 1,
                                        Version = 1,
                                        Describe = null,
                                        DeleteTime = mainEntity.DeleteTime,
                                        InsertTime = mainEntity.InsertTime,
                                        UpdateTime = mainEntity.UpdateTime
                                    });
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

                if (itemEntities.Count > 0)
                {
                    if (/*_barSHChartService.UpdateSort(model.Sort) > 0 &&*/ _barSHChartService.OnEditBarChartByTransaction(mainEntity, itemEntities))
                    {
                        string logContent = $"柱状图报表编辑成功，报表名称：【{model.Name}】";
                        base.SuccessNotification(logContent);
                        base.InsetActionLog(ActionType.Create, logContent);
                        return RedirectToAction("SHBarChartEdit", new { id = mainEntity.Id });
                    }
                    else
                    {
                        base.ErrorNotification("保存柱状图报表失败");
                    }
                }
                else
                {
                    base.ErrorNotification("请完善报表相关数据项");
                }
            }
            else
            {
                string errorMessage = ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "请完善报表相关数据项";
                }
                base.ErrorNotification(errorMessage);
            }

            model.DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == model.DataSegmentId
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// 编辑柱状图初始化
        /// </summary>
        /// <param name="mainEntity">柱状图报表实体</param>
        /// <returns></returns>
        private SHBarChartModel InitSHBarChartEdit(Chart_SHBarChart mainEntity)
        {
            SHBarChartModel model = new SHBarChartModel
            {
                Id = mainEntity.Id,
                DataSegmentId = mainEntity.DataSegmentId,
                DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == mainEntity.DataSegmentId
                }).ToList(),
                Name = mainEntity.Name,
                Title = mainEntity.Title,
                SubTitle = mainEntity.SubTitle,
                Sort = mainEntity.Sort,
                Display = mainEntity.Display,
                Unit = mainEntity.Unit,
                MobileDisplayScale = mainEntity.MobileDisplayScale,
                DataItemType = mainEntity.DataItemType,
                Antibiotics = mainEntity.DataItemType == ChartSHDataItemType.MultipleData ? mainEntity.Antibiotics.DeserializeObject<List<SHBarChartDataItemWithAntibioticModel>>() : new List<SHBarChartDataItemWithAntibioticModel>(),
                IsEditPage = true
            };
            model.ItemModel = _barSHChartWithItemDataService.Query(r => r.SHBarChartId == mainEntity.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenByDescending(r => r.UpdateTime).Select(item => new SHBarChartDataItemModel
            {
                Id = item.Id,
                ChartType = item.ChartType,
                BarColor = item.ChartColor,
                Name = item.Name,
                Value = item.Value,
                Display = item.Display,
                Sort = item.Sort
            }).ToList();

            return model;
        }

        /// <summary>
        /// 编辑table表格
        /// </summary>
        /// <param name="id">柱状图报表id</param>
        /// <returns></returns>
        public ActionResult SHBarChartEditTable(long id)
        {
            var mainEntity = _barSHChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("SHBarChartList");
            }
            SHBarChartTableModel model = InitSHBarChartEditTable(mainEntity);
            return View(model);
        }

        /// <summary>
        /// 提交编辑柱状图
        /// </summary>
        /// <param name="model">请求的数据</param>
        /// <param name="BarColor">柱状图颜色</param>
        /// <param name="ChartType">图表类型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHBarChartEditTable(SHBarChartTableModel model)
        {
            model.Name = (model.Name ?? "").Trim();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "报表名称不能为空");
            }

            //// 判断报表名称是否存在
            //if (!string.IsNullOrWhiteSpace(model.Name) && _barSHChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Id != model.Id && r.Name == model.Name && r.Mark > 0) > 0)
            //{
            //    ModelState.AddModelError("Name", "此报表名称已经存在");
            //}

            Chart_SHBarChart mainEntity = _barSHChartService.QueryEntity(model.Id);
            if (mainEntity == null || mainEntity.Id <= 0 || mainEntity.Mark <= 0)
            {
                ModelState.AddModelError("Name", "此报表不存在或已经被删除");
            }
            string[] MICNames = model.MICName.Split(',');
            string[] MIC_Ranges = model.MIC_Range.Split(',');
            string[] MIC50s = model.MIC50.Split(',');
            string[] MIC90s = model.MIC90.Split(',');
            string[] Ss = model.S.Split(',');
            string[] SDDs = model.SDD.Split(',');
            string[] Is = model.I.Split(',');
            string[] Rs = model.R.Split(',');
            List<SHBarChartDataItemTableModel> list = new List<SHBarChartDataItemTableModel>();         
            for (int i = 0; i < MICNames.Length - 1; i++)
            {
                SHBarChartDataItemTableModel SHmodel = new SHBarChartDataItemTableModel();
                SHmodel.Name = MICNames[i].ToString();
                SHmodel.MIC_Range = MIC_Ranges[i].ToString();
                SHmodel.MIC50 = MIC50s[i].ToString();
                SHmodel.MIC90 = MIC90s[i].ToString();
                SHmodel.S = Ss[i].ToString();
                SHmodel.SDD = SDDs[i].ToString();
                SHmodel.I = Is[i].ToString();
                SHmodel.R = Rs[i].ToString();
                list.Add(SHmodel);
            }
            if (ModelState.IsValid)
            {
                mainEntity.Name = model.Name;
                mainEntity.Title = model.Title;
                mainEntity.SubTitle = model.SubTitle;
                mainEntity.Sort = model.Sort;
                mainEntity.Display = model.Display;
                mainEntity.Unit = model.Unit;
                mainEntity.MobileDisplayScale = model.MobileDisplayScale;
                mainEntity.Antibiotics = list.SerializeObject();
                mainEntity.UpdateTime = DateTime.Now;
                mainEntity.Id = model.Id;
              
                    if (_barSHChartService.OnEditTableTransaction(mainEntity))
                    {
                        string logContent = $"表格报表编辑成功，报表名称：【{model.Name}】";
                        base.SuccessNotification(logContent);
                        base.InsetActionLog(ActionType.Create, logContent);
                        return RedirectToAction("SHBarChartEditTable", new { id = mainEntity.Id });
                    }
                    else
                    {
                        base.ErrorNotification("保存柱状图报表失败");
                    }                
            }
            else
            {
                string errorMessage = ModelState.Values.Where(r => r.Errors.Count > 0).FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = "请完善报表相关数据项";
                }
                base.ErrorNotification(errorMessage);
            }

            model.DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == model.DataSegmentId
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// 编辑柱状图初始化
        /// </summary>
        /// <param name="mainEntity">柱状图报表实体</param>
        /// <returns></returns>
        private SHBarChartTableModel InitSHBarChartEditTable(Chart_SHBarChart mainEntity)
        {
            SHBarChartTableModel model = new SHBarChartTableModel
            {
                Id = mainEntity.Id,
                DataSegmentId = mainEntity.DataSegmentId,
                DropDataSegment = _SHdataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == mainEntity.DataSegmentId
                }).ToList(),
                Name = mainEntity.Name,
                Title = mainEntity.Title,
                SubTitle = mainEntity.SubTitle,
                Sort = mainEntity.Sort,
                Display = mainEntity.Display,
                Unit = mainEntity.Unit,
                MobileDisplayScale = mainEntity.MobileDisplayScale,
                DataItemType = mainEntity.DataItemType,
                Antibiotics = mainEntity.Antibiotics.DeserializeObject<List<SHBarChartDataItemTableModel>>(),            
                IsEditPage = true
            };
             return model;
        }
        #endregion

        #region 查看
        /// <summary>
        /// 柱状图报表详情
        /// </summary>
        /// <param name="id">柱状图报表id</param>
        /// <returns></returns>
        public ActionResult SHBarChartDetail(long id)
        {
            var mainEntity = _barSHChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("SHBarChartList");
            }

            var model = InitSHBarChartEdit(mainEntity);
            return View(model);
        }


        /// <summary>
        /// 柱状图报表详情
        /// </summary>
        /// <param name="id">柱状图报表id</param>
        /// <returns></returns>
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHTableDetail(long id)
        {
            var mainEntity = _barSHChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("SHBarChartList");
            }

            var model = InitSHBarChartEditTable(mainEntity);
            return View(model);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除柱状图报表
        /// </summary>
        /// <param name="id">要删除的柱状图报表id</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHBarChart_OnDelete(long id)
        {
            var entity = _barSHChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                if (_barSHChartService.OnDeleteBarChartByTransaction(id))
                {
                    string logContent = $"【手动】成功删除报表，报表名称：【{entity.Name}】";
                    base.SuccessNotification(logContent);
                    base.InsetActionLog(ActionType.Delete, logContent);
                }
            }
            else
            {
                ErrorNotification("删除报表失败，报表数据不存在或已删除");
            }
            return RedirectToAction("SHBarChartList");
        }
        
        #endregion

        #region 设置状态显示/不显示
        /// <summary>
        /// 设置柱状图报表id
        /// </summary>
        /// <param name="id">柱状图报表id</param>
        /// <param name="status">是否显示</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHBarChart_OnSetDisplay(long id, bool status)
        {
            var entity = _barSHChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Display = status;
                entity.UpdateTime = DateTime.Now;
                _barSHChartService.Update(entity);
                string logContent = $"成功设置报表【{entity.Name}】的状态为：【{(status ? "显示" : "不显示")}】";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("状态变更失败");
            }
            return RedirectToAction("SHBarChartList");
        }
        #endregion

        #region 单数据Excel导入
        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public JsonResult SHBarChartSingleExcel_OnSubmit(HttpPostedFileWrapper file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return Json(new { status = false, message = "未能读取数据文件" });
            }

            string fileSuffix = System.IO.Path.GetExtension(file.FileName);
            if (!new List<string> { "XLS", ".XLS", "XLSX", ".XLSX" }.Contains(fileSuffix.ToUpperInvariant()))
            {
                return Json(new { status = false, message = "请使用指定的数据模板上传" });
            }
            DataTable table = new DataTable();

            switch (fileSuffix.ToUpperInvariant())
            {
                case "XLS":
                case ".XLS":
                    table = ImportDataTable.ExcelToDataTable(file.InputStream, ExcelEnum.Excel2003, true, 2);
                    break;
                case "XLSX":
                case ".XLSX":
                    ExcelPackage excelPackage = new ExcelPackage(file.InputStream);
                    if (excelPackage == null || excelPackage.Workbook == null || excelPackage.Workbook.Worksheets == null || excelPackage.Workbook.Worksheets.Count == 0)
                    {
                        return Json(new { status = false, message = "未能读取数据文件" });
                    }
                    table = EPPlusHelper.WorksheetToTable(worksheet: excelPackage.Workbook.Worksheets[1], startRow: 2);
                    break;
                default:
                    break;
            }
            if (table == null || table.Rows.Count == 0)
            {
                return Json(new { status = false, message = "未能读取到导入的数据" });
            }
            List<dynamic> data = new List<dynamic>();

            int rowIndex = 1;
            int _scale = 1;
            string _strValue = "";
            foreach (DataRow dataRow in table.Rows)
            {
                string _name = (dataRow["数据项名称"] ?? "").ToString().Trim();
                string _value = (dataRow["数值"] ?? "").ToString().Trim();
                string _display = (dataRow["是否显示"] ?? "").ToString().Trim();
                string _sort = (dataRow["显示顺序"] ?? "").ToString().Trim();
                _scale = 1;
                _strValue = "";
                if (string.IsNullOrWhiteSpace(_name) || string.IsNullOrWhiteSpace(_value) || !_value.IsNumeric())
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(_sort) || !_sort.IsNumeric())
                {
                    _sort = rowIndex.ToString();
                }

                if (_sort.IsInt(out int sortValue))
                {
                    if (sortValue <= 0)
                    {
                        _sort = rowIndex.ToString();
                    }
                    else
                    {
                        _sort = sortValue.ToString();
                    }
                }

                if (double.TryParse(_value, out double __value))
                {
                    _strValue = __value.ToString();

                    if (_strValue.Contains(".") && (_strValue.Length - _strValue.IndexOf(".") - 1) > _scale)
                    {
                        _scale = _strValue.Length - _strValue.IndexOf(".") - 1;
                    }

                    data.Add(new
                    {
                        uuid = Guid.NewGuid().ToString().ToLowerInvariant(),
                        name = _name,
                        value = __value.ToString(_scale == 2 ? "0.00" : "0.0"),
                        display = _display.Equals("否") ? false : true,
                        sort = _sort
                    });
                }

                rowIndex++;
            }

            if (data.Count == 0)
            {
                return Json(new { status = false, message = "请合理填写导入模板中的数据项名称和数值" });
            }

            return Json(new { status = true, message = "数据导入完成", data = data.OrderBy(r => r.sort).ThenBy(r => r.name) });
        }
        #endregion

        #region 多数据Excel导入
        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public JsonResult SHBarChartMultipleExcel_OnSubmit(HttpPostedFileWrapper file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return Json(new { status = false, message = "未能读取数据文件" });
            }

            string fileSuffix = System.IO.Path.GetExtension(file.FileName);
            if (!new List<string> { "XLS", ".XLS", "XLSX", ".XLSX" }.Contains(fileSuffix.ToUpperInvariant()))
            {
                return Json(new { status = false, message = "请使用指定的数据模板上传" });
            }

            DataTable table = new DataTable();

            switch (fileSuffix.ToUpperInvariant())
            {
                case "XLS":
                case ".XLS":
                    table = ImportDataTable.ExcelToDataTable(file.InputStream, ExcelEnum.Excel2003, true);
                    break;
                case "XLSX":
                case ".XLSX":
                    ExcelPackage excelPackage = new ExcelPackage(file.InputStream);
                    if (excelPackage == null || excelPackage.Workbook == null || excelPackage.Workbook.Worksheets == null || excelPackage.Workbook.Worksheets.Count == 0)
                    {
                        return Json(new { status = false, message = "未能读取数据文件" });
                    }
                    table = EPPlusHelper.WorksheetToTable(worksheet: excelPackage.Workbook.Worksheets[1]);
                    break;
                default:
                    break;
            }
            if (table == null || table.Rows.Count == 0)
            {
                return Json(new { status = false, message = "未能读取到导入的数据" });
            }

            if (table.Columns.Count <= 3)
            {
                return Json(new { status = false, message = "至少需要添加一列抗生素数据" });
            }

            List<SHBarChartDataItemWithAntibioticModel> antibiotics = new List<SHBarChartDataItemWithAntibioticModel>();

            List<string> fixedColumns = new List<string> { "数据项名称", "是否显示", "图表类型" };

            foreach (DataColumn item in table.Columns)
            {
                if (string.IsNullOrWhiteSpace(item.ColumnName ?? "") || RegexHelper.IsMatch(item.ColumnName.Trim(), @"[Cc]olumn\d+") || fixedColumns.Contains(item.ColumnName.Trim()))
                {
                    continue;
                }

                antibiotics.Add(new SHBarChartDataItemWithAntibioticModel
                {
                    Guid = Guid.NewGuid(),
                    Name = item.ColumnName.Trim(),
                    Value = null
                });
            }

            List<string> colors = new List<string> { "#015baa", "#c1232b", "#fe8463", "#ecbf00", "#cf7ca6", "#749f83" };
            List<dynamic> data = new List<dynamic>();
            int _rowIndex = 0;
            int _scale = 1;
            string _strValue = "";
            foreach (DataRow dataRow in table.Rows)
            {
                string _name = (dataRow["数据项名称"] ?? "").ToString().Trim();
                string _display = (dataRow["是否显示"] ?? "").ToString().Trim();
                string _chartType = (dataRow["图表类型"] ?? "").ToString().Trim();

                if (string.IsNullOrWhiteSpace(_name)) { continue; }

                List<SHBarChartDataItemWithAntibioticModel> _values = new List<SHBarChartDataItemWithAntibioticModel>();
                foreach (SHBarChartDataItemWithAntibioticModel antibioticItem in antibiotics)
                {
                    _scale = 1;
                    _strValue = "";

                    string _value = (dataRow[antibioticItem.Name] ?? "").ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(_value) && _value.IsNumeric())
                    {
                        if (double.TryParse(_value, out double __value))
                        {
                            if (__value >= 0.0)
                            {
                                _strValue = __value.ToString();
                                if (_strValue.Contains(".") && (_strValue.Length - _strValue.IndexOf(".") - 1) > _scale)
                                {
                                    _scale = _strValue.Length - _strValue.IndexOf(".") - 1;
                                }
                                _value = __value.ToString(_scale == 2 ? "0.00" : "0.0");
                            }
                            else
                            {
                                _value = "";
                            }
                        }
                        else
                        {
                            _value = "";
                        }
                    }
                    else
                    {
                        _value = "";
                    }

                    _values.Add(new SHBarChartDataItemWithAntibioticModel
                    {
                        Guid = antibioticItem.Guid,
                        Name = antibioticItem.Name,
                        Value = _value
                    });
                }

                data.Add(new
                {
                    uuid = Guid.NewGuid().ToString().ToLowerInvariant(),
                    name = _name,
                    display = _display.Equals("是"),
                    chartType = _chartType.Equals("折线图") ? 3 : 2,
                    color = colors.Count > _rowIndex ? colors[_rowIndex++] : GenerateColorTo16(),
                    values = _values
                });
            }

            if (data.Count == 0)
            {
                return Json(new { status = false, message = "请合理填写导入模板中数据，参考模板中的填写示例工作表" });
            }

            return Json(new
            {
                status = true,
                message = "数据导入完成",
                antibiotics,
                tbody = data
            });
        }
        #endregion

        #region 设置默认选中
        /// <summary>
        /// 设置默认选中
        /// </summary>
        /// <param name="id"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult SHBarChart_OnSettingDefault(long id, bool @default)
        {
            var entity = _barSHChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Default = @default;
                entity.UpdateTime = DateTime.Now;
                if (@default)
                {
                    _barSHChartService.Update(r => r.Default && r.Mark > 0, update => new Chart_SHBarChart { Default = false });
                }
                _barSHChartService.Update(entity);

                string logContent = $"{(@default ? "设置" : "取消")}报表【{entity.Name}】默认选中";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("操作失败");
            }
            return RedirectToAction("SHBarChartList");
        }
        #endregion


        #region 私有方法

        #region 私有属性
        /// <summary>
        /// 随随机生成颜色代码
        /// </summary>
        private readonly static List<string> _colorChartList = new List<string> {
            "0", "0", "0", "F", "0", "F",
            "1", "F", "1", "E", "1", "E",
            "2", "E", "2", "D", "2", "D",
            "3", "D", "3", "C", "3", "C",
            "4", "C", "4", "B", "4", "B",
            "5", "B", "A", "A", "5", "A",
            "6", "A", "B", "9", "6", "9",
            "7", "9", "C", "8", "7", "8",
            "8", "8", "8", "7", "8", "7",
            "9", "7", "9", "6", "9", "6",
            "A", "6", "5", "5", "A", "5",
            "B", "5", "6", "4", "B", "4",
            "C", "4", "7", "3", "C", "3",
            "D", "3", "D", "2", "D", "2",
            "E", "2", "E", "1", "E", "1",
            "F", "1", "F", "0", "F", "0"};
        #endregion

        /// <summary>
        /// 随机生成16进制颜色代码
        /// </summary>
        /// <returns></returns>
        private string GenerateColorTo16()
        {
            return $"#{string.Join("", _colorChartList.OrderBy(item => Guid.NewGuid()).Skip(6).Take(6))}";
        }
        #endregion

    }
}