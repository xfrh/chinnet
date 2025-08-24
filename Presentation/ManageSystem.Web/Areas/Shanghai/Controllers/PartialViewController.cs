using ManageSystem.Core.Utility;
using ManageSystem.Services.Members;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.Shanghai.Controllers
{
    [CheckRole(false)]
    public class PartialViewController : WebBaseController
    {
        private readonly IAreaService AreaService;

        public PartialViewController(IAreaService _areaService)
        {
            this.AreaService = _areaService;
        }

        [CheckRole(false)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据指定的id获取下一级的列表区域数据 
        /// </summary>
        /// <param name="parentId">区域id，如果小于等于0表示获取所有省份，否则返回该id的下一级的列表数据</param>
        /// <returns></returns>
        [CheckRole(false)]
        public ContentResult GetDistrictList(long parentId)
        {
            var data = this.AreaService.QueryByParentId(parentId).SerializeObject();

            return this.Content(data);
        }

        /// <summary>
        /// 头部菜单，用户信息部分
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [CheckRole(false)]
        public PartialViewResult _MenuMember()
        {
            var member = base.LoginUserinfo;
            bool isLogin = false;
            if (member == null)
            {
                member = new Core.Domain.Members.Member()
                {
                    Id = 0,
                    Name = "匿名",
                    HeadImage = ""
                };
            }
            else
            {
                isLogin = true;
            }
            this.ViewBag.Name = member.Name;
            this.ViewBag.IsLogin = isLogin;
            this.ViewBag.Img = MemberExtensions.GetHeadImage(member.HeadImage);

            return this.PartialView();
        }

        /// <summary>
        /// 菜单部分视图
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [CheckRole(false)]
        public PartialViewResult _LayoutMenu()
        {
            var member = base.LoginUserinfo;
            if (member != null)
            {
                if (member.Median.Contains("1") || member.Median.Contains("2"))
                {
                    ViewBag.Median = true;
                }
                else
                {
                    ViewBag.Median = false;
                }
            }
            return this.PartialView(member);
        }

    }
}