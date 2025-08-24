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
using ManageSystem.Services.Medicine;
using ManageSystem.Admin.Models.Medicine;
using ManageSystem.Core.Domain.Medicine;
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

namespace ManageSystem.Admin.Controllers
{

    public class MedicineProjectController : AdminBaseController
    {
        private readonly IMedicalDataProjectService medicalDataProjectService;


        public MedicineProjectController(
       
             IMedicalDataProjectService _medicalDataProjectService
        )
        {
        
            this.medicalDataProjectService = _medicalDataProjectService;
        }

  
        #region 医学数据分类

        #region 列表
        /// <summary>
        /// 医学数据分类 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            MedicalDataProjectModel model = new MedicalDataProjectModel();
            return View(model);
        }

        /// <summary>
        /// 医学数据分类 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult List(DataSourceRequest command, MedicalDataProjectModel model)
        {
            //获得数据
            var list = this.medicalDataProjectService.QueryPage(model.Name, command.Page-1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        x.Name,
                        x.Path,
                        x.Sort,
                        x.Remark,
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 新增

        /// <summary>
        /// 医学数据分类 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            MedicalDataProjectModel model = new MedicalDataProjectModel();
            return View(model);
        }


        /// <summary>
        /// 医学数据分类 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(MedicalDataProjectModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                if (entity.Remark == null) entity.Remark = "";
                this.medicalDataProjectService.Insert(entity);


                string logContent = "添加项目管理 ";
                base.InsetActionLog(ActionType.Create, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("Create");
            }

            return View(this.SetData(model.Id));
        }

        /// <summary>
        /// 医学数据分类 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        private MedicalDataProjectModel SetData(long id)
        {
            var entity = this.medicalDataProjectService.QueryEntity(m => m.Id == id && m.Mark > 0);
            var model = entity.ToModel();
            return model;
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 医学数据分类类型 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(long id)
        {
            return this.View(this.SetData(id));
        }

        /// <summary>
        /// 医学数据分类类型 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(MedicalDataProjectModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.medicalDataProjectService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);
                if (entity.Remark == null) entity.Remark = "";
                this.medicalDataProjectService.Update(entity);

                string logContent = "修改项目管理 ";
                base.InsetActionLog(ActionType.Edit, logContent, entity.SerializeObject());
                base.SuccessNotification(logContent);

                return this.RedirectToAction("List");
            }
            return this.View(this.SetData(model.Id));
        }

        #endregion

        #region 查看
        /// <summary>
        /// 医学数据分类类型 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult View(long id)
        {
            var entity = this.medicalDataProjectService.QueryEntity(m => m.Id == id && m.Mark > 0);
            return this.View(this.SetData(id));
        }
        #endregion

        #region 删除
        /// <summary>
        /// 医学数据分类类型 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("List");
            }

            this.medicalDataProjectService.Delete(selectedIds);

            string logContent = "【手动】删除项目管理，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("List");
        }

        #endregion

        #endregion

    }
}

