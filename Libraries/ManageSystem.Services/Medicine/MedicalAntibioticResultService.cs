using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Data;
using Dapper;
using ManageSystem.Services.Medicine.Model;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using ManageSystem.Core.DynamicLinq;
using ManageSystem.Core.Caching;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalAntibioticResult 
    /// </summary>
    public partial class MedicalAntibioticResultService : BaseService<MedicalAntibioticResult>, IMedicalAntibioticResultService
    {
        private readonly IMedicalOrganismService MedicalOrganismService;
        private readonly IMedicalAntibioticService MedicalAntibioticService;
        private readonly IMedicalAntibioticRuleService MedicalAntibioticRuleService;
        private readonly ISystemLogService SystemLogService;
        private readonly IMedicalDataItemService MedicalDataItemService;
        private readonly IMedicalDataService MedicalDataService;
        private readonly IActionLogService ActionLogService;
        private readonly ICacheManager CacheManager;

        public MedicalAntibioticResultService(
                            IRepository<MedicalAntibioticResult> repository,
                            IMedicalOrganismService medicalOrganismService,
                            IMedicalAntibioticService medicalAntibioticService,
                            IMedicalAntibioticRuleService medicalAntibioticRuleService,
                                   ISystemLogService systemLogService,
                                   IMedicalDataItemService medicalDataItemService,
                                   IMedicalDataService medicalDataService,
                                   IActionLogService actionLogService,
                                   ICacheManager cacheManager

            ) : base(repository)
        {
            this.MedicalOrganismService = medicalOrganismService;
            this.MedicalAntibioticService = medicalAntibioticService;
            this.MedicalAntibioticRuleService = medicalAntibioticRuleService;
            this.SystemLogService = systemLogService;
            this.MedicalDataItemService = medicalDataItemService;
            this.MedicalDataService = medicalDataService;
            this.ActionLogService = actionLogService;
            CacheManager = cacheManager;
        }

        #region 根据数据获取抗生素对应的检测结果，敏感、中介、耐药的值。服务自动处理预计是5分钟运行一次

        /// <summary>
        /// 根据数据获取抗生素对应的检测结果，敏感、中介、耐药的值。服务自动处理预计是5分钟运行一次
        /// </summary>
        /// <param name="member">当前登录用户</param>
        /// <param name="medicalId">医学数据的id，如果传递了则获取指定的数据，没有传递则系统查询所有数据</param>
        public string AutoDispose(Member member, long medicalId = 0)
        {
            bool isDeleteLock = true; //清除分布式锁
            string cacheKey = "managesystem.services.medicine.medicalantibioticresultservice.autodispose." + medicalId;

            try
            {
                // 验证分布式锁 ，系统60分钟内还未处理完成则自动释放
                if (this.CacheManager.ExistLock(cacheKey))
                {
                    isDeleteLock = false;
                    throw new Exception("数据耐药性已在计算中，请等待处理完成,数据ID=" + medicalId);
                }

                if (!this.CacheManager.AddLock(cacheKey, 60))
                {
                    isDeleteLock = false;
                    throw new Exception("接口请求过于频繁，请等待处理完成");
                }

                IMedicalDataService medicalDataService = EngineContext.Current.Resolve<IMedicalDataService>();
                IMedicalDataItemService medicalDataItemService = EngineContext.Current.Resolve<IMedicalDataItemService>();

                if (member == null || member.Id <= 0)
                    member = new Member() { Id = 0, Name = "自动作业" };

                //1、读取需要处理的数据
                List<MedicalData> medicalDataList = null;
                if (medicalId > 0)
                    medicalDataList = medicalDataService.Query(m => m.Id == medicalId && m.AntibioticResultStatue == (int)MedicalDataAntibioticResultStatueEnum.Wait && m.Status == (int)MedicalDataStatusEnum.Effective);
                else
                    medicalDataList = medicalDataService.Query(m => m.AntibioticResultStatue == (int)MedicalDataAntibioticResultStatueEnum.Wait && m.Status == (int)MedicalDataStatusEnum.Effective);

                if (medicalDataList == null || !medicalDataList.Any())
                {
                    //没有需要处理的数据
                    //this.ActionLogService.Insert(ActionType.Create, ActionSource.Web, 0, "系统", "自动处理医学信息的耐药性规则，未查询到数据", "");
                    return "自动处理医学信息的耐药性规则，未查询到数据";
                }

                //2、产生SQL和保存到数据库
                List<MedicalAntibioticRule> antibioticRuleList = this.MedicalAntibioticRuleService.Query().ToList();       //医学数据 抗生素规则（基础数据）
                string logMedicalIds = "";
                int logCount = 0;
                int successCount = 0;
                string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff");

                foreach (var medical in medicalDataList)
                {
                    logMedicalIds += medical.Id.ToString() + ",";
                    logCount++;

                    //获取指定上传数据的明细数据
                    List<MedicalDataItem> data = medicalDataItemService.Query(m => m.MedicalDataId == medical.Id).ToList();
                    if (data == null || !data.Any()) continue;

                    List<MedicalAntibioticResult> resultList = new List<MedicalAntibioticResult>();
                    foreach (var item in data)
                    {
                        //没有细菌，所以无法验证抗生素，这里直接返回
                        if (string.IsNullOrWhiteSpace(item.ORGANISM)) continue;

                        resultList.Add(new MedicalAntibioticResult()
                        {
                            MedicalDataId = medical.Id,
                            MedicalDataItemId = item.Id,
                            IsValid = item.IsValid,
                            Sort = item.Sort,
                            MemberId = member.Id,
                            Id = CommonHelper.GuidToLongID,
                            UploadRowIndex = item.UploadRowIndex,
                            ORG_TYPE = item.ORG_TYPE,
                            OrganismId = item.OrganismId,
                            OrganismCode = item.ORGANISM,
                            OrganismName = item.OrganismName,
                            INDUC_CLI = item.INDUC_CLI,//新增字段
                            CARBAPENEM = item.CARBAPENEM,//新增字段
                            COMMENT = this.GetResultValue(antibioticRuleList, item.ORGANISM, "COMMENT", item.COMMENT),
                            QDA_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "QDA_NM", item.QDA_NM),
                            VAN_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "VAN_ND30", item.VAN_ND30),
                            AMK_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMK_ND30", item.AMK_ND30),
                            AMC_ND20 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMC_ND20", item.AMC_ND20),
                            AZM_ND15 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AZM_ND15", item.AZM_ND15),
                            AMP_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMP_ND10", item.AMP_ND10),
                            SAM_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "SAM_ND10", item.SAM_ND10),
                            ATM_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ATM_ND30", item.ATM_ND30),
                            OXA_ND1 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "OXA_ND1", item.OXA_ND1),
                            POL_ND300 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "POL_ND300", item.POL_ND300),
                            NIT_ND300 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "NIT_ND300", item.NIT_ND300),
                            SXT_ND1_2 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "SXT_ND1_2", item.SXT_ND1_2),
                            STH_ND300 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "STH_ND300", item.STH_ND300),
                            GEH_ND120 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "GEH_ND120", item.GEH_ND120),
                            ERY_ND15 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ERY_ND15", item.ERY_ND15),
                            CIP_ND5 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CIP_ND5", item.CIP_ND5),
                            CLI_ND2 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CLI_ND2", item.CLI_ND2),
                            RIF_ND5 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "RIF_ND5", item.RIF_ND5),
                            LNZ_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "LNZ_ND30", item.LNZ_ND30),
                            STR_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "STR_ND10", item.STR_ND10),
                            FOS_ND200 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FOS_ND200", item.FOS_ND200),
                            CHL_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CHL_ND30", item.CHL_ND30),
                            MEM_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MEM_ND10", item.MEM_ND10),
                            MNO_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MNO_ND30", item.MNO_ND30),
                            MFX_ND5 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MFX_ND5", item.MFX_ND5),
                            PIP_ND100 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "PIP_ND100", item.PIP_ND100),
                            TZP_ND100 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TZP_ND100", item.TZP_ND100),
                            PEN_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "PEN_ND10", item.PEN_ND10),
                            GEN_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "GEN_ND10", item.GEN_ND10),
                            TCY_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TCY_ND30", item.TCY_ND30),
                            TCC_ND75 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TCC_ND75", item.TCC_ND75),
                            TIC_ND75 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TIC_ND75", item.TIC_ND75),
                            TEC_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TEC_ND30", item.TEC_ND30),
                            TGC_ND15 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TGC_ND15", item.TGC_ND15),
                            FEP_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FEP_ND30", item.FEP_ND30),
                            CXM_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CXM_ND30", item.CXM_ND30),
                            CEC_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CEC_ND30", item.CEC_ND30),
                            CFP_ND75 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CFP_ND75", item.CFP_ND75),
                            CSL_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CSL_ND30", item.CSL_ND30),
                            CRO_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CRO_ND30", item.CRO_ND30),
                            CTX_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CTX_ND30", item.CTX_ND30),
                            CAZ_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CAZ_ND30", item.CAZ_ND30),
                            FOX_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FOX_ND30", item.FOX_ND30),
                            CZO_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZO_ND30", item.CZO_ND30),
                            TOB_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TOB_ND10", item.TOB_ND10),
                            IPM_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "IPM_ND10", item.IPM_ND10),
                            OFX_ND5 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "OFX_ND5", item.OFX_ND5),
                            DOX_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "DOX_ND30", item.DOX_ND30),
                            AMK_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMK_NM", item.AMK_NM),
                            AMC_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMC_NM", item.AMC_NM),
                            AZM_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AZM_NM", item.AZM_NM),
                            AMP_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMP_NM", item.AMP_NM),
                            SAM_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "SAM_NM", item.SAM_NM),
                            ATM_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ATM_NM", item.ATM_NM),
                            OXA_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "OXA_NM", item.OXA_NM),
                            POL_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "POL_NM", item.POL_NM),
                            NIT_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "NIT_NM", item.NIT_NM),
                            SXT_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "SXT_NM", item.SXT_NM),
                            STH_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "STH_NM", item.STH_NM),
                            GEH_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "GEH_NM", item.GEH_NM),
                            ERY_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ERY_NM", item.ERY_NM),
                            CIP_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CIP_NM", item.CIP_NM),
                            CLI_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CLI_NM", item.CLI_NM),
                            RIF_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "RIF_NM", item.RIF_NM),
                            LNZ_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "LNZ_NM", item.LNZ_NM),
                            STR_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "STR_NM", item.STR_NM),
                            FOS_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FOS_NM", item.FOS_NM),
                            CHL_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CHL_NM", item.CHL_NM),
                            MEM_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MEM_NM", item.MEM_NM),
                            MNO_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MNO_NM", item.MNO_NM),
                            MFX_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MFX_NM", item.MFX_NM),
                            PIP_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "PIP_NM", item.PIP_NM),
                            TZP_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TZP_NM", item.TZP_NM),
                            PEN_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "PEN_NM", item.PEN_NM),
                            GEN_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "GEN_NM", item.GEN_NM),
                            PEN_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "PEN_NE", item.PEN_NE),
                            TCY_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TCY_NM", item.TCY_NM),
                            TCC_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TCC_NM", item.TCC_NM),
                            TIC_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TIC_NM", item.TIC_NM),
                            TEC_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TEC_NM", item.TEC_NM),
                            TGC_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TGC_NM", item.TGC_NM),
                            FEP_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FEP_NM", item.FEP_NM),
                            CXM_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CXM_NM", item.CXM_NM),
                            CFP_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CFP_NM", item.CFP_NM),
                            CSL_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CSL_NM", item.CSL_NM),
                            CRO_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CRO_NM", item.CRO_NM),
                            CTX_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CTX_NM", item.CTX_NM),
                            CAZ_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CAZ_NM", item.CAZ_NM),
                            FOX_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FOX_NM", item.FOX_NM),
                            CZO_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZO_NM", item.CZO_NM),
                            TOB_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TOB_NM", item.TOB_NM),
                            VAN_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "VAN_NM", item.VAN_NM),
                            VAN_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "VAN_NE", item.VAN_NE),
                            IPM_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "IPM_NM", item.IPM_NM),
                            LVX_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "LVX_NM", item.LVX_NM),
                            LVX_ND5 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "LVX_ND5", item.LVX_ND5),
                            CTX_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CTX_NE", item.CTX_NE),
                            CSL_ND75 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CSL_ND75", item.CSL_ND75),
                            ETP_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ETP_ND10", item.ETP_ND10),
                            ETP_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ETP_NM", item.ETP_NM),
                            CTT_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CTT_ND30", item.CTT_ND30),
                            CTT_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CTT_NM", item.CTT_NM),
                            DOR_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "DOR_ND10", item.DOR_ND10),
                            DOR_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "DOR_NM", item.DOR_NM),
                            NET_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "NET_ND30", item.NET_ND30),
                            NET_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "NET_NM", item.NET_NM),
                            QDA_ND15 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "QDA_ND15", item.QDA_ND15),
                            CPT_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CPT_ND30", item.CPT_ND30),
                            CPT_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CPT_NM", item.CPT_NM),
                            CPT_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CPT_NE", item.CPT_NE),
                            CZA_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZA_ND30", item.CZA_ND30),
                            CZA_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZA_NM", item.CZA_NM),
                            CZA_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZA_NE", item.CZA_NE),
                            AZA_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AZA_ND30", item.AZA_ND30),
                            AZA_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AZA_NM", item.AZA_NM),
                            AZA_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AZA_NE", item.AZA_NE),
                            CZT_ND30 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZT_ND30", item.CZT_ND30),
                            CZT_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZT_NM", item.CZT_NM),
                            CZT_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZT_NE", item.CZT_NE),
                            DOX_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "DOX_NM", item.DOX_NM),//新增字段  
                            COL_ND10 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "COL_ND10", item.COL_ND10),
                            COL_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "COL_NM", item.COL_NM),
                            COL_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "COL_NE", item.COL_NE),
                            AMC_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMC_NE", item.AMC_NE),
                            AMK_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMK_NE", item.AMK_NE),
                            AMP_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AMP_NE", item.AMP_NE),
                            ATM_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ATM_NE", item.ATM_NE),
                            AZM_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "AZM_NE", item.AZM_NE),
                            CAZ_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CAZ_NE", item.CAZ_NE),
                            CEC_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CEC_NE", item.CEC_NE),
                            CFP_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CFP_NE", item.CFP_NE),
                            CHL_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CHL_NE", item.CHL_NE),
                            CIP_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CIP_NE", item.CIP_NE),
                            CLI_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CLI_NE", item.CLI_NE),
                            CRO_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CRO_NE", item.CRO_NE),
                            CTT_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CTT_NE", item.CTT_NE),
                            CXM_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CXM_NE", item.CXM_NE),
                            CZO_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "CZO_NE", item.CZO_NE),
                            DOR_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "DOR_NE", item.DOR_NE),
                            DOX_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "DOX_NE", item.DOX_NE),
                            ERY_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ERY_NE", item.ERY_NE),
                            ETP_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ETP_NE", item.ETP_NE),
                            FEP_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FEP_NE", item.FEP_NE),
                            FOS_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FOS_NE", item.FOS_NE),
                            FOX_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "FOX_NE", item.FOX_NE),
                            GEN_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "GEN_NE", item.GEN_NE),
                            IPM_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "IPM_NE", item.IPM_NE),
                            LNZ_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "LNZ_NE", item.LNZ_NE),
                            LVX_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "LVX_NE", item.LVX_NE),
                            MEM_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MEM_NE", item.MEM_NE),
                            MFX_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MFX_NE", item.MFX_NE),
                            MNO_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "MNO_NE", item.MNO_NE),
                            NET_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "NET_NE", item.NET_NE),
                            NIT_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "NIT_NE", item.NIT_NE),
                            OXA_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "OXA_NE", item.OXA_NE),
                            PIP_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "PIP_NE", item.PIP_NE),
                            POL_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "POL_NE", item.POL_NE),
                            QDA_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "QDA_NE", item.QDA_NE),
                            RIF_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "RIF_NE", item.RIF_NE),
                            SAM_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "SAM_NE", item.SAM_NE),
                            STH_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "STH_NE", item.STH_NE),
                            STR_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "STR_NE", item.STR_NE),
                            SXT_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "SXT_NE", item.SXT_NE),
                            TCC_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TCC_NE", item.TCC_NE),
                            TCY_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TCY_NE", item.TCY_NE),
                            TEC_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TEC_NE", item.TEC_NE),
                            TGC_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TGC_NE", item.TGC_NE),
                            TIC_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TIC_NE", item.TIC_NE),
                            TOB_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TOB_NE", item.TOB_NE),
                            TZP_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "TZP_NE", item.TZP_NE),
                            ERV_NM = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ERV_NM", item.ERV_NM),
                            ERV_ND20 = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ERV_ND20", item.ERV_ND20),
                            ERV_NE = this.GetResultValue(antibioticRuleList, item.ORGANISM, "ERV_NE", item.ERV_NE),
                        });
                    }

                    //将耐药性计算的结果保存到数据库
                    this.BatchInsertResultData(resultList);

                    //更新主表上的状态
                    medical.AntibioticResultStatue = (int)MedicalDataAntibioticResultStatueEnum.Success;
                    medical.AntibioticResultTime = DateTime.Now;
                    this.MedicalDataService.Update(medical);
                    successCount++;
                }

                //3、添加操作记录
                string returnLog = "自动处理医学信息的耐药性规则，处理完成，结果：" + string.Format("共处理：{0}条数据，共成功：{1}条数据，处理的医学数据Id集合：{2}，开始时间：{3}，结束时间：{4}", logCount, successCount, logMedicalIds, startTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff"));
                this.ActionLogService.Insert(ActionType.Create, ActionSource.InnerApi, 0, "系统", returnLog, "");
                return returnLog;

            }
            catch (Exception ex)
            {
                string errorMessage = "自动处理医学信息的耐药性规则，发生异常，错误信息：" + ex.Message;
                this.ActionLogService.Insert(ActionType.Create, ActionSource.InnerApi, 0, "系统", errorMessage, ex.ToString());
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                return errorMessage;
            }
            finally
            {
                if (isDeleteLock)
                {
                    this.CacheManager.DeleteLock(cacheKey);
                }
            }
        }

        /// <summary>
        /// 批量把医学明细数据插入到数据库中
        /// </summary>
        /// <param name="model">上传的数据</param>
        public void BatchInsertResultData(List<MedicalAntibioticResult> resultList)
        {
            var connection = DapperHelper.GetConnection();
            connection.Open();

            //先将list的数据转换成datatable
            var table = DataTableHelper.ToDataTable<MedicalAntibioticResult>(resultList);
            using (SqlBulkCopy sbc = new SqlBulkCopy(connection))
            {
                try
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                        sbc.ColumnMappings.Add(table.Columns[i].ColumnName, table.Columns[i].ColumnName); //设置目标表和源数据的列映射

                    sbc.DestinationTableName = "MedicalAntibioticResult";  //取得目标表名
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
        /// 根据抗生素的规则获取敏感、中介、耐药、无效的
        /// </summary>
        /// <param name="antibioticRuleList">抗生素规则数据（基础数据）</param>
        /// <param name="organismCode">本行数据对应的细菌编码</param>
        /// <param name="keyName">字段的名称，是完整字段也就是导入Excel中的列头</param>
        /// <param name="value">对应的值</param>
        /// <returns></returns>
        private int GetResultValue(List<MedicalAntibioticRule> antibioticRuleList, string organismCode, string keyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return (int)MedicalAntibioticResultValueEnum.Invalid; // 如果没有填写值，或者值不是数字，则返回0，表示无效

            string oldValue = value;  // 原始的值
            float valueTemp = this.GetResultItemValue(value, keyName); // 根据字段的值获取里面实际的值，同时处理符号的问题

            if (valueTemp <= 0) return (int)MedicalAntibioticResultValueEnum.Invalid;

            //获取对应的规则
            MedicalAntibioticRule rule = antibioticRuleList.Where(m => m.OrganismCode.ToLower().Equals(organismCode.ToLower()) && m.Code.ToLower().Equals(keyName.ToLower())).FirstOrDefault();
            if (organismCode.ToUpperInvariant().Trim().Equals("SLU"))
            {
                rule = antibioticRuleList.Where(m => m.OrganismCode.ToLowerInvariant().Equals("sau") && m.Code.ToLower().Equals(keyName.ToLower())).FirstOrDefault();
            }

            if (rule == null || rule.Id <= 0)
            {
                return (int)MedicalAntibioticResultValueEnum.Invalid; // 没有保存相关的规则，返回表示无效
            }

            //验证是否敏感
            if (rule.SensitivityMin != -1 && rule.SensitivityMax != -1 && valueTemp >= rule.SensitivityMin && valueTemp <= rule.SensitivityMax)
            {
                return (int)MedicalAntibioticResultValueEnum.Sensitive;
            }

            //验证是否中介
            if (rule.IntermediaryMin != -1 && rule.IntermediaryMax != -1 && valueTemp >= rule.IntermediaryMin && valueTemp <= rule.IntermediaryMax)
                return (int)MedicalAntibioticResultValueEnum.Intermediary;

            //验证是否耐药
            if (rule.ResistanceMin != -1 && rule.ResistanceMax != -1 && valueTemp >= rule.ResistanceMin && valueTemp <= rule.ResistanceMax)
                return (int)MedicalAntibioticResultValueEnum.Resistance;

            return (int)MedicalAntibioticResultValueEnum.Invalid;
        }

        /// <summary>
        /// 根据抗生素的规则获取敏感，根据上传的数据值获取实际计算所需的值，主要是处理符号的问题（>、<）
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="keyName">字段的名称</param>
        /// <returns></returns>
        private float GetResultItemValue(String value, string keyName)
        {
            if (String.IsNullOrWhiteSpace(value))
                return 0;

            string oldValue = value;

            try
            {
                //先将值中的中文符号替换成英文的符号。
                value = value.Trim().ReplaceSymbol();
                value = value.Replace("mm", "").Replace("MM", ""); // 删除mm单位
                value = value.TrimEnd('.'); //删除值中最后的一个点，例如：128.  则替换成128
                if (value.Contains("."))
                {
                    value = Regex.Replace(value, "0+?$", "");
                    value = Regex.Replace(value, "[.]$", "");
                }

                // 值中带有 >、< 符号，例如：>12
                string sym = "";
                if (Regex.IsMatch(value, @"^[><]\d*[.]?\d*$")) // 正则匹配是 ">" 或 "<" 开头且不是 ">=" 或者 "<="
                {
                    sym = value.Substring(0, 1);//获取值中的符号，第一位   >
                }

                #region 部分特殊字段，先处理

                if (keyName.ToUpperInvariant().Equals("GEH_NM"))
                {
                    /**
                    * MIC法特殊字段-【GEH_NM】高浓度庆大霉素的填写规则：
                    * 以下特殊规则将直接替代普通MIC法的校验规则，具体如下：
                    * 1、仅允许出现如下值：250，256，500，512，513，1000，1024，1025，不在这些数字范围内的，均为非法值;
                    * 2、可以有前导【>、<、=】符号;
                    * 3、如果填写了【SYN-S】，系统将自动替换为【<=500】;
                    * 4、如果填写了【SYN-R】，则将自动替换为【>=1000】。
                    */
                    string _value = value.DeleteSymbol();
                    if (!_value.IsNumeric()) return 0; // 无法识别

                    float tempValue = _value.GetFloat();
                    if (tempValue <= 0) return 0;   // 无法识别

                    List<float> gen_value_support = new List<float> { 250, 256, 500, 512, 513, 1000, 1024, 1025 };
                    if (!gen_value_support.Contains(tempValue)) return 0;   //无法识别

                    #region 符号处理
                    switch (tempValue)
                    {
                        case 250:
                            tempValue = sym.Equals(">") ? 256 : sym.Equals("<") ? 240 : 250; //240做特殊标记
                            break;
                        case 256:
                            tempValue = sym.Equals(">") ? 500 : sym.Equals("<") ? 250 : 256;
                            break;
                        case 500:
                            tempValue = sym.Equals(">") ? 512 : sym.Equals("<") ? 256 : 500;
                            break;
                        case 512:
                            tempValue = sym.Equals(">") ? 513 : sym.Equals("<") ? 500 : 512;
                            break;
                        case 513:
                            tempValue = sym.Equals(">") ? 1000 : sym.Equals("<") ? 512 : 513;
                            break;
                        case 1000:
                            tempValue = sym.Equals(">") ? 1024 : sym.Equals("<") ? 513 : 1000;
                            break;
                        case 1024:
                            tempValue = sym.Equals(">") ? 1025 : sym.Equals("<") ? 1000 : 1024;
                            break;
                        case 1025:
                            tempValue = sym.Equals(">") ? 2500 : sym.Equals("<") ? 1024 : 1025;
                            break;
                    }
                    #endregion

                    return (tempValue >= 240 && tempValue <= 2500) ? tempValue : 0;
                }

                if (keyName.ToUpperInvariant().Equals("STH_NM"))
                {
                    /**
                      *  MIC法特殊字段-【STH_NM】高浓度链霉素的填写规则：
                      * 以下特殊规则直接替代普通MIC法的校验规则，具体如下：
                      * 1、仅允许出现如下值：500，512，1000，1024，2000，2048，2049，不在这些数字范围内的，均视为非法值;
                      * 2、可以有前导【>、<、=】符号;
                      * 3、如果填写了【SYN-S】，系统将自动替换为【<=1000】;
                      * 4、如果填写了【SYN-R】则将自动替换为【>=2000】。
                      */
                    string _value = value.DeleteSymbol();
                    if (!_value.IsInt()) return 0; // 无法识别

                    int tempValue = _value.GetInt();
                    if (tempValue <= 0) return 0;   //无法识别

                    List<int> sth_value_support = new List<int> { 500, 512, 1000, 1024, 2000, 2048, 2049 };
                    if (!sth_value_support.Contains(tempValue)) return 0;   //无法识别

                    #region 符号处理
                    switch (tempValue)
                    {
                        case 500:
                            tempValue = sym.Equals(">") ? 512 : sym.Equals("<") ? 490 : 500; // 490做特殊标记
                            break;
                        case 512:
                            tempValue = sym.Equals(">") ? 1000 : sym.Equals("<") ? 500 : 512;
                            break;
                        case 1000:
                            tempValue = sym.Equals(">") ? 1024 : sym.Equals("<") ? 512 : 1000;
                            break;
                        case 1024:
                            tempValue = sym.Equals(">") ? 2000 : sym.Equals("<") ? 1000 : 1024;
                            break;
                        case 2000:
                            tempValue = sym.Equals(">") ? 2048 : sym.Equals("<") ? 1024 : 2000;
                            break;
                        case 2048:
                            tempValue = sym.Equals(">") ? 2049 : sym.Equals("<") ? 2000 : 2048;
                            break;
                        case 2049:
                            tempValue = sym.Equals(">") ? 2500 : sym.Equals("<") ? 2048 : 2049;
                            break;
                    }
                    #endregion

                    return (tempValue >= 490 && tempValue <= 2500) ? tempValue : 0;

                }

                if (keyName.ToUpperInvariant().Equals("SXT_NM"))
                {
                    string _value = value.DeleteSymbol();
                    if (!_value.IsNumeric()) return 0; // 无法识别

                    float tempValue = _value.GetFloat();
                    if (tempValue <= 0) return 0;   // 无法识别

                    //List<float> sxt_value_support = new List<float> { 10f / 20f, 20f / 20f, 40f / 20f, 80f / 20f, 160f / 20f, 320f / 20f, 640f / 20f, 1280f / 20f, 2560f / 20f, 5120f / 20f };
                    //if (!sxt_value_support.Contains(tempValue)) return 0;   // 无法识别

                    // 符号处理
                    tempValue = sym.Equals(">") ? (tempValue * 2.0f) : sym.Equals("<") ? (tempValue / 2.0f) : tempValue;

                    return tempValue;
                }

                // 头孢唑林小于等于4暂归入敏感吧全国的数据统计也是这个规则
                if (keyName.Trim().StartsWith("CZO"))
                {
                    string _value = value.DeleteSymbol();
                    if (!_value.IsNumeric()) return 0; // 无法识别

                    float tempValue = _value.GetFloat();
                    if (tempValue <= 0) return 0;   //无法识别

                    tempValue = sym.StartsWith(">") ? tempValue * 2.0f : sym.StartsWith("<") ? tempValue / 2.0f : tempValue;
                    return tempValue;
                }
                #endregion

                //如果直接是数字，不带符号的就直接返回处理，包括：1、1.0 、0.1、.1
                if (value.IsNumeric())
                {
                    return value.GetFloat(); //纯数字不需要处理符号的问题
                }

                // 值中带有 >=、<=、=，直接删除符号即可 ，例如：<=12、=12、>=12
                if (value.LastIndexOf("=") > 0)
                {
                    return value.DeleteSymbol().GetFloat(); //删除符号得到数字
                }

                value = value.Substring(1); // 获取实际的值， 12；有一种特殊情况  <.12，这里取到的值是.12，在转成小数的时候会自动加0，所以不用特殊处理

                if (string.IsNullOrWhiteSpace(value))
                {
                    return 0;
                }

                #region KB法的处理 ND

                //纸片法代码验证，值必须是[6,80]范围的整数
                if (keyName.ToUpperInvariant().Contains("_ND"))
                {
                    if (!value.IsInt()) { return 0; }

                    int tempValue = value.Replace("mm", "").Replace("MM", "").GetInt();

                    if (tempValue <= 0) return 0;   // 无法识别

                    //值只能在6(包含)-80(包含)之间的整数
                    tempValue = (tempValue >= 6 && tempValue <= 80) ? tempValue : 0;

                    // 符号处理
                    tempValue = sym.Equals(">") ? tempValue + 1 : sym.Equals("<") ? tempValue - 1 : tempValue;

                    return tempValue;
                }

                #endregion

                #region MIC法的处理 NM
                if (keyName.ToUpper().Contains("_NM"))
                {
                    //  MIC法代码验证，以及数据合格性验证。
                    //数据为倍比，0.03、0.06、0.125、0.25、0.5、1、2、4、8、16、32、64、128、256、512、1024、2048
                    float tempValue = value.GetFloat();
                    if (tempValue <= 0) return 0;   //无法识别

                    /**
                     * 符号处理
                     * 大于当前值，所以增加一倍，例如：>4，则返回的是 8
                     * 小于当前值，所以减少一倍，例如：<4，则返回的是 2
                     */
                    tempValue = sym.Equals(">") ? tempValue * 2f : sym.Equals("<") ? tempValue / 2f : tempValue;

                    //  0 < 值 <= 6000
                    return (tempValue > 0.0f && tempValue <= 6000.0f) ? tempValue : 0;
                }
                #endregion

                #region E-Test 法
                if (keyName.ToUpper().Contains("_NE"))
                {
                    //  MIC法代码验证，以及数据合格性验证。数据为倍比，0.03、0.06、0.125、0.25、0.5、1、2、4、8、16、32、64、128、256、512、1024、2048
                    float tempValue = value.GetFloat();
                    if (tempValue <= 0) return 0;   //无法识别

                    /**
                     * 符号处理
                     * 大于当前值，所以增加一倍，例如：>4，则返回的是 8
                     * 小于当前值，所以减少一倍，例如：<4，则返回的是 2
                     */
                    tempValue = sym.Equals(">") ? tempValue * 2f : sym.Equals("<") ? tempValue / 2f : tempValue;

                    //  0 < 值 <= 6000
                    return (tempValue > 0.0f && tempValue <= 6000.0f) ? tempValue : 0;
                }
                #endregion

                return 0;
            }
            catch (Exception ex)
            {
                string message = "oldValue =" + oldValue + "， value= " + value + "，keyName=" + keyName;
                this.SystemLogService.Insert("根据抗生素的规则获取敏感，获取值发生异常：", message, SystemLogLevel.Error, "", "");
                throw;
            }
        }

        #endregion

        #region  获取耐药性结果数据

        /// <summary>
        /// 获取耐药性的结果
        /// </summary>
        /// <param name="medicalId">上传的id</param>
        /// <param name="organismId">指定的细菌id，如果不指定则查询本次上传所有的细菌</param>
        /// <param name="excludeSpecType">需要排除的标本类型，多个采用英文逗号分隔</param>
        /// <param name="otherSqlWhere">其他的sql查询条件，查询的表示：MedicalAntibioticResult，格式：AND ID > 0 </param>
        /// <returns></returns>
        public List<GetAntibioticResultModel> GetResult(long medicalId, long organismId = 0, string excludeSpecType = "", string otherSqlWhere = "")
        {
            List<MedicalAntibioticResult> list = this.GetResultList(medicalId, organismId, excludeSpecType, otherSqlWhere);

            return this.GetResult(list);
        }

        /// <summary>
        /// 获取耐药性的结果
        /// </summary>
        /// <param name="medicalId">上传的id</param>
        /// <param name="organismId">指定的细菌id，如果不指定则查询本次上传所有的细菌</param>
        /// <param name="excludeSpecType">需要排除的标本类型，多个采用英文逗号分隔</param>
        /// <param name="otherSqlWhere">其他的sql查询条件，查询的表示：MedicalAntibioticResult，格式：AND ID > 0 </param>
        /// <returns></returns>
        public List<GetAntibioticResultModel> GetResult(long medicalId, List<long> organismIds, string excludeSpecType = "", string otherSqlWhere = "")
        {
            List<MedicalAntibioticResult> list = this.GetResultList(medicalId, organismIds, excludeSpecType, otherSqlWhere);

            return this.GetResult(list);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        private void GetResultNodeCount(List<GetAntibioticResultModel> resultList, long organismId, string code, int value1, int value2 = 0, int value3 = 0, int value4 = 0)
        {
            var model = resultList.Where(m => m.OrganismId == organismId && m.AntibioticCode.ToUpper().Equals(code.ToUpper())).FirstOrDefault();
            if (model == null) return;

            //0：无效值  1：敏感   2：中介   3：耐药
            if (value1 > 0)
            {
                switch (value1)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value2 > 0)
            {
                switch (value2)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value3 > 0)
            {
                switch (value3)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value4 > 0)
            {
                switch (value4)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <param name="value4"></param>
        private void GetResultNodeCount(List<GetAntibioticResultModel> resultList, List<long> organismIds, string code, int value1, int value2 = 0, int value3 = 0, int value4 = 0)
        {
            var model = resultList.Where(m => organismIds.Contains(m.OrganismId) && m.AntibioticCode.ToUpper().Equals(code.ToUpper())).FirstOrDefault();
            if (model == null) return;

            //0：无效值  1：敏感   2：中介   3：耐药
            if (value1 > 0)
            {
                switch (value1)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value2 > 0)
            {
                switch (value2)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value3 > 0)
            {
                switch (value3)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
            else if (value4 > 0)
            {
                switch (value4)
                {
                    case 1:
                        model.SensitiveCount += 1;
                        break;
                    case 2:
                        model.IntermediaryCount += 1;
                        break;
                    case 3:
                        model.ResistanceCount += 1;
                        break;
                }
            }
        }

        /// <summary>
        /// 获取耐药性的耐药性数据
        /// </summary>
        /// <param name="medicalId">上传的id</param>
        /// <param name="organismId">指定的细菌id，如果不指定则查询本次上传所有的细菌</param>
        /// <param name="excludeSpecType">需要排除的标本类型，多个采用英文逗号分隔</param>
        /// <param name="otherSqlWhere">其他的sql查询条件，查询的表示：MedicalAntibioticResult，格式：AND ID > 0 </param>
        /// <returns></returns>
        public List<MedicalAntibioticResult> GetResultList(long medicalId, long organismId = 0, string excludeSpecType = "", string otherSqlWhere = "")
        {
            DynamicParameters parameters = new DynamicParameters();

            //string sql = $"SELECT * FROM [dbo].[MedicalAntibioticResult](NOLOCK) WHERE [Mark] > 0 AND [MedicalDataId] = {medicalId} ";

            string partialSQL = $" FROM [dbo].[MedicalAntibioticResult](NOLOCK) ";

            StringBuilder whereSQL = new StringBuilder(" AND [Mark] > 0 AND [MedicalDataId] = @MedicalDataId ");
            parameters.Add("@MedicalDataId", medicalId);


            if (organismId > 0)
            {
                whereSQL.Append($" AND [OrganismId] = @OrganismId ");
                parameters.Add("@OrganismId", organismId);
            }

            if (!string.IsNullOrWhiteSpace(excludeSpecType))
            {
                whereSQL.Append($" AND [MedicalDataItemId] IN (SELECT [Id] FROM [dbo].[MedicalDataItem] WHERE [MedicalDataId] = @MedicalDataId AND [SPEC_TYPE] NOT IN (@ExcludeSpecType)) ");
                parameters.Add("@ExcludeSpecType", excludeSpecType);
            }

            if (!string.IsNullOrWhiteSpace(otherSqlWhere))
            {
                whereSQL.Append(otherSqlWhere);
            }

            //if (organismId > 0)
            //    sql += "  AND OrganismId='" + organismId + "' ";
            //if (!string.IsNullOrWhiteSpace(excludeSpecType))
            //    sql += " AND MedicalDataItemId  IN (SELECT Id FROM dbo.MedicalDataItem WHERE MedicalDataId ='" + medicalId + "'  AND SPEC_TYPE NOT IN(" + excludeSpecType + ")) ";

            //if (!string.IsNullOrWhiteSpace(otherSqlWhere))
            //    sql += otherSqlWhere;

            List<MedicalAntibioticResult> list = new List<MedicalAntibioticResult>();
            using (var connection = DapperHelper.GetConnection())
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                int total = connection.ExecuteScalar<int>($"SELECT COUNT(1) AS [Count] {partialSQL} WHERE 1 = 1 {whereSQL.ToString()} ", parameters);
                int pageSize = 5000;
                int pageCount = (total + pageSize - 1) / pageSize;
                DapperPageHelper pageHelper = new DapperPageHelper();

                // 一次读取可能出现超时，这里改为分页读取数据
                for (int page = 0; page < pageCount; page++)
                {
                    IEnumerable<MedicalAntibioticResult> partialResult = pageHelper.QueryPageToEnumerable<MedicalAntibioticResult>(sql: $"SELECT * {partialSQL} ",
                          where: whereSQL.ToString(),
                          orderby: " ORDER BY [InsertTime], [Id] ",
                          param: parameters,
                          pageIndex: page,
                          pageSize: pageSize);

                    list.AddRange(partialResult);
                }
                //list = connection.Query<MedicalAntibioticResult>(sql).ToList();
                Log4Helper.Info($"GetResultList().Count = {list.Count()}");
            }

            return list ?? new List<MedicalAntibioticResult>();
        }

        /// <summary>
        /// 获取耐药性的耐药性数据
        /// </summary>
        /// <param name="medicalId">上传的id</param>
        /// <param name="organismId">指定的细菌id，如果不指定则查询本次上传所有的细菌</param>
        /// <param name="excludeSpecType">需要排除的标本类型，多个采用英文逗号分隔</param>
        /// <param name="otherSqlWhere">其他的sql查询条件，查询的表示：MedicalAntibioticResult，格式：AND ID > 0 </param>
        /// <returns></returns>
        private List<MedicalAntibioticResult> GetResultList(long medicalId, List<long> organismIds, string excludeSpecType = "", string otherSqlWhere = "")
        {
            DynamicParameters parameters = new DynamicParameters();

            //string sql = $"SELECT * FROM [dbo].[MedicalAntibioticResult](NOLOCK) WHERE [Mark] > 0 AND [MedicalDataId] = {medicalId} ";

            string partialSQL = $" FROM [dbo].[MedicalAntibioticResult](NOLOCK) ";

            StringBuilder whereSQL = new StringBuilder(" AND [Mark] > 0 AND [MedicalDataId] = @MedicalDataId ");
            parameters.Add("@MedicalDataId", medicalId);

            if (organismIds != null && organismIds.Count > 0)
            {
                whereSQL.Append($" AND [OrganismId] IN ({string.Join(",", organismIds)}) ");
            }

            if (!string.IsNullOrWhiteSpace(excludeSpecType))
            {
                whereSQL.Append($" AND [MedicalDataItemId] IN (SELECT [Id] FROM [dbo].[MedicalDataItem] WHERE [MedicalDataId] = @MedicalDataId AND [SPEC_TYPE] NOT IN (@ExcludeSpecType)) ");
                parameters.Add("@ExcludeSpecType", excludeSpecType);
            }

            if (!string.IsNullOrWhiteSpace(otherSqlWhere))
            {
                whereSQL.Append(otherSqlWhere);
            }

            //if (organismId > 0)
            //    sql += "  AND OrganismId='" + organismId + "' ";
            //if (!string.IsNullOrWhiteSpace(excludeSpecType))
            //    sql += " AND MedicalDataItemId  IN (SELECT Id FROM dbo.MedicalDataItem WHERE MedicalDataId ='" + medicalId + "'  AND SPEC_TYPE NOT IN(" + excludeSpecType + ")) ";

            //if (!string.IsNullOrWhiteSpace(otherSqlWhere))
            //    sql += otherSqlWhere;

            List<MedicalAntibioticResult> list = new List<MedicalAntibioticResult>();
            using (var connection = DapperHelper.GetConnection())
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                int total = connection.ExecuteScalar<int>($"SELECT COUNT(1) AS [Count] {partialSQL} WHERE 1 = 1 {whereSQL.ToString()} ", parameters);
                int pageSize = 5000;
                int pageCount = (total + pageSize - 1) / pageSize;
                DapperPageHelper pageHelper = new DapperPageHelper();

                // 一次读取可能出现超时，这里改为分页读取数据
                for (int page = 0; page < pageCount; page++)
                {
                    IEnumerable<MedicalAntibioticResult> partialResult = pageHelper.QueryPageToEnumerable<MedicalAntibioticResult>(sql: $"SELECT * {partialSQL} ",
                          where: whereSQL.ToString(),
                          orderby: " ORDER BY [InsertTime], [Id] ",
                          param: parameters,
                          pageIndex: page,
                          pageSize: pageSize);

                    list.AddRange(partialResult);
                }
                //list = connection.Query<MedicalAntibioticResult>(sql).ToList();
                Log4Helper.Info($"GetResultList().Count = {list.Count()}");
            }

            return list ?? new List<MedicalAntibioticResult>();
        }

        /// <summary>
        /// 根据已有的耐药性结果数据，进行耐药性分析
        /// </summary>
        /// <param name="list">耐药性结果的集合数据</param>
        /// <returns></returns>
        public List<GetAntibioticResultModel> GetResult(List<MedicalAntibioticResult> list)
        {
            List<GetAntibioticResultModel> resultList = new List<GetAntibioticResultModel>();
            if (list == null || !list.Any()) return resultList;  //未查询到数据，直接返回

            // 细菌id集合
            var organismIdList = list.Where(m => m.OrganismId > 0).Select(m => m.OrganismId).Distinct().ToList();

            // 细菌集合
            var organismList = this.MedicalOrganismService.Query(m => organismIdList.Contains(m.Id)).ToList();

            // 医学数据 抗生素（基础数据）
            var antibioticList = this.MedicalAntibioticService.Query().ToList();

            //2、把细菌和药物的关联关系建立好
            foreach (var item in organismList)
            {
                // 上传的数据项
                var arList = list.Where(m => m.OrganismId == item.Id).ToList();
                if (arList == null || !arList.Any()) continue;//这个细菌没有规则数据，所以不用处理，理论上应该不会出现

                if (item.Id == 908)
                {
                    string temp2 = "";
                }

                //A：添加细菌和药物的关联数据
                foreach (var antItem in antibioticList)
                {
                    GetAntibioticResultModel r = new GetAntibioticResultModel()
                    {
                        OrganismId = item.Id,
                        OrganismCode = item.Code,
                        OrganismName = item.Name,
                        AntibioticCode = antItem.Code,
                        AntibioticId = antItem.Id,
                        AntibioticName = antItem.Name,
                        OrganismCount = arList.Count
                    };

                    resultList.Add(r);
                }

                //B：计算药物和细菌的耐药性数量
                foreach (var node in arList)
                {
                    //设置数据的耐药性数量  优先级：E-TSET > MIC > KB （_NE > __NM > _ND）	 
                    this.GetResultNodeCount(resultList, item.Id, "AMK", node.AMK_NE, node.AMK_NM, node.AMK_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "AMP", node.AMP_NE, node.AMP_NM, node.AMP_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "AMX", node.AMX_NM, node.AMX_ND30, node.AMX_ND25);
                    this.GetResultNodeCount(resultList, item.Id, "ATM", node.ATM_NE, node.ATM_NM, node.ATM_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "AMC", node.AMC_NE, node.AMC_NM, node.AMC_ND20);
                    this.GetResultNodeCount(resultList, item.Id, "AZM", node.AZM_NE, node.AZM_NM, node.AZM_ND15);
                    this.GetResultNodeCount(resultList, item.Id, "CAZ", node.CAZ_NE, node.CAZ_NM, node.CAZ_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CEC", node.CEC_NE, node.CEC_NM, node.CEC_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CEP", node.CEP_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CFP", node.CFP_NE, node.CFP_NM, node.CFP_ND75);
                    this.GetResultNodeCount(resultList, item.Id, "CHL", node.CHL_NE, node.CHL_NM, node.CHL_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CIP", node.CIP_NE, node.CIP_NM, node.CIP_ND5);
                    this.GetResultNodeCount(resultList, item.Id, "CLI", node.CLI_NE, node.CLI_NM, node.CLI_ND2);
                    this.GetResultNodeCount(resultList, item.Id, "CRB", node.CRB_ND100);
                    this.GetResultNodeCount(resultList, item.Id, "CRO", node.CRO_NE, node.CRO_NM, node.CRO_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CSL", node.CSL_NM, node.CSL_ND30, node.CSL_ND75);
                    this.GetResultNodeCount(resultList, item.Id, "CTT", node.CTT_NE, node.CTT_NM, node.CTT_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CTX", node.CTX_NE, node.CTX_NM, node.CTX_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CXM", node.CXM_NE, node.CXM_NM, node.CXM_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CZO", node.CZO_NE, node.CZO_NM, node.CZO_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CZX", node.CZX_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "DOR", node.DOR_NE, node.DOR_NM, node.DOR_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "DOX", node.DOX_NE, node.DOX_NM, node.DOX_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "ERY", node.ERY_NE, node.ERY_NM, node.ERY_ND15);
                    this.GetResultNodeCount(resultList, item.Id, "ETP", node.ETP_NE, node.ETP_NM, node.ETP_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "FEP", node.FEP_NE, node.FEP_NM, node.FEP_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "FOS", node.FOS_NE, node.FOS_NM, node.FOS_ND200);
                    this.GetResultNodeCount(resultList, item.Id, "FOX", node.FOX_NE, node.FOX_NM, node.FOX_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "GEH", node.GEH_NM, node.GEH_ND120);
                    this.GetResultNodeCount(resultList, item.Id, "GEN", node.GEN_NE, node.GEN_NM, node.GEN_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "IPM", node.IPM_NE, node.IPM_NM, node.IPM_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "LNZ", node.LNZ_NE, node.LNZ_NM, node.LNZ_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "LVX", node.LVX_NE, node.LVX_NM, node.LVX_ND5);
                    this.GetResultNodeCount(resultList, item.Id, "MAN", node.MAN_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "MEM", node.MEM_NE, node.MEM_NM, node.MEM_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "MET", node.MET_NM, node.MET_ND5);
                    this.GetResultNodeCount(resultList, item.Id, "MEZ", node.MEZ_ND75);
                    this.GetResultNodeCount(resultList, item.Id, "MFX", node.MFX_NE, node.MFX_NM, node.MFX_ND5, node.MFX_ND);
                    this.GetResultNodeCount(resultList, item.Id, "MNO", node.MNO_NE, node.MNO_NM, node.MNO_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "NET", node.NET_NE, node.NET_NM, node.NET_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "NIT", node.NIT_NE, node.NIT_NM, node.NIT_ND300);
                    this.GetResultNodeCount(resultList, item.Id, "NOR", node.NOR_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "NOV", node.NOV_ND5, node.NOV_ND5);
                    this.GetResultNodeCount(resultList, item.Id, "OFX", node.OFX_ND5);
                    this.GetResultNodeCount(resultList, item.Id, "OXA", node.OXA_NE, node.OXA_NM, node.OXA_ND1);
                    this.GetResultNodeCount(resultList, item.Id, "PEN", node.PEN_NE, node.PEN_NM, node.PEN_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "PIP", node.PIP_NE, node.PIP_NM, node.PIP_ND100);
                    this.GetResultNodeCount(resultList, item.Id, "POL", node.POL_NE, node.POL_NM, node.POL_ND300);
                    this.GetResultNodeCount(resultList, item.Id, "QDA", node.QDA_NE, node.QDA_NM, node.QDA_ND15);
                    this.GetResultNodeCount(resultList, item.Id, "RIF", node.RIF_NE, node.RIF_NM, node.RIF_ND5);
                    this.GetResultNodeCount(resultList, item.Id, "SAM", node.SAM_NE, node.SAM_NM, node.SAM_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "SSS", node.SSS_ND200);
                    this.GetResultNodeCount(resultList, item.Id, "STH", node.STH_NE, node.STH_NM, node.STH_ND300);
                    this.GetResultNodeCount(resultList, item.Id, "STR", node.STR_NE, node.STR_NM, node.STR_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "SXT", node.SXT_NE, node.SXT_NM, node.SXT_ND1_2);
                    this.GetResultNodeCount(resultList, item.Id, "TCC", node.TCC_NE, node.TCC_NM, node.TCC_ND75);
                    this.GetResultNodeCount(resultList, item.Id, "TCY", node.TCY_NE, node.TCY_NM, node.TCY_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "TEC", node.TEC_NE, node.TEC_NM, node.TEC_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "TGC", node.TGC_NE, node.TGC_NM, node.TGC_ND15);
                    this.GetResultNodeCount(resultList, item.Id, "TIC", node.TIC_NE, node.TIC_NM, node.TIC_ND75);
                    this.GetResultNodeCount(resultList, item.Id, "TOB", node.TOB_NE, node.TOB_NM, node.TOB_ND10);
                    this.GetResultNodeCount(resultList, item.Id, "TZP", node.TZP_NE, node.TZP_NM, node.TZP_ND100);
                    this.GetResultNodeCount(resultList, item.Id, "VAN", node.VAN_NE, node.VAN_NM, node.VAN_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CPT", node.CPT_NE, node.CPT_NM, node.CPT_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CZA", node.CZA_NE, node.CZA_NM, node.CZA_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "AZA", node.AZA_NE, node.AZA_NM, node.AZA_ND30);
                    this.GetResultNodeCount(resultList, item.Id, "CZT", node.CZT_NE, node.CZT_NM, node.CZT_ND30);
                    //CZT->COL,应该是以前代码写错了
                    this.GetResultNodeCount(resultList, item.Id, "COL", node.COL_NE, node.COL_NM, node.COL_ND10);

                    //依拉环素
                    this.GetResultNodeCount(resultList, item.Id, "ERV", node.ERV_NE, node.ERV_NM, node.ERV_ND20);
                }
            }

            //3、修正之前计算好的数据和计算耐药性比例
            resultList = resultList.Where(m => m.IntermediaryCount > 0 || m.ResistanceCount > 0 || m.SensitiveCount > 0).ToList();

            long _medicalDataId = list.FirstOrDefault()?.MedicalDataId ?? 0;
            List<long> dataitemIds = list.Select(m => m.MedicalDataItemId).Distinct().ToList();

            Expression<Func<MedicalDataItem, bool>> predicate2 = r => r.MedicalDataId == _medicalDataId && dataitemIds.Contains(r.Id) && r.Mark > 0 && r.IsValid;
            if (organismIdList != null && organismIdList.Count > 0)
            {
                predicate2 = predicate2.And(r => organismIdList.Contains(r.OrganismId));
            }
            List<MedicalDataItem> _dataItems = MedicalDataItemService.Query(predicate2);

            IEnumerable<string> fields = from item in typeof(MedicalDataItem).GetProperties() select item.Name;
            foreach (var item in resultList)
            {
                Expression<Func<MedicalDataItem, bool>> predicate = r => r.ORGANISM == item.OrganismCode && r.IsValid == true && r.Mark > 0;

                List<string> fieldItems = fields.Where(r => r.StartsWith(item.AntibioticCode)).ToList();
                Expression<Func<MedicalDataItem, bool>> _innerPredicate = r => false;
                foreach (var fieldItem in fieldItems)
                {
                    predicate = predicate.And(r => !(r.GetType().GetProperty(fieldItem).GetValue(r, null) ?? "").ToString().Trim().Equals("0"));
                    _innerPredicate = _innerPredicate.Or(r => (r.GetType().GetProperty(fieldItem).GetValue(r, null) ?? "").ToString().Trim().Length > 0);
                }
                predicate = predicate.And(_innerPredicate);

                item.DataCount = _dataItems.Count(predicate.Compile());
            }

            //4、返回
            return resultList;
        }

        /// <summary>
        /// 根据已有的耐药性结果数据，进行耐药性分析
        /// </summary>
        /// <param name="list">耐药性结果的集合数据</param>
        /// <returns></returns>
        public List<GetAntibioticResultModel> GetResultByOrganismIds(List<MedicalAntibioticResult> list, List<long> organismIds)
        {
            List<GetAntibioticResultModel> resultList = new List<GetAntibioticResultModel>();
            if (organismIds == null || organismIds.Count == 0 || list == null || !list.Any()) return resultList;  //未查询到数据，直接返回

            // 细菌集合
            var organismList = this.MedicalOrganismService.Query(m => organismIds.Contains(m.Id)).ToList();

            // 医学数据 抗生素（基础数据）
            var antibioticList = this.MedicalAntibioticService.Query().ToList();

            //2、把细菌和药物的关联关系建立好
            foreach (var item in organismList)
            {
                // 上传的数据项
                var arList = list.Where(m => m.OrganismId == item.Id).ToList();
                if (arList == null || !arList.Any()) continue;//这个细菌没有规则数据，所以不用处理，理论上应该不会出现

                //A：添加细菌和药物的关联数据
                foreach (var antItem in antibioticList)
                {
                    GetAntibioticResultModel r = new GetAntibioticResultModel()
                    {
                        OrganismId = item.Id,
                        OrganismCode = item.Code,
                        OrganismName = item.Name,
                        AntibioticCode = antItem.Code,
                        AntibioticId = antItem.Id,
                        AntibioticName = antItem.Name,
                        OrganismCount = arList.Count
                    };

                    resultList.Add(r);
                }

                //B：计算药物和细菌的耐药性数量
                foreach (var node in arList)
                {
                    //设置数据的耐药性数量  优先级：E-TSET > MIC > KB （_NE > __NM > _ND）	 
                    this.GetResultNodeCount(resultList, organismIds, "AMK",node.AMK_NE, node.AMK_NM, node.AMK_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "AMP",node.AMP_NE, node.AMP_NM, node.AMP_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "AMX",node.AMX_NM, node.AMX_ND30, node.AMX_ND25);
                    this.GetResultNodeCount(resultList, organismIds, "ATM",node.ATM_NE, node.ATM_NM, node.ATM_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "AMC",node.AMC_NE, node.AMC_NM, node.AMC_ND20);
                    this.GetResultNodeCount(resultList, organismIds, "AZM",node.AZM_NE, node.AZM_NM, node.AZM_ND15);
                    this.GetResultNodeCount(resultList, organismIds, "CAZ",node.CAZ_NE, node.CAZ_NM, node.CAZ_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CEC",node.CEC_NE, node.CEC_NM, node.CEC_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CEP",node.CEP_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CFP",node.CFP_NE, node.CFP_NM, node.CFP_ND75);
                    this.GetResultNodeCount(resultList, organismIds, "CHL",node.CHL_NE, node.CHL_NM, node.CHL_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CIP",node.CIP_NE, node.CIP_NM, node.CIP_ND5);
                    this.GetResultNodeCount(resultList, organismIds, "CLI",node.CLI_NE, node.CLI_NM, node.CLI_ND2);
                    this.GetResultNodeCount(resultList, organismIds, "CRB",node.CRB_ND100);
                    this.GetResultNodeCount(resultList, organismIds, "CRO",node.CRO_NE, node.CRO_NM, node.CRO_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CSL",node.CSL_NM, node.CSL_ND30, node.CSL_ND75);
                    this.GetResultNodeCount(resultList, organismIds, "CTT",node.CTT_NE, node.CTT_NM, node.CTT_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CTX",node.CTX_NE, node.CTX_NM, node.CTX_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CXM",node.CXM_NE, node.CXM_NM, node.CXM_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CZO",node.CZO_NE, node.CZO_NM, node.CZO_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CZX",node.CZX_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "DOR",node.DOR_NE, node.DOR_NM, node.DOR_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "DOX",node.DOX_NE,node.DOX_NM,node.DOX_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "ERY",node.ERY_NE, node.ERY_NM, node.ERY_ND15);
                    this.GetResultNodeCount(resultList, organismIds, "ETP",node.ETP_NE, node.ETP_NM, node.ETP_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "FEP",node.FEP_NE, node.FEP_NM, node.FEP_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "FOS",node.FOS_NE, node.FOS_NM, node.FOS_ND200);
                    this.GetResultNodeCount(resultList, organismIds, "FOX",node.FOX_NE, node.FOX_NM, node.FOX_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "GEH",node.GEH_NM, node.GEH_ND120);
                    this.GetResultNodeCount(resultList, organismIds, "GEN",node.GEN_NE, node.GEN_NM, node.GEN_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "IPM",node.IPM_NE, node.IPM_NM, node.IPM_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "LNZ",node.LNZ_NE, node.LNZ_NM, node.LNZ_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "LVX",node.LVX_NE, node.LVX_NM, node.LVX_ND5);
                    this.GetResultNodeCount(resultList, organismIds, "MAN",node.MAN_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "MEM",node.MEM_NE, node.MEM_NM, node.MEM_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "MET",node.MET_NM, node.MET_ND5);
                    this.GetResultNodeCount(resultList, organismIds, "MEZ",node.MEZ_ND75);
                    this.GetResultNodeCount(resultList, organismIds, "MFX",node.MFX_NE, node.MFX_NM, node.MFX_ND5, node.MFX_ND);
                    this.GetResultNodeCount(resultList, organismIds, "MNO",node.MNO_NE, node.MNO_NM, node.MNO_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "NET",node.NET_NE, node.NET_NM, node.NET_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "NIT",node.NIT_NE, node.NIT_NM, node.NIT_ND300);
                    this.GetResultNodeCount(resultList, organismIds, "NOR",node.NOR_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "NOV",node.NOV_ND5, node.NOV_ND5);
                    this.GetResultNodeCount(resultList, organismIds, "OFX",node.OFX_ND5);
                    this.GetResultNodeCount(resultList, organismIds, "OXA",node.OXA_NE, node.OXA_NM, node.OXA_ND1);
                    this.GetResultNodeCount(resultList, organismIds, "PEN",node.PEN_NE, node.PEN_NM, node.PEN_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "PIP",node.PIP_NE, node.PIP_NM, node.PIP_ND100);
                    this.GetResultNodeCount(resultList, organismIds, "POL",node.POL_NE, node.POL_NM, node.POL_ND300);
                    this.GetResultNodeCount(resultList, organismIds, "QDA",node.QDA_NE, node.QDA_NM, node.QDA_ND15);
                    this.GetResultNodeCount(resultList, organismIds, "RIF",node.RIF_NE, node.RIF_NM, node.RIF_ND5);
                    this.GetResultNodeCount(resultList, organismIds, "SAM",node.SAM_NE, node.SAM_NM, node.SAM_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "SSS",node.SSS_ND200);
                    this.GetResultNodeCount(resultList, organismIds, "STH",node.STH_NE, node.STH_NM, node.STH_ND300);
                    this.GetResultNodeCount(resultList, organismIds, "STR",node.STR_NE, node.STR_NM, node.STR_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "SXT",node.SXT_NE, node.SXT_NM, node.SXT_ND1_2);
                    this.GetResultNodeCount(resultList, organismIds, "TCC",node.TCC_NE, node.TCC_NM, node.TCC_ND75);
                    this.GetResultNodeCount(resultList, organismIds, "TCY",node.TCY_NE, node.TCY_NM, node.TCY_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "TEC",node.TEC_NE, node.TEC_NM, node.TEC_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "TGC",node.TGC_NE, node.TGC_NM, node.TGC_ND15);
                    this.GetResultNodeCount(resultList, organismIds, "TIC",node.TIC_NE, node.TIC_NM, node.TIC_ND75);
                    this.GetResultNodeCount(resultList, organismIds, "TOB",node.TOB_NE, node.TOB_NM, node.TOB_ND10);
                    this.GetResultNodeCount(resultList, organismIds, "TZP",node.TZP_NE, node.TZP_NM, node.TZP_ND100);
                    this.GetResultNodeCount(resultList, organismIds, "VAN",node.VAN_NE, node.VAN_NM, node.VAN_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CPT",node.CPT_NE, node.CPT_NM, node.CPT_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CZA",node.CZA_NE, node.CZA_NM, node.CZA_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "AZA",node.AZA_NE, node.AZA_NM, node.AZA_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CZT",node.CZT_NE, node.CZT_NM, node.CZT_ND30);
                    this.GetResultNodeCount(resultList, organismIds, "CZT",node.COL_NE, node.COL_NM, node.COL_ND10);

                    //依拉环素
                    this.GetResultNodeCount(resultList, organismIds, "ERV", node.ERV_NE, node.ERV_NM, node.ERV_ND20);
                }
            }

            //3、修正之前计算好的数据和计算耐药性比例
            resultList = resultList.Where(m => m.IntermediaryCount > 0 || m.ResistanceCount > 0 || m.SensitiveCount > 0).ToList();

            long _medicalDataId = list.FirstOrDefault()?.MedicalDataId ?? 0;
            List<long> dataitemIds = list.Select(m => m.MedicalDataItemId).Distinct().ToList();

            Expression<Func<MedicalDataItem, bool>> predicate2 = r => r.MedicalDataId == _medicalDataId && dataitemIds.Contains(r.Id) && r.Mark > 0 && r.IsValid;
            if (organismIds != null && organismIds.Count > 0)
            {
                predicate2 = predicate2.And(r => organismIds.Contains(r.OrganismId));
            }

            List<MedicalDataItem> _dataItems = MedicalDataItemService.Query(predicate2);

            IEnumerable<string> fields = from item in typeof(MedicalDataItem).GetProperties() select item.Name;

            List<string> organismCodes = organismList.Select(r => r.Code).ToList();
            foreach (var item in resultList)
            {
                Expression<Func<MedicalDataItem, bool>> predicate = r => organismCodes.Contains(r.ORGANISM) && r.IsValid == true && r.Mark > 0;

                List<string> fieldItems = fields.Where(r => r.StartsWith(item.AntibioticCode)).ToList();
                Expression<Func<MedicalDataItem, bool>> _innerPredicate = r => false;
                foreach (var fieldItem in fieldItems)
                {
                    predicate = predicate.And(r => !(r.GetType().GetProperty(fieldItem).GetValue(r, null) ?? "").ToString().Trim().Equals("0"));
                    _innerPredicate = _innerPredicate.Or(r => (r.GetType().GetProperty(fieldItem).GetValue(r, null) ?? "").ToString().Trim().Length > 0);
                }
                predicate = predicate.And(_innerPredicate);

                item.DataCount = _dataItems.Count(predicate.Compile());
            }

            //4、返回
            return resultList;
        }

        /// <summary>
        /// 获取MRSA	MSSA	MRCNS	MSCNS 等的统计数据，专用函数
        /// </summary>
        /// <param name="medicalId">上传id</param>
        /// <param name="organismGroupName">菌属分组名称，区分 MRSA	MSSA	MRCNS	MSCNS </param>
        /// <returns></returns>
        public List<MedicalAntibioticResult> QueryMRSAList(long medicalId, string organismGroupName)
        {
            string sql = string.Format(@"
                SELECT * FROM  dbo.MedicalAntibioticResult
                WHERE Mark>0  AND MedicalDataId = {0} AND  OrganismId IN 
                ( SELECT Id FROM  dbo.MedicalOrganism WHERE GroupName ='{1}' AND Mark>0 )
             ", medicalId, organismGroupName);

            using (var connection = DapperHelper.GetConnection())
            {
                connection.Open();
                var list = connection.Query<MedicalAntibioticResult>(sql).ToList();
                connection.Close();

                list = list ?? new List<MedicalAntibioticResult>();

                return list;
            }
        }

        #endregion

    }
}
