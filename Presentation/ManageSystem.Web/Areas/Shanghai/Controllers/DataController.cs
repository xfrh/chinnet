using ManageSystem.Core.Domain.SHChart;
using ManageSystem.Core.Utility;
using ManageSystem.Services.SHChart;
using ManageSystem.Services.Teams;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using ManageSystem.Web.Models.ShanghaiData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Areas.Shanghai.Controllers
{
    /// <summary>
    /// 数据报表
    /// </summary>
    [CheckRole(false)]
    public class DataController : WebBaseController
    {
        private readonly ISHDataSegmentService _dataSHSegmentService;
        private readonly ISHBarChartService _barSHChartService;
        private readonly ISHBarChartWithItemDataService _barSHChartWithItemDataService;

        private readonly ITeamService _teamService;
        public DataController(ISHDataSegmentService SHdataSegmentService,
            ISHBarChartService SHbarChartService,
            ISHBarChartWithItemDataService SHbarChartWithItemDataService, ITeamService teamService)
        {
            this._dataSHSegmentService = SHdataSegmentService;
            this._barSHChartService = SHbarChartService;
            this._barSHChartWithItemDataService = SHbarChartWithItemDataService;
            _teamService = teamService;
        }
       [CheckRole(false)]
        public ActionResult Year()
        {
            List<SHBarChartDTO> barCharts = _dataSHSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new SHBarChartDTO
            {
                Id = item.Id,
                Name = item.Name,
                Data_type=item.Data_type,
                DataItems = _barSHChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new SHBarChartDataItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Default = m.Default
                }).ToList()
            }).ToList();

            return View(barCharts);
        }

        [HttpPost]
        [CheckRole(false)]
        public ActionResult AntibioticDrugFastData(long id)
        {
            Chart_SHBarChart barChart = _barSHChartService.QueryEntity(id);

            List<string> legends = new List<string>();
            List<string> xdata = new List<string>();
            List<dynamic> series = new List<dynamic>();
            List<string> colors = new List<string>();

            IQueryable<Chart_SHBarChartWithItemData> dataItems = _barSHChartWithItemDataService.Query(r => r.Mark > 0 && r.SHBarChartId == barChart.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).AsQueryable();
            int xRotate = 0;
            switch (barChart.DataItemType)
            {
                case ChartSHDataItemType.SingleData:
                    {
                        List<dynamic> series_data = new List<dynamic>();
                        foreach (Chart_SHBarChartWithItemData item in dataItems)
                        {
                            xdata.Add(item.Name);
                            series_data.Add(new { name = item.Name, value = item.Display ? item.Value : "-" });
                        }

                        colors.Add(string.IsNullOrWhiteSpace(dataItems.FirstOrDefault().ChartColor) ? "#015baa" : dataItems.FirstOrDefault().ChartColor);
                        series.Add(new
                        {
                            name = "数值",
                            type = dataItems.FirstOrDefault().ChartType == SHChartTypeEnum.Line ? "line" : "bar",
                            barWidth = barChart.Name.StartsWith("各医院") ? 15 : 30,
                            data = series_data,
                            itemStyle = new
                            {
                                normal = new
                                {
                                    label = new
                                    {
                                        show = true,
                                        position = "top",
                                        formatter = "{c}"
                                    }
                                }
                            }
                        });

                        if (xdata.Count > 10 && xdata.Count <= 12)
                        {
                            xRotate = 15;
                        }
                        else if (xdata.Count > 12 && xdata.Count <= 18)
                        {
                            xRotate = 30;
                        }
                        else if (xdata.Count > 18 && xdata.Count <= 25)
                        {
                            xRotate = 45;
                        }
                        else if (xdata.Count > 25)
                        {
                            xRotate = 60;
                        }
                        xRotate = barChart.Name.StartsWith("各医院") ? 30 : xRotate;
                    }
                    break;
                case ChartSHDataItemType.MultipleData:
                    {
                        List<SHBarChartDataItemWithAntibioticModel> Antibiotics = barChart.Antibiotics.DeserializeObject<List<SHBarChartDataItemWithAntibioticModel>>() ?? new List<SHBarChartDataItemWithAntibioticModel>();
                        Antibiotics.ForEach(item =>
                        {
                            xdata.Add(item.Name);
                        });

                        foreach (Chart_SHBarChartWithItemData item in dataItems)
                        {
                            if (!legends.Contains(item.Name))
                            {
                                legends.Add(item.Name);
                            }

                            if (!colors.Contains(item.ChartColor))
                            {
                                colors.Add(item.ChartColor);
                            }

                            List<dynamic> series_data = new List<dynamic>();

                            List<SHBarChartDataItemWithAntibioticModel> values = item.Value.DeserializeObject<List<SHBarChartDataItemWithAntibioticModel>>();

                            foreach (SHBarChartDataItemWithAntibioticModel antibiotic in Antibiotics)
                            {

                                var _value = values.Where(r => r.Guid == antibiotic.Guid).FirstOrDefault().Value;
                                series_data.Add(new { name = antibiotic.Name, value = item.Display ? _value : "-" });
                            }

                            series.Add(new
                            {
                                name = item.Name,
                                type = item.ChartType == SHChartTypeEnum.Line ? "line" : "bar",
                                stack = "",
                                barWidth = dataItems.Count() <= 4 && Antibiotics.Count() <=9 ? 30 : 0,
                                label = new
                                {
                                    normal = new
                                    {
                                        show = true,
                                        position = "top",
                                        formatter = "{c}"
                                    }
                                },
                                data = series_data
                            });
                        }

                        if (xdata.Count > 8 && xdata.Count <= 12)
                        {
                            xRotate = 15;
                        }
                        else if (xdata.Count > 12 && xdata.Count <= 18)
                        {
                            xRotate = 35;
                        }
                        else if (xdata.Count > 18 && xdata.Count <= 25)
                        {
                            xRotate = 45;
                        }
                        else if (xdata.Count > 25)
                        {
                            xRotate = 60;
                        }
                    }
                    break;
                default:
                    break;
            }



            return Json(new
            {
                title = barChart.Title,
                subtitle = barChart.SubTitle,
                color = colors,
                rotate = xRotate,
                xAxis = xdata,
                legend =legends,
            series
            });
        }

       
        [CheckRole(false)]
        public string Get_SHBarChartsTable(long Id)
        {
            string res = _barSHChartService.Get_SHBarChartsTable(Id).SerializeObject();
            return res;
        }

        public string GetTeamlist()
        {
            string json = JsonConvert.SerializeObject(_teamService.GetSHTeams());
            return json;
        }
    }
}