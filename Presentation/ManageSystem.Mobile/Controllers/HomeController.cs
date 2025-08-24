using ManageSystem.Services.Meetings;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Teams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    /// <summary>
    /// CHINET数据云
    /// </summary>
    public class HomeController : MobileBaseController
    {

        private readonly ITeamService _teamService;
        public HomeController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        #region 首页
        /// <summary>
        /// 简介
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }
        #endregion

        #region 关于
        /// <summary>
        /// 关于CHINET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult About()
        {
            return View();
        }
        #endregion

        #region 单位成员
        /// <summary>
        /// CHINET数据云单位成员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Members()
        {
            return View();
        }

        //[CheckRole(false)]
        public string GetTeamlist()
        {
            string json = JsonConvert.SerializeObject(_teamService.GetTeams());
            return json;
        }
        #endregion
    }
}