using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Chart;
using ManageSystem.Services.Datas;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Teams;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Models.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 数据报表
    /// </summary>
    public class DataController : WebBaseController
    {
        #region 业务声明
        private readonly IDataAntibioticDrugFastService _dataAntibioticDrugFastService;
        private readonly IDataGermYearService _dataGermYearService;
        private readonly IAreaService _areaService;
        private readonly IMemberService _memberService;

        private readonly IDataSegmentService _dataSegmentService;
        private readonly IBarChartService _barChartService;
        private readonly IBarChartWithItemDataService _barChartWithItemDataService;
        private readonly ITrendChartService _trendChartService;
        private readonly ITrendChartWithItemDataService _trendChartWithItemDataService;

        private readonly IHospitalService _hospitalService;
        private readonly ITeamService _teamService;
        private readonly ISatelliteService _satelliteService;
        #endregion

        #region 构造器
        public DataController(
            IDataAntibioticDrugFastService dataAntibioticDrugFastService,
            IDataGermYearService dataGermYearService,
            IAreaService areaService,
            IMemberService memberService,
            IDataSegmentService dataSegmentService,
            IBarChartService barChartService,
            IBarChartWithItemDataService barChartWithItemDataService,
            ITrendChartService trendChartService,
            ITrendChartWithItemDataService trendChartWithItemDataService,
            ITeamService teamService,
            IHospitalService hospitalService,
            ISatelliteService satelliteService
        )
        {
            this._dataAntibioticDrugFastService = dataAntibioticDrugFastService;
            this._dataGermYearService = dataGermYearService;
            this._areaService = areaService;
            this._memberService = memberService;

            this._dataSegmentService = dataSegmentService;
            this._barChartService = barChartService;
            this._barChartWithItemDataService = barChartWithItemDataService;
            this._trendChartService = trendChartService;
            this._trendChartWithItemDataService = trendChartWithItemDataService;

            this._teamService = teamService;
            this._hospitalService = hospitalService;

            this._satelliteService = satelliteService;
    }
        #endregion

        #region 柱状图

        /// <summary>
        /// 抗生素耐药性  柱状图
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult AntibioticDrugFast()
        {
            try
            {
                // Check for null services first
                if (_dataSegmentService == null)
                    throw new InvalidOperationException("_dataSegmentService is null - dependency injection failed");
                if (_barChartService == null)
                    throw new InvalidOperationException("_barChartService is null - dependency injection failed");
                
                // Test basic database connectivity first
                try
                {
                    var testCount = _dataSegmentService.Count();
                    ViewBag.DebugInfo = $"Database connection test successful. DataSegment count: {testCount}";
                }
                catch (Exception dbEx)
                {
                    ViewBag.ErrorMessage = $"Database connection failed: {dbEx.Message}";
                    if (dbEx.InnerException != null)
                        ViewBag.ErrorMessage += $" Inner: {dbEx.InnerException.Message}";
                    return View(new List<BarChartDTO>());
                }
                
                // Fix null reference issue by adding null checking
                var dataSegments = _dataSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.BarChart);
                if (dataSegments == null)
                {
                    // Return empty list if service returns null
                    List<BarChartDTO> emptyBarCharts = new List<BarChartDTO>();
                    return View(emptyBarCharts);
                }

                List<BarChartDTO> barCharts = dataSegments.OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    DataItems = _barChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0 && r.SatelliteId == 0)
                                ?.OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new BarChartDataItemDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Default = m.Default
                    }).ToList() ?? new List<BarChartDataItemDTO>()
                }).ToList();

                return View(barCharts);
            }
            catch (Exception ex)
            {
                // Log the error and return a safe view
                ViewBag.ErrorMessage = "Error loading data: " + ex.Message;
                return View(new List<BarChartDTO>());
            }
        }

        /// <summary>
        /// 抗生素耐药性  柱状图
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult AntibioticDrugFastMap(string city)
        {
            Satellite satellite = _satelliteService.Query().Single(x => x.RealmName == city);
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

        [HttpPost]
        [CheckRole(false)]
        public ActionResult AntibioticDrugFastData(long id)
        {
            Chart_BarChart barChart = _barChartService.QueryEntity(id);

            List<string> xdata = new List<string>();
            List<dynamic> series = new List<dynamic>();
            List<string> colors = new List<string>();

            IQueryable<Chart_BarChartWithItemData> dataItems = _barChartWithItemDataService.Query(r => r.Mark > 0 && r.BarChartId == barChart.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).AsQueryable();
            int xRotate = 0;
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

                        if (xdata.Count > 8 && xdata.Count <= 12)
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
                                series_data.Add(new { name = antibiotic.Name, value = item.Display ? _value : "-" });
                            }

                            series.Add(new
                            {
                                name = item.Name,
                                type = item.ChartType == ChartTypeEnum.Line ? "line" : "bar",
                                stack = "",
                                barWidth = 30,
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
                series
            });
        }

        /// <summary>
        /// 抗生素耐药性  柱状图   获取数据
        /// </summary>
        /// <param name="germValue">细菌的名称</param>
        /// <param name="antibioticValue">抗生素的名称</param>
        /// <returns></returns>
        [CheckRole(false)]
        public ContentResult GetAntibioticDrugFastData(String germValue, String antibioticValue)
        {
            var data = this._dataAntibioticDrugFastService.Query(antibioticValue, germValue).OrderBy(m => m.Value).ToList();
            if (data == null || !data.Any())
                return this.Content(new { Status = false, Message = "未获取到数据" }.SerializeObject());

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

            return this.Content(new { Status = true, Text = text, Value = value }.SerializeObject());
        }

        #endregion

        #region 趋势图

        /// <summary>
        /// 抗生素耐药性  柱状图
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult GermYear()
        {
            List<BarChartDTO> barCharts = _dataSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.TrendChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _trendChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0 &&string.IsNullOrEmpty(r.SatelliteId)).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new BarChartDataItemDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Default = m.Default
                }).ToList()
            }).ToList();

            return View(barCharts);
        }

        /// <summary>
        /// 抗生素耐药性  柱状图(卫星网)
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult GermYearMap(string city)
        {
            
            string loginId = this._satelliteService.Query().First(x=>x.RealmName == city).Id.ToString();
            List<BarChartDTO> barCharts = _dataSegmentService.Query(r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.TrendChart).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new BarChartDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _trendChartService.Query(r => r.DataSegmentId == item.Id && r.Display && r.Mark > 0&&r.SatelliteId == loginId).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(m => new BarChartDataItemDTO
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
        [CheckRole(false)]
        public JsonResult GermYearData(long id)
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
                            barWidth = dataItems.Count() < 10 ? 30 : 0,
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
                                barWidth = dataItems.Count() <= 4 && Antibiotics.Count() <= 5 ? 30 : 0,
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
                xAxis = xdata,
                series
            });
        }

        /// <summary>
        /// 细菌年限  柱状图   获取数据
        /// </summary>
        /// <param name="germValue">细菌的名称</param>
        /// <param name="yearValue">年份</param>
        /// <returns></returns>
        public ContentResult GetGermYearData(String stage, int yearValue = 0)
        {
            if (string.IsNullOrWhiteSpace(stage))
                return this.Content(new { Status = false, Message = "请选择数据段" }.SerializeObject());

            var data = this._dataGermYearService.QueryByStage(stage);
            if (data == null || !data.Any())
                return this.Content(new { Status = false, Message = "未获取到数据" }.SerializeObject());


            return this.Content("");

        }

        /// <summary>
        /// 细菌年限  柱状图   获取数据
        /// </summary>
        /// <param name="germValue">细菌的名称</param>
        /// <param name="yearValue">年份</param>
        /// <returns></returns>
        public ContentResult GetGermYearData2(String germValue, int yearValue = 0)
        {
            var data = this._dataGermYearService.Query(yearValue, germValue);
            if (data == null || !data.Any())
                return this.Content(new { Status = false, Message = "未获取到数据" }.SerializeObject());

            string text = "";
            string value = "";

            if (yearValue > 0)
            {
                //选择了年份，X轴则是 年份
                foreach (var item in data)
                {
                    text += string.Format("{0},", item.GermName);
                    value += item.Value + ",";
                }
            }
            else
            {
                //其他的，X轴则是 年限
                foreach (var item in data)
                {
                    text += string.Format("{0},", item.Year);
                    value += item.Value + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(text))
                text = text.Trim(',');

            if (!string.IsNullOrWhiteSpace(value))
                value = value.Trim(',');

            return this.Content(new { Status = true, Text = text, Value = value }.SerializeObject());
        }

        #endregion

        #region 地图

        /// <summary>
        /// 地图
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Map()
        {
            return this.View();
        }

        #endregion


        /// <summary>
        /// 移动端测试
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Mobile()
        {

            return View();
        }
        #region 主动检测

        /// <summary>
        /// 主动检测
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ViewResult Monitor()
        {
            return View();
        }
        #endregion

        [CheckRole(false)]
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

                string filename = Name.Replace("（%）", "") + sb.ToString().Substring(sb.Length - 8).ToUpper() + ".jpg";//所要保存的相对路径及名字
                //string filename = DateTime.Now.ToString("yyyyMMddHHmmssss")+".jpg";
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


        [HttpGet]
        [CheckRole(false)]
        public ContentResult Guanyuchinet()
        {
            var result = _teamService.TeamList();
            List<MapProvinceName> Titlelist = new List<MapProvinceName>();
            List<MapHospital> MapDTO = new List<MapHospital>();
            List<MapList> Map = new List<MapList>();
            List<string> ProvinceName = new List<string>();

            MapList Mapmodel = new MapList();
            for (int i = 0; i < result.Count; i++)
            {
                if (!ProvinceName.Contains(result[i].ProvinceName))
                {
                    ProvinceName.Add(result[i].ProvinceName);
                }
            }

            for (int i = 0; i < ProvinceName.Count; i++)
            {
                MapProvinceName Province = new MapProvinceName();
                MapHospital Hospital = new MapHospital();
                Hospital.name = ProvinceName[i];
                Province.name = ProvinceName[i];
                for (int n = 0; n < result.Count; n++)
                {
                    if (result[n].ProvinceName == ProvinceName[i])
                    {

                        if (Hospital.value == null)
                        {
                            Hospital.value = result[n].Title;
                            Province.value = 1;
                        }
                        else
                        {
                            Hospital.value += "<br/>" + result[n].Title;
                            Province.value++;
                        }

                    }
                }
                MapDTO.Add(Hospital);
                Titlelist.Add(Province);
            }
            Mapmodel.ProvinceName = Titlelist;
            Mapmodel.Hospital = MapDTO;
            Map.Add(Mapmodel);
            return this.Content(Map.SerializeObject());

        }
    }
}