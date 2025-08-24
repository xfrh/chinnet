using ManageSystem.Core.Utility;
using ManageSystem.Services.CRs;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.CR.Controllers
{
    [CheckRole(false)]
    public class CRDataController : WebBaseController
    {
        // GET: CRs/CRData
        private readonly ICRService CRService;
        private readonly IAreaService AreaService;

        public CRDataController(ICRService _CRService, IAreaService _areaService)
        {
            this.CRService = _CRService;
            this.AreaService = _areaService;
        }

        [CheckRole(false)]
        // GET: CR/Data
        public ActionResult Index()
        {
            ViewBag.year = CRService.GetyearList();
            ViewBag.def = CRService.GetYear();
            return View();
        }
        /// <summary>
        /// 默认显示数据
        /// </summary>
        /// <returns></returns>
        public string GetCRslist(string year)
        {
            //EchartscofingController.GetEcharts(null,null);
            string json = JsonConvert.SerializeObject(CRService.GetCRslist(year));
            return json;
        }

        /// <summary>
        /// 数据展示市区
        /// </summary>
        /// <returns></returns>
        public string GetCitylist(string year)
        {
            //EchartscofingController.GetEcharts(null,null);
            string json = JsonConvert.SerializeObject(CRService.GetCitylist(year));
            return json;
        }

        [CheckRole(false)]
        // GET: CR/Data
        public ActionResult Histogram()
        {
            return View();
        }
        [CheckRole(false)]
        public ActionResult Introduction()
        {
            return View();
        }
    }
}