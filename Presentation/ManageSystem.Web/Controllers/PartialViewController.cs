using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Members;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class PartialViewController : WebBaseController
    {
        private readonly IAreaService AreaService;
        private readonly ISatelliteMenuRoleService SatelliteMenuRoleService;

        private readonly ISatelliteMenuService SatelliteMenuService;

        public PartialViewController(ISatelliteMenuRoleService _SatelliteMenuRoleService,
            ISatelliteMenuService _SatelliteMenuService, IAreaService _areaService)
        {
            this.AreaService = _areaService;
            this.SatelliteMenuRoleService = _SatelliteMenuRoleService;
            this.SatelliteMenuService = _SatelliteMenuService;
        }

        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 根据指定的id获取下一级的列表区域数据 
        /// </summary>
        /// <param name="parentId">区域id，如果小于等于0表示获取所有省份，否则返回该id的下一级的列表数据</param>
        /// <returns></returns>
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
        public PartialViewResult _MenuMemberMap()
        {
            var member = base.LoginSatelliteUserinfo;
            bool isLogin = false;

            if (member == null)
            {
                member = new Core.Domain.Sate.SatelliteUser()
                {
                    Id = 0,
                    UserName = "匿名",
                };
            }
            else
            {
                isLogin = true;
            }

            this.ViewBag.IsLogin = isLogin;
            this.ViewBag.Name = member.UserName;

            return this.PartialView();
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

            this.ViewBag.IsLogin = isLogin;
            this.ViewBag.Name = member.Name;
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
                if (member.Median != null)
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

                if (member.OnlineUpload != 0)
                {
                    ViewBag.OnlineUpload = true;
                }
                else
                {
                    ViewBag.OnlineUpload = false;
                }
            }
            return this.PartialView(member);
        }


        /// <summary>
        /// 菜单部分视图
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [CheckRole(false)]
        public PartialViewResult _LayoutMenuMap()
        {
            var member = base.LoginSatelliteUserinfo;
            List<SatelliteMenu> userRoleList = new List<SatelliteMenu>();
            var list = this.SatelliteMenuService.Query();
            //所有的角色列表
            //List<SatelliteMenu> roleList = this.SatelliteMenuService.Query(m => m.Mark > 0).Select(x => x).ToList();
            //List<RoleModel> roleList = this.RoleService.Query(m => m.Mark > 0).Select(x => x.ToModel()).ToList();
            if (member == null)
            {
                userRoleList.Add(this.SatelliteMenuService.Query().Single(x => x.Name.Contains("成员单位")));
                userRoleList.Add(this.SatelliteMenuService.Query().Single(x => x.Name.Contains("信息动态")));
                userRoleList.Add(this.SatelliteMenuService.Query().Single(x => x.Name.Contains("资料下载")));
                userRoleList.Add(this.SatelliteMenuService.Query().Single(x => x.Name.Contains("上传数据")));

            }
            else
            {
                //用户所属的角色
                userRoleList = this.SatelliteMenuService.GetListBySatelliteUserId(member.Id).Select(x => x).ToList();
            }



            this.ViewBag.MenuList = userRoleList;
            return this.PartialView(member);
        }
    }
}