using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models.Satellite;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Extensions;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Users;
using Microsoft.International.Converters.PinYinConverter;
using System.Collections.Generic;
using System.Linq;
using System.Web.ApplicationServices;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class SatelliteUserController : AdminBaseController
    {
        private readonly ISatelliteService SatelliteService;
        
        private readonly ISatelliteUserService SatelliteUserService;

        private readonly ISatelliteRoleService SatelliteRoleService;

        private readonly ISatelliteMenuRoleService SatelliteMenuRoleService;

        private readonly ISatelliteMenuService SatelliteMenuService;

        private readonly IRoleService RoleService;

        private readonly IEncryptionService encryptionService;

        private readonly IUserinfoService userinfoService;
        private readonly IUserRoleService userRoleService;


        public SatelliteUserController(ISatelliteService _SatelliteService,
            IEncryptionService _encryptionService,
            ISatelliteRoleService _SatelliteRoleService,
            ISatelliteMenuRoleService _SatelliteMenuRoleService,
            ISatelliteMenuService _SatelliteMenuService,
            IRoleService _RoleService,
            ISatelliteUserService _SatelliteUserService, IUserinfoService _userinfoService,
            IUserRoleService _userRoleService)
        {
            SatelliteService = _SatelliteService;
           
            SatelliteUserService = _SatelliteUserService;

            this.SatelliteRoleService = _SatelliteRoleService;
            this.SatelliteMenuRoleService = _SatelliteMenuRoleService;
            this.SatelliteMenuService = _SatelliteMenuService;
            this.RoleService = _RoleService;
            this.encryptionService = _encryptionService;
            this.userinfoService = _userinfoService;
            this.userRoleService = _userRoleService;


        }

        [HttpPost]
        [CheckRole(false, false)]
        public ActionResult SatelliteUserListPage(DataSourceRequest command, SatelliteUserModel model)
        {
            var list = this.SatelliteUserService.QueryPage(model.SatelliteId, model.UserName,command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        UserName = x.UserName,
                        PassWord = x.PassWord,
                        EditBtn = "<a href=\"/SatelliteUser/SatelliteEdit/" + x.Id + "\">编辑</a>",
                        DelBtn = "<a href=\"javascript:DelUser('" + x.Id.ToString() + "')\">删除</a>",
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }



        [CheckRole(false, false)]
        public ActionResult SatelliteUserList(string ID)
        {
            var modelentity = this.SatelliteService.QueryEntity(long.Parse(ID));
            SatelliteUserModel satelliteUser = new SatelliteUserModel();
            satelliteUser.SatelliteId = modelentity.Id;
            
            return View(satelliteUser);
        }
        [CheckRole(false, false)]
        public ActionResult SatelliteUserInfo(long ID)
        {
            //var modelentity = this.SatelliteUserService.QueryEntity(long.Parse(ID));
            //SatelliteUserModel satellite = new SatelliteUserModel();
            //satellite.PassWord = modelentity.PassWord;
            //satellite.UserName = modelentity.UserName;
            //所有的角色列表
            SatelliteUserModel model = new SatelliteUserModel();
            model.State = (int)UserinfoState.WaitCheck;
            model.SatelliteId = ID;
            var func = base.LoginUserinfo.FunctionList;

            this.SetUserPageModel(model);
            return View(model);
        }

        /// <summary>
        /// 用户表 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false, false)]
        public ActionResult SatelliteUserDelete(SatelliteUserModel model,string selectedIds)
        {
            //if (string.IsNullOrWhiteSpace(selectedIds))
            //{
            //    base.ErrorNotification("请选择需要删除的数据");
            //    return this.RedirectToAction("SatelliteUserInfo", new { id = model.SatelliteId });
            //}

            this.SatelliteUserService.Delete(model.Id);

            string logContent = "【手动】删除卫星网用户表，删除的id集合:" + model.Id;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SatelliteUserList", new { id=model.SatelliteId });
        }


        /// <summary>
        /// 用户表 编辑
        /// </summary>
        /// <returns></returns>
        [CheckRole(false, false)]
        public ActionResult SatelliteEdit(long id)
        {
            var entity = this.SatelliteUserService.QueryEntity(id);

            var model = entity.ToModel();

            this.SetUserPageModel(model);

            return View(model);
        }
        [HttpPost]
        [CheckRole(false, false)]
        public ActionResult SatelliteEdit(SatelliteUserModel model, string roleIds)
        {
            if (this.SatelliteUserService.Count(m => m.UserName.Equals(model.UserName) && m.Mark > 0 && m.Id != model.Id) > 0)
                this.ModelState.AddModelError("", "卫星网帐号已经存在");

            if (ModelState.IsValid)
            {
                var entity = this.SatelliteUserService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);

                //if (!model.PassWord.Equals("**********"))
                //{
                //    model.PassWord = this.encryptionService.EncryptText(model.PassWord);
                //}
                //else
                //{
                //    model.PassWord = entity.PassWord;
                //}


                entity = model.ToEntity(entity);

                this.SatelliteUserService.Update(entity);
                this.SatelliteMenuRoleService.Insert(entity, roleIds);

                string logContent = "修改用户，登录帐号： " + entity.UserName;
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                
            }

            this.SetUserPageModel(model);

            return this.RedirectToAction("SatelliteUserList", new { id = model.SatelliteId });
        }
        /// <summary>
        /// 用户表 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false, false)]
        public ActionResult SatelliteUserCreate(SatelliteUserModel model, string roleIds)
        {

            if (this.SatelliteUserService.Count(m => m.UserName.Equals(model.UserName) && m.Mark > 0) > 0)
                this.ModelState.AddModelError("", "卫星网帐号已经存在");

            if (ModelState.IsValid)
            {


                var entity = model.ToEntity();
                this.SatelliteUserService.Insert(entity);
                this.SatelliteMenuRoleService.Insert(entity, roleIds);

                string logContent = "添加卫星网用户表，用户登录帐号： " + model.UserName;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                this.SetUserPageModel(entity.ToModel());
            }

            

            return this.RedirectToAction("SatelliteUserList", new { id = model.SatelliteId });

        }

        /// <summary>
        /// 列表页面上方创建用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false, false)]
        public ActionResult CreatUser(long ID,string account,string password)
        {
            SatelliteUserModel model = new SatelliteUserModel();
            model.UserName = account;
            model.PassWord = password;
            model.SatelliteId = ID;

            if (this.SatelliteUserService.Count(m => m.UserName.Equals(model.UserName) && m.Mark > 0) > 0)
                this.ModelState.AddModelError("", "卫星网帐号已经存在");

            string roleIds = "3,4,5";

            if (ModelState.IsValid)
            {

                var entity = model.ToEntity();
                this.SatelliteUserService.Insert(entity);
                this.SatelliteMenuRoleService.Insert(entity, roleIds);

                string logContent = "添加卫星网用户表，用户登录帐号： " + model.UserName;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                this.SetUserPageModel(entity.ToModel());
            }
            return this.RedirectToAction("SatelliteUserList", new { id = ID });
        }


        protected virtual void SetUserPageModel(SatelliteUserModel model)
        {
            model.UserinfoStateList = ((UserinfoState)model.State).ToSelectList(true, false).ToList();

            //所有的角色列表
            List<SatelliteMenuModel> roleList = this.SatelliteMenuService.Query(m => m.Mark > 0).Select(x => x.ToModel()).ToList();
            //List<RoleModel> roleList = this.RoleService.Query(m => m.Mark > 0).Select(x => x.ToModel()).ToList();
            this.ViewBag.RoleList = roleList;

            if (model.Id > 0)
            {
                //用户所属的角色
                List<SatelliteMenuModel> userRoleList = this.SatelliteMenuService.GetListBySatelliteUserId(model.Id).Select(x => x.ToModel()).ToList();
                this.ViewBag.UserRoleList = userRoleList;

            }
        }

    }
}