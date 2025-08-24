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
using ManageSystem.Services.SystemSet;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Core.Domain.SystemSet;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Framework;
using ManageSystem.Core.Extensions;
using ManageSystem.Services.Users;
using ManageSystem.Services.Security;
using ManageSystem.Admin.Models.Configuration;
using ManageSystem.Services.Configuration;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core.Caching;
using ManageSystem.Services.Caching;
using ManageSystem.Services.Tasks;
using ManageSystem.Core.Utility;
using ManageSystem.Admin.App_Start;
using System.Text.RegularExpressions;
using ManageSystem.Services.Satellites;

namespace ManageSystem.Admin.Controllers
{

    public class SystemSetController : AdminBaseController
    {
        private readonly IActionLogService actionLogService;
        private readonly IFunctionService functionService;
        private readonly IRoleService roleService;
        private readonly IUserinfoService userinfoService;
        private readonly IRoleFunctionService roleFunctionService;
        private readonly IEncryptionService encryptionService;
        private readonly ISystemConfigService systemConfigService;
        private readonly IScheduleTaskService ScheduleTaskService;
        private readonly ISatelliteService SatelliteService;


        private List<Function> FunctionListByRole = null;

        public SystemSetController(
            IActionLogService _actionLogService,
            IFunctionService _functionService,
            IRoleService _roleService,
            IRoleFunctionService _roleFunctionService,
            IUserinfoService _userinfoService,
            IEncryptionService _encryptionService,
            ISystemConfigService _systemConfigService,
              IScheduleTaskService _ScheduleTaskService,
              ISatelliteService _SatelliteService
        )
        {
            this.actionLogService = _actionLogService;
            this.functionService = _functionService;
            this.roleService = _roleService;
            this.roleFunctionService = _roleFunctionService;
            this.userinfoService = _userinfoService;
            this.encryptionService = _encryptionService;
            this.systemConfigService = _systemConfigService;
            this.ScheduleTaskService = _ScheduleTaskService;
            SatelliteService = _SatelliteService;
        }

        #region 系统功能

        /// <summary>
        /// 系统功能 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FunctionList()
        {
            FunctionModel model = new FunctionModel();
            model.FunctionTypeList = FunctionType.Button.ToSelectList(false).ToList();

            //设置上级功能的下拉列表
            PrepareAllFunctionModel(model);

            return View(model);
        }

        /// <summary>
        /// 系统功能 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FunctionList(DataSourceRequest command, FunctionModel model)
        {
            //获得数据
            var list = this.functionService.QueryPage(model.Name, model.FunctionId, model.Type, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Name = x.Name,
                        Sort = x.Sort,
                        Id = x.Id.ToString(),
                        Url = x.Url,
                        Breadcrumb = x.GetFormattedBreadCrumb(this.functionService),
                        TypeName = x.FunctionType.GetDescription()
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 系统功能 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult FunctionCreate()
        {
            FunctionModel model = new FunctionModel();
            model.FunctionTypeList = FunctionType.Button.ToSelectList(false, false).ToList();
            model.CreateName = base.LoginUserinfo.Name;
            model.Sort = 1;

            //设置上级功能的下拉列表
            PrepareAllFunctionModel(model);

            return View(model);
        }


        /// <summary>
        /// 系统功能 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FunctionCreate(FunctionModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.functionService.Insert(entity);

                string logContent = "添加系统功能，功能名称：" + entity.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("FunctionCreate");
            }

            model.FunctionTypeList = FunctionType.Button.ToSelectList(false, false).ToList();
            //设置上级功能的下拉列表
            PrepareAllFunctionModel(model);

            return View(model);
        }


        /// <summary>
        /// 系统功能 创建
        /// </summary>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ActionResult FunctionCreate2()
        {
            FunctionModel model = new FunctionModel();
            model.FunctionTypeList = FunctionType.Button.ToSelectList(false, false).ToList();
            model.CreateName = base.LoginUserinfo.Name;
            model.Sort = 1;

            //设置上级功能的下拉列表
            PrepareAllFunctionModel(model);

            return View(model);
        }


        /// <summary>
        /// 系统功能 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(true, false)]
        public ActionResult FunctionCreate2(FunctionModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                entity.Id = CommonHelper.GuidToLongID;

                string ptemp = model.Url;
                entity.Url = ptemp + "List";

                this.functionService.Insert(entity);

                var user = base.LoginUserinfo;

                //新增
                this.functionService.Insert(
                    new Function()
                    {
                        ControlId = "",
                        CreateName = user.Name,
                        CustomClass = "",
                        Describe = "",
                        FunctionId = entity.Id,
                        Image = "",
                        IsMenu = false,
                        Name = "添加",
                        Sort = 1,
                        Type = 3,
                        Url = ptemp + "Create"
                    });

                //编辑
                this.functionService.Insert(
                   new Function()
                   {
                       ControlId = "",
                       CreateName = user.Name,
                       CustomClass = "",
                       Describe = "",
                       FunctionId = entity.Id,
                       Image = "",
                       IsMenu = false,
                       Name = "编辑",
                       Sort = 2,
                       Type = 3,
                       Url = ptemp + "Edit"
                   });
                //查看 
                this.functionService.Insert(
                  new Function()
                  {
                      ControlId = "",
                      CreateName = user.Name,
                      CustomClass = "",
                      Describe = "",
                      FunctionId = entity.Id,
                      Image = "",
                      IsMenu = false,
                      Name = "查看",
                      Sort = 3,
                      Type = 3,
                      Url = ptemp + "View"
                  });

                //删除
                this.functionService.Insert(
                 new Function()
                 {
                     ControlId = "",
                     CreateName = user.Name,
                     CustomClass = "",
                     Describe = "",
                     FunctionId = entity.Id,
                     Image = "",
                     IsMenu = false,
                     Name = "删除",
                     Sort = 4,
                     Type = 2,
                     Url = ptemp + "Delete"
                 });

                string logContent = "添加系统功能，功能名称：" + entity.Name;
                base.InsetActionLog(ActionType.Create, logContent);


                base.SuccessNotification(logContent);

                return this.RedirectToAction("FunctionCreate");
            }

            model.FunctionTypeList = FunctionType.Button.ToSelectList(false, false).ToList();
            //设置上级功能的下拉列表
            PrepareAllFunctionModel(model);

            return View(model);
        }




        /// <summary>
        /// 系统功能 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult FunctionEdit(long id)
        {
            var entity = this.functionService.QueryEntity(id);

            var model = entity.ToModel();
            model.FunctionTypeList = ((FunctionType)model.Type).ToSelectList(true, false).ToList();

            PrepareAllFunctionModel(model);

            return View(model);
        }

        /// <summary>
        /// 系统功能 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FunctionEdit(FunctionModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.functionService.QueryEntity(model.Id);

                base.SetDefaultValue(model, entity);

                entity = model.ToEntity(entity);

                this.functionService.Update(entity);

                string logContent = "修改系统功能，功能名称：" + entity.Name;
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("FunctionList");
            }

            model.FunctionTypeList = ((FunctionType)model.Type).ToSelectList(true, false).ToList();
            PrepareAllFunctionModel(model);

            return View(model);


        }


        /// <summary>
        /// 系统功能 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult FunctionView(long id)
        {
            var entity = this.functionService.QueryEntity(id);

            var model = entity.ToModel();
            model.FunctionTypeList = FunctionType.Button.ToSelectList(false).ToList();

            return View(model);
        }


        /// <summary>
        /// 系统功能 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FunctionDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("FunctionList");
            }

            this.functionService.Delete(selectedIds);

            string logContent = "【手动】删除系统功能，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("FunctionList");
        }


        /// <summary>
        /// 加载所有的功能集合
        /// </summary>
        /// <param name="model"></param>
        [NonAction]
        protected virtual void PrepareAllFunctionModel(FunctionModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableFunctions.Add(new SelectListItem
            {
                Text = "顶级功能",
                Value = "0"
            });

            var functionList = this.functionService.Query(null, 0, false, p => p.Sort);
            foreach (var c in functionList)
            {
                model.AvailableFunctions.Add(new SelectListItem
                {
                    Text = c.GetFormattedBreadCrumb(functionList),
                    Value = c.Id.ToString(),
                    Selected = c.Id == model.Id
                });
            }
        }

        #endregion

        #region 角色管理

        /// <summary>
        /// 角色管理 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleList()
        {
            return View();
        }

        /// <summary>
        /// 角色管理 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleList(DataSourceRequest command, RoleModel model)
        {
            //获得数据
            var list = this.roleService.QueryPage(model.Name, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Name = x.Name,
                        Sort = x.Sort,
                        Id = x.Id.ToString(),
                        Describe = x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 角色管理 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleCreate()
        {
            RoleModel model = new RoleModel();
            model.CreateName = base.LoginUserinfo.Name;
            model.Sort = 100;
            model.SatelliteList = SatelliteService.Query(x=>x.Mark>0).Select(x => { return new SelectListItem() { Text = x.SatelliteName, Value = x.Id.ToString() }; }).ToList();

            //功能的html代码
            model.FunctionHtml = this.GetRoleListHtml(new StringBuilder(), 0);

            return View(model);
        }


        /// <summary>
        /// 角色管理 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.roleService.Insert(entity);

                this.roleFunctionService.Insert(entity, model.FunctionIds);

                string logContent = "添加角色，角色名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("RoleCreate");
            }

            //功能的html代码
            model.FunctionHtml = this.GetRoleListHtml(new StringBuilder(), 0);

            return View(model);

        }


        /// <summary>
        /// 角色管理 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleEdit(long id)
        {
            var entity = this.roleService.QueryEntity(id);
           
            var model = entity.ToModel();

            if (model.SatelliteList==null)
            {
                model.SatelliteList = SatelliteService.Query(x => x.Mark > 0).Select(x => { return new SelectListItem() { Text = x.SatelliteName, Value = x.Id.ToString() }; }).ToList();

                model.SatelliteList.Insert(0, new SelectListItem() { Text = "所属卫星网", Value = "0" });
            }

            this.FunctionListByRole = this.functionService.QueryByRoleId(model.Id);

            //功能的html代码
            model.FunctionHtml = this.GetRoleListHtml(new StringBuilder(), 0);

            return View(model);
        }

        /// <summary>
        /// 角色管理 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleEdit(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.roleService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.roleService.Update(entity);

                this.roleFunctionService.Insert(entity, model.FunctionIds);

                string logContent = "修改角色，角色名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent);

                base.SuccessNotification(logContent);
            }
            else
            {
                base.ErrorNotification("修改失败，请重试");
            }

            return this.RedirectToAction("RoleList", new { id = model.Id });
        }


        /// <summary>
        /// 角色管理 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleView(long id)
        {
            var entity = this.roleService.QueryEntity(id);

            var model = entity.ToModel();
            this.FunctionListByRole = this.functionService.QueryByRoleId(model.Id);

            //功能的html代码
            model.FunctionHtml = this.GetRoleListHtml(new StringBuilder(), 0);

            return View(model);
        }


        /// <summary>
        /// 角色管理 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("RoleList");
            }

            this.roleService.Delete(selectedIds);

            string logContent = "【手动】删除角色，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("RoleList");
        }



        /// <summary>
        /// 获取角色中分配权限的HTML。（递归函数）
        /// </summary>
        /// <param name="html"></param>
        /// <param name="functionId"></param>
        /// <returns></returns>
        private string GetRoleListHtml(StringBuilder html, long functionId)
        {
            var entityList = this.functionService.Query(m => m.FunctionId == functionId, 0, false, o => o.Sort);

            if (entityList.Count > 0)
            {
                html.Append(" <ul>");
                foreach (var item in entityList)
                {
                    //编辑页面设置选中
                    string check = "";
                    if (this.FunctionListByRole != null && this.FunctionListByRole.Count() > 0)
                    {
                        foreach (var functionItem in this.FunctionListByRole)
                        {
                            if (functionItem.Id == item.Id)
                            {
                                check = " checked ='checked' ";
                                break;
                            }
                        }
                    }


                    if (item.FunctionId == 0) html.Append("<div class=\"f-div row\">");

                    var nodeCount = this.functionService.Count(m => m.FunctionId == item.Id && m.Id != item.Id && m.Mark > 0);
                    html.AppendFormat("<li class=\"{0}\">", nodeCount > 0 ? "" : "role-li");
                    html.Append("<span> <div class=\"m-check\"> <label>  ");
                    html.Append("<input type=\"checkbox\" id=\"chkBox\" " + check + " class=\"colored-blue role-chk\" runat=\"server\" value=\"" + item.Id + "\" />");
                    html.Append("<span class=\"text\"></span></label>");
                    html.Append("</div> " + item.Name + "</span > ");

                    GetRoleListHtml(html, item.Id);

                    html.Append(" </li>");

                    if (item.FunctionId == 0) html.Append("</div>");
                }
                html.Append("</ul>");
            }

            return html.ToString();
        }




        #endregion

        #region 修改密码

        public ActionResult Password()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Password(PasswordModel model)
        {
            if (!model.NewPassword.Equals(model.ConfirmPassword))
                ModelState.AddModelError("ConfirmPassword", "2次输入密码不一致");

            if (model.OldPassword.Equals(model.NewPassword))
                ModelState.AddModelError("OldPassword", "新密码不能原始密码一样");

            if (ModelState.IsValid)
            {
                var oldMd5 = this.encryptionService.EncryptText(model.OldPassword);
                var userEntity = this.userinfoService.QueryEntity(base.LoginUserinfo.Id);
                if (userEntity == null || !userEntity.Password.Equals(oldMd5))
                {
                    base.ErrorNotification("原始密码不正确");
                    return View(model);
                }

                userEntity.Password = this.encryptionService.EncryptText(model.NewPassword);
                this.userinfoService.Update(userEntity);

                string logContent = "修改密码成功";
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("Password");
            }

            return View();
        }

        #endregion

        #region 系统配置

        /// <summary>
        /// 系统配置 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SettingList()
        {
            return View();
        }

        /// <summary>
        /// 系统配置 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SettingList(DataSourceRequest command, SettingModel model)
        {

            //获得数据
            var list = this.SettingService.QueryPage(model.Title, model.Name, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Name = x.Name,
                        Title = x.Title,
                        Id = x.Id.ToString(),
                        IsAdmin = x.IsAdmin ? "否" : "是",
                        IsCache = x.IsCache ? "是" : "否",
                        Describe = x.Describe,
                        InsertTime = x.InsertTime,
                        Type = x.Type,
                        Value = x.Value
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 系统配置 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public SettingModel SettingCreateData()
        {
            SettingModel model = new SettingModel();

            return model;
        }

        /// <summary>
        /// 系统配置 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult SettingCreate()
        {
            return View(this.SettingCreateData());

        }

        /// <summary>
        /// 系统配置 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SettingCreate(SettingModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.SettingService.Insert(entity);

                string logContent = "添加系统配置成功，配置名称： " + model.Title;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("SettingCreate");
            }

            return View(this.SettingCreateData());
        }


        /// <summary>
        /// 系统配置 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public SettingModel SettingEditData(long id)
        {
            var entity = this.SettingService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 系统配置 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult SettingEdit(long id)
        {
            return this.View(this.SettingEditData(id));
        }

        /// <summary>
        /// 系统配置 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SettingEdit(SettingModel model)
        {
            var entity = this.SettingService.QueryEntity(model.Id);
            base.SetDefaultValue(model, entity);

            if (!entity.IsAdmin)
                entity.Value = model.Value;

            entity.Describe = model.Describe;
            entity.Type = model.Type;

            this.SettingService.Update(entity);

            string logContent = "修改系统配置成功，配置名称： " + entity.Title;
            base.InsetActionLog(ActionType.Edit, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SettingList", new { id = entity.Id });
        }


        /// <summary>
        /// 系统配置 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult SettingView(long id)
        {
            return this.View(this.SettingEditData(id));
        }


        /// <summary>
        /// 系统配置 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SettingDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("SystemConfigList");
            }

            this.systemConfigService.Delete(selectedIds);

            string logContent = "【手动】删除系统配置，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SystemConfigList");
        }

        /// <summary>
        /// 系统配置 刷新缓存
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        public ActionResult SettingCache(string key)
        {
            string logContent = "";
            if (!string.IsNullOrWhiteSpace(key))
            {
                //清理单个缓存
                base.CacheManager.Remove(key);
                logContent = "刷新系统缓存，key：" + key;
            }
            else
            {
                //清除所有的缓存
                base.CacheManager.Clear();

                //重新缓存数据
                base.SettingService.SetAllSettingsCached();

                logContent = "刷新系统所有缓存";
            }

            base.InsetActionLog(ActionType.Delete, "【手动】" + logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SettingList");
        }

        #endregion

        #region 网站设置

        /// <summary>
        /// 网站设置
        /// </summary>
        /// <returns></returns>
        public ActionResult WebSet()
        {
            return View(this.SetWebSetData());
        }

        private WebSetModel SetWebSetData()
        {
            WebSetModel model = new WebSetModel();

            model.WebState = this.SettingService.QueryValue<bool>("web.state"); //网站维护状态
            model.WebMaintain = this.SettingService.QueryValue<string>("web.maintain"); //网站维护说明
            model.NewMessage = this.SettingService.QueryValue<string>("web.new.message"); //网站最新消息
            model.WebName = this.SettingService.QueryValue<string>("web.name"); //网站名称
            model.SystemEmail = this.SettingService.QueryValue<string>("web.system.email"); //邮箱地址
            model.Copyright = this.SettingService.QueryValue<string>("web.copyright");  //版权信息
            model.FlowCount = this.SettingService.QueryValue<string>("web.flow.count");  //流量统计代码
            model.HeadPublicJS = this.SettingService.QueryValue<string>("web.head.public.js");//页面头部公共代码
            model.FootPublicJS = this.SettingService.QueryValue<string>("web.foot.public.js");  //页面底部公共代码

            model.SEOTitle = this.SettingService.QueryValue<string>("web.seo.title");  //SEO网站标题
            model.SEOKey = this.SettingService.QueryValue<string>("web.seo.key");  //SEO网站关键词
            model.SEODescribe = this.SettingService.QueryValue<string>("web.seo.describe");  //SEO网站描述

            model.EmailUserName = this.SettingService.QueryValue<string>("web.email.user.name");  //发件人名称
            model.EmailSMTP = this.SettingService.QueryValue<string>("web.email.smtp");  //邮件服务器
            model.EmailAddress = this.SettingService.QueryValue<string>("web.email.address");  //邮箱地址
            model.EmailPassword = this.SettingService.QueryValue<string>("web.email.password");  //邮箱密码
            model.EmailPort = this.SettingService.QueryValue<string>("web.email.port");  //服务器端口

            model.NoticeEmail = this.SettingService.QueryValue<string>("web.notice.email");  //通知邮箱
            model.NoticePhone = this.SettingService.QueryValue<string>("web.notice.phone");  //通知短信

            //model.WeixinToken = this.SettingService.QueryValue<string>("weixin.app.token");  //微信Token
            //model.WeixinAppId = this.SettingService.QueryValue<string>("weixin.app.id");  //微信AppId
            //model.WeixinAppSecret = this.SettingService.QueryValue<string>("weixin.app.secret");  //微信AppSecret

            return model;

        }

        /// <summary>
        /// 保存网站配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WebSet(WebSetModel model)
        {
            //this.SettingService.UpdateValue("web.state", model.WebState.ToString());//网站维护状态
            //this.SettingService.UpdateValue("web.maintain", model.WebMaintain);//网站维护说明
            //this.SettingService.UpdateValue("web.new.message", model.NewMessage);//网站最新消息
            this.SettingService.UpdateValue("web.name", model.WebName);//网站名称

            //this.SettingService.UpdateValue("web.system.email", model.SystemEmail);//邮箱地址
            this.SettingService.UpdateValue("web.copyright", model.Copyright);//版权信息
            //this.SettingService.UpdateValue("web.flow.count", model.FlowCount);//流量统计代码
            //this.SettingService.UpdateValue("web.head.public.js", model.HeadPublicJS);//页面头部公共代码
            //this.SettingService.UpdateValue("web.foot.public.js", model.FootPublicJS);//页面底部公共代码

            this.SettingService.UpdateValue("web.seo.title", model.SEOTitle);//SEO网站标题
            this.SettingService.UpdateValue("web.seo.key", model.SEOKey);//SEO网站关键词
            this.SettingService.UpdateValue("web.seo.describe", model.SEODescribe);//SEO网站描述

            //this.SettingService.UpdateValue("web.email.user.name", model.EmailUserName);//发件人名称
            //this.SettingService.UpdateValue("web.email.smtp", model.EmailSMTP);//邮件服务器
            //this.SettingService.UpdateValue("web.email.address", model.EmailAddress);//邮箱地址
            //if (model.EmailPassword.Length < 30)
            //    this.SettingService.UpdateValue("web.email.password", model.EmailPassword);//邮箱密码
            //this.SettingService.UpdateValue("web.email.port", model.EmailPort);//服务器端口

            model.NoticeEmail = Regex.Replace(model.NoticeEmail, "，", ",");
            this.SettingService.UpdateValue("web.notice.email", model.NoticeEmail);//通知邮箱
            model.NoticePhone = Regex.Replace(model.NoticePhone, "，", ",");
            this.SettingService.UpdateValue("web.notice.phone", model.NoticePhone);//通知短信

            //this.SettingService.UpdateValue("weixin.app.token", model.WeixinToken);//微信Token
            //this.SettingService.UpdateValue("weixin.app.id", model.WeixinAppId);//微信AppId
            //this.SettingService.UpdateValue("weixin.app.secret", model.WeixinAppSecret);//微信AppSecret

            string logContent = "修改网站设置完成";
            base.InsetActionLog(ActionType.Edit, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("WebSet");
        }

        #endregion

        #region 个人中心

        public ActionResult UserProfile()
        {
            return View(this.UserProfileData());
        }

        public UserProfileModel UserProfileData()
        {
            var user = this.userinfoService.QueryEntity(base.LoginUserinfo.Id);

            UserProfileModel model = new UserProfileModel();
            model.Birthday = user.Birthday;
            model.Email = user.Email;
            model.Name = user.Name;
            model.Id = user.Id;
            model.Phone = user.Phone;
            model.Sex = user.Sex;
            model.NickName = user.NickName;
            model.LoginId = user.LoginId;

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UserProfileModel model)
        {

            if (ModelState.IsValid)
            {
                var userEntity = this.userinfoService.QueryEntity(base.LoginUserinfo.Id);

                userEntity.Name = model.Name;
                userEntity.Email = model.Email;
                userEntity.Phone = model.Phone;
                userEntity.Sex = model.Sex;
                userEntity.NickName = model.NickName;

                string logContent = "修改个人信息成功，下次登录生效 ";
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("UserProfile");
            }

            return View(this.UserProfileData());

        }

        #endregion

        #region 计划任务

        /// <summary>
        /// 计划任务 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScheduleTaskList()
        {
            return View();
        }

        /// <summary>
        /// 计划任务 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScheduleTaskList(DataSourceRequest command, SettingModel model)
        {

            //获得数据
            var list = this.ScheduleTaskService.QueryPage(model.Name, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Name = x.Name,
                        Seconds = x.Seconds,
                        Id = x.Id.ToString(),
                        Enabled = x.Enabled ? "是" : "否",
                        StopOnError = x.StopOnError ? "是" : "否",
                        LastStartUtc = x.LastStartUtc == null ? "" : x.LastStartUtc.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                        LastEndUtc = x.LastEndUtc == null ? "" : x.LastEndUtc.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                        LastSuccessUtc = x.LastSuccessUtc == null ? "" : x.LastSuccessUtc.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 计划任务 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ScheduleTaskModel ScheduleTaskEditData(long id)
        {
            var entity = this.ScheduleTaskService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 计划任务 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ScheduleTaskEdit(long id)
        {
            return this.View(this.ScheduleTaskEditData(id));
        }

        /// <summary>
        /// 计划任务 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScheduleTaskEdit(ScheduleTaskModel model)
        {
            var entity = this.ScheduleTaskService.QueryEntity(model.Id);
            base.SetDefaultValue(model, entity);

            entity.Name = model.Name;
            entity.Enabled = model.Enabled;
            entity.StopOnError = model.StopOnError;

            this.ScheduleTaskService.Update(entity);

            string logContent = "修改计划任务成功，计划任务名称： " + entity.Name;
            base.InsetActionLog(ActionType.Edit, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ScheduleTaskEdit", new { id = entity.Id });
        }


        /// <summary>
        /// 系统配置 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ScheduleTaskView(long id)
        {
            return this.View(this.ScheduleTaskEditData(id));
        }


        public ActionResult ScheduleTaskRun(int id)
        {
            try
            {
                var scheduleTask = this.ScheduleTaskService.QueryEntity(id);
                if (scheduleTask == null)
                    throw new Exception("Schedule task cannot be loaded");

                var task = new Task(scheduleTask);

                task.Enabled = true;
                task.Execute(true, false, false);

                string logContent = "运行计划任务，计划任务名称： " + scheduleTask.Name;
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return RedirectToAction("ScheduleTaskList");
        }

        #endregion

    }
}
