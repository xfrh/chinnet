using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Organism;
using ManageSystem.Web.Models.Organism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 细菌分布图表
    /// </summary>
    public class OrganismChartController : WebBaseController
    {
        private readonly IMedicalOrganismService medicalOrganismService;
        private readonly IBacteriaDetailedDataService bacteriaDetailedDataService;
        public OrganismChartController(
                IMedicalOrganismService _medicalOrganismService,
        IBacteriaDetailedDataService _bacteriaDetailedDataService
            )
        {
            medicalOrganismService = _medicalOrganismService;
            bacteriaDetailedDataService = _bacteriaDetailedDataService;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(long id = 0)
        {
            //默认显示第一个细菌的表格数据
            BacteriaModel model = this.SetBacteriaModel();

            if (id > 0)
            {
                model.MedicalOrganismId = id;
            }

            var member = base.LoginUserinfo;
            this.ViewBag.Name = member.Name;
            this.ViewBag.Img = MemberExtensions.GetHeadImage(member.HeadImage);

            var organism = this.medicalOrganismService.QueryEntity(model.MedicalOrganismId);
            model.OrganismName = organism == null ? "" : organism.Name;

            return View(model);
        }

        /// <summary>
        /// 设置 ChartList 试图模型
        /// </summary>
        /// <returns></returns>
        private BacteriaModel SetBacteriaModel()
        {
            BacteriaModel model = new BacteriaModel();

            var ids = this.bacteriaDetailedDataService.Query(c => c.Mark > 0).GroupBy(c => c.OrganismTypeId).Select(c => c.Key).ToList();
            model.MedicalOrganism = this.medicalOrganismService.Query(c => ids.Contains(c.Id)).OrderBy(c => c.Id).Select(c =>
              {
                  return new SelectListItem { Text = c.Name, Value = c.Id.ToString() };
              }).ToList();

            model.MedicalOrganismId = long.Parse(model.MedicalOrganism.FirstOrDefault().Value);

            return model;
        }


        /// <summary>
        /// 获取 细菌详细信息 绘制统计图
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBacteria(long Id)
        {
            var entity = this.bacteriaDetailedDataService.QueryEntity(Id);
            var result = new
            {
                Title = entity.Name,
                Data = new string[] {
                    entity.V0002.Contains(":") ? entity.V0002.Split(':')[1] : entity.V0002,
                    entity.V0004.Contains(":") ? entity.V0004.Split(':')[1] : entity.V0004,
                    entity.V0008.Contains(":") ? entity.V0008.Split(':')[1] : entity.V0008,
                    entity.V0016.Contains(":") ? entity.V0016.Split(':')[1] : entity.V0016,
                    entity.V0032.Contains(":") ? entity.V0032.Split(':')[1] : entity.V0032,
                    entity.V0064.Contains(":") ? entity.V0064.Split(':')[1] : entity.V0064,
                    entity.V0125.Contains(":") ? entity.V0125.Split(':')[1] : entity.V0125,
                    entity.V0025.Contains(":") ? entity.V0025.Split(':')[1] : entity.V0025,
                    entity.V0005.Contains(":") ? entity.V0005.Split(':')[1] : entity.V0005,
                    entity.V1001.Contains(":") ? entity.V1001.Split(':')[1] : entity.V1001,
                    entity.V1002.Contains(":") ? entity.V1002.Split(':')[1] : entity.V1002,
                    entity.V1004.Contains(":") ? entity.V1004.Split(':')[1] : entity.V1004,
                    entity.V1008.Contains(":") ? entity.V1008.Split(':')[1] : entity.V1008,
                    entity.V1016.Contains(":") ? entity.V1016.Split(':')[1] : entity.V1016,
                    entity.V1032.Contains(":") ? entity.V1032.Split(':')[1] : entity.V1032,
                    entity.V1064.Contains(":") ? entity.V1064.Split(':')[1] : entity.V1064,
                    entity.V1128.Contains(":") ? entity.V1128.Split(':')[1] : entity.V1128,
                    entity.V1256.Contains(":") ? entity.V1256.Split(':')[1] : entity.V1256,
                    entity.V1512.Contains(":") ? entity.V1512.Split(':')[1] : entity.V1512 },
                Distributions = entity.Distributions,
                Observations = entity.Observations,
                Ecoff = entity.Ecoff

            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询 细菌详细列表
        /// </summary>
        /// <param name="medicalOrganismTypeId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public JsonResult GetBacteriaDetailedDataModelList(long medicalOrganismId = 0)
        {

            var list = this.bacteriaDetailedDataService.Query(c => c.OrganismTypeId == medicalOrganismId);
            return Json(new { code = 0, msg = "", count = list.Count, data = list }, JsonRequestBehavior.AllowGet);
        }


    }
}