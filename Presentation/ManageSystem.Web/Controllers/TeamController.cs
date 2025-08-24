using ManageSystem.Services.Teams;
using ManageSystem.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ManageSystem.Services.Satellites;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 团队介绍
    /// </summary>
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly ISatelliteService SatelliteService;
        public TeamController(ITeamService teamService, ISatelliteService satelliteService)
        {
            _teamService = teamService;
            SatelliteService = satelliteService;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult SaIndex()
        {
            return View();
        }

        [CheckRole(false)]
        public string GetTeamlist()
        {
            var list = _teamService.GetTeams();
            foreach (var item in list)
            {
                if (item.Title.Contains("重庆"))
                    item.Title= item.Title.Replace("重庆","冲庆");
            }
            list = list.OrderBy(x => x.Title).ToList();
            foreach (var item in list)
            {
                if (item.Title.Contains("冲庆"))
                    item.Title = item.Title.Replace("冲庆", "重庆");
            }
            string json = JsonConvert.SerializeObject(list);
            return json;
        }

        [CheckRole(false)]
        public string GetSatelliteTeamlist(string city)
        {
            if (string.IsNullOrEmpty(city))
                return null;
            var entty = this.SatelliteService.Query().Single(x => x.RealmName == city);
            string json = JsonConvert.SerializeObject(_teamService.GetTeamsSatellite(entty.SatelliteName));
            return json;
        }

    }
}