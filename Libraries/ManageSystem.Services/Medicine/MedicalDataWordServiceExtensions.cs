using Aspose.Words;
using Aspose.Words.Tables;
using Dapper;
using ManageSystem.Core.Domain.Medicine.Statistics;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Services.Medicine.Model;
using ManageSystem.Services.Medicine.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 医学数据生成word报表的业务处理类
    /// 
    /// word模版所使用的书签说明：
    /// 
    /// data_specimen_distribution   临床分离菌在各类标本中的分布 表格
    /// 
    /// </summary>
    public class MedicalDataWordServiceExtensions
    {
        private readonly IMedicalDataItemService _medicalDataItemService;
        private readonly IMedicalDataService _medicalDataService;
        private readonly IMedicalAntibioticResultService _medicalAntibioticResultService;

        public MedicalDataWordServiceExtensions()
        {
            this._medicalDataItemService = EngineContext.Current.Resolve<IMedicalDataItemService>();
            this._medicalDataService = EngineContext.Current.Resolve<IMedicalDataService>();
            this._medicalAntibioticResultService = EngineContext.Current.Resolve<IMedicalAntibioticResultService>();
        }


        #region  生成Word文件

        /// <summary>
        /// 生成Word文件
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateWord(long medicalDataId, ref string fileName)
        {

            var entity = this._medicalDataService.QueryEntity(medicalDataId);
            if (entity == null || entity.Id <= 0)
                throw new Exception("数据不存在");

            #region 1、基础数据和整理

            MedicalDataWordModel model = new MedicalDataWordModel()
            {
                Amount = 0,
                Id = medicalDataId,
                Doc = new Document()
            };

            //有效的数据行总数
            model.Amount = this._medicalDataItemService.Count(m => m.Mark > 0 && m.MedicalDataId == medicalDataId && m.IsValid == true);

            //本次上传数据的耐药性分析结果。基础数据，在前面先赋值，后面涉及到耐药性计算的可以都直接使用
            model.AntibioticResultList = this._medicalAntibioticResultService.GetResult(medicalDataId, organismIds: new List<long> { 996, 908, 493, 1093, 535, 882, 516, 601, 460, 732, 1007, 1155, 1135 }) ?? new List<GetAntibioticResultModel>();
            #endregion

            //2、生成word数据
            string file = this.CreateWordGetData(model);

            if (string.IsNullOrWhiteSpace(file))
            {
                return null;
            }

            //3、将地址保存到数据库中，缓存起来
            entity.WordFile = file;
            this._medicalDataService.Update(entity);
            //4、返回文件的地址
           // fileName = entity.FileName.Substring(0, entity.FileName.LastIndexOf('.')) + "_word_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".docx";
            fileName = _medicalDataService.QueryEntity(model.Id).Year.ToString()+ _medicalDataService.QueryEntity(model.Id)?.HospitalName + "细菌耐药性监测结果.docx";
            return file;
        }

        public bool WordToPDF(string sourcePath)
        {
            bool result = false;
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document document = null;
            try
            {
                //wordApp.Visible = false;
                //document = wordApp.Documents.Open(sourcePath);
                string PDFPath = sourcePath.Replace(".docx", ".pdf");//pdf存放位置
                if (!File.Exists(@PDFPath))//存在PDF，不需要继续转换
                {
                    //    document.ExportAsFixedFormat(PDFPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                    //}

                    document = wordApp.Documents.Open(sourcePath, missing, missing, missing, missing, missing,
                                   missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    //word另存为pdf

                    document.ExportAsFixedFormat(PDFPath, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF
                        , false, Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument, 1, 1,
                        Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent
                        , false, true, Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks, true, true, false, ref missing);
                }

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
            }
            finally
            {
                document.Close();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="medicalDataId"></param>
        private string CreateWordGetData(MedicalDataWordModel model)
        {
            try
            {
                string tmppath = System.Web.HttpContext.Current.Server.MapPath("/Content/File/CHINET细菌耐药性监测模板.docx");
                model.Doc = new Document(tmppath);

                // 设置年份
                this.SetWordYear(model);

                // 设置医院名称
                this.SetWordHospitalName(model);

                MedicalStatisticsService _medicalStatisticsService = new MedicalStatisticsService();

                #region 表01. 临床分离菌在各类标本中的分布
                /**
                 * 呼吸道标本: at,fn,br,no,rl,ru,sp,th,tr,ta,lu,mo,ea,em,eo,ba,tn
                 * 尿道标本: ue,ur,uc,cv,ub,uz
                 * 血液标本: bl
                 * 脑脊液标本: sf
                 * 伤口脓液标本: as,ad,ps,pt,ux,ui,sw,wd,fi,sb,um,ud,de,ac,pt,ak,ul
                 * 无菌体液: ab,am,bi,mi,di,fl,ga,pf,bn,su
                 * 生殖道分泌物: gn,gf,gm,va,sm,cx,pl,ut,iu,ed
                 * 粪便标本: st,re,mc
                 * 其他: 除上以外的标本
                 */
                this.WordToTable1(model, _medicalStatisticsService);
                #endregion

                // 表02. xxxx年主要临床主要分离菌种分布（前20位）
                this.WordToTable2(model, DateTime.Now.Year);

                // 表03. xxx株呼吸道标本分离菌主要菌种分布
                this.WordToTable3(model, _medicalStatisticsService);

                // 表04. XXX株尿道标本分离菌主要菌种分布
                this.WordToTable4(model, _medicalStatisticsService);

                // 表05. XXX株血液标本分离菌主要菌种分布
                this.WordToTable5(model, _medicalStatisticsService);

                // 表06. XXX株脑脊液标本分离菌主要菌种分布
                this.WordToTable6(model, _medicalStatisticsService);

                // 表07. XXX株伤口脓液标本分离菌主要菌种分布
                this.WordToTable7(model, _medicalStatisticsService);

                // 表08. XXX株胸水标本分离菌主要菌种分布
                this.WordToTable8(model, _medicalStatisticsService);

                // 表09. XXX株腹水标本分离菌主要菌种分布
                this.WordToTable9(model, _medicalStatisticsService);

                // 表10. 金黄色葡萄球菌对抗菌药物的耐药率和敏感率
                this.WordToTable10(model);

                // 表11. 凝固酶阴性葡萄球菌对抗菌药物的耐药率和敏感率
                this.WordToTable11(model);

                // 表12. 屎肠球菌和粪肠球菌对抗菌药物的耐药率和敏感率
                this.WordToTable12(model);

                // 表13. 患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率
                this.WordToTable13(model);

                // 表14. 大肠埃希菌和肺炎克雷伯菌对抗菌药物的耐药率和敏感率
                this.WordToTable14(model);

                // 表15. 奇异变形杆菌和阴沟肠杆菌对抗菌药物的耐药率和敏感率
                this.WordToTable15(model);

                // 表16. 黏质沙雷菌和弗劳地柠檬酸杆菌对抗菌药物的耐药率和敏感率
                this.WordToTable16(model);

                // 表17. 铜绿假单胞菌和鲍曼不动杆菌对抗菌药物的耐药率和敏感率
                this.WordToTable17(model);

                // 表18. 嗜麦芽窄食单胞菌和洋葱伯克霍尔德菌对抗菌药物的耐药率和敏感率
                this.WordToTable18(model);

                // 表19. 流感嗜血杆菌对抗菌药物的耐药率和敏感率
                this.WordToTable19(model, _medicalStatisticsService);

                //// 设置年份
                //this.SetWordDate(model);

                ////临床主要分离菌种分布 表格
                //this.CreateWordData1(model);

                ////葡萄球菌属对抗菌药物的耐药率和敏感率 表格
                //this.CreateWordData2(model);

                ////粪肠球菌和屎肠球菌对抗菌药物的耐药率和敏感率   表格
                //this.CreateWordData3(model);

                ////患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率   表格
                //this.CreateWordData4(model);

                ////肠杆菌科对抗菌药物的耐药率和敏感率   表格
                //this.CreateWordData5(model);

                ////不发酵糖革兰阴性杆菌对抗菌药物的耐药率和敏感率   表格
                //this.CreateWordData6(model);

                ////流感嗜血杆菌(Haemophilus influenzae)   表格
                //this.CreateWordData7(model);

                ////临床分离菌在各类标本中的分布   表格
                //this.CreateWordData8(model);

                ////标本分离菌主要菌分布   表格
                //this.CreateWordData9(model);

                String fileTemp = "/Content/Upload/MedicalData/Word/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".docx";
                string newPath = System.Web.HttpContext.Current.Server.MapPath(fileTemp);
                model.Doc.Save(newPath);

                return fileTemp;
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                Log4Helper.Debug(this.GetType(), ex.InnerException);
                throw;
            }

        }

        /// <summary>
        /// 设置表头的日期
        /// </summary>
        /// <param name="model"></param>
        private void SetWordDate(MedicalDataWordModel model)
        {
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_chinet_date");//跳转到书签名的位置
            builder.Write(DateTime.Now.Year.ToString());
        }

        #region 2019 Word
        /// <summary>
        /// 临床主要分离菌种分布   表格
        /// </summary>
        /// <param name="model">数据封装</param>
        private void CreateWordData1(MedicalDataWordModel model)
        {
            //获取数据
            var list = new MedicalStatisticsService().SpecTypeQueryTop20(model.Id, 1900) ?? new List<StatisticsMedicalOrganism>();

            #region  生成word表格

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_medical_organism");//跳转到书签名的位置

            // 标题
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
            builder.RowFormat.Height = 20;
            builder.Writeln(DateTime.Now.Year.ToString() + "年主要临床主要分离菌种分布（前20位）");
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Writeln("（%）");
            builder.Font.Size = 10;
            builder.CellFormat.Borders.Top.Color = Color.Black;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Top.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;

            //表头
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.InsertCell();
            builder.Write("细菌名称");
            builder.InsertCell();
            builder.Write("数量");
            builder.InsertCell();
            builder.Write("占比（%）");
            builder.EndRow();

            //数据
            builder.CellFormat.Borders.Color = Color.White;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;

            if (list == null || !list.Any())
            {
                //没有数据
                builder.InsertCell();
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中对齐
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center; // 水平居中对齐
                builder.Write("没有数据");
                builder.InsertCell();
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.InsertCell();
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
            }
            else
            {
                foreach (var item in list)
                {
                    builder.InsertCell();
                    builder.Write(item.Name);
                    builder.InsertCell();
                    builder.Write(item.DataCount.ToString());
                    builder.InsertCell();
                    builder.Write(item.Ratio >= 0 ? Convert.ToDecimal(item.Ratio).ToString("N") : "");
                    builder.EndRow();
                }
            }

            builder.EndTable();

            #endregion
        }

        /// <summary>
        /// 葡萄球菌属对抗菌药物的耐药率和敏感率   表格
        /// </summary>
        /// <param name="model">数据封装</param>
        private void CreateWordData2(MedicalDataWordModel model)
        {
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_medical_data_table_3"); // 跳转到指定书签

            #region 葡萄球菌属对抗菌药物的耐药率和敏感率

            //获取数据
            var data = new MedicalStatisticsService().GetByMedicalDataTable3(model.Id) ?? new MedicalDataWordMRSAModel();

            #region 标题

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
            builder.RowFormat.Height = 20;
            builder.Writeln("金黄色葡萄球菌对抗菌药物的耐药率和敏感率");
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Writeln("（%）");
            builder.Font.Size = 10;
            builder.CellFormat.Borders.Top.Color = Color.Black;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Top.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;
            #endregion

            #region 表头

            List<string> tableHeaderData = new List<string> { $"MRSA（n={data.MrsaCount}）", $"MSSA（n={data.MssaCount}）" };

            int nullWidth = 60;
            builder.InsertCell();
            builder.CellFormat.Width = 60;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中
            builder.CellFormat.VerticalMerge = CellMerge.First; // 垂直合并单元格
            builder.Write("抗菌药物");

            foreach (string item in tableHeaderData)
            {
                nullWidth += 60;
                builder.InsertCell();
                builder.CellFormat.Width = 60;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.Write(item);
            }

            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.Width = 60;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            builder.CellFormat.VerticalMerge = CellMerge.Previous;

            foreach (string item in tableHeaderData)
            {
                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.Font.Size = 9;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.Font.Size = 9;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.Font.Size = 9;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("敏感率");

            }

            builder.EndRow();

            #endregion

            #region 填充数据
            builder.CellFormat.Borders.Color = Color.Gray;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            if (data.List != null && data.List.Any())
            {
                foreach (var item in data.List)
                {
                    // 抗生素名称
                    builder.InsertCell();
                    builder.CellFormat.Width = 60;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item.AntibioticName);

                    // MRSA 数量
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.MRSAAntibioticCount.ToString());
                    // MRSA 耐药率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MRSAR) ? "0" : item.MRSAR);
                    // MRSA 敏感率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MRSAS) ? "0" : item.MRSAS);

                    // MSSA 数量
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.MSSAAntibioticCount.ToString());
                    // MSSA 耐药率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MSSAR) ? "0" : item.MSSAR);
                    // MSSA 敏感率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MSSAS) ? "0" : item.MSSAS);

                    builder.EndRow();
                }
            }
            else
            {
                builder.InsertCell();
                builder.CellFormat.Width = nullWidth;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Write("没有数据");
                builder.EndRow();
            }

            #endregion

            #endregion

            // 空2行
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent);

            #region 凝固酶阴性葡萄球菌对抗菌药物的耐药率和敏感率
            var data2 = new MedicalStatisticsService().GetByMedicalDataTable3_2(model.Id) ?? new MedicalDataWordMRSAModel();

            #region 标题

            builder.InsertCell();
            builder.CellFormat.Width = nullWidth;
            builder.Font.Size = 11;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.LineWidth = 0;
            builder.CellFormat.Borders.Color = Color.Transparent;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write("凝固酶阴性葡萄球菌对抗菌药物的耐药率和敏感率");
            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.Width = nullWidth;
            builder.Font.Size = 10;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.LineWidth = 0;
            builder.CellFormat.Borders.Color = Color.Transparent;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Write("（%）");
            builder.EndRow();

            //builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            //builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
            //builder.RowFormat.Height = 20;
            //builder.Writeln("凝固酶阴性葡萄球菌对抗菌药物的耐药率和敏感率");

            //builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            //builder.Writeln("（%）");
            //builder.Font.Size = 11;
            //builder.CellFormat.Borders.Top.Color = Color.Black;
            //builder.CellFormat.Borders.LineStyle = LineStyle.None;
            //builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
            //builder.CellFormat.Borders.Top.LineWidth = 1;
            //builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            //builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
            //builder.CellFormat.Borders.Bottom.LineWidth = 1;
            #endregion

            #region 表头

            List<string> tableHeaderData2 = new List<string> { $"MRCNS（n={data2.MrcnsCount}）", $"MSCNS（n={data2.MscnsCount}）" };

            int nullWidth2 = 60;
            builder.InsertCell();
            builder.CellFormat.Width = 60;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中
            builder.CellFormat.VerticalMerge = CellMerge.First; // 垂直合并单元格
            builder.Write("抗菌药物");

            foreach (string item in tableHeaderData2)
            {
                nullWidth2 += 60;
                builder.InsertCell();
                builder.CellFormat.Width = 60;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.Write(item);
            }

            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.Width = 60;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
            builder.CellFormat.VerticalMerge = CellMerge.Previous;

            foreach (string item in tableHeaderData2)
            {
                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.Font.Size = 9;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.Font.Size = 9;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.Font.Size = 9;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("敏感率");

            }
            builder.EndRow();

            #endregion

            #region 填充数据
            builder.CellFormat.Borders.Color = Color.Gray;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            if (data2.List != null && data2.List.Any())
            {
                foreach (var item in data2.List)
                {
                    // 抗生素名称
                    builder.InsertCell();
                    builder.CellFormat.Width = 60;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item.AntibioticName);

                    // MRCNS数量
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.MRCNSAntibioticCount.ToString());
                    // MRCNS耐药率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MRCNSR) ? "0" : item.MRCNSR);
                    // MRCNS敏感率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MRCNSS) ? "0" : item.MRCNSS);

                    // MSCNS数量
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.MSCNSAntibioticCount.ToString());
                    // MSCNS耐药率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MSCNSR) ? "0" : item.MSCNSR);
                    // MSCNS敏感率
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(string.IsNullOrWhiteSpace(item.MSCNSS) ? "0" : item.MSCNSS);

                    builder.EndRow();
                }
            }
            else
            {
                builder.InsertCell();
                builder.CellFormat.Width = nullWidth2;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Write("没有数据");
                builder.EndRow();
            }

            #endregion

            #endregion

            builder.EndTable();
        }

        /// <summary>
        /// 粪肠球菌和屎肠球菌对抗菌药物的耐药率和敏感率   表格
        /// </summary>
        /// <param name="model"></param>
        private void CreateWordData3(MedicalDataWordModel model)
        {
            //获取数据
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_medical_data_table_4");//跳转到书签名是zy的位置
            int nullWidth = 60 + 50 + 50;

            //粪肠球菌对抗菌药物的耐药率和敏感率
            this.SingleOrganismData(908, model, "粪肠球菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            //屎肠球菌对抗菌药物的耐药率和敏感率
            this.SingleOrganismData(996, model, "屎肠球菌对抗菌药物的耐药率和敏感率", builder);

            builder.EndTable();

        }

        /// <summary>
        /// 患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率   表格
        /// </summary>
        /// <param name="model">数据封装</param>
        private void CreateWordData4(MedicalDataWordModel model)
        {
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_medical_data_table_7");//跳转到书签名是zy的位置

            //粪肠球菌对抗菌药物的耐药率和敏感率
            this.SingleOrganismData(493, model, "患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率", builder);

            builder.EndTable();
        }

        /// <summary>
        /// 肠杆菌科对抗菌药物的耐药率和敏感率   表格
        /// </summary>
        /// <param name="model"></param>
        private void CreateWordData5(MedicalDataWordModel model)
        {
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_medical_data_table_8");//跳转到书签名是zy的位置
            int nullWidth = 60 + 50 + 50;

            // 大肠埃希菌(Escherichia coli)
            this.SingleOrganismData(1093, model, "大肠埃希菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 肺炎克雷伯菌(Klebsiella pneumoniae)
            this.SingleOrganismData(535, model, "肺炎克雷伯菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 奇异变形杆菌(Proteus mirabilis)
            this.SingleOrganismData(882, model, "奇异变形杆菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 阴沟肠杆菌(Enterobacter cloacae)
            this.SingleOrganismData(516, model, "阴沟肠杆菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 黏质沙雷菌(Serratia marcescens)
            this.SingleOrganismData(601, model, "黏质沙雷菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 弗劳地柠檬酸杆菌(Citrobacter freundii)
            this.SingleOrganismData(460, model, "弗劳地柠檬酸杆菌对抗菌药物的耐药率和敏感率", builder);

            builder.EndTable();
        }

        /// <summary>
        /// 不发酵糖革兰阴性杆菌对抗菌药物的耐药率和敏感率   表格
        /// </summary>
        /// <param name="model"></param>
        private void CreateWordData6(MedicalDataWordModel model)
        {
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_medical_data_table_11");//跳转到书签名是zy的位置

            int nullWidth = 60 + 50 + 50;

            // 铜绿假单胞菌(Pseudomonas aeruginosa)
            this.SingleOrganismData(732, model, "铜绿假单胞菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 鲍曼不动杆菌(Acinetobacter baumannii)
            this.SingleOrganismData(1007, model, "鲍曼不动杆菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 嗜麦芽窄食单胞菌(Smaltophilia)
            this.SingleOrganismData(1155, model, "嗜麦芽窄食单胞菌对抗菌药物的耐药率和敏感率", builder);
            this.GenerateNullRow(builder, 2, nullWidth, "", Color.Transparent); // 空2行

            // 洋葱伯克霍尔德菌(Burkholderia cepacia)
            this.SingleOrganismData(1135, model, "洋葱伯克霍尔德菌对抗菌药物的耐药率和敏感率", builder);

            builder.EndTable();

        }

        /// <summary>
        /// 流感嗜血杆菌(Haemophilus influenzae)   表格
        /// </summary>
        /// <param name="model"></param>
        private void CreateWordData7(MedicalDataWordModel model)
        {
            //获取数据
            var data = this.GetCreateWordData7(model.Id);

            #region  产生word
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_medical_data_table_12");//跳转到书签名是zy的位置

            #region 标题
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
            builder.RowFormat.Height = 20;
            builder.Writeln($"流感嗜血杆菌对抗菌药物的耐药率和敏感率");
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Writeln("（%）");
            builder.Font.Size = 10;
            builder.CellFormat.Borders.Top.Color = Color.Black;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Top.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;
            #endregion

            #region 表头
            int nullWidth = 60 + 40 + 40 + 40;

            #region 第一行
            builder.InsertCell();
            builder.CellFormat.Borders.Color = Color.Gray;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.Width = 45;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中
            builder.CellFormat.VerticalMerge = CellMerge.First; // 垂直合并单元格
            builder.Write("抗生素");

            builder.InsertCell();
            builder.CellFormat.Width = 45;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.Write($"合计 (n={data.Item1["合计"]})");

            builder.InsertCell();
            builder.CellFormat.Width = 45;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.Write($"成人 (n={data.Item1["成人"]})");

            builder.InsertCell();
            builder.CellFormat.Width = 45;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.Write($"儿童 (n={data.Item1["儿童"]})");

            builder.EndRow();
            #endregion

            #region 第二行
            builder.InsertCell();
            builder.CellFormat.Width = 45;
            builder.CellFormat.VerticalMerge = CellMerge.Previous;

            for (int i = 0; i < 3; i++)
            {
                builder.InsertCell();
                builder.CellFormat.Width = 15;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Font.Size = 8;
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 15;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Font.Size = 8;
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 15;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Font.Size = 8;
                builder.Write("敏感率");
            }

            builder.EndRow();
            #endregion

            #endregion

            #region 填充数据
            builder.CellFormat.Borders.Color = Color.Gray;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            if (data.Item2.Any())
            {
                foreach (KeyValuePair<string, List<string>> item in data.Item2)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 45;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item.Key);

                    foreach (string node in item.Value)
                    {
                        builder.InsertCell();
                        builder.CellFormat.Width = 15;
                        builder.Font.Size = 8;
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        builder.Write(node);
                    }

                    builder.EndRow();
                }
            }
            else
            {
                builder.InsertCell();
                builder.CellFormat.Width = nullWidth;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Write("没有数据");
                builder.EndRow();
            }
            #endregion

            builder.EndTable();

            #endregion

        }

        /// <summary>
        /// 流感嗜血杆菌(Haemophilus influenzae)   获取数据
        /// </summary>
        /// <param name="medicalDataId"></param>
        /// <returns></returns>
        private Tuple<Dictionary<string, int>, Dictionary<string, List<string>>> GetCreateWordData7(long medicalDataId)
        {
            var statisticsService = new MedicalStatisticsService();

            return statisticsService.GetMedicalDataTableByhin(medicalDataId);
        }

        /// <summary>
        /// 临床分离菌在各类标本中的分布   表格
        /// </summary>
        /// <param name="model"></param>
        private void CreateWordData8(MedicalDataWordModel model)
        {
            //获取数据
            var list = new MedicalStatisticsService().SpecTypeQuery(model.Id) ?? new List<StatisticsMedicalSpecTypeModel>();

            #region 绑定数据

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_specimen_distribution");//跳转到书签名是zy的位置

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
            builder.RowFormat.Height = 20;
            //builder.Writeln(list.Sum(m => m.Count).ToString() + "株临床分离菌在各类标本中的分布");
            builder.Writeln("临床分离菌在各类标本中的分布");
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Writeln("（%）");
            builder.Font.Size = 10;
            builder.CellFormat.Borders.Top.Color = Color.Black;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Top.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;

            //表头
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.InsertCell();
            builder.Write("标本名称");
            builder.InsertCell();
            builder.Write("数量");
            builder.InsertCell();
            builder.Write("分布比例（%）");
            builder.EndRow();

            //数据
            builder.CellFormat.Borders.Color = Color.White;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;

            if (list == null || !list.Any())
            {
                //没有数据
                builder.InsertCell();
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中对齐
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center; // 水平居中对齐
                builder.Write("没有数据");
                builder.InsertCell();
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.InsertCell();
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
            }
            else
            {
                foreach (var item in list)
                {
                    builder.InsertCell();
                    builder.Write(item.Name);
                    builder.InsertCell();
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();
                    builder.Write(item.Ratio.GetDecimal2(1).ToString(""));
                    builder.EndRow();
                }
            }

            builder.EndTable();

            #endregion

        }

        /// <summary>
        /// 标本分离菌主要菌分布   表格
        /// </summary>
        /// <param name="model"></param>
        private void CreateWordData9(MedicalDataWordModel model)
        {
            //获取数据
            DocumentBuilder builder = new DocumentBuilder(model.Doc);

            //呼吸道标本分离菌主要菌分布
            builder.MoveToBookmark("data_specimen_distribution_hxd");
            this.SpecTypeData("at,fn,br,no,rl,ru,sp,th,tr,ta,lu,mo,ea,em,eo,ba,tn", model, "呼吸道标本分离菌主要菌种分布", builder);
            builder.EndBookmark("data_specimen_distribution_hxd");

            //尿道标本分离菌主要菌种分布
            builder.MoveToBookmark("data_specimen_distribution_nd");
            this.SpecTypeData("ue,ur,uc,cv,ub,uz", model, "尿道标本分离菌主要菌种分布", builder);
            builder.EndBookmark("data_specimen_distribution_nd");

            //血液标本分离菌主要菌种分布
            builder.MoveToBookmark("data_specimen_distribution_xy");
            this.SpecTypeData("bl", model, "血液标本分离菌主要菌种分布", builder);
            builder.EndBookmark("data_specimen_distribution_xy");

            //脑脊液标本分离菌主要菌种分布
            builder.MoveToBookmark("data_specimen_distribution_njy");
            this.SpecTypeData("sf", model, "脑脊液标本分离菌主要菌种分布", builder);
            builder.EndBookmark("data_specimen_distribution_njy");

            //伤口脓液标本分离菌主要菌种分布 
            builder.MoveToBookmark("data_specimen_distribution_skny");
            this.SpecTypeData("as,ad,ps,pt,ux,ui,sw,wd,fi,sb,um,ud,de,ac,pt,ak,ul", model, "伤口脓液标本分离菌主要菌种分布", builder);
            builder.EndBookmark("data_specimen_distribution_skny");

            // 胸水
            builder.MoveToBookmark("data_specimen_distribution_xshui");
            this.SpecTypeData("pf", model, "胸水标本分离菌主要菌种分布", builder);
            builder.EndBookmark("data_specimen_distribution_xshui");

            // 腹水
            builder.MoveToBookmark("data_specimen_distribution_fshui");
            this.SpecTypeData("ab", model, "腹水标本分离菌主要菌种分布", builder);

            builder.EndTable();

        }
        #endregion 2019 Word

        #endregion

        #region 2020新Word

        /// <summary>
        /// 设置表头的年份
        /// </summary>
        /// <param name="model"></param>
        private void SetWordYear(MedicalDataWordModel model)
        {
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_chinet_year");//跳转到书签名的位置
            builder.Write(_medicalDataService.QueryEntity(model.Id).Year.ToString());
        }

        /// <summary>
        /// 设置医院名称
        /// </summary>
        /// <param name="model"></param>
        private void SetWordHospitalName(MedicalDataWordModel model)
        {
            string hospitalName = _medicalDataService.QueryEntity(model.Id)?.HospitalName;
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            builder.MoveToBookmark("data_chinet_hospital");
            builder.Write(hospitalName);
        }

        /// <summary>
        /// 表格序号
        /// </summary>
        private int table_index = 1;
        /// <summary>
        /// 表1. 临床分离菌在各类标本中的分布
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable1(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            Dictionary<string, Tuple<int, decimal>> data_table_01 = statisticsService.GetTable01_2020(model.Id);
            DocumentBuilder builder = new DocumentBuilder(model.Doc);

            if (data_table_01.Count > 0)
            {
                builder.MoveToBookmark("data_table_01");

                #region 标题/表名
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.临床分离菌在各类标本中的分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                builder.CellFormat.Borders.Top.Color = Color.Black;
                builder.CellFormat.Borders.LineStyle = LineStyle.None;
                builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Top.LineWidth = 1;
                builder.CellFormat.Borders.Bottom.Color = Color.Gray;
                builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Bottom.LineWidth = 1;
                #endregion

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.Write("细菌名称");
                builder.InsertCell();
                builder.Write("数量");
                builder.InsertCell();
                builder.Write("占比（%）");
                builder.EndRow();
                #endregion

                #region tbody
                builder.CellFormat.Borders.Color = Color.White;
                builder.CellFormat.Borders.LineStyle = LineStyle.None;

                int rowCount = data_table_01.Count();
                int rowIndex = 1;
                foreach (KeyValuePair<string, Tuple<int, decimal>> item in data_table_01)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        // 最后一行有边框
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Write(item.Key);
                    builder.InsertCell();
                    builder.Write(item.Value.Item1.ToString());
                    builder.InsertCell();
                    builder.Write(item.Value.Item2 >= 0 ? item.Value.Item2.GetDecimal2(1) > 0.0m ? item.Value.Item2.GetDecimal2(1).ToString() : item.Value.Item2.ToString() : "");
                    builder.EndRow();
                }
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.InsertCell();
                //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                //builder.InsertCell();
                //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                //builder.EndRow();
            }
            #endregion
        }

        List<KeyValuePair<string, string>> item_count = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// 表02. xxxx年主要临床主要分离菌种分布（前20位）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="year"></param>
        private void WordToTable2(MedicalDataWordModel model, int year)
        {
            // 获取数据
            var list = new MedicalStatisticsService().SpecTypeQueryTop20(model.Id, year) ?? new List<StatisticsMedicalOrganism>();
            item_count = new List<KeyValuePair<string, string>>();
            foreach (var item in list)
            {
                item_count.Add(new KeyValuePair<string, string>(item.Name, item.DataCount.ToString()));
            }
            list = list.Take(20).ToList();
            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (list != null && list.Count > 0)
            {
                builder.MoveToBookmark("data_table_02");//跳转到书签名的位置

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{_medicalDataService.QueryEntity(model.Id).Year}年主要临床主要分离菌种分布（前20位）（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                builder.CellFormat.Borders.Top.Color = Color.Black;
                builder.CellFormat.Borders.LineStyle = LineStyle.None;
                builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Top.LineWidth = 1;
                builder.CellFormat.Borders.Bottom.Color = Color.Gray;
                builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Bottom.LineWidth = 1;
                #endregion

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.Write("细菌名称");
                builder.InsertCell();
                builder.Write("数量");
                builder.InsertCell();
                builder.Write("占比（%）");
                builder.EndRow();
                #endregion

                #region tbody
                builder.CellFormat.Borders.Color = Color.White;
                builder.CellFormat.Borders.LineStyle = LineStyle.None;

                int rowCount = list.Count;
                int rowIndex = 1;
               
                foreach (var item in list)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        // 最后一行有边框
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Write(item.Name);
                    builder.InsertCell();
                    builder.Write(item.DataCount.ToString());
                    builder.InsertCell();
                    builder.Write(item.Ratio >= 0 ? item.Ratio.ToString() : "");
                    builder.EndRow();
                   
                }
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Borders.Bottom.LineWidth = 1;
                //builder.CellFormat.Borders.Bottom.Color = Color.Gray;
                //builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中对齐
                //builder.ParagraphFormat.Alignment = ParagraphAlignment.Center; // 水平居中对齐
                //builder.Write("没有数据");
                //builder.InsertCell();
                //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                //builder.InsertCell();
                //builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                //builder.EndRow();
            }
            #endregion
        }

        /// <summary>
        /// 表03. xxx株呼吸道标本分离菌主要菌种分布
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable3(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            //获取数据
            var data = statisticsService.GetTable03_2020(model.Id, "at,fn,br,no,rl,ru,sp,th,tr,ta,lu,mo,ea,em,eo,ba,tn") ?? new List<StatisticsMedicalSpecTypeModel>();
            int count = data.Sum(r => (int?)r.Count) ?? 0;
            data = data.Take(20).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (data != null && data.Count() > 0)
            {
                builder.MoveToBookmark("data_table_03");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{count}株呼吸道标本分离菌主要菌种分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                builder.CellFormat.Borders.Top.Color = Color.Black;
                builder.CellFormat.Borders.LineStyle = LineStyle.None;
                builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Top.LineWidth = 1;
                builder.CellFormat.Borders.Bottom.Color = Color.Gray;
                builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Bottom.LineWidth = 1;
                #endregion

                var table = builder.StartTable();

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.CellFormat.Width = 105;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("菌种名称");
                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Write("数量");
                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.Write("分布比例");
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.EndRow();
                #endregion

                #region tbody
                builder.CellFormat.Borders.Color = Color.White;
                builder.CellFormat.Borders.LineStyle = LineStyle.None;


                int rowCount = data.Count();
                int rowIndex = 1;
                foreach (var item in data)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item.Name);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();

                    builder.CellFormat.Width = 45;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.Ratio > 0.00m ? item.Ratio.GetDecimal2(1) > 0.0m ? item.Ratio.GetDecimal2(1).ToString() : item.Ratio.GetDecimal2().ToString() : "0.0");
                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;

                table_index++;
                builder.EndTable();

            }
            else
            {
                ////没有数据
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
                //builder.CellFormat.Borders.Bottom.Color = Color.Gray;
                //builder.CellFormat.Borders.Bottom.LineWidth = 1;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                //builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

        }

        /// <summary>
        /// 表04. XXX株尿道标本分离菌主要菌种分布
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable4(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            //获取数据
            var data = statisticsService.GetTable03_2020(model.Id, "ue,ur,uc,cv,ub,uz") ?? new List<StatisticsMedicalSpecTypeModel>();
            int count = data.Sum(r => (int?)r.Count) ?? 0;
            data = data.Take(20).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);

            if (data != null && data.Count() > 0)
            {
                builder.MoveToBookmark("data_table_04");

                #region 标题
              //  this.SetCellFormatAlignmentCenter(builder);
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{count}株尿道标本分离菌主要菌种分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                this.SetCellBorders(builder, "Top", "Bottom");
                #endregion

                var table = builder.StartTable();

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.CellFormat.Width = 105;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("菌种名称");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("分布比例");
                builder.EndRow();
                #endregion

                #region tbody
                this.ClearCellBorders(builder);

                int rowCount = data.Count();
                int rowIndex = 1;
                foreach (var item in data)
                {
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }

                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.Write(item.Name);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();

                    builder.CellFormat.Width = 45;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Ratio > 0.00m ? item.Ratio.GetDecimal2(1) > 0.0m ? item.Ratio.GetDecimal2(1).ToString() : item.Ratio.GetDecimal2().ToString() : "0.0");
                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                ////没有数据
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

        }

        /// <summary>
        /// 表05. XXX株血液标本分离菌主要菌种分布
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable5(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            //获取数据
            var data = statisticsService.GetTable03_2020(model.Id, "bl") ?? new List<StatisticsMedicalSpecTypeModel>();
            int count = data.Sum(r => (int?)r.Count) ?? 0;
            data = data.Take(20).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (data != null && data.Count() > 0)
            {
                builder.MoveToBookmark("data_table_05");

                #region 标题
              //  this.SetCellFormatAlignmentCenter(builder);
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{count}株血液标本分离菌主要菌种分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                this.SetCellBorders(builder, "Top", "Bottom");
                #endregion

                var table = builder.StartTable();

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.CellFormat.Width = 105;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("菌种名称");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("分布比例");
                builder.EndRow();
                #endregion

                #region tbody
                this.ClearCellBorders(builder);


                int rowCount = data.Count();
                int rowIndex = 1;
                foreach (var item in data)
                {
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }

                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.Write(item.Name);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();

                    builder.CellFormat.Width = 45;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Ratio > 0.00m ? item.Ratio.GetDecimal2(1) > 0.0m ? item.Ratio.GetDecimal2(1).ToString() : item.Ratio.GetDecimal2().ToString() : "0.0");
                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                ////没有数据
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

        }

        /// <summary>
        /// 表06. XXX株脑脊液标本分离菌主要菌种分布
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable6(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            //获取数据
            var data = statisticsService.GetTable03_2020(model.Id, "sf") ?? new List<StatisticsMedicalSpecTypeModel>();
            int count = data.Sum(r => (int?)r.Count) ?? 0;
            data = data.Take(20).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);

            if (data != null && data.Count() > 0)
            {
                builder.MoveToBookmark("data_table_06");

                #region 标题
              //  this.SetCellFormatAlignmentCenter(builder);
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{count}株脑脊液标本分离菌主要菌种分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                this.SetCellBorders(builder, "Top", "Bottom");
                #endregion

                var table = builder.StartTable();

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.CellFormat.Width = 105;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("菌种名称");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("分布比例");
                builder.EndRow();
                #endregion

                #region tbody
                this.ClearCellBorders(builder);


                int rowCount = data.Count();
                int rowIndex = 1;
                foreach (var item in data)
                {
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }

                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.Write(item.Name);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();

                    builder.CellFormat.Width = 45;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Ratio > 0.00m ? item.Ratio.GetDecimal2(1) > 0.0m ? item.Ratio.GetDecimal2(1).ToString() : item.Ratio.GetDecimal2().ToString() : "0.0");
                    builder.EndRow();
                }

                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                ////没有数据
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
               // builder.EndRow();
            }
            #endregion

            //table.Alignment = TableAlignment.Center;
        }

        /// <summary>
        /// 表07. XXX株伤口脓液标本分离菌主要菌种分布
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable7(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            //获取数据
            var data = statisticsService.GetTable03_2020(model.Id, "as,ad,ps,pt,ux,ui,sw,wd,fi,sb,um,ud,de,ac,pt,ak,ul") ?? new List<StatisticsMedicalSpecTypeModel>();
            int count = data.Sum(r => (int?)r.Count) ?? 0;
            data = data.Take(20).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);

            if (data != null && data.Count() > 0)
            {
                builder.MoveToBookmark("data_table_07");

                #region 标题
                this.SetCellFormatAlignmentCenter(builder);
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{count}株伤口脓液标本分离菌主要菌种分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                this.SetCellBorders(builder, "Top", "Bottom");
                #endregion

                var table = builder.StartTable();

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.CellFormat.Width = 105;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("菌种名称");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("分布比例");
                builder.EndRow();
                #endregion

                #region tbody
                this.ClearCellBorders(builder);

                int rowCount = data.Count();
                int rowIndex = 1;
                foreach (var item in data)
                {
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }

                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.Write(item.Name);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();

                    builder.CellFormat.Width = 45;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Ratio > 0.00m ? item.Ratio.GetDecimal2(1) > 0.0m ? item.Ratio.GetDecimal2(1).ToString() : item.Ratio.GetDecimal2().ToString() : "0.0");
                    builder.EndRow();
                }

                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                ////没有数据
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

        }

        /// <summary>
        /// 表08. XXX株胸水标本分离菌主要菌种分布
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable8(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            //获取数据
            var data = statisticsService.GetTable03_2020(model.Id, "pf") ?? new List<StatisticsMedicalSpecTypeModel>();
            int count = data.Sum(r => (int?)r.Count) ?? 0;
            data = data.Take(20).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);

            if (data != null && data.Count() > 0)
            {
                builder.MoveToBookmark("data_table_08");

                #region 标题
                this.SetCellFormatAlignmentCenter(builder);
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{count}株胸水标本分离菌主要菌种分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                this.SetCellBorders(builder, "Top", "Bottom");
                #endregion

                var table = builder.StartTable();

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.CellFormat.Width = 105;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("菌种名称");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("分布比例");
                builder.EndRow();
                #endregion

                #region tbody
                this.ClearCellBorders(builder);

                int rowCount = data.Count();
                int rowIndex = 1;
                foreach (var item in data)
                {
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }

                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.Write(item.Name);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();

                    builder.CellFormat.Width = 45;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Ratio > 0.00m ? item.Ratio.GetDecimal2(1) > 0.0m ? item.Ratio.GetDecimal2(1).ToString() : item.Ratio.GetDecimal2().ToString() : "0.0");
                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                ////没有数据
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

        }

        /// <summary>
        /// 表09. XXX株腹水标本分离菌主要菌种分布
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable9(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            //获取数据
            var data = statisticsService.GetTable03_2020(model.Id, "ab") ?? new List<StatisticsMedicalSpecTypeModel>();
            int count = data.Sum(r => (int?)r.Count) ?? 0;
            data = data.Take(20).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (data != null && data.Count() > 0)
            {
                builder.MoveToBookmark("data_table_09");

                #region 标题
                this.SetCellFormatAlignmentCenter(builder);
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.{count}株腹水标本分离菌主要菌种分布（%）");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                //builder.Writeln("（%）");
                builder.Font.Size = 10;
                this.SetCellBorders(builder, "Top", "Bottom");
                #endregion

                var table = builder.StartTable();

                #region thead
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.InsertCell();
                builder.CellFormat.Width = 105;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.Write("菌种名称");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("分布比例");
                builder.EndRow();
                #endregion

                #region tbody
                this.ClearCellBorders(builder);


                int rowCount = data.Count();
                int rowIndex = 1;
                foreach (var item in data)
                {
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }

                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.Write(item.Name);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();

                    builder.CellFormat.Width = 45;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item.Ratio > 0.00m ? item.Ratio.GetDecimal2(1) > 0.0m ? item.Ratio.GetDecimal2(1).ToString() : item.Ratio.GetDecimal2().ToString() : "0.0");
                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                ////没有数据
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            //table.Alignment = TableAlignment.Center;
        }

        /// <summary>
        /// 表10. 金黄色葡萄球菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable10(MedicalDataWordModel model)
        {
            List<string> antcs = new List<string> { "万古霉素", "利奈唑胺", "替考拉宁", "头孢罗膦", "替加环素", "甲氧苄啶/磺胺甲噁唑", "利福平", "庆大霉素", "环丙沙星", "左氧氟沙星", "克林霉素", "红霉素", "青霉素", "苯唑西林" };
            //获取数据
            var data = new MedicalStatisticsService().GetByMedicalDataTable3(model.Id) ?? new MedicalDataWordMRSAModel();
            data.List = data.List.Where(item => antcs.Contains(item.AntibioticName)).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (data.List != null && data.List.Count() > 0)
            {
                builder.MoveToBookmark("data_table_10");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.金黄色葡萄球菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                Table table = builder.StartTable();

                #region thead

                List<string> tableHeaderData = new List<string> { $"MRSA(n={data.MrsaCount})", $"MSSA(n={data.MssaCount})" };

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");

                builder.InsertCell();
                builder.CellFormat.Width = 55;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中方式
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中方式
                builder.CellFormat.VerticalMerge = CellMerge.First; // 垂直合并单元格
                builder.Write("抗菌药物");

                foreach (var item in tableHeaderData)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Top", "Bottom");
                    builder.CellFormat.HorizontalMerge = CellMerge.First;
                    builder.Write(item);

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellBorders(builder, "Top", "Bottom");
                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellBorders(builder, "Top", "Bottom");
                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                    if (0 == tableHeaderData.IndexOf(item))
                    {
                        builder.InsertCell();
                        builder.CellFormat.Width = 5;
                        this.SetCellFormatAlignmentCenter(builder);
                        this.SetCellBorders(builder, "Top");
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.First;
                    }
                }
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 55;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                foreach (var item in tableHeaderData)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Bottom");
                    builder.Write("数量");

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Bottom");
                    builder.Write("耐药率");

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Bottom");
                    builder.Write("敏感率");

                    if (0 == tableHeaderData.IndexOf(item))
                    {
                        builder.InsertCell();
                        builder.CellFormat.Width = 5;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        this.SetCellFormatAlignmentCenter(builder);
                        this.SetCellBorders(builder, "Bottom");
                    }
                }

                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;


                int rowCount = data.List.Count();
                int rowIndex = 1;
                bool gapMerge = true;
                antcs.ForEach(item =>
                {
                    var itemData = data.List.Where(r => r.AntibioticName == item).FirstOrDefault();
                    if (itemData != null)
                    {
                        this.ClearCellBorders(builder);
                        if (rowCount == rowIndex++)
                        {
                            this.SetCellBorders(builder, "Bottom");
                        }

                        // 抗生素名称
                        builder.InsertCell();
                        builder.CellFormat.Width = 55;
                        builder.Font.Size = 10;
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                        builder.Write(itemData.AntibioticName);

                        // MRSA 数量
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(itemData.MRSAAntibioticCount.ToString());
                        // MRSA 耐药率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MRSAR) ? "0" : itemData.MRSAR);
                        // MRSA 敏感率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MRSAS) ? "0" : itemData.MRSAS);

                        // 分隔间隙
                        builder.InsertCell();
                        builder.CellFormat.Width = 5;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        if (gapMerge)
                        {
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            gapMerge = !gapMerge;
                        }
                        else
                        {
                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        }

                        // MSSA 数量
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(itemData.MSSAAntibioticCount.ToString());

                        // MSSA 耐药率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MSSAR) ? "0" : itemData.MSSAR);
                        // MSSA 敏感率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MSSAS) ? "0" : itemData.MSSAS);

                        builder.EndRow();
                    }
                });
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }

            #endregion

        }

        /// <summary>
        /// 表11. 凝固酶阴性葡萄球菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable11(MedicalDataWordModel model)
        {
            List<string> antcs = new List<string> { "万古霉素", "替加环素", "替考拉宁", "利奈唑胺", "利福平", "庆大霉素", "甲氧苄啶/磺胺甲噁唑", "克林霉素", "左氧氟沙星", "环丙沙星", "红霉素", "青霉素", "苯唑西林" };
            //获取数据
            var data = new MedicalStatisticsService().GetByMedicalDataTable3_2(model.Id) ?? new MedicalDataWordMRSAModel();
            data.List = data.List.Where(item => antcs.Contains(item.AntibioticName)).ToList();

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (data.List != null && data.List.Count() > 0)
            {
                builder.MoveToBookmark("data_table_11");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.凝固酶阴性葡萄球菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                Table table = builder.StartTable();

                #region thead
                List<string> tableHeaderData = new List<string> { $"MRCNS(n={data.MrcnsCount})", $"MSCNS(n={data.MscnsCount})" };

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");

                builder.InsertCell();
                builder.CellFormat.Width = 55;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中方式
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中方式
                builder.CellFormat.VerticalMerge = CellMerge.First; // 垂直合并单元格
                builder.Write("抗菌药物");

                foreach (var item in tableHeaderData)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Top", "Bottom");
                    builder.CellFormat.HorizontalMerge = CellMerge.First;
                    builder.Write(item);

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellBorders(builder, "Top", "Bottom");
                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellBorders(builder, "Top", "Bottom");
                    builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                    if (0 == tableHeaderData.IndexOf(item))
                    {
                        builder.InsertCell();
                        builder.CellFormat.Width = 5;
                        this.SetCellFormatAlignmentCenter(builder);
                        this.SetCellBorders(builder, "Top");
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.First;
                    }
                }
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 55;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                foreach (var item in tableHeaderData)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Bottom");
                    builder.Write("数量");

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Bottom");
                    builder.Write("耐药率");

                    builder.InsertCell();
                    builder.CellFormat.Width = 20;
                    this.SetCellFormatAlignmentCenter(builder);
                    this.SetCellBorders(builder, "Bottom");
                    builder.Write("敏感率");

                    if (0 == tableHeaderData.IndexOf(item))
                    {
                        builder.InsertCell();
                        builder.CellFormat.Width = 5;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        this.SetCellFormatAlignmentCenter(builder);
                        this.SetCellBorders(builder, "Bottom");
                    }
                }

                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;


                int rowCount = data.List.Count();
                int rowIndex = 1;
                bool gapMerge = true;
                antcs.ForEach(item =>
                {
                    var itemData = data.List.Where(r => r.AntibioticName == item).FirstOrDefault();
                    if (itemData != null)
                    {
                        this.ClearCellBorders(builder);
                        if (rowCount == rowIndex++)
                        {
                            this.SetCellBorders(builder, "Bottom");
                        }

                        // 抗生素名称
                        builder.InsertCell();
                        builder.CellFormat.Width = 55;
                        builder.Font.Size = 10;
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                        builder.Write(itemData.AntibioticName);

                        // MSCNS 数量
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(itemData.MSCNSAntibioticCount.ToString());
                        // MSCNS 耐药率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MRCNSR) ? "0" : itemData.MRCNSR);
                        // MSCNS 敏感率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MRCNSS) ? "0" : itemData.MRCNSS);

                        // 分隔间隙
                        builder.InsertCell();
                        builder.CellFormat.Width = 5;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        if (gapMerge)
                        {
                            builder.CellFormat.VerticalMerge = CellMerge.First;
                            gapMerge = !gapMerge;
                        }
                        else
                        {
                            builder.CellFormat.VerticalMerge = CellMerge.Previous;
                        }

                        // MSCNS 数量
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(itemData.MSCNSAntibioticCount.ToString());

                        // MSCNS 耐药率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MSCNSR) ? "0" : itemData.MSCNSR);
                        // MSCNS 敏感率
                        builder.InsertCell();
                        builder.CellFormat.Width = 20;
                        builder.Font.Size = 10;
                        builder.CellFormat.HorizontalMerge = CellMerge.None;
                        builder.CellFormat.VerticalMerge = CellMerge.None;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(string.IsNullOrWhiteSpace(itemData.MSCNSS) ? "0" : itemData.MSCNSS);

                        builder.EndRow();
                    }
                });
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }

            #endregion

        }

        /// <summary>
        /// 表12. 屎肠球菌和粪肠球菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable12(MedicalDataWordModel model)
        {
            #region 获取数据
            /**
             * 屎肠球菌: 996
             * 粪肠球菌: 908
             */
            List<string> antcs = new List<string> { "替考拉宁", "万古霉素", "替加环素", "呋喃妥因", "利奈唑胺", "氨苄西林", "磷霉素", "氯霉素", "高浓度链霉素", "左氧氟沙星", "环丙沙星", "高浓度庆大霉素", "利福平", "红霉素" };
            var _996data = model.AntibioticResultList.Where(r => r.OrganismId == 996).ToList() ?? new List<GetAntibioticResultModel>();
            var _908data = model.AntibioticResultList.Where(r => r.OrganismId == 908).ToList() ?? new List<GetAntibioticResultModel>();
            _996data = _996data.Where(item => antcs.Contains(item.AntibioticName)).OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            _908data = _908data.Where(item => antcs.Contains(item.AntibioticName)).OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            DataTable dataTable = new DataTable("Table");
            dataTable.Columns.Add(new DataColumn("_996_Name", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("_996_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("_996_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("_996_S", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("_908_Name", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("_908_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("_908_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("_908_S", Type.GetType("System.String")));

            antcs.ForEach(item =>
            {
                var _996item = _996data.Where(r => r.AntibioticName == item).FirstOrDefault();
                var _908item = _908data.Where(r => r.AntibioticName == item).FirstOrDefault();
                if (_996item != null || _908item != null)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["_996_Name"] = _996data.Count() > _908data.Count() ? ((_996item?.AntibioticName) ?? "") : ((_908item?.AntibioticName) ?? "");
                    dataRow["_996_Count"] = _996item != null ? _996item.DataCount.ToString() : "";
                    dataRow["_996_R"] = _996item != null ? _996item.ResistanceRatio.GetDecimal2(1).ToString() : "";
                    dataRow["_996_S"] = _996item != null ? _996item.SensitiveRatio.GetDecimal2(1).ToString() : "";
                    dataRow["_908_Name"] = (_908item?.AntibioticName) ?? "";
                    dataRow["_908_Count"] = _908item != null ? _908item.DataCount.ToString() : "";
                    dataRow["_908_R"] = _908item != null ? _908item.ResistanceRatio.GetDecimal2(1).ToString() : "";
                    dataRow["_908_S"] = _908item != null ? _908item.SensitiveRatio.GetDecimal2(1).ToString() : "";
                    dataTable.Rows.Add(dataRow);
                }
            });

            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                builder.MoveToBookmark("data_table_12");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.屎肠球菌和粪肠球菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中方式
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中方式
                builder.CellFormat.VerticalMerge = CellMerge.First; // 垂直合并单元格
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                // 
                try
                {
                    builder.Write($"屎肠球菌(n={(int.Parse(item_count.Single(x => x.Key == "屎肠球菌").Value))})");

                }
                catch (Exception)
                {
                    builder.Write($"屎肠球菌(n={(_996data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中方式
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中方式
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.First; // 垂直合并单元格
                                                                    //builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //builder.Write($"粪肠球菌(n={(_908data.Sum(r => (int?)r.DataCount)) ?? 0})");
                try
                {
                    builder.Write($"粪肠球菌(n={(int.Parse(item_count.Single(x => x.Key == "粪肠球菌").Value))})");

                }
                catch (Exception)
                {
                    builder.Write($"粪肠球菌(n={(_908data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");
                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;


                int rowCount = dataTable.Rows.Count;
                int rowIndex = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["_996_Name"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["_996_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["_996_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["_996_S"].ToString());

                    // 分隔间隙
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    //builder.Write(item["_908_Name"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["_908_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["_908_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["_908_S"].ToString());

                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 235;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion


            #endregion
        }

        /// <summary>
        /// 表13. 患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable13(MedicalDataWordModel model)
        {
            #region 获取数据
            List<string> antcs = new List<string> { "青霉素", "万古霉素", "利奈唑胺", "红霉素", "克林霉素", "甲氧苄啶/磺胺甲噁唑", "左氧氟沙星", "莫西沙星", "氯霉素" };

            // 肺炎链球菌: 493
            var _493data = model.AntibioticResultList.Where(r => r.OrganismId == 493).ToList() ?? new List<GetAntibioticResultModel>();
            _493data = _493data.Where(item => antcs.Contains(item.AntibioticName)).OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (_493data.Count > 0)
            {
                builder.MoveToBookmark("data_table_13");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.患者中非脑膜炎肺炎链球菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead
                this.SetCellBorders(builder, "Top", "Bottom");

                builder.InsertCell();
                builder.CellFormat.Width = 60;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.Write("抗菌药物");


                builder.InsertCell();
                builder.CellFormat.Width = 40;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("数量");


                builder.InsertCell();
                builder.CellFormat.Width = 40;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("耐药率");


                builder.InsertCell();
                builder.CellFormat.Width = 40;
                this.SetCellFormatAlignmentCenter(builder);
                builder.Write("敏感率");

                builder.EndRow();
                #endregion

                #region tbody

                int rowCount = _493data.Count();
                int rowIndex = 1;

                antcs.ForEach(item =>
                {
                    var itemData = _493data.Where(r => r.AntibioticName == item).FirstOrDefault();
                    if (itemData != null)
                    {
                        this.ClearCellBorders(builder);
                        if (rowCount == rowIndex++)
                        {
                            this.SetCellBorders(builder, "Bottom");
                        }

                        builder.InsertCell();
                        builder.CellFormat.Width = 60;
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                        builder.Write(itemData.AntibioticName);


                        builder.InsertCell();
                        builder.CellFormat.Width = 40;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(itemData.DataCount.ToString());


                        builder.InsertCell();
                        builder.CellFormat.Width = 40;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(itemData.ResistanceRatio.GetDecimal2(1).ToString());


                        builder.InsertCell();
                        builder.CellFormat.Width = 40;
                        this.SetCellFormatAlignmentCenter(builder);
                        builder.Write(itemData.SensitiveRatio.GetDecimal2(1).ToString());

                        builder.EndRow();
                    }
                });
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 180;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 表14. 大肠埃希菌和肺炎克雷伯菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable14(MedicalDataWordModel model)
        {
            #region 获取数据
            /**
             * 大肠埃希菌: 1093
             * 肺炎克雷伯菌: 535
             */
            List<string> _1093Antcs = new List<string> { "替加环素", "多黏菌素B", "亚胺培南", "美罗培南", "厄他培南", "阿米卡星", "呋喃妥因", "哌拉西林/他唑巴坦", "磷霉素", "头孢哌酮/舒巴坦", "头孢西丁", "头孢他啶", "头孢吡肟", "庆大霉素", "氨苄西林/舒巴坦", "甲氧苄啶/磺胺甲噁唑", "左氧氟沙星", "头孢呋辛", "环丙沙星", "头孢噻肟", "哌拉西林", "氨苄西林" };
            List<string> _535Antcs = new List<string> { "多黏菌素B", "替加环素", "阿米卡星", "厄他培南", "头孢西丁", "亚胺培南", "美罗培南", "哌拉西林/他唑巴坦", "头孢哌酮/舒巴坦", "庆大霉素", "左氧氟沙星", "头孢吡肟", "甲氧苄啶/磺胺甲噁唑", "头孢他啶", "环丙沙星", "头孢呋辛", "氨苄西林/舒巴坦", "哌拉西林", "头孢噻肟", "氨苄西林" };

            var _1093data = model.AntibioticResultList.Where(r => r.OrganismId == 1093 && _1093Antcs.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();
            var _535data = model.AntibioticResultList.Where(r => r.OrganismId == 535 && _535Antcs.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();
            _1093data = _1093data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            _535data = _535data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();

            DataTable dataTable = new DataTable("Table");
            dataTable.Columns.Add(new DataColumn("Column1_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_S", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_S", Type.GetType("System.String")));

            var countGrup1 = _1093data.Where(r => _1093Antcs.Contains(r.AntibioticName)).Count();
            var countGrup2 = _535data.Where(r => _535Antcs.Contains(r.AntibioticName)).Count();

            int idx = 1;
            List<dynamic> dynamicDataGrup1 = new List<dynamic>();
            foreach (var item in _1093Antcs)
            {
                if (_1093data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _1093data.First(r => r.AntibioticName == item);
                    dynamicDataGrup1.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            idx = 1;
            List<dynamic> dynamicDataGrup2 = new List<dynamic>();
            foreach (var item in _535Antcs)
            {
                if (_535data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _535data.First(r => r.AntibioticName == item);
                    dynamicDataGrup2.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            if (countGrup2 > dynamicDataGrup1.Count())
            {
                int count = dynamicDataGrup1.Count() + 1;
                for (int i = count; i <= countGrup2; i++)
                {
                    dynamicDataGrup1.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }
            else if (countGrup1 > dynamicDataGrup2.Count())
            {
                int count = dynamicDataGrup2.Count() + 1;
                for (int i = count; i <= countGrup1; i++)
                {
                    dynamicDataGrup2.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }

            var tableData = dynamicDataGrup1.Join(dynamicDataGrup2,
                 c1 => new { c1.id },
                 c2 => new { c2.id },
                 (item1, item2) => new
                 {
                     name1 = item1.name,
                     count1 = item1.count,
                     r1 = item1.r,
                     s1 = item1.s,
                     name2 = item2.name,
                     count2 = item2.count,
                     r2 = item2.r,
                     s2 = item2.s
                 });

            foreach (dynamic item in tableData)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Column1_AntibioticName"] = item.name1;
                dataRow["Column1_Count"] = item.count1;
                dataRow["Column1_R"] = item.r1;
                dataRow["Column1_S"] = item.s1;
                dataRow["Column2_AntibioticName"] = item.name2;
                dataRow["Column2_Count"] = item.count2;
                dataRow["Column2_R"] = item.r2;
                dataRow["Column2_S"] = item.s2;
                dataTable.Rows.Add(dataRow);
            }
            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                builder.MoveToBookmark("data_table_14");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.大肠埃希菌和肺炎克雷伯菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //
                try
                {
                    builder.Write($"大肠埃希菌(n={item_count.Single(x => x.Key == "大肠埃希菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"大肠埃希菌(n={(_1093data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                this.SetCellFormatAlignmentCenter(builder);
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.First;

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //
                try
                {
                    builder.Write($"肺炎克雷伯菌(n={item_count.Single(x => x.Key == "肺炎克雷伯菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"肺炎克雷伯菌(n={(_535data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");
                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;

                int rowCount = dataTable.Rows.Count;
                int rowIndex = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column1_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_S"].ToString());

                    // 分隔间隙
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column2_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_S"].ToString());

                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 235 + 45;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 表15. 奇异变形杆菌和阴沟肠杆菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable15(MedicalDataWordModel model)
        {
            #region 获取数据
            /**
             * 奇异变形杆菌: 882
             * 阴沟肠杆菌: 516
             */
            List<string> antcGroup1 = new List<string> { "头孢哌酮/舒巴坦", "厄他培南", "哌拉西林/他唑巴坦", "美罗培南", "阿米卡星", "头孢西丁", "头孢吡肟", "头孢他啶", "亚胺培南", "庆大霉素", "哌拉西林", "左氧氟沙星", "氨苄西林/舒巴坦", "头孢噻肟", "环丙沙星", "头孢呋辛", "甲氧苄啶/磺胺甲噁唑", "氨苄西林", "头孢唑林" };
            List<string> antcGroup2 = new List<string> { "阿米卡星", "多黏菌素B", "替加环素", "亚胺培南", "美罗培南", "左氧氟沙星", "庆大霉素", "厄他培南", "环丙沙星", "头孢哌酮/舒巴坦", "头孢吡肟", "哌拉西林/他唑巴坦", "甲氧苄啶/磺胺甲噁唑", "头孢他啶", "哌拉西林", "头孢噻肟", "头孢呋辛", "氨苄西林/舒巴坦", "氨苄西林", "头孢西丁" };
            var _882data = model.AntibioticResultList.Where(r => r.OrganismId == 882 && antcGroup1.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();
            var _516data = model.AntibioticResultList.Where(r => r.OrganismId == 516 && antcGroup2.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();

            _882data = _882data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            _516data = _516data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();

            DataTable dataTable = new DataTable("Table");
            dataTable.Columns.Add(new DataColumn("Column1_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_S", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_S", Type.GetType("System.String")));

            var countGrup1 = _882data.Where(r => antcGroup1.Contains(r.AntibioticName)).Count();
            var countGrup2 = _516data.Where(r => antcGroup2.Contains(r.AntibioticName)).Count();

            int idx = 1;
            List<dynamic> dynamicDataGrup1 = new List<dynamic>();
            foreach (var item in antcGroup1)
            {
                if (_882data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _882data.First(r => r.AntibioticName == item);
                    dynamicDataGrup1.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            idx = 1;
            List<dynamic> dynamicDataGrup2 = new List<dynamic>();
            foreach (var item in antcGroup2)
            {
                if (_516data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _516data.First(r => r.AntibioticName == item);
                    dynamicDataGrup2.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            if (countGrup2 > dynamicDataGrup1.Count())
            {
                int count = dynamicDataGrup1.Count() + 1;
                for (int i = count; i <= countGrup2; i++)
                {
                    dynamicDataGrup1.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }
            else if (countGrup1 > dynamicDataGrup2.Count())
            {
                int count = dynamicDataGrup2.Count() + 1;
                for (int i = count; i <= countGrup1; i++)
                {
                    dynamicDataGrup2.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }

            var tableData = dynamicDataGrup1.Join(dynamicDataGrup2,
                 c1 => new { c1.id },
                 c2 => new { c2.id },
                 (item1, item2) => new
                 {
                     name1 = item1.name,
                     count1 = item1.count,
                     r1 = item1.r,
                     s1 = item1.s,
                     name2 = item2.name,
                     count2 = item2.count,
                     r2 = item2.r,
                     s2 = item2.s
                 });

            foreach (dynamic item in tableData)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Column1_AntibioticName"] = item.name1;
                dataRow["Column1_Count"] = item.count1;
                dataRow["Column1_R"] = item.r1;
                dataRow["Column1_S"] = item.s1;
                dataRow["Column2_AntibioticName"] = item.name2;
                dataRow["Column2_Count"] = item.count2;
                dataRow["Column2_R"] = item.r2;
                dataRow["Column2_S"] = item.s2;
                dataTable.Rows.Add(dataRow);
            }
            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                builder.MoveToBookmark("data_table_15");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.奇异变形杆菌和阴沟肠杆菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
               // 
                try
                {
                    builder.Write($"奇异变形杆菌(n={item_count.Single(x => x.Key == "奇异变形杆菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"奇异变形杆菌(n={(_882data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                this.SetCellFormatAlignmentCenter(builder);
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.First;

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //
                try
                {
                    builder.Write($"阴沟肠杆菌(n={item_count.Single(x => x.Key == "阴沟肠杆菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"阴沟肠杆菌(n={(_516data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");
                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;

                int rowCount = dataTable.Rows.Count;
                int rowIndex = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column1_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_S"].ToString());

                    // 分隔间隙
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column2_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_S"].ToString());

                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 235 + 45;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 表16. 黏质沙雷菌和弗劳地柠檬酸杆菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable16(MedicalDataWordModel model)
        {
            #region 获取数据
            /**
             * 黏质沙雷菌: 601
             * 弗劳地柠檬酸杆菌: 460
             */
            List<string> antcGroup1 = new List<string> { "替加环素", "阿米卡星", "甲氧苄啶/磺胺甲噁唑", "哌拉西林/他唑巴坦", "美罗培南", "头孢他啶", "厄他培南", "亚胺培南", "左氧氟沙星", "头孢吡肟", "头孢哌酮/舒巴坦", "庆大霉素", "环丙沙星", "哌拉西林", "头孢噻肟", "头孢西丁", "氨苄西林/舒巴坦", "氨苄西林", "头孢呋辛" };
            List<string> antcGroup2 = new List<string> { "多黏菌素B", "替加环素", "阿米卡星", "厄他培南", "美罗培南", "亚胺培南", "头孢哌酮/舒巴坦", "头孢吡肟", "哌拉西林/他唑巴坦", "左氧氟沙星", "庆大霉素", "环丙沙星", "甲氧苄啶/磺胺甲噁唑", "头孢他啶", "头孢噻肟", "哌拉西林", "头孢呋辛", "氨苄西林/舒巴坦", "头孢西丁", "氨苄西林" };
            var _601data = model.AntibioticResultList.Where(r => r.OrganismId == 601 && antcGroup1.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();
            var _460data = model.AntibioticResultList.Where(r => r.OrganismId == 460 && antcGroup2.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();
            _601data = _601data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            _460data = _460data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();

            DataTable dataTable = new DataTable("Table");
            dataTable.Columns.Add(new DataColumn("Column1_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_S", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_S", Type.GetType("System.String")));

            var countGrup1 = _601data.Where(r => antcGroup1.Contains(r.AntibioticName)).Count();
            var countGrup2 = _460data.Where(r => antcGroup2.Contains(r.AntibioticName)).Count();

            int idx = 1;
            List<dynamic> dynamicDataGrup1 = new List<dynamic>();
            foreach (var item in antcGroup1)
            {
                if (_601data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _601data.First(r => r.AntibioticName == item);
                    dynamicDataGrup1.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            idx = 1;
            List<dynamic> dynamicDataGrup2 = new List<dynamic>();
            foreach (var item in antcGroup2)
            {
                if (_460data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _460data.First(r => r.AntibioticName == item);
                    dynamicDataGrup2.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            if (countGrup2 > dynamicDataGrup1.Count())
            {
                int count = dynamicDataGrup1.Count() + 1;
                for (int i = count; i <= countGrup2; i++)
                {
                    dynamicDataGrup1.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }
            else if (countGrup1 > dynamicDataGrup2.Count())
            {
                int count = dynamicDataGrup2.Count() + 1;
                for (int i = count; i <= countGrup1; i++)
                {
                    dynamicDataGrup2.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }

            var tableData = dynamicDataGrup1.Join(dynamicDataGrup2,
                 c1 => new { c1.id },
                 c2 => new { c2.id },
                 (item1, item2) => new
                 {
                     name1 = item1.name,
                     count1 = item1.count,
                     r1 = item1.r,
                     s1 = item1.s,
                     name2 = item2.name,
                     count2 = item2.count,
                     r2 = item2.r,
                     s2 = item2.s
                 });

            foreach (dynamic item in tableData)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Column1_AntibioticName"] = item.name1;
                dataRow["Column1_Count"] = item.count1;
                dataRow["Column1_R"] = item.r1;
                dataRow["Column1_S"] = item.s1;
                dataRow["Column2_AntibioticName"] = item.name2;
                dataRow["Column2_Count"] = item.count2;
                dataRow["Column2_R"] = item.r2;
                dataRow["Column2_S"] = item.s2;
                dataTable.Rows.Add(dataRow);
            }
            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                builder.MoveToBookmark("data_table_16");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.黏质沙雷菌和弗劳地柠檬酸杆菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                // builder.Write($"黏质沙雷菌(n={(_601data.Sum(r => (int?)r.DataCount)) ?? 0})");
                try
                {
                    builder.Write($"黏质沙雷菌(n={item_count.Single(x => x.Key == "黏质沙雷菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"黏质沙雷菌(n={(_601data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                this.SetCellFormatAlignmentCenter(builder);
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.First;

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //
                try
                {
                    builder.Write($"弗劳地柠檬酸杆菌(n={item_count.Single(x => x.Key == "弗劳地柠檬酸杆菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"弗劳地柠檬酸杆菌(n={(_460data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }


                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");
                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;

                int rowCount = dataTable.Rows.Count;
                int rowIndex = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column1_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_S"].ToString());

                    // 分隔间隙
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column2_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_S"].ToString());

                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 235 + 45;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 表17. 铜绿假单胞菌和鲍曼不动杆菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable17(MedicalDataWordModel model)
        {
            #region 获取数据
            /**
             * 铜绿假单胞菌: 732
             * 鲍曼不动杆菌: 1007
             */
            List<string> antcGroup1 = new List<string> { "多黏菌素B", "阿米卡星", "庆大霉素", "头孢吡肟", "头孢哌酮/舒巴坦", "哌拉西林/他唑巴坦", "环丙沙星", "头孢他啶", "左氧氟沙星", "哌拉西林", "美罗培南", "头孢哌酮", "氨曲南", "亚胺培南", "替卡西林/克拉维酸" };
            List<string> antcGroup2 = new List<string> { "多黏菌素B", "替加环素", "米诺环素", "头孢哌酮/舒巴坦", "甲氧苄啶/磺胺甲噁唑", "阿米卡星", "左氧氟沙星", "庆大霉素", "氨苄西林/舒巴坦", "头孢吡肟", "亚胺培南", "头孢他啶", "环丙沙星", "美罗培南", "哌拉西林/他唑巴坦", "哌拉西林" };
            var _732data = model.AntibioticResultList.Where(r => r.OrganismId == 732 && antcGroup1.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();
            var _1007data = model.AntibioticResultList.Where(r => r.OrganismId == 1007 && antcGroup2.Contains(r.AntibioticName)).ToList() ?? new List<GetAntibioticResultModel>();

            _732data = _732data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            _1007data = _1007data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();

            DataTable dataTable = new DataTable("Table");
            dataTable.Columns.Add(new DataColumn("Column1_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_S", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_S", Type.GetType("System.String")));
            var countGrup1 = _732data.Where(r => antcGroup1.Contains(r.AntibioticName)).Count();
            var countGrup2 = _1007data.Where(r => antcGroup2.Contains(r.AntibioticName)).Count();

            int idx = 1;
            List<dynamic> dynamicDataGrup1 = new List<dynamic>();
            foreach (var item in antcGroup1)
            {
                if (_732data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _732data.First(r => r.AntibioticName == item);
                    dynamicDataGrup1.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            idx = 1;
            List<dynamic> dynamicDataGrup2 = new List<dynamic>();
            foreach (var item in antcGroup2)
            {
                if (_1007data.Any(r => r.AntibioticName == item))
                {
                    var itemData = _1007data.First(r => r.AntibioticName == item);
                    dynamicDataGrup2.Add(new
                    {
                        id = idx++,
                        name = itemData.AntibioticName,
                        count = itemData.DataCount.ToString(),
                        r = itemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = itemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            if (countGrup2 > dynamicDataGrup1.Count())
            {
                int count = dynamicDataGrup1.Count() + 1;
                for (int i = count; i <= countGrup2; i++)
                {
                    dynamicDataGrup1.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }
            else if (countGrup1 > dynamicDataGrup2.Count())
            {
                int count = dynamicDataGrup2.Count() + 1;
                for (int i = count; i <= countGrup1; i++)
                {
                    dynamicDataGrup2.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }

            var tableData = dynamicDataGrup1.Join(dynamicDataGrup2,
                 c1 => new { c1.id },
                 c2 => new { c2.id },
                 (item1, item2) => new
                 {
                     name1 = item1.name,
                     count1 = item1.count,
                     r1 = item1.r,
                     s1 = item1.s,
                     name2 = item2.name,
                     count2 = item2.count,
                     r2 = item2.r,
                     s2 = item2.s
                 });

            foreach (dynamic item in tableData)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Column1_AntibioticName"] = item.name1;
                dataRow["Column1_Count"] = item.count1;
                dataRow["Column1_R"] = item.r1;
                dataRow["Column1_S"] = item.s1;
                dataRow["Column2_AntibioticName"] = item.name2;
                dataRow["Column2_Count"] = item.count2;
                dataRow["Column2_R"] = item.r2;
                dataRow["Column2_S"] = item.s2;
                dataTable.Rows.Add(dataRow);
            }
            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                builder.MoveToBookmark("data_table_17");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.铜绿假单胞菌和鲍曼不动杆菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //
                try
                {
                    builder.Write($"铜绿假单胞菌(n={item_count.Single(x => x.Key == "铜绿假单胞菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"铜绿假单胞菌(n={(_732data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                this.SetCellFormatAlignmentCenter(builder);
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.First;

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
               // 
                try
                {
                    builder.Write($"鲍曼不动杆菌(n={item_count.Single(x => x.Key == "鲍曼不动杆菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"鲍曼不动杆菌(n={(_1007data.Sum(r => (int?)r.DataCount)) ?? 0})");
                    
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");
                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;

                int rowCount = dataTable.Rows.Count;
                int rowIndex = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column1_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_S"].ToString());

                    // 分隔间隙
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column2_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_S"].ToString());

                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 235 + 45;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 表18. 嗜麦芽窄食单胞菌和洋葱伯克霍尔德菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        private void WordToTable18(MedicalDataWordModel model)
        {
            #region 获取数据
            /**
             * 嗜麦芽窄食单胞菌: 1155
             * 洋葱伯克霍尔德菌: 1135
             */
            var _1155data = model.AntibioticResultList.Where(r => r.OrganismId == 1155).ToList() ?? new List<GetAntibioticResultModel>();
            var _1135data = model.AntibioticResultList.Where(r => r.OrganismId == 1135).ToList() ?? new List<GetAntibioticResultModel>();
            _1155data = _1155data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();
            _1135data = _1135data.OrderBy(item => item.ResistanceRatio).ThenByDescending(item => item.SensitiveRatio).ToList();

            DataTable dataTable = new DataTable("Table");
            dataTable.Columns.Add(new DataColumn("Column1_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column1_S", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_AntibioticName", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_Count", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_R", Type.GetType("System.String")));
            dataTable.Columns.Add(new DataColumn("Column2_S", Type.GetType("System.String")));

            List<string> _1155Antcs = new List<string>() { "米诺环素", "甲氧苄啶/磺胺甲噁唑", "替加环素", "左氧氟沙星", "头孢哌酮/舒巴坦", "氯霉素", "替卡西林/克拉维酸", "头孢他啶" };
            List<string> _1135Antcs = new List<string>() { "头孢他啶", "米诺环素", "甲氧苄啶/磺胺甲噁唑", "美罗培南", "左氧氟沙星", "替卡西林/克拉维酸", "氯霉素" };

            var _1155Count = _1155data.Where(r => _1155Antcs.Contains(r.AntibioticName)).Count();
            var _1135Count = _1135data.Where(r => _1135Antcs.Contains(r.AntibioticName)).Count();

            int idx = 1;
            List<dynamic> _1155Dynamic = new List<dynamic>();
            foreach (var item in _1155Antcs)
            {
                if (_1155data.Any(r => r.AntibioticName == item))
                {
                    var _1155ItemData = _1155data.First(r => r.AntibioticName == item);
                    _1155Dynamic.Add(new
                    {
                        id = idx++,
                        name = _1155ItemData.AntibioticName,
                        count = _1155ItemData.DataCount.ToString(),
                        r = _1155ItemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = _1155ItemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            idx = 1;
            List<dynamic> _1135Dynamic = new List<dynamic>();
            foreach (var item in _1135Antcs)
            {
                if (_1135data.Any(r => r.AntibioticName == item))
                {
                    var _1135ItemData = _1135data.First(r => r.AntibioticName == item);
                    _1135Dynamic.Add(new
                    {
                        id = idx++,
                        name = _1135ItemData.AntibioticName,
                        count = _1135ItemData.DataCount.ToString(),
                        r = _1135ItemData.ResistanceRatio.GetDecimal2(1).ToString(),
                        s = _1135ItemData.SensitiveRatio.GetDecimal2(1).ToString()
                    });
                }
            }

            if (_1135Count > _1155Dynamic.Count())
            {
                int count = _1155Dynamic.Count() + 1;
                for (int i = count; i <= _1135Count; i++)
                {
                    _1155Dynamic.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }
            else if (_1155Count > _1135Dynamic.Count())
            {
                int count = _1135Dynamic.Count() + 1;
                for (int i = count; i <= _1155Count; i++)
                {
                    _1135Dynamic.Add(new
                    {
                        id = i,
                        name = "",
                        count = "",
                        r = "",
                        s = ""
                    });
                }
            }

            var tableData = _1155Dynamic.Join(_1135Dynamic,
                 c1 => new { c1.id },
                 c2 => new { c2.id },
                 (item1, item2) => new
                 {
                     name1 = item1.name,
                     count1 = item1.count,
                     r1 = item1.r,
                     s1 = item1.s,
                     name2 = item2.name,
                     count2 = item2.count,
                     r2 = item2.r,
                     s2 = item2.s
                 });

            foreach (dynamic item in tableData)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Column1_AntibioticName"] = item.name1;
                dataRow["Column1_Count"] = item.count1;
                dataRow["Column1_R"] = item.r1;
                dataRow["Column1_S"] = item.s1;
                dataRow["Column2_AntibioticName"] = item.name2;
                dataRow["Column2_Count"] = item.count2;
                dataRow["Column2_R"] = item.r2;
                dataRow["Column2_S"] = item.s2;
                dataTable.Rows.Add(dataRow);
            }

            #region 合并抗菌药物为一列
            //List<string> antibioticNames = _1155data.Select(r => r.AntibioticName).Intersect(_1135data.Select(r => r.AntibioticName)).ToList();
            //antibioticNames.ForEach(item =>
            //{
            //    var _1155Item = _1155data.Where(r => r.AntibioticName == item).First();
            //    var _1135Item = _1135data.Where(r => r.AntibioticName == item).First();
            //    DataRow dataRow = dataTable.NewRow();
            //    dataRow["Column1_AntibioticName"] = item;
            //    dataRow["Column1_Count"] = _1155Item.DataCount.ToString();
            //    dataRow["Column1_R"] = _1155Item.ResistanceRatio.GetDecimal2(1).ToString();
            //    dataRow["Column1_S"] = _1155Item.SensitiveRatio.GetDecimal2(1).ToString();
            //    dataRow["Column2_AntibioticName"] = item;
            //    dataRow["Column2_Count"] = _1135Item.DataCount.ToString();
            //    dataRow["Column2_R"] = _1135Item.ResistanceRatio.GetDecimal2(1).ToString();
            //    dataRow["Column2_S"] = _1135Item.SensitiveRatio.GetDecimal2(1).ToString();
            //    dataTable.Rows.Add(dataRow);
            //});

            //_1155data.Where(r => !antibioticNames.Contains(r.AntibioticName)).ToList().ForEach(item =>
            //{
            //    DataRow dataRow = dataTable.NewRow();
            //    dataRow["Column1_AntibioticName"] = item.AntibioticName;
            //    dataRow["Column1_Count"] = item.DataCount.ToString();
            //    dataRow["Column1_R"] = item.ResistanceRatio.GetDecimal2(1).ToString();
            //    dataRow["Column1_S"] = item.SensitiveRatio.GetDecimal2(1).ToString();
            //    dataRow["Column2_AntibioticName"] = "";
            //    dataRow["Column2_Count"] = "";
            //    dataRow["Column2_R"] = "";
            //    dataRow["Column2_S"] = "";
            //    dataTable.Rows.Add(dataRow);
            //});

            //_1135data.Where(r => !antibioticNames.Contains(r.AntibioticName)).ToList().ForEach(item =>
            //{
            //    DataRow dataRow = dataTable.NewRow();
            //    dataRow["Column1_AntibioticName"] = item.AntibioticName;
            //    dataRow["Column1_Count"] = "";
            //    dataRow["Column1_R"] = "";
            //    dataRow["Column1_S"] = "";
            //    dataRow["Column2_AntibioticName"] = item.AntibioticName;
            //    dataRow["Column2_Count"] = item.DataCount.ToString();
            //    dataRow["Column2_R"] = item.ResistanceRatio.GetDecimal2(1).ToString();
            //    dataRow["Column2_S"] = item.SensitiveRatio.GetDecimal2(1).ToString();
            //    dataTable.Rows.Add(dataRow);
            //});
            #endregion

            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                builder.MoveToBookmark("data_table_18");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.嗜麦芽窄食单胞菌和洋葱伯克霍尔德菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
               // 
                try
                {
                    builder.Write($"嗜麦芽窄食单胞菌(n={item_count.Single(x => x.Key == "嗜麦芽窄食单胞菌").Value})");

                }
                catch (Exception)
                {
                    builder.Write($"嗜麦芽窄食单胞菌(n={(_1155data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                this.SetCellFormatAlignmentCenter(builder);
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.First;

                builder.InsertCell();
                builder.CellFormat.Width = 45;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                //builder.Write($"洋葱伯克霍尔德菌(n={(_1135data.Sum(r => (int?)r.DataCount)) ?? 0})");
                try
                {
                    //builder.Write($"洋葱伯克霍尔德菌(n={item_count.Single(x => x.Key == "洋葱伯克霍尔德菌").Value})");
                    builder.Write($"洋葱伯克霍尔德菌(n={_1135Count})");

                }
                catch (Exception)
                {
                    builder.Write($"洋葱伯克霍尔德菌(n={(_1135data.Sum(r => (int?)r.DataCount)) ?? 0})");
                }


                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;
                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 45;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 30;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");
                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;

                int rowCount = dataTable.Rows.Count;
                int rowIndex = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    this.ClearCellBorders(builder);
                    if (rowCount == rowIndex++)
                    {
                        this.SetCellBorders(builder, "Bottom");
                    }
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column1_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column1_S"].ToString());

                    // 分隔间隙
                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 45;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item["Column2_AntibioticName"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_Count"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_R"].ToString());

                    builder.InsertCell();
                    builder.Font.Size = 10;
                    builder.CellFormat.Width = 30;
                    this.SetCellFormatAlignmentCenter(builder);
                    builder.Write(item["Column2_S"].ToString());

                    builder.EndRow();
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 235 + 45;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 表19. 流感嗜血杆菌对抗菌药物的耐药率和敏感率
        /// </summary>
        /// <param name="model"></param>
        /// <param name="statisticsService"></param>
        private void WordToTable19(MedicalDataWordModel model, MedicalStatisticsService statisticsService)
        {
            #region 获取数据
            var data = statisticsService.GetMedicalDataTableByhin(model.Id);
            #endregion

            DocumentBuilder builder = new DocumentBuilder(model.Doc);
            if (data.Item2.Any())
            {
                builder.MoveToBookmark("data_table_19");

                #region 标题
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.RowFormat.Height = 20;
                builder.Writeln($"表{table_index}.流感嗜血杆菌对抗菌药物的耐药率和敏感率（%）");

                builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Font.Size = 10;
                //builder.Writeln("（%）");
                #endregion

                #region table
                Table table = builder.StartTable();

                #region thead

                #region row 1
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.InsertCell();
                builder.CellFormat.Width = 50;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.Write("抗菌药物");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.Write($"合计(n={data.Item1["合计"]})");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                this.SetCellFormatAlignmentCenter(builder);
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.First;

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.Write($"成人(n={data.Item1["成人"]})");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 10;
                this.SetCellFormatAlignmentCenter(builder);
                builder.CellFormat.HorizontalMerge = CellMerge.None;

                builder.CellFormat.VerticalMerge = CellMerge.First;
                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.Write($"儿童(n={data.Item1["儿童"]})");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                this.SetCellBorders(builder, "Top", "Bottom");
                builder.CellFormat.VerticalMerge = CellMerge.None;
                builder.CellFormat.HorizontalMerge = CellMerge.Previous;

                builder.EndRow();
                #endregion

                #region row 2
                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 50;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.InsertCell();
                builder.Font.Size = 9;
                builder.CellFormat.Width = 10;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.Previous;

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("数量");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("耐药率");

                builder.InsertCell();
                builder.CellFormat.Width = 20;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;
                this.SetCellFormatAlignmentCenter(builder);
                this.SetCellBorders(builder, "Bottom");
                builder.Write("敏感率");

                builder.EndRow();
                #endregion

                #endregion

                #region tbody
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.HorizontalMerge = CellMerge.None;
                builder.CellFormat.VerticalMerge = CellMerge.None;

                // TMP-SMZ：甲氧苄啶/磺胺甲噁唑
                List<string> dataItem = new List<string>() { "氨苄西林", "阿莫西林/克拉维酸", "氨苄西林/舒巴坦", "头孢呋辛", "头孢曲松", "氯霉素", "阿奇霉素", "左氧氟沙星", "甲氧苄啶/磺胺甲噁唑", "美罗培南" };
                int rowCount = data.Item2.Where(r => dataItem.Contains(r.Key)).Count();
                int rowIndex = 1;

                foreach (var item in dataItem)
                {
                    if (data.Item2.ContainsKey(item))
                    {
                        this.ClearCellBorders(builder);
                        if (rowCount == rowIndex++)
                        {
                            this.SetCellBorders(builder, "Bottom");
                        }

                        builder.InsertCell();
                        builder.CellFormat.Width = 50;
                        builder.Font.Size = 10;
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                        builder.Write(item);

                        foreach (string node in data.Item2[item])
                        {
                            builder.InsertCell();
                            builder.Font.Size = 10;
                            builder.CellFormat.Width = 20;
                            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                            builder.Write(node);

                            if (data.Item2[item].IndexOf(node) == 2 || data.Item2[item].IndexOf(node) == 5)
                            {
                                // 分隔间隙
                                builder.InsertCell();
                                builder.Font.Size = 10;
                                builder.CellFormat.Width = 10;
                                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                            }
                        }

                        builder.EndRow();
                    }
                }
                table.Alignment = TableAlignment.Center;
                table_index++;
                builder.EndTable();

            }
            else
            {
                //builder.InsertCell();
                //builder.CellFormat.Width = 250;
                //builder.CellFormat.HorizontalMerge = CellMerge.First;
                //this.SetCellFormatAlignmentCenter(builder);
                //this.SetCellBorders(builder, "Bottom");
                //builder.Write("没有数据");
                //builder.EndRow();
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 设置word表格边框
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="borders">Top(或T),Right(或R),Bottom(或B),Left(或L)</param>
        private void SetCellBorders(DocumentBuilder builder, params string[] borders)
        {
            builder.CellFormat.Borders.Color = Color.Transparent;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.LineWidth = 0;

            if (borders == null || borders.Count() == 0)
            {
                builder.CellFormat.Borders.Color = Color.Gray;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.LineWidth = 1;
                return;
            }

            foreach (var item in borders)
            {
                switch (item.ToUpper().Trim())
                {
                    case "T":
                    case "TOP":
                        builder.CellFormat.Borders.Top.Color = Color.Gray;
                        builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
                        builder.CellFormat.Borders.Top.LineWidth = 1;
                        break;
                    case "R":
                    case "RIGHT":
                        builder.CellFormat.Borders.Right.Color = Color.Gray;
                        builder.CellFormat.Borders.Right.LineStyle = LineStyle.Single;
                        builder.CellFormat.Borders.Right.LineWidth = 1;
                        break;
                    case "B":
                    case "BOTTOM":
                        builder.CellFormat.Borders.Bottom.Color = Color.Gray;
                        builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
                        builder.CellFormat.Borders.Bottom.LineWidth = 1;
                        break;
                    case "L":
                    case "LEFT":
                        builder.CellFormat.Borders.Left.Color = Color.Gray;
                        builder.CellFormat.Borders.Left.LineStyle = LineStyle.Single;
                        builder.CellFormat.Borders.Left.LineWidth = 1;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 取消word表格边框
        /// </summary>
        /// <param name="builder"></param>
        private void ClearCellBorders(DocumentBuilder builder)
        {
            builder.CellFormat.Borders.Color = Color.Transparent;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
        }

        /// <summary>
        /// 设置word表格单元格内容水平垂直居中
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="alignments"></param>
        private void SetCellFormatAlignmentCenter(DocumentBuilder builder, params string[] alignments)
        {
            if (alignments == null || alignments.Count() == 0)
            {
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center; // 水平居中
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中
            }
            else
            {
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中
            }
        }
        #endregion

        #region  基础函数

        /// <summary>
        /// 标本类型的统计，基础函数
        /// </summary>
        /// <param name="organismId"></param>
        /// <param name="model"></param>
        /// <param name="title"></param>
        /// <param name="builder"></param>
        private void SpecTypeData(string type, MedicalDataWordModel model, string title, DocumentBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(type))
            {
                type = type.Trim().Trim(',').Trim();
            }
            else
            {
                type = "";
            }
            List<System.Data.SqlClient.SqlParameter> sqlParameters = new List<System.Data.SqlClient.SqlParameter>();
            List<string> specTypeList = type.Split(',').Distinct().ToList();
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            specTypeList.ForEach(item =>
            {
                item = item.Trim();
                stringBuilder.Append($"@{item}, ");
                sqlParameters.Add(new System.Data.SqlClient.SqlParameter($"@{item}", item));
            });
            var data = new MedicalStatisticsService().GetByBacteriaInSpecimenRatio(model.Id, stringBuilder.ToString().Trim().TrimEnd(','), sqlParameters.ToArray());

            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
            builder.RowFormat.Height = 20;
            //builder.Writeln(((data == null || !data.Any()) ? 0 : data.Sum(m => m.Count)) + "株" + title);
            builder.Writeln(title);
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Writeln("（%）");
            builder.Font.Size = 11;
            builder.CellFormat.Borders.Top.Color = Color.Black;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Top.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Top.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;

            //表头
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            builder.InsertCell();
            builder.CellFormat.Width = 105;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("标本名称");
            builder.InsertCell();
            builder.CellFormat.Width = 30;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write("数量");
            builder.InsertCell();
            builder.CellFormat.Width = 45;
            builder.Write("分布比例");
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.EndRow();

            //数据
            builder.CellFormat.Borders.Color = Color.White;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.Bottom.LineWidth = 1;
            builder.CellFormat.Borders.Bottom.Color = Color.Gray;
            builder.CellFormat.Borders.Bottom.LineStyle = LineStyle.Single;

            if (data == null || !data.Any())
            {
                //没有数据
                builder.InsertCell();
                builder.CellFormat.Width = 180;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = Color.Black;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Write("没有数据");
                builder.EndRow();
            }
            else
            {
                foreach (var item in data)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 105;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item.Name);
                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.Count.ToString());
                    builder.InsertCell();
                    builder.CellFormat.Width = 45;
                    builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.Ratio.GetDecimal2(1).ToString(""));
                    builder.EndRow();
                }
            }

        }

        /// <summary>
        /// 单个细菌针对药物的耐药性统计，基础函数
        /// </summary>
        /// <param name="organismId"></param>
        /// <param name="model"></param>
        /// <param name="title"></param>
        /// <param name="builder"></param>
        private void SingleOrganismData(long organismId, MedicalDataWordModel model, string title, DocumentBuilder builder)
        {
            var data = model.AntibioticResultList.Where(r => r.OrganismId == organismId).ToList() ?? new List<GetAntibioticResultModel>();
            int nullWidth = 50 + 30 + 40 + 40;
            #region 标题

            builder.InsertCell();
            builder.CellFormat.Width = nullWidth;
            builder.Font.Size = 11;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.LineWidth = 0;
            builder.CellFormat.Borders.Color = Color.Transparent;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.Write(title);
            builder.EndRow();

            builder.InsertCell();
            builder.CellFormat.Width = nullWidth;
            builder.Font.Size = 10;
            builder.CellFormat.Borders.LineStyle = LineStyle.None;
            builder.CellFormat.Borders.LineWidth = 0;
            builder.CellFormat.Borders.Color = Color.Transparent;
            builder.CellFormat.HorizontalMerge = CellMerge.First;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Write("（%）");
            builder.EndRow();

            #endregion

            #region 表头

            #region 第一行
            builder.InsertCell();
            builder.CellFormat.Borders.Color = Color.Gray;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.Width = 50;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Left; // 水平居中
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center; // 垂直居中
            builder.Write("抗生素");

            builder.InsertCell();
            builder.CellFormat.Width = 30;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("数量");

            builder.InsertCell();
            builder.CellFormat.Width = 40;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("耐药率");

            builder.InsertCell();
            builder.CellFormat.Width = 40;
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("敏感率");

            builder.EndRow();
            #endregion

            #endregion

            #region 填充数据
            builder.CellFormat.Borders.Color = Color.Gray;
            builder.CellFormat.Borders.LineStyle = LineStyle.Single;
            builder.CellFormat.Borders.LineWidth = 1;
            builder.CellFormat.VerticalMerge = CellMerge.None;
            builder.CellFormat.HorizontalMerge = CellMerge.None;
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

            if (data != null && data.Count() > 0)
            {
                foreach (var item in data)
                {
                    builder.InsertCell();
                    builder.CellFormat.Width = 50;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    builder.Write(item.AntibioticName);

                    builder.InsertCell();
                    builder.CellFormat.Width = 30;
                    builder.Font.Size = 10;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.DataCount.ToString());

                    builder.InsertCell();
                    builder.CellFormat.Width = 40;
                    builder.Font.Size = 7;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.ResistanceRatio.ToString());

                    builder.InsertCell();
                    builder.CellFormat.Width = 40;
                    builder.Font.Size = 7;
                    builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    builder.Write(item.SensitiveRatio.ToString());

                    builder.EndRow();
                }
            }
            else
            {
                builder.InsertCell();
                builder.CellFormat.Width = nullWidth;
                builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                builder.CellFormat.Borders.Color = Color.Black;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Write("没有数据");
                builder.EndRow();
            }
            #endregion
        }

        /// <summary>
        /// 生成空行
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="rowCount"></param>
        /// <param name="cellWidth"></param>
        /// <param name="content"></param>
        /// <param name="borderColor"></param>
        private void GenerateNullRow(DocumentBuilder builder, int rowCount, double cellWidth, string content, System.Drawing.Color borderColor)
        {
            for (int i = 0; i < rowCount; i++)
            {
                builder.InsertCell();
                builder.CellFormat.Width = cellWidth;
                builder.CellFormat.Borders.LineStyle = LineStyle.None;
                builder.CellFormat.Borders.Color = borderColor;
                builder.CellFormat.HorizontalMerge = CellMerge.First;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                builder.Write(content);
                builder.EndRow();
            }
        }

        private int GetMedicalDataItemDataCount(long id, string organismCode, string antibioticCode)
        {
            if (id == 0) return 0;

            using (var conn = DapperHelper.GetConnection())
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                IEnumerable<string> antibioticColumns = conn.Query<string>("SELECT [name] FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MedicalDataItem]') AND [name] LIKE @antibioticCode;", new { antibioticCode = $"{antibioticCode}%" });
                if (antibioticColumns.Count() > 0)
                {
                    List<string> cols = new List<string>();
                    foreach (string item in antibioticColumns)
                    {
                        cols.Add($" LEN({item}) <> 0 ");
                    }

                    return conn.ExecuteScalar<int>($"SELECT COUNT(1) FROM dbo.MedicalDataItem WHERE MedicalDataId = {id} AND ORGANISM = @organismCode AND ({string.Join(" OR ", cols)}) AND [Mark] > 0;", new { organismCode = organismCode });
                }
            }
            return 0;
        }
        #endregion

    }
}
