using ManageSystem.Services.MicdataDistribution;
using ManageSystem.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using ManageSystem.Web.App_Start;
using ManageSystem.Core.Domain.MicdataDistribution;
using System.Collections;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using ManageSystem.Services.Teams;
using ManageSystem.Services.Members;

namespace ManageSystem.Web.Areas.DataDistribution.Controllers
{
    //[CheckRole(false)]
    public class MicDistributionController : WebBaseController
    {
        public IddYearService ddYearService;
        public IddGermService ddGermService;
        public IddAntibioticService ddAntibioticService;
        public IddDocumentItemService ddDocumentItemService;
        private readonly ITeamService _teamService;
        private readonly IMemberService MemberService;

        public MicDistributionController(IddYearService _ddYearService, IddGermService _ddGermService, IddAntibioticService _ddAntibioticService, IddDocumentItemService _ddDocumentItemService, ITeamService teamService, IMemberService _MemberService)
        {
            ddYearService = _ddYearService;
            ddGermService = _ddGermService;
            ddAntibioticService = _ddAntibioticService;
            ddDocumentItemService = _ddDocumentItemService;
            _teamService = teamService;
            MemberService = _MemberService;
        }
        // GET: DataDistribution/MicDistribution
        //[CheckRole(false)]
        public ActionResult Index()
        {
            //获取当前用户的登录名
            var member = base.LoginUserinfo;
            var userinfo = this.MemberService.QueryEntity(member.Id);
            if (userinfo.MICjurisdiction == 0)
            {
                //跳转到MIC申请页面
                return Redirect("/MICapply/Index");
            }
            else
            {
                return View();
            }
            //return View();
        }


        public ActionResult Index1()
        {
            return View();
        }
        /// <summary>
        /// 查询年份
        /// </summary>
        /// <returns></returns>
        public string GetddYear()
        {
            string data =JsonConvert.SerializeObject(ddYearService.GetDdYears());
            return data;

        }
        /// <summary>
        /// 查询细菌
        /// </summary>
        /// <returns></returns>
        public string GetGerms()
        {
            string data = JsonConvert.SerializeObject(ddGermService.GetGerms());
            return data;

        }
        /// <summary>
        /// 查询抗生素
        /// </summary>
        /// <returns></returns>
        public string GetDdAntibiotics()
        {
            string data = JsonConvert.SerializeObject(ddAntibioticService.GetDdAntibiotics());
            return data;

        }

        /// <summary>
        /// 查询抗生素
        /// </summary>
        /// <returns></returns>
        public string GetddDocumentItem(string[] gremcode, string yeraid, string antibiotics)
        {
            if (gremcode==null || yeraid=="" || antibiotics == "")
            {
                return"暂无数据";
            }
            List<ddDocumentItem> DocumentItemlist = ddDocumentItemService.GetDdDocuments(gremcode, yeraid, antibiotics);
            Dictionary<String, List<ddDocumentItem>> contractItemDic = new Dictionary<String, List<ddDocumentItem>>(); //用户封装返回的多个list
            List<ddDocumentItem> DocumentItemlists = new List<ddDocumentItem>();
            //根据传来的细菌数据量拆分list集合
            int n = 0;
            for (int i = 0; i < gremcode.Length; i++)
            {                
                for (int j = 0; j < DocumentItemlist.Count; j++)
                {
                    if (DocumentItemlist[j].organism.Contains(gremcode[i].ToString()))
                    {
                        //for循环依次放入每个list中
                        DocumentItemlists.Add(DocumentItemlist[j]);
                    }                   
                }
                //先将对象放入list,以防止最后一个没有放入                                                                               
                //如果l+1 除以 要分的份数 为整除,或者是最后一份,为结束循环.那就算作一份list,
                contractItemDic.Add(gremcode[i].ToString(), DocumentItemlists); //将这一份放入Map中.
                DocumentItemlists = new List<ddDocumentItem>();                            
            }           
            List<ArrayList> keyValuePairs = new List<ArrayList>();                  
            ArrayList temporary = new ArrayList();
            string head = ddDocumentItemService.GetCategoryValues();
            string[] heads = head.Split(',');
            Double value = 0;
            int min = 0;
            int max = 0;           
            for (int i = 0; i < contractItemDic.Values.Count; i++)
            {
                string[] medicine = { "抗菌药物","株数" };
                string[] num = { };
                List<string> nu = new List<string>(num);
                string[] head1 = heads;
                List<string> temp = new List<string>(head1);
                List<ddDocumentItem> DocumentItem = contractItemDic.Values.ElementAt(i);
                for (int j = 0; j < DocumentItem.Count; j++)
                {
                    string[] anti = DocumentItem[j].antibiotics.Split(',');
                    string[] antival = DocumentItem[j].antibiotics_value.Split(',');
                    for (int s = 0; s < anti.Length-1; s++)
                    {
                        string biotica = anti[s].ToString().Replace("_NE", "").Replace("_NM", "").Replace("_ND10", "").Replace("_ND30", "").Replace("_ND20", "").Replace("_ND40", "");
                        if (biotica.ToLower()==antibiotics.ToLower())
                        {
                            if (antival[s].Contains("<"))
                            {
                                value = Double.Parse(antival[s].Replace("<=.", "0.").Replace("<=", "").Replace("<.", "0.").Replace("<", ""));
                                for (int u = 0; u < heads.Length; u++)
                                {
                                    Double resul1 = Double.Parse(heads[u]);
                                    Double resul2 = Double.Parse(value.ToString());
                                    if (resul1 == resul2)
                                    {
                                        if (!temp.Contains(antival[s].ToString()))
                                        {
                                            temp.Insert(u, antival[s].ToString());
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else if (antival[s].Contains(">"))
                            {
                                //string aa = anti[s].ToString().ToLower();
                                //string ss = antival[s].ToString();

                                value = Double.Parse(antival[s].Replace(">=.", "0.").Replace(">=", "").Replace(">.", "0.").Replace(">", ""));
                                for (int u = 0; u < heads.Length; u++)
                                {
                                    Double resul1 = Double.Parse(heads[u]);
                                    Double resul2 = Double.Parse(value.ToString());
                                    if (resul1 == resul2)
                                    {
                                        if (!temp.Contains(antival[s].ToString()))
                                        {
                                            if (temp.Count == heads.Length)
                                            {
                                                temp.Insert(u + 1, antival[s].ToString());
                                            }
                                            else if (temp.Count> heads.Length)
                                            {
                                                temp.Insert(u + 2, antival[s].ToString());
                                            }


                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }                           
                        }                       
                       
                    }                   
                }
                head1 = temp.ToArray();
                max = head1.Length;
                for (int l = 0; l < head1.Length; l++)
                {
                    if (head1[l].Contains("<"))
                    {
                        min = l;
                    }
                    else if (head1[l].Contains(">"))
                    {
                        max = l;
                    }                    
                }               
                head1 = head1.Skip(min).Take(max - min+1).ToArray();
                int[] values = new int[head1.Length];              
                for (int j = 0; j < DocumentItem.Count; j++)
                {
                    string[] anti = DocumentItem[j].antibiotics.Split(',');
                    string[] antival = DocumentItem[j].antibiotics_value.Split(',');
                    for (int k = 0; k < head1.Length; k++)
                    {
                        for (int s = 0; s < anti.Length - 1; s++)
                        {
                            string biotica = anti[s].ToString().Replace("_NE", "").Replace("_NM", "").Replace("_ND10", "").Replace("_ND30", "").Replace("_ND20", "").Replace("_ND40", "");
                            if (biotica.ToLower().Equals(antibiotics.ToLower()))
                            {                            
                                if (antival[s] == head1[k])
                                {
                                    values[k]++;
                                }
                            }
                        }
                    }
                }
                //List<int> val = new List<int>(values);
                //List<string> temps = new List<string>(heards1);
                //temps.Insert(0, "株数");
                //val.Insert(0, DocumentItem.Count);
                //values = val.ToArray();
                //heards1 = temps.ToArray();
                nu.Add(ddAntibioticService.QueryEntityCode(antibiotics).title);
                int count = 0;
                for (int u = 0; u < values.Length; u++)
                {
                    count += values[u];
                }
                //nu.Add(DocumentItem.Count.ToString());
                nu.Add(count.ToString());
                num = nu.ToArray();
                temporary.Add(ddGermService.QueryEntityCode(contractItemDic.Keys.ElementAt(i)).title);
                temporary.Add(medicine);
                for (int p = 0; p < head1.Length; p++)
                {
                    if (head1[p].Contains("<"))
                    {
                        head1[p] = head1[p].Replace("<=.", "<=0.").Replace("<.", "<0.");
                    } 
                    else if(head1[p].Contains(">"))
                    {
                        head1[p] = head1[p].Replace(">=.", ">=0.").Replace(">.", ">0.");
                    }
                }
                List<string> head1list = head1.ToList();
                List<int> valueslist = values.ToList();
                for (int y = 0; y < head1.Length; y++)
                {
                    if (head1[y].Contains("<="))
                    {
                        string head1s = head1[y].Replace("<=", "");
                        if (head1[y + 1] == head1s)
                        {
                            if (values[y + 1] == 0)
                            {                               
                                head1list.RemoveAt(y + 1);
                                valueslist.RemoveAt(y + 1);
                            }
                            else
                            {
                                double newhead1s = double.Parse(head1s) / 2;
                                head1list[y] = "<=" + newhead1s.ToString();
                            }
                        }
                    }
                    else if(head1[y].Contains(">="))
                    {
                        string head1s = head1[y].Replace(">=", "");
                        if (y > 0)
                        {
                            if (head1[y - 1] == head1s)
                            {
                                if (values[y - 1] == 0)
                                {
                                    if (head1list.Count == head1.Length)
                                    {
                                        head1list.RemoveAt(y - 1);
                                        valueslist.RemoveAt(y - 1);
                                    } 
                                    else if(head1list.Count < head1.Length)
                                    {
                                        head1list.RemoveAt(y - 2);
                                        valueslist.RemoveAt(y - 2);
                                    }
                                       
                                }
                                else
                                {
                                    double newhead1s = double.Parse(head1s) * 2;
                                    head1list[y] = ">=" + newhead1s.ToString();
                                }
                            }
                        }
                    }
                    //if (!nhead1.Contains(head1s))
                    //{
                    //    nhead1.Add(head1s);
                    //}
                }
                temporary.Add(head1list.ToArray());
                temporary.Add(num);
                temporary.Add(valueslist.ToArray());                              
                keyValuePairs.Add(temporary);
                temporary = new ArrayList();
            }
            return JsonConvert.SerializeObject(keyValuePairs.ToList());

        }
     
        //[HttpPost,CheckRole(false)]
        public ActionResult Exportexcel(string head, string value, string name)
        {
            ExcelPackage ep = new ExcelPackage();          
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add(name);
            ws.Cells[1, 1].Value =name+ "MIC分布(mg/L)";
            string[] heads= head.Split(',');
            ExcelRange excelRange = ws.Cells[1, 1, 1, heads.Length];
            excelRange.Merge = true;
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
           
            for (int i = 0; i < heads.Length; i++)
            {              
               ws.Cells[2, i+1].Value = heads[i].ToString();
               ws.Cells[2, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }           
            string[] values = value.Split(',');
            int index = 3;
            for (int j = 0; j < values.Length; j++)
            {
                ws.Cells[index, j+1].Value = values[j].ToString();
                ws.Cells[index, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                if (j == values.Length - 1)
                {
                    index++;
                }
            }           
            string path= ""+name+ "MIC分布" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            Response.AddHeader("content-disposition", "attachment;filename=" + path);
            Response.ContentType = "application/vnd.open";
            ep.SaveAs(Response.OutputStream);
            Response.Flush();
            Response.End();
            
            return View();
        }
        /// <summary>
        /// 查询默认显示抗生素
        /// </summary>
        /// <returns></returns>
        public string GetdefaultddAntibiotic()
        {
            string data = JsonConvert.SerializeObject(ddAntibioticService.GetdefaultddAntibiotic());
            return data;
        }
        /// <summary>
        /// 查询默认显示细菌
        /// </summary>
        /// <returns></returns>
        public string GetdefaultGerms()
        {
            string data = JsonConvert.SerializeObject(ddGermService.GetdefaultGerms());
            return data;
        }
        /// <summary>
        /// 查询菌株来源
        /// </summary>
        /// <returns></returns>
        public string GetTeamlist()
        {
            string json = JsonConvert.SerializeObject(_teamService.GetTeams());
            return json;
        }
    }
}