using ManageSystem.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Core;
using ManageSystem.Services.Log;
using ManageSystem.Admin.Models.Log;
using ManageSystem.Core.Domain.Log;
using System.Linq.Expressions;
using ManageSystem.Admin.Extensions;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Core.Extensions;
using ManageSystem.Framework;

namespace ManageSystem.Admin.Controllers
{
    public class LogController : AdminBaseController
    {
        private readonly IActionLogService actionLogService;
        private readonly ISystemLogService systemLogService;

        public LogController(
            IActionLogService _actionLogService,
            ISystemLogService _systemLogService
        )
        {
            this.actionLogService = _actionLogService;
            this.systemLogService = _systemLogService;
        }

        #region 系统日志

        /// <summary>
        /// 系统日志 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemLogList()
        {
            SystemLogModel model = new SystemLogModel();
            model.LevelList = SystemLogLevel.Debug.ToSelectList(false).ToList() ;

            return View(model);
        }

        /// <summary>
        /// 系统日志 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SystemLogList(DataSourceRequest command, SystemLogModel model)
        {
            //获得数据
            var list = this.systemLogService.QueryPage(model.Title,model.LevelId, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new 
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        LevelName = x.Level.GetDescription(),
                        Logger = x.Logger,
                        InsertTime = x.InsertTime,
                        IPAddress=x.IPAddress
                    };

                }),
                Total = list.TotalCount
            };
                   
            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 系统日志 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SystemLogDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("SystemLogList");
            }

            this.systemLogService.Delete(selectedIds);

            string logContent = "【手动】删除系统日志成功，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SystemLogList");
        }


        /// <summary>
        /// 系统日志 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemLogView(long id)
        {
            var entity = this.systemLogService.QueryEntity(id);

            var model = entity.ToModel();
            model.LevelList = ((SystemLogLevel)model.LevelId).ToSelectList(true, false).ToList();

            return View(model);
        }

        #endregion


        #region 操作日志

        /// <summary>
        /// 操作日志 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ActionLogList()
        {
            ActionLogModel model = new ActionLogModel();
            model.TypeList = ActionType.Create.ToSelectList(false).ToList();
            model.SourceList = ActionSource.Admin.ToSelectList(false).ToList();

            return View(model);
        }

        /// <summary>
        /// 操作日志 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActionLogList(DataSourceRequest command, ActionLogModel model)
        {
            //获得数据
            var list = this.actionLogService.QueryPage(model.Content, model.TypeId,model.Source, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = list.Select(x=> { return new {
                    Id=x.Id.ToString(),
                    IPAddress = x.IPAddress,
                    InsertTime = x.InsertTime,
                    SourceValue=((ActionSource)x.Source).GetDescription(),
                    Type=x.Type,
                    Content=x.Content,
                    UserinfoName=x.UserinfoName
                }; }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }



        /// <summary>
        /// 操作日志 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ActionLogView(long id)
        {
            var entity = this.actionLogService.QueryEntity(id);

            var model = entity.ToModel();

            return View(model);
        }


        /// <summary>
        /// 操作日志 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ActionLogDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ActionLogList");
            }

            this.actionLogService.Delete(selectedIds);

            string logContent = "【手动】删除操作日志成功，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ActionLogList");
        }


        #endregion

    }
}