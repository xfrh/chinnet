
using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;
using ManageSystem.Admin.Models;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Admin.Models.Users;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace ManageSystem.Admin.Controllers
{
    public class SharedController : AdminBaseController
    {
        private readonly IUserinfoService userinfoService;
        private readonly IAuthenticationService authenticationService;
        private readonly IFunctionService functionService;

        public SharedController(IUserinfoService _userinfoService,
             IAuthenticationService _authenticationService,
              IFunctionService _functionService)
        {
            this.userinfoService = _userinfoService;
            this.authenticationService = _authenticationService;
            this.functionService = _functionService;
        }

        [CheckRole(true, false)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 后台头部横向导航菜单
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [CheckRole(true, false)]
        public PartialViewResult AdminNavbar()
        {
            var userinfoEntity = this.authenticationService.GetAuthenticatedUser() as Userinfo;
            LoginModel model = new LoginModel();
            model.LoginId = userinfoEntity.LoginId;
            model.Name = userinfoEntity.Name;
            model.NickName = userinfoEntity.NickName;
            model.Password = userinfoEntity.Password;
            model.Phone = userinfoEntity.Phone;


            //当前登录用户
            this.ViewBag.UserinfoModel = model;

            //登录用户头像
            this.ViewBag.HeadImage = "";

            //修改个人资料权限
            this.ViewBag.UserProfile = true;

            //修改密码权限
            this.ViewBag.PasswordUpdate = true;

            return PartialView(model);
        }

        /// <summary>
        /// 后台菜单
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [CheckRole(true, false)]
        public PartialViewResult AdminMenu()
        {
            string url = this.Request.Url.ToString().ToLower();
            if (string.IsNullOrWhiteSpace(url)) return PartialView();

            //获取当前访问的url地址
            var routeList = this.Request.Url.Segments;
            if (routeList != null && routeList.Length > 2)
            {
                url = routeList[0] + routeList[1] + routeList[2];
                url = url.TrimEnd('/').ToLower();
            }
            url = url.ToLower();

            //当前页面的功能对象
            FunctionModel functionModel = this.functionService.QueryEntity(m => m.Mark > 0 && url.Contains(m.Url.ToLower())).ToModel();
            if (functionModel == null)
            {
                functionModel = new FunctionModel();
                functionModel.Id = 0;
            }

            //开发阶段不缓存数据
            string menuHtml = SessionLibrariy.MenuHtml;
            if (string.IsNullOrWhiteSpace(menuHtml))
            {
                menuHtml = this.GetMenuHtml(new StringBuilder(), 0);
                SessionLibrariy.MenuHtml = menuHtml;
            }

            this.ViewBag.MenuHtml = menuHtml;

            //设置前台那个菜单被显示
            functionModel.ShowFunctinId = functionModel.Id;
            if (functionModel.Id > 0)
            {
                if (!functionModel.IsMenu)
                {
                    //如果不是菜单，比如按钮（添加、修改等等），就设置他的父级菜单被选中
                    FunctionModel temp = this.functionService.QueryEntity(functionModel.FunctionId).ToModel();
                    functionModel.ShowFunctinId = temp.Id;
                }
            }

            return PartialView(functionModel);
        }


        /// <summary>
        /// 获取左侧菜单的html
        /// </summary>
        /// <param name="html"></param>
        /// <param name="functionId"></param>
        /// <returns></returns>
        [NonAction]
        [CheckRole(true, false)]
        protected string GetMenuHtml(StringBuilder html, long functionId)
        {
            var entityList = base.LoginUserinfo.FunctionList.Where(m => m.FunctionId == functionId && m.IsMenu == true && m.Mark > 0).OrderBy(p => p.Sort).ToList();

            foreach (var item in entityList)
            {
                var nodeCount = base.LoginUserinfo.FunctionList.Count(m => m.FunctionId == item.Id && m.Id != item.Id && m.Mark > 0 && m.IsMenu == true);

                html.AppendFormat("<li class=\" menu-item function-" + item.Id + " \">", "");
                html.AppendFormat("<a href=\"{0}\" class=\"{1}\">", (string.IsNullOrWhiteSpace(item.Url) ? "#" : item.Url), nodeCount > 0 ? "menu-dropdown" : "");
                html.AppendLine("   <i class=\"menu-icon " + item.CustomClass + "\"></i>");
                html.AppendLine("   <span class=\"menu-text\">" + item.Name + "</span>");
                if (nodeCount > 0) html.AppendLine("   <i class=\"menu-expand\"></i>");

                html.AppendLine("</a>");

                if (nodeCount > 0) html.AppendLine(" <ul class=\"submenu\" >");

                GetMenuHtml(html, item.Id);
                html.AppendLine(" </li>");

                if (nodeCount > 0) html.AppendLine(" </ul>");
            }

            return html.ToString();
        }


        /// <summary>
        /// 后台面包屑导航
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [CheckRole(true, false)]
        public PartialViewResult Breadcrumb(Breadcrumb model)
        {
            return PartialView(model);
        }
    }
}