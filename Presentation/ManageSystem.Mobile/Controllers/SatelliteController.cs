using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    public class SatelliteController : MobileBaseController
    {
        private readonly IMemberService MemberService;
        private readonly IAreaService AreaService;
        private readonly ISatelliteService SatelliteService;

        public SatelliteController(
          IMemberService _MemberService,
          IAreaService _AreaService,
          ISatelliteService _SatelliteService
          )
        {
            MemberService = _MemberService;
            AreaService = _AreaService;
            SatelliteService = _SatelliteService;
        }

        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 根据省市获取已经审批通过的卫星网
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <param name="cityId">市ID</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult GetSatelliteListByPC(string provinceId, string cityId)
        {
            List<Satellite> resultList = this.SatelliteService.GetLoginList().Where(x => !string.IsNullOrEmpty(x.Describe) && x.RealmName != "&").ToList();;
            return Content(resultList.SerializeObject());
        }

        [HttpPost]
        public ContentResult GetLoginList()
        {
            List<Core.Domain.SystemSet.Area> areas = new List<Core.Domain.SystemSet.Area>();
            List<Satellite> satellites = this.SatelliteService.GetLoginList().Where(x => !string.IsNullOrEmpty(x.Describe) && x.RealmName != "&").ToList();

            foreach (Satellite satellite in satellites)
            {
                Core.Domain.SystemSet.Area area = this.AreaService.QueryEntity(long.Parse(satellite.Province));
                area.Describe = satellite.RealmName;
                areas.Add(area);
            }
            return Content(areas.SerializeObject());
        }
    } 
}