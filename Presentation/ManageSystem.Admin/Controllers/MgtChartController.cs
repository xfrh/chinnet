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
using ManageSystem.Services.Chart;
using ManageSystem.Core.Domain.Chart;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Services.SystemSet;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ManageSystem.Admin.App_Start;
using ManageSystem.Services.Users;
using ManageSystem.Services.Members;
using ManageSystem.Core.Extensions;
using ManageSystem.Admin.Models.Chart;
using ManageSystem.Core.Utility.Excel;
using Webdiyer.WebControls.Mvc;
using ManageSystem.Core.DynamicLinq;
using Newtonsoft.Json;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Services.Satellites;
using NPOI.HPSF;

namespace ManageSystem.Admin.Controllers
{
    /// <summary>
    /// 图表管理
    /// </summary>
    public class MgtChartController : AdminBaseController
    {
        #region 业务声明
        private readonly IMemberService _memberService;
        private readonly IDataSegmentService _dataSegmentService;
        private readonly IHeatmapService _heatmapService;
        private readonly IHeatmapItemService _heatmapItemService;
        private readonly IBarChartService _barChartService;
        private readonly IBarChartWithItemDataService _barChartWithItemDataService;
        private readonly ITrendChartService _trendChartService;
        private readonly ITrendChartWithItemDataService _trendChartWithItemDataService;
        private readonly IMICPermissionapplication MICPermissionapplication;
        private readonly IUserRoleService UserRoleService;
        private readonly IRoleService RoleService;
        private readonly ISatelliteService SatelliteService;

        private static readonly Dictionary<string, string> _provinceMap = new Dictionary<string, string>
        {
            ["北京"] = "北京市",
            ["天津"] = "天津市",
            ["河北"] = "河北省",
            ["山西"] = "山西省",
            ["内蒙古"] = "内蒙古自治区",
            ["辽宁"] = "辽宁省",
            ["吉林"] = "吉林省",
            ["黑龙江"] = "黑龙江省",
            ["上海"] = "上海市",
            ["江苏"] = "江苏省",
            ["浙江"] = "浙江省",
            ["安徽"] = "安徽省",
            ["福建"] = "福建省",
            ["江西"] = "江西省",
            ["山东"] = "山东省",
            ["河南"] = "河南省",
            ["湖北"] = "湖北省",
            ["湖南"] = "湖南省",
            ["广东"] = "广东省",
            ["广西"] = "广西壮族自治区",
            ["海南"] = "海南省",
            ["重庆"] = "重庆市",
            ["四川"] = "四川省",
            ["贵州"] = "贵州省",
            ["云南"] = "云南省",
            ["西藏"] = "西藏自治区",
            ["陕西"] = "陕西省",
            ["甘肃"] = "甘肃省",
            ["青海"] = "青海省",
            ["宁夏"] = "宁夏回族自治区",
            ["新疆"] = "新疆维吾尔自治区",
            ["台湾"] = "台湾省",
            ["香港"] = "香港特别行政区",
            ["澳门"] = "澳门特别行政区"
        };
        #endregion

        #region 构造器
        public MgtChartController(IMemberService memberService, IDataSegmentService dataSegmentService, IHeatmapService heatmapService, IHeatmapItemService heatmapItemService, IBarChartService barChartService, IBarChartWithItemDataService barChartWithItemDataService, ITrendChartService trendChartService, ITrendChartWithItemDataService trendChartWithItemDataService, IMICPermissionapplication _MICPermissionapplication, IUserRoleService _userRoleService, IRoleService _RoleService, ISatelliteService _SatelliteService)
        {
            this._memberService = memberService;
            this._dataSegmentService = dataSegmentService;
            this._heatmapService = heatmapService;
            this._heatmapItemService = heatmapItemService;
            this._barChartService = barChartService;
            this._barChartWithItemDataService = barChartWithItemDataService;
            this._trendChartService = trendChartService;
            this._trendChartWithItemDataService = trendChartWithItemDataService;
            this.MICPermissionapplication = _MICPermissionapplication;
            UserRoleService = _userRoleService;
            RoleService = _RoleService;
            SatelliteService = _SatelliteService;
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
        public JsonResult DataSegment_OnCheck(DataSegmentModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                Expression<Func<Chart_DataSegment, bool>> predicate = r => r.Name == model.Name && r.Mark > 0 && r.ProjectType == model.ProjectType;
                if (model.Id > 0)
                {
                    predicate = predicate.And(r => r.Id != model.Id);
                }
                return Json(_dataSegmentService.Count(predicate) == 0);
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
        public ActionResult DataSegment_OnCreateSubmit(DataSegmentModel model)
        {
            if (ModelState.IsValid)
            {
                Chart_DataSegment entity = new Chart_DataSegment
                {
                    Id = CommonHelper.GuidToLongID,
                    Name = model.Name,
                    Sort = model.Sort,
                    ProjectType = model.ProjectType,
                    InsertTime = DateTime.Now,
                    Describe = null,
                    Mark = 1,
                    Version = 1
                };
                entity.UpdateTime = entity.InsertTime;

                _dataSegmentService.Insert(entity);
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
                case DataSegmentEnum.Heatmap:
                    return RedirectToAction("HeatmapList");
                case DataSegmentEnum.BarChart:
                    return RedirectToAction("BarChartList");
                case DataSegmentEnum.TrendChart:
                    return RedirectToAction("TrendChartList");
                default:
                    return Redirect("/");
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
        public ActionResult DataSegment_OnEditSubmit(DataSegmentModel model)
        {

            if (ModelState.IsValid)
            {
                Chart_DataSegment entity = _dataSegmentService.QueryEntity(model.Id);
                if (entity != null && entity.Id > 0 && entity.Mark > 0 && entity.ProjectType == model.ProjectType)
                {
                    // 判断重名
                    if (_dataSegmentService.Count(r => r.ProjectType == model.ProjectType && r.Mark > 0 && r.Id != entity.Id && r.Name == model.Name) == 0)
                    {
                        entity.Name = model.Name;
                        entity.Sort = model.Sort;
                        _dataSegmentService.Update(entity);
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
                case DataSegmentEnum.Heatmap:
                    return RedirectToAction("HeatmapList");
                case DataSegmentEnum.BarChart:
                    return RedirectToAction("BarChartList");
                case DataSegmentEnum.TrendChart:
                    return RedirectToAction("TrendChartList");
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
        public ActionResult DataSegment_OnDelete(long id, DataSegmentEnum type)
        {
            switch (type)
            {
                case DataSegmentEnum.Heatmap:
                    {
                        if (_dataSegmentService.OnDeleteByHeatmapWithTransaction(id))
                        {
                            string logContent = $"【手动】已成功删除数据段";
                            base.SuccessNotification(logContent);
                            InsetActionLog(ActionType.Delete, logContent);
                        }
                        else
                        {
                            ErrorNotification("删除失败");
                        }
                        return RedirectToAction("HeatmapList");
                    }
                case DataSegmentEnum.BarChart:
                    {
                        if (_dataSegmentService.OnDeleteByBarChartWithTransaction(id))
                        {
                            string logContent = $"【手动】已成功删除数据段";
                            base.SuccessNotification(logContent);
                            InsetActionLog(ActionType.Delete, logContent);
                        }
                        else
                        {
                            ErrorNotification("删除失败");
                        }
                        return RedirectToAction("BarChartList");
                    }
                case DataSegmentEnum.TrendChart:
                    {
                        if (_dataSegmentService.OnDeleteByTrendChartWithTransaction(id))
                        {
                            string logContent = $"【手动】已成功删除数据段";
                            base.SuccessNotification(logContent);
                            InsetActionLog(ActionType.Delete, logContent);
                        }
                        else
                        {
                            ErrorNotification("删除失败");
                        }
                        return RedirectToAction("TrendChartList");
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
        public ActionResult DataSegment_SettingDisplay(DataSegmentModel model)
        {
            try
            {
                Chart_DataSegment entity = _dataSegmentService.QueryEntity(model.Id);
                if (entity != null && entity.Id > 0 && entity.Mark > 0 && entity.ProjectType == model.ProjectType)
                {
                    entity.Display = model.Display;
                    _dataSegmentService.Update(entity);

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
                case DataSegmentEnum.Heatmap:
                    return RedirectToAction("HeatmapList");
                case DataSegmentEnum.BarChart:
                    return RedirectToAction("BarChartList");
                case DataSegmentEnum.TrendChart:
                    return RedirectToAction("TrendChartList");
                default:
                    return Redirect("/");
            }
        }
        #endregion

        #endregion

        #region 热图管理

        #region 列表
        /// <summary>
        /// 热图列表
        /// </summary>
        /// <param name="page">分页索引</param>
        /// <returns></returns>
        public ActionResult HeatmapList(int page = 1)
        {
            Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(base.LoginUserinfo.Id.ToString())));
            // 默认第一页
            page = page > 1 ? page : 1;
            // 条件查询
            Expression<Func<Chart_DataSegment, bool>> predicate = r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.Heatmap;
            IEnumerable<HeatmapListDTO> dataSource;
            if (string.IsNullOrEmpty(role.SatelliteId))
            {
                dataSource = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new HeatmapListDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProjectType = item.ProjectType,
                    Sort = item.Sort,
                    Display = item.Display,
                    DataItems = _heatmapService.Query(r => r.DataSegmentId == item.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).Select(r => new HeatmapItemDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Sort = r.Sort,
                        Display = r.Display,
                        Default = r.Default
                    })
                });
            }
            else
            {
                string sate = base.LoginUserinfo.Describe;
                dataSource = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new HeatmapListDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProjectType = item.ProjectType,
                    Sort = item.Sort,
                    Display = item.Display,
                    DataItems = _heatmapService.Query(r => r.DataSegmentId == item.Id && r.Mark > 0 && r.SatelliteId == sate).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).Select(r => new HeatmapItemDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Sort = r.Sort,
                        Display = r.Display,
                        Default = r.Default
                    })
                });

            }

            // 分页，默认每页显示5个数据段内容
            IPagedList<HeatmapListDTO> model = new PagedList<HeatmapListDTO>(dataSource, page, 5);
            return View(model);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增热图
        /// </summary>
        /// <param name="id">数据段id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HeatmapCreate(long id)
        {
            return View(InitHeatmapCreate(id));
        }

        /// <summary>
        /// 新增热图页面初始化
        /// </summary>
        /// <param name="dsid">数据段id</param>
        /// <returns></returns>
        private HeatmapModel InitHeatmapCreate(long dsid)
        {
            HeatmapModel model = new HeatmapModel
            {
                Id = CommonHelper.GuidToLongID,
                ChartType = 1,
                Mark = 1,
                Describe = null,
                Name = null,
                Display = true,
                DeleteTime = DateTime.MinValue,
                InsertTime = DateTime.Now,
                DataSegmentId = dsid,
                DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.Heatmap).OrderBy(r => r.Sort).Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == dsid
                }).ToList(),
                Sort = _heatmapService.NextSort()
            };

            if (!model.DropDataSegment.Any() || model.DropDataSegment.Count == 0)
            {
                model.DropDataSegment = new List<SelectListItem> { new SelectListItem { Text = "请先添加数据段", Value = "0" } };
            }

            return model;
        }

        /// <summary>
        /// 提交新增热图报表
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult HeatmapCreate(HeatmapModel model)
        {
            // 页面初始化model
            var pageModel = InitHeatmapCreate(model.DataSegmentId);

            if (model.ItemModel == null || model.ItemModel.Count == 0)
            {
                base.ErrorNotification("请录入报表数据项");
                pageModel.Name = model.Name;
                pageModel.Sort = model.Sort;
                pageModel.Display = model.Display;
                pageModel.ChartType = model.ChartType;
                return View(pageModel);
            }

            Dictionary<string, string> dicHeatmapItem = new Dictionary<string, string>();
            foreach (var item in model.ItemModel)
            {
                // 简化省份名称，如:上海市 简化为 上海，浙江省 简化为 浙江
                dicHeatmapItem[_provinceMap[item.Name]] = item.Value;
            }

            var dataSegmentEntity = _dataSegmentService.QueryEntity(model.DataSegmentId);
            if (dataSegmentEntity == null || dataSegmentEntity.Id < 0 || dataSegmentEntity.Mark <= 0 || dataSegmentEntity.ProjectType != DataSegmentEnum.Heatmap)
            {
                ModelState.AddModelError("DataSegmentId", "请选择数据段或所选数据段不正确");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "报表名称不能为空");
            }

            model.Name = model.Name.Trim();

            if (_heatmapService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Name == model.Name && r.Mark > 0) > 0)
            {
                ModelState.AddModelError("Name", "该数据段下报表名称已经存在");
            }

