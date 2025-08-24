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
using ManageSystem.Services.Users;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core.Domain.Users;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Extensions;
using ManageSystem.Framework;
using ManageSystem.Services.SystemSet;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Services.Security;

namespace ManageSystem.Admin.Controllers
{
	
	public  class UsersController : AdminBaseController
	{
		private readonly IActionLogService actionLogService;
		private readonly IUserinfoService userinfoService;
        private readonly IRoleService roleService;
        private readonly IUserRoleService userRoleService;
        private readonly IEncryptionService encryptionService;
        private readonly IFunctionService functionService;
        public UsersController( 
		    IActionLogService  _actionLogService,
		    IUserinfoService  _userinfoService,
            IRoleService _roleService,
            IUserRoleService _userRoleService,
             IEncryptionService _encryptionService,
             IFunctionService _functionService
        )
		{
			this.actionLogService = _actionLogService;
			this. userinfoService =  _userinfoService;
            this.roleService = _roleService;
            this.userRoleService = _userRoleService;
            this.encryptionService = _encryptionService;
            this.functionService = _functionService;
        }

		#region 用户表

		 /// <summary>
		 /// 用户表 列表页面
		 /// </summary>
		 /// <returns></returns>
		public ActionResult UserinfoList()
		{
            var model = new UserinfoModel();
            model.UserinfoStateList = UserinfoState.Normal.ToSelectList(false).ToList();

            return View(model);
		}

		 /// <summary>
		 /// 用户表 获取数据
		 /// </summary>
		 /// <param name="command">数据源对象，分页等数据</param>
		 /// <param name="model">查询数据对象</param>
		 /// <returns></returns>
		[HttpPost]
		public ActionResult UserinfoList(DataSourceRequest command, UserinfoModel model)
		{
			//获得数据
			var list = this.userinfoService.QueryPage(model.LoginId,model.Name,model.State, command.Page - 1, command.PageSize);
			
			var gridModel = new DataSourceResult
			{
				Data = list.Select(x =>
                   {
                       return new 
                       {
                           Id = x.Id.ToString(),
                           LoginId = x.LoginId,
                           Name=x.Name,
                           NickName=x.NickName,
                           Phone=x.Phone,
                           Email=x.Email,
                           StateName=x.UserinfoState.GetDescription(),
                       };
                   } ),
				Total = list.TotalCount
			};
			
			return new JsonResult { Data = gridModel };
		}

		 /// <summary>
		 /// 用户表 创建
		 /// </summary>
		 /// <returns></returns>
		public ActionResult UserinfoCreate()
		{
			UserinfoModel model = new UserinfoModel();
            model.State = (int)UserinfoState.WaitCheck;

            var func = base.LoginUserinfo.FunctionList;

            this.SetUserPageModel(model);

            return View(model);
		}

        protected virtual void SetUserPageModel(UserinfoModel model)
        {
            model.UserinfoStateList = ((UserinfoState)model.State).ToSelectList(true, false).ToList();

            //所有的角色列表
            List<RoleModel> roleList = this.roleService.Query(m => m.Mark > 0).Select(x => x.ToModel()).ToList();
            this.ViewBag.RoleList = roleList;
            
            if (model.Id > 0)
            {
                //用户所属的角色
                List<RoleModel> userRoleList = this.roleService.GetListByUserId(model.Id).Select(x=>x.ToModel()).ToList();
                this.ViewBag.UserRoleList = userRoleList;

                model.Birthday =model.Birthday.IsNormal()?model.Birthday: null;
            }
        } 

		 /// <summary>
		 /// 用户表 保存数据
		 /// </summary>
		 /// <param name="model">保存对象</param>
		 /// <returns></returns>
		[HttpPost]
		public ActionResult UserinfoCreate(UserinfoModel model,string roleIds)
		{

            if (this.userinfoService.Count(m => m.LoginId.Equals(model.LoginId) && m.Mark > 0) > 0)
                this.ModelState.AddModelError("", "登录帐号已经存在");

            if (ModelState.IsValid) 
			{
                model.Birthday = model.Birthday ?? model.Birthday.DefaultValue();
             
                var entity = model.ToEntity();
                entity.LastLoginDate = entity.LastLoginDate.DefaultValue();

				this.userinfoService.Insert(entity);
                this.userRoleService.Insert(entity, roleIds);

                string logContent = "添加用户表，用户登录帐号： "+entity.LoginId ;
				base.InsetActionLog(ActionType.Create,logContent);
				base.SuccessNotification(logContent);

                return this.RedirectToAction("UserinfoCreate");
            }

            this.SetUserPageModel(model);

            return View(model);

		}

		 /// <summary>
		 /// 用户表 编辑
		 /// </summary>
		 /// <returns></returns>
		public ActionResult UserinfoEdit(long id)
		{
			var entity = this.userinfoService.QueryEntity(id);
			
			var model = entity.ToModel();
            model.Password = "**********";

            this.SetUserPageModel(model);

            return View(model);
		}

		 /// <summary>
		 /// 用户表 保存编辑数据
		 /// </summary>
		 /// <param name="model">保存对象</param>
		 /// <returns></returns>
		[HttpPost]
		public ActionResult UserinfoEdit(UserinfoModel model, string roleIds)
		{
            if (this.userinfoService.Count(m => m.LoginId.Equals(model.LoginId) && m.Mark > 0 && m.Id != model.Id) > 0)
                this.ModelState.AddModelError("", "登录帐号已经存在");

            if (ModelState.IsValid) 
			{
				var entity = this.userinfoService.QueryEntity(model.Id);
				base.SetDefaultValue(model, entity);

                if (!model.Password.Equals("**********"))
                {
                    model.Password = this.encryptionService.EncryptText(model.Password);
                }
                else
                {
                    model.Password = entity.Password;
                }
                  
                model.Birthday = model.Birthday ?? model.Birthday.DefaultValue();

                entity = model.ToEntity(entity);
                
				this.userinfoService.Update(entity);
                this.userRoleService.Insert(entity, roleIds);

                string logContent = "修改用户，登录帐号： "+entity.LoginId ;
				base.InsetActionLog(ActionType.Edit,logContent);
				base.SuccessNotification(logContent);

                return this.RedirectToAction("UserinfoList");
            }

            this.SetUserPageModel(model);

            return View(model);
		}

		 /// <summary>
		 /// 用户表 查看
		 /// </summary>
		 /// <returns></returns>
		public ActionResult UserinfoView(long id)
		{
			var entity = this.userinfoService.QueryEntity(id);
			
			var model = entity.ToModel();

            this.SetUserPageModel(model);
            return View(model);
		}

		 /// <summary>
		 /// 用户表 删除
		 /// </summary>
		 /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
		 /// <returns></returns>
		[HttpPost]
		public ActionResult UserinfoDelete(string selectedIds)
		{
			if (string.IsNullOrWhiteSpace(selectedIds))
			{
				base.ErrorNotification("请选择需要删除的数据");
				return this.RedirectToAction("UserinfoList");
			}
			
			this.userinfoService.Delete(selectedIds);
			
			string logContent = "【手动】删除用户表，删除的id集合:" + selectedIds  ;
			base.InsetActionLog(ActionType.Delete,logContent);
			base.SuccessNotification(logContent);
			
			return this.RedirectToAction("UserinfoList");
		}


		#endregion

	}
}
