using Dapper;
using ICSharpCode.SharpZipLib.Zip;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using ManageSystem.Core.Domain.Members;
using OfficeOpenXml;
using ManageSystem.Services.Medicine;
using ManageSystem.Core.Domain.Survey;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ManageSystem.Services.Survey
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalData 
    /// </summary>
    public partial class SurveySurveyService : BaseService<Survey_Survey>, ISurveySurveyService
    {
        private readonly ISurveySubjectService surveySubjectService;
        private readonly ISurveyRecordService surveyRecordService;

        public SurveySurveyService(IRepository<Survey_Survey> repository,
                ISurveySubjectService _surveySubjectService,
                ISurveyRecordService _surveyRecordService
            ) : base(repository)
        {
            this.surveySubjectService = _surveySubjectService;
            this.surveyRecordService = _surveyRecordService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">调查标题</param>
        /// <param name="hospitalId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<Survey_Survey> QueryPage(string name, long hospitalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }

            //if (areaId > 0)
            //    query = query.Where(m => m.AreaId == areaId);

            //if (hospitalId > 0)
            //    query = query.Where(m => m.HospitalId == hospitalId);

            //if (year > 0)
            //    query = query.Where(m => m.Year == year);

            //if (quarter > 0)
            //    query = query.Where(m => m.Quarter == quarter);

            //if (projectType > 0)
            //{
            //    query = query.Where(r => r.ProjectType == projectType);
            //}
            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<Survey_Survey>(query, pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 会员中心，CR复敏信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public IQueryable<Survey_Survey> Query(long memberId)
        {
            var data = base._repository.Table.Where(m => m.Mark > 0);

            return data.OrderByDescending(m => m.InsertTime);
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ep">excel导出组建</param>
        /// <param name="surveyId">问卷调查ID</param>
        /// <param name="content29">搜索医院</param>
        /// <param name="content30">搜索名称</param>
        /// <param name="content31">搜索手机号</param>
        /// <returns></returns>
        public string Export(ExcelPackage ep, long surveyId, string content29, string content30, string content31)
        {
            var list = this.surveyRecordService.Query().Where(m => m.Mark > 0).ToList();

            if (surveyId > 0)
                list = list.Where(m => m.SurveyId == surveyId).ToList();

            if (!string.IsNullOrEmpty(content29))
            {
                list = list.Where(m => m.Content29.Contains(content29)).ToList();
            }
            if (!string.IsNullOrEmpty(content30))
            {
                list = list.Where(m => m.Content30.Contains(content30)).ToList();
            }
            if (!string.IsNullOrEmpty(content31))
            {
                list = list.Where(m => m.Content31.Contains(content31)).ToList();
            }

            //查题目
            var listSubject = this.surveySubjectService.Query().Where(m => m.Mark > 0).ToList();

            this.ExportProject(list, listSubject, ep);

            return "问卷调查导出数据_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
        }


        /// <summary>
        /// 导出项目主表的医院相关信息
        /// </summary>
        /// <param name="list">本次导出的项目数据</param>
        /// <param name="ep"></param>
        private void ExportProject(List<Survey_Record> list, List<Survey_Subject> listSubject, ExcelPackage ep)
        {
            //写入表头
            ExcelWorksheet ws = ep.Workbook.Worksheets.Add("问卷调查信息");

            //首行标题赋值
            int cell_index = 1;
            ws.Cells[1, cell_index++].Value = "时间";

            for (int i = 0; i < listSubject.Count; i++)
            {
                switch (i)
                {
                    case 2:
                        // Content3
                        ws.Cells[1, 4].Value = listSubject[i].Name;
                        ws.Cells[1, 4, 1, 9].Merge = true;
                        break;
                    case 3:
                        // Content4
                        ws.Cells[1, 10].Value = listSubject[i].Name;
                        ws.Cells[1, 10, 1, 15].Merge = true;
                        cell_index = 16;
                        break;
                    case 6:
                        // Content7
                        ws.Cells[1, 18].Value = listSubject[i].Name;
                        ws.Cells[1, 18, 1, 23].Merge = true;
                        break;
                    case 7:
                        // Content8
                        ws.Cells[1, 24].Value = listSubject[i].Name;
                        ws.Cells[1, 24, 1, 29].Merge = true;
                        cell_index = 30;
                        break;
                    case 10:
                        // Content11
                        ws.Cells[1, 32].Value = listSubject[i].Name;
                        ws.Cells[1, 32, 1, 37].Merge = true;
                        break;
                    case 11:
                        // Content12
                        ws.Cells[1, 38].Value = listSubject[i].Name;
                        ws.Cells[1, 38, 1, 43].Merge = true;
                        cell_index = 44;
                        break;
                    case 14:
                        // Content15
                        ws.Cells[1, 46].Value = listSubject[i].Name;
                        ws.Cells[1, 46, 1, 51].Merge = true;
                        break;
                    case 15:
                        // Content16
                        ws.Cells[1, 52].Value = listSubject[i].Name;
                        ws.Cells[1, 52, 1, 57].Merge = true;
                        cell_index = 58;
                        break;
                    case 18:
                        // Content19
                        ws.Cells[1, 60].Value = listSubject[i].Name;
                        ws.Cells[1, 60, 1, 65].Merge = true;
                        break;
                    case 19:
                        // Content20
                        ws.Cells[1, 66].Value = listSubject[i].Name;
                        ws.Cells[1, 66, 1, 71].Merge = true;
                        cell_index = 72;
                        break;
                    case 22:
                        // Content23
                        ws.Cells[1, 74].Value = listSubject[i].Name;
                        ws.Cells[1, 74, 1, 79].Merge = true;
                        break;
                    case 23:
                        // Content24
                        ws.Cells[1, 80].Value = listSubject[i].Name;
                        ws.Cells[1, 80, 1, 85].Merge = true;
                        cell_index = 86;
                        break;
                    case 25:
                        // Content26
                        ws.Cells[1, 87].Value = listSubject[i].Name;
                        ws.Cells[1, 88].Value = "采用方法";
                        cell_index = 89;
                        break;
                    default:
                        ws.Cells[1, cell_index++].Value = listSubject[i].Name;
                        break;
                }
            }

            ws.Row(1).Style.Font.Size = 14;
            //内容赋值
            int index = 2;
            foreach (var item in list)
            {
                ws.Cells[index, 1].Value = item.InsertTime.GetNormalString("L");
                ws.Cells[index, 2].Value = string.IsNullOrWhiteSpace(item.Content1) ? "" : item.Content1;
                ws.Cells[index, 3].Value = string.IsNullOrWhiteSpace(item.Content2) ? "" : item.Content2;

                #region Content3、Content4
                string strContent3 = string.IsNullOrWhiteSpace(item.Content3) ? "" : item.Content3;
                string[] arrContent3 = strContent3.Split(',');
                Tuple<string, string> tupleContent3_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent3_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent3_item3 = new Tuple<string, string>(null, null);
                switch (arrContent3.Length)
                {
                    case 1:
                        {
                            tupleContent3_item1 = RegexSplit(arrContent3[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent3_item1 = RegexSplit(arrContent3[0], @"([^:]+)$");
                            tupleContent3_item2 = RegexSplit(arrContent3[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent3_item1 = RegexSplit(arrContent3[0], @"([^:]+)$");
                            tupleContent3_item2 = RegexSplit(arrContent3[1], @"([^:]+)$");
                            tupleContent3_item3 = RegexSplit(arrContent3[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 4].Value = tupleContent3_item1.Item1;
                ws.Cells[index, 5].Value = tupleContent3_item1.Item2;
                ws.Cells[index, 6].Value = tupleContent3_item2.Item1;
                ws.Cells[index, 7].Value = tupleContent3_item2.Item2;
                ws.Cells[index, 8].Value = tupleContent3_item3.Item1;
                ws.Cells[index, 9].Value = tupleContent3_item3.Item2;

                string strContent4 = string.IsNullOrWhiteSpace(item.Content4) ? "" : item.Content4;
                string[] arrContent4 = strContent4.Split(',');
                Tuple<string, string> tupleContent4_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent4_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent4_item3 = new Tuple<string, string>(null, null);
                switch (arrContent4.Length)
                {
                    case 1:
                        {
                            tupleContent4_item1 = RegexSplit(arrContent4[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent4_item1 = RegexSplit(arrContent4[0], @"([^:]+)$");
                            tupleContent4_item2 = RegexSplit(arrContent4[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent4_item1 = RegexSplit(arrContent4[0], @"([^:]+)$");
                            tupleContent4_item2 = RegexSplit(arrContent4[1], @"([^:]+)$");
                            tupleContent4_item3 = RegexSplit(arrContent4[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 10].Value = tupleContent4_item1.Item1;
                ws.Cells[index, 11].Value = tupleContent4_item1.Item2;
                ws.Cells[index, 12].Value = tupleContent4_item2.Item1;
                ws.Cells[index, 13].Value = tupleContent4_item2.Item2;
                ws.Cells[index, 14].Value = tupleContent4_item3.Item1;
                ws.Cells[index, 15].Value = tupleContent4_item3.Item2;
                #endregion

                ws.Cells[index, 16].Value = string.IsNullOrWhiteSpace(item.Content5) ? "" : item.Content5;
                ws.Cells[index, 17].Value = string.IsNullOrWhiteSpace(item.Content6) ? "" : item.Content6;

                #region Content7、Content8
                string strContent7 = string.IsNullOrWhiteSpace(item.Content7) ? "" : item.Content7;
                string[] arrContent7 = strContent7.Split(',');
                Tuple<string, string> tupleContent7_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent7_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent7_item3 = new Tuple<string, string>(null, null);
                switch (arrContent7.Length)
                {
                    case 1:
                        {
                            tupleContent7_item1 = RegexSplit(arrContent7[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent7_item1 = RegexSplit(arrContent7[0], @"([^:]+)$");
                            tupleContent7_item2 = RegexSplit(arrContent7[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent7_item1 = RegexSplit(arrContent7[0], @"([^:]+)$");
                            tupleContent7_item2 = RegexSplit(arrContent7[1], @"([^:]+)$");
                            tupleContent7_item3 = RegexSplit(arrContent7[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 18].Value = tupleContent7_item1.Item1;
                ws.Cells[index, 19].Value = tupleContent7_item1.Item2;
                ws.Cells[index, 20].Value = tupleContent7_item2.Item1;
                ws.Cells[index, 21].Value = tupleContent7_item2.Item2;
                ws.Cells[index, 22].Value = tupleContent7_item3.Item1;
                ws.Cells[index, 23].Value = tupleContent7_item3.Item2;

                string strContent8 = string.IsNullOrWhiteSpace(item.Content8) ? "" : item.Content8;
                string[] arrContent8 = strContent8.Split(',');
                Tuple<string, string> tupleContent8_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent8_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent8_item3 = new Tuple<string, string>(null, null);
                switch (arrContent8.Length)
                {
                    case 1:
                        {
                            tupleContent8_item1 = RegexSplit(arrContent8[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent8_item1 = RegexSplit(arrContent8[0], @"([^:]+)$");
                            tupleContent8_item2 = RegexSplit(arrContent8[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent8_item1 = RegexSplit(arrContent8[0], @"([^:]+)$");
                            tupleContent8_item2 = RegexSplit(arrContent8[1], @"([^:]+)$");
                            tupleContent8_item3 = RegexSplit(arrContent8[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 24].Value = tupleContent8_item1.Item1;
                ws.Cells[index, 25].Value = tupleContent8_item1.Item2;
                ws.Cells[index, 26].Value = tupleContent8_item2.Item1;
                ws.Cells[index, 27].Value = tupleContent8_item2.Item2;
                ws.Cells[index, 28].Value = tupleContent8_item3.Item1;
                ws.Cells[index, 29].Value = tupleContent8_item3.Item2;
                #endregion

                ws.Cells[index, 30].Value = string.IsNullOrWhiteSpace(item.Content9) ? "" : item.Content9;
                ws.Cells[index, 31].Value = string.IsNullOrWhiteSpace(item.Content10) ? "" : item.Content10;

                #region Content11、Content12
                string strContent11 = string.IsNullOrWhiteSpace(item.Content11) ? "" : item.Content11;
                string[] arrContent11 = strContent11.Split(',');
                Tuple<string, string> tupleContent11_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent11_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent11_item3 = new Tuple<string, string>(null, null);
                switch (arrContent11.Length)
                {
                    case 1:
                        {
                            tupleContent11_item1 = RegexSplit(arrContent11[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent11_item1 = RegexSplit(arrContent11[0], @"([^:]+)$");
                            tupleContent11_item2 = RegexSplit(arrContent11[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent11_item1 = RegexSplit(arrContent11[0], @"([^:]+)$");
                            tupleContent11_item2 = RegexSplit(arrContent11[1], @"([^:]+)$");
                            tupleContent11_item3 = RegexSplit(arrContent11[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 32].Value = tupleContent11_item1.Item1;
                ws.Cells[index, 33].Value = tupleContent11_item1.Item2;
                ws.Cells[index, 34].Value = tupleContent11_item2.Item1;
                ws.Cells[index, 35].Value = tupleContent11_item2.Item2;
                ws.Cells[index, 36].Value = tupleContent11_item3.Item1;
                ws.Cells[index, 37].Value = tupleContent11_item3.Item2;

                string strContent12 = string.IsNullOrWhiteSpace(item.Content12) ? "" : item.Content12;
                string[] arrContent12 = strContent12.Split(',');
                Tuple<string, string> tupleContent12_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent12_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent12_item3 = new Tuple<string, string>(null, null);
                switch (arrContent12.Length)
                {
                    case 1:
                        {
                            tupleContent12_item1 = RegexSplit(arrContent12[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent12_item1 = RegexSplit(arrContent12[0], @"([^:]+)$");
                            tupleContent12_item2 = RegexSplit(arrContent12[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent12_item1 = RegexSplit(arrContent12[0], @"([^:]+)$");
                            tupleContent12_item2 = RegexSplit(arrContent12[1], @"([^:]+)$");
                            tupleContent12_item3 = RegexSplit(arrContent12[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 38].Value = tupleContent12_item1.Item1;
                ws.Cells[index, 39].Value = tupleContent12_item1.Item2;
                ws.Cells[index, 40].Value = tupleContent12_item2.Item1;
                ws.Cells[index, 41].Value = tupleContent12_item2.Item2;
                ws.Cells[index, 42].Value = tupleContent12_item3.Item1;
                ws.Cells[index, 43].Value = tupleContent12_item3.Item2;
                #endregion

                ws.Cells[index, 44].Value = string.IsNullOrWhiteSpace(item.Content13) ? "" : item.Content13;
                ws.Cells[index, 45].Value = string.IsNullOrWhiteSpace(item.Content14) ? "" : item.Content14;

                #region Content15、Content16
                string strContent15 = string.IsNullOrWhiteSpace(item.Content15) ? "" : item.Content15;
                string[] arrContent15 = strContent15.Split(',');
                Tuple<string, string> tupleContent15_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent15_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent15_item3 = new Tuple<string, string>(null, null);
                switch (arrContent15.Length)
                {
                    case 1:
                        {
                            tupleContent15_item1 = RegexSplit(arrContent15[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent15_item1 = RegexSplit(arrContent15[0], @"([^:]+)$");
                            tupleContent15_item2 = RegexSplit(arrContent15[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent15_item1 = RegexSplit(arrContent15[0], @"([^:]+)$");
                            tupleContent15_item2 = RegexSplit(arrContent15[1], @"([^:]+)$");
                            tupleContent15_item3 = RegexSplit(arrContent15[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 46].Value = tupleContent15_item1.Item1;
                ws.Cells[index, 47].Value = tupleContent15_item1.Item2;
                ws.Cells[index, 48].Value = tupleContent15_item2.Item1;
                ws.Cells[index, 49].Value = tupleContent15_item2.Item2;
                ws.Cells[index, 50].Value = tupleContent15_item3.Item1;
                ws.Cells[index, 51].Value = tupleContent15_item3.Item2;

                string strContent16 = string.IsNullOrWhiteSpace(item.Content16) ? "" : item.Content16;
                string[] arrContent16 = strContent16.Split(',');
                Tuple<string, string> tupleContent16_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent16_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent16_item3 = new Tuple<string, string>(null, null);
                switch (arrContent16.Length)
                {
                    case 1:
                        {
                            tupleContent16_item1 = RegexSplit(arrContent16[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent16_item1 = RegexSplit(arrContent16[0], @"([^:]+)$");
                            tupleContent16_item2 = RegexSplit(arrContent16[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent16_item1 = RegexSplit(arrContent16[0], @"([^:]+)$");
                            tupleContent16_item2 = RegexSplit(arrContent16[1], @"([^:]+)$");
                            tupleContent16_item3 = RegexSplit(arrContent16[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 52].Value = tupleContent16_item1.Item1;
                ws.Cells[index, 53].Value = tupleContent16_item1.Item2;
                ws.Cells[index, 54].Value = tupleContent16_item2.Item1;
                ws.Cells[index, 55].Value = tupleContent16_item2.Item2;
                ws.Cells[index, 56].Value = tupleContent16_item3.Item1;
                ws.Cells[index, 57].Value = tupleContent16_item3.Item2;
                #endregion

                ws.Cells[index, 58].Value = string.IsNullOrWhiteSpace(item.Content17) ? "" : item.Content17;
                ws.Cells[index, 59].Value = string.IsNullOrWhiteSpace(item.Content18) ? "" : item.Content18;

                #region Content19、Content20
                string strContent19 = string.IsNullOrWhiteSpace(item.Content19) ? "" : item.Content19;
                string[] arrContent19 = strContent19.Split(',');
                Tuple<string, string> tupleContent19_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent19_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent19_item3 = new Tuple<string, string>(null, null);
                switch (arrContent19.Length)
                {
                    case 1:
                        {
                            tupleContent19_item1 = RegexSplit(arrContent19[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent19_item1 = RegexSplit(arrContent19[0], @"([^:]+)$");
                            tupleContent19_item2 = RegexSplit(arrContent19[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent19_item1 = RegexSplit(arrContent19[0], @"([^:]+)$");
                            tupleContent19_item2 = RegexSplit(arrContent19[1], @"([^:]+)$");
                            tupleContent19_item3 = RegexSplit(arrContent19[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 60].Value = tupleContent19_item1.Item1;
                ws.Cells[index, 61].Value = tupleContent19_item1.Item2;
                ws.Cells[index, 62].Value = tupleContent19_item2.Item1;
                ws.Cells[index, 63].Value = tupleContent19_item2.Item2;
                ws.Cells[index, 64].Value = tupleContent19_item3.Item1;
                ws.Cells[index, 65].Value = tupleContent19_item3.Item2;

                string strContent20 = string.IsNullOrWhiteSpace(item.Content20) ? "" : item.Content20;
                string[] arrContent20 = strContent20.Split(',');
                Tuple<string, string> tupleContent20_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent20_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent20_item3 = new Tuple<string, string>(null, null);
                switch (arrContent20.Length)
                {
                    case 1:
                        {
                            tupleContent20_item1 = RegexSplit(arrContent20[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent20_item1 = RegexSplit(arrContent20[0], @"([^:]+)$");
                            tupleContent20_item2 = RegexSplit(arrContent20[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent20_item1 = RegexSplit(arrContent20[0], @"([^:]+)$");
                            tupleContent20_item2 = RegexSplit(arrContent20[1], @"([^:]+)$");
                            tupleContent20_item3 = RegexSplit(arrContent20[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 66].Value = tupleContent20_item1.Item1;
                ws.Cells[index, 67].Value = tupleContent20_item1.Item2;
                ws.Cells[index, 68].Value = tupleContent20_item2.Item1;
                ws.Cells[index, 69].Value = tupleContent20_item2.Item2;
                ws.Cells[index, 70].Value = tupleContent20_item3.Item1;
                ws.Cells[index, 71].Value = tupleContent20_item3.Item2;
                #endregion

                ws.Cells[index, 72].Value = string.IsNullOrWhiteSpace(item.Content21) ? "" : item.Content21;
                ws.Cells[index, 73].Value = string.IsNullOrWhiteSpace(item.Content22) ? "" : item.Content22;

                #region Content23、Content24
                string strContent23 = string.IsNullOrWhiteSpace(item.Content23) ? "" : item.Content23;
                string[] arrContent23 = strContent23.Split(',');
                Tuple<string, string> tupleContent23_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent23_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent23_item3 = new Tuple<string, string>(null, null);
                switch (arrContent23.Length)
                {
                    case 1:
                        {
                            tupleContent23_item1 = RegexSplit(arrContent23[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent23_item1 = RegexSplit(arrContent23[0], @"([^:]+)$");
                            tupleContent23_item2 = RegexSplit(arrContent23[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent23_item1 = RegexSplit(arrContent23[0], @"([^:]+)$");
                            tupleContent23_item2 = RegexSplit(arrContent23[1], @"([^:]+)$");
                            tupleContent23_item3 = RegexSplit(arrContent23[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 74].Value = tupleContent23_item1.Item1;
                ws.Cells[index, 75].Value = tupleContent23_item1.Item2;
                ws.Cells[index, 76].Value = tupleContent23_item2.Item1;
                ws.Cells[index, 77].Value = tupleContent23_item2.Item2;
                ws.Cells[index, 78].Value = tupleContent23_item3.Item1;
                ws.Cells[index, 79].Value = tupleContent23_item3.Item2;

                string strContent24 = string.IsNullOrWhiteSpace(item.Content24) ? "" : item.Content24;
                string[] arrContent24 = strContent24.Split(',');
                Tuple<string, string> tupleContent24_item1 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent24_item2 = new Tuple<string, string>(null, null);
                Tuple<string, string> tupleContent24_item3 = new Tuple<string, string>(null, null);
                switch (arrContent24.Length)
                {
                    case 1:
                        {
                            tupleContent24_item1 = RegexSplit(arrContent24[0], @"([^:]+)$");
                        }
                        break;
                    case 2:
                        {
                            tupleContent24_item1 = RegexSplit(arrContent24[0], @"([^:]+)$");
                            tupleContent24_item2 = RegexSplit(arrContent24[1], @"([^:]+)$");
                        }
                        break;
                    case 3:
                        {
                            tupleContent24_item1 = RegexSplit(arrContent24[0], @"([^:]+)$");
                            tupleContent24_item2 = RegexSplit(arrContent24[1], @"([^:]+)$");
                            tupleContent24_item3 = RegexSplit(arrContent24[2], @"([^:]+)$");
                        }
                        break;
                    default:
                        break;
                }
                ws.Cells[index, 80].Value = tupleContent24_item1.Item1;
                ws.Cells[index, 81].Value = tupleContent24_item1.Item2;
                ws.Cells[index, 82].Value = tupleContent24_item2.Item1;
                ws.Cells[index, 83].Value = tupleContent24_item2.Item2;
                ws.Cells[index, 84].Value = tupleContent24_item3.Item1;
                ws.Cells[index, 85].Value = tupleContent24_item3.Item2;
                #endregion

                ws.Cells[index, 86].Value = string.IsNullOrWhiteSpace(item.Content25) ? "" : item.Content25;

                #region Content26
                string strContent26 = string.IsNullOrWhiteSpace(item.Content26) ? "" : item.Content26;
                if (strContent26.StartsWith("是"))
                {
                    ws.Cells[index, 87].Value = "是";
                    ws.Cells[index, 88].Value = strContent26.Split(':')[1];
                }
                else
                {
                    ws.Cells[index, 87].Value = "否";
                    ws.Cells[index, 88].Value = "";
                }
                #endregion

                ws.Cells[index, 89].Value = string.IsNullOrWhiteSpace(item.Content27) ? "" : item.Content27;
                ws.Cells[index, 90].Value = string.IsNullOrWhiteSpace(item.Content28) ? "" : item.Content28;

                ws.Cells[index, 91].Value = string.IsNullOrWhiteSpace(item.Content29) ? "" : item.Content29;
                ws.Cells[index, 92].Value = string.IsNullOrWhiteSpace(item.Content30) ? "" : item.Content30;
                ws.Cells[index, 93].Value = string.IsNullOrWhiteSpace(item.Content31) ? "" : item.Content31;
                index++;
            }
        }

        private Tuple<string, string> RegexSplit(string input, string pattern)
        {
            string value = "";
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, pattern);
            if (match.Success)
            {
                value = match.Value;
            }
            input = input.Replace($":{value}", "");
            return new Tuple<string, string>(input, value);
        }
    }
}
