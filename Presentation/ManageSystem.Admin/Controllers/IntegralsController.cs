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
using ManageSystem.Services.Integrals;
using ManageSystem.Admin.Models.Integrals;
using ManageSystem.Core.Domain.Integrals;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Extensions;

namespace ManageSystem.Admin.Controllers
{
	
	public  class IntegralsController : AdminBaseController
	{
		private readonly IIntegralSettingService IntegralSettingService;
		  
		public IntegralsController( 
		    IIntegralSettingService  _integralSettingService
		)
		{
			this. IntegralSettingService =  _integralSettingService;
		}

		#region 积分制度设置

		 /// <summary>
		 /// 积分制度设置 列表页面
		 /// </summary>
		 /// <returns></returns>
		public ActionResult IntegralSettingList()
		{
            IntegralSettingModel model = new IntegralSettingModel();
            model.StatusList = base.GetEnabledSelectList();
            model.TypeList = IntegralSettingTypeEnum.CreateMeeting.ToSelectList(false, true).ToList();
  
            return View(model);
		}

		 /// <summary>
		 /// 积分制度设置 获取数据
		 /// </summary>
		 /// <param name="command">数据源对象，分页等数据</param>
		 /// <param name="model">查询数据对象</param>
		 /// <returns></returns>
		[HttpPost]
		public ActionResult IntegralSettingList(DataSourceRequest command, IntegralSettingModel model)
		{
			
			//获得数据
			var list = this.IntegralSettingService.QueryPage(model.Name,model.Type,model.StateValue, command.Page - 1, command.PageSize);
			
			var gridModel = new DataSourceResult
			{
				Data = list.Select(x => {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Value = x.Value,
                        Name=x.Name,
                        Type = ((IntegralSettingTypeEnum)x.Type).GetDescription(),
                        Remark = x.Remark,
                        Status = x.Status ? "启用" : "禁用"
                    };
                }),
				Total = list.TotalCount
			};
			
			return new JsonResult { Data = gridModel };
		}


		 /// <summary>
		 /// 积分制度设置 设置创建页面的数据
		 /// </summary>
		 /// <returns></returns>
		public IntegralSettingModel  SetIntegralSettingCreateData()
		{
			IntegralSettingModel model = new IntegralSettingModel() ;
            model.TypeList = IntegralSettingTypeEnum.CreateMeeting.ToSelectList(true, false).ToList();
            return model;
		}

		 /// <summary>
		 /// 积分制度设置 创建
		 /// </summary>
		 /// <returns></returns>
		public ActionResult IntegralSettingCreate()
		{
			return View(this.SetIntegralSettingCreateData());
			
		}

		 /// <summary>
		 /// 积分制度设置 保存数据
		 /// </summary>
		 /// <param name="model">保存对象</param>
		 /// <returns></returns>
		[ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult IntegralSettingCreate(IntegralSettingModel model)
		{
			if (ModelState.IsValid) 
			{
				var entity = model.ToEntity();
				this.IntegralSettingService.Insert(entity);
				
				string logContent = "添加积分制度设置，制度名称："+model.Name;
				base.InsetActionLog(ActionType.Create,logContent,entity.SerializeObject());
				base.SuccessNotification(logContent);
				
				return this.RedirectToAction("IntegralSettingCreate");
			}
			
			return View(this.SetIntegralSettingCreateData());
		}


		 /// <summary>
		 /// 积分制度设置 设置编辑页面的数据
		 /// </summary>
		 /// <returns></returns>
		public IntegralSettingModel  SetIntegralSettingEditData(long id)
		{
			var entity = this.IntegralSettingService.QueryEntity(id);
			
			var model = entity.ToModel();
            model.TypeList = ((IntegralSettingTypeEnum)model.Type).ToSelectList(true, false).ToList();
          
            return model;
		}

		 /// <summary>
		 /// 积分制度设置 编辑
		 /// </summary>
		 /// <returns></returns>
		public ActionResult IntegralSettingEdit(long id)
		{
			 return this.View(this.SetIntegralSettingEditData(id));
		}

		 /// <summary>
		 /// 积分制度设置 保存编辑数据
		 /// </summary>
		 /// <param name="model">保存对象</param>
		 /// <returns></returns>
		[ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult IntegralSettingEdit(IntegralSettingModel model)
		{
			if (ModelState.IsValid) 
			{
				var entity = this.IntegralSettingService.QueryEntity(model.Id);
				base.SetDefaultValue(model, entity);
				entity = model.ToEntity(entity);
				
				this.IntegralSettingService.Update(entity);
				
				string logContent = "修改积分制度设置，制度名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit,logContent,entity.SerializeObject());
				base.SuccessNotification(logContent);
				
				return this.RedirectToAction("IntegralSettingList");
			}
			return this.View(this.SetIntegralSettingEditData(model.Id));
		}


		 /// <summary>
		 /// 积分制度设置 查看
		 /// </summary>
		 /// <returns></returns>
		public ActionResult IntegralSettingView(long id)
		{
			 return this.View(this.SetIntegralSettingEditData(id));
		}


		 /// <summary>
		 /// 积分制度设置 删除
		 /// </summary>
		 /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
		 /// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult IntegralSettingDelete(string selectedIds)
		{
			if (string.IsNullOrWhiteSpace(selectedIds))
			{
				base.ErrorNotification("请选择需要删除的数据");
				return this.RedirectToAction("IntegralSettingList");
			}
			
			this.IntegralSettingService.Delete(selectedIds);
			
			string logContent = "【手动】删除积分制度设置，删除的id集合:" + selectedIds ;
			base.InsetActionLog(ActionType.Delete,logContent,selectedIds.SerializeObject());
			base.SuccessNotification(logContent);
			
			return this.RedirectToAction("IntegralSettingList");
		}


		#endregion

	}
}