            if (ModelState.IsValid)
            {
                var user = base.LoginUserinfo;
                Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(user.Id.ToString())));
                DateTime now = DateTime.Now;
                Chart_Heatmap mainEntity = new Chart_Heatmap
                {
                    Id = CommonHelper.GuidToLongID,
                    DataSegmentId = model.DataSegmentId,
                    Name = model.Name,
                    Sort = model.Sort,
                    Display = model.Display,
                    ChartType = model.ChartType,
                    Mark = 1,
                    Version = 1,
                    Describe = null,
                    InsertTime = now,
                    UpdateTime = now,
                    DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                    SatelliteId = user.Describe
                };

                List<Chart_HeatmapItem> itemEntities = new List<Chart_HeatmapItem>();
                foreach (KeyValuePair<string, string> item in dicHeatmapItem)
                {
                    itemEntities.Add(new Chart_HeatmapItem { Name = item.Key, Value = item.Value });
                }
                if (_heatmapService.UpdateSort(model.Sort) >= 0 && _heatmapService.OnCreateHeatmapByTransaction(mainEntity, itemEntities))
                {
                    string logContent = $"数据项添加成功，报表名称：【{model.Name}】";
                    base.SuccessNotification(logContent);
                    base.InsetActionLog(ActionType.Create, logContent);
                    pageModel.Name = "";
                    pageModel.Sort = _heatmapService.NextSort();
                    return View(pageModel);
                }
            }

            base.ErrorNotification("请完善相关数据项");
            pageModel.Name = model.Name;
            pageModel.Sort = model.Sort;
            pageModel.Display = model.Display;
            pageModel.ChartType = model.ChartType;
            return View(pageModel);
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑热图
        /// </summary>
        /// <param name="id">热图报表id</param>
        /// <returns></returns>
        public ActionResult HeatmapEdit(long id)
        {
            return View(InitHeatmapEdit(id));
        }

        /// <summary>
        /// 提交热图报表
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult HeatmapEdit(HeatmapModel model)
        {
            var pageModel = InitHeatmapEdit(model.Id);

            if (pageModel.Id <= 0)
            {
                ModelState.AddModelError("Id", "未能获取到数据，请刷新后重试");
            }

            if (model.ItemModel == null || model.ItemModel.Count == 0)
            {
                base.ErrorNotification("请录入报表数据项");
                return View(pageModel);
            }

            Dictionary<string, string> dicHeatmapItem = new Dictionary<string, string>();
            foreach (var item in model.ItemModel)
            {
                // 简化省份名称，如:上海市 简化为 上海，浙江省 简化为 浙江
                dicHeatmapItem[_provinceMap[item.Name]] = item.Value;
            }

            var dataSegmentEntity = _dataSegmentService.QueryEntity(model.DataSegmentId);
            if (dataSegmentEntity == null || dataSegmentEntity.Id < 0 || dataSegmentEntity.Mark <= 0 || dataSegmentEntity.ProjectType != DataSegmentEnum.Heatmap)
            {
                ModelState.AddModelError("DataSegmentId", "请选择数据段或所选数据段不正确");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "报表名称不能为空");
            }

            model.Name = model.Name.Trim();

            // 判断重名
            if (_heatmapService.Count(r => r.Id != model.Id && r.DataSegmentId == model.DataSegmentId && r.Name == model.Name && r.Mark > 0) > 0)
            {
                ModelState.AddModelError("Name", "该数据段下报表名称已经存在");
            }

            if (ModelState.IsValid)
            {
                DateTime now = DateTime.Now;
                Chart_Heatmap mainEntity = _heatmapService.QueryEntity(model.Id);
                mainEntity.Name = model.Name;
                mainEntity.Sort = model.Sort;
                mainEntity.Display = model.Display;
                mainEntity.ChartType = model.ChartType;
                mainEntity.Mark = 2;
                mainEntity.UpdateTime = now;

                // 关联报表数据项
                List<Chart_HeatmapItem> itemEntities = new List<Chart_HeatmapItem>();
                foreach (KeyValuePair<string, string> item in dicHeatmapItem)
                {
                    itemEntities.Add(new Chart_HeatmapItem { Name = item.Key, Value = item.Value });
                }

                if (_heatmapService.UpdateSort(model.Sort) > 0 && _heatmapService.OnEditHeatmapByTransaction(mainEntity, itemEntities))
                {
                    string logContent = $"数据项编辑成功，报表名称：【{model.Name}】";
                    base.SuccessNotification(logContent);
                    base.InsetActionLog(ActionType.Edit, logContent);
                    return RedirectToAction("HeatmapList", new { id = model.Id });
                }
            }

            base.ErrorNotification("请完善相关数据项");
            pageModel.ItemModel = dicHeatmapItem.Select(item => new HeatmapItemModel { Name = item.Key, Value = item.Value }).ToList();
            return View(pageModel);
        }

        /// <summary>
        /// 编辑页面model初始化
        /// </summary>
        /// <param name="id">热图报表id</param>
        /// <returns></returns>
        private HeatmapModel InitHeatmapEdit(long id)
        {
            Chart_Heatmap entity = _heatmapService.QueryEntity(id);
            if (entity == null || entity.Id == 0 || entity.Mark <= 0)
            {
                return new HeatmapModel();
            }

            HeatmapModel model = new HeatmapModel
            {
                Id = entity.Id,
                ChartType = entity.ChartType,
                Mark = entity.Mark,
                Describe = entity.Describe,
                Name = entity.Name,
                Display = entity.Display,
                DeleteTime = entity.DeleteTime,
                InsertTime = entity.InsertTime,
                DataSegmentId = entity.DataSegmentId,
                DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.Heatmap).OrderBy(r => r.Sort).Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == entity.DataSegmentId
                }).ToList(),
                Sort = entity.Sort,
                ItemModel = _heatmapItemService.Query(r => r.HeatmapId == entity.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.Name).Select(item => new HeatmapItemModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Value = item.Value,
                    Sort = item.Sort
                }).ToList()
            };

            if (!model.DropDataSegment.Any() || model.DropDataSegment.Count == 0)
            {
                model.DropDataSegment = new List<SelectListItem> { new SelectListItem { Text = "请先添加数据段", Value = "0" } };
            }

            return model;
        }
        #endregion

        #region 查看
        /// <summary>
        /// 热图报表详情
        /// </summary>
        /// <param name="id">热图报表id</param>
        /// <returns></returns>
        public ActionResult HeatmapDetail(long id)
        {
            var model = InitHeatmapEdit(id);
            model.IsEditPage = true;
            return View(model);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除热图报表
        /// </summary>
        /// <param name="id">要删除的热图报表id</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult Heatmap_OnDelete(long id)
        {
            var entity = _heatmapService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                if (_heatmapService.OnDeleteHeatmapByTransaction(id))
                {
                    string logContent = $"成功删除报表";
                    base.SuccessNotification(logContent);
                    base.InsetActionLog(ActionType.Edit, logContent);
                }
            }
            else
            {
                ErrorNotification("删除报表失败");
            }
            return RedirectToAction("HeatmapList");
        }
        #endregion

        #region 读取热图导入模块
        /// <summary>
        /// 读取Excel数据
        /// </summary>
        /// <param name="file">热图报表数据</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public JsonResult HeatmapExcel_OnSubmit(HttpPostedFileWrapper file)
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
                return Json(new { status = false, message = "请按省份填写正确的合理的数值" });
            }
            List<dynamic> data = new List<dynamic>();
            foreach (DataRow dataRow in table.Rows)
            {
                var value = (dataRow["数值"] ?? "").ToString().Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (double.TryParse(value, out double _value))
                    {
                        data.Add(new { name = (dataRow["省份"] ?? "").ToString().Trim(), value = _value.ToString("0.00") });
                    }
                }
            }

            if (data.Count == 0)
            {
                return Json(new { status = false, message = "请按省份填写正确的合理的数值" });
            }

            return Json(new { status = true, message = "数据导入完成", data = data });
        }
        #endregion

        #region 设置状态显示/不显示
        /// <summary>
        /// 设置热图报表显示与否
        /// </summary>
        /// <param name="id">热图报表id</param>
        /// <param name="status">是否显示</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult Heatmap_OnSetDisplay(long id, bool status)
        {
            var entity = _heatmapService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Display = status;
                entity.UpdateTime = DateTime.Now;
                _heatmapService.Update(entity);
                string logContent = $"成功设置报表【{entity.Name}】的状态为：【{(status ? "显示" : "不显示")}】";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("状态变更失败");
            }
            return RedirectToAction("HeatmapList");
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
        public ActionResult Heatmap_OnSettingDefault(long id, bool @default)
        {
            var entity = _heatmapService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Default = @default;
                entity.UpdateTime = DateTime.Now;
                if (@default)
                {
                    _heatmapService.Update(r => r.Default && r.Mark > 0, update => new Chart_Heatmap { Default = false });
                }
                _heatmapService.Update(entity);

                string logContent = $"{(@default ? "设置" : "取消")}报表【{entity.Name}】默认选中";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("操作失败");
            }
            return RedirectToAction("HeatmapList");
        }
        #endregion

        #endregion

        #region 柱状图管理

        #region 列表
        /// <summary>
        /// 柱状图列表
        /// </summary>
        /// <param name="page">分页索引</param>
        /// <returns></returns>0
        public ActionResult BarChartList(int page = 1)
        {
            var user = base.LoginUserinfo;
            Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(user.Id.ToString())));

            page = page > 1 ? page : 1;
            Expression<Func<Chart_DataSegment, bool>> predicate = r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart;

            IEnumerable<BarChartListDTO> dataSource;
            if (string.IsNullOrEmpty(role.SatelliteId))
            {
                dataSource = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartListDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProjectType = item.ProjectType,
                    Sort = item.Sort,
                    Display = item.Display,
                    DataItems = _barChartService.Query(r => r.DataSegmentId == item.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ThenBy(r => r.Id).Select(r => new BarChartItemDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Title = r.Title,
                        SubTitle = r.SubTitle,
                        Sort = r.Sort,
                        Display = r.Display,
                        Default = r.Default,
                        SatelliteId = r.SatelliteId
                    })
                });
            }
            else
            {
                long sateId = long.Parse(user.Describe);
                dataSource = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartListDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProjectType = item.ProjectType,
                    Sort = item.Sort,
                    Display = item.Display,
                    DataItems = _barChartService.Query(r => r.DataSegmentId == item.Id && r.Mark > 0 && r.SatelliteId == sateId).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ThenBy(r => r.Id).Select(r => new BarChartItemDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Title = r.Title,
                        SubTitle = r.SubTitle,
                        Sort = r.Sort,
                        Display = r.Display,
                        Default = r.Default,
                        SatelliteId = r.SatelliteId
                    })
                });
            }
            IPagedList<BarChartListDTO> model = new PagedList<BarChartListDTO>(dataSource, page, 5);
            return View(model);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增柱状图
        /// </summary>
        /// <param name="id">数据段id</param>
        /// <returns></returns>
        public ActionResult BarChartCreate(long id)
        {
            BarChartModel model = InitBarChartCreate(id);
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
        public ActionResult BarChartCreate(BarChartModel model, string BarColor, ChartTypeEnum ChartType = ChartTypeEnum.Bar)
        {
            try
            {
                model.Name = (model.Name ?? "").Trim();

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    ModelState.AddModelError("Name", "报表名称不能为空");
                }

                // 判断报表名称是否存在
                if (!string.IsNullOrWhiteSpace(model.Name) && _barChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Name == model.Name && r.Mark > 0) > 0)
                {
                    ModelState.AddModelError("Name", "此报表名称已经存在");
                }

                if (model.DataItemType == ChartDataItemType.MultipleData)
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
                    var user = base.LoginUserinfo;
                    Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(user.Id.ToString())));

                    Chart_BarChart mainEntity = new Chart_BarChart
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
                        Antibiotics = model.DataItemType == ChartDataItemType.MultipleData ? model.Antibiotics.SerializeObject() : null,
                        Mark = 1,
                        Version = 1,
                        Describe = null,
                        InsertTime = DateTime.Now,
                        DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                        SatelliteId = string.IsNullOrEmpty(user.Describe) ? 0 : long.Parse(user.Describe)
                    };
                    mainEntity.UpdateTime = mainEntity.InsertTime;

                    // 关联数据项集合
                    List<Chart_BarChartWithItemData> itemEntities = new List<Chart_BarChartWithItemData>();

                    switch (model.DataItemType)
                    {
                        case ChartDataItemType.SingleData:
                            {
                                foreach (BarChartDataItemModel item in model.ItemModel)
                                {
                                    if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                    {
                                        itemEntities.Add(new Chart_BarChartWithItemData
                                        {
                                            Id = CommonHelper.GuidToLongID,
                                            BarChartId = mainEntity.Id,
                                            ChartType = ChartType,
                                            BarColor = BarColor,
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
                        case ChartDataItemType.MultipleData:
                            {
                                foreach (BarChartDataItemModel item in model.ItemModel)
                                {
                                    if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                    {
                                        item.Antibiotics = item.Antibiotics.Where(r => model.Antibiotics.Select(x => x.Guid).Contains(r.Guid)).ToList();
                                        item.Antibiotics.ForEach(x => { x.Name = model.Antibiotics.Where(r => r.Guid == x.Guid).FirstOrDefault().Name; });

                                        item.Value = item.Antibiotics.SerializeObject();

                                        itemEntities.Add(new Chart_BarChartWithItemData
                                        {
                                            Id = CommonHelper.GuidToLongID,
                                            BarChartId = mainEntity.Id,
                                            ChartType = item.ChartType,
                                            BarColor = item.BarColor,
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
                        //Gerry：2023-10-27
                        //if (_barChartService.UpdateSort(model.Sort) > 0 && _barChartService.OnCreateBarChartByTransaction(mainEntity, itemEntities))
                        if (_barChartService.UpdateSort(model.Sort) >= 0 && _barChartService.OnCreateBarChartByTransaction(mainEntity, itemEntities))
                        {
                            string logContent = $"柱状图报表添加成功，报表名称：【{model.Name}】";
                            base.SuccessNotification(logContent);
                            base.InsetActionLog(ActionType.Create, logContent);
                            return RedirectToAction("BarChartCreate", new { id = mainEntity.DataSegmentId });
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


            model.DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == model.DataSegmentId
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// 初始化新增柱状图
        /// </summary>
        /// <param name="dsid">数据段id</param>
        /// <returns></returns>
        private BarChartModel InitBarChartCreate(long dsid)
        {
            List<SelectListItem> selectListItems = this.SatelliteService.Query(x => !string.IsNullOrEmpty(x.Describe) && x.RealmName != "&").Select(item => new SelectListItem {Value = item.Id.ToString(),Text = item.SatelliteName }).ToList();

            BarChartModel model = new BarChartModel
            {
                Id = CommonHelper.GuidToLongID,
                DataSegmentId = dsid,
                DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == dsid
                }).ToList(),
                Name = null,
                Title = null,
                SubTitle = null,
                Sort = _barChartService.NextSort(dsid),
                Display = true,
                Unit = "%",
                MobileDisplayScale = 100,
                DataItemType = ChartDataItemType.SingleData,
                SatelliteList = selectListItems
            };
            return model;
        }

        #endregion

        #region 编辑
        /// <summary>
        /// 编辑柱状图
        /// </summary>
        /// <param name="id">柱状图报表id</param>
        /// <returns></returns>
        public ActionResult BarChartEdit(long id)
        {
            var mainEntity = _barChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("BarChartList");
            }
            BarChartModel model = InitBarChartEdit(mainEntity);
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
        public ActionResult BarChartEdit(BarChartModel model, string BarColor, ChartTypeEnum ChartType = ChartTypeEnum.Bar)
        {
            model.Name = (model.Name ?? "").Trim();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "报表名称不能为空");
            }

            // 判断报表名称是否存在
            if (!string.IsNullOrWhiteSpace(model.Name) && _barChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Id != model.Id && r.Name == model.Name && r.Mark > 0) > 0)
            {
                ModelState.AddModelError("Name", "此报表名称已经存在");
            }

            Chart_BarChart mainEntity = _barChartService.QueryEntity(model.Id);
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

                List<Chart_BarChartWithItemData> itemEntities = new List<Chart_BarChartWithItemData>();

                switch (model.DataItemType)
                {
                    case ChartDataItemType.SingleData:
                        {
                            foreach (BarChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    itemEntities.Add(new Chart_BarChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        BarChartId = mainEntity.Id,
                                        ChartType = ChartType,
                                        BarColor = BarColor,
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
                    case ChartDataItemType.MultipleData:
                        {
                            mainEntity.Antibiotics = model.Antibiotics.SerializeObject();
                            foreach (BarChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    item.Antibiotics = item.Antibiotics.Where(r => model.Antibiotics.Select(x => x.Guid).Contains(r.Guid)).ToList();
                                    item.Antibiotics.ForEach(x => { x.Name = model.Antibiotics.Where(r => r.Guid == x.Guid).FirstOrDefault().Name; });
                                    item.Value = item.Antibiotics.SerializeObject();

                                    itemEntities.Add(new Chart_BarChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        BarChartId = mainEntity.Id,
                                        ChartType = item.ChartType,
                                        BarColor = item.BarColor,
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
                    if (_barChartService.UpdateSort(model.Sort) > 0 && _barChartService.OnEditBarChartByTransaction(mainEntity, itemEntities))
                    {
                        string logContent = $"柱状图报表编辑成功，报表名称：【{model.Name}】";
                        base.SuccessNotification(logContent);
                        base.InsetActionLog(ActionType.Create, logContent);
                        return RedirectToAction("BarChartList", new { id = mainEntity.Id });
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

            model.DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
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
        private BarChartModel InitBarChartEdit(Chart_BarChart mainEntity)
        {
            BarChartModel model = new BarChartModel
            {
                Id = mainEntity.Id,
                DataSegmentId = mainEntity.DataSegmentId,
                DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
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
                Antibiotics = mainEntity.DataItemType == ChartDataItemType.MultipleData ? mainEntity.Antibiotics.DeserializeObject<List<BarChartDataItemWithAntibioticModel>>() : new List<BarChartDataItemWithAntibioticModel>(),
                IsEditPage = true
            };
            model.ItemModel = _barChartWithItemDataService.Query(r => r.BarChartId == mainEntity.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenByDescending(r => r.UpdateTime).Select(item => new BarChartDataItemModel
            {
                Id = item.Id,
                ChartType = item.ChartType,
                BarColor = item.BarColor,
                Name = item.Name,
                Value = item.Value,
                Display = item.Display,
                Sort = item.Sort
            }).ToList();

            return model;
        }
        #endregion

        #region 查看
        /// <summary>
        /// 柱状图报表详情
        /// </summary>
        /// <param name="id">柱状图报表id</param>
        /// <returns></returns>
        public ActionResult BarChartDetail(long id)
        {
            var mainEntity = _barChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("BarChartList");
            }

            var model = InitBarChartEdit(mainEntity);
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
        public ActionResult BarChart_OnDelete(long id)
        {
            var entity = _barChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                if (_barChartService.OnDeleteBarChartByTransaction(id))
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
            return RedirectToAction("BarChartList");
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
        public ActionResult BarChart_OnSetDisplay(long id, bool status)
        {
            var entity = _barChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Display = status;
                entity.UpdateTime = DateTime.Now;
                _barChartService.Update(entity);
                string logContent = $"成功设置报表【{entity.Name}】的状态为：【{(status ? "显示" : "不显示")}】";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("状态变更失败");
            }
            return RedirectToAction("BarChartList");
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
        public JsonResult BarChartSingleExcel_OnSubmit(HttpPostedFileWrapper file)
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
        public JsonResult BarChartMultipleExcel_OnSubmit(HttpPostedFileWrapper file)
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

            List<BarChartDataItemWithAntibioticModel> antibiotics = new List<BarChartDataItemWithAntibioticModel>();

            List<string> fixedColumns = new List<string> { "数据项名称", "是否显示", "图表类型" };

            foreach (DataColumn item in table.Columns)
            {
                if (string.IsNullOrWhiteSpace(item.ColumnName ?? "") || RegexHelper.IsMatch(item.ColumnName.Trim(), @"[Cc]olumn\d+") || fixedColumns.Contains(item.ColumnName.Trim()))
                {
                    continue;
                }

                antibiotics.Add(new BarChartDataItemWithAntibioticModel
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

                List<BarChartDataItemWithAntibioticModel> _values = new List<BarChartDataItemWithAntibioticModel>();
                foreach (BarChartDataItemWithAntibioticModel antibioticItem in antibiotics)
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

                    _values.Add(new BarChartDataItemWithAntibioticModel
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
        public ActionResult BarChart_OnSettingDefault(long id, bool @default)
        {
            var entity = _barChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Default = @default;
                entity.UpdateTime = DateTime.Now;
                if (@default)
                {
                    _barChartService.Update(r => r.Default && r.Mark > 0, update => new Chart_BarChart { Default = false });
                }
                _barChartService.Update(entity);

                string logContent = $"{(@default ? "设置" : "取消")}报表【{entity.Name}】默认选中";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("操作失败");
            }
            return RedirectToAction("BarChartList");
        }
        #endregion

        #endregion

        #region 趋势图管理

        #region 列表
        /// <summary>
        /// 趋势图列表
        /// </summary>
        /// <param name="page">分页索引</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TrendChartList(int page = 1)
        {
            Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(base.LoginUserinfo.Id.ToString())));
            page = page > 1 ? page : 1;
            Expression<Func<Chart_DataSegment, bool>> predicate = r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.TrendChart;
            IEnumerable<TrendChartListDTO> dataSource;
            if (string.IsNullOrEmpty(role.SatelliteId))
            {
                dataSource = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new TrendChartListDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProjectType = item.ProjectType,
                    Sort = item.Sort,
                    Display = item.Display,
                    DataItems = _trendChartService.Query(r => r.DataSegmentId == item.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).Select(r => new TrendChartItemDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Title = r.Title,
                        SubTitle = r.SubTitle,
                        Sort = r.Sort,
                        Display = r.Display,
                        Default = r.Default
                    })
                });
            }
            else
            {
                var user = base.LoginUserinfo;
                dataSource = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new TrendChartListDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    ProjectType = item.ProjectType,
                    Sort = item.Sort,
                    Display = item.Display,
                    DataItems = _trendChartService.Query(r => r.DataSegmentId == item.Id && r.Mark > 0 && r.SatelliteId == user.Describe).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).Select(r => new TrendChartItemDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Title = r.Title,
                        SubTitle = r.SubTitle,
                        Sort = r.Sort,
                        Display = r.Display,
                        Default = r.Default
                    })
                });
            }
            IPagedList<TrendChartListDTO> model = new PagedList<TrendChartListDTO>(dataSource, page, 5);
            return View(model);
        }

        #endregion

        #region 新增
        /// <summary>
        /// 新增趋势图
        /// </summary>
        /// <param name="id">数据段id</param>
        /// <returns></returns>
        public ActionResult TrendChartCreate(long id)
        {
            TrendChartModel model = InitTrendChartCreate(id);
            return View(model);
        }

        /// <summary>
        /// 提交新增趋势图
        /// </summary>
        /// <param name="model">请求的数据</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult TrendChartCreate(TrendChartModel model)
        {
            model.Name = (model.Name ?? "").Trim();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "报表名称不能为空");
            }

            // 判断报表名称是否存在
            if (!string.IsNullOrWhiteSpace(model.Name) && _trendChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Name == model.Name && r.Mark > 0) > 0)
            {
                ModelState.AddModelError("Name", "此报表名称已经存在");
            }

            if (ModelState.IsValid)
            {
                Role role = RoleService.QueryEntity(long.Parse(UserRoleService.GetRoleId(base.LoginUserinfo.Id.ToString())));
                string sate = base.LoginUserinfo.Describe;
                Chart_TrendChart mainEntity = new Chart_TrendChart
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
                    Antibiotics = model.DataItemType == ChartDataItemType.MultipleData ? model.Antibiotics.SerializeObject() : null,
                    Mark = 1,
                    Version = 1,
                    Describe = null,
                    InsertTime = DateTime.Now,
                    DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0),
                    SatelliteId = sate
                };
                mainEntity.UpdateTime = mainEntity.InsertTime;
                List<Chart_TrendChartWithItemData> itemEntities = new List<Chart_TrendChartWithItemData>();

                switch (model.DataItemType)
                {
                    case ChartDataItemType.SingleData:
                        {
                            foreach (TrendChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    itemEntities.Add(new Chart_TrendChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        TrendChartId = mainEntity.Id,
                                        ChartType = model.ChartType,
                                        ChartColor = model.ChartColor,
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
                    case ChartDataItemType.MultipleData:
                        {
                            foreach (TrendChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    item.Antibiotics = item.Antibiotics.Where(r => model.Antibiotics.Select(x => x.Guid).Contains(r.Guid)).ToList();
                                    item.Value = item.Antibiotics.SerializeObject();

                                    itemEntities.Add(new Chart_TrendChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        TrendChartId = mainEntity.Id,
                                        ChartType = item.ChartType,
                                        ChartColor = item.ChartColor,
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
                    //2023.10.31-Peng Edit
                    // if (_trendChartService.UpdateSort(model.Sort) > 0 && _trendChartService.OnCreateByTransaction(mainEntity, itemEntities))
                    if (_trendChartService.UpdateSort(model.Sort) >= 0 && _trendChartService.OnCreateByTransaction(mainEntity, itemEntities))
                    {
                        string logContent = $"趋势图报表添加成功，报表名称：【{model.Name}】";
                        base.SuccessNotification(logContent);
                        base.InsetActionLog(ActionType.Create, logContent);
                        return RedirectToAction("TrendChartCreate", new { id = mainEntity.DataSegmentId });
                    }
                    else
                    {
                        base.ErrorNotification("保存趋势图报表失败");
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

            model.DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == model.DataSegmentId
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// 新增初始化
        /// </summary>
        /// <param name="dsid">数据段id</param>
        /// <returns></returns>
        private TrendChartModel InitTrendChartCreate(long dsid)
        {
            TrendChartModel model = new TrendChartModel
            {
                Id = CommonHelper.GuidToLongID,
                DataSegmentId = dsid,
                DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.TrendChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == dsid
                }).ToList(),
                Name = null,
                Title = null,
                SubTitle = null,
                Sort = _trendChartService.NextSort(dsid),
                Display = true,
                Unit = "%",
                MobileDisplayScale = 100,
                DataItemType = ChartDataItemType.SingleData
            };
            return model;
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑趋势图报表
        /// </summary>
        /// <param name="id">趋势图报表id</param>
        /// <returns></returns>
        public ActionResult TrendChartEdit(long id)
        {
            var mainEntity = _trendChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("TrendChartList");
            }
            TrendChartModel model = InitTrendChartEdit(mainEntity);
            return View(model);
        }

        /// <summary>
        /// 提交编辑趋势图报表
        /// </summary>
        /// <param name="model">请求的数据</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult TrendChartEdit(TrendChartModel model)
        {
            model.Name = (model.Name ?? "").Trim();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "报表名称不能为空");
            }

            // 判断报表名称是否存在
            if (!string.IsNullOrWhiteSpace(model.Name) && _trendChartService.Count(r => r.DataSegmentId == model.DataSegmentId && r.Id != model.Id && r.Name == model.Name && r.Mark > 0) > 0)
            {
                ModelState.AddModelError("Name", "此报表名称已经存在");
            }

            Chart_TrendChart mainEntity = _trendChartService.QueryEntity(model.Id);
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

                List<Chart_TrendChartWithItemData> itemEntities = new List<Chart_TrendChartWithItemData>();

                switch (model.DataItemType)
                {
                    case ChartDataItemType.SingleData:
                        {
                            foreach (TrendChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    itemEntities.Add(new Chart_TrendChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        TrendChartId = mainEntity.Id,
                                        ChartType = model.ChartType,
                                        ChartColor = model.ChartColor,
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
                    case ChartDataItemType.MultipleData:
                        {
                            mainEntity.Antibiotics = model.Antibiotics.SerializeObject();
                            foreach (TrendChartDataItemModel item in model.ItemModel)
                            {
                                if (itemEntities.Where(r => r.Name == item.Name).Count() == 0)
                                {
                                    item.Antibiotics = item.Antibiotics.Where(r => model.Antibiotics.Select(x => x.Guid).Contains(r.Guid)).ToList();
                                    item.Value = item.Antibiotics.SerializeObject();

                                    itemEntities.Add(new Chart_TrendChartWithItemData
                                    {
                                        Id = CommonHelper.GuidToLongID,
                                        TrendChartId = mainEntity.Id,
                                        ChartType = item.ChartType,
                                        ChartColor = item.ChartColor,
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
                    if (_trendChartService.UpdateSort(model.Sort) > 0 && _trendChartService.OnEditByTransaction(mainEntity, itemEntities))
                    {
                        string logContent = $"趋势图报表编辑成功，报表名称：【{model.Name}】";
                        base.SuccessNotification(logContent);
                        base.InsetActionLog(ActionType.Create, logContent);
                        return RedirectToAction("TrendChartList", new { id = mainEntity.Id });
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

            model.DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString(),
                Selected = item.Id == model.DataSegmentId
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// 编辑初始化
        /// </summary>
        /// <param name="mainEntity"></param>
        /// <returns></returns>
        private TrendChartModel InitTrendChartEdit(Chart_TrendChart mainEntity)
        {
            TrendChartModel model = new TrendChartModel
            {
                Id = mainEntity.Id,
                DataSegmentId = mainEntity.DataSegmentId,
                DropDataSegment = _dataSegmentService.Query(r => r.Mark > 0 && r.ProjectType == DataSegmentEnum.TrendChart).OrderBy(r => r.Sort).Select(item => new SelectListItem
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
                Antibiotics = mainEntity.DataItemType == ChartDataItemType.MultipleData ? mainEntity.Antibiotics.DeserializeObject<List<TrendChartDataItemWithAntibioticModel>>() : new List<TrendChartDataItemWithAntibioticModel>(),
                IsEditPage = true
            };
            model.ItemModel = _trendChartWithItemDataService.Query(r => r.TrendChartId == mainEntity.Id && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenByDescending(r => r.UpdateTime).Select(item => new TrendChartDataItemModel
            {
                Id = item.Id,
                ChartType = item.ChartType,
                ChartColor = item.ChartColor,
                Name = item.Name,
                Value = item.Value,
                Display = item.Display,
                Sort = item.Sort
            }).ToList();

            return model;
        }
        #endregion

        #region 查看
        /// <summary>
        /// 趋势图报表详情
        /// </summary>
        /// <param name="id">趋势图报表id</param>
        /// <returns></returns>
        public ActionResult TrendChartDetail(long id)
        {
            var mainEntity = _trendChartService.QueryEntity(id);
            if (mainEntity == null || mainEntity.Id <= 0)
            {
                ErrorNotification("参数错误，无效的操作");
                return RedirectToAction("TrendChartList");
            }

            var model = InitTrendChartEdit(mainEntity);

            return View(model);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除趋势图报表
        /// </summary>
        /// <param name="id">趋势图报表id</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult TrendChart_OnDelete(long id)
        {
            var entity = _trendChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                if (_trendChartService.OnDeleteByTransaction(id))
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
            return RedirectToAction("TrendChartList");
        }
        #endregion

        #region 设置状态显示/不显示
        /// <summary>
        /// 设置趋势图报表显示与否
        /// </summary>
        /// <param name="id">趋势图报表id</param>
        /// <param name="status">是否显示</param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public ActionResult TrendChart_OnSetDisplay(long id, bool status)
        {
            var entity = _trendChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Display = status;
                entity.UpdateTime = DateTime.Now;
                _trendChartService.Update(entity);
                string logContent = $"成功设置报表【{entity.Name}】的状态为：【{(status ? "显示" : "不显示")}】";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("状态变更失败");
            }
            return RedirectToAction("TrendChartList");
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
        public ActionResult TrendChart_OnSettingDefault(long id, bool @default)
        {
            var entity = _trendChartService.QueryEntity(id);
            if (entity != null && entity.Id > 0 && entity.Mark > 0)
            {
                entity.Default = @default;
                entity.UpdateTime = DateTime.Now;
                if (@default)
                {
                    _trendChartService.Update(r => r.Default && r.Mark > 0, update => new Chart_TrendChart { Default = false });
                }
                _trendChartService.Update(entity);

                string logContent = $"{(@default ? "设置" : "取消")}报表【{entity.Name}】默认选中";
                base.SuccessNotification(logContent);
                base.InsetActionLog(ActionType.Edit, logContent);
            }
            else
            {
                ErrorNotification("操作失败");
            }
            return RedirectToAction("TrendChartList");
        }
        #endregion

        #region 单数据Excel导入
        /// <summary>
        /// 读取Excel报表数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public JsonResult TrendChartSingleExcel_OnSubmit(HttpPostedFileWrapper file)
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
            foreach (DataRow dataRow in table.Rows)
            {
                string _name = (dataRow["数据项名称"] ?? "").ToString().Trim();
                string _value = (dataRow["数值"] ?? "").ToString().Trim();
                string _display = (dataRow["是否显示"] ?? "").ToString().Trim();
                string _sort = (dataRow["显示顺序"] ?? "").ToString().Trim();

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
                    data.Add(new
                    {
                        uuid = Guid.NewGuid().ToString().ToLowerInvariant(),
                        name = _name,
                        value = __value.ToString("0.00"),
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
        /// 读取Excel报表数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [App_Start.CheckRole(CheckRole = false)]
        public JsonResult TrendChartMultipleExcel_OnSubmit(HttpPostedFileWrapper file)
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

            List<BarChartDataItemWithAntibioticModel> antibiotics = new List<BarChartDataItemWithAntibioticModel>();

            List<string> fixedColumns = new List<string> { "数据项名称", "是否显示", "图表类型" };

            foreach (DataColumn item in table.Columns)
            {
                if (string.IsNullOrWhiteSpace(item.ColumnName ?? "") || RegexHelper.IsMatch(item.ColumnName.Trim(), @"[Cc]olumn\d+") || fixedColumns.Contains(item.ColumnName.Trim()))
                {
                    continue;
                }

                antibiotics.Add(new BarChartDataItemWithAntibioticModel
                {
                    Guid = Guid.NewGuid(),
                    Name = item.ColumnName.Trim(),
                    Value = null
                });
            }

            List<string> colors = new List<string> { "#015baa", "#c1232b", "#fe8463", "#ecbf00", "#cf7ca6", "#749f83" };
            List<dynamic> data = new List<dynamic>();
            int _rowIndex = 0;
            foreach (DataRow dataRow in table.Rows)
            {
                string _name = (dataRow["数据项名称"] ?? "").ToString().Trim();
                string _display = (dataRow["是否显示"] ?? "").ToString().Trim();
                string _chartType = (dataRow["图表类型"] ?? "").ToString().Trim();

                if (string.IsNullOrWhiteSpace(_name)) { continue; }

                List<BarChartDataItemWithAntibioticModel> _values = new List<BarChartDataItemWithAntibioticModel>();
                foreach (BarChartDataItemWithAntibioticModel antibioticItem in antibiotics)
                {
                    string _value = (dataRow[antibioticItem.Name] ?? "").ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(_value) && _value.IsNumeric())
                    {
                        if (double.TryParse(_value, out double __value))
                        {
                            if (__value >= 0.0)
                            {
                                _value = __value.ToString("0.00");
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

                    _values.Add(new BarChartDataItemWithAntibioticModel
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