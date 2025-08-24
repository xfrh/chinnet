using ManageSystem.Admin.App_Start;
using ManageSystem.Core.Utility;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class PartialViewController : Controller
    {
        private readonly IAreaService AreaService;

        public PartialViewController(IAreaService _areaService)
        {
            this.AreaService = _areaService;
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
        [CheckRole(true,false)]
        public ContentResult GetDistrictList(long parentId)
        {
            var data = this.AreaService.QueryByParentId(parentId).SerializeObject();

            return this.Content(data);
        }
    }
}