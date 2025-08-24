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
using Newtonsoft.Json;

namespace ManageSystem.Web.Areas.CR.Controllers
{
    public class CRPartialViewController : WebBaseController
    {
        // GET: CRs/CRPartialView
        private readonly IAreaService AreaService;

        public CRPartialViewController(IAreaService _areaService)
        {
            this.AreaService = _areaService;
        }

        // GET: CR/PartialView
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据指定的id获取下一级的列表区域数据 
        /// </summary>
        /// <param name="parentId">区域id，如果小于等于0表示获取所有省份，否则返回该id的下一级的列表数据</param>
        /// <returns></returns>
        public ContentResult GetDistrict(long parentId)
        {
            var data = this.AreaService.QueryByParentId(parentId).SerializeObject();

            return this.Content(data);
        }

        [CheckRole(false)]
        public PartialViewResult _MenuMember()
        {
            var member = base.LoginUserinfo;
            if (member == null)
                member = new Core.Domain.Members.Member()
                {
                    Id = 0,
                    Name = "",
                    HeadImage = ""
                };

            this.ViewBag.Name = member.Name;
            this.ViewBag.Img = MemberExtensions.GetHeadImage(member.HeadImage);

            return this.PartialView();
        }
        [CheckRole(false)]
        public PartialViewResult _LayoutMenu()
        {
            var member = base.LoginUserinfo;          
            return this.PartialView(member);
        }
    }
}