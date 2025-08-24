using Dapper;
using ICSharpCode.SharpZipLib.Zip;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Teams;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Teams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static Dapper.SqlMapper;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalData 
    /// </summary>
    public partial class MedicalDataService : BaseService<MedicalData>, IMedicalDataService
    {
        private readonly IHospitalService HospitalService;
        private readonly ISystemLogService SystemLogService;
        private readonly IActionLogService ActionLogService;
        private readonly ISpecimenService SpecimenService;
        private readonly IBacteriaTypeService BacteriaTypeService;
        private readonly IHospitalDepartmentService HospitalDepartmentService;
        private readonly IAutoCodeService AutoCodeService;
        private readonly IMedicalDataItemService MedicalDataItemService;
        private readonly IMedicalDataItemValidateService MedicalDataItemValidateService;
        private readonly IMedicalDataWardTypeService MedicalDataWardTypeService;
        private readonly IMedicalDepartmentTypeService MedicalDepartmentTypeService;
        private readonly IMedicalSpecTypeService MedicalSpecTypeService;
        private readonly IMedicalOrganismTypeService MedicalOrganismTypeService;
        private readonly IMedicalAntibioticRuleService MedicalAntibioticRuleService;
        private readonly IMedicalOrganismService MedicalOrganismService;
        private readonly IMedicalAntibioticService MedicalAntibioticService;
        //private readonly IMedicalAntibioticResultService MedicalAntibioticResultService;
        private readonly IMedicalDataDisposeLogService MedicalDataDisposeLogService;
        private readonly IHospitalWardLocationService HospitalWardLocationService;
        private readonly ICacheManager CacheManager;
        private readonly IMedicalDataProjectService _medicalDataProjectService;
        private readonly IMedicalDataHandleLockService _medicalDataHandleLockService;
        private readonly ITeamService _teamService;

        private int DataSort = 0; //验证数据的排序

        public MedicalDataService(IRepository<MedicalData> repository,
                IHospitalService hospitalService,
                ISpecimenService specimenService,
                IBacteriaTypeService bacteriaTypeService,
                IHospitalDepartmentService hospitalDepartmentService,
                IAutoCodeService autoCodeService,
                IMedicalDataItemService medicalDataItemService,
                IMedicalDataItemValidateService medicalDataItemValidateService,
                IMedicalDataWardTypeService medicalDataWardTypeService,
                IMedicalDepartmentTypeService medicalDepartmentTypeService,
                IMedicalSpecTypeService medicalSpecTypeService,
                IMedicalOrganismTypeService medicalOrganismTypeService,
                IMedicalAntibioticRuleService medicalAntibioticRuleService,
                IMedicalOrganismService medicalOrganismService,
                IMedicalAntibioticService medicalAntibioticService,
                 //IMedicalAntibioticResultService medicalAntibioticResultService,
                 ISystemLogService systemLogService,
                 IHospitalWardLocationService hospitalWardLocationService,
                 IMedicalDataDisposeLogService medicalDataDisposeLogService,
                  ICacheManager cacheManager,
                  IActionLogService actionLogService,
                  IMedicalDataProjectService medicalDataProjectService,
                  IMedicalDataHandleLockService medicalDataHandleLockService,
                  ITeamService teamService

            ) : base(repository)
        {
            this.HospitalService = hospitalService;
            this.SpecimenService = specimenService;
            this.BacteriaTypeService = bacteriaTypeService;
            this.HospitalDepartmentService = hospitalDepartmentService;
            this.AutoCodeService = autoCodeService;
            this.MedicalDataItemService = medicalDataItemService;
            this.MedicalDataItemValidateService = medicalDataItemValidateService;
            this.MedicalDataWardTypeService = medicalDataWardTypeService;
            this.MedicalDepartmentTypeService = medicalDepartmentTypeService;
            this.MedicalSpecTypeService = medicalSpecTypeService;
            this.MedicalOrganismTypeService = medicalOrganismTypeService;
            this.MedicalAntibioticRuleService = medicalAntibioticRuleService;
            this.MedicalOrganismService = medicalOrganismService;
            this.MedicalAntibioticService = medicalAntibioticService;
            //this.MedicalAntibioticResultService = medicalAntibioticResultService;
            this.SystemLogService = systemLogService;
            this.MedicalDataDisposeLogService = medicalDataDisposeLogService;
            HospitalWardLocationService = hospitalWardLocationService;
            this.CacheManager = cacheManager;
            this.ActionLogService = actionLogService;
            _medicalDataProjectService = medicalDataProjectService;
            _medicalDataHandleLockService = medicalDataHandleLockService;
            _teamService = teamService;
        }

        /// <summary>
        ///插入数据库  使用服务完成数据分析版本
        /// </summary>
        /// <param name="entity"></param>
        public void Insert2(UploadMedicalResult uploadModel)
        {
            try
            {
                //1、添加主表的数据
                uploadModel.MedicalEntity.SN = this.AutoCodeService.GetCode(AutoCodeType.MedicalData);
                uploadModel.MedicalEntity.UploadMessage = new UploadMessageModel() { BaseMessage = uploadModel.BaseMessage }.SerializeObject();
                uploadModel.MedicalEntity.AntibioticResultStatue = (int)MedicalDataAntibioticResultStatueEnum.Wait;
                uploadModel.MedicalEntity.AntibioticResultTime = DateTime.Parse("1900-01-01 00:00");
                uploadModel.MedicalEntity.Status = (int)MedicalDataStatusEnum.Wait;
                this.Insert(uploadModel.MedicalEntity);

                //2、添加数据处理记录
                this.MedicalDataDisposeLogService.Insert(new MedicalDataDisposeLog()
                {
                    Data = "",
                    MedicalDataId = uploadModel.MedicalEntity.Id,
                    Result = "",
                    Status = (int)MedicalDataDisposeLogStatusEnum.Wait,
                    DisposeTime = DateHelper.DefaultValue()
                });

            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                throw ex;
            }
        }

        /// <summary>
        /// 获取上传文件的数据
        /// </summary>
        /// <param name="filePath">Excel绝对路径</param>
        /// <param name="errorList">记录错误数据，每行数据记录一条</param>
        /// <returns></returns>
        private List<MedicalDataItem> GetImportDataByExcel(string filePath, UploadMedicalResult resultModel)
        {
            try
            {
                System.IO.FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    Log4Helper.Info($"读取excel文件中的数据 未能读取到数据文件，文件[{filePath}]不存在。");
                    throw new Exception($"读取excel文件中的数据 ManageSystem.Services.Medicine.MedicalDataService.GetImportDataByExcel() 未能读取到数据文件，文件[{filePath}]不存在。");
                }
                DataTable table = null;
                switch (fileInfo.Extension.ToUpperInvariant())
                {
                    case ".XLS":
                        table = ImportDataTable.ExcelToDataTable(fileInfo.FullName, true);
                        break;
                    case ".XLSX":
                        table = EPPlusHelper.WorksheetToTable(fileInfo.FullName);
                        break;
                    default:
                        throw new Exception("上传文件格式不支持。");
                }

                if (table == null || table.Rows.Count <= 0)
                {
                    Log4Helper.Info($"读取excel文件中的数据 : FilePath = {filePath},  结果 : Table = null || Table.Rows.Count <= 0");
                    return null;
                }
                int rowCount = 0;

                var data = this.GetDataByTable(table, resultModel, ref rowCount);
                resultModel.BaseMessage.ReadCount = rowCount;

                return data;
            }
            catch (Exception ex)
            {
                Log4Helper.Info($"取取文件内容出错,GetImportDataByExcel,{ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// 读取dbf文件中的数据 如所在目录为E:\manman\aa.dbf
        /// </summary>
        /// <param name="tableName">要读取的表名（也就是文件名）aa</param>
        /// <param name="defaultDir">文件所在的路径不包括文件名E:\manman</param>
        /// <returns></returns>
        private List<MedicalDataItem> GetImportDataByDBF(string filePath, UploadMedicalResult resultModel)
        {
            try
            {
                System.IO.FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    throw new Exception($"读取dbf文件中的数据 ManageSystem.Services.Medicine.MedicalDataService.GetImportDataByDBF() 未能读取到数据文件，文件[{filePath}]不存在。");
                }

                TDbfTable db = new TDbfTable(filePath);

                if (db == null || db.Table == null || db.Table.Rows.Count <= 0)
                {
                    Log4Helper.Info($"读取dbf文件中的数据 : FilePath = {filePath},  结果 : db.Table = null || db.Table.Rows.Count <= 0");
                    return null;
                }

                int rowCount = 0;

                var data = this.GetDataByTable(db.Table, resultModel, ref rowCount);
                resultModel.BaseMessage.ReadCount = rowCount;

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据获取到的数据 DataTable 转换成List集合 用于保存到数据库
        /// </summary>
        /// <param name="table">通过Excel 和DBF 文件获取数据</param>
        /// <param name="resultModel"></param>
        /// <returns></returns>
        private List<MedicalDataItem> GetDataByTable(DataTable table, UploadMedicalResult resultModel, ref int rowCount)
        {
            try
            {
                long zheJiangProjectId = _medicalDataProjectService.QueryEntity(r => r.Mark > 0 && r.Name == "浙江省细菌耐药监测网")?.Id ?? 0;

                List<MedicalDataItem> list = new List<MedicalDataItem>();

                for (int n = 0; n < table.Rows.Count; n++)
                {
                    var item = table.Rows[n];
                    MedicalDataItem enttiy = new MedicalDataItem();

                    try
                    {
                        enttiy.SPEC_TYPE = this.GetString(item, "SPEC_TYPE");
                        enttiy.SPEC_CODE = this.GetString(item, "SPEC_CODE");
                        enttiy.ORGANISM = this.GetString(item, "ORGANISM");
                        enttiy.COUNTRY_A = this.GetString(item, "COUNTRY_A");

                        //改行没有数据，不处理
                        if (string.IsNullOrWhiteSpace(enttiy.SPEC_TYPE) && string.IsNullOrWhiteSpace(enttiy.SPEC_CODE) &&
                            string.IsNullOrWhiteSpace(enttiy.ORGANISM) && string.IsNullOrWhiteSpace(enttiy.COUNTRY_A))
                            continue;

                        enttiy.Id = CommonHelper.GuidToLongID;
                        enttiy.UploadRowIndex = n + 2;
                        enttiy.Sort = n;
                        enttiy.IsValid = true;
                        rowCount++;

                        #region 优先删除不需要的数据

                        //1、标本类型未填写的数据不要 ，SPEC_TYPE、SPEC_CODE
                        if (string.IsNullOrWhiteSpace(enttiy.SPEC_TYPE))
                        {
                            this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "SPEC_TYPE", "", "【SPEC_TYPE】字段未填写，自动忽略该条数据");
                            continue;
                        }

                        //2、细菌未填写不的数据不要，ORGANISM
                        if (string.IsNullOrWhiteSpace(enttiy.ORGANISM))
                        {
                            this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "ORGANISM", "", "【ORGANISM】字段未填写，自动忽略该条数据");
                            continue;
                        }

                        // 20200103新规则，上传数据所属“浙江耐药监测网”时，不删除以下数据
                        //3、删除不需要被统计的细菌
                        if (zheJiangProjectId != resultModel.MedicalEntity.ProjectType && this.DeleteOrganism(enttiy.ORGANISM))
                        {
                            this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "ORGANISM", "", "【ORGANISM】 " + enttiy.ORGANISM + " 不被统计，自动忽略该条数据");
                            continue;
                        }

                        //4、剔除非无菌体液中的凝固酶阴性葡萄球菌和草绿色链球菌
                        if (this.DeleteSpecType(enttiy.SPEC_TYPE, enttiy.ORGANISM))
                        {
                            this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "ORGANISM", "", "剔除非无菌体液中的凝固酶阴性葡萄球菌和草绿色链球菌，自动忽略该条数据");
                            continue;
                        }

                        #endregion

                        enttiy.COUNTRY_A = this.GetString(item, "COUNTRY_A");
                        enttiy.LABORATORY = this.GetString(item, "LABORATORY");
                        enttiy.PATIENT_ID = this.GetString(item, "PATIENT_ID");
                        enttiy.FIRST_NAME = this.GetString(item, "FIRST_NAME");
                        enttiy.LAST_NAME = this.GetString(item, "LAST_NAME");
                        enttiy.FULL_NAME = this.GetString(item, "FULL_NAME");
                        //enttiy.FULL_NAME = string.IsNullOrWhiteSpace(enttiy.FULL_NAME) ? enttiy.FIRST_NAME + enttiy.LAST_NAME : enttiy.FULL_NAME;
                        enttiy.SEX = this.GetString(item, "SEX");
                        enttiy.AGE = this.GetString(item, "AGE");
                        enttiy.DATE_BIRTH = this.GetString(item, "DATE_BIRTH");

                        //重新设置医院的科室、部门等编码，
                        this.ReplaceHospitalWard(item, enttiy, resultModel);

                        enttiy.INSTITUT = this.GetString(item, "INSTITUT");
                        enttiy.SPEC_NUM = this.GetString(item, "SPEC_NUM");
                        enttiy.SPEC_DATE = this.GetString(item, "SPEC_DATE");

                        #region 验证并自动修复国家  COUNTRY_A  
                        /**
                         * 非必填字段，最长3个字符。若填写，则其必须为“CHN”，若未填写，数据上传后系统会自动补充。
                         */
                        if (string.IsNullOrWhiteSpace(enttiy.COUNTRY_A))
                        {
                            enttiy.COUNTRY_A = "CHN";
                            this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "COUNTRY_A", "", "【COUNTRY_A】字段未填写值，系统已经自动设置为：CHN");
                        }
                        else if (!enttiy.COUNTRY_A.ToUpper().Equals("CHN"))
                        {
                            enttiy.COUNTRY_A = "CHN";
                            this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Warning, "COUNTRY_A", "", $"【COUNTRY_A】填写值不正确，系统已经自动设置为：CHN");
                        }
                        #endregion

                        #region 验证送检日期（标本日期） SPEC_DATE，送检日期验证提前，方便后期对日期排序处理 2020-01-13 新加
                        /**
                         * 本字段为必填项，最低校验通过率为100%！
                         * 所填写的日期范围为：上报数据的季度的上一个月的1号到该季度末的最后一天，例如：上报的数据是2014年第二季度的数据，
                         * 则该字段可接受的日期范围为（2014年3月1日到6月30日），四个月的时间比该季度的日期范围（2014年4月1日到6月30日）向前放宽了一个月。
                         */
                        //if (string.IsNullOrWhiteSpace(enttiy.SPEC_DATE))
                        //{
                        //    enttiy.SPEC_DATE = "2000/01/01";
                        //    // 无效的日期格式
                        //    this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "SPEC_DATE", "", $"【SPEC_DATE】字段必须填写且必须为日期格式");
                        //   // continue;
                        //}

                        TimeSpan ts = Tools.DateDiff(DateTime.Now, new DateTime(1900, 1, 1, 0, 0, 0, 0));
                        if (enttiy.SPEC_DATE.IsDateTime(out DateTime _dateTime) && _dateTime.Year > 1900)
                        {
                            enttiy.SPEC_DATE = _dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else if (!string.IsNullOrWhiteSpace(enttiy.SPEC_DATE) && enttiy.SPEC_DATE.IsDouble(out double _value) && _value > 32872 && _value <= ts.TotalDays)
                        {
                            enttiy.SPEC_DATE = new DateTime(1900, 1, 1, 0, 0, 0, 0).AddDays(_value).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            enttiy.SPEC_DATE = "1900-01-01 00:00:00.000";
                            // 无效的日期格式
                            this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "SPEC_DATE", "", $"【SPEC_DATE】字段必填且必须为合理的日期格式");
                            // continue;
                        }
                        #endregion

                        enttiy.ORG_TYPE = this.GetString(item, "ORG_TYPE");
                        enttiy.ESBL = this.GetString(item, "ESBL");
                        enttiy.BETA_LACT = this.GetString(item, "BETA_LACT");
                        enttiy.AMK_ND30 = this.GetString(item, "AMK_ND30");
                        enttiy.AMC_ND20 = this.GetString(item, "AMC_ND20");
                        enttiy.AZM_ND15 = this.GetString(item, "AZM_ND15");
                        enttiy.AMP_ND10 = this.GetString(item, "AMP_ND10");
                        enttiy.SAM_ND10 = this.GetString(item, "SAM_ND10");
                        enttiy.ATM_ND30 = this.GetString(item, "ATM_ND30");
                        enttiy.OXA_ND1 = this.GetString(item, "OXA_ND1");
                        enttiy.POL_ND300 = this.GetString(item, "POL_ND300");
                        enttiy.NIT_ND300 = this.GetString(item, "NIT_ND300");
                        enttiy.SXT_ND1_2 = this.GetString(item, "SXT_ND1_2");
                        enttiy.STH_ND300 = this.GetString(item, "STH_ND300");
                        enttiy.GEH_ND120 = this.GetString(item, "GEH_ND120");
                        enttiy.ERY_ND15 = this.GetString(item, "ERY_ND15");
                        enttiy.CIP_ND5 = this.GetString(item, "CIP_ND5");
                        enttiy.CLI_ND2 = this.GetString(item, "CLI_ND2");
                        enttiy.RIF_ND5 = this.GetString(item, "RIF_ND5");
                        enttiy.LNZ_ND30 = this.GetString(item, "LNZ_ND30");
                        enttiy.STR_ND10 = this.GetString(item, "STR_ND10");
                        enttiy.FOS_ND200 = this.GetString(item, "FOS_ND200");
                        enttiy.CHL_ND30 = this.GetString(item, "CHL_ND30");
                        enttiy.MEM_ND10 = this.GetString(item, "MEM_ND10");
                        enttiy.MNO_ND30 = this.GetString(item, "MNO_ND30");
                        enttiy.MFX_ND5 = this.GetString(item, "MFX_ND5");
                        enttiy.PIP_ND100 = this.GetString(item, "PIP_ND100");
                        enttiy.TZP_ND100 = this.GetString(item, "TZP_ND100");
                        enttiy.PEN_ND10 = this.GetString(item, "PEN_ND10");
                        enttiy.GEN_ND10 = this.GetString(item, "GEN_ND10");
                        enttiy.TCY_ND30 = this.GetString(item, "TCY_ND30");
                        enttiy.TCC_ND75 = this.GetString(item, "TCC_ND75");
                        enttiy.TIC_ND75 = this.GetString(item, "TIC_ND75");
                        enttiy.TEC_ND30 = this.GetString(item, "TEC_ND30");
                        enttiy.TGC_ND15 = this.GetString(item, "TGC_ND15");
                        enttiy.FEP_ND30 = this.GetString(item, "FEP_ND30");
                        enttiy.CXM_ND30 = this.GetString(item, "CXM_ND30");
                        enttiy.CEC_ND30 = this.GetString(item, "CEC_ND30");
                        enttiy.CFP_ND75 = this.GetString(item, "CFP_ND75");
                        enttiy.CSL_ND30 = this.GetString(item, "CSL_ND30");
                        enttiy.CRO_ND30 = this.GetString(item, "CRO_ND30");
                        enttiy.CTX_ND30 = this.GetString(item, "CTX_ND30");
                        enttiy.CAZ_ND30 = this.GetString(item, "CAZ_ND30");
                        enttiy.FOX_ND30 = this.GetString(item, "FOX_ND30");
                        enttiy.CZO_ND30 = this.GetString(item, "CZO_ND30");
                        enttiy.TOB_ND10 = this.GetString(item, "TOB_ND10");
                        enttiy.VAN_ND30 = this.GetString(item, "VAN_ND30");
                        enttiy.IPM_ND10 = this.GetString(item, "IPM_ND10");
                        enttiy.LVX_ND5 = this.GetString(item, "LVX_ND5");
                        enttiy.OFX_ND5 = this.GetString(item, "OFX_ND5");
                        enttiy.DOX_ND30 = this.GetString(item, "DOX_ND30");
                        enttiy.AMK_NM = this.GetString(item, "AMK_NM");
                        enttiy.AMC_NM = this.GetString(item, "AMC_NM");
                        enttiy.AZM_NM = this.GetString(item, "AZM_NM");
                        enttiy.AMP_NM = this.GetString(item, "AMP_NM");
                        enttiy.SAM_NM = this.GetString(item, "SAM_NM");
                        enttiy.ATM_NM = this.GetString(item, "ATM_NM");
                        enttiy.OXA_NM = this.GetString(item, "OXA_NM");
                        enttiy.POL_NM = this.GetString(item, "POL_NM");
                        enttiy.NIT_NM = this.GetString(item, "NIT_NM");
                        enttiy.SXT_NM = this.GetString(item, "SXT_NM");
                        enttiy.STH_NM = this.GetString(item, "STH_NM");
                        enttiy.GEH_NM = this.GetString(item, "GEH_NM");
                        enttiy.ERY_NM = this.GetString(item, "ERY_NM");
                        enttiy.CIP_NM = this.GetString(item, "CIP_NM");
                        enttiy.CLI_NM = this.GetString(item, "CLI_NM");
                        enttiy.RIF_NM = this.GetString(item, "RIF_NM");
                        enttiy.LNZ_NM = this.GetString(item, "LNZ_NM");
                        enttiy.STR_NM = this.GetString(item, "STR_NM");
                        enttiy.FOS_NM = this.GetString(item, "FOS_NM");
                        enttiy.CHL_NM = this.GetString(item, "CHL_NM");
                        enttiy.MEM_NM = this.GetString(item, "MEM_NM");
                        enttiy.MNO_NM = this.GetString(item, "MNO_NM");
                        enttiy.MFX_NM = this.GetString(item, "MFX_NM");
                        enttiy.PIP_NM = this.GetString(item, "PIP_NM");
                        enttiy.TZP_NM = this.GetString(item, "TZP_NM");
                        enttiy.PEN_NM = this.GetString(item, "PEN_NM");
                        enttiy.GEN_NM = this.GetString(item, "GEN_NM");
                        enttiy.PEN_NE = this.GetString(item, "PEN_NE");
                        enttiy.TCY_NM = this.GetString(item, "TCY_NM");
                        enttiy.TCC_NM = this.GetString(item, "TCC_NM");
                        enttiy.TIC_NM = this.GetString(item, "TIC_NM");
                        enttiy.TEC_NM = this.GetString(item, "TEC_NM");
                        enttiy.TGC_NM = this.GetString(item, "TGC_NM");
                        enttiy.FEP_NM = this.GetString(item, "FEP_NM");
                        enttiy.CXM_NM = this.GetString(item, "CXM_NM");
                        enttiy.CEC_NM = this.GetString(item, "CEC_NM");
                        enttiy.CFP_NM = this.GetString(item, "CFP_NM");
                        enttiy.CSL_NM = this.GetString(item, "CSL_NM");
                        enttiy.CRO_NM = this.GetString(item, "CRO_NM");
                        enttiy.CTX_NM = this.GetString(item, "CTX_NM");
                        enttiy.CAZ_NM = this.GetString(item, "CAZ_NM");
                        enttiy.FOX_NM = this.GetString(item, "FOX_NM");
                        enttiy.CZO_NM = this.GetString(item, "CZO_NM");
                        enttiy.TOB_NM = this.GetString(item, "TOB_NM");
                        enttiy.VAN_NM = this.GetString(item, "VAN_NM");
                        enttiy.VAN_NE = this.GetString(item, "VAN_NE");
                        enttiy.IPM_NM = this.GetString(item, "IPM_NM");
                        enttiy.LVX_NM = this.GetString(item, "LVX_NM");
                        enttiy.CTX_NE = this.GetString(item, "CTX_NE");
                        enttiy.CSL_ND75 = this.GetString(item, "CSL_ND75");
                        enttiy.ETP_ND10 = this.GetString(item, "ETP_ND10");
                        enttiy.ETP_NM = this.GetString(item, "ETP_NM");
                        enttiy.CTT_ND30 = this.GetString(item, "CTT_ND30");
                        enttiy.CTT_NM = this.GetString(item, "CTT_NM");
                        enttiy.DOR_ND10 = this.GetString(item, "DOR_ND10");
                        enttiy.DOR_NM = this.GetString(item, "DOR_NM");
                        enttiy.NET_ND30 = this.GetString(item, "NET_ND30");
                        enttiy.NET_NM = this.GetString(item, "NET_NM");
                        enttiy.QDA_ND15 = this.GetString(item, "QDA_ND15");
                        enttiy.QDA_NM = this.GetString(item, "QDA_NM");
                        enttiy.CPT_ND30 = this.GetString(item, "CPT_ND30");
                        enttiy.CPT_NM = this.GetString(item, "CPT_NM");
                        enttiy.CPT_NE = this.GetString(item, "CPT_NE");
                        enttiy.CZA_ND30 = this.GetString(item, "CZA_ND30");
                        enttiy.CZA_NM = this.GetString(item, "CZA_NM");
                        enttiy.CZA_NE = this.GetString(item, "CZA_NE");
                        enttiy.AZA_ND30 = this.GetString(item, "AZA_ND30");
                        enttiy.AZA_NM = this.GetString(item, "AZA_NM");
                        enttiy.AZA_NE = this.GetString(item, "AZA_NE");
                        enttiy.CZT_ND30 = this.GetString(item, "CZT_ND30");
                        enttiy.CZT_NM = this.GetString(item, "CZT_NM");
                        enttiy.CZT_NE = this.GetString(item, "CZT_NE");
                        //enttiy.TGC_NE = this.GetString(item, "TGC_NE");//新增字段
                        enttiy.DOX_NM = this.GetString(item, "DOX_NM");//新增字段
                        enttiy.SPEC_REAS = this.GetString(item, "SPEC_REAS");
                        enttiy.COMMENT = this.GetString(item, "COMMENT");
                        enttiy.INDUC_CLI = this.GetString(item, "INDUC_CLI");//新增字段

                        if (this.GetString(item, "CARBAPENEM") != "")
                        {
                            enttiy.CARBAPENEM = this.GetString(item, "CARBAPENEM");//新增字段
                        }
                        else if (this.GetString(item, "CARBA") != "")
                        {
                            enttiy.CARBAPENEM = this.GetString(item, "CARBA");//新增字段
                        }
                        else if (this.GetString(item, "CARBAPENEMASE") != "")
                        {
                            enttiy.CARBAPENEM = this.GetString(item, "CARBAPENEMASE");//新增字段
                        }
                        enttiy.COL_ND10 = this.GetString(item, "COL_ND10");//新增字段
                        enttiy.COL_NM = this.GetString(item, "COL_NM");//新增字段
                        enttiy.COL_NE = this.GetString(item, "COL_NE");//新增字段
                        enttiy.AMC_NE = this.GetString(item, "AMC_NE");//新增字段
                        enttiy.AMK_NE = this.GetString(item, "AMK_NE");//新增字段
                        enttiy.AMP_NE = this.GetString(item, "AMP_NE");//新增字段
                        enttiy.ATM_NE = this.GetString(item, "ATM_NE");//新增字段
                        enttiy.AZM_NE = this.GetString(item, "AZM_NE");//新增字段
                        enttiy.CAZ_NE = this.GetString(item, "CAZ_NE");//新增字段
                        enttiy.CEC_NE = this.GetString(item, "CEC_NE");//新增字段
                        enttiy.CFP_NE = this.GetString(item, "CFP_NE");//新增字段
                        enttiy.CHL_NE = this.GetString(item, "CHL_NE");//新增字段
                        enttiy.CIP_NE = this.GetString(item, "CIP_NE");//新增字段
                        enttiy.CLI_NE = this.GetString(item, "CLI_NE");//新增字段
                        enttiy.CRO_NE = this.GetString(item, "CRO_NE");//新增字段
                        enttiy.CTT_NE = this.GetString(item, "CTT_NE");//新增字段
                        enttiy.CXM_NE = this.GetString(item, "CXM_NE");//新增字段
                        enttiy.CZO_NE = this.GetString(item, "CZO_NE");//新增字段
                        enttiy.DOR_NE = this.GetString(item, "DOR_NE");//新增字段
                        enttiy.DOX_NE = this.GetString(item, "DOX_NE");//新增字段
                        enttiy.ERY_NE = this.GetString(item, "ERY_NE");//新增字段
                        enttiy.ETP_NE = this.GetString(item, "ETP_NE");//新增字段
                        enttiy.FEP_NE = this.GetString(item, "FEP_NE");//新增字段
                        enttiy.FOS_NE = this.GetString(item, "FOS_NE");//新增字段
                        enttiy.FOX_NE = this.GetString(item, "FOX_NE");//新增字段
                        enttiy.GEN_NE = this.GetString(item, "GEN_NE");//新增字段
                        enttiy.IPM_NE = this.GetString(item, "IPM_NE");//新增字段
                        enttiy.LNZ_NE = this.GetString(item, "LNZ_NE");//新增字段
                        enttiy.LVX_NE = this.GetString(item, "LVX_NE");//新增字段
                        enttiy.MEM_NE = this.GetString(item, "MEM_NE");//新增字段
                        enttiy.MFX_NE = this.GetString(item, "MFX_NE");//新增字段
                        enttiy.MNO_NE = this.GetString(item, "MNO_NE");//新增字段
                        enttiy.NET_NE = this.GetString(item, "NET_NE");//新增字段
                        enttiy.NIT_NE = this.GetString(item, "NIT_NE");//新增字段
                        enttiy.OXA_NE = this.GetString(item, "OXA_NE");//新增字段
                        enttiy.PIP_NE = this.GetString(item, "PIP_NE");//新增字段
                        enttiy.POL_NE = this.GetString(item, "POL_NE");//新增字段
                        enttiy.QDA_NE = this.GetString(item, "QDA_NE");//新增字段
                        enttiy.RIF_NE = this.GetString(item, "RIF_NE");//新增字段
                        enttiy.SAM_NE = this.GetString(item, "SAM_NE");//新增字段
                        enttiy.STH_NE = this.GetString(item, "STH_NE");//新增字段
                        enttiy.STR_NE = this.GetString(item, "STR_NE");//新增字段
                        enttiy.SXT_NE = this.GetString(item, "SXT_NE");//新增字段
                        enttiy.TCC_NE = this.GetString(item, "TCC_NE");//新增字段
                        enttiy.TCY_NE = this.GetString(item, "TCY_NE");//新增字段
                        enttiy.TEC_NE = this.GetString(item, "TEC_NE");//新增字段
                        enttiy.TGC_NE = this.GetString(item, "TGC_NE");//新增字段
                        enttiy.TIC_NE = this.GetString(item, "TIC_NE");//新增字段
                        enttiy.TOB_NE = this.GetString(item, "TOB_NE");//新增字段
                        enttiy.TZP_NE = this.GetString(item, "TZP_NE");//新增字段

                        enttiy.ERV_NM = this.GetString(item, "ERV_NM");
                        enttiy.ERV_ND20 = this.GetString(item, "ERV_ND20");
                        enttiy.ERV_NE = this.GetString(item, "ERV_NE");

                        list.Add(enttiy);
                    }
                    catch (Exception ex)
                    {
                        this.AddValidateItem(resultModel.ValidateList, 0, enttiy.UploadRowIndex, MedicalDataItemValidateLevelEnum.Error, "", "", "第" + n + "行数据错误，原因：" + ex.Message);
                        continue;
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Log4Helper.Error(this.GetType(), ex);
                throw ex;
            }
        }

        /// <summary>
        ///  获取验证对象数据
        /// </summary>
        private void AddValidateItem(List<MedicalDataItemValidate> list, long id, int rowIndex, MedicalDataItemValidateLevelEnum level, string keyName, string errorName, string content)
        {
            list.Add(new MedicalDataItemValidate()
            {
                Sort = ++DataSort,
                UploadRowIndex = rowIndex,
                KeyName = keyName,
                Content = content,
                ErrorName = errorName,
                Level = (int)level,
                MedicalDataItemId = id
            });
        }

        /// <summary>
        /// 检查需要被删除的细菌值
        /// </summary>
        /// <param name="organismCode">细菌编码</param>
        /// <returns>返回 true 则表示该行数据需要被删除，false则不需要被删除</returns>
        private bool DeleteOrganism(string organismCode)
        {
            //所有需要被删除的细菌
            //string delete = ",4c2,aal,aat,abe,abu,acb,ach,acl,act,acv,ade,adf,aec,aer,aeu,afl,afp,afr,afs,afu,afv,agb,agl,ahe,ahy,aja,amd,an-,an+,ana,anb,anc,and,ani,anp,anr,ans,anv,aoi,apb,apc,apn,apr,apy,arh,ars,asa,asb,ash,asn,aso,asp,asy,ate,atr,atu,aun,aur,aus,avc,ave,avi,avl,avr,avv,bab,bac,bad,bbi,bcc,bce,bcg,bci,bco,bcp,bcs,bde,bdi,bds,beg,bfg,bfo,bfr,bgr,bhm,bif,ble,bli,bme,bmr,bmt,boc,bov,bpa,bpu,br-,bra,bre,bsl,bsp,bst,bte,bth,bun,bur,bvu,c2c,c2e,c2h,c2i,c4c,c4d,c58,c59,c60,c63,c64,c68,c69,caa,cac,caf,caj,cal,can,cao,cap,caq,cbv,ccd,cci,ccl,ccm,ccn,cco,cct,ccy,cd2,cd3,cda,cdf,cdp,cdt,cdu,cel,cf1,cg2,cgl,cgm,cgt,cgu,cgy,cho,chp,chr,chu,ci1,ci2,cin,cjk,ckr,cku,cle,clm,clo,clp,clt,clu,cmc,cmg,cmi,cmt,cmy,cne,cnp,cor,cpa,cpb,cpc,cpd,cpe,cpi,cpl,cpq,cps,cra,crc,crg,crn,cru,csg,csl,csm,cso,cst,ctg,ctn,ctr,cul,cur,cut,cvi,cvn,cvw,cxa,cxe,cze,dho,dpn,e,e26,e4a,e4b,ear,ebc,ebr,ecr,eei,ef4,ehm,ele,eo2,eo3,erh,eta,evu,fmn,fnu,for,fso,fth,fus,G-,G+,gca,geo,gha,gm-,gm+,gmi,gmo,gnc,gne,gnr,gpc,gpr,gva,gvb,gvc,gvr,hac,haf,hal,hap,ifn,kas,kcr,kde,kki,kkr,kro,kur,kva,laa,lac,lad,lap,lat,lau,lba,lbi,lbn,lbo,lbt,lcc,lch,lcn,lcp,lcr,ldu,leg,lem,le,ler,lfe,lfr,lga,lge,lgo,lgp,lgr,lha,lic,lir,lja,lje,ljr,lla,llc,llg,lln,llo,lma,lmc,lmr,lms,lna,lok,lpa,lpc,lps,lqi,lqu,lru,lsa,lsc,lsh,lsk,lsp,lst,ltu,lwa,lwo,maa,mab,mac,mae,maf,mag,mai,mak,mas,mat,mau,mav,mbo,mbr,mbu,mcb,mce,mch,mcn,mct,mde,mdu,mfa,mfc,mfe,mfl,mfo,mfx,mga,mgd,mge,mgi,mgn,mgo,mha,mho,mib,mic,mij,min,mka,mko,mle,mlf,mli,mlp,mlu,mmammc,mmi,mml,mne,mno,moa,mol,mos,mou,mpa,mpc,mpf,mpg,mph,mpm,mpn,mpp,mpt,mrd,msa,msc,mse,msg,msh,msi,msl,msm,msz,mtc,mte,mth,mto,mtr,mtu,muc,mul,mur,mva,mwi,mxe,mya,myc,myp,myr,nas,nbr,ncb,nci,nco,ncr,nda,nde,ne-,nfa,nfl,nfr,nni,nno,noc,nod,not,nsi,ntr,nwe,oan,och,oth,otu,oul,our,paa,pai,pat,pbc,pbe,pbu,pcb,pcn,pco,pdi,pdm,pen,peo,pep,pes,pgi,pha,php,phy,pid,pie,pim,pin,pll,plo,plt,pmc,pmg,pmn,pni,pog,poh,poi,pol,ppa,ppn,ppo,ppp,pps,ppt,prb,prf,pro,pru,psh,pte,pva,pve,pvp,ral,rao,raq,rco,rde,req,rfa,rgi,rho,rhz,rgl,rot,rpr,saj,sas,sce,scr,sdf,she,sid,sla,smg,sml,smy,sne,spa,spf,ssv,sub,svd,swl,tas,tca,tgu,tha,tin,tpt,tri,vfl,vho,vi-,vip,vme,vmi,vpa,vvu,wvi,wzo,xxx,yok,yre,yro,"; 

            string delete = ",4c2,aal,aat,abe,abu,acb,ach,acl,act,ade,adf,aec,afl,afp,afr,afs,afu,afv,agb,agl,ahe,ahy,an-,an+,ana,anb,anc,and,ani,anp,anr,ans,anv,aoi,apb,apc,apr,apy,arh,ars,asn,asp,asy,ate,atu,aun,aur,aus,avc,avi,avl,avr,bac,bad,bbi,bcc,bce,bcg,bci,bco,bcp,bcs,bde,bdi,bds,beg,bfg,bfo,bfr,bgr,bhm,bif,ble,bli,bme,bmr,boc,bov,bpa,bpu,bra,bre,bsl,bsp,bst,bte,bth,bun,bur,bvu,c2c,c2e,c2h,c2i,c4d,c58,c59,c60,c63,c64,c68,c69,caa,cac,caf,caj,cal,can,cao,cap,caq,cbv,ccd,cci,ccl,ccm,ccn,cco,cct,ccy,cd2,cd3,cda,cdf,cdp,cdt,cdu,cel,cf1,cg2,cgl,cgm,cgt,cgu,cgy,cho,chp,chr,chu,ci1,ci2,cin,cjk,ckr,cku,cle,clm,clo,clp,clt,clu,cmc,cmg,cmi,cmt,cmy,cne,cnp,cor,cpa,cpb,cpc,cpd,cpe,cpi,cpl,cpq,cps,cra,crc,crg,crn,cru,csg,csl,csm,cso,cst,ctg,ctn,ctr,cul,cur,cut,cvi,cvn,cvw,cxa,cxe,cze,dho,dpn,e,e26,e4a,e4b,ear,ebc,ebr,ecr,eei,ef4,ehm,ele,eo2,eo3,erh,eta,evu,fmn,fnu,for,fso,fth,fus,G-,G+,gca,geo,gha,gm-,gm+,gmi,gmo,gnc,gne,gnr,gpc,gpr,gva,gvb,gvc,gvr,hac,haf,hal,hap,ifn,kas,kcr,kde,kki,kkr,kro,kur,kva,laa,lac,lad,lap,lat,lau,lba,lbi,lbn,lbo,lbt,lcc,lch,lcn,lcp,lcr,ldu,leg,lem,le,ler,lfe,lfr,lga,lge,lgo,lgp,lgr,lha,lic,lir,lja,lje,ljr,lla,llc,llg,lln,llo,lma,lmc,lmr,lms,lna,lok,lpa,lpc,lps,lqi,lqu,lru,lsa,lsc,lsh,lsk,lsp,lst,ltu,lwa,lwo,maa,mab,mac,mae,maf,mag,mai,mak,mas,mat,mau,mav,mbo,mbr,mbu,mcb,mce,mch,mcn,mct,mde,mdu,mfa,mfc,mfe,mfl,mfo,mfx,mga,mgd,mge,mgi,mgn,mgo,mha,mho,mib,mic,mij,min,mka,mko,mle,mlf,mli,mlp,mlu,mmammc,mmi,mml,mne,mno,moa,mol,mos,mou,mpa,mpc,mpf,mpg,mph,mpm,mpn,mpp,mpt,mrd,msa,msc,mse,msg,msh,msi,msl,msm,msz,mtc,mte,mth,mto,mtr,mtu,muc,mul,mur,mva,mwi,mxe,mya,myc,myp,myr,nas,nbr,ncb,nci,nco,ncr,nda,nde,ne-,nfa,nfl,nfr,nni,nno,noc,nod,not,nsi,ntr,nwe,oan,och,oth,otu,oul,our,paa,pai,pat,pbc,pbe,pbu,pcb,pcn,pco,pdi,pdm,pen,peo,pep,pes,pgi,pha,php,phy,pid,pie,pim,pin,pll,plo,plt,pmc,pmg,pmn,pni,pog,poh,poi,pol,ppa,ppn,ppo,ppp,pps,ppt,prb,prf,pro,pru,psh,pte,pva,pve,pvp,rao,raq,rco,rde,req,rfa,rgi,rho,rhz,rgl,rot,rpr,saj,sas,sce,scr,sdf,she,sid,sla,smg,sml,smy,sne,spa,spf,ssv,sub,svd,swl,tas,tca,tgu,tha,tin,tpt,tri,vfl,vho,vi-,vip,vme,vmi,vpa,vvu,wvi,wzo,xxx,yok,yre,yro,";

            return delete.ToLower().Contains("," + organismCode.ToLower() + ",");
        }

        /// <summary>
        /// 8.	剔除非无菌体液中的凝固酶阴性葡萄球菌和草绿色链球菌
        ///      a)	如果不是这些标本来源(ab, am, bi, mi, di, fl, ga, pf, bn, bl, sf, su)，
        ///     剔除以下细菌：san, scc, sct, sin, smt, smu, sol, str, svi, sad, sti, ssa, ssn, sub, sep, sae, sai,sur, scp, scu, scl, scg, slc, stc, stc, sul, sde, sep, seq, sfe, sgl, shl, sho, sho, snb, shy, sit, skl, sle, slu, sms, stt, sps, spv , pps, sap, ssb, sap, slc, ssf, ssr, sle, ssr, ssi, sta, spv, swa, sxy, scn, sc+
        /// </summary>
        /// <param name="organismCode">细菌编码</param>
        /// <returns>返回 true 则表示该行数据需要被删除，false则不需要被删除</returns>
        private bool DeleteSpecType(string specType, string organismCode)
        {
            //所有需要被删除的细菌
            string delete = ",san,scc,sct,sin,smt,smu,sol,str,svi,sad,sti,ssa,ssn,sub,sep,sae,sai,sur,scp,scu,scl,scg,slc,stc,stc,sul,sde,sep,seq,sfe,sgl,shl,sho,sho,snb,shy,sit,skl,sle,slu,sms,stt,sps,spv,pps,ssb,slc,ssf,ssr,sle,ssr,ssi,sta,spv,swa,sxy,scn,sc+,";
            string temp = ",ab,am,bi,mi,di,fl,ga,pf,bn,bl,sf,su,dr,";
            //string temp = ",ab,am,bm,mi,di,ga,pf,bn,bl,sf,";

            specType = "," + specType + ",";
            organismCode = "," + organismCode + ",";

            return (!temp.Contains(specType.ToLower().Trim()) && delete.Contains(organismCode.ToLower()));
        }

        /// <summary>
        /// 根据传入key和数据行，获取string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private String GetString(DataRow row, string key)
        {
            try
            {
                string value = row[key.TrimEnd()].ToString();
                return string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 重新设置上传文件中的 WARD 值
        /// </summary>
        /// <param name="oldWard">原始excel中的ward值</param>
        /// <param name="entity"></param>
        /// <param name="resultModel"></param>
        private void ReplaceHospitalWard(DataRow row, MedicalDataItem entity, UploadMedicalResult resultModel)
        {
            //先设置为excel中的
            entity.WARD = this.GetString(row, "WARD"); // 换成Location
            entity.WARD_TYPE = this.GetString(row, "WARD_TYPE"); // 换成Location_Type
            entity.DEPARTMENT = this.GetString(row, "DEPARTMENT"); // 换成Department.WARD = this.GetString(item, "WARD", "Location"); // 换成Location

            // 2020-01-20新增 当entity.WARD为空字符串时，会无视以下规则
            if (string.IsNullOrWhiteSpace(entity.WARD))
            {
                return;
            }

            if (resultModel == null || resultModel.HospitalWardList == null || !resultModel.HospitalWardList.Any())
                return;

            // 2019-02-21 新修改规则：设置优先符合所有文字替换，如果没有就替换前三个文字符合的。
            var item = resultModel.HospitalWardList.Where(m => m.Ward.ToLower().Equals(entity.WARD.ToLower())).FirstOrDefault();
            if (item == null)
            {
                // 2020-01-20修改之前 当entity.WARD为空字符串时，会无视以下规则，现已在之前读取判断entity.WARD不为空
                item = resultModel.HospitalWardList.Where(r => r.Ward.ToUpperInvariant().StartsWith(entity.WARD.ToLower())).FirstOrDefault();
                if (item == null) { return; }
            }

            //如果医院重新设置了对应的编码，则需要重新赋值
            entity.WARD = item.Location;
            entity.DEPARTMENT = item.Department_EN;
            entity.WARD_TYPE = item.Location_Type;
        }

        /// <summary>
        /// 根据传入key和数据行，获取string
        /// </summary>
        /// <param name="row"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private DateTime GetDateTime(DataRow row, string key)
        {
            try
            {
                return DateTime.Parse(row[key].ToString());
            }
            catch (Exception ex)
            {
                return DateTime.Parse("1900-01-01");
            }
        }

        /// <summary>
        /// 按照规则验证日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ValidateDateTime(string value)
        {
            //支持的格式：
            /*
            2014 11 05、2014 11 5、2014-11-05、2014-11-5、2014年11月05日、2014年11月5日、
            Nov - 25-2014、Nov/05/2016、05 Nov 2014、05-Nov-2014、05/Nov/2014、
             2014 05 Nov、2014-05-Nov、2014/05/Nov、2014 Nov 05、2014-Nov-05、2014/Nov/05
            */

            if (value.IsDateTime()) return value;

            //20141105（如果没有分隔符，那么长度必须为8位）
            if (value.IsLong() && value.Length == 8) return value;

            throw new Exception("格式不正确");
        }

        /// <summary>
        /// 导入数据的时候，因为要批量导入数据，所以需要生成SQL，然后导入到数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetMedicalDataInsertSql(MedicalDataItem model)
        {
            StringBuilder i = new StringBuilder();
            StringBuilder v = new StringBuilder();

            i.Append("  INSERT INTO [dbo].[MedicalDataItem]  (  Id ,InsertTime,UpdateTime,DeleteTime ,Version ,Mark  ,Describe  ,Sort   ,UploadRowIndex ,IsValid  ,MedicalDataId,    OrganismName  ,OrganismId, ");
            v.Append(" VALUES('" + model.Id + "','" + model.InsertTime + "','" + model.UpdateTime + "','" + model.DeleteTime + "','" + model.Version + "','" + model.Mark + "','" + model.Describe + "','" + model.Sort + "','" + model.UploadRowIndex + "','" + model.IsValid + "', '" + model.MedicalDataId + "','" + model.OrganismName + "','" + model.OrganismId + "',");

            this.GetMedicalDataItemSql(i, v, "COUNTRY_A", model.COUNTRY_A);
            this.GetMedicalDataItemSql(i, v, "LABORATORY", model.LABORATORY);
            this.GetMedicalDataItemSql(i, v, "PATIENT_ID", model.PATIENT_ID);
            this.GetMedicalDataItemSql(i, v, "FIRST_NAME", model.FIRST_NAME);
            this.GetMedicalDataItemSql(i, v, "LAST_NAME", model.LAST_NAME);
            this.GetMedicalDataItemSql(i, v, "SEX", model.SEX);
            this.GetMedicalDataItemSql(i, v, "AGE", model.AGE);
            this.GetMedicalDataItemSql(i, v, "DATE_BIRTH", model.DATE_BIRTH);
            this.GetMedicalDataItemSql(i, v, "WARD", model.WARD);
            this.GetMedicalDataItemSql(i, v, "WARD_TYPE", model.WARD_TYPE);
            this.GetMedicalDataItemSql(i, v, "INSTITUT", model.INSTITUT);
            this.GetMedicalDataItemSql(i, v, "DEPARTMENT", model.DEPARTMENT);
            this.GetMedicalDataItemSql(i, v, "SPEC_NUM", model.SPEC_NUM);
            this.GetMedicalDataItemSql(i, v, "SPEC_DATE", model.SPEC_DATE);
            this.GetMedicalDataItemSql(i, v, "SPEC_TYPE", model.SPEC_TYPE);
            this.GetMedicalDataItemSql(i, v, "SPEC_CODE", model.SPEC_CODE);
            this.GetMedicalDataItemSql(i, v, "ORGANISM", model.ORGANISM);
            this.GetMedicalDataItemSql(i, v, "ORG_TYPE", model.ORG_TYPE);
            this.GetMedicalDataItemSql(i, v, "ESBL", model.ESBL);
            this.GetMedicalDataItemSql(i, v, "BETA_LACT", model.BETA_LACT);
            this.GetMedicalDataItemSql(i, v, "INDUC_CLI", model.INDUC_CLI);///新增字段
            this.GetMedicalDataItemSql(i, v, "CARBAPENEM", model.CARBAPENEM);///新增字段
            this.GetMedicalDataItemSql(i, v, "COMMENT", model.COMMENT);///新增字段
            this.GetMedicalDataItemSql(i, v, "AMK_ND30", model.AMK_ND30);
            this.GetMedicalDataItemSql(i, v, "AMC_ND20", model.AMC_ND20);
            this.GetMedicalDataItemSql(i, v, "AZM_ND15", model.AZM_ND15);
            this.GetMedicalDataItemSql(i, v, "AMP_ND10", model.AMP_ND10);
            this.GetMedicalDataItemSql(i, v, "SAM_ND10", model.SAM_ND10);
            this.GetMedicalDataItemSql(i, v, "ATM_ND30", model.ATM_ND30);
            this.GetMedicalDataItemSql(i, v, "OXA_ND1", model.OXA_ND1);
            this.GetMedicalDataItemSql(i, v, "POL_ND300", model.POL_ND300);
            this.GetMedicalDataItemSql(i, v, "NIT_ND300", model.NIT_ND300);
            this.GetMedicalDataItemSql(i, v, "SXT_ND1_2", model.SXT_ND1_2);
            this.GetMedicalDataItemSql(i, v, "STH_ND300", model.STH_ND300);
            this.GetMedicalDataItemSql(i, v, "GEH_ND120", model.GEH_ND120);
            this.GetMedicalDataItemSql(i, v, "ERY_ND15", model.ERY_ND15);
            this.GetMedicalDataItemSql(i, v, "CIP_ND5", model.CIP_ND5);
            this.GetMedicalDataItemSql(i, v, "CLI_ND2", model.CLI_ND2);
            this.GetMedicalDataItemSql(i, v, "RIF_ND5", model.RIF_ND5);
            this.GetMedicalDataItemSql(i, v, "LNZ_ND30", model.LNZ_ND30);
            this.GetMedicalDataItemSql(i, v, "STR_ND10", model.STR_ND10);
            this.GetMedicalDataItemSql(i, v, "FOS_ND200", model.FOS_ND200);
            this.GetMedicalDataItemSql(i, v, "CHL_ND30", model.CHL_ND30);
            this.GetMedicalDataItemSql(i, v, "MEM_ND10", model.MEM_ND10);
            this.GetMedicalDataItemSql(i, v, "MNO_ND30", model.MNO_ND30);
            this.GetMedicalDataItemSql(i, v, "MFX_ND5", model.MFX_ND5);
            this.GetMedicalDataItemSql(i, v, "PIP_ND100", model.PIP_ND100);
            this.GetMedicalDataItemSql(i, v, "TZP_ND100", model.TZP_ND100);
            this.GetMedicalDataItemSql(i, v, "PEN_ND10", model.PEN_ND10);
            this.GetMedicalDataItemSql(i, v, "GEN_ND10", model.GEN_ND10);
            this.GetMedicalDataItemSql(i, v, "TCY_ND30", model.TCY_ND30);
            this.GetMedicalDataItemSql(i, v, "TCC_ND75", model.TCC_ND75);
            this.GetMedicalDataItemSql(i, v, "TIC_ND75", model.TIC_ND75);
            this.GetMedicalDataItemSql(i, v, "TEC_ND30", model.TEC_ND30);
            this.GetMedicalDataItemSql(i, v, "TGC_ND15", model.TGC_ND15);
            this.GetMedicalDataItemSql(i, v, "FEP_ND30", model.FEP_ND30);
            this.GetMedicalDataItemSql(i, v, "CXM_ND30", model.CXM_ND30);
            this.GetMedicalDataItemSql(i, v, "CEC_ND30", model.CEC_ND30);
            this.GetMedicalDataItemSql(i, v, "CFP_ND75", model.CFP_ND75);
            this.GetMedicalDataItemSql(i, v, "CSL_ND30", model.CSL_ND30);
            this.GetMedicalDataItemSql(i, v, "CRO_ND30", model.CRO_ND30);
            this.GetMedicalDataItemSql(i, v, "CTX_ND30", model.CTX_ND30);
            this.GetMedicalDataItemSql(i, v, "CAZ_ND30", model.CAZ_ND30);
            this.GetMedicalDataItemSql(i, v, "FOX_ND30", model.FOX_ND30);
            this.GetMedicalDataItemSql(i, v, "CZO_ND30", model.CZO_ND30);
            this.GetMedicalDataItemSql(i, v, "TOB_ND10", model.TOB_ND10);
            this.GetMedicalDataItemSql(i, v, "VAN_ND30", model.VAN_ND30);
            this.GetMedicalDataItemSql(i, v, "IPM_ND10", model.IPM_ND10);
            this.GetMedicalDataItemSql(i, v, "LVX_ND5", model.LVX_ND5);
            this.GetMedicalDataItemSql(i, v, "OFX_ND5", model.OFX_ND5);
            this.GetMedicalDataItemSql(i, v, "DOX_ND30", model.DOX_ND30);
            this.GetMedicalDataItemSql(i, v, "AMK_NM", model.AMK_NM);
            this.GetMedicalDataItemSql(i, v, "AMC_NM", model.AMC_NM);
            this.GetMedicalDataItemSql(i, v, "AZM_NM", model.AZM_NM);
            this.GetMedicalDataItemSql(i, v, "AMP_NM", model.AMP_NM);
            this.GetMedicalDataItemSql(i, v, "SAM_NM", model.SAM_NM);
            this.GetMedicalDataItemSql(i, v, "ATM_NM", model.ATM_NM);
            this.GetMedicalDataItemSql(i, v, "OXA_NM", model.OXA_NM);
            this.GetMedicalDataItemSql(i, v, "POL_NM", model.POL_NM);
            this.GetMedicalDataItemSql(i, v, "NIT_NM", model.NIT_NM);
            this.GetMedicalDataItemSql(i, v, "SXT_NM", model.SXT_NM);
            this.GetMedicalDataItemSql(i, v, "STH_NM", model.STH_NM);
            this.GetMedicalDataItemSql(i, v, "GEH_NM", model.GEH_NM);
            this.GetMedicalDataItemSql(i, v, "ERY_NM", model.ERY_NM);
            this.GetMedicalDataItemSql(i, v, "CIP_NM", model.CIP_NM);
            this.GetMedicalDataItemSql(i, v, "CLI_NM", model.CLI_NM);
            this.GetMedicalDataItemSql(i, v, "RIF_NM", model.RIF_NM);
            this.GetMedicalDataItemSql(i, v, "LNZ_NM", model.LNZ_NM);
            this.GetMedicalDataItemSql(i, v, "STR_NM", model.STR_NM);
            this.GetMedicalDataItemSql(i, v, "FOS_NM", model.FOS_NM);
            this.GetMedicalDataItemSql(i, v, "CHL_NM", model.CHL_NM);
            this.GetMedicalDataItemSql(i, v, "MEM_NM", model.MEM_NM);
            this.GetMedicalDataItemSql(i, v, "MNO_NM", model.MNO_NM);
            this.GetMedicalDataItemSql(i, v, "MFX_NM", model.MFX_NM);
            this.GetMedicalDataItemSql(i, v, "PIP_NM", model.PIP_NM);
            this.GetMedicalDataItemSql(i, v, "TZP_NM", model.TZP_NM);
            this.GetMedicalDataItemSql(i, v, "PEN_NM", model.PEN_NM);
            this.GetMedicalDataItemSql(i, v, "GEN_NM", model.GEN_NM);
            this.GetMedicalDataItemSql(i, v, "PEN_NE", model.PEN_NE);
            this.GetMedicalDataItemSql(i, v, "TCY_NM", model.TCY_NM);
            this.GetMedicalDataItemSql(i, v, "TCC_NM", model.TCC_NM);
            this.GetMedicalDataItemSql(i, v, "TIC_NM", model.TIC_NM);
            this.GetMedicalDataItemSql(i, v, "TEC_NM", model.TEC_NM);
            this.GetMedicalDataItemSql(i, v, "TGC_NM", model.TGC_NM);
            this.GetMedicalDataItemSql(i, v, "FEP_NM", model.FEP_NM);
            this.GetMedicalDataItemSql(i, v, "CXM_NM", model.CXM_NM);
            this.GetMedicalDataItemSql(i, v, "CEC_NM", model.CEC_NM);
            this.GetMedicalDataItemSql(i, v, "CFP_NM", model.CFP_NM);
            this.GetMedicalDataItemSql(i, v, "CSL_NM", model.CSL_NM);
            this.GetMedicalDataItemSql(i, v, "CRO_NM", model.CRO_NM);
            this.GetMedicalDataItemSql(i, v, "CTX_NM", model.CTX_NM);
            this.GetMedicalDataItemSql(i, v, "CAZ_NM", model.CAZ_NM);
            this.GetMedicalDataItemSql(i, v, "FOX_NM", model.FOX_NM);
            this.GetMedicalDataItemSql(i, v, "CZO_NM", model.CZO_NM);
            this.GetMedicalDataItemSql(i, v, "TOB_NM", model.TOB_NM);
            this.GetMedicalDataItemSql(i, v, "VAN_NM", model.VAN_NM);
            this.GetMedicalDataItemSql(i, v, "VAN_NE", model.VAN_NE);
            this.GetMedicalDataItemSql(i, v, "IPM_NM", model.IPM_NM);
            this.GetMedicalDataItemSql(i, v, "LVX_NM", model.LVX_NM);
            this.GetMedicalDataItemSql(i, v, "CTX_NE", model.CTX_NE);
            this.GetMedicalDataItemSql(i, v, "CSL_ND75", model.CSL_ND75);
            this.GetMedicalDataItemSql(i, v, "ETP_ND10", model.ETP_ND10);
            this.GetMedicalDataItemSql(i, v, "ETP_NM", model.ETP_NM);
            this.GetMedicalDataItemSql(i, v, "CTT_ND30", model.CTT_ND30);
            this.GetMedicalDataItemSql(i, v, "CTT_NM", model.CTT_NM);
            this.GetMedicalDataItemSql(i, v, "DOR_ND10", model.DOR_ND10);
            this.GetMedicalDataItemSql(i, v, "DOR_NM", model.DOR_NM);
            this.GetMedicalDataItemSql(i, v, "NET_ND30", model.NET_ND30);
            this.GetMedicalDataItemSql(i, v, "NET_NM", model.NET_NM);
            this.GetMedicalDataItemSql(i, v, "QDA_ND15", model.QDA_ND15);
            this.GetMedicalDataItemSql(i, v, "QDA_NM", model.QDA_NM);
            this.GetMedicalDataItemSql(i, v, "CPT_ND30", model.CPT_ND30);
            this.GetMedicalDataItemSql(i, v, "CPT_NM", model.CPT_NM);
            this.GetMedicalDataItemSql(i, v, "CPT_NE", model.CPT_NE);
            this.GetMedicalDataItemSql(i, v, "CZA_ND30", model.CZA_ND30);
            this.GetMedicalDataItemSql(i, v, "CZA_NM", model.CZA_NM);
            this.GetMedicalDataItemSql(i, v, "CZA_NE", model.CZA_NE);
            this.GetMedicalDataItemSql(i, v, "AZA_ND30", model.AZA_ND30);
            this.GetMedicalDataItemSql(i, v, "AZA_NM", model.AZA_NM);
            this.GetMedicalDataItemSql(i, v, "AZA_NE", model.AZA_NE);
            this.GetMedicalDataItemSql(i, v, "CZT_ND30", model.CZT_ND30);
            this.GetMedicalDataItemSql(i, v, "CZT_NM", model.CZT_NM);
            this.GetMedicalDataItemSql(i, v, "CZT_NE", model.CZT_NE);
            //this.GetMedicalDataItemSql(i, v, "TGC_NE", model.TGC_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "DOX_NM", model.DOX_NM);//新增字段
            this.GetMedicalDataItemSql(i, v, "COL_ND10", model.COL_ND10);//新增字段
            this.GetMedicalDataItemSql(i, v, "COL_NM", model.COL_NM);//新增字段
            this.GetMedicalDataItemSql(i, v, "COL_NE", model.COL_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "AMC_NE", model.AMC_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "AMK_NE", model.AMK_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "AMP_NE", model.AMP_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "ATM_NE", model.ATM_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "AZM_NE", model.AZM_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CAZ_NE", model.CAZ_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CEC_NE", model.CEC_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CFP_NE", model.CFP_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CHL_NE", model.CHL_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CIP_NE", model.CIP_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CLI_NE", model.CLI_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CRO_NE", model.CRO_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CTT_NE", model.CTT_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CXM_NE", model.CXM_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "CZO_NE", model.CZO_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "DOR_NE", model.DOR_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "DOX_NE", model.DOX_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "ERY_NE", model.ERY_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "ETP_NE", model.ETP_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "FEP_NE", model.FEP_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "FOS_NE", model.FOS_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "FOX_NE", model.FOX_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "GEN_NE", model.GEN_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "IPM_NE", model.IPM_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "LNZ_NE", model.LNZ_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "LVX_NE", model.LVX_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "MEM_NE", model.MEM_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "MFX_NE", model.MFX_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "MNO_NE", model.MNO_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "NET_NE", model.NET_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "NIT_NE", model.NIT_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "OXA_NE", model.OXA_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "PIP_NE", model.PIP_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "POL_NE", model.POL_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "QDA_NE", model.QDA_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "RIF_NE", model.RIF_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "SAM_NE", model.SAM_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "STH_NE", model.STH_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "STR_NE", model.STR_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "SXT_NE", model.SXT_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "TCC_NE", model.TCC_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "TCY_NE", model.TCY_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "TEC_NE", model.TEC_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "TGC_NE", model.TGC_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "TIC_NE", model.TIC_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "TOB_NE", model.TOB_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "TZP_NE", model.TZP_NE);//新增字段
            this.GetMedicalDataItemSql(i, v, "ERV_NM", model.ERV_NM);//新增字段
            this.GetMedicalDataItemSql(i, v, "ERV_ND20", model.ERV_ND20);//新增字段
            this.GetMedicalDataItemSql(i, v, "ERV_NE", model.ERV_NE);//新增字段

            string valueeee = i.ToString().TrimEnd(',') + ")" + v.ToString().TrimEnd(',') + ");";
            return valueeee;
        }

        /// <summary>
        /// 获取SQL
        /// </summary>
        /// <param name="i"></param>
        /// <param name="v"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void GetMedicalDataItemSql(StringBuilder i, StringBuilder v, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                key = key.Replace("'", "‘");
                value = value.Replace("'", "‘");
                i.Append(key + ",");
                v.Append("'" + value + "',");
            }
        }

        /// <summary>
        /// 导入数据的时候，因为要批量导入数据，所以需要生成SQL，然后导入到数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetMedicalValidateInsertSql(MedicalDataItemValidate model)
        {
            string insertSql = " INSERT INTO dbo.MedicalDataItemValidate  ( Id ,MedicalDataItemId ,  KeyName , Level ,ErrorName ,Content ,InsertTime ," +
                "    UpdateTime ,DeleteTime ,Version ,Mark ,Describe ,UploadRowIndex , MedicalDataId ,Sort) ";

            string values = string.Format(" VALUES  ( '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}' ); ",
                model.Id, model.MedicalDataItemId, model.KeyName, model.Level, model.ErrorName, model.Content,
                model.InsertTime, model.UpdateTime, model.DeleteTime, model.Version, model.Mark, model.Describe,
                model.UploadRowIndex, model.MedicalDataId, model.Sort);

            return insertSql + values;
        }

        /// <summary>
        /// 分页查询数据   前台医学数据首页
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="hospitalId"></param>
        /// <param name="specimenId"></param>
        /// <param name="bacteriaTypeId"></param>
        /// <param name="departmentId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<MedicalData> Query(long areaId, long hospitalId, long specimenId, long bacteriaTypeId, long departmentId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            string sql = " SELECT  *, ( SELECT Name FROM  dbo.Hospital(NOLOCK) WHERE  Id=MedicalData.HospitalId ) AS HospitalName,( SELECT COUNT(*) FROM dbo.MedicalDataItem(NOLOCK)  WHERE Mark > 0 AND MedicalDataId = MedicalData.Id  ) AS AreaId FROM  dbo.MedicalData(NOLOCK)     ";
            string where = " AND  Mark > 0  ";
            SpringSqlParameters par = new SpringSqlParameters();
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (areaId > 0)
            {
                where += " AND AreaId = @AreaId  ";
                par.Add("AreaId", areaId);
            }

            if (hospitalId > 0)
            {
                where += " AND HospitalId = @HospitalId  ";
                par.Add("HospitalId", hospitalId);
            }

            if (specimenId > 0)
            {
                where += " AND SpecimenId = @SpecimenId  ";
                par.Add("SpecimenId", specimenId);
            }

            if (departmentId > 0)
            {
                where += " AND HospitalDepartmentId = @HospitalDepartmentId  ";
                par.Add("HospitalDepartmentId", departmentId);
            }

            if (bacteriaTypeId > 0)
            {
                where += " AND  BacteriaTypeIds IN ('@BacteriaTypeIds')  ";
                par.Add("BacteriaTypeIds", bacteriaTypeId.ToString());
            }

            return new DapperPageHelper().QueryPage<MedicalData>(sql, where, "ORDER BY InsertTime DESC  ", "  MedicalData ", par, pageIndex, pageSize);
        }

        /// <summary>
        /// 会员中心，医学信息管理的分页数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IQueryable<MedicalData> Query(long memberId, int year, int quarter, string fileName)
        {
            var data = base._repository.Table.Where(m => m.Mark > 0 && m.MemberId == memberId && m.Display == true);
            if (year > 0)
                data = data.Where(m => m.Year == year);
            if (quarter > 0)
                data = data.Where(m => m.Quarter == quarter);
            if (!string.IsNullOrWhiteSpace(fileName))
                data = data.Where(m => m.FileName.Contains(fileName));

            return data.OrderByDescending(m => m.InsertTime);
        }

        public IPagedList<MedicalData> QueryPage(string hospitalName, long areaId, long hospitalId, long projectType, int year, int quarter, int pageIndex = 0, int pageSize = int.MaxValue, string satelliteId = "")
        {
            var query = this._repository.Table.Where(m => m.Mark > 0 && m.Display == true);

            if (!string.IsNullOrWhiteSpace(hospitalName))
                query = query.Where(m => m.HospitalName.Contains(hospitalName));

            if (areaId > 0)
                query = query.Where(m => m.AreaId == areaId);

            if (hospitalId > 0)
                query = query.Where(m => m.HospitalId == hospitalId);

            if (year > 0)
                query = query.Where(m => m.Year == year);

            if (quarter > 0)
                query = query.Where(m => m.Quarter == quarter);

            if (projectType > 0)
            {
                query = query.Where(r => r.ProjectType == projectType);
            }

            if (!string.IsNullOrEmpty(satelliteId))
            {
                long sateId = long.Parse(satelliteId);
                query = query.Where(r => r.MemberId == sateId);
            }

            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<MedicalData>(query, pageIndex, pageSize);

            return list;
        }

        public MedicalData Query(long id)
        {
            var data = base._repository.Table.Where(m => m.Id == id);
            return data.OrderByDescending(m => m.InsertTime).FirstOrDefault();
        }

        #region  数据分析

        /// <summary>
        /// Peng-测试单独
        /// </summary>
        /// <param name="medicalId">医学数据Id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string OneAutoDispose(long medicalId = 0)
        {
            Log4Helper.Info($"{medicalId}->开始执行2");

            var entity = this.QueryEntity(m => m.Id == medicalId);
            if (entity == null || entity.Id <= 0 )
            {
                Log4Helper.Info($"{medicalId}->没有需要处理的数据, 本次操作结束");
                throw new Exception("没有需要处理的数据, 本次操作结束");
            }

            Log4Helper.Info($"{medicalId}->查询是否已生成");
            var _DisposeExcelFileUrl = string.Empty;
            var itemList = this.MedicalDataItemService.Query(t => t.MedicalDataId == medicalId).ToList();
            if (itemList == null || itemList.Count <= 0)
            {
                Log4Helper.Info($"{medicalId}->查询上传用户信息");
                IMemberService memberService = EngineContext.Current.Resolve<IMemberService>();
                var member = memberService.QueryEntity(entity.MemberId);

                //记录日志
                Log4Helper.Info($"{medicalId}的上传用户是{member.NickName}");
                UploadMedicalResult result = this.AutoDisposeInsert(entity);
            }
            else
            {
                Log4Helper.Info($"{medicalId}的文件数据已写入数据库,不再重复写入");
            }

            Log4Helper.Info($"{medicalId}开始生成容错文件");
            string urlPath = this.DownloadNewData(entity.Id);
            if (!string.IsNullOrWhiteSpace(urlPath))
            {
                _DisposeExcelFileUrl = "http://www.chinets.com" + urlPath;
            }

            Log4Helper.Info($"{medicalId}的容错文件{_DisposeExcelFileUrl}");

            return _DisposeExcelFileUrl;
        }

        /// <summary>
        /// 自动化作业处理数据  
        /// </summary>
        /// <param name="medicalId">医学数据Id，没传递则操作所有</param>
        /// <returns></returns>
        public string AutoDispose(long medicalId = 0)
        {
            /*
                上传数据表：MedicalData、保存数据表：MedicalDataItem、锁定数据表：MedicalDataHandleLock
                流程：读取MedicalData表数据，解析后写入MedicalDataItem表，通过MedicalDataHandleLock来判断状态
             */

            bool autoServer = medicalId == 0;

            bool isDeleteLock = true; //清除分布式锁
            string cacheKey = "managesystem.services.medicine.medicaldataservice.autodispose." + medicalId;

            MedicalData entity = null;//
            int step = 0;
            bool sendErrorEmail = true;
            Member member = null;

            try
            {
                // 验证分布式锁 ，系统5分钟内还未处理完成则自动释放
                if (this.CacheManager.ExistLock(cacheKey))
                {
                    //isDeleteLock = false;
                    //throw new Exception("本次上传的数据已在计算中，请等待处理完成");
                }

                if (!this.CacheManager.AddLock(cacheKey, 60))
                {
                    //isDeleteLock = false;
                    //throw new Exception("接口请求过于频繁，请等待处理完成");
                }

                //this.CacheManager.AddLock(cacheKey, 5);

                try
                {
                    //1、读取一个需要处理的数据，返回的数据必然是有值的，没值会抛出异常
                    entity = this.AutoDisposeGetEntity(medicalId);
                    if (entity == null || entity.Id <= 0 || entity.Mark == 0)
                    {
                        // 2010-01-11 新增
                        // 没有需要处理的数据
                        // 不需要发送错误信息
                        sendErrorEmail = false;
                        throw new Exception("没有需要处理的数据, 本次操作结束");
                    }

                    IMemberService memberService = EngineContext.Current.Resolve<IMemberService>();
                    member = memberService.QueryEntity(entity.MemberId);
                }
                catch (Exception Ex)
                {
                    step = 1;
                    throw new Exception("上传的文件读取数据失败！" + Ex.Message);
                }

                #region 新增MedicalDataHandleLock
                try
                {
                    int handleLockStatus = _medicalDataHandleLockService.GetStatusByMedicalDataId(entity.Id);
                    switch (handleLockStatus)
                    {
                        case 1:
                            {
                                isDeleteLock = false;
                                sendErrorEmail = false;
                                throw new Exception("本次上传的数据已在计算中，请等待处理完成");
                            }
                        case 2:
                            {
                                isDeleteLock = false;
                                sendErrorEmail = false;
                                throw new Exception("本次上传的数据已处理完成, 无需重复处理");
                            }
                        case 3:
                            {
                                isDeleteLock = false;
                                sendErrorEmail = false;
                                throw new Exception("上传的数据处理已发生错误");
                            }
                        default:
                            _medicalDataHandleLockService.Insert(entity.Id, 1);
                            break;
                    }
                }
                catch (Exception)
                {
                    sendErrorEmail = false;
                    throw new Exception("MedicalDataHandleLock Exception");
                }
                #endregion

                UploadMedicalResult result;

                try
                {
                    //2、进行数据分析，得到处理的结果
                    result = this.AutoDisposeInsert(entity);
                }
                catch (Exception ex)
                {
                    step = 2;
                    throw new Exception("文件解析失败，请核对数据填写情况!失败原因：" + ex.ToString());
                }

                try
                {
                    //3、产生处理数据后的Excel文件
                    Log4Helper.Info("获取容错文件地址");
                    string urlPath = this.DownloadNewData(entity.Id);
                    if (!string.IsNullOrWhiteSpace(urlPath))
                    {
                        result.DisposeExcelFileUrl = "http://www.chinets.com" + urlPath;
                    }
                }
                catch (Exception ex)
                {
                    step = 3;
                    if (result != null)
                    {
                        int errorCount = result.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Error).Count();
                        int warningCount = result.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Warning).Count();
                        int hintCount = result.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Hint).Count();
                        string uploadResult = $"本次共读取：{result.BaseMessage.ReadCount}行数据，本次共成功上传{result.BaseMessage.SuccessCount}行数据，共产生：{errorCount}个错误、{warningCount}个警告、{hintCount}个提醒。{ex.Message}";
                        throw new Exception(uploadResult);
                    }
                    else
                    {
                        throw new Exception("数据已上传成功但未能成功生成容错文件！");
                    }
                }

                //4、发送消息通知
                this.AutoDisposeSendEmail(result);

                //5、记录操作日志
                string logContent = "上传数据Id：" + result.MedicalEntity.Id.ToString() + "，所属医院名称：" + result.MedicalEntity.HospitalName + "，" + result.UploadResultLog;
                this.ActionLogService.Insert(ActionType.Edit, ActionSource.InnerApi, 0, "自动化服务", logContent);

                // 更新MedicalDataHandleLock状态
                _medicalDataHandleLockService.Complete(entity.Id, 2);

                return result.UploadResultLog;
            }
            catch (Exception ex)
            {
                if (entity != null && entity.Id > 0)
                {
                    //记录这条上传记录的错误信息
                    entity.ExceptionMessage = ex.Message;
                    entity.ExceptionStackTrace = ex.StackTrace;
                    this.Update(entity);

                    if (step != 0)
                    {
                        string result = "";
                        switch (step)
                        {
                            case 1:
                            case 2:
                            case 3:
                                result = ex.Message;
                                break;
                            default:
                                result = "上传文件解析失败";
                                break;
                        }

                        if (sendErrorEmail)
                        {
                            // 发送消息通知
                            this.AutoDisposeSendEmail_Error(member, result);
                        }
                    }

                    // 更新MedicalDataHandleLock状态
                    _medicalDataHandleLockService.Error(entity.Id, 3, $"\r\n{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}发生异常, Status: 3");

                }

                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                throw ex;
            }
            finally
            {
                if (isDeleteLock)
                    this.CacheManager.DeleteLock(cacheKey);
            }
        }

        /// <summary>
        /// 自动化作业处理数据    获取一个需要处理的数据
        /// </summary>
        /// <param name="medicalId">医学数据的id，如果传递了则获取指定的数据，没有传递则系统查询所有数据</param>
        /// <returns></returns>
        private MedicalData AutoDisposeGetEntity(long medicalId = 0)
        {
            if (medicalId > 0)
            {
                //var entity = this.QueryEntity(m => m.Id == medicalId);//Peng-2023.01.17
                var entity = this.QueryEntity(m => m.Id == medicalId && m.Status == (int)MedicalDataStatusEnum.Wait && m.Mark > 0);
                if (entity == null || entity.Id <= 0)
                {
                    Log4Helper.Info(this.GetType(), $"AutoDisposeGetEntity(long medicalId = {medicalId}) => 没有需要处理的数据(entity == null || entity.Id <= 0)");
                    return null; // 2020-01-11新增
                    // throw new Exception("Id：" + medicalId + " 数据不存在"); // 2020-01-11注释，没有就说明不需要处理
                }

                return entity;
            }

            //查询 状态是待处理的，没有异常信息的（有异常的表示处理过了，但是处理不了，这种数据只能靠人工处理），时间是最近2天的数据（不然容易循环老的错误数据导致程序慢）
            //优先处理最早上传的数据
            var list = this.Query(m => m.Status == (int)MedicalDataStatusEnum.Wait && m.Mark > 0);
            if (list == null || !list.Any())
            {
                Log4Helper.Info(this.GetType(), $"AutoDisposeGetEntity(long medicalId = {medicalId}) => 没有需要处理的数据(list == null || !list.Any())");
                return null;
                //throw new Exception("本次操作未查询到数据"); // 2020-01-11注释，没有就说明不需要处理
            }

            // 获取一个近两天内待处理且没有发生异常的数据
            var entity2 = list.Where(m => m.Status == (int)MedicalDataStatusEnum.Wait
                    && m.Mark > 0
                    && string.IsNullOrWhiteSpace(m.ExceptionMessage)
                    && m.InsertTime >= DateTime.Now.AddDays(-2))
                .OrderBy(m => m.InsertTime)
                .FirstOrDefault();

            if (entity2 == null || entity2.Id <= 0)
            {
                Log4Helper.Info(this.GetType(), $"AutoDisposeGetEntity(long medicalId = {medicalId}) => 没有需要处理的数据(entity2 == null || entity2.Id <= 0)");
                return null;
                //throw new Exception("本次操作未查询到数据"); // 2020-01-11注释，没有就说明不需要处理
            }

            return entity2;
        }

        /// <summary>
        /// 自动化作业处理数据   数据处理和保存到数据库
        /// </summary>
        /// <param name="entity">需要操作的医学数据</param>
        /// <returns>返回操作完成的数据封装</returns>
        public UploadMedicalResult AutoDisposeInsert(MedicalData entity)
        {
            string uploadTimeLog = "开始业务处理时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            try
            {
                DataSort = 0;
                IMemberService memberService = EngineContext.Current.Resolve<IMemberService>();

                UploadMedicalResult uploadModel = new UploadMedicalResult();
                uploadModel.ValidateList = new List<MedicalDataItemValidate>();
                uploadModel.ItemList = new List<MedicalDataItem>();
                uploadModel.MedicalEntity = entity;
                uploadModel.HospitalEntity = this.HospitalService.QueryEntity(entity.HospitalId);
                uploadModel.MemberEntity = memberService.QueryEntity(entity.MemberId);
                uploadModel.HospitalWardList = this.HospitalWardLocationService.Query(m => m.HospitalId == entity.HospitalId).ToList();
                UploadMessageModel uploadMessageModel = entity.UploadMessage.DeserializeObject<UploadMessageModel>();
                uploadModel.BaseMessage = uploadMessageModel.BaseMessage;

                #region  通过Excel获取导入的数据

                uploadTimeLog += "，开始读取Excel数据：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                Log4Helper.Info(uploadTimeLog);
                if (uploadModel.BaseMessage.FileName.ToLower().Contains(".dbf"))
                {
                    uploadModel.ExcelItemList = this.GetImportDataByDBF(System.Web.HttpContext.Current.Server.MapPath(uploadModel.MedicalEntity.UploadFilePath), uploadModel);
                }
                else
                {
                    var _filePath = System.Web.HttpContext.Current.Server.MapPath(uploadModel.MedicalEntity.UploadFilePath);
                    Log4Helper.Info($"读取文件：{_filePath}");
                    if (!System.IO.File.Exists(_filePath))
                    {
                        Log4Helper.Info("未找到文件：" + _filePath);
                        throw new Exception("未找到文件：" + _filePath);
                    }
                    uploadModel.ExcelItemList = this.GetImportDataByExcel(_filePath, uploadModel);
                }

                uploadTimeLog += "，完成读取Excel数据：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                Log4Helper.Info(uploadTimeLog);

                if (uploadModel.ExcelItemList == null || !uploadModel.ExcelItemList.Any())
                {
                    Log4Helper.Info($"未读取到数据[MedicalDataId = {entity.Id}]");
                    throw new Exception($"未读取到数据[MedicalDataId = {entity.Id}]");           //解析Excel的文件
                }

                #endregion

                #region 获取一些基础数据

                uploadModel.OrganismList = this.MedicalOrganismService.Query(m => true, 0, false, m => m.Sort).ToList(); //医学数据 细菌（基础数据）
                uploadModel.AntibioticList = this.MedicalAntibioticService.Query().ToList();//医学数据 抗生素（基础数据）
                uploadModel.AntibioticRuleList = this.MedicalAntibioticRuleService.Query().ToList();       //医学数据 抗生素规则（基础数据）
                uploadModel.WardTypeList = this.MedicalDataWardTypeService.Query(m => true, 0, false, m => m.Sort).ToList(); //医学数据 科室类别 数据 （基础数据）
                uploadModel.DepartmentTypeList = this.MedicalDepartmentTypeService.Query(m => true, 0, false, m => m.Sort).ToList();//医学数据 系统内置标准科室分类 数据 （基础数据）
                uploadModel.SpecTypeList = this.MedicalSpecTypeService.Query(m => true, 0, false, m => m.Sort).ToList();    //医学数据 标本类型（基础数据）
                uploadModel.OrganismTypeList = this.MedicalOrganismTypeService.Query(m => true, 0, false, m => m.Sort).ToList();   //医学数据 细菌类型（基础数据）
                uploadModel.HospitalWardList = this.HospitalWardLocationService.Query(m => m.HospitalId == uploadModel.HospitalEntity.Id).ToList();//对应医院的编码替换数据（基础数据）

                #endregion

                #region 验证数据文件

                Log4Helper.Info("开始验证数据");

                this.MedicalAntibioticRuleService.ValidateUploadData(uploadModel);
                uploadTimeLog += "，验证数据完成：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                Log4Helper.Info(uploadTimeLog);

                #endregion

                //做成服务来处理，用服务来计算规则，这里将每行数据插入到数据库，服务的时候每次读取数据产生对应的规则数据
                int index = 0;

                //1、保存验证信息，在读取Excel的时候删除的数据
                if (uploadModel.ValidateList != null && uploadModel.ValidateList.Any())
                {
                    foreach (var node in uploadModel.ValidateList)
                    {
                        node.MedicalDataId = uploadModel.MedicalEntity.Id;
                        node.Sort = index++;
                        node.InsertTime = DateTime.Now;
                        node.UpdateTime = DateTime.Now;
                        node.Version = 0;
                        node.Mark = 1;
                        node.Id = CommonHelper.GuidToLongID;
                        node.DeleteTime = DateTime.Parse("1900-01-01 00:00");
                    }
                }

                //2、把上传以的医学数据项保存到数据库
                foreach (var item in uploadModel.ItemList)
                {
                    item.MedicalDataId = uploadModel.MedicalEntity.Id;
                    item.InsertTime = DateTime.Now;
                    item.UpdateTime = DateTime.Now;
                    item.Version = 0;
                    item.Mark = 1;
                    item.DeleteTime = DateTime.Parse("1900-01-01 00:00");

                    if (item.IsValid)
                        uploadModel.BaseMessage.SuccessCount += 1;
                }

                uploadTimeLog += "，保存数据库之前整理数据完成：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                //4、插入明细数据
                this.BatchInsertItemData(uploadModel);
                uploadTimeLog += "，明细数据插入到数据库完成：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                //5、插入验证的数据
                this.BatchInsertValidateData(uploadModel);
                uploadTimeLog += "，验证数据插入到数据库完成：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

                #region 6、更新医学数据主表

                uploadModel.MedicalEntity.Status = uploadModel.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Error).Count() > 0 ? (int)MedicalDataStatusEnum.Invalid : (int)MedicalDataStatusEnum.Effective;
                uploadModel.MedicalEntity.UploadMessage = new UploadMessageModel() { BaseMessage = uploadModel.BaseMessage }.SerializeObject();
                this.Update(uploadModel.MedicalEntity);

                uploadTimeLog += "，完成业务处理时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                IActionLogService actionLogService = EngineContext.Current.Resolve<IActionLogService>();
                uploadModel.UploadResultLog = "上传医学数据完成，结果：" + string.Format("共处理1条数据，共读取：{0}条数据，共成功：{1}条数据，处理的医学数据Id：{2}。时间记录：{3}", uploadModel.BaseMessage.ReadCount, uploadModel.ItemList.Count, uploadModel.MedicalEntity.Id, uploadTimeLog);

                #endregion

                //7、更新之前上传的数据显示状态
                this.BatchInsertUpdateDisplay(uploadModel);

                //8、添加操作日志和记录处理日志
                actionLogService.Insert(ActionType.Create, ActionSource.Web, uploadModel.MemberEntity.Id, uploadModel.MemberEntity.Name, uploadModel.UploadResultLog, "");
                Log4Helper.Info($"记录日志-保存数据到数据库完成({entity.Id})：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")},{uploadTimeLog},{uploadModel.UploadResultLog}");

                return uploadModel;
            }
            catch (Exception ex)
            {
                Log4Helper.Info($"发生异常(id = {entity.Id})");
                Log4Helper.Error(typeof(MedicalDataService), ex);
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                throw ex;
            }
        }

        /// <summary>
        /// 自动化作业处理数据  批量把医学明细数据插入到数据库中
        /// </summary>
        /// <param name="model">上传的数据</param>
        public void BatchInsertItemData(UploadMedicalResult model)
        {
            var connection = DapperHelper.GetConnection();
            connection.Open();

            //先将list的数据转换成datatable
            var table = DataTableHelper.ToDataTable<MedicalDataItem>(model.ItemList);
            using (SqlBulkCopy sbc = new SqlBulkCopy(connection))
            {
                try
                {
                    sbc.BulkCopyTimeout = 90; // 超时时间
                    sbc.BatchSize = 10000; // 分批插入， 每次10000条数据
                    for (int i = 0; i < table.Columns.Count; i++)
                        sbc.ColumnMappings.Add(table.Columns[i].ColumnName, table.Columns[i].ColumnName); //设置目标表和源数据的列映射

                    sbc.DestinationTableName = "MedicalDataItem";  //取得目标表名
                    sbc.WriteToServer(table);
                    sbc.Close();

                    table.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    else
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 自动化作业处理数据  批量把医学的验证结果数据插入到数据库中
        /// </summary>
        /// <param name="model">上传的数据</param>
        public void BatchInsertValidateData(UploadMedicalResult model)
        {
            var connection = DapperHelper.GetConnection();
            connection.Open();

            //先将list的数据转换成datatable
            var table = DataTableHelper.ToDataTable<MedicalDataItemValidate>(model.ValidateList);
            using (SqlBulkCopy sbc = new SqlBulkCopy(connection))
            {
                try
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                        sbc.ColumnMappings.Add(table.Columns[i].ColumnName, table.Columns[i].ColumnName); //设置目标表和源数据的列映射

                    sbc.DestinationTableName = "MedicalDataItemValidate";  //取得目标表名
                    sbc.WriteToServer(table);
                    sbc.Close();

                    table.Dispose();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    else
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 自动化作业处理数据  修改数据的显示状态
        /// 规则：一个医院同一年，同一季度，同一项目，最后上传的显示出来，之前的则不显示出来
        /// </summary>
        /// <param name="model">上传的数据</param>
        public void BatchInsertUpdateDisplay(UploadMedicalResult model)
        {
            //之前的设置成不显示
            string sql = String.Format("UPDATE dbo.[MedicalData] SET [Display] = {0} WHERE   [HospitalId] = {1} AND Year={2} AND Quarter={3} AND ProjectType={4} ;",
                "0", model.HospitalEntity.Id, model.MedicalEntity.Year, model.MedicalEntity.Quarter, model.MedicalEntity.ProjectType);

            //本次上传的设置为显示
            string sql2 = String.Format("UPDATE dbo.[MedicalData] SET [Display] = {0} WHERE ID={1} ;",
                "1", model.MedicalEntity.Id);
            using (var conn = DapperHelper.GetConnection())
            {
                conn.Execute(sql + sql2, null);
            }
        }

        /// <summary>
        /// 自动化作业处理数据  发送邮件通知提醒用户
        /// </summary>
        private void AutoDisposeSendEmail(UploadMedicalResult model)
        {
            string url = "http://chinets.com/Medicine/Detail/" + model.MedicalEntity.Id.ToString();
            int errorCount = model.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Error).Count();
            int warningCount = model.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Warning).Count();
            int hintCount = model.ValidateList.Where(m => m.Level == (int)MedicalDataItemValidateLevelEnum.Hint).Count();

            string uploadResult = string.Format("本次共读取：{0}行数据，本次共成功上传{1}行数据，共产生：{2}个错误、{3}个警告、{4}个提醒。",
                model.BaseMessage.ReadCount, model.BaseMessage.SuccessCount, errorCount, warningCount, hintCount);

            string htmlStyle = "<style type='text/css'>*{margin:0;font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;box-sizing:border-box;font-size:14px}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100% !important;height:100%;line-height:1.6em}table td{vertical-align:top}body{background-color:#ecf0f5;color:#6c7b88}.body-wrap{background-color:#ecf0f5;width:100%}.container{display:block !important;max-width:600px !important;margin:0 auto !important;clear:both !important}.content{max-width:600px;margin:0 auto;display:block;padding:20px}.main{background-color:#fff;border-bottom:2px solid #d7d7d7}.content-wrap{padding:20px}.content-block{padding:0 0 20px}.header{width:100%;margin-bottom:20px}.footer{width:100%;clear:both;color:#999;padding:20px}.footer p,.footer a,.footer td{color:#999;font-size:12px}h1,h2,h3{font-family:'Helvetica Neue',Helvetica,Arial,'Lucida Grande',sans-serif;color:#1a2c3f;margin:30px 0 0;line-height:1.2em;font-weight:400}h1{font-size:32px;font-weight:500}h2{font-size:24px}h3{font-size:18px}h4{font-size:14px;font-weight:600}p,ul,ol{margin-bottom:10px;font-weight:normal}p li,ul li,ol li{margin-left:5px;list-style-position:inside}a{color:#348eda;text-decoration:underline}.btn-primary{text-decoration:none;color:#FFF;background-color:#42A5F5;line-height:1.5em;text-align:center;cursor:pointer;display:inline-block;text-transform:capitalize;margin-right:30px;padding:10px;font-size:12px}.last{margin-bottom:0}.first{margin-top:0}.aligncenter{text-align:center}.alignright{text-align:right}.alignleft{text-align:left}.clear{clear:both}.alert{font-size:16px;color:#fff;font-weight:500;padding:20px;text-align:center}.alert a{color:#fff;text-decoration:none;font-weight:500;font-size:16px}.alert.alert-warning{background-color:#FFA726}.alert.alert-bad{background-color:#ef5350}.alert.alert-good{background-color:#8BC34A}.invoice{margin:25px auto;text-align:left;width:100%}.invoice td{padding:5px 0}.invoice .invoice-items{width:100%}.invoice .invoice-items td{border-top:#eee 1px solid}.invoice .invoice-items .total td{border-top:2px solid #6c7b88;font-size:18px},*{margin:0;font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;box-sizing:border-box;font-size:14px}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100% !important;height:100%;line-height:1.6em}table td{vertical-align:top}body{background-color:#ecf0f5;color:#6c7b88}.body-wrap{background-color:#ecf0f5;width:100%}.container{display:block !important;max-width:600px !important;margin:0 auto !important;clear:both !important}.content{max-width:600px;margin:0 auto;display:block;padding:20px}.main{background-color:#fff;border-bottom:2px solid #d7d7d7}.content-wrap{padding:20px}.content-block{padding:0 0 20px}.header{width:100%;margin-bottom:20px}.footer{width:100%;clear:both;color:#999;padding:20px}.footer p,.footer a,.footer td{color:#999;font-size:12px}h1,h2,h3{font-family:'Helvetica Neue',Helvetica,Arial,'Lucida Grande',sans-serif;color:#1a2c3f;margin:30px 0 0;line-height:1.2em;font-weight:400}h1{font-size:32px;font-weight:500}h2{font-size:24px}h3{font-size:18px}h4{font-size:14px;font-weight:600}p,ul,ol{margin-bottom:10px;font-weight:normal}p li,ul li,ol li{margin-left:5px;list-style-position:inside}a{color:#348eda;text-decoration:underline}.btn-primary{text-decoration:none;color:#FFF;background-color:#42A5F5;line-height:1.5em;text-align:center;cursor:pointer;display:inline-block;text-transform:capitalize;margin-right:30px;padding:10px;font-size:12px}.last{margin-bottom:0}.first{margin-top:0}.aligncenter{text-align:center}.alignright{text-align:right}.alignleft{text-align:left}.clear{clear:both}.alert{font-size:16px;color:#fff;font-weight:500;padding:20px;text-align:center}.alert a{color:#fff;text-decoration:none;font-weight:500;font-size:16px}.alert.alert-warning{background-color:#FFA726}.alert.alert-bad{background-color:#ef5350}.alert.alert-good{background-color:#8BC34A}.invoice{margin:25px auto;text-align:left;width:100%}.invoice td{padding:5px 0}.invoice .invoice-items{width:100%}.invoice .invoice-items td{border-top:#eee 1px solid}.invoice .invoice-items .total td{border-top:2px solid #6c7b88;font-size:18px}</style>";
            string content = htmlStyle + string.Format(@"
              <table class='body-wrap'>
                    <tr>
                        <td></td>
                        <td class='container' width='600'>
                            <div class='content'>
                                <table class='main' width='100%' cellpadding='0' cellspacing='0'>
                                    <tr>
                                        <td class='content-wrap'>
                                            <meta itemprop='name' content='Confirm Email' />
                                            <table width='100%' cellpadding='0' cellspacing='0'>
                                                <tr> <td> <div style='text-align: center;margin-bottom: 20px;'><img src='http://chinets.com/Content/Images/logo_2.png' /></div> </td>  </tr>
                                                <tr><td class='content-block' style='    border-bottom: #eee 1px solid;padding-bottom: 15px;'> <h3>数据上传结果通知</h3></td></tr>
                                                <tr><td class='content-block' style='padding-top: 20px;'>  亲爱的用户，您好：</td></tr>
                                                <tr><td class='content-block'> 您上传的数据已经智能化分析完成，详细分析结果如下：<br /> {0} </td></tr>
                                                <tr><td class='content-block'></td></tr>
                                                <tr>
                                                    <td class='content-block aligncenter'>
                                                        <a href='{1}' target='_blank' class='btn-primary' style='text-decoration: none !important;' >查看分析结果</a>
                                                        <a href='{2}' target='_blank' class='btn-primary' style=' background-color:#29b2a6;text-decoration: none !important;'>下载原始上传文件</a>
                                                        <a href='{3}' target='_blank' class='btn-primary'  style='  background-color:#5cb85c;text-decoration: none !important;' >下载校验后的文件</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div class='footer'>
                                    <table width='100%'><tr> <td class='aligncenter content-block'> <a href='http://chinets.com/' target='_blank'>CHINET 数据云</a></td></tr></table>
                                </div>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>
                ", uploadResult, url, "http://chinets.com/Medicine/DownloadOriginalData/" + model.MedicalEntity.Id, "http://chinets.com/Medicine/DownloadNewData/" + model.MedicalEntity.Id);

            MessageEmail email = new MessageEmail()
            {
                Content = content,
                Email = model.MemberEntity.MedicineEmail,
                MemberId = model.MemberEntity.Id,
                MemberName = model.MemberEntity.Name,
                Remark = "",
                SceneType = "数据分析结果通知",
                SendType = 1,
                Source = "web",
                Status = 1,//状态：1、待发送   2：已发送   3：失败
                Title = "医学数据分析结果通知"
            };

            try
            {
                //更新数据处理日志
                var logEntity = this.MedicalDataDisposeLogService.QueryEntity(m => m.MedicalDataId == model.MedicalEntity.Id);
                logEntity.DisposeTime = DateTime.Now;
                logEntity.Status = (int)MedicalDataDisposeLogStatusEnum.Success;
                logEntity.Result = uploadResult;
                this.MedicalDataDisposeLogService.Update(logEntity);

                //发送邮件
                string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
                string servicePassword = ConfigHelper.GetConfigString("message.email.password");
                int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
                bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
                string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
                string displayname = ConfigHelper.GetConfigString("message.email.displayname");

                bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { email.Email }, email.Title, email.Content);
                if (result)
                {
                    email.Status = 2;
                    model.UploadResultLog += "，发送邮件成功";
                }
                else
                {
                    email.Status = 3;
                    model.UploadResultLog += "，发送邮件失败";
                }
            }
            catch (Exception ex)
            {
                email.Remark = "发送邮件发生异常，异常信息：" + ex.Message;
            }

            //保存发送邮件记录
            EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);
        }

        /// <summary>
        /// 自动化作业处理数据  发送邮件通知提醒用户
        /// </summary>
        private void AutoDisposeSendEmail_Error(Member member, string result)
        {
            string htmlStyle = "<style type='text/css'>*{margin:0;font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;box-sizing:border-box;font-size:14px}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100% !important;height:100%;line-height:1.6em}table td{vertical-align:top}body{background-color:#ecf0f5;color:#6c7b88}.body-wrap{background-color:#ecf0f5;width:100%}.container{display:block !important;max-width:600px !important;margin:0 auto !important;clear:both !important}.content{max-width:600px;margin:0 auto;display:block;padding:20px}.main{background-color:#fff;border-bottom:2px solid #d7d7d7}.content-wrap{padding:20px}.content-block{padding:0 0 20px}.header{width:100%;margin-bottom:20px}.footer{width:100%;clear:both;color:#999;padding:20px}.footer p,.footer a,.footer td{color:#999;font-size:12px}h1,h2,h3{font-family:'Helvetica Neue',Helvetica,Arial,'Lucida Grande',sans-serif;color:#1a2c3f;margin:30px 0 0;line-height:1.2em;font-weight:400}h1{font-size:32px;font-weight:500}h2{font-size:24px}h3{font-size:18px}h4{font-size:14px;font-weight:600}p,ul,ol{margin-bottom:10px;font-weight:normal}p li,ul li,ol li{margin-left:5px;list-style-position:inside}a{color:#348eda;text-decoration:underline}.btn-primary{text-decoration:none;color:#FFF;background-color:#42A5F5;line-height:1.5em;text-align:center;cursor:pointer;display:inline-block;text-transform:capitalize;margin-right:30px;padding:10px;font-size:12px}.last{margin-bottom:0}.first{margin-top:0}.aligncenter{text-align:center}.alignright{text-align:right}.alignleft{text-align:left}.clear{clear:both}.alert{font-size:16px;color:#fff;font-weight:500;padding:20px;text-align:center}.alert a{color:#fff;text-decoration:none;font-weight:500;font-size:16px}.alert.alert-warning{background-color:#FFA726}.alert.alert-bad{background-color:#ef5350}.alert.alert-good{background-color:#8BC34A}.invoice{margin:25px auto;text-align:left;width:100%}.invoice td{padding:5px 0}.invoice .invoice-items{width:100%}.invoice .invoice-items td{border-top:#eee 1px solid}.invoice .invoice-items .total td{border-top:2px solid #6c7b88;font-size:18px},*{margin:0;font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;box-sizing:border-box;font-size:14px}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100% !important;height:100%;line-height:1.6em}table td{vertical-align:top}body{background-color:#ecf0f5;color:#6c7b88}.body-wrap{background-color:#ecf0f5;width:100%}.container{display:block !important;max-width:600px !important;margin:0 auto !important;clear:both !important}.content{max-width:600px;margin:0 auto;display:block;padding:20px}.main{background-color:#fff;border-bottom:2px solid #d7d7d7}.content-wrap{padding:20px}.content-block{padding:0 0 20px}.header{width:100%;margin-bottom:20px}.footer{width:100%;clear:both;color:#999;padding:20px}.footer p,.footer a,.footer td{color:#999;font-size:12px}h1,h2,h3{font-family:'Helvetica Neue',Helvetica,Arial,'Lucida Grande',sans-serif;color:#1a2c3f;margin:30px 0 0;line-height:1.2em;font-weight:400}h1{font-size:32px;font-weight:500}h2{font-size:24px}h3{font-size:18px}h4{font-size:14px;font-weight:600}p,ul,ol{margin-bottom:10px;font-weight:normal}p li,ul li,ol li{margin-left:5px;list-style-position:inside}a{color:#348eda;text-decoration:underline}.btn-primary{text-decoration:none;color:#FFF;background-color:#42A5F5;line-height:1.5em;text-align:center;cursor:pointer;display:inline-block;text-transform:capitalize;margin-right:30px;padding:10px;font-size:12px}.last{margin-bottom:0}.first{margin-top:0}.aligncenter{text-align:center}.alignright{text-align:right}.alignleft{text-align:left}.clear{clear:both}.alert{font-size:16px;color:#fff;font-weight:500;padding:20px;text-align:center}.alert a{color:#fff;text-decoration:none;font-weight:500;font-size:16px}.alert.alert-warning{background-color:#FFA726}.alert.alert-bad{background-color:#ef5350}.alert.alert-good{background-color:#8BC34A}.invoice{margin:25px auto;text-align:left;width:100%}.invoice td{padding:5px 0}.invoice .invoice-items{width:100%}.invoice .invoice-items td{border-top:#eee 1px solid}.invoice .invoice-items .total td{border-top:2px solid #6c7b88;font-size:18px}</style>";
            string content = $@"{htmlStyle}
                    <table class='body-wrap'>
                        <tr>
                            <td></td>
                            <td class='container' width='600'>
                                <div class='content'>
                                    <table class='main' width='100%' cellpadding='0' cellspacing='0'>
                                        <tr>
                                            <td class='content-wrap'>
                                                <meta itemprop='name' content='Confirm Email' />
                                                <table width='100%' cellpadding='0' cellspacing='0'>
                                                    <tr> <td> <div style='text-align: center;margin-bottom: 20px;'><img src='http://chinets.com/Content/Images/logo_2.png' /></div> </td>  </tr>
                                                    <tr><td class='content-block' style='    border-bottom: #eee 1px solid;padding-bottom: 15px;'> <h3>数据上传结果通知</h3></td></tr>
                                                    <tr><td class='content-block' style='padding-top: 20px;'>  亲爱的用户，您好：</td></tr>
                                                    <tr><td class='content-block'> 您上传的数据已经智能化分析完成，详细分析结果如下：<br /> {result} </td></tr>
                                                    <tr><td class='content-block'></td></tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class='footer'>
                                        <table width='100%'><tr> <td class='aligncenter content-block'> <a href='http://chinets.com/' target='_blank'>CHINET 数据云</a></td></tr></table>
                                    </div>
                                </div>
                            </td>
                            <td></td>
                        </tr>
                    </table>";

            MessageEmail email = new MessageEmail()
            {
                Content = content,
                Email = (member?.MedicineEmail) ?? "",
                MemberId = (member?.Id) ?? 0,
                MemberName = (member?.Name) ?? "",
                Remark = "",
                SceneType = "数据分析结果通知",
                SendType = 1,
                Source = "web",
                Status = 1,//状态：1、待发送   2：已发送   3：失败
                Title = "医学数据分析结果通知"
            };


            if (member == null || member.Id <= 0)
            {
                email.Remark = "获取用户信息失败";

                //保存发送邮件记录
                EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);
                return;
            }

            if (string.IsNullOrWhiteSpace(member.MedicineEmail))
            {
                email.Remark = "用户未配置接受邮箱【MedicineEmail】";
                //保存发送邮件记录
                EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);
                return;
            }

            try
            {
                //发送邮件
                string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
                string servicePassword = ConfigHelper.GetConfigString("message.email.password");
                int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
                bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
                string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
                string displayname = ConfigHelper.GetConfigString("message.email.displayname");

                bool email_result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { email.Email }, email.Title, email.Content);
                if (email_result)
                {
                    email.Status = 2;
                }
                else
                {
                    email.Status = 3;
                }
            }
            catch (Exception ex)
            {
                email.Remark = "发送邮件发生异常，异常信息：" + ex.Message;
            }

            //保存发送邮件记录
            EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);
        }

        /// <summary>
        /// 根据Id检查上传数据的状态
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns>true 表示成功  false 表示失败</returns>
        public bool CheckStatus(long id)
        {
            if (id <= 0) return false;
            var entity = this.QueryEntity(m => m.Id == id && m.Mark > 0);
            if ((entity != null && entity.Id > 0 && !string.IsNullOrWhiteSpace(entity.DisposeFilePath)))
            {
                if (this.MedicalDataItemValidateService.Count(m => m.Mark > 0 && m.MedicalDataId == entity.Id && m.Level == (int)MedicalDataItemValidateLevelEnum.Error) == 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("文件上传已完成, 但存在验证错误.");
                }
            }

            return false;
        }

        #endregion


        /// <summary>
        /// 后台，打包下载文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="downloadEnum"></param>
        /// <returns></returns>
        public string Download(IEnumerable<long> ids, MedicalDataDownloadEnum downloadEnum)
        {
            try
            {
                //var data = Query(r => ids.Contains(r.Id) && r.Mark > 0 && r.Status == (int)MedicalDataStatusEnum.Effective);
                var data = Query(r => ids.Contains(r.Id) && r.Mark > 0);
                IEnumerable<string> filesPath = null;
                switch (downloadEnum)
                {
                    case MedicalDataDownloadEnum.Original:
                        filesPath = data.Where(r => !string.IsNullOrWhiteSpace(r.UploadFilePath)).Select(r => r.UploadFilePath);
                        break;
                    case MedicalDataDownloadEnum.FaultTolerant:
                        filesPath = data.Where(r => !string.IsNullOrWhiteSpace(r.DisposeFilePath)).Select(r => r.DisposeFilePath);
                        break;
                    default:
                        break;
                }

                if (filesPath != null && filesPath.Count() > 0)
                {
                    string outPath = $"/Content/Download/MedicalData/{Guid.NewGuid().ToString("N")}.zip";
                    string zipPath = System.Web.HttpContext.Current.Server.MapPath(outPath);
                    if (CompressFile(filesPath, zipPath))
                    {
                        return outPath;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 后台打包下载文件
        /// </summary>
        /// <param name="sqlToIds"></param>
        /// <param name="downloadEnum"></param>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        public string Download(String sqlToIds, MedicalDataDownloadEnum downloadEnum, object sqlParam = null)
        {
            IEnumerable<long> ids = new List<long>();
            try
            {
                using (var conn = DapperHelper.GetConnection())
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    ids = conn.Query<long>(sqlToIds, sqlParam);
                }
            }
            catch (Exception)
            {

            }
            return Download(ids, downloadEnum);
        }

        public string DownloadByProjectType(List<long> projectTypes, MedicalDataDownloadEnum downloadEnum)
        {
            try
            {
                var data = Query(r => projectTypes.Contains(r.ProjectType) && r.Mark > 0 && r.Status == (int)MedicalDataStatusEnum.Effective);
                IEnumerable<string> filesPath = null;
                switch (downloadEnum)
                {
                    case MedicalDataDownloadEnum.Original:
                        filesPath = data.Where(r => !string.IsNullOrWhiteSpace(r.UploadFilePath)).Select(r => r.UploadFilePath);
                        break;
                    case MedicalDataDownloadEnum.FaultTolerant:
                        filesPath = data.Where(r => !string.IsNullOrWhiteSpace(r.DisposeFilePath)).Select(r => r.DisposeFilePath);
                        break;
                    default:
                        break;
                }

                if (filesPath != null && filesPath.Count() > 0)
                {
                    string outPath = $"/Content/Download/MedicalData/{Guid.NewGuid().ToString("N")}.zip";

                    if (CompressFile(filesPath, System.Web.HttpContext.Current.Server.MapPath(outPath)))
                    {
                        return outPath;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 单个获取原始文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        public string DownloadOriginalData(long id)
        {
            var entity = this.QueryEntity(m => m.Id == id && m.Mark > 0);
            if (string.IsNullOrWhiteSpace(entity.UploadFilePath))
                throw new Exception("原始数据文件丢失");

            return entity.UploadFilePath;
        }

        /// <summary>
        /// 生成容错文件，给接口使用的。DownloadNewData 函数则是给前台页面使用的
        /// </summary>
        /// <param name="medicalId">医学数据的id，如果传递了则获取指定的数据，没有传递则系统查询所有数据</param>
        /// <returns></returns>
        public string CreateNewFile(long medicalId = 0)
        {
            List<MedicalData> list = new List<MedicalData>();
            if (medicalId > 0)
            {
                //传递了则生成指定文件的
                var entity = this.QueryEntity(m => m.Id == medicalId && m.Mark > 0);
                if (entity == null || entity.Id <= 0 || !string.IsNullOrWhiteSpace(entity.DisposeFilePath))
                    throw new Exception("Id：" + medicalId + " 数据不存在或文件已生成了");

                list.Add(entity);
            }
            else
            {
                //没有传递，则生成所有
                list = this.Query(m => m.Mark > 0) ?? new List<MedicalData>();
                list = list.Where(m => string.IsNullOrWhiteSpace(m.DisposeFilePath)).ToList();
            }

            if (list == null || !list.Any())
                throw new Exception("本次操作未查询到数据");

            int successCount = 0;
            int errorCount = 0;

            foreach (var item in list)
            {
                try
                {
                    this.DownloadNewData(item.Id);
                    successCount++;
                }
                catch (Exception)
                {
                    errorCount++;
                }
            }

            return string.Format("共读取：{0}，成功数量：{1}，失败数量：{2}", list.Count.ToString(), successCount.ToString(), errorCount.ToString());
        }

        /// <summary>
        /// 单个获取容错文件的相对地址
        /// </summary>
        /// <param name="id">上传的数据Id</param>
        /// <returns></returns>
        public string DownloadNewData(long id)
        {
            try
            {
                var entity = this.QueryEntity(m => m.Id == id && m.Mark > 0);
                if (entity == null || entity.Id <= 0)
                {
                    Log4Helper.Error($"ManageSystem.Services.Medicine.MedicalDataService.DownloadNewData({id}) => {id}不存在");
                    //throw new Exception("数据不存在或者错误"); // 2020-01-11 注释
                    // 2020-01-11 新加
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(entity.DisposeFilePath))
                {
                    if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(entity.DisposeFilePath)))
                    {
                        return entity.DisposeFilePath; //如果生存了的，则直接返回地址
                    }
                }
                Log4Helper.Info($"文件不存在，继续生成容错文件{entity.DisposeFilePath}");

                string fileExtension = Path.GetExtension(entity.UploadFilePath).ToUpperInvariant();

                entity.DisposeFilePath = string.Empty;
                if (string.IsNullOrWhiteSpace(entity.DisposeFilePath) || (".DBF".Equals(fileExtension) && !".DBF".Equals(Path.GetExtension(entity.DisposeFilePath).ToUpperInvariant())))
                {
                    HospitalTeamList hospitalTeam = new HospitalTeamList();
                    var Team = _teamService.GetTeamsbyHospitalId(entity.HospitalId);
                    if (Team != null)
                    {
                        hospitalTeam.Id = Team.Id;
                        hospitalTeam.Code = Team.project_code;
                        hospitalTeam.Name = Team.Title;
                        if (Team == null || Team.Id <= 0)
                        {
                            Log4Helper.Error($"ManageSystem.Services.Medicine.MedicalDataService.DownloadNewData({id}) => {id}关联医院不存在");
                            //throw new Exception("数据不存在或者错误"); // 2020-01-11 注释
                            return null; // 2020-01-11 新加

                        }
                    }
                    else
                    {
                        var hospital = HospitalService.QueryEntity(entity.HospitalId);
                        hospitalTeam.Id = hospital.Id;
                        hospitalTeam.Code = null;
                        hospitalTeam.Name = hospital.Name;
                        if (hospital == null || hospital.Id <= 0)
                        {
                            Log4Helper.Error($"ManageSystem.Services.Medicine.MedicalDataService.DownloadNewData({id}) => {id}关联医院不存在");
                            //throw new Exception("数据不存在或者错误"); // 2020-01-11 注释
                            return null; // 2020-01-11 新加

                        }
                    }
                    string tempPath = this.MedicalDataItemService.GetDBF(entity, hospitalTeam);

                    if (string.IsNullOrWhiteSpace(tempPath))
                    {
                        Log4Helper.Error($"ManageSystem.Services.Medicine.MedicalDataService.DownloadNewData({id}) => 未能成功返回DBF文件路径");
                        //throw new Exception("生成容错文件失败，请重试"); // 2020-01-11 注释
                        return null; // 2020-01-11 新加
                    }

                    //保存到数据库
                    entity.DisposeFilePath = tempPath;
                    this.Update(entity);

                    Log4Helper.Info(this.GetType(), $"容错文件生成成功\r\n{entity.DisposeFilePath}");
                }

                return entity.DisposeFilePath;
            }
            catch (Exception ex)
            {
                Log4Helper.Error($"ManageSystem.Services.Medicine.MedicalDataService.DownloadNewData({id}) => Exception");
                Log4Helper.Error(this.GetType(), ex);
                return null;
            }
        }

        #region 压缩文件

        /// <summary>
        /// 压缩多个文件/文件夹
        /// </summary>
        /// <param name="sourceList">源文件/文件夹路径列表</param>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="comment">注释信息</param>
        /// <param name="password">压缩密码</param>
        /// <param name="compressionLevel">压缩等级，范围从0到9，可选，默认为6</param>
        /// <returns></returns>
        private bool CompressFile(IEnumerable<string> sourceList, string zipFilePath,string comment = null, string password = null, int compressionLevel = 6)
        {
            bool result = false;

            try
            {
                //检测目标文件所属的文件夹是否存在，如果不存在则建立
                string zipFileDirectory = Path.GetDirectoryName(zipFilePath);
                if (!Directory.Exists(zipFileDirectory))
                {
                    Directory.CreateDirectory(zipFileDirectory);
                }

                Dictionary<string, string> dictionaryList = PrepareFileSystementities(sourceList);

                using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFilePath)))
                {
                   // zipStream.Password = password;//设置密码
                    zipStream.SetComment(comment);//添加注释
                    zipStream.SetLevel(compressionLevel);//设置压缩等级

                    foreach (string key in dictionaryList.Keys)//从字典取文件添加到压缩文件
                    {
                        if (File.Exists(key))//判断是文件还是文件夹
                        {
                            FileInfo fileItem = new FileInfo(key);

                            using (FileStream readStream = fileItem.Open(FileMode.Open,
                                FileAccess.Read, FileShare.Read))
                            {
                                ZipEntry zipEntry = new ZipEntry(dictionaryList[key]);
                                zipEntry.DateTime = fileItem.LastWriteTime;
                                zipEntry.Size = readStream.Length;
                                zipStream.PutNextEntry(zipEntry);
                                int readLength = 0;
                                byte[] buffer = new byte[8192];

                                do
                                {
                                    readLength = readStream.Read(buffer, 0, 8192);
                                    zipStream.Write(buffer, 0, readLength);
                                } while (readLength == 8192);

                                readStream.Close();
                            }
                        }
                        else//对文件夹的处理
                        {
                            ZipEntry zipEntry = new ZipEntry(dictionaryList[key] + "/");
                            zipStream.PutNextEntry(zipEntry);
                        }
                    }

                    zipStream.Flush();
                    zipStream.Finish();
                    zipStream.Close();
                }

                result = true;
            }
            catch (System.Exception ex)
            {
                throw new Exception("压缩文件失败", ex);
            }

            return result;
        }

        /// <summary>
        /// 为压缩准备文件系统对象
        /// </summary>
        /// <param name="sourceFileEntityPathList"></param>
        /// <returns></returns>
        private Dictionary<string, string> PrepareFileSystementities(IEnumerable<string> sourceFileEntityPathList)
        {
            Dictionary<string, string> fileEntityDictionary = new Dictionary<string, string>();//文件字典
            string parentDirectoryPath = "";
            foreach (string fileEntityPath in sourceFileEntityPathList)
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(fileEntityPath);
                //保证传入的文件夹也被压缩进文件
                if (path.EndsWith(@"\"))
                {
                    path = path.Remove(path.LastIndexOf(@"\"));
                }

                parentDirectoryPath = Path.GetDirectoryName(path) + @"\";

                if (parentDirectoryPath.EndsWith(@":\\"))//防止根目录下把盘符压入的错误
                {
                    parentDirectoryPath = parentDirectoryPath.Replace(@"\\", @"\");
                }

                //获取目录中所有的文件系统对象
                Dictionary<string, string> subDictionary = GetAllFileSystemEntities(path, parentDirectoryPath);

                //将文件系统对象添加到总的文件字典中
                foreach (string key in subDictionary.Keys)
                {
                    if (!fileEntityDictionary.ContainsKey(key))//检测重复项
                    {
                        fileEntityDictionary.Add(key, subDictionary[key]);
                    }
                }
            }
            return fileEntityDictionary;
        }

        /// <summary>
        /// 获取所有文件系统对象
        /// </summary>
        /// <param name="source">源路径</param>
        /// <param name="topDirectory">顶级文件夹</param>
        /// <returns>字典中Key为完整路径，Value为文件(夹)名称</returns>
        private Dictionary<string, string> GetAllFileSystemEntities(string source, string topDirectory)
        {
            Dictionary<string, string> entitiesDictionary = new Dictionary<string, string>();
            entitiesDictionary.Add(source, source.Replace(topDirectory, ""));

            if (Directory.Exists(source))
            {
                //一次性获取下级所有目录，避免递归
                string[] directories = Directory.GetDirectories(source, "*.*", SearchOption.AllDirectories);
                foreach (string directory in directories)
                {
                    entitiesDictionary.Add(directory, directory.Replace(topDirectory, ""));
                }

                string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    entitiesDictionary.Add(file, file.Replace(topDirectory, ""));
                }
            }

            return entitiesDictionary;
        }

        #endregion

        /// <summary>
        /// 根据数据上传的年份和季度，获取完整的显示名称
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="quarter">季度</param>
        /// <returns></returns>
        public string GetDataQuarter(int year, int quarter)
        {
            switch (quarter)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return year + "年 第" + quarter + "季度";
                case 5:
                    return year + "年 全年";
                case 6:
                    return year + "年 上半年";
                case 7:
                    return year + "年 下半年";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 根据项目id获取对应的项目名称
        /// </summary>
        /// <param name="project">项目id</param>
        /// <returns></returns>
        public string GetProejctName(long project)
        {
            return (_medicalDataProjectService.QueryEntity(project)?.Name) ?? "-";
        }

        /// <summary>
        /// 用户中心，项目数据管理
        /// </summary>
        /// <param name="projectItems"></param>
        /// <param name="hospital"></param>
        /// <param name="fileName"></param>
        /// <param name="projectItem"></param>
        /// <param name="year"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IQueryable<MedicalData> Query(List<long> projectItems, string hospital, string fileName, long projectItem, int year, int status)
        {
            IQueryable<MedicalData> data = null;

            if (projectItems != null && projectItems.Any() && projectItems.Count > 0)
                data = base._repository.Table.Where(m => m.Mark > 0 && m.Display == true && projectItems.Contains(m.ProjectType));
            else
                return base._repository.Table.Where(r => false).OrderByDescending(r => r.InsertTime);

            if (year > 1900)
                data = data.Where(m => m.Year == year);

            if (!string.IsNullOrWhiteSpace(hospital))
                data = data.Where(m => m.HospitalName.Contains(hospital));

            if (!string.IsNullOrWhiteSpace(fileName))
                data = data.Where(m => m.FileName.Contains(fileName));

            if (projectItem > 0)
                data = data.Where(m => m.ProjectType == projectItem);

            if (status > 0)
                data = data.Where(m => m.Status == status);

            return data.OrderByDescending(m => m.InsertTime);
        }


        #region  刷新数据专用，请勿随意操作

        /// <summary>
        /// 刷新之前老的文件名称
        /// 修改原始文件的路径，原始文件名称_年月日_医院名称.后缀
        /// </summary>
        /// <returns></returns>
        public string UpdateDisposeFilePath3()
        {
            var data = this.Query();
            var host = this.HospitalService.Query();
            int successCount = 0;

            foreach (var item in data)
            {
                var hostEntity = host.Where(m => m.Id == item.HospitalId).FirstOrDefault();
                if (hostEntity == null | hostEntity.Id <= 0) continue;

                //更新数据库，/Content/Upload/MedicalData/ZheJiang/20190131141514/W1619.dbf
                if (item.UploadFilePath.ToLower().Contains("_" + hostEntity.Name + "."))
                    continue;//已修改过了不能重复操作

                ///Content/Upload/MedicalData/ZheJiang/20190306080743/2018年新20190306.xlsx
                string newFilePath = item.UploadFilePath.Substring(0, item.UploadFilePath.LastIndexOf('.')) + "_" + hostEntity.Name + "." + this.GetExtensionName(item.UploadFilePath);
                if (string.IsNullOrWhiteSpace(newFilePath)) continue;

                try
                {
                    //原始文件
                    string oldFullPath = System.Web.HttpContext.Current.Server.MapPath(item.UploadFilePath);
                    string newFullPath = System.Web.HttpContext.Current.Server.MapPath(newFilePath);
                    //更新本地文件
                    FileInfo oldFile = new FileInfo(oldFullPath);
                    if (oldFile.Exists)
                        oldFile.CopyTo(newFullPath);

                    //更新数据库
                    item.UploadFilePath = newFilePath;
                    this.Update(item);

                    ++successCount;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return "处理完成，成功数量：" + successCount;
        }

        /// <summary>
        /// 刷新之前老的文件名称
        /// 修改原始文件的路径，原始文件名称_医院名称.后缀
        /// </summary>
        /// <returns></returns>
        public string UpdateDisposeFilePath4(int type)
        {
            var data = this.Query(r => r.Status != (int)MedicalDataStatusEnum.Invalid).OrderBy(R => R.InsertTime);
            var host = this.HospitalService.Query();

            int successCount = 0;
            bool uploadFlag = false;
            Log4Helper.Info($"data.Count() = {data.Count()}");

            foreach (var item in data)
            {

                uploadFlag = false;
                var hostEntity = host.Where(m => m.Id == item.HospitalId).FirstOrDefault();
                if (hostEntity == null | hostEntity.Id <= 0) continue;

                #region 原始文件
                if (type == 1 || type == 3)
                {
                    // 判断不为空且已/Content开头
                    if (!string.IsNullOrWhiteSpace(item.UploadFilePath) && item.UploadFilePath.ToUpperInvariant().StartsWith("/CONTENT"))
                    {
                        // 获得文件名
                        string fileName = Path.GetFileName(item.FileName);

                        if (string.IsNullOrWhiteSpace(fileName))
                        {
                            fileName = Path.GetFileNameWithoutExtension(item.UploadFilePath).Replace($"_{hostEntity.Name}", "");
                        }

                        // 新文件名：原始文件名称_医院名称.后缀
                        string newFileName = $"{fileName}_{hostEntity.Name}{Path.GetExtension(item.UploadFilePath)}";

                        // 判断原文件名是否等于新文件名
                        if (!Path.GetFileName(item.UploadFilePath).ToUpperInvariant().Equals(newFileName.ToUpperInvariant()))
                        {
                            if (item.UploadFilePath.StartsWith("\\"))
                            {
                                item.UploadFilePath = item.UploadFilePath.Replace("\\", "/");
                            }

                            try
                            {
                                string newFilePath = item.UploadFilePath.Substring(0, item.UploadFilePath.LastIndexOf('/')) + "/" + newFileName;
                                if (!string.IsNullOrWhiteSpace(newFilePath))
                                {
                                    // 原始文件路径
                                    string oldFullPath = System.Web.HttpContext.Current.Server.MapPath(item.UploadFilePath);
                                    // 目标文件路径
                                    string newFullPath = System.Web.HttpContext.Current.Server.MapPath(newFilePath);

                                    // 判断原文件路径是否等于新文件路径
                                    if (!item.UploadFilePath.ToUpperInvariant().Equals(newFilePath.ToUpperInvariant()))
                                    {
                                        //更新本地文件
                                        FileInfo oldFile = new FileInfo(oldFullPath);
                                        if (oldFile.Exists)
                                        {
                                            oldFile.CopyTo(newFullPath, true);

                                            //更新数据库
                                            item.UploadFilePath = newFilePath;
                                            uploadFlag = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log4Helper.Info(typeof(MedicalDataService), ex);
                            }
                        }
                    }
                }
                #endregion

                #region 容错文件
                if (type == 2 || type == 3)
                {
                    // 判断不为空且已/Content开头
                    if (!string.IsNullOrWhiteSpace(item.DisposeFilePath) && item.DisposeFilePath.ToUpperInvariant().StartsWith("/CONTENT"))
                    {
                        /**
                        * 上传数据的时候根据用户选择的年份和季度来生成文件名称。
                        * 上半年：w + 年份2位 + 16 + 医院编码.DBF
                        * 下半年： w + 年份2位 + 712 + 医院编码.DBF
                        * 全年：w + 年份2位 + T + 医院编码.DBF
                        * 如果医院编码没有，则使用医院的名称代替。
                        */

                        string newRcName = "";
                        string code = string.IsNullOrWhiteSpace(hostEntity.Code) ? hostEntity.Name : hostEntity.Code;
                        string year = item.Year.ToString().Substring(2); //年份的后2位
                        switch (item.Quarter)
                        {
                            case 5:
                                //全年：W+年份2位+T+医院编码.DBF   
                                newRcName = "w" + year + "t" + code + Path.GetExtension(item.DisposeFilePath);
                                break;
                            case 1:
                            case 2:
                            case 6:
                                //上半年：W+年份2位+16+医院编码.DBF 
                                newRcName = "w" + year + "16" + code + Path.GetExtension(item.DisposeFilePath);
                                break;
                            case 3:
                            case 4:
                            case 7:
                                //下半年： W+年份2位+712+医院编码.DBF
                                newRcName = "w" + year + "712" + code + Path.GetExtension(item.DisposeFilePath);
                                break;
                            default:
                                newRcName = "";
                                break;
                        }

                        if (!string.IsNullOrWhiteSpace(newRcName))
                        {
                            string newRcFilePath = "";

                            if (string.IsNullOrWhiteSpace(item.DisposeFilePath) && !string.IsNullOrWhiteSpace(item.UploadFilePath))
                            {
                                newRcFilePath = item.UploadFilePath.Substring(0, item.UploadFilePath.LastIndexOf('/')) + "/" + newRcName;
                            }
                            else
                            {
                                if (item.DisposeFilePath.StartsWith("\\"))
                                {
                                    item.DisposeFilePath = item.DisposeFilePath.Replace("\\", "/");
                                }
                                newRcFilePath = item.DisposeFilePath.Substring(0, item.DisposeFilePath.LastIndexOf('/')) + "/" + newRcName;
                            }

                            if (!string.IsNullOrWhiteSpace(newRcFilePath) && !string.IsNullOrWhiteSpace(item.DisposeFilePath))
                            {
                                //原容错文件
                                string oldFullPath = System.Web.HttpContext.Current.Server.MapPath(item.DisposeFilePath);
                                string newFullPath = System.Web.HttpContext.Current.Server.MapPath(newRcFilePath);
                                if (!item.DisposeFilePath.ToUpperInvariant().Equals(newRcFilePath.ToUpperInvariant()))
                                {
                                    try
                                    {
                                        //更新本地文件
                                        FileInfo oldFile = new FileInfo(oldFullPath);
                                        if (oldFile.Exists)
                                        {
                                            oldFile.CopyTo(newFullPath, true);

                                            //更新数据库
                                            item.DisposeFilePath = newRcFilePath;
                                            uploadFlag = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log4Helper.Info(typeof(MedicalDataService), ex);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                if (uploadFlag)
                {
                    this.Update(item);
                    ++successCount;
                }
            }

            return "处理完成，成功数量：" + successCount;
        }


        /// <summary>
        /// 获取名称的文件后缀名
        /// </summary>
        /// <param name="name"></param>
        /// <returns>错误将返回空字符串，不包含最后的.，返回的是小写形式</returns>
        private string GetExtensionName(string name)
        {
            try
            {
                string ext = name.Substring(name.LastIndexOf(".") + 1, name.Length - (name.LastIndexOf(".") + 1));

                return ext.ToLower();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion
        public IPagedList<MedicalData> QueryPage(long projectType, int year, int quarter, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0 && m.Display == true);

            if (year > 0)
                query = query.Where(m => m.Year == year);

            if (quarter > 0)
                query = query.Where(m => m.Quarter == quarter);

            if (projectType > 0)
            {
                query = query.Where(r => r.ProjectType == projectType);
            }

            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<MedicalData>(query, pageIndex, pageSize);

            return list;
        }

    }
}
