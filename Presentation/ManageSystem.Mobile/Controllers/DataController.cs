using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Domain.SHChart;
using ManageSystem.Core.Utility;
using ManageSystem.Mobile.Models.Data;
using ManageSystem.Services.Chart;
using ManageSystem.Services.Datas;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.SHChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    /// <summary>
    /// 图表控制器
    /// </summary>
    public class DataController : MobileBaseController
    {
        #region 业务声明
        /// <summary>
        /// 细菌药物业务层
        /// </summary>
        private readonly IDataAntibioticDrugFastService DataAntibioticDrugFastService;

        private readonly IDataSegmentService _dataSegmentService;
        private readonly IHeatmapService _heatmapService;
        private readonly IHeatmapItemService _heatmapItemService;
        private readonly IBarChartService _barChartService;
        private readonly IBarChartWithItemDataService _barChartWithItemDataService;
        private readonly ITrendChartService _trendChartService;
        private readonly ITrendChartWithItemDataService _trendChartWithItemDataService;
        private readonly ISatelliteService _SatelliteService;
        private readonly ISHDataSegmentService _dataSHSegmentService;
        private readonly ISHBarChartService _barSHChartService;
        private readonly ISHBarChartWithItemDataService _barSHChartWithItemDataService;


        private static readonly Dictionary<string, string> _provinceMap = new Dictionary<string, string>
        {
            ["北京"] = "北京市",
            ["天津"] = "天津市",
            ["河北"] = "河北省",
            ["山西"] = "山西省",
            ["内蒙古"] = "内蒙古自治区",
            ["辽宁"] = "辽宁省",
            ["吉林"] = "吉林省",
            ["黑龙江"] = "黑龙江省",
            ["上海"] = "上海市",
            ["江苏"] = "江苏省",
            ["浙江"] = "浙江省",
            ["安徽"] = "安徽省",
            ["福建"] = "福建省",
            ["江西"] = "江西省",
            ["山东"] = "山东省",
            ["河南"] = "河南省",
            ["湖北"] = "湖北省",
            ["湖南"] = "湖南省",
            ["广东"] = "广东省",
            ["广西"] = "广西壮族自治区",
            ["海南"] = "海南省",
            ["重庆"] = "重庆市",
            ["四川"] = "四川省",
            ["贵州"] = "贵州省",
            ["云南"] = "云南省",
            ["西藏"] = "西藏自治区",
            ["陕西"] = "陕西省",
            ["甘肃"] = "甘肃省",
            ["青海"] = "青海省",
            ["宁夏"] = "宁夏回族自治区",
            ["新疆"] = "新疆维吾尔自治区",
            ["台湾"] = "台湾省",
            ["香港"] = "香港特别行政区",
            ["澳门"] = "澳门特别行政区"
        };
        #endregion

        #region 构造函数
        /// <summary>
        /// 图表有参构造函数
        /// </summary>
        /// <param name="_dataAntibioticDrugFastService"></param>
        public DataController(
            IDataAntibioticDrugFastService _dataAntibioticDrugFastService,
            IDataSegmentService dataSegmentService,
            IHeatmapService heatmapService,
            IHeatmapItemService heatmapItemService,
            IBarChartService barChartService,
            IBarChartWithItemDataService barChartWithItemDataService,
            ITrendChartService trendChartService,
            ITrendChartWithItemDataService trendChartWithItemDataService,
            ISatelliteService satelliteService,
            ISHDataSegmentService sHDataSegmentService,
            ISHBarChartService sHBarChartService,
            ISHBarChartWithItemDataService sHBarChartWithItemDataService)
        {
            DataAntibioticDrugFastService = _dataAntibioticDrugFastService;

            this._dataSegmentService = dataSegmentService;
            this._heatmapService = heatmapService;
            this._heatmapItemService = heatmapItemService;
            this._barChartService = barChartService;
            this._barChartWithItemDataService = barChartWithItemDataService;
            this._trendChartService = trendChartService;
            this._trendChartWithItemDataService = trendChartWithItemDataService;
            this._SatelliteService = satelliteService;
            this._dataSHSegmentService = sHDataSegmentService;
            this._barSHChartService = sHBarChartService;
            this._barSHChartWithItemDataService = sHBarChartWithItemDataService;
        }
        #endregion

        // GET: Data
        public ActionResult GermYear()
        {
            return View();
        }

        public ActionResult Data1()
        {
            return View();
        }

        #region 热图
        /// <summary>
        /// 热图
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("heatmap.cshtml")]
        public ViewResult HeatMap()
        {
            Expression<Func<Chart_DataSegment, bool>> predicate = r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.Heatmap;

            List<HeatmapDTO> heatmaps = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new HeatmapDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _heatmapService.Query(r => r.Display && r.Mark > 0 && r.DataSegmentId == item.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).Select(r => new HeatmapItemDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Default = r.Default
                }).ToList()
            }).ToList();

            return View(heatmaps);
        }

        [HttpPost]
        public ActionResult HeatMapData(long id)
        {
            var entity = _heatmapService.QueryEntity(id);
            var _ds = _dataSegmentService.QueryEntity(entity.DataSegmentId);
            var data = _heatmapItemService.Query(r => r.HeatmapId == id && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).Select(item => new
            {
                name = _provinceMap.FirstOrDefault(r => r.Value == item.Name).Key,
                value = item.Value
            }).ToList();
            return Json(new
            {
                title = $"{_ds.Name}",
                subtitle = $"({entity.Name})",
                data = data
            });
        }
        #endregion

        #region 柱状图
        /// <summary>
        /// 柱状图
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("germ/bar.cshtml")]
        public ViewResult GermWithBar()
        {
            List<BarChartDTO> barCharts = _dataSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _barChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new BarChartDataItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Default = m.Default
                }).ToList()
            }).ToList();

            return View(barCharts);
        }

        /// <summary>
        /// 柱状图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult GermWithBarMap(string cityId)
        {
            Satellite satellite = this._SatelliteService.GetLoginList().First(x=>x.RealmName == cityId);
            List<BarChartDTO> barCharts = _dataSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _barChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0&&r.SatelliteId == satellite.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new BarChartDataItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Default = m.Default
                }).ToList()
            }).ToList();

            return View(barCharts);
        }

        public ActionResult Shanghai()
        {
            List<SHBarChartDTO> barCharts = _dataSHSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == SHDataSegmentEnum.SHBarChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new SHBarChartDTO
            {
                Id = item.Id,
                Name = item.Name,
                Data_type = item.Data_type,
                DataItems = _barSHChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new SHBarChartDataItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Default = m.Default
                }).ToList()
            }).ToList();

            //List<BarChartDTO> _barCharts = barCharts.Select(item => new BarChartDTO { Id = item.Id, Name = item.Name, DataItems = item.DataItems.Select(m => new BarChartDataItemDTO { Id = m.Id, Name = m.Name, Default = m.Default }).ToList() }).ToList();

            return View(barCharts);
        }



        [HttpPost]
        public ActionResult GermWithBarData(long id)
        {
            Chart_BarChart barChart = _barChartService.QueryEntity(id);

            List<string> xdata = new List<string>();
            List<dynamic> series = new List<dynamic>();
            List<string> colors = new List<string>();

            IQueryable<Chart_BarChartWithItemData> dataItems = _barChartWithItemDataService.Query(r => r.Mark > 0 && r.BarChartId == barChart.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).AsQueryable();
            int xRotate = 15;
            switch (barChart.DataItemType)
            {
                case ChartDataItemType.SingleData:
                    {
                        List<dynamic> series_data = new List<dynamic>();
                        foreach (Chart_BarChartWithItemData item in dataItems)
                        {
                            xdata.Add(item.Name);
                            series_data.Add(new { name = item.Name, value = item.Display ? item.Value : "-" });
                        }

                        colors.Add(string.IsNullOrWhiteSpace(dataItems.FirstOrDefault().BarColor) ? "#015baa" : dataItems.FirstOrDefault().BarColor);
                        series.Add(new
                        {
                            name = "数值",
                            type = dataItems.FirstOrDefault().ChartType == ChartTypeEnum.Line ? "line" : "bar",
                            barWidth = barChart.Name.StartsWith("各医院") ? 10 : 15,
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

                        if (xdata.Count > 8 && xdata.Count <= 12)
                        {
                            xRotate = 35;
                        }
                        else if (xdata.Count > 12 && xdata.Count <= 18)
                        {
                            xRotate = 45;
                        }
                        else if (xdata.Count > 18 && xdata.Count <= 25)
                        {
                            xRotate = 55;
                        }
                        else if (xdata.Count > 25)
                        {
                            xRotate = 65;
                        }
                        xRotate = barChart.Name.StartsWith("各医院") ? 30 : xRotate;
                    }
                    break;
                case ChartDataItemType.MultipleData:
                    {
                        List<BarChartDataItemWithAntibioticModel> Antibiotics = barChart.Antibiotics.DeserializeObject<List<BarChartDataItemWithAntibioticModel>>() ?? new List<BarChartDataItemWithAntibioticModel>();
                        Antibiotics.ForEach(item =>
                        {
                            xdata.Add(item.Name);
                        });

                        foreach (Chart_BarChartWithItemData item in dataItems)
                        {
                            if (!colors.Contains(item.BarColor))
                            {
                                colors.Add(item.BarColor);
                            }

                            List<dynamic> series_data = new List<dynamic>();

                            List<BarChartDataItemWithAntibioticModel> values = item.Value.DeserializeObject<List<BarChartDataItemWithAntibioticModel>>();

                            foreach (BarChartDataItemWithAntibioticModel antibiotic in Antibiotics)
                            {

                                var _value = values.Where(r => r.Guid == antibiotic.Guid).FirstOrDefault().Value;
                                series_data.Add(new { name = antibiotic.Name, value = item.Display ? _value : "" });
                            }

                            series.Add(new
                            {
                                name = item.Name,
                                type = item.ChartType == ChartTypeEnum.Line ? "line" : "bar",
                                stack = "",
                                barWidth = 10,
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
                            xRotate = 35;
                        }
                        else if (xdata.Count > 12 && xdata.Count <= 18)
                        {
                            xRotate = 45;
                        }
                        else if (xdata.Count > 18 && xdata.Count <= 25)
                        {
                            xRotate = 55;
                        }
                        else if (xdata.Count > 25)
                        {
                            xRotate = 65;
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
                dataZoomArea = barChart.MobileDisplayScale,
                xAxis = xdata,
                series
            });
        }


        [HttpPost]
        public ActionResult AntibioticDrugFastDataShanghai(long id)
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
                                barWidth = dataItems.Count() <= 4 && Antibiotics.Count() <= 9 ? 30 : 0,
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
                legend = legends,
                series
            });
        }

        public string Picturesharing(string dataURL = "", string imgName = "", string Name = "")
        {
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(imgName));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            //string decode = Convert.ToBase64String(bytes);
            string path = "";
            String base64 = dataURL.Substring(dataURL.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除
            System.Drawing.Bitmap bitmap = null;//定义一个Bitmap对象，接收转换完成的图片
            string dummyData = base64.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+").Replace('-', '+').Replace('_', '/');
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            try//会有异常抛出，try，catch一下
            {

                byte[] arr = Convert.FromBase64String(dummyData);//将纯净资源Base64转换成等效的8位无符号整形数组

                System.IO.MemoryStream ms = new System.IO.MemoryStream(arr);//转换成无法调整大小的MemoryStream对象
                bitmap = new System.Drawing.Bitmap(ms);//将MemoryStream对象转换成Bitmap对象

                string filename = Name + sb.ToString().Substring(sb.Length - 8).ToUpper() + ".jpg";//所要保存的相对路径及名字
                string url = HttpRuntime.AppDomainAppPath.ToString();
                string tmpRootDir = "/Content/QRcode/Images";
                string imagesurl2 = System.Web.HttpContext.Current.Server.MapPath(tmpRootDir + "/" + filename); //转换成绝对路径 
                path = tmpRootDir + "/" + filename;
                if (System.IO.File.Exists(imagesurl2))
                {
                    return path;
                }
                else
                {
                    bitmap.Save(imagesurl2, System.Drawing.Imaging.ImageFormat.Jpeg);//保存到服务器路径
                }

                //if (!string.IsNullOrEmpty(filename))
                //{
                //    try
                //    {
                //        System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(tmpRootDir + "/" + filename));
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}
                //bitmap.Save(filePath + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //bitmap.Save(filePath + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
                //bitmap.Save(filePath + ".png", System.Drawing.Imaging.ImageFormat.Png);
                ms.Close();//关闭当前流，并释放所有与之关联的资源
                bitmap.Dispose();
            }
            catch (Exception e)
            {
                string massage = e.Message;
            }
            return path;
        }

        /// <summary>
        /// 加载细菌或药物数据
        /// </summary>
        /// <param name="germValue">细菌名称</param>
        /// <param name="antibioticValue">药物名称</param>
        /// <returns></returns>
        public JsonResult GetAntibioticDrugFastData(string germValue, string antibioticValue)
        {
            var data = DataAntibioticDrugFastService.Query(antibioticValue, germValue).OrderBy(m => m.Value).ToList();
            if (data == null || !data.Any())
                return Json(new { Status = false, Message = "未获取到数据" });

            string text = "";
            string value = "";

            if (!string.IsNullOrWhiteSpace(antibioticValue))
            {
                //选择了抗菌药物，X轴则是 细菌分类
                foreach (var item in data)
                {
                    text += string.Format("{0},", item.GermName);
                    value += item.Value + ",";
                }
            }
            else
            {
                //其他的，X轴则是 抗菌药品
                foreach (var item in data)
                {
                    text += string.Format("{0},", item.AntibioticName);
                    value += item.Value + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
                text = text.Trim(',');

            if (!string.IsNullOrWhiteSpace(value))
                value = value.Trim(',');

            return Json(new { Status = true, Text = text, Value = value });
        }
        #endregion

        #region 趋势图
        /// <summary>
        /// 趋势图
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("germ/year.cshtml")]
        public ViewResult GermWithYear()
        {
            List<BarChartDTO> barCharts = _dataSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.TrendChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _trendChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new BarChartDataItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Default = m.Default
                }).ToList()
            }).ToList();

            return View(barCharts);
        }

        /// <summary>
        /// 加载趋势图数据
        /// </summary>
        /// <param name="id">报表id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GermWithYearData(long id)
        {
            Chart_TrendChart trendChart = _trendChartService.QueryEntity(id);

            List<string> legends = new List<string>();
            List<string> xdata = new List<string>();
            List<dynamic> series = new List<dynamic>();
            List<string> colors = new List<string>();

            IQueryable<Chart_TrendChartWithItemData> dataItems = _trendChartWithItemDataService.Query(r => r.Mark > 0 && r.TrendChartId == trendChart.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).AsQueryable();

            int xRotate = 0;
            switch (trendChart.DataItemType)
            {
                case ChartDataItemType.SingleData:
                    {
                        List<dynamic> series_data = new List<dynamic>();
                        foreach (Chart_TrendChartWithItemData item in dataItems)
                        {
                            if (!legends.Contains(item.Name))
                            {
                                legends.Add(item.Name);
                            }

                            xdata.Add(item.Name);
                            series_data.Add(new { name = item.Name, value = item.Display ? item.Value : "-" });
                        }

                        colors.Add(string.IsNullOrWhiteSpace(dataItems.FirstOrDefault().ChartColor) ? "#015baa" : dataItems.FirstOrDefault().ChartColor);
                        series.Add(new
                        {
                            name = "数值",
                            type = dataItems.FirstOrDefault().ChartType == ChartTypeEnum.Line ? "line" : "bar",
                            barWidth = dataItems.Count() < 10 ? 15 : 0,
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

                    }
                    break;
                case ChartDataItemType.MultipleData:
                    {
                        List<BarChartDataItemWithAntibioticModel> Antibiotics = trendChart.Antibiotics.DeserializeObject<List<BarChartDataItemWithAntibioticModel>>() ?? new List<BarChartDataItemWithAntibioticModel>();
                        Antibiotics.ForEach(item =>
                        {
                            xdata.Add(item.Name);
                        });

                        foreach (Chart_TrendChartWithItemData item in dataItems)
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

                            List<BarChartDataItemWithAntibioticModel> values = item.Value.DeserializeObject<List<BarChartDataItemWithAntibioticModel>>();

                            foreach (BarChartDataItemWithAntibioticModel antibiotic in Antibiotics)
                            {

                                var _value = values.Where(r => r.Guid == antibiotic.Guid).FirstOrDefault().Value;
                                series_data.Add(new { name = antibiotic.Name, value = item.Display ? _value : "-" });
                            }

                            series.Add(new
                            {
                                name = item.Name,
                                type = item.ChartType == ChartTypeEnum.Line ? "line" : "bar",
                                stack = "",
                                barWidth = dataItems.Count() <= 4 && Antibiotics.Count() <= 5 ? 15 : 0,
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
                    }
                    break;
                default:
                    break;
            }



            return Json(new
            {
                title = trendChart.Title,
                subtitle = trendChart.SubTitle,
                color = colors,
                legend = legends,
                rotate = xRotate,
                dataZoomArea = trendChart.MobileDisplayScale,
                xAxis = xdata,
                series
            });
        }
        #endregion

        #region 主动监测
        /// <summary>
        /// 主动监测
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Monitor()
        {
            return View();
        }
        #endregion

        #region 图表
        /// <summary>
        /// 柱状图/趋势图/热图 合并图表
        /// </summary>
        /// <returns></returns>
        public ViewResult ECharts()
        {
            ViewBag.GermList = DataAntibioticDrugFastService.QueryGerm();
            ViewBag.AntibioticList = DataAntibioticDrugFastService.QueryAntibiotic();

            return View();
        }
        #endregion
    }
}