using System;
using System.Linq;
using System.Text;
using System.Data;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ManageSystem.Core.Utility;
using System.IO;
using ManageSystem.Core.Utility.FastDBF;
using ManageSystem.Data;
using Dapper;
using ManageSystem.Core.Domain.Teams;
using static ManageSystem.Core.Utility.FastDBF.DbfColumn;
using DotNetDBF;
using System.Collections.Generic;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalDataItem 
    /// </summary>
    public partial class MedicalDataItemService : BaseService<MedicalDataItem>, IMedicalDataItemService
    {

        public MedicalDataItemService(IRepository<MedicalDataItem> repository) : base(repository)
        {

        }

        /// <summary>
        /// 根据医学数据Id获取对应的分页数据
        /// </summary>
        /// <param name="medicalDataId">对应的医学数据Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<MedicalDataItem> QueryPage(long medicalDataId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (medicalDataId > 0)
                query = query.Where(m => m.MedicalDataId == medicalDataId);

            query = query.OrderBy(m => m.Sort);
            var list = new PagedList<MedicalDataItem>(query, pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 根据医学数据的Id，产生Excel数据，成功返回文件的相对路径
        /// </summary>
        /// <param name="entity">医学数据</param>
        ///  <param name="hospital">所属医院</param>
        /// <returns></returns>
        public string GetExcel(MedicalData entity, HospitalTeamList hospitalTeam)
        {
            try
            {
                ExcelPackage ep = new ExcelPackage();
                //ExportExcel ex = new ExportExcel();

                #region 数据源
                var data = this.Query(m => m.Mark > 0 && m.MedicalDataId == entity.Id).OrderBy(m => m.Sort)
                    .Select(x =>
                    {
                        return new
                        {
                            COUNTRY_A = x.COUNTRY_A,
                            LABORATORY = x.LABORATORY,
                            PATIENT_ID = x.PATIENT_ID,
                            FIRST_NAME = x.FIRST_NAME,
                            LAST_NAME = x.LAST_NAME,
                            SEX = x.SEX,
                            AGE = x.AGE,
                            DATE_BIRTH = x.DATE_BIRTH.Contains("1900") ? "" : x.DATE_BIRTH,
                            WARD = x.WARD,
                            WARD_TYPE = x.WARD_TYPE,
                            INSTITUT = x.INSTITUT,
                            DEPARTMENT = x.DEPARTMENT,
                            SPEC_NUM = x.SPEC_NUM,
                            SPEC_DATE = x.SPEC_DATE,
                            SPEC_TYPE = x.SPEC_TYPE,
                            SPEC_CODE = x.SPEC_CODE,
                            ORGANISM = x.ORGANISM,
                            ORG_TYPE = x.ORG_TYPE,
                            ESBL = x.ESBL,
                            BETA_LACT = x.BETA_LACT,
                            INDUC_CLI = x.INDUC_CLI,
                            CARBAPENEM = x.CARBAPENEM,
                            COMMENT = x.COMMENT,
                            #region
                            //AMC_ND20 = x.AMC_ND20,
                            //AMC_NM = x.AMC_NM,
                            //AMK_ND30 = x.AMK_ND30,                          
                            //AZM_ND15 = x.AZM_ND15,
                            //AMP_ND10 = x.AMP_ND10,
                            //SAM_ND10 = x.SAM_ND10,
                            //ATM_ND30 = x.ATM_ND30,
                            //OXA_ND1 = x.OXA_ND1,
                            //POL_ND300 = x.POL_ND300,
                            //NIT_ND300 = x.NIT_ND300,
                            //SXT_ND1_2 = x.SXT_ND1_2,
                            //STH_ND300 = x.STH_ND300,
                            //GEH_ND120 = x.GEH_ND120,
                            //ERY_ND15 = x.ERY_ND15,
                            //CIP_ND5 = x.CIP_ND5,
                            //CLI_ND2 = x.CLI_ND2,
                            //RIF_ND5 = x.RIF_ND5,
                            //LNZ_ND30 = x.LNZ_ND30,
                            //STR_ND10 = x.STR_ND10,
                            //FOS_ND200 = x.FOS_ND200,
                            //CHL_ND30 = x.CHL_ND30,
                            //MEM_ND10 = x.MEM_ND10,
                            //MNO_ND30 = x.MNO_ND30,
                            //MFX_ND5 = x.MFX_ND5,
                            //PIP_ND100 = x.PIP_ND100,
                            //TZP_ND100 = x.TZP_ND100,
                            //PEN_ND10 = x.PEN_ND10,
                            //GEN_ND10 = x.GEN_ND10,
                            //TCY_ND30 = x.TCY_ND30,
                            //TCC_ND75 = x.TCC_ND75,
                            //TIC_ND75 = x.TIC_ND75,
                            //TEC_ND30 = x.TEC_ND30,
                            //TGC_ND15 = x.TGC_ND15,
                            //FEP_ND30 = x.FEP_ND30,
                            //CXM_ND30 = x.CXM_ND30,
                            //CEC_ND30 = x.CEC_ND30,
                            //CFP_ND75 = x.CFP_ND75,
                            //CSL_ND30 = x.CSL_ND30,
                            //CRO_ND30 = x.CRO_ND30,
                            //CTX_ND30 = x.CTX_ND30,
                            //CAZ_ND30 = x.CAZ_ND30,
                            //FOX_ND30 = x.FOX_ND30,
                            //CZO_ND30 = x.CZO_ND30,
                            //TOB_ND10 = x.TOB_ND10,
                            //VAN_ND30 = x.VAN_ND30,
                            //IPM_ND10 = x.IPM_ND10,
                            //LVX_ND5 = x.LVX_ND5,
                            //OFX_ND5 = x.OFX_ND5,
                            //DOX_ND30 = x.DOX_ND30,
                            //AMK_NM = x.AMK_NM,                  
                            //AZM_NM = x.AZM_NM,
                            //AMP_NM = x.AMP_NM,
                            //SAM_NM = x.SAM_NM,
                            //ATM_NM = x.ATM_NM,
                            //OXA_NM = x.OXA_NM,
                            //POL_NM = x.POL_NM,
                            //NIT_NM = x.NIT_NM,
                            //SXT_NM = x.SXT_NM,
                            //STH_NM = x.STH_NM,
                            //GEH_NM = x.GEH_NM,
                            //ERY_NM = x.ERY_NM,
                            //CIP_NM = x.CIP_NM,
                            //CLI_NM = x.CLI_NM,
                            //RIF_NM = x.RIF_NM,
                            //LNZ_NM = x.LNZ_NM,
                            //STR_NM = x.STR_NM,
                            //FOS_NM = x.FOS_NM,
                            //CHL_NM = x.CHL_NM,
                            //MEM_NM = x.MEM_NM,
                            //MNO_NM = x.MNO_NM,
                            //MFX_NM = x.MFX_NM,
                            //PIP_NM = x.PIP_NM,
                            //TZP_NM = x.TZP_NM,
                            //PEN_NM = x.PEN_NM,
                            //GEN_NM = x.GEN_NM,
                            //PEN_NE = x.PEN_NE,
                            //TCY_NM = x.TCY_NM,
                            //TCC_NM = x.TCC_NM,
                            //TIC_NM = x.TIC_NM,
                            //TEC_NM = x.TEC_NM,
                            //TGC_NM = x.TGC_NM,
                            //FEP_NM = x.FEP_NM,
                            //CXM_NM = x.CXM_NM,
                            //CEC_NM = x.CEC_NM,
                            //CFP_NM = x.CFP_NM,
                            //CSL_NM = x.CSL_NM,
                            //CRO_NM = x.CRO_NM,
                            //CTX_NM = x.CTX_NM,
                            //CAZ_NM = x.CAZ_NM,
                            //FOX_NM = x.FOX_NM,
                            //CZO_NM = x.CZO_NM,
                            //TOB_NM = x.TOB_NM,
                            //VAN_NM = x.VAN_NM,
                            //VAN_NE = x.VAN_NE,
                            //IPM_NM = x.IPM_NM,
                            //LVX_NM = x.LVX_NM,
                            //CTX_NE = x.CTX_NE,
                            //CSL_ND75 = x.CSL_ND75,
                            //ETP_ND10 = x.ETP_ND10,
                            //ETP_NM = x.ETP_NM,
                            //CTT_ND30 = x.CTT_ND30,
                            //CTT_NM = x.CTT_NM,
                            //DOR_ND10 = x.DOR_ND10,
                            //DOR_NM = x.DOR_NM,
                            //NET_ND30 = x.NET_ND30,
                            //NET_NM = x.NET_NM,
                            //QDA_ND15 = x.QDA_ND15,
                            //QDA_NM = x.QDA_NM,
                            //CPT_ND30 = x.CPT_ND30,
                            //CPT_NM = x.CPT_NM,
                            //CPT_NE = x.CPT_NE,
                            //CZA_ND30 = x.CZA_ND30,
                            //CZA_NM = x.CZA_NM,
                            //CZA_NE = x.CZA_NE,
                            //AZA_ND30 = x.AZA_ND30,
                            //AZA_NM = x.AZA_NM,
                            //AZA_NE = x.AZA_NE,
                            //CZT_ND30 = x.CZT_ND30,
                            //CZT_NM = x.CZT_NM,
                            //CZT_NE = x.CZT_NE,
                            ////TGC_NE = x.TGC_NE,//新增字段
                            //DOX_NM = x.DOX_NM //新增字段
                            #endregion
                            AMC_ND20 = x.AMC_ND20,
                            AMC_NE = x.AMC_NE,
                            AMC_NM = x.AMC_NM,
                            AMK_ND30 = x.AMK_ND30,
                            AMK_NE = x.AMK_NE,
                            AMK_NM = x.AMK_NM,
                            AMP_ND10 = x.AMP_ND10,
                            AMP_NE = x.AMP_NE,
                            AMP_NM = x.AMP_NM,
                            ATM_ND30 = x.ATM_ND30,
                            ATM_NE = x.ATM_NE,
                            ATM_NM = x.ATM_NM,
                            AZA_ND30 = x.AZA_ND30,
                            AZA_NE = x.AZA_NE,
                            AZA_NM = x.AZA_NM,
                            AZM_ND15 = x.AZM_ND15,
                            AZM_NE = x.AZM_NE,
                            AZM_NM = x.AZM_NM,
                            CAZ_ND30 = x.CAZ_ND30,
                            CAZ_NE = x.CAZ_NE,
                            CAZ_NM = x.CAZ_NM,
                            CEC_ND30 = x.CEC_ND30,
                            CEC_NE = x.CEC_NE,
                            CEC_NM = x.CEC_NM,
                            CFP_ND75 = x.CFP_ND75,
                            CFP_NE = x.CFP_NE,
                            CFP_NM = x.CFP_NM,
                            CHL_ND30 = x.CHL_ND30,
                            CHL_NE = x.CHL_NE,
                            CHL_NM = x.CHL_NM,
                            CIP_ND5 = x.CIP_ND5,
                            CIP_NE = x.CIP_NE,
                            CIP_NM = x.CIP_NM,
                            CLI_ND2 = x.CLI_ND2,
                            CLI_NE = x.CLI_NE,
                            CLI_NM = x.CLI_NM,
                            COL_ND10 = x.COL_ND10,
                            COL_NE = x.COL_NE,
                            COL_NM = x.COL_NM,
                            CPT_ND30 = x.CPT_ND30,
                            CPT_NE = x.CPT_NE,
                            CPT_NM = x.CPT_NM,
                            CRO_ND30 = x.CRO_ND30,
                            CRO_NE = x.CRO_NE,
                            CRO_NM = x.CRO_NM,
                            CSL_ND30 = x.CSL_ND30,
                            CSL_ND75 = x.CSL_ND75,
                            CSL_NM = x.CSL_NM,
                            CTT_ND30 = x.CTT_ND30,
                            CTT_NE = x.CTT_NE,
                            CTT_NM = x.CTT_NM,
                            CTX_ND30 = x.CTX_ND30,
                            CTX_NE = x.CTX_NE,
                            CTX_NM = x.CTX_NM,
                            CXM_ND30 = x.CXM_ND30,
                            CXM_NE = x.CXM_NE,
                            CXM_NM = x.CXM_NM,
                            CZA_ND30 = x.CZA_ND30,
                            CZA_NE = x.CZA_NE,
                            CZA_NM = x.CZA_NM,
                            CZO_ND30 = x.CZO_ND30,
                            CZO_NE = x.CZO_NE,
                            CZO_NM = x.CZO_NM,
                            CZT_ND30 = x.CZT_ND30,
                            CZT_NE = x.CZT_NE,
                            CZT_NM = x.CZT_NM,
                            DOR_ND10 = x.DOR_ND10,
                            DOR_NE = x.DOR_NE,
                            DOR_NM = x.DOR_NM,
                            DOX_ND30 = x.DOX_ND30,
                            DOX_NE = x.DOX_NE,
                            DOX_NM = x.DOX_NM,
                            ERY_ND15 = x.ERY_ND15,
                            ERY_NE = x.ERY_NE,
                            ERY_NM = x.ERY_NM,
                            ETP_ND10 = x.ETP_ND10,
                            ETP_NE = x.ETP_NE,
                            ETP_NM = x.ETP_NM,
                            FEP_ND30 = x.FEP_ND30,
                            FEP_NE = x.FEP_NE,
                            FEP_NM = x.FEP_NM,
                            FOS_ND200 = x.FOS_ND200,
                            FOS_NE = x.FOS_NE,
                            FOS_NM = x.FOS_NM,
                            FOX_ND30 = x.FOX_ND30,
                            FOX_NE = x.FOX_NE,
                            FOX_NM = x.FOX_NM,
                            GEH_ND120 = x.GEH_ND120,
                            GEH_NM = x.GEH_NM,
                            GEN_ND10 = x.GEN_ND10,
                            GEN_NE = x.GEN_NE,
                            GEN_NM = x.GEN_NM,
                            IPM_ND10 = x.IPM_ND10,
                            IPM_NE = x.IPM_NE,
                            IPM_NM = x.IPM_NM,
                            LNZ_ND30 = x.LNZ_ND30,
                            LNZ_NE = x.LNZ_NE,
                            LNZ_NM = x.LNZ_NM,
                            LVX_ND5 = x.LVX_ND5,
                            LVX_NE = x.LVX_NE,
                            LVX_NM = x.LVX_NM,
                            MEM_ND10 = x.MEM_ND10,
                            MEM_NE = x.MEM_NE,
                            MEM_NM = x.MEM_NM,
                            MFX_ND5 = x.MFX_ND5,
                            MFX_NE = x.MFX_NE,
                            MFX_NM = x.MFX_NM,
                            MNO_ND30 = x.MNO_ND30,
                            MNO_NE = x.MNO_NE,
                            MNO_NM = x.MNO_NM,
                            NET_ND30 = x.NET_ND30,
                            NET_NE = x.NET_NE,
                            NET_NM = x.NET_NM,
                            NIT_ND300 = x.NIT_ND300,
                            NIT_NE = x.NIT_NE,
                            NIT_NM = x.NIT_NM,
                            OFX_ND5 = x.OFX_ND5,
                            OXA_ND1 = x.OXA_ND1,
                            OXA_NE = x.OXA_NE,
                            OXA_NM = x.OXA_NM,
                            PEN_ND10 = x.PEN_ND10,
                            PEN_NE = x.PEN_NE,
                            PEN_NM = x.PEN_NM,
                            PIP_ND100 = x.PIP_ND100,
                            PIP_NE = x.PIP_NE,
                            PIP_NM = x.PIP_NM,
                            POL_ND300 = x.POL_ND300,
                            POL_NE = x.POL_NE,
                            POL_NM = x.POL_NM,
                            QDA_ND15 = x.QDA_ND15,
                            QDA_NE = x.QDA_NE,
                            QDA_NM = x.QDA_NM,
                            RIF_ND5 = x.RIF_ND5,
                            RIF_NE = x.RIF_NE,
                            RIF_NM = x.RIF_NM,
                            SAM_ND10 = x.SAM_ND10,
                            SAM_NE = x.SAM_NE,
                            SAM_NM = x.SAM_NM,
                            STH_ND300 = x.STH_ND300,
                            STH_NE = x.STH_NE,
                            STH_NM = x.STH_NM,
                            STR_ND10 = x.STR_ND10,
                            STR_NE = x.STR_NE,
                            STR_NM = x.STR_NM,
                            SXT_ND1_2 = x.SXT_ND1_2,
                            SXT_NE = x.SXT_NE,
                            SXT_NM = x.SXT_NM,
                            TCC_ND75 = x.TCC_ND75,
                            TCC_NE = x.TCC_NE,
                            TCC_NM = x.TCC_NM,
                            TCY_ND30 = x.TCY_ND30,
                            TCY_NE = x.TCY_NE,
                            TCY_NM = x.TCY_NM,
                            TEC_ND30 = x.TEC_ND30,
                            TEC_NE = x.TEC_NE,
                            TEC_NM = x.TEC_NM,
                            TGC_ND15 = x.TGC_ND15,
                            TGC_NE = x.TGC_NE,
                            TGC_NM = x.TGC_NM,
                            TIC_ND75 = x.TIC_ND75,
                            TIC_NE = x.TIC_NE,
                            TIC_NM = x.TIC_NM,
                            TOB_ND10 = x.TOB_ND10,
                            TOB_NE = x.TOB_NE,
                            TOB_NM = x.TOB_NM,
                            TZP_ND100 = x.TZP_ND100,
                            TZP_NE = x.TZP_NE,
                            TZP_NM = x.TZP_NM,
                            VAN_ND30 = x.VAN_ND30,
                            VAN_NE = x.VAN_NE,
                            VAN_NM = x.VAN_NM,


                        };
                    });
                #endregion

                ExcelWorksheet ws = ep.Workbook.Worksheets.Add(typeof(T).Name);

                #region 设置表头  弃用

                //设置表头，因为自动设置表头会导致下划线丢失，所以手动设置
                //ws.Cells["A1"].Value = "COUNTRY_A";
                //ws.Cells["B1"].Value = "LABORATORY";
                //ws.Cells["C1"].Value = "PATIENT_ID";
                //ws.Cells["D1"].Value = "FIRST_NAME";
                //ws.Cells["E1"].Value = "LAST_NAME";
                //ws.Cells["F1"].Value = "SEX";
                //ws.Cells["G1"].Value = "AGE";
                //ws.Cells["H1"].Value = "DATE_BIRTH";
                //ws.Cells["I1"].Value = "WARD";
                //ws.Cells["J1"].Value = "WARD_TYPE";
                //ws.Cells["K1"].Value = "INSTITUT";
                //ws.Cells["L1"].Value = "DEPARTMENT";
                //ws.Cells["M1"].Value = "SPEC_NUM";
                //ws.Cells["N1"].Value = "SPEC_DATE";
                //ws.Cells["O1"].Value = "SPEC_TYPE";
                //ws.Cells["P1"].Value = "SPEC_CODE";
                //ws.Cells["Q1"].Value = "ORGANISM";
                //ws.Cells["R1"].Value = "ORG_TYPE";
                //ws.Cells["S1"].Value = "ESBL";
                //ws.Cells["T1"].Value = "BETA_LACT";
                //ws.Cells["U1"].Value = "AMK_ND30";
                //ws.Cells["V1"].Value = "AMC_ND20";
                //ws.Cells["W1"].Value = "AZM_ND15";
                //ws.Cells["X1"].Value = "AMP_ND10";
                //ws.Cells["Y1"].Value = "SAM_ND10";
                //ws.Cells["Z1"].Value = "ATM_ND30";
                //ws.Cells["AA1"].Value = "OXA_ND1";
                //ws.Cells["AB1"].Value = "POL_ND300";
                //ws.Cells["AC1"].Value = "NIT_ND300";
                //ws.Cells["AD1"].Value = "SXT_ND1_2";
                //ws.Cells["AE1"].Value = "STH_ND300";
                //ws.Cells["AF1"].Value = "GEH_ND120";
                //ws.Cells["AG1"].Value = "ERY_ND15";
                //ws.Cells["AH1"].Value = "CIP_ND5";
                //ws.Cells["AI1"].Value = "CLI_ND2";
                //ws.Cells["AJ1"].Value = "RIF_ND5";
                //ws.Cells["AK1"].Value = "LNZ_ND30";
                //ws.Cells["AL1"].Value = "STR_ND10";
                //ws.Cells["AM1"].Value = "FOS_ND200";
                //ws.Cells["AN1"].Value = "CHL_ND30";
                //ws.Cells["AO1"].Value = "MEM_ND10";
                //ws.Cells["AP1"].Value = "MNO_ND30";
                //ws.Cells["AQ1"].Value = "MFX_ND5";
                //ws.Cells["AR1"].Value = "PIP_ND100";
                //ws.Cells["AS1"].Value = "TZP_ND100";
                //ws.Cells["AT1"].Value = "PEN_ND10";
                //ws.Cells["AU1"].Value = "GEN_ND10";
                //ws.Cells["AV1"].Value = "TCY_ND30";
                //ws.Cells["AW1"].Value = "TCC_ND75";
                //ws.Cells["AX1"].Value = "TIC_ND75";
                //ws.Cells["AY1"].Value = "TEC_ND30";
                //ws.Cells["AZ1"].Value = "TGC_ND15";
                //ws.Cells["BA1"].Value = "FEP_ND30";
                //ws.Cells["BB1"].Value = "CXM_ND30";
                //ws.Cells["BC1"].Value = "CEC_ND30";
                //ws.Cells["BD1"].Value = "CFP_ND75";
                //ws.Cells["BE1"].Value = "CSL_ND30";
                //ws.Cells["BF1"].Value = "CRO_ND30";
                //ws.Cells["BG1"].Value = "CTX_ND30";
                //ws.Cells["BH1"].Value = "CAZ_ND30";
                //ws.Cells["BI1"].Value = "FOX_ND30";
                //ws.Cells["BJ1"].Value = "CZO_ND30";
                //ws.Cells["BK1"].Value = "TOB_ND10";
                //ws.Cells["BL1"].Value = "VAN_ND30";
                //ws.Cells["BM1"].Value = "IPM_ND10";
                //ws.Cells["BN1"].Value = "LVX_ND5";
                //ws.Cells["BO1"].Value = "OFX_ND5";
                //ws.Cells["BP1"].Value = "DOX_ND30";
                //ws.Cells["BQ1"].Value = "AMK_NM";
                //ws.Cells["BR1"].Value = "AMC_NM";
                //ws.Cells["BS1"].Value = "AZM_NM";
                //ws.Cells["BT1"].Value = "AMP_NM";
                //ws.Cells["BU1"].Value = "SAM_NM";
                //ws.Cells["BV1"].Value = "ATM_NM";
                //ws.Cells["BW1"].Value = "OXA_NM";
                //ws.Cells["BX1"].Value = "POL_NM";
                //ws.Cells["BY1"].Value = "NIT_NM";
                //ws.Cells["BZ1"].Value = "SXT_NM";
                //ws.Cells["CA1"].Value = "STH_NM";
                //ws.Cells["CB1"].Value = "GEH_NM";
                //ws.Cells["CC1"].Value = "ERY_NM";
                //ws.Cells["CD1"].Value = "CIP_NM";
                //ws.Cells["CE1"].Value = "CLI_NM";
                //ws.Cells["CF1"].Value = "RIF_NM";
                //ws.Cells["CG1"].Value = "LNZ_NM";
                //ws.Cells["CH1"].Value = "STR_NM";
                //ws.Cells["CI1"].Value = "FOS_NM";
                //ws.Cells["CJ1"].Value = "CHL_NM";
                //ws.Cells["CK1"].Value = "MEM_NM";
                //ws.Cells["CL1"].Value = "MNO_NM";
                //ws.Cells["CM1"].Value = "MFX_NM";
                //ws.Cells["CN1"].Value = "PIP_NM";
                //ws.Cells["CO1"].Value = "TZP_NM";
                //ws.Cells["CP1"].Value = "PEN_NM";
                //ws.Cells["CQ1"].Value = "GEN_NM";
                //ws.Cells["CR1"].Value = "PEN_NE";
                //ws.Cells["CS1"].Value = "TCY_NM";
                //ws.Cells["CT1"].Value = "TCC_NM";
                //ws.Cells["CU1"].Value = "TIC_NM";
                //ws.Cells["CV1"].Value = "TEC_NM";
                //ws.Cells["CW1"].Value = "TGC_NM";
                //ws.Cells["CX1"].Value = "FEP_NM";
                //ws.Cells["CY1"].Value = "CXM_NM";
                //ws.Cells["CZ1"].Value = "CEC_NM";
                //ws.Cells["DA1"].Value = "CFP_NM";
                //ws.Cells["DB1"].Value = "CSL_NM";
                //ws.Cells["DC1"].Value = "CRO_NM";
                //ws.Cells["DD1"].Value = "CTX_NM";
                //ws.Cells["DE1"].Value = "CAZ_NM";
                //ws.Cells["DF1"].Value = "FOX_NM";
                //ws.Cells["DG1"].Value = "CZO_NM";
                //ws.Cells["DH1"].Value = "TOB_NM";
                //ws.Cells["DI1"].Value = "VAN_NM";
                //ws.Cells["DJ1"].Value = "VAN_NE";
                //ws.Cells["DK1"].Value = "IPM_NM";
                //ws.Cells["DL1"].Value = "LVX_NM";
                //ws.Cells["DM1"].Value = "CTX_NE";
                //ws.Cells["DN1"].Value = "CSL_ND75";
                //ws.Cells["DO1"].Value = "ETP_ND10";
                //ws.Cells["DP1"].Value = "ETP_NM";
                //ws.Cells["DQ1"].Value = "CTT_ND30";
                //ws.Cells["DR1"].Value = "CTT_NM";
                //ws.Cells["DS1"].Value = "DOR_ND10";
                //ws.Cells["DT1"].Value = "DOR_NM";
                //ws.Cells["DU1"].Value = "NET_ND30";
                //ws.Cells["DV1"].Value = "NET_NM";
                //ws.Cells["DW1"].Value = "QDA_ND15";
                //ws.Cells["DX1"].Value = "QDA_NM";
                //ws.Cells["DY1"].Value = "CPT_ND30";
                //ws.Cells["DZ1"].Value = "CPT_NM";
                //ws.Cells["EA1"].Value = "CPT_NE";
                //ws.Cells["EB1"].Value = "CZA_ND30";
                //ws.Cells["EC1"].Value = "CZA_NM";
                //ws.Cells["ED1"].Value = "CZA_NE";
                //ws.Cells["EE1"].Value = "AZA_ND30";
                //ws.Cells["EF1"].Value = "AZA_NM";
                //ws.Cells["EG1"].Value = "AZA_NE";
                //ws.Cells["EH1"].Value = "CZT_ND30";
                //ws.Cells["EI1"].Value = "CZT_NM";
                //ws.Cells["EJ1"].Value = "CZT_NE";
                ////ws.Cells["EK1"].Value = "TGC_NE";//新增字段
                //ws.Cells["EK1"].Value = "DOX_NM";//新增字段
                #endregion

                #region 设置表头
                //设置表头，因为自动设置表头会导致下划线丢失，所以手动设置
                ws.Cells["A1"].Value = "COUNTRY_A";
                ws.Cells["B1"].Value = "LABORATORY";
                ws.Cells["C1"].Value = "PATIENT_ID";
                ws.Cells["D1"].Value = "FIRST_NAME";
                ws.Cells["E1"].Value = "LAST_NAME";
                ws.Cells["F1"].Value = "SEX";
                ws.Cells["G1"].Value = "AGE";
                ws.Cells["H1"].Value = "DATE_BIRTH";
                ws.Cells["I1"].Value = "WARD";
                ws.Cells["J1"].Value = "WARD_TYPE";
                ws.Cells["K1"].Value = "INSTITUT";
                ws.Cells["L1"].Value = "DEPARTMENT";
                ws.Cells["M1"].Value = "SPEC_NUM";
                ws.Cells["N1"].Value = "SPEC_DATE";
                ws.Cells["O1"].Value = "SPEC_TYPE";
                ws.Cells["P1"].Value = "SPEC_CODE";
                ws.Cells["Q1"].Value = "ORGANISM";
                ws.Cells["R1"].Value = "ORG_TYPE";
                ws.Cells["S1"].Value = "ESBL";
                ws.Cells["T1"].Value = "BETA_LACT";
                ws.Cells["U1"].Value = "INDUC_CLI";
                ws.Cells["V1"].Value = "CARBAPENEM";
                ws.Cells["W1"].Value = "COMMENT";
                ws.Cells["X1"].Value = "AMC_ND20";
                ws.Cells["Y1"].Value = "AMC_NE"  ;
                ws.Cells["Z1"].Value = "AMC_NM"  ;
                ws.Cells["AA1"].Value = "AMK_ND30";
                ws.Cells["AB1"].Value = "AMK_NE" ;
                ws.Cells["AC1"].Value = "AMK_NM" ;
                ws.Cells["AD1"].Value = "AMP_ND10";
                ws.Cells["AE1"].Value = "AMP_NE" ;
                ws.Cells["AF1"].Value = "AMP_NM" ;
                ws.Cells["AG1"].Value = "ATM_ND30";
                ws.Cells["AH1"].Value = "ATM_NE" ;
                ws.Cells["AI1"].Value = "ATM_NM" ;
                ws.Cells["AJ1"].Value = "AZA_ND30";
                ws.Cells["AK1"].Value = "AZA_NE" ;
                ws.Cells["AL1"].Value = "AZA_NM" ;
                ws.Cells["AM1"].Value = "AZM_ND15";
                ws.Cells["AN1"].Value = "AZM_NE" ;
                ws.Cells["AO1"].Value = "AZM_NM" ;
                ws.Cells["AP1"].Value = "CAZ_ND30";
                ws.Cells["AQ1"].Value = "CAZ_NE" ;
                ws.Cells["AR1"].Value = "CAZ_NM" ;
                ws.Cells["AS1"].Value = "CEC_ND30";
                ws.Cells["AT1"].Value = "CEC_NE" ;
                ws.Cells["AU1"].Value = "CEC_NM" ;
                ws.Cells["AV1"].Value = "CFP_ND75";
                ws.Cells["AW1"].Value = "CFP_NE" ;
                ws.Cells["AX1"].Value = "CFP_NM" ;
                ws.Cells["AY1"].Value = "CHL_ND30";
                ws.Cells["AZ1"].Value = "CHL_NE" ;
                ws.Cells["BA1"].Value = "CHL_NM" ;
                ws.Cells["BB1"].Value = "CIP_ND5";
                ws.Cells["BC1"].Value = "CIP_NE" ;
                ws.Cells["BD1"].Value = "CIP_NM" ;
                ws.Cells["BE1"].Value = "CLI_ND2";
                ws.Cells["BF1"].Value = "CLI_NE" ;
                ws.Cells["BG1"].Value = "CLI_NM" ;
                ws.Cells["BH1"].Value = "COL_ND10";
                ws.Cells["BI1"].Value = "COL_NE" ;
                ws.Cells["BJ1"].Value = "COL_NM" ;
                ws.Cells["BK1"].Value = "CPT_ND30";
                ws.Cells["BL1"].Value = "CPT_NE" ;
                ws.Cells["BM1"].Value = "CPT_NM" ;
                ws.Cells["BN1"].Value = "CRO_ND30";
                ws.Cells["BO1"].Value = "CRO_NE" ;
                ws.Cells["BP1"].Value = "CRO_NM" ;
                ws.Cells["BQ1"].Value = "CSL_ND30";
                ws.Cells["BR1"].Value = "CSL_ND75";
                ws.Cells["BS1"].Value = "CSL_NM" ;
                ws.Cells["BT1"].Value = "CTT_ND30";
                ws.Cells["BU1"].Value = "CTT_NE" ;
                ws.Cells["BV1"].Value = "CTT_NM" ;
                ws.Cells["BW1"].Value = "CTX_ND30";
                ws.Cells["BX1"].Value = "CTX_NE" ;
                ws.Cells["BY1"].Value = "CTX_NM" ;
                ws.Cells["BZ1"].Value = "CXM_ND30";
                ws.Cells["CA1"].Value = "CXM_NE" ;
                ws.Cells["CB1"].Value = "CXM_NM" ;
                ws.Cells["CC1"].Value = "CZA_ND30";
                ws.Cells["CD1"].Value = "CZA_NE" ;
                ws.Cells["CE1"].Value = "CZA_NM" ;
                ws.Cells["CF1"].Value = "CZO_ND30";
                ws.Cells["CG1"].Value = "CZO_NE" ;
                ws.Cells["CH1"].Value = "CZO_NM" ;
                ws.Cells["CI1"].Value = "CZT_ND30";
                ws.Cells["CJ1"].Value = "CZT_NE" ;
                ws.Cells["CK1"].Value = "CZT_NM" ;
                ws.Cells["CL1"].Value = "DOR_ND10";
                ws.Cells["CM1"].Value = "DOR_NE" ;
                ws.Cells["CN1"].Value = "DOR_NM" ;
                ws.Cells["CO1"].Value = "DOX_ND30";
                ws.Cells["CP1"].Value = "DOX_NE" ;
                ws.Cells["CQ1"].Value = "DOX_NM" ;
                ws.Cells["CR1"].Value = "ERY_ND15";
                ws.Cells["CS1"].Value = "ERY_NE" ;
                ws.Cells["CT1"].Value = "ERY_NM" ;
                ws.Cells["CU1"].Value = "ETP_ND10";
                ws.Cells["CV1"].Value = "ETP_NE" ;
                ws.Cells["CW1"].Value = "ETP_NM" ;
                ws.Cells["CX1"].Value = "FEP_ND30";
                ws.Cells["CY1"].Value = "FEP_NE" ;
                ws.Cells["CZ1"].Value = "FEP_NM" ;
                ws.Cells["DA1"].Value = "FOS_ND20";
                ws.Cells["DB1"].Value = "FOS_NE" ;
                ws.Cells["DC1"].Value = "FOS_NM" ;
                ws.Cells["DD1"].Value = "FOX_ND30";
                ws.Cells["DE1"].Value = "FOX_NE" ;
                ws.Cells["DF1"].Value = "FOX_NM" ;
                ws.Cells["DG1"].Value = "GEH_ND12";
                ws.Cells["DH1"].Value = "GEH_NM" ;
                ws.Cells["DI1"].Value = "GEN_ND10";
                ws.Cells["DJ1"].Value = "GEN_NE" ;
                ws.Cells["DK1"].Value = "GEN_NM" ;
                ws.Cells["DL1"].Value = "IPM_ND10";
                ws.Cells["DM1"].Value = "IPM_NE" ;
                ws.Cells["DN1"].Value = "IPM_NM" ;
                ws.Cells["DO1"].Value = "LNZ_ND30";
                ws.Cells["DP1"].Value = "LNZ_NE" ;
                ws.Cells["DQ1"].Value = "LNZ_NM" ;
                ws.Cells["DR1"].Value = "LVX_ND5";
                ws.Cells["DS1"].Value = "LVX_NE" ;
                ws.Cells["DT1"].Value = "LVX_NM" ;
                ws.Cells["DU1"].Value = "MEM_ND10";
                ws.Cells["DV1"].Value = "MEM_NE" ;
                ws.Cells["DW1"].Value = "MEM_NM" ;
                ws.Cells["DX1"].Value = "MFX_ND5";
                ws.Cells["DY1"].Value = "MFX_NE" ;
                ws.Cells["DZ1"].Value = "MFX_NM" ;
                ws.Cells["EA1"].Value = "MNO_ND30";
                ws.Cells["EB1"].Value = "MNO_NE" ;
                ws.Cells["EC1"].Value = "MNO_NM" ;
                ws.Cells["ED1"].Value = "NET_ND30";
                ws.Cells["EE1"].Value = "NET_NE" ;
                ws.Cells["EF1"].Value = "NET_NM" ;
                ws.Cells["EG1"].Value = "NIT_ND30";
                ws.Cells["EH1"].Value = "NIT_NE" ;
                ws.Cells["EI1"].Value = "NIT_NM" ;
                ws.Cells["EJ1"].Value = "OFX_ND5";
                ws.Cells["EK1"].Value = "OXA_ND1";
                ws.Cells["EL1"].Value = "OXA_NE" ;
                ws.Cells["EM1"].Value = "OXA_NM" ;
                ws.Cells["EN1"].Value = "PEN_ND10";
                ws.Cells["EO1"].Value = "PEN_NE" ;
                ws.Cells["EP1"].Value = "PEN_NM" ;
                ws.Cells["EQ1"].Value = "PIP_ND10";
                ws.Cells["ER1"].Value = "PIP_NE" ;
                ws.Cells["ES1"].Value = "PIP_NM" ;
                ws.Cells["ET1"].Value = "POL_ND30";
                ws.Cells["EU1"].Value = "POL_NE" ;
                ws.Cells["EV1"].Value = "POL_NM" ;
                ws.Cells["EW1"].Value = "QDA_ND15";
                ws.Cells["EX1"].Value = "QDA_NE" ;
                ws.Cells["EY1"].Value = "QDA_NM" ;
                ws.Cells["EZ1"].Value = "RIF_ND5";
                ws.Cells["FA1"].Value = "RIF_NE" ;
                ws.Cells["FB1"].Value = "RIF_NM" ;
                ws.Cells["FC1"].Value = "SAM_ND10";
                ws.Cells["FD1"].Value = "SAM_NE" ;
                ws.Cells["FE1"].Value = "SAM_NM" ;
                ws.Cells["FF1"].Value = "STH_ND30";
                ws.Cells["FG1"].Value = "STH_NE" ;
                ws.Cells["FH1"].Value = "STH_NM" ;
                ws.Cells["FI1"].Value = "STR_ND10";
                ws.Cells["FJ1"].Value = "STR_NE" ;
                ws.Cells["FK1"].Value = "STR_NM" ;
                ws.Cells["FL1"].Value = "SXT_ND1_2";
                ws.Cells["FM1"].Value = "SXT_NE" ;
                ws.Cells["FN1"].Value = "SXT_NM" ;
                ws.Cells["FO1"].Value = "TCC_ND75";
                ws.Cells["FP1"].Value = "TCC_NE" ;
                ws.Cells["FQ1"].Value = "TCC_NM" ;
                ws.Cells["FR1"].Value = "TCY_ND30";
                ws.Cells["FS1"].Value = "TCY_NE" ;
                ws.Cells["FT1"].Value = "TCY_NM" ;
                ws.Cells["FU1"].Value = "TEC_ND30";
                ws.Cells["FV1"].Value = "TEC_NE" ;
                ws.Cells["FW1"].Value = "TEC_NM" ;
                ws.Cells["FX1"].Value = "TGC_ND15";
                ws.Cells["FY1"].Value = "TGC_NE" ;
                ws.Cells["FZ1"].Value = "TGC_NM" ;
                ws.Cells["GA1"].Value = "TIC_ND75";
                ws.Cells["GB1"].Value = "TIC_NE" ;
                ws.Cells["GC1"].Value = "TIC_NM" ;
                ws.Cells["GD1"].Value = "TOB_ND10";
                ws.Cells["GE1"].Value = "TOB_NE" ;
                ws.Cells["GF1"].Value = "TOB_NM" ;
                ws.Cells["GG1"].Value = "TZP_ND10";
                ws.Cells["GH1"].Value = "TZP_NE" ;
                ws.Cells["GI1"].Value = "TZP_NM" ;
                ws.Cells["GJ1"].Value = "VAN_ND30";
                ws.Cells["GK1"].Value = "VAN_NE" ;
                ws.Cells["GL1"].Value = "VAN_NM";

                //2025.04.23 新增：依拉环素
                ws.Cells["GM1"].Value = "ERV_NM";
                ws.Cells["GN1"].Value = "ERV_ND20";
                ws.Cells["GO1"].Value = "ERV_NE";

                #endregion
                ws.Cells["A2"].LoadFromCollection(data, false);

                //生成路径和保存文件
                string path = this.GetNewFilePath(entity, hospitalTeam);
                string mapPath = System.Web.HttpContext.Current.Server.MapPath(path);
                if (mapPath.ToUpper().StartsWith("C:"))
                {
                    mapPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                }

                //if (mapPath.ToUpper().StartsWith("C:"))
                //{
                //    Log4Helper.Info(this.GetType(), $"ManageSystem.Services.Medicine.MedicalDataItemService.GetExcel(MedicalData entity, Hospital hospital)生成容错文件方法中文件路径不正确\r\nMapPath = {mapPath}");
                //    return null;
                //}

                FileInfo file = new FileInfo(mapPath);
                ep.SaveAs(file);

                return path;
            }
            catch (Exception ex)
            {
                Log4Helper.Error(this.GetType(), ex);
                return null;
            }
        }

        /// <summary>
        /// 根据医学数据的Id，生产DBF文件数据，成功返回文件的相对路径
        /// </summary>
        /// <param name="entity">医学数据</param>
        /// <param name="hospital">所属医院</param>
        /// <returns></returns>
        public string GetDBF(MedicalData entity, HospitalTeamList hospitalTeam)
        {
            try
            {
                Log4Helper.Error($"ManageSystem.Services.Medicine.GetDBF({entity.Id})");

                #region 数据源
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@MedicalDataId", entity.Id);
                var data = DapperHelper.GetConnection().Query(@"SELECT T.* FROM (
SELECT ROW_NUMBER() OVER(PARTITION BY UploadRowIndex ORDER BY InsertTime ASC) AS UploadGroupIdx, * 
FROM dbo.MedicalDataItem
WHERE MedicalDataId = @MedicalDataId AND IsValid = 1 AND Mark > 0 
) AS T
WHERE T.UploadGroupIdx = 1
ORDER BY Sort", parameters,null,false,60,null).Select(x =>
               {
                   return new
                   {
                       MedicalDataItemId = x.Id,
                       COUNTRY_A = x.COUNTRY_A,
                       LABORATORY = x.LABORATORY,
                       PATIENT_ID = x.PATIENT_ID,
                       FIRST_NAME = x.FIRST_NAME,
                       LAST_NAME = x.LAST_NAME,
                       SEX = x.SEX,
                       AGE = x.AGE,
                       DATE_BIRTH = x.DATE_BIRTH.Contains("1900") ? "" : x.DATE_BIRTH,
                       WARD = x.WARD,
                       WARD_TYPE = x.WARD_TYPE,
                       INSTITUT = x.INSTITUT,
                       DEPARTMENT = x.DEPARTMENT,
                       SPEC_NUM = x.SPEC_NUM,
                       SPEC_DATE = x.SPEC_DATE,
                       SPEC_TYPE = x.SPEC_TYPE,
                       SPEC_CODE = x.SPEC_CODE,
                       ORGANISM = x.ORGANISM,
                       ORG_TYPE = x.ORG_TYPE,
                       ESBL = x.ESBL,
                       BETA_LACT = x.BETA_LACT,
                       INDUC_CLI = x.INDUC_CLI,
                       CARBAPENEM = x.CARBAPENEM,
                       COMMENT = x.COMMENT,
                       AMC_ND20 = x.AMC_ND20,
                       AMC_NE = x.AMC_NE,
                       AMC_NM = x.AMC_NM,
                       AMK_ND30 = x.AMK_ND30,
                       AMK_NE = x.AMK_NE,
                       AMK_NM = x.AMK_NM,
                       AMP_ND10 = x.AMP_ND10,
                       AMP_NE = x.AMP_NE,
                       AMP_NM = x.AMP_NM,
                       ATM_ND30 = x.ATM_ND30,
                       ATM_NE = x.ATM_NE,
                       ATM_NM = x.ATM_NM,
                       AZA_ND30 = x.AZA_ND30,
                       AZA_NE = x.AZA_NE,
                       AZA_NM = x.AZA_NM,
                       AZM_ND15 = x.AZM_ND15,
                       AZM_NE = x.AZM_NE,
                       AZM_NM = x.AZM_NM,
                       CAZ_ND30 = x.CAZ_ND30,
                       CAZ_NE = x.CAZ_NE,
                       CAZ_NM = x.CAZ_NM,
                       CEC_ND30 = x.CEC_ND30,
                       CEC_NE = x.CEC_NE,
                       CEC_NM = x.CEC_NM,
                       CFP_ND75 = x.CFP_ND75,
                       CFP_NE = x.CFP_NE,
                       CFP_NM = x.CFP_NM,
                       CHL_ND30 = x.CHL_ND30,
                       CHL_NE = x.CHL_NE,
                       CHL_NM = x.CHL_NM,
                       CIP_ND5 = x.CIP_ND5,
                       CIP_NE = x.CIP_NE,
                       CIP_NM = x.CIP_NM,
                       CLI_ND2 = x.CLI_ND2,
                       CLI_NE = x.CLI_NE,
                       CLI_NM = x.CLI_NM,
                       COL_ND10 = x.COL_ND10,
                       COL_NE = x.COL_NE,
                       COL_NM = x.COL_NM,
                       CPT_ND30 = x.CPT_ND30,
                       CPT_NE = x.CPT_NE,
                       CPT_NM = x.CPT_NM,
                       CRO_ND30 = x.CRO_ND30,
                       CRO_NE = x.CRO_NE,
                       CRO_NM = x.CRO_NM,
                       CSL_ND30 = x.CSL_ND30,
                       CSL_ND75 = x.CSL_ND75,
                       CSL_NM = x.CSL_NM,
                       CTT_ND30 = x.CTT_ND30,
                       CTT_NE = x.CTT_NE,
                       CTT_NM = x.CTT_NM,
                       CTX_ND30 = x.CTX_ND30,
                       CTX_NE = x.CTX_NE,
                       CTX_NM = x.CTX_NM,
                       CXM_ND30 = x.CXM_ND30,
                       CXM_NE = x.CXM_NE,
                       CXM_NM = x.CXM_NM,
                       CZA_ND30 = x.CZA_ND30,
                       CZA_NE = x.CZA_NE,
                       CZA_NM = x.CZA_NM,
                       CZO_ND30 = x.CZO_ND30,
                       CZO_NE = x.CZO_NE,
                       CZO_NM = x.CZO_NM,
                       CZT_ND30 = x.CZT_ND30,
                       CZT_NE = x.CZT_NE,
                       CZT_NM = x.CZT_NM,
                       DOR_ND10 = x.DOR_ND10,
                       DOR_NE = x.DOR_NE,
                       DOR_NM = x.DOR_NM,
                       DOX_ND30 = x.DOX_ND30,
                       DOX_NE = x.DOX_NE,
                       DOX_NM = x.DOX_NM,
                       ERY_ND15 = x.ERY_ND15,
                       ERY_NE = x.ERY_NE,
                       ERY_NM = x.ERY_NM,
                       ETP_ND10 = x.ETP_ND10,
                       ETP_NE = x.ETP_NE,
                       ETP_NM = x.ETP_NM,
                       FEP_ND30 = x.FEP_ND30,
                       FEP_NE = x.FEP_NE,
                       FEP_NM = x.FEP_NM,
                       FOS_ND200 = x.FOS_ND200,
                       FOS_NE = x.FOS_NE,
                       FOS_NM = x.FOS_NM,
                       FOX_ND30 = x.FOX_ND30,
                       FOX_NE = x.FOX_NE,
                       FOX_NM = x.FOX_NM,
                       GEH_ND120 = x.GEH_ND120,
                       GEH_NM = x.GEH_NM,
                       GEN_ND10 = x.GEN_ND10,
                       GEN_NE = x.GEN_NE,
                       GEN_NM = x.GEN_NM,
                       IPM_ND10 = x.IPM_ND10,
                       IPM_NE = x.IPM_NE,
                       IPM_NM = x.IPM_NM,
                       LNZ_ND30 = x.LNZ_ND30,
                       LNZ_NE = x.LNZ_NE,
                       LNZ_NM = x.LNZ_NM,
                       LVX_ND5 = x.LVX_ND5,
                       LVX_NE = x.LVX_NE,
                       LVX_NM = x.LVX_NM,
                       MEM_ND10 = x.MEM_ND10,
                       MEM_NE = x.MEM_NE,
                       MEM_NM = x.MEM_NM,
                       MFX_ND5 = x.MFX_ND5,
                       MFX_NE = x.MFX_NE,
                       MFX_NM = x.MFX_NM,
                       MNO_ND30 = x.MNO_ND30,
                       MNO_NE = x.MNO_NE,
                       MNO_NM = x.MNO_NM,
                       NET_ND30 = x.NET_ND30,
                       NET_NE = x.NET_NE,
                       NET_NM = x.NET_NM,
                       NIT_ND300 = x.NIT_ND300,
                       NIT_NE = x.NIT_NE,
                       NIT_NM = x.NIT_NM,
                       OFX_ND5 = x.OFX_ND5,
                       OXA_ND1 = x.OXA_ND1,
                       OXA_NE = x.OXA_NE,
                       OXA_NM = x.OXA_NM,
                       PEN_ND10 = x.PEN_ND10,
                       PEN_NE = x.PEN_NE,
                       PEN_NM = x.PEN_NM,
                       PIP_ND100 = x.PIP_ND100,
                       PIP_NE = x.PIP_NE,
                       PIP_NM = x.PIP_NM,
                       POL_ND300 = x.POL_ND300,
                       POL_NE = x.POL_NE,
                       POL_NM = x.POL_NM,
                       QDA_ND15 = x.QDA_ND15,
                       QDA_NE = x.QDA_NE,
                       QDA_NM = x.QDA_NM,
                       RIF_ND5 = x.RIF_ND5,
                       RIF_NE = x.RIF_NE,
                       RIF_NM = x.RIF_NM,
                       SAM_ND10 = x.SAM_ND10,
                       SAM_NE = x.SAM_NE,
                       SAM_NM = x.SAM_NM,
                       STH_ND300 = x.STH_ND300,
                       STH_NE = x.STH_NE,
                       STH_NM = x.STH_NM,
                       STR_ND10 = x.STR_ND10,
                       STR_NE = x.STR_NE,
                       STR_NM = x.STR_NM,
                       SXT_ND1_2 = x.SXT_ND1_2,
                       SXT_NE = x.SXT_NE,
                       SXT_NM = x.SXT_NM,
                       TCC_ND75 = x.TCC_ND75,
                       TCC_NE = x.TCC_NE,
                       TCC_NM = x.TCC_NM,
                       TCY_ND30 = x.TCY_ND30,
                       TCY_NE = x.TCY_NE,
                       TCY_NM = x.TCY_NM,
                       TEC_ND30 = x.TEC_ND30,
                       TEC_NE = x.TEC_NE,
                       TEC_NM = x.TEC_NM,
                       TGC_ND15 = x.TGC_ND15,
                       TGC_NE = x.TGC_NE,
                       TGC_NM = x.TGC_NM,
                       TIC_ND75 = x.TIC_ND75,
                       TIC_NE = x.TIC_NE,
                       TIC_NM = x.TIC_NM,
                       TOB_ND10 = x.TOB_ND10,
                       TOB_NE = x.TOB_NE,
                       TOB_NM = x.TOB_NM,
                       TZP_ND100 = x.TZP_ND100,
                       TZP_NE = x.TZP_NE,
                       TZP_NM = x.TZP_NM,
                       VAN_ND30 = x.VAN_ND30,
                       VAN_NE = x.VAN_NE,
                       VAN_NM = x.VAN_NM,
                       ERV_NM=x.ERV_NM,//2025.04.23 Gerry 新增：易拉环素
                       ERV_ND20=x.ERV_ND20,
                       ERV_NE=x.ERV_NE,
                   };
               });

                #region 弃用
                //AMK_ND30 = x.AMK_ND30,
                //AMC_ND20 = x.AMC_ND20,
                //AZM_ND15 = x.AZM_ND15,
                //AMP_ND10 = x.AMP_ND10,
                //SAM_ND10 = x.SAM_ND10,
                //ATM_ND30 = x.ATM_ND30,
                //OXA_ND1 = x.OXA_ND1,
                //POL_ND300 = x.POL_ND300,
                //NIT_ND300 = x.NIT_ND300,
                //SXT_ND1_2 = x.SXT_ND1_2,
                //STH_ND300 = x.STH_ND300,
                //GEH_ND120 = x.GEH_ND120,
                //ERY_ND15 = x.ERY_ND15,
                //CIP_ND5 = x.CIP_ND5,
                //CLI_ND2 = x.CLI_ND2,
                //RIF_ND5 = x.RIF_ND5,
                //LNZ_ND30 = x.LNZ_ND30,
                //STR_ND10 = x.STR_ND10,
                //FOS_ND200 = x.FOS_ND200,
                //CHL_ND30 = x.CHL_ND30,
                //MEM_ND10 = x.MEM_ND10,
                //MNO_ND30 = x.MNO_ND30,
                //MFX_ND5 = x.MFX_ND5,
                //PIP_ND100 = x.PIP_ND100,
                //TZP_ND100 = x.TZP_ND100,
                //PEN_ND10 = x.PEN_ND10,
                //GEN_ND10 = x.GEN_ND10,
                //TCY_ND30 = x.TCY_ND30,
                //TCC_ND75 = x.TCC_ND75,
                //TIC_ND75 = x.TIC_ND75,
                //TEC_ND30 = x.TEC_ND30,
                //TGC_ND15 = x.TGC_ND15,
                //FEP_ND30 = x.FEP_ND30,
                //CXM_ND30 = x.CXM_ND30,
                //CEC_ND30 = x.CEC_ND30,
                //CFP_ND75 = x.CFP_ND75,
                //CSL_ND30 = x.CSL_ND30,
                //CRO_ND30 = x.CRO_ND30,
                //CTX_ND30 = x.CTX_ND30,
                //CAZ_ND30 = x.CAZ_ND30,
                //FOX_ND30 = x.FOX_ND30,
                //CZO_ND30 = x.CZO_ND30,
                //TOB_ND10 = x.TOB_ND10,
                //VAN_ND30 = x.VAN_ND30,
                //IPM_ND10 = x.IPM_ND10,
                //LVX_ND5 = x.LVX_ND5,
                //OFX_ND5 = x.OFX_ND5,
                //DOX_ND30 = x.DOX_ND30,
                //AMK_NM = x.AMK_NM,
                //AMC_NM = x.AMC_NM,
                //AZM_NM = x.AZM_NM,
                //AMP_NM = x.AMP_NM,
                //SAM_NM = x.SAM_NM,
                //ATM_NM = x.ATM_NM,
                //OXA_NM = x.OXA_NM,
                //POL_NM = x.POL_NM,
                //NIT_NM = x.NIT_NM,
                //SXT_NM = x.SXT_NM,
                //STH_NM = x.STH_NM,
                //GEH_NM = x.GEH_NM,
                //ERY_NM = x.ERY_NM,
                //CIP_NM = x.CIP_NM,
                //CLI_NM = x.CLI_NM,
                //RIF_NM = x.RIF_NM,
                //LNZ_NM = x.LNZ_NM,
                //STR_NM = x.STR_NM,
                //FOS_NM = x.FOS_NM,
                //CHL_NM = x.CHL_NM,
                //MEM_NM = x.MEM_NM,
                //MNO_NM = x.MNO_NM,
                //MFX_NM = x.MFX_NM,
                //PIP_NM = x.PIP_NM,
                //TZP_NM = x.TZP_NM,
                //PEN_NM = x.PEN_NM,
                //GEN_NM = x.GEN_NM,
                //PEN_NE = x.PEN_NE,
                //TCY_NM = x.TCY_NM,
                //TCC_NM = x.TCC_NM,
                //TIC_NM = x.TIC_NM,
                //TEC_NM = x.TEC_NM,
                //TGC_NM = x.TGC_NM,
                //FEP_NM = x.FEP_NM,
                //CXM_NM = x.CXM_NM,
                //CEC_NM = x.CEC_NM,
                //CFP_NM = x.CFP_NM,
                //CSL_NM = x.CSL_NM,
                //CRO_NM = x.CRO_NM,
                //CTX_NM = x.CTX_NM,
                //CAZ_NM = x.CAZ_NM,
                //FOX_NM = x.FOX_NM,
                //CZO_NM = x.CZO_NM,
                //TOB_NM = x.TOB_NM,
                //VAN_NM = x.VAN_NM,
                //VAN_NE = x.VAN_NE,
                //IPM_NM = x.IPM_NM,
                //LVX_NM = x.LVX_NM,
                //CTX_NE = x.CTX_NE,
                //CSL_ND75 = x.CSL_ND75,
                //ETP_ND10 = x.ETP_ND10,
                //ETP_NM = x.ETP_NM,
                //CTT_ND30 = x.CTT_ND30,
                //CTT_NM = x.CTT_NM,
                //DOR_ND10 = x.DOR_ND10,
                //DOR_NM = x.DOR_NM,
                //NET_ND30 = x.NET_ND30,
                //NET_NM = x.NET_NM,
                //QDA_ND15 = x.QDA_ND15,
                //QDA_NM = x.QDA_NM,
                //CPT_ND30 = x.CPT_ND30,
                //CPT_NM = x.CPT_NM,
                //CPT_NE = x.CPT_NE,
                //CZA_ND30 = x.CZA_ND30,
                //CZA_NM = x.CZA_NM,
                //CZA_NE = x.CZA_NE,
                //AZA_ND30 = x.AZA_ND30,
                //AZA_NM = x.AZA_NM,
                //AZA_NE = x.AZA_NE,
                //CZT_ND30 = x.CZT_ND30,
                //CZT_NM = x.CZT_NM,
                //CZT_NE = x.CZT_NE,
                ////TGC_NE = x.TGC_NE,//新增字段
                //DOX_NM = x.DOX_NM //新增字段
                #endregion

                #endregion

                #region // 容错文件路径

                string path = this.GetNewFilePath(entity, hospitalTeam);

                string mapPath = System.Web.HttpContext.Current.Server.MapPath(path);
                if (mapPath.ToUpper().StartsWith("C:"))
                {
                    mapPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
                }

                //if (mapPath.ToUpper().StartsWith("C:"))
                //{
                //    Log4Helper.Info(this.GetType(), $"ManageSystem.Services.Medicine.MedicalDataItemService.GetDBF(MedicalData entity, Hospital hospital)生成容错文件方法中文件路径不正确\r\nMapPath = {mapPath}");
                //    return null;
                //}

                Log4Helper.Error($"{entity.Id}容错文件路径");

                #endregion

                System.Data.DataTable dataTable = new System.Data.DataTable();

                #region //表头

                dataTable.Columns.Add("COUNTRY_A");
                dataTable.Columns.Add("LABORATORY");
                dataTable.Columns.Add("PATIENT_ID");
                dataTable.Columns.Add("FIRST_NAME");
                dataTable.Columns.Add("LAST_NAME");
                dataTable.Columns.Add("SEX");
                dataTable.Columns.Add("AGE");
                dataTable.Columns.Add("DATE_BIRTH");
                dataTable.Columns.Add("WARD");
                dataTable.Columns.Add("WARD_TYPE");
                dataTable.Columns.Add("INSTITUT");
                dataTable.Columns.Add("DEPARTMENT");
                dataTable.Columns.Add("SPEC_NUM");
                dataTable.Columns.Add("SPEC_DATE");
                dataTable.Columns.Add("SPEC_TYPE");
                dataTable.Columns.Add("SPEC_CODE");
                dataTable.Columns.Add("ORGANISM");
                dataTable.Columns.Add("ORG_TYPE");
                dataTable.Columns.Add("ESBL");
                dataTable.Columns.Add("BETA_LACT");
                dataTable.Columns.Add("INDUC_CLI");
                dataTable.Columns.Add("CARBAPENEM");
                dataTable.Columns.Add("COMMENT");
                dataTable.Columns.Add("AMC_ND20");
                dataTable.Columns.Add("AMC_NE");
                dataTable.Columns.Add("AMC_NM");
                dataTable.Columns.Add("AMK_ND30");
                dataTable.Columns.Add("AMK_NE");
                dataTable.Columns.Add("AMK_NM");
                dataTable.Columns.Add("AMP_ND10");
                dataTable.Columns.Add("AMP_NE");
                dataTable.Columns.Add("AMP_NM");
                dataTable.Columns.Add("ATM_ND30");
                dataTable.Columns.Add("ATM_NE");
                dataTable.Columns.Add("ATM_NM");
                dataTable.Columns.Add("AZA_ND30");
                dataTable.Columns.Add("AZA_NE");
                dataTable.Columns.Add("AZA_NM");
                dataTable.Columns.Add("AZM_ND15");
                dataTable.Columns.Add("AZM_NE");
                dataTable.Columns.Add("AZM_NM");
                dataTable.Columns.Add("CAZ_ND30");
                dataTable.Columns.Add("CAZ_NE");
                dataTable.Columns.Add("CAZ_NM");
                dataTable.Columns.Add("CEC_ND30");
                dataTable.Columns.Add("CEC_NE");
                dataTable.Columns.Add("CEC_NM");
                dataTable.Columns.Add("CFP_ND75");
                dataTable.Columns.Add("CFP_NE");
                dataTable.Columns.Add("CFP_NM");
                dataTable.Columns.Add("CHL_ND30");
                dataTable.Columns.Add("CHL_NE");
                dataTable.Columns.Add("CHL_NM");
                dataTable.Columns.Add("CIP_ND5");
                dataTable.Columns.Add("CIP_NE");
                dataTable.Columns.Add("CIP_NM");
                dataTable.Columns.Add("CLI_ND2");
                dataTable.Columns.Add("CLI_NE");
                dataTable.Columns.Add("CLI_NM");
                dataTable.Columns.Add("COL_ND10");
                dataTable.Columns.Add("COL_NE");
                dataTable.Columns.Add("COL_NM");
                dataTable.Columns.Add("CPT_ND30");
                dataTable.Columns.Add("CPT_NE");
                dataTable.Columns.Add("CPT_NM");
                dataTable.Columns.Add("CRO_ND30");
                dataTable.Columns.Add("CRO_NE");
                dataTable.Columns.Add("CRO_NM");
                dataTable.Columns.Add("CSL_ND30");
                dataTable.Columns.Add("CSL_ND75");
                dataTable.Columns.Add("CSL_NM");
                dataTable.Columns.Add("CTT_ND30");
                dataTable.Columns.Add("CTT_NE");
                dataTable.Columns.Add("CTT_NM");
                dataTable.Columns.Add("CTX_ND30");
                dataTable.Columns.Add("CTX_NE");
                dataTable.Columns.Add("CTX_NM");
                dataTable.Columns.Add("CXM_ND30");
                dataTable.Columns.Add("CXM_NE");
                dataTable.Columns.Add("CXM_NM");
                dataTable.Columns.Add("CZA_ND30");
                dataTable.Columns.Add("CZA_NE");
                dataTable.Columns.Add("CZA_NM");
                dataTable.Columns.Add("CZO_ND30");
                dataTable.Columns.Add("CZO_NE");
                dataTable.Columns.Add("CZO_NM");
                dataTable.Columns.Add("CZT_ND30");
                dataTable.Columns.Add("CZT_NE");
                dataTable.Columns.Add("CZT_NM");
                dataTable.Columns.Add("DOR_ND10");
                dataTable.Columns.Add("DOR_NE");
                dataTable.Columns.Add("DOR_NM");
                dataTable.Columns.Add("DOX_ND30");
                dataTable.Columns.Add("DOX_NE");
                dataTable.Columns.Add("DOX_NM");
                dataTable.Columns.Add("ERY_ND15");
                dataTable.Columns.Add("ERY_NE");
                dataTable.Columns.Add("ERY_NM");
                dataTable.Columns.Add("ETP_ND10");
                dataTable.Columns.Add("ETP_NE");
                dataTable.Columns.Add("ETP_NM");
                dataTable.Columns.Add("FEP_ND30");
                dataTable.Columns.Add("FEP_NE");
                dataTable.Columns.Add("FEP_NM");
                dataTable.Columns.Add("FOS_ND200");
                dataTable.Columns.Add("FOS_NE");
                dataTable.Columns.Add("FOS_NM");
                dataTable.Columns.Add("FOX_ND30");
                dataTable.Columns.Add("FOX_NE");
                dataTable.Columns.Add("FOX_NM");
                dataTable.Columns.Add("GEH_ND120");
                dataTable.Columns.Add("GEH_NM");
                dataTable.Columns.Add("GEN_ND10");
                dataTable.Columns.Add("GEN_NE");
                dataTable.Columns.Add("GEN_NM");
                dataTable.Columns.Add("IPM_ND10");
                dataTable.Columns.Add("IPM_NE");
                dataTable.Columns.Add("IPM_NM");
                dataTable.Columns.Add("LNZ_ND30");
                dataTable.Columns.Add("LNZ_NE");
                dataTable.Columns.Add("LNZ_NM");
                dataTable.Columns.Add("LVX_ND5");
                dataTable.Columns.Add("LVX_NE");
                dataTable.Columns.Add("LVX_NM");
                dataTable.Columns.Add("MEM_ND10");
                dataTable.Columns.Add("MEM_NE");
                dataTable.Columns.Add("MEM_NM");
                dataTable.Columns.Add("MFX_ND5");
                dataTable.Columns.Add("MFX_NE");
                dataTable.Columns.Add("MFX_NM");
                dataTable.Columns.Add("MNO_ND30");
                dataTable.Columns.Add("MNO_NE");
                dataTable.Columns.Add("MNO_NM");
                dataTable.Columns.Add("NET_ND30");
                dataTable.Columns.Add("NET_NE");
                dataTable.Columns.Add("NET_NM");
                dataTable.Columns.Add("NIT_ND300");
                dataTable.Columns.Add("NIT_NE");
                dataTable.Columns.Add("NIT_NM");
                dataTable.Columns.Add("OFX_ND5");
                dataTable.Columns.Add("OXA_ND1");
                dataTable.Columns.Add("OXA_NE");
                dataTable.Columns.Add("OXA_NM");
                dataTable.Columns.Add("PEN_ND10");
                dataTable.Columns.Add("PEN_NE");
                dataTable.Columns.Add("PEN_NM");
                dataTable.Columns.Add("PIP_ND100");
                dataTable.Columns.Add("PIP_NE");
                dataTable.Columns.Add("PIP_NM");
                dataTable.Columns.Add("POL_ND300");
                dataTable.Columns.Add("POL_NE");
                dataTable.Columns.Add("POL_NM");
                dataTable.Columns.Add("QDA_ND15");
                dataTable.Columns.Add("QDA_NE");
                dataTable.Columns.Add("QDA_NM");
                dataTable.Columns.Add("RIF_ND5");
                dataTable.Columns.Add("RIF_NE");
                dataTable.Columns.Add("RIF_NM");
                dataTable.Columns.Add("SAM_ND10");
                dataTable.Columns.Add("SAM_NE");
                dataTable.Columns.Add("SAM_NM");
                dataTable.Columns.Add("STH_ND300");
                dataTable.Columns.Add("STH_NE");
                dataTable.Columns.Add("STH_NM");
                dataTable.Columns.Add("STR_ND10");
                dataTable.Columns.Add("STR_NE");
                dataTable.Columns.Add("STR_NM");
                dataTable.Columns.Add("SXT_ND1_2");
                dataTable.Columns.Add("SXT_NE");
                dataTable.Columns.Add("SXT_NM");
                dataTable.Columns.Add("TCC_ND75");
                dataTable.Columns.Add("TCC_NE");
                dataTable.Columns.Add("TCC_NM");
                dataTable.Columns.Add("TCY_ND30");
                dataTable.Columns.Add("TCY_NE");
                dataTable.Columns.Add("TCY_NM");
                dataTable.Columns.Add("TEC_ND30");
                dataTable.Columns.Add("TEC_NE");
                dataTable.Columns.Add("TEC_NM");
                dataTable.Columns.Add("TGC_ND15");
                dataTable.Columns.Add("TGC_NE");
                dataTable.Columns.Add("TGC_NM");
                dataTable.Columns.Add("TIC_ND75");
                dataTable.Columns.Add("TIC_NE");
                dataTable.Columns.Add("TIC_NM");
                dataTable.Columns.Add("TOB_ND10");
                dataTable.Columns.Add("TOB_NE");
                dataTable.Columns.Add("TOB_NM");
                dataTable.Columns.Add("TZP_ND100");
                dataTable.Columns.Add("TZP_NE");
                dataTable.Columns.Add("TZP_NM");
                dataTable.Columns.Add("VAN_ND30");
                dataTable.Columns.Add("VAN_NE");
                dataTable.Columns.Add("VAN_NM");

                //2025.04.23 Gerry 新增：依拉环素
                dataTable.Columns.Add("ERV_NM");
                dataTable.Columns.Add("ERV_ND20");
                dataTable.Columns.Add("ERV_NE");

                #endregion

                #region //内容

                foreach (var item in data)
                {
                    var dataRow = dataTable.NewRow();
                    dataRow["COUNTRY_A"] = !string.IsNullOrWhiteSpace(item.COUNTRY_A) ? item.COUNTRY_A : "";
                    dataRow["LABORATORY"] = !string.IsNullOrWhiteSpace(item.LABORATORY) ? item.LABORATORY : "";
                    dataRow["PATIENT_ID"] = !string.IsNullOrWhiteSpace(item.PATIENT_ID) ? item.PATIENT_ID : "";
                    dataRow["FIRST_NAME"] = !string.IsNullOrWhiteSpace(item.FIRST_NAME) ? item.FIRST_NAME : "";
                    dataRow["LAST_NAME"] = !string.IsNullOrWhiteSpace(item.LAST_NAME) ? item.LAST_NAME : "";
                    dataRow["SEX"] = !string.IsNullOrWhiteSpace(item.SEX) ? item.SEX : "";
                    dataRow["AGE"] = !string.IsNullOrWhiteSpace(item.AGE) ? item.AGE : "";
                    dataRow["DATE_BIRTH"] = !string.IsNullOrWhiteSpace(item.DATE_BIRTH) ? item.DATE_BIRTH : "";
                    dataRow["WARD"] = !string.IsNullOrWhiteSpace(item.WARD) ? item.WARD : "";
                    dataRow["WARD_TYPE"] = !string.IsNullOrWhiteSpace(item.WARD_TYPE) ? item.WARD_TYPE : "";
                    dataRow["INSTITUT"] = !string.IsNullOrWhiteSpace(item.INSTITUT) ? item.INSTITUT : "";
                    dataRow["DEPARTMENT"] = !string.IsNullOrWhiteSpace(item.DEPARTMENT) ? item.DEPARTMENT : "";
                    dataRow["SPEC_NUM"] = !string.IsNullOrWhiteSpace(item.SPEC_NUM) ? item.SPEC_NUM : "";
                    dataRow["SPEC_DATE"] = !string.IsNullOrWhiteSpace(item.SPEC_DATE) ? item.SPEC_DATE : "";
                    dataRow["SPEC_TYPE"] = !string.IsNullOrWhiteSpace(item.SPEC_TYPE) ? item.SPEC_TYPE : "";
                    dataRow["SPEC_CODE"] = !string.IsNullOrWhiteSpace(item.SPEC_CODE) ? item.SPEC_CODE : "";
                    dataRow["ORGANISM"] = !string.IsNullOrWhiteSpace(item.ORGANISM) ? item.ORGANISM : "";
                    dataRow["ORG_TYPE"] = !string.IsNullOrWhiteSpace(item.ORG_TYPE) ? item.ORG_TYPE : "";
                    dataRow["ESBL"] = !string.IsNullOrWhiteSpace(item.ESBL) ? item.ESBL : "";
                    dataRow["BETA_LACT"] = !string.IsNullOrWhiteSpace(item.BETA_LACT) ? item.BETA_LACT : "";
                    dataRow["INDUC_CLI"] = !string.IsNullOrWhiteSpace(item.INDUC_CLI) ? item.INDUC_CLI : "";
                    dataRow["CARBAPENEM"] = !string.IsNullOrWhiteSpace(item.CARBAPENEM) ? item.CARBAPENEM : "";
                    dataRow["COMMENT"] = !string.IsNullOrWhiteSpace(item.COMMENT) ? item.COMMENT : "";
                    dataRow["AMC_ND20"] = !string.IsNullOrWhiteSpace(item.AMC_ND20) ? item.AMC_ND20 : "";
                    dataRow["AMC_NE"] = !string.IsNullOrWhiteSpace(item.AMC_NE) ? item.AMC_NE : "";
                    dataRow["AMC_NM"] = !string.IsNullOrWhiteSpace(item.AMC_NM) ? item.AMC_NM : "";
                    dataRow["AMK_ND30"] = !string.IsNullOrWhiteSpace(item.AMK_ND30) ? item.AMK_ND30 : "";
                    dataRow["AMK_NE"] = !string.IsNullOrWhiteSpace(item.AMK_NE) ? item.AMK_NE : "";
                    dataRow["AMK_NM"] = !string.IsNullOrWhiteSpace(item.AMK_NM) ? item.AMK_NM : "";
                    dataRow["AMP_ND10"] = !string.IsNullOrWhiteSpace(item.AMP_ND10) ? item.AMP_ND10 : "";
                    dataRow["AMP_NE"] = !string.IsNullOrWhiteSpace(item.AMP_NE) ? item.AMP_NE : "";
                    dataRow["AMP_NM"] = !string.IsNullOrWhiteSpace(item.AMP_NM) ? item.AMP_NM : "";
                    dataRow["ATM_ND30"] = !string.IsNullOrWhiteSpace(item.ATM_ND30) ? item.ATM_ND30 : "";
                    dataRow["ATM_NE"] = !string.IsNullOrWhiteSpace(item.ATM_NE) ? item.ATM_NE : "";
                    dataRow["ATM_NM"] = !string.IsNullOrWhiteSpace(item.ATM_NM) ? item.ATM_NM : "";
                    dataRow["AZA_ND30"] = !string.IsNullOrWhiteSpace(item.AZA_ND30) ? item.AZA_ND30 : "";
                    dataRow["AZA_NE"] = !string.IsNullOrWhiteSpace(item.AZA_NE) ? item.AZA_NE : "";
                    dataRow["AZA_NM"] = !string.IsNullOrWhiteSpace(item.AZA_NM) ? item.AZA_NM : "";
                    dataRow["AZM_ND15"] = !string.IsNullOrWhiteSpace(item.AZM_ND15) ? item.AZM_ND15 : "";
                    dataRow["AZM_NE"] = !string.IsNullOrWhiteSpace(item.AZM_NE) ? item.AZM_NE : "";
                    dataRow["AZM_NM"] = !string.IsNullOrWhiteSpace(item.AZM_NM) ? item.AZM_NM : "";
                    dataRow["CAZ_ND30"] = !string.IsNullOrWhiteSpace(item.CAZ_ND30) ? item.CAZ_ND30 : "";
                    dataRow["CAZ_NE"] = !string.IsNullOrWhiteSpace(item.CAZ_NE) ? item.CAZ_NE : "";
                    dataRow["CAZ_NM"] = !string.IsNullOrWhiteSpace(item.CAZ_NM) ? item.CAZ_NM : "";
                    dataRow["CEC_ND30"] = !string.IsNullOrWhiteSpace(item.CEC_ND30) ? item.CEC_ND30 : "";
                    dataRow["CEC_NE"] = !string.IsNullOrWhiteSpace(item.CEC_NE) ? item.CEC_NE : "";
                    dataRow["CEC_NM"] = !string.IsNullOrWhiteSpace(item.CEC_NM) ? item.CEC_NM : "";
                    dataRow["CFP_ND75"] = !string.IsNullOrWhiteSpace(item.CFP_ND75) ? item.CFP_ND75 : "";
                    dataRow["CFP_NE"] = !string.IsNullOrWhiteSpace(item.CFP_NE) ? item.CFP_NE : "";
                    dataRow["CFP_NM"] = !string.IsNullOrWhiteSpace(item.CFP_NM) ? item.CFP_NM : "";
                    dataRow["CHL_ND30"] = !string.IsNullOrWhiteSpace(item.CHL_ND30) ? item.CHL_ND30 : "";
                    dataRow["CHL_NE"] = !string.IsNullOrWhiteSpace(item.CHL_NE) ? item.CHL_NE : "";
                    dataRow["CHL_NM"] = !string.IsNullOrWhiteSpace(item.CHL_NM) ? item.CHL_NM : "";
                    dataRow["CIP_ND5"] = !string.IsNullOrWhiteSpace(item.CIP_ND5) ? item.CIP_ND5 : "";
                    dataRow["CIP_NE"] = !string.IsNullOrWhiteSpace(item.CIP_NE) ? item.CIP_NE : "";
                    dataRow["CIP_NM"] = !string.IsNullOrWhiteSpace(item.CIP_NM) ? item.CIP_NM : "";
                    dataRow["CLI_ND2"] = !string.IsNullOrWhiteSpace(item.CLI_ND2) ? item.CLI_ND2 : "";
                    dataRow["CLI_NE"] = !string.IsNullOrWhiteSpace(item.CLI_NE) ? item.CLI_NE : "";
                    dataRow["CLI_NM"] = !string.IsNullOrWhiteSpace(item.CLI_NM) ? item.CLI_NM : "";
                    dataRow["COL_ND10"] = !string.IsNullOrWhiteSpace(item.COL_ND10) ? item.COL_ND10 : "";
                    dataRow["COL_NE"] = !string.IsNullOrWhiteSpace(item.COL_NE) ? item.COL_NE : "";
                    dataRow["COL_NM"] = !string.IsNullOrWhiteSpace(item.COL_NM) ? item.COL_NM : "";
                    dataRow["CPT_ND30"] = !string.IsNullOrWhiteSpace(item.CPT_ND30) ? item.CPT_ND30 : "";
                    dataRow["CPT_NE"] = !string.IsNullOrWhiteSpace(item.CPT_NE) ? item.CPT_NE : "";
                    dataRow["CPT_NM"] = !string.IsNullOrWhiteSpace(item.CPT_NM) ? item.CPT_NM : "";
                    dataRow["CRO_ND30"] = !string.IsNullOrWhiteSpace(item.CRO_ND30) ? item.CRO_ND30 : "";
                    dataRow["CRO_NE"] = !string.IsNullOrWhiteSpace(item.CRO_NE) ? item.CRO_NE : "";
                    dataRow["CRO_NM"] = !string.IsNullOrWhiteSpace(item.CRO_NM) ? item.CRO_NM : "";
                    dataRow["CSL_ND30"] = !string.IsNullOrWhiteSpace(item.CSL_ND30) ? item.CSL_ND30 : "";
                    dataRow["CSL_ND75"] = !string.IsNullOrWhiteSpace(item.CSL_ND75) ? item.CSL_ND75 : "";
                    dataRow["CSL_NM"] = !string.IsNullOrWhiteSpace(item.CSL_NM) ? item.CSL_NM : "";
                    dataRow["CTT_ND30"] = !string.IsNullOrWhiteSpace(item.CTT_ND30) ? item.CTT_ND30 : "";
                    dataRow["CTT_NE"] = !string.IsNullOrWhiteSpace(item.CTT_NE) ? item.CTT_NE : "";
                    dataRow["CTT_NM"] = !string.IsNullOrWhiteSpace(item.CTT_NM) ? item.CTT_NM : "";
                    dataRow["CTX_ND30"] = !string.IsNullOrWhiteSpace(item.CTX_ND30) ? item.CTX_ND30 : "";
                    dataRow["CTX_NE"] = !string.IsNullOrWhiteSpace(item.CTX_NE) ? item.CTX_NE : "";
                    dataRow["CTX_NM"] = !string.IsNullOrWhiteSpace(item.CTX_NM) ? item.CTX_NM : "";
                    dataRow["CXM_ND30"] = !string.IsNullOrWhiteSpace(item.CXM_ND30) ? item.CXM_ND30 : "";
                    dataRow["CXM_NE"] = !string.IsNullOrWhiteSpace(item.CXM_NE) ? item.CXM_NE : "";
                    dataRow["CXM_NM"] = !string.IsNullOrWhiteSpace(item.CXM_NM) ? item.CXM_NM : "";
                    dataRow["CZA_ND30"] = !string.IsNullOrWhiteSpace(item.CZA_ND30) ? item.CZA_ND30 : "";
                    dataRow["CZA_NE"] = !string.IsNullOrWhiteSpace(item.CZA_NE) ? item.CZA_NE : "";
                    dataRow["CZA_NM"] = !string.IsNullOrWhiteSpace(item.CZA_NM) ? item.CZA_NM : "";
                    dataRow["CZO_ND30"] = !string.IsNullOrWhiteSpace(item.CZO_ND30) ? item.CZO_ND30 : "";
                    dataRow["CZO_NE"] = !string.IsNullOrWhiteSpace(item.CZO_NE) ? item.CZO_NE : "";
                    dataRow["CZO_NM"] = !string.IsNullOrWhiteSpace(item.CZO_NM) ? item.CZO_NM : "";
                    dataRow["CZT_ND30"] = !string.IsNullOrWhiteSpace(item.CZT_ND30) ? item.CZT_ND30 : "";
                    dataRow["CZT_NE"] = !string.IsNullOrWhiteSpace(item.CZT_NE) ? item.CZT_NE : "";
                    dataRow["CZT_NM"] = !string.IsNullOrWhiteSpace(item.CZT_NM) ? item.CZT_NM : "";
                    dataRow["DOR_ND10"] = !string.IsNullOrWhiteSpace(item.DOR_ND10) ? item.DOR_ND10 : "";
                    dataRow["DOR_NE"] = !string.IsNullOrWhiteSpace(item.DOR_NE) ? item.DOR_NE : "";
                    dataRow["DOR_NM"] = !string.IsNullOrWhiteSpace(item.DOR_NM) ? item.DOR_NM : "";
                    dataRow["DOX_ND30"] = !string.IsNullOrWhiteSpace(item.DOX_ND30) ? item.DOX_ND30 : "";
                    dataRow["DOX_NE"] = !string.IsNullOrWhiteSpace(item.DOX_NE) ? item.DOX_NE : "";
                    dataRow["DOX_NM"] = !string.IsNullOrWhiteSpace(item.DOX_NM) ? item.DOX_NM : "";
                    dataRow["ERY_ND15"] = !string.IsNullOrWhiteSpace(item.ERY_ND15) ? item.ERY_ND15 : "";
                    dataRow["ERY_NE"] = !string.IsNullOrWhiteSpace(item.ERY_NE) ? item.ERY_NE : "";
                    dataRow["ERY_NM"] = !string.IsNullOrWhiteSpace(item.ERY_NM) ? item.ERY_NM : "";
                    dataRow["ETP_ND10"] = !string.IsNullOrWhiteSpace(item.ETP_ND10) ? item.ETP_ND10 : "";
                    dataRow["ETP_NE"] = !string.IsNullOrWhiteSpace(item.ETP_NE) ? item.ETP_NE : "";
                    dataRow["ETP_NM"] = !string.IsNullOrWhiteSpace(item.ETP_NM) ? item.ETP_NM : "";
                    dataRow["FEP_ND30"] = !string.IsNullOrWhiteSpace(item.FEP_ND30) ? item.FEP_ND30 : "";
                    dataRow["FEP_NE"] = !string.IsNullOrWhiteSpace(item.FEP_NE) ? item.FEP_NE : "";
                    dataRow["FEP_NM"] = !string.IsNullOrWhiteSpace(item.FEP_NM) ? item.FEP_NM : "";
                    dataRow["FOS_ND200"] = !string.IsNullOrWhiteSpace(item.FOS_ND200) ? item.FOS_ND200 : "";
                    dataRow["FOS_NE"] = !string.IsNullOrWhiteSpace(item.FOS_NE) ? item.FOS_NE : "";
                    dataRow["FOS_NM"] = !string.IsNullOrWhiteSpace(item.FOS_NM) ? item.FOS_NM : "";
                    dataRow["FOX_ND30"] = !string.IsNullOrWhiteSpace(item.FOX_ND30) ? item.FOX_ND30 : "";
                    dataRow["FOX_NE"] = !string.IsNullOrWhiteSpace(item.FOX_NE) ? item.FOX_NE : "";
                    dataRow["FOX_NM"] = !string.IsNullOrWhiteSpace(item.FOX_NM) ? item.FOX_NM : "";
                    dataRow["GEH_ND120"] = !string.IsNullOrWhiteSpace(item.GEH_ND120) ? item.GEH_ND120 : "";
                    dataRow["GEH_NM"] = !string.IsNullOrWhiteSpace(item.GEH_NM) ? item.GEH_NM : "";
                    dataRow["GEN_ND10"] = !string.IsNullOrWhiteSpace(item.GEN_ND10) ? item.GEN_ND10 : "";
                    dataRow["GEN_NE"] = !string.IsNullOrWhiteSpace(item.GEN_NE) ? item.GEN_NE : "";
                    dataRow["GEN_NM"] = !string.IsNullOrWhiteSpace(item.GEN_NM) ? item.GEN_NM : "";
                    dataRow["IPM_ND10"] = !string.IsNullOrWhiteSpace(item.IPM_ND10) ? item.IPM_ND10 : "";
                    dataRow["IPM_NE"] = !string.IsNullOrWhiteSpace(item.IPM_NE) ? item.IPM_NE : "";
                    dataRow["IPM_NM"] = !string.IsNullOrWhiteSpace(item.IPM_NM) ? item.IPM_NM : "";
                    dataRow["LNZ_ND30"] = !string.IsNullOrWhiteSpace(item.LNZ_ND30) ? item.LNZ_ND30 : "";
                    dataRow["LNZ_NE"] = !string.IsNullOrWhiteSpace(item.LNZ_NE) ? item.LNZ_NE : "";
                    dataRow["LNZ_NM"] = !string.IsNullOrWhiteSpace(item.LNZ_NM) ? item.LNZ_NM : "";
                    dataRow["LVX_ND5"] = !string.IsNullOrWhiteSpace(item.LVX_ND5) ? item.LVX_ND5 : "";
                    dataRow["LVX_NE"] = !string.IsNullOrWhiteSpace(item.LVX_NE) ? item.LVX_NE : "";
                    dataRow["LVX_NM"] = !string.IsNullOrWhiteSpace(item.LVX_NM) ? item.LVX_NM : "";
                    dataRow["MEM_ND10"] = !string.IsNullOrWhiteSpace(item.MEM_ND10) ? item.MEM_ND10 : "";
                    dataRow["MEM_NE"] = !string.IsNullOrWhiteSpace(item.MEM_NE) ? item.MEM_NE : "";
                    dataRow["MEM_NM"] = !string.IsNullOrWhiteSpace(item.MEM_NM) ? item.MEM_NM : "";
                    dataRow["MFX_ND5"] = !string.IsNullOrWhiteSpace(item.MFX_ND5) ? item.MFX_ND5 : "";
                    dataRow["MFX_NE"] = !string.IsNullOrWhiteSpace(item.MFX_NE) ? item.MFX_NE : "";
                    dataRow["MFX_NM"] = !string.IsNullOrWhiteSpace(item.MFX_NM) ? item.MFX_NM : "";
                    dataRow["MNO_ND30"] = !string.IsNullOrWhiteSpace(item.MNO_ND30) ? item.MNO_ND30 : "";
                    dataRow["MNO_NE"] = !string.IsNullOrWhiteSpace(item.MNO_NE) ? item.MNO_NE : "";
                    dataRow["MNO_NM"] = !string.IsNullOrWhiteSpace(item.MNO_NM) ? item.MNO_NM : "";
                    dataRow["NET_ND30"] = !string.IsNullOrWhiteSpace(item.NET_ND30) ? item.NET_ND30 : "";
                    dataRow["NET_NE"] = !string.IsNullOrWhiteSpace(item.NET_NE) ? item.NET_NE : "";
                    dataRow["NET_NM"] = !string.IsNullOrWhiteSpace(item.NET_NM) ? item.NET_NM : "";
                    dataRow["NIT_ND300"] = !string.IsNullOrWhiteSpace(item.NIT_ND300) ? item.NIT_ND300 : "";
                    dataRow["NIT_NE"] = !string.IsNullOrWhiteSpace(item.NIT_NE) ? item.NIT_NE : "";
                    dataRow["NIT_NM"] = !string.IsNullOrWhiteSpace(item.NIT_NM) ? item.NIT_NM : "";
                    dataRow["OFX_ND5"] = !string.IsNullOrWhiteSpace(item.OFX_ND5) ? item.OFX_ND5 : "";
                    dataRow["OXA_ND1"] = !string.IsNullOrWhiteSpace(item.OXA_ND1) ? item.OXA_ND1 : "";
                    dataRow["OXA_NE"] = !string.IsNullOrWhiteSpace(item.OXA_NE) ? item.OXA_NE : "";
                    dataRow["OXA_NM"] = !string.IsNullOrWhiteSpace(item.OXA_NM) ? item.OXA_NM : "";
                    dataRow["PEN_ND10"] = !string.IsNullOrWhiteSpace(item.PEN_ND10) ? item.PEN_ND10 : "";
                    dataRow["PEN_NE"] = !string.IsNullOrWhiteSpace(item.PEN_NE) ? item.PEN_NE : "";
                    dataRow["PEN_NM"] = !string.IsNullOrWhiteSpace(item.PEN_NM) ? item.PEN_NM : "";
                    dataRow["PIP_ND100"] = !string.IsNullOrWhiteSpace(item.PIP_ND100) ? item.PIP_ND100 : "";
                    dataRow["PIP_NE"] = !string.IsNullOrWhiteSpace(item.PIP_NE) ? item.PIP_NE : "";
                    dataRow["PIP_NM"] = !string.IsNullOrWhiteSpace(item.PIP_NM) ? item.PIP_NM : "";
                    dataRow["POL_ND300"] = !string.IsNullOrWhiteSpace(item.POL_ND300) ? item.POL_ND300 : "";
                    dataRow["POL_NE"] = !string.IsNullOrWhiteSpace(item.POL_NE) ? item.POL_NE : "";
                    dataRow["POL_NM"] = !string.IsNullOrWhiteSpace(item.POL_NM) ? item.POL_NM : "";
                    dataRow["QDA_ND15"] = !string.IsNullOrWhiteSpace(item.QDA_ND15) ? item.QDA_ND15 : "";
                    dataRow["QDA_NE"] = !string.IsNullOrWhiteSpace(item.QDA_NE) ? item.QDA_NE : "";
                    dataRow["QDA_NM"] = !string.IsNullOrWhiteSpace(item.QDA_NM) ? item.QDA_NM : "";
                    dataRow["RIF_ND5"] = !string.IsNullOrWhiteSpace(item.RIF_ND5) ? item.RIF_ND5 : "";
                    dataRow["RIF_NE"] = !string.IsNullOrWhiteSpace(item.RIF_NE) ? item.RIF_NE : "";
                    dataRow["RIF_NM"] = !string.IsNullOrWhiteSpace(item.RIF_NM) ? item.RIF_NM : "";
                    dataRow["SAM_ND10"] = !string.IsNullOrWhiteSpace(item.SAM_ND10) ? item.SAM_ND10 : "";
                    dataRow["SAM_NE"] = !string.IsNullOrWhiteSpace(item.SAM_NE) ? item.SAM_NE : "";
                    dataRow["SAM_NM"] = !string.IsNullOrWhiteSpace(item.SAM_NM) ? item.SAM_NM : "";
                    dataRow["STH_ND300"] = !string.IsNullOrWhiteSpace(item.STH_ND300) ? item.STH_ND300 : "";
                    dataRow["STH_NE"] = !string.IsNullOrWhiteSpace(item.STH_NE) ? item.STH_NE : "";
                    dataRow["STH_NM"] = !string.IsNullOrWhiteSpace(item.STH_NM) ? item.STH_NM : "";
                    dataRow["STR_ND10"] = !string.IsNullOrWhiteSpace(item.STR_ND10) ? item.STR_ND10 : "";
                    dataRow["STR_NE"] = !string.IsNullOrWhiteSpace(item.STR_NE) ? item.STR_NE : "";
                    dataRow["STR_NM"] = !string.IsNullOrWhiteSpace(item.STR_NM) ? item.STR_NM : "";
                    dataRow["SXT_ND1_2"] = !string.IsNullOrWhiteSpace(item.SXT_ND1_2) ? item.SXT_ND1_2 : "";
                    dataRow["SXT_NE"] = !string.IsNullOrWhiteSpace(item.SXT_NE) ? item.SXT_NE : "";
                    dataRow["SXT_NM"] = !string.IsNullOrWhiteSpace(item.SXT_NM) ? item.SXT_NM : "";
                    dataRow["TCC_ND75"] = !string.IsNullOrWhiteSpace(item.TCC_ND75) ? item.TCC_ND75 : "";
                    dataRow["TCC_NE"] = !string.IsNullOrWhiteSpace(item.TCC_NE) ? item.TCC_NE : "";
                    dataRow["TCC_NM"] = !string.IsNullOrWhiteSpace(item.TCC_NM) ? item.TCC_NM : "";
                    dataRow["TCY_ND30"] = !string.IsNullOrWhiteSpace(item.TCY_ND30) ? item.TCY_ND30 : "";
                    dataRow["TCY_NE"] = !string.IsNullOrWhiteSpace(item.TCY_NE) ? item.TCY_NE : "";
                    dataRow["TCY_NM"] = !string.IsNullOrWhiteSpace(item.TCY_NM) ? item.TCY_NM : "";
                    dataRow["TEC_ND30"] = !string.IsNullOrWhiteSpace(item.TEC_ND30) ? item.TEC_ND30 : "";
                    dataRow["TEC_NE"] = !string.IsNullOrWhiteSpace(item.TEC_NE) ? item.TEC_NE : "";
                    dataRow["TEC_NM"] = !string.IsNullOrWhiteSpace(item.TEC_NM) ? item.TEC_NM : "";
                    dataRow["TGC_ND15"] = !string.IsNullOrWhiteSpace(item.TGC_ND15) ? item.TGC_ND15 : "";
                    dataRow["TGC_NE"] = !string.IsNullOrWhiteSpace(item.TGC_NE) ? item.TGC_NE : "";
                    dataRow["TGC_NM"] = !string.IsNullOrWhiteSpace(item.TGC_NM) ? item.TGC_NM : "";
                    dataRow["TIC_ND75"] = !string.IsNullOrWhiteSpace(item.TIC_ND75) ? item.TIC_ND75 : "";
                    dataRow["TIC_NE"] = !string.IsNullOrWhiteSpace(item.TIC_NE) ? item.TIC_NE : "";
                    dataRow["TIC_NM"] = !string.IsNullOrWhiteSpace(item.TIC_NM) ? item.TIC_NM : "";
                    dataRow["TOB_ND10"] = !string.IsNullOrWhiteSpace(item.TOB_ND10) ? item.TOB_ND10 : "";
                    dataRow["TOB_NE"] = !string.IsNullOrWhiteSpace(item.TOB_NE) ? item.TOB_NE : "";
                    dataRow["TOB_NM"] = !string.IsNullOrWhiteSpace(item.TOB_NM) ? item.TOB_NM : "";
                    dataRow["TZP_ND100"] = !string.IsNullOrWhiteSpace(item.TZP_ND100) ? item.TZP_ND100 : "";
                    dataRow["TZP_NE"] = !string.IsNullOrWhiteSpace(item.TZP_NE) ? item.TZP_NE : "";
                    dataRow["TZP_NM"] = !string.IsNullOrWhiteSpace(item.TZP_NM) ? item.TZP_NM : "";
                    dataRow["VAN_ND30"] = !string.IsNullOrWhiteSpace(item.VAN_ND30) ? item.VAN_ND30 : "";
                    dataRow["VAN_NE"] = !string.IsNullOrWhiteSpace(item.VAN_NE) ? item.VAN_NE : "";
                    dataRow["VAN_NM"] = !string.IsNullOrWhiteSpace(item.VAN_NM) ? item.VAN_NM : "";

                    //2025..04.23 Gerry 新增：依拉环素
                    dataRow["ERV_NM"] = !string.IsNullOrWhiteSpace(item.ERV_NM) ? item.ERV_NM : "";
                    dataRow["ERV_ND20"] = !string.IsNullOrWhiteSpace(item.ERV_ND20) ? item.ERV_ND20 : "";
                    dataRow["ERV_NE"] = !string.IsNullOrWhiteSpace(item.ERV_NE) ? item.ERV_NE : "";


                    dataTable.Rows.Add(dataRow);
                }

                #endregion

                #region 创建dbf

                var odbf = new DbfFile(Encoding.GetEncoding("GB2312"));
                odbf.Open(mapPath, FileMode.Create);

                #endregion

                #region //填充数据-Peng-24.01.21

                foreach (DataColumn dc in dataTable.Columns)
                {
                    var dcName = dc.ColumnName;
                    if (dcName == "FIRST_NAME" || dcName == "DATE_BIRTH" || dcName == "SPEC_DATE"|| dcName == "PATIENT_ID" || dcName == "LAST_NAME" || dcName == "SPEC_NUM" || dcName == "COMMENT")
                    {
                        odbf.Header.AddColumn(new DbfColumn(dc.ColumnName, DbfColumn.DbfColumnType.Character, 15, 0));
                        continue;
                    }
                    odbf.Header.AddColumn(new DbfColumn(dc.ColumnName, DbfColumn.DbfColumnType.Character, 10, 0));
                }
                var _orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                foreach (DataRow dr in dataTable.Rows)
                {
                    foreach (DataColumn dc in dataTable.Columns)
                    {
                        _orec[dc.ColumnName] = !string.IsNullOrWhiteSpace(dr[dc.ColumnName].ToString()) ? dr[dc.ColumnName].ToString() : string.Empty;
                    }
                    odbf.Write(_orec, true);
                }
                odbf.WriteHeader();
                odbf.Close();

                Log4Helper.Info(this.GetType(), $"容错文件生成成功，路径如下Peng\r\n{path}");

                #endregion

                return path;//返回，不执行下面的了

                #region //填充数据-暂不使用

                #region 设置表头 弃用
                //odbf.Header.AddColumn(new DbfColumn("COUNTRY_A", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("LABORATORY", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("PATIENT_ID", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("FIRST_NAME", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("LAST_NAME", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SEX", DbfColumn.DbfColumnType.Character, 15, 0));
                //odbf.Header.AddColumn(new DbfColumn("AGE", DbfColumn.DbfColumnType.Character, 10, 0));
                //odbf.Header.AddColumn(new DbfColumn("DATE_BIRTH", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("WARD", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("WARD_TYPE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("INSTITUT", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("DEPARTMENT", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SPEC_NUM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SPEC_DATE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SPEC_TYPE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SPEC_CODE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ORGANISM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ORG_TYPE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ESBL", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("BETA_LACT", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AMK_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AMC_ND20", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AZM_ND15", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AMP_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SAM_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ATM_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("OXA_ND1", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("POL_ND300", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("NIT_ND300", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SXT_ND1_2", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("STH_ND300", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("GEH_ND120", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ERY_ND15", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CIP_ND5", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CLI_ND2", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("RIF_ND5", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("LNZ_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("STR_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("FOS_ND200", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CHL_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("MEM_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("MNO_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("MFX_ND5", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("PIP_ND100", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TZP_ND100", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("PEN_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("GEN_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TCY_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TCC_ND75", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TIC_ND75", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TEC_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TGC_ND15", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("FEP_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CXM_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CEC_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CFP_ND75", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CSL_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CRO_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CTX_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CAZ_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("FOX_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZO_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TOB_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("VAN_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("IPM_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("LVX_ND5", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("OFX_ND5", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("DOX_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AMK_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AMC_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AZM_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AMP_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SAM_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ATM_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("OXA_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("POL_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("NIT_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("SXT_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("STH_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("GEH_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ERY_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CIP_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CLI_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("RIF_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("LNZ_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("STR_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("FOS_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CHL_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("MEM_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("MNO_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("MFX_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("PIP_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TZP_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("PEN_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("GEN_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("PEN_NE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TCY_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TCC_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TIC_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TEC_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TGC_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("FEP_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CXM_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CEC_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CFP_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CSL_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CRO_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CTX_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CAZ_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("FOX_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZO_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("TOB_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("VAN_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("VAN_NE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("IPM_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("LVX_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CTX_NE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CSL_ND75", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ETP_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("ETP_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CTT_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CTT_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("DOR_ND10", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("DOR_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("NET_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("NET_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("QDA_ND15", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("QDA_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CPT_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CPT_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CPT_NE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZA_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZA_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZA_NE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AZA_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AZA_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("AZA_NE", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZT_ND30", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZT_NM", DbfColumn.DbfColumnType.Character, 50, 0));
                //odbf.Header.AddColumn(new DbfColumn("CZT_NE", DbfColumn.DbfColumnType.Character, 50, 0));
                ////odbf.Header.AddColumn(new DbfColumn("TGC_NE", DbfColumn.DbfColumnType.Character, 50, 0));//新增字段
                //odbf.Header.AddColumn(new DbfColumn("DOX_NM", DbfColumn.DbfColumnType.Character, 50, 0));//新增字段
                #endregion

                #region 设置表头
                odbf.Header.AddColumn(new DbfColumn("COUNTRY_A", DbfColumn.DbfColumnType.Character, 3, 0));
                odbf.Header.AddColumn(new DbfColumn("LABORATORY", DbfColumn.DbfColumnType.Character, 3, 0));
                odbf.Header.AddColumn(new DbfColumn("PATIENT_ID", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("FIRST_NAME", DbfColumn.DbfColumnType.Character, 20, 0));
                odbf.Header.AddColumn(new DbfColumn("LAST_NAME", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("SEX", DbfColumn.DbfColumnType.Character, 5, 0));
                odbf.Header.AddColumn(new DbfColumn("AGE", DbfColumn.DbfColumnType.Character, 5, 0));
                odbf.Header.AddColumn(new DbfColumn("DATE_BIRTH", DbfColumn.DbfColumnType.Character, 20, 0));
                odbf.Header.AddColumn(new DbfColumn("WARD", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("WARD_TYPE", DbfColumn.DbfColumnType.Character, 6, 0));
                odbf.Header.AddColumn(new DbfColumn("INSTITUT", DbfColumn.DbfColumnType.Character, 3, 0));
                odbf.Header.AddColumn(new DbfColumn("DEPARTMENT", DbfColumn.DbfColumnType.Character, 3, 0));
                odbf.Header.AddColumn(new DbfColumn("SPEC_NUM", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("SPEC_DATE", DbfColumn.DbfColumnType.Character, 20, 0));
                odbf.Header.AddColumn(new DbfColumn("SPEC_TYPE", DbfColumn.DbfColumnType.Character, 2, 0));
                odbf.Header.AddColumn(new DbfColumn("SPEC_CODE", DbfColumn.DbfColumnType.Character, 3, 0));
                odbf.Header.AddColumn(new DbfColumn("ORGANISM", DbfColumn.DbfColumnType.Character, 3, 0));
                odbf.Header.AddColumn(new DbfColumn("ORG_TYPE", DbfColumn.DbfColumnType.Character, 1, 0));
                odbf.Header.AddColumn(new DbfColumn("ESBL", DbfColumn.DbfColumnType.Character, 1, 0));
                odbf.Header.AddColumn(new DbfColumn("BETA_LACT", DbfColumn.DbfColumnType.Character, 1, 0));
                odbf.Header.AddColumn(new DbfColumn("INDUC_CLI", DbfColumn.DbfColumnType.Character, 1, 0));
                odbf.Header.AddColumn(new DbfColumn("CARBAPENEM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("COMMENT", DbfColumn.DbfColumnType.Character, 30, 0));
                odbf.Header.AddColumn(new DbfColumn("AMC_ND20", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMC_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMC_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMK_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMK_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMK_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMP_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMP_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AMP_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ATM_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ATM_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ATM_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AZA_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AZA_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AZA_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AZM_ND15", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AZM_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("AZM_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CAZ_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CAZ_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CAZ_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CEC_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CEC_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CEC_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CFP_ND75", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CFP_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CFP_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CHL_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CHL_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CHL_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CIP_ND5", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CIP_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CIP_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CLI_ND2", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CLI_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CLI_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("COL_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("COL_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("COL_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CPT_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CPT_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CPT_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CRO_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CRO_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CRO_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CSL_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CSL_ND75", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CSL_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CTT_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CTT_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CTT_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CTX_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CTX_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CTX_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CXM_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CXM_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CXM_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZA_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZA_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZA_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZO_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZO_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZO_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZT_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZT_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("CZT_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("DOR_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("DOR_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("DOR_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("DOX_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("DOX_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("DOX_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ERY_ND15", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ERY_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ERY_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ETP_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ETP_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("ETP_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FEP_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FEP_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FEP_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FOS_ND200", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FOS_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FOS_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FOX_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FOX_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("FOX_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("GEH_ND120", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("GEH_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("GEN_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("GEN_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("GEN_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("IPM_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("IPM_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("IPM_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("LNZ_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("LNZ_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("LNZ_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("LVX_ND5", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("LVX_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("LVX_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MEM_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MEM_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MEM_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MFX_ND5", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MFX_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MFX_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MNO_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MNO_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("MNO_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("NET_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("NET_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("NET_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("NIT_ND300", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("NIT_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("NIT_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("OFX_ND5", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("OXA_ND1", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("OXA_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("OXA_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("PEN_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("PEN_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("PEN_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("PIP_ND100", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("PIP_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("PIP_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("POL_ND300", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("POL_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("POL_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("QDA_ND15", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("QDA_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("QDA_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("RIF_ND5", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("RIF_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("RIF_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("SAM_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("SAM_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("SAM_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("STH_ND300", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("STH_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("STH_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("STR_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("STR_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("STR_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("SXT_ND1_2", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("SXT_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("SXT_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TCC_ND75", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TCC_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TCC_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TCY_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TCY_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TCY_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TEC_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TEC_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TEC_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TGC_ND15", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TGC_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TGC_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TIC_ND75", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TIC_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TIC_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TOB_ND10", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TOB_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TOB_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TZP_ND100", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TZP_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("TZP_NM", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("VAN_ND30", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("VAN_NE", DbfColumn.DbfColumnType.Character, 10, 0));
                odbf.Header.AddColumn(new DbfColumn("VAN_NM", DbfColumn.DbfColumnType.Character, 10, 0));

                #endregion

                Log4Helper.Info(this.GetType(), $"开始填充容错文件数据");

                #region 填充数据

                var orec = new DbfRecord(odbf.Header) { AllowDecimalTruncate = true };
                foreach (var item in data)
                {
                    #region //弃用-已注销

                    //orec["COUNTRY_A"] = !string.IsNullOrWhiteSpace(item.COUNTRY_A) ? item.COUNTRY_A : "";
                    //orec["LABORATORY"] = !string.IsNullOrWhiteSpace(item.LABORATORY) ? item.LABORATORY : "";
                    //orec["PATIENT_ID"] = !string.IsNullOrWhiteSpace(item.PATIENT_ID) ? item.PATIENT_ID : "";
                    //orec["FIRST_NAME"] = !string.IsNullOrWhiteSpace(item.FIRST_NAME) ? item.FIRST_NAME : "";
                    //orec["LAST_NAME"] = !string.IsNullOrWhiteSpace(item.LAST_NAME) ? item.LAST_NAME : "";
                    //orec["SEX"] = !string.IsNullOrWhiteSpace(item.SEX) ? item.SEX : "";
                    //orec["AGE"] = !string.IsNullOrWhiteSpace(item.AGE) ? item.AGE : "";
                    //orec["DATE_BIRTH"] = !string.IsNullOrWhiteSpace(item.DATE_BIRTH) ? item.DATE_BIRTH : "";
                    //orec["WARD"] = !string.IsNullOrWhiteSpace(item.WARD) ? item.WARD : "";
                    //orec["WARD_TYPE"] = !string.IsNullOrWhiteSpace(item.WARD_TYPE) ? item.WARD_TYPE : "";
                    //orec["INSTITUT"] = !string.IsNullOrWhiteSpace(item.INSTITUT) ? item.INSTITUT : "";
                    //orec["DEPARTMENT"] = !string.IsNullOrWhiteSpace(item.DEPARTMENT) ? item.DEPARTMENT : "";
                    //orec["SPEC_NUM"] = !string.IsNullOrWhiteSpace(item.SPEC_NUM) ? item.SPEC_NUM : "";
                    //orec["SPEC_DATE"] = !string.IsNullOrWhiteSpace(item.SPEC_DATE) ? item.SPEC_DATE : "";
                    //orec["SPEC_TYPE"] = !string.IsNullOrWhiteSpace(item.SPEC_TYPE) ? item.SPEC_TYPE : "";
                    //orec["SPEC_CODE"] = !string.IsNullOrWhiteSpace(item.SPEC_CODE) ? item.SPEC_CODE : "";
                    //orec["ORGANISM"] = !string.IsNullOrWhiteSpace(item.ORGANISM) ? item.ORGANISM : "";
                    //orec["ORG_TYPE"] = !string.IsNullOrWhiteSpace(item.ORG_TYPE) ? item.ORG_TYPE : "";
                    //orec["ESBL"] = !string.IsNullOrWhiteSpace(item.ESBL) ? item.ESBL : "";
                    //orec["BETA_LACT"] = !string.IsNullOrWhiteSpace(item.BETA_LACT) ? item.BETA_LACT : "";
                    //orec["AMK_ND30"] = !string.IsNullOrWhiteSpace(item.AMK_ND30) ? item.AMK_ND30 : "";
                    //orec["AMC_ND20"] = !string.IsNullOrWhiteSpace(item.AMC_ND20) ? item.AMC_ND20 : "";
                    //orec["AZM_ND15"] = !string.IsNullOrWhiteSpace(item.AZM_ND15) ? item.AZM_ND15 : "";
                    //orec["AMP_ND10"] = !string.IsNullOrWhiteSpace(item.AMP_ND10) ? item.AMP_ND10 : "";
                    //orec["SAM_ND10"] = !string.IsNullOrWhiteSpace(item.SAM_ND10) ? item.SAM_ND10 : "";
                    //orec["ATM_ND30"] = !string.IsNullOrWhiteSpace(item.ATM_ND30) ? item.ATM_ND30 : "";
                    //orec["OXA_ND1"] = !string.IsNullOrWhiteSpace(item.OXA_ND1) ? item.OXA_ND1 : "";
                    //orec["POL_ND300"] = !string.IsNullOrWhiteSpace(item.POL_ND300) ? item.POL_ND300 : "";
                    //orec["NIT_ND300"] = !string.IsNullOrWhiteSpace(item.NIT_ND300) ? item.NIT_ND300 : "";
                    //orec["SXT_ND1_2"] = !string.IsNullOrWhiteSpace(item.SXT_ND1_2) ? item.SXT_ND1_2 : "";
                    //orec["STH_ND300"] = !string.IsNullOrWhiteSpace(item.STH_ND300) ? item.STH_ND300 : "";
                    //orec["GEH_ND120"] = !string.IsNullOrWhiteSpace(item.GEH_ND120) ? item.GEH_ND120 : "";
                    //orec["ERY_ND15"] = !string.IsNullOrWhiteSpace(item.ERY_ND15) ? item.ERY_ND15 : "";
                    //orec["CIP_ND5"] = !string.IsNullOrWhiteSpace(item.CIP_ND5) ? item.CIP_ND5 : "";
                    //orec["CLI_ND2"] = !string.IsNullOrWhiteSpace(item.CLI_ND2) ? item.CLI_ND2 : "";
                    //orec["RIF_ND5"] = !string.IsNullOrWhiteSpace(item.RIF_ND5) ? item.RIF_ND5 : "";
                    //orec["LNZ_ND30"] = !string.IsNullOrWhiteSpace(item.LNZ_ND30) ? item.LNZ_ND30 : "";
                    //orec["STR_ND10"] = !string.IsNullOrWhiteSpace(item.STR_ND10) ? item.STR_ND10 : "";
                    //orec["FOS_ND200"] = !string.IsNullOrWhiteSpace(item.FOS_ND200) ? item.FOS_ND200 : "";
                    //orec["CHL_ND30"] = !string.IsNullOrWhiteSpace(item.CHL_ND30) ? item.CHL_ND30 : "";
                    //orec["MEM_ND10"] = !string.IsNullOrWhiteSpace(item.MEM_ND10) ? item.MEM_ND10 : "";
                    //orec["MNO_ND30"] = !string.IsNullOrWhiteSpace(item.MNO_ND30) ? item.MNO_ND30 : "";
                    //orec["MFX_ND5"] = !string.IsNullOrWhiteSpace(item.MFX_ND5) ? item.MFX_ND5 : "";
                    //orec["PIP_ND100"] = !string.IsNullOrWhiteSpace(item.PIP_ND100) ? item.PIP_ND100 : "";
                    //orec["TZP_ND100"] = !string.IsNullOrWhiteSpace(item.TZP_ND100) ? item.TZP_ND100 : "";
                    //orec["PEN_ND10"] = !string.IsNullOrWhiteSpace(item.PEN_ND10) ? item.PEN_ND10 : "";
                    //orec["GEN_ND10"] = !string.IsNullOrWhiteSpace(item.GEN_ND10) ? item.GEN_ND10 : "";
                    //orec["TCY_ND30"] = !string.IsNullOrWhiteSpace(item.TCY_ND30) ? item.TCY_ND30 : "";
                    //orec["TCC_ND75"] = !string.IsNullOrWhiteSpace(item.TCC_ND75) ? item.TCC_ND75 : "";
                    //orec["TIC_ND75"] = !string.IsNullOrWhiteSpace(item.TIC_ND75) ? item.TIC_ND75 : "";
                    //orec["TEC_ND30"] = !string.IsNullOrWhiteSpace(item.TEC_ND30) ? item.TEC_ND30 : "";
                    //orec["TGC_ND15"] = !string.IsNullOrWhiteSpace(item.TGC_ND15) ? item.TGC_ND15 : "";
                    //orec["FEP_ND30"] = !string.IsNullOrWhiteSpace(item.FEP_ND30) ? item.FEP_ND30 : "";
                    //orec["CXM_ND30"] = !string.IsNullOrWhiteSpace(item.CXM_ND30) ? item.CXM_ND30 : "";
                    //orec["CEC_ND30"] = !string.IsNullOrWhiteSpace(item.CEC_ND30) ? item.CEC_ND30 : "";
                    //orec["CFP_ND75"] = !string.IsNullOrWhiteSpace(item.CFP_ND75) ? item.CFP_ND75 : "";
                    //orec["CSL_ND30"] = !string.IsNullOrWhiteSpace(item.CSL_ND30) ? item.CSL_ND30 : "";
                    //orec["CRO_ND30"] = !string.IsNullOrWhiteSpace(item.CRO_ND30) ? item.CRO_ND30 : "";
                    //orec["CTX_ND30"] = !string.IsNullOrWhiteSpace(item.CTX_ND30) ? item.CTX_ND30 : "";
                    //orec["CAZ_ND30"] = !string.IsNullOrWhiteSpace(item.CAZ_ND30) ? item.CAZ_ND30 : "";
                    //orec["FOX_ND30"] = !string.IsNullOrWhiteSpace(item.FOX_ND30) ? item.FOX_ND30 : "";
                    //orec["CZO_ND30"] = !string.IsNullOrWhiteSpace(item.CZO_ND30) ? item.CZO_ND30 : "";
                    //orec["TOB_ND10"] = !string.IsNullOrWhiteSpace(item.TOB_ND10) ? item.TOB_ND10 : "";
                    //orec["VAN_ND30"] = !string.IsNullOrWhiteSpace(item.VAN_ND30) ? item.VAN_ND30 : "";
                    //orec["IPM_ND10"] = !string.IsNullOrWhiteSpace(item.IPM_ND10) ? item.IPM_ND10 : "";
                    //orec["LVX_ND5"] = !string.IsNullOrWhiteSpace(item.LVX_ND5) ? item.LVX_ND5 : "";
                    //orec["OFX_ND5"] = !string.IsNullOrWhiteSpace(item.OFX_ND5) ? item.OFX_ND5 : "";
                    //orec["DOX_ND30"] = !string.IsNullOrWhiteSpace(item.DOX_ND30) ? item.DOX_ND30 : "";
                    //orec["AMK_NM"] = !string.IsNullOrWhiteSpace(item.AMK_NM) ? item.AMK_NM : "";
                    //orec["AMC_NM"] = !string.IsNullOrWhiteSpace(item.AMC_NM) ? item.AMC_NM : "";
                    //orec["AZM_NM"] = !string.IsNullOrWhiteSpace(item.AZM_NM) ? item.AZM_NM : "";
                    //orec["AMP_NM"] = !string.IsNullOrWhiteSpace(item.AMP_NM) ? item.AMP_NM : "";
                    //orec["SAM_NM"] = !string.IsNullOrWhiteSpace(item.SAM_NM) ? item.SAM_NM : "";
                    //orec["ATM_NM"] = !string.IsNullOrWhiteSpace(item.ATM_NM) ? item.ATM_NM : "";
                    //orec["OXA_NM"] = !string.IsNullOrWhiteSpace(item.OXA_NM) ? item.OXA_NM : "";
                    //orec["POL_NM"] = !string.IsNullOrWhiteSpace(item.POL_NM) ? item.POL_NM : "";
                    //orec["NIT_NM"] = !string.IsNullOrWhiteSpace(item.NIT_NM) ? item.NIT_NM : "";
                    //orec["SXT_NM"] = !string.IsNullOrWhiteSpace(item.SXT_NM) ? item.SXT_NM : "";
                    //orec["STH_NM"] = !string.IsNullOrWhiteSpace(item.STH_NM) ? item.STH_NM : "";
                    //orec["GEH_NM"] = !string.IsNullOrWhiteSpace(item.GEH_NM) ? item.GEH_NM : "";
                    //orec["ERY_NM"] = !string.IsNullOrWhiteSpace(item.ERY_NM) ? item.ERY_NM : "";
                    //orec["CIP_NM"] = !string.IsNullOrWhiteSpace(item.CIP_NM) ? item.CIP_NM : "";
                    //orec["CLI_NM"] = !string.IsNullOrWhiteSpace(item.CLI_NM) ? item.CLI_NM : "";
                    //orec["RIF_NM"] = !string.IsNullOrWhiteSpace(item.RIF_NM) ? item.RIF_NM : "";
                    //orec["LNZ_NM"] = !string.IsNullOrWhiteSpace(item.LNZ_NM) ? item.LNZ_NM : "";
                    //orec["STR_NM"] = !string.IsNullOrWhiteSpace(item.STR_NM) ? item.STR_NM : "";
                    //orec["FOS_NM"] = !string.IsNullOrWhiteSpace(item.FOS_NM) ? item.FOS_NM : "";
                    //orec["CHL_NM"] = !string.IsNullOrWhiteSpace(item.CHL_NM) ? item.CHL_NM : "";
                    //orec["MEM_NM"] = !string.IsNullOrWhiteSpace(item.MEM_NM) ? item.MEM_NM : "";
                    //orec["MNO_NM"] = !string.IsNullOrWhiteSpace(item.MNO_NM) ? item.MNO_NM : "";
                    //orec["MFX_NM"] = !string.IsNullOrWhiteSpace(item.MFX_NM) ? item.MFX_NM : "";
                    //orec["PIP_NM"] = !string.IsNullOrWhiteSpace(item.PIP_NM) ? item.PIP_NM : "";
                    //orec["TZP_NM"] = !string.IsNullOrWhiteSpace(item.TZP_NM) ? item.TZP_NM : "";
                    //orec["PEN_NM"] = !string.IsNullOrWhiteSpace(item.PEN_NM) ? item.PEN_NM : "";
                    //orec["GEN_NM"] = !string.IsNullOrWhiteSpace(item.GEN_NM) ? item.GEN_NM : "";
                    //orec["PEN_NE"] = !string.IsNullOrWhiteSpace(item.PEN_NE) ? item.PEN_NE : "";
                    //orec["TCY_NM"] = !string.IsNullOrWhiteSpace(item.TCY_NM) ? item.TCY_NM : "";
                    //orec["TCC_NM"] = !string.IsNullOrWhiteSpace(item.TCC_NM) ? item.TCC_NM : "";
                    //orec["TIC_NM"] = !string.IsNullOrWhiteSpace(item.TIC_NM) ? item.TIC_NM : "";
                    //orec["TEC_NM"] = !string.IsNullOrWhiteSpace(item.TEC_NM) ? item.TEC_NM : "";
                    //orec["TGC_NM"] = !string.IsNullOrWhiteSpace(item.TGC_NM) ? item.TGC_NM : "";
                    //orec["FEP_NM"] = !string.IsNullOrWhiteSpace(item.FEP_NM) ? item.FEP_NM : "";
                    //orec["CXM_NM"] = !string.IsNullOrWhiteSpace(item.CXM_NM) ? item.CXM_NM : "";
                    //orec["CEC_NM"] = !string.IsNullOrWhiteSpace(item.CEC_NM) ? item.CEC_NM : "";
                    //orec["CFP_NM"] = !string.IsNullOrWhiteSpace(item.CFP_NM) ? item.CFP_NM : "";
                    //orec["CSL_NM"] = !string.IsNullOrWhiteSpace(item.CSL_NM) ? item.CSL_NM : "";
                    //orec["CRO_NM"] = !string.IsNullOrWhiteSpace(item.CRO_NM) ? item.CRO_NM : "";
                    //orec["CTX_NM"] = !string.IsNullOrWhiteSpace(item.CTX_NM) ? item.CTX_NM : "";
                    //orec["CAZ_NM"] = !string.IsNullOrWhiteSpace(item.CAZ_NM) ? item.CAZ_NM : "";
                    //orec["FOX_NM"] = !string.IsNullOrWhiteSpace(item.FOX_NM) ? item.FOX_NM : "";
                    //orec["CZO_NM"] = !string.IsNullOrWhiteSpace(item.CZO_NM) ? item.CZO_NM : "";
                    //orec["TOB_NM"] = !string.IsNullOrWhiteSpace(item.TOB_NM) ? item.TOB_NM : "";
                    //orec["VAN_NM"] = !string.IsNullOrWhiteSpace(item.VAN_NM) ? item.VAN_NM : "";
                    //orec["VAN_NE"] = !string.IsNullOrWhiteSpace(item.VAN_NE) ? item.VAN_NE : "";
                    //orec["IPM_NM"] = !string.IsNullOrWhiteSpace(item.IPM_NM) ? item.IPM_NM : "";
                    //orec["LVX_NM"] = !string.IsNullOrWhiteSpace(item.LVX_NM) ? item.LVX_NM : "";
                    //orec["CTX_NE"] = !string.IsNullOrWhiteSpace(item.CTX_NE) ? item.CTX_NE : "";
                    //orec["CSL_ND75"] = !string.IsNullOrWhiteSpace(item.CSL_ND75) ? item.CSL_ND75 : "";
                    //orec["ETP_ND10"] = !string.IsNullOrWhiteSpace(item.ETP_ND10) ? item.ETP_ND10 : "";
                    //orec["ETP_NM"] = !string.IsNullOrWhiteSpace(item.ETP_NM) ? item.ETP_NM : "";
                    //orec["CTT_ND30"] = !string.IsNullOrWhiteSpace(item.CTT_ND30) ? item.CTT_ND30 : "";
                    //orec["CTT_NM"] = !string.IsNullOrWhiteSpace(item.CTT_NM) ? item.CTT_NM : "";
                    //orec["DOR_ND10"] = !string.IsNullOrWhiteSpace(item.DOR_ND10) ? item.DOR_ND10 : "";
                    //orec["DOR_NM"] = !string.IsNullOrWhiteSpace(item.DOR_NM) ? item.DOR_NM : "";
                    //orec["NET_ND30"] = !string.IsNullOrWhiteSpace(item.NET_ND30) ? item.NET_ND30 : "";
                    //orec["NET_NM"] = !string.IsNullOrWhiteSpace(item.NET_NM) ? item.NET_NM : "";
                    //orec["QDA_ND15"] = !string.IsNullOrWhiteSpace(item.QDA_ND15) ? item.QDA_ND15 : "";
                    //orec["QDA_NM"] = !string.IsNullOrWhiteSpace(item.QDA_NM) ? item.QDA_NM : "";
                    //orec["CPT_ND30"] = !string.IsNullOrWhiteSpace(item.CPT_ND30) ? item.CPT_ND30 : "";
                    //orec["CPT_NM"] = !string.IsNullOrWhiteSpace(item.CPT_NM) ? item.CPT_NM : "";
                    //orec["CPT_NE"] = !string.IsNullOrWhiteSpace(item.CPT_NE) ? item.CPT_NE : "";
                    //orec["CZA_ND30"] = !string.IsNullOrWhiteSpace(item.CZA_ND30) ? item.CZA_ND30 : "";
                    //orec["CZA_NM"] = !string.IsNullOrWhiteSpace(item.CZA_NM) ? item.CZA_NM : "";
                    //orec["CZA_NE"] = !string.IsNullOrWhiteSpace(item.CZA_NE) ? item.CZA_NE : "";
                    //orec["AZA_ND30"] = !string.IsNullOrWhiteSpace(item.AZA_ND30) ? item.AZA_ND30 : "";
                    //orec["AZA_NM"] = !string.IsNullOrWhiteSpace(item.AZA_NM) ? item.AZA_NM : "";
                    //orec["AZA_NE"] = !string.IsNullOrWhiteSpace(item.AZA_NE) ? item.AZA_NE : "";
                    //orec["CZT_ND30"] = !string.IsNullOrWhiteSpace(item.CZT_ND30) ? item.CZT_ND30 : "";
                    //orec["CZT_NM"] = !string.IsNullOrWhiteSpace(item.CZT_NM) ? item.CZT_NM : "";
                    //orec["CZT_NE"] = !string.IsNullOrWhiteSpace(item.CZT_NE) ? item.CZT_NE : "";
                    ////orec["TGC_NE"] = !string.IsNullOrWhiteSpace(item.TGC_NE) ? item.TGC_NE : "";//新增字段
                    //orec["DOX_NM"] = !string.IsNullOrWhiteSpace(item.DOX_NM) ? item.DOX_NM : "";//新增字段

                    #endregion

                    orec["COUNTRY_A"] = !string.IsNullOrWhiteSpace(item.COUNTRY_A) ? item.COUNTRY_A : "";
                    orec["LABORATORY"] = !string.IsNullOrWhiteSpace(item.LABORATORY) ? item.LABORATORY : "";
                    orec["PATIENT_ID"] = !string.IsNullOrWhiteSpace(item.PATIENT_ID) ? item.PATIENT_ID : "";
                    orec["FIRST_NAME"] = !string.IsNullOrWhiteSpace(item.FIRST_NAME) ? item.FIRST_NAME : "";
                    orec["LAST_NAME"] = !string.IsNullOrWhiteSpace(item.LAST_NAME) ? item.LAST_NAME : "";
                    orec["SEX"] = !string.IsNullOrWhiteSpace(item.SEX) ? item.SEX : "";
                    orec["AGE"] = !string.IsNullOrWhiteSpace(item.AGE) ? item.AGE : "";
                    orec["DATE_BIRTH"] = !string.IsNullOrWhiteSpace(item.DATE_BIRTH) ? item.DATE_BIRTH : "";
                    orec["WARD"] = !string.IsNullOrWhiteSpace(item.WARD) ? item.WARD : "";
                    orec["WARD_TYPE"] = !string.IsNullOrWhiteSpace(item.WARD_TYPE) ? item.WARD_TYPE : "";
                    orec["INSTITUT"] = !string.IsNullOrWhiteSpace(item.INSTITUT) ? item.INSTITUT : "";
                    orec["DEPARTMENT"] = !string.IsNullOrWhiteSpace(item.DEPARTMENT) ? item.DEPARTMENT : "";
                    orec["SPEC_NUM"] = !string.IsNullOrWhiteSpace(item.SPEC_NUM) ? item.SPEC_NUM : "";
                    orec["SPEC_DATE"] = !string.IsNullOrWhiteSpace(item.SPEC_DATE) ? item.SPEC_DATE : "";
                    orec["SPEC_TYPE"] = !string.IsNullOrWhiteSpace(item.SPEC_TYPE) ? item.SPEC_TYPE : "";
                    orec["SPEC_CODE"] = !string.IsNullOrWhiteSpace(item.SPEC_CODE) ? item.SPEC_CODE : "";
                    orec["ORGANISM"] = !string.IsNullOrWhiteSpace(item.ORGANISM) ? item.ORGANISM : "";
                    orec["ORG_TYPE"] = !string.IsNullOrWhiteSpace(item.ORG_TYPE) ? item.ORG_TYPE : "";
                    orec["ESBL"] = !string.IsNullOrWhiteSpace(item.ESBL) ? item.ESBL : "";
                    orec["BETA_LACT"] = !string.IsNullOrWhiteSpace(item.BETA_LACT) ? item.BETA_LACT : "";
                    orec["INDUC_CLI"] = !string.IsNullOrWhiteSpace(item.INDUC_CLI) ? item.INDUC_CLI : "";
                    orec["CARBAPENEM"] = !string.IsNullOrWhiteSpace(item.CARBAPENEM) ? item.CARBAPENEM : "";
                    orec["COMMENT"] = !string.IsNullOrWhiteSpace(item.COMMENT) ? item.COMMENT : "";
                    orec["AMC_ND20"] = !string.IsNullOrWhiteSpace(item.AMC_ND20) ? item.AMC_ND20 : "";
                    orec["AMC_NE"] = !string.IsNullOrWhiteSpace(item.AMC_NE) ? item.AMC_NE : "";
                    orec["AMC_NM"] = !string.IsNullOrWhiteSpace(item.AMC_NM) ? item.AMC_NM : "";
                    orec["AMK_ND30"] = !string.IsNullOrWhiteSpace(item.AMK_ND30) ? item.AMK_ND30 : "";
                    orec["AMK_NE"] = !string.IsNullOrWhiteSpace(item.AMK_NE) ? item.AMK_NE : "";
                    orec["AMK_NM"] = !string.IsNullOrWhiteSpace(item.AMK_NM) ? item.AMK_NM : "";
                    orec["AMP_ND10"] = !string.IsNullOrWhiteSpace(item.AMP_ND10) ? item.AMP_ND10 : "";
                    orec["AMP_NE"] = !string.IsNullOrWhiteSpace(item.AMP_NE) ? item.AMP_NE : "";
                    orec["AMP_NM"] = !string.IsNullOrWhiteSpace(item.AMP_NM) ? item.AMP_NM : "";
                    orec["ATM_ND30"] = !string.IsNullOrWhiteSpace(item.ATM_ND30) ? item.ATM_ND30 : "";
                    orec["ATM_NE"] = !string.IsNullOrWhiteSpace(item.ATM_NE) ? item.ATM_NE : "";
                    orec["ATM_NM"] = !string.IsNullOrWhiteSpace(item.ATM_NM) ? item.ATM_NM : "";
                    orec["AZA_ND30"] = !string.IsNullOrWhiteSpace(item.AZA_ND30) ? item.AZA_ND30 : "";
                    orec["AZA_NE"] = !string.IsNullOrWhiteSpace(item.AZA_NE) ? item.AZA_NE : "";
                    orec["AZA_NM"] = !string.IsNullOrWhiteSpace(item.AZA_NM) ? item.AZA_NM : "";
                    orec["AZM_ND15"] = !string.IsNullOrWhiteSpace(item.AZM_ND15) ? item.AZM_ND15 : "";
                    orec["AZM_NE"] = !string.IsNullOrWhiteSpace(item.AZM_NE) ? item.AZM_NE : "";
                    orec["AZM_NM"] = !string.IsNullOrWhiteSpace(item.AZM_NM) ? item.AZM_NM : "";
                    orec["CAZ_ND30"] = !string.IsNullOrWhiteSpace(item.CAZ_ND30) ? item.CAZ_ND30 : "";
                    orec["CAZ_NE"] = !string.IsNullOrWhiteSpace(item.CAZ_NE) ? item.CAZ_NE : "";
                    orec["CAZ_NM"] = !string.IsNullOrWhiteSpace(item.CAZ_NM) ? item.CAZ_NM : "";
                    orec["CEC_ND30"] = !string.IsNullOrWhiteSpace(item.CEC_ND30) ? item.CEC_ND30 : "";
                    orec["CEC_NE"] = !string.IsNullOrWhiteSpace(item.CEC_NE) ? item.CEC_NE : "";
                    orec["CEC_NM"] = !string.IsNullOrWhiteSpace(item.CEC_NM) ? item.CEC_NM : "";
                    orec["CFP_ND75"] = !string.IsNullOrWhiteSpace(item.CFP_ND75) ? item.CFP_ND75 : "";
                    orec["CFP_NE"] = !string.IsNullOrWhiteSpace(item.CFP_NE) ? item.CFP_NE : "";
                    orec["CFP_NM"] = !string.IsNullOrWhiteSpace(item.CFP_NM) ? item.CFP_NM : "";
                    orec["CHL_ND30"] = !string.IsNullOrWhiteSpace(item.CHL_ND30) ? item.CHL_ND30 : "";
                    orec["CHL_NE"] = !string.IsNullOrWhiteSpace(item.CHL_NE) ? item.CHL_NE : "";
                    orec["CHL_NM"] = !string.IsNullOrWhiteSpace(item.CHL_NM) ? item.CHL_NM : "";
                    orec["CIP_ND5"] = !string.IsNullOrWhiteSpace(item.CIP_ND5) ? item.CIP_ND5 : "";
                    orec["CIP_NE"] = !string.IsNullOrWhiteSpace(item.CIP_NE) ? item.CIP_NE : "";
                    orec["CIP_NM"] = !string.IsNullOrWhiteSpace(item.CIP_NM) ? item.CIP_NM : "";
                    orec["CLI_ND2"] = !string.IsNullOrWhiteSpace(item.CLI_ND2) ? item.CLI_ND2 : "";
                    orec["CLI_NE"] = !string.IsNullOrWhiteSpace(item.CLI_NE) ? item.CLI_NE : "";
                    orec["CLI_NM"] = !string.IsNullOrWhiteSpace(item.CLI_NM) ? item.CLI_NM : "";
                    orec["COL_ND10"] = !string.IsNullOrWhiteSpace(item.COL_ND10) ? item.COL_ND10 : "";
                    orec["COL_NE"] = !string.IsNullOrWhiteSpace(item.COL_NE) ? item.COL_NE : "";
                    orec["COL_NM"] = !string.IsNullOrWhiteSpace(item.COL_NM) ? item.COL_NM : "";
                    orec["CPT_ND30"] = !string.IsNullOrWhiteSpace(item.CPT_ND30) ? item.CPT_ND30 : "";
                    orec["CPT_NE"] = !string.IsNullOrWhiteSpace(item.CPT_NE) ? item.CPT_NE : "";
                    orec["CPT_NM"] = !string.IsNullOrWhiteSpace(item.CPT_NM) ? item.CPT_NM : "";
                    orec["CRO_ND30"] = !string.IsNullOrWhiteSpace(item.CRO_ND30) ? item.CRO_ND30 : "";
                    orec["CRO_NE"] = !string.IsNullOrWhiteSpace(item.CRO_NE) ? item.CRO_NE : "";
                    orec["CRO_NM"] = !string.IsNullOrWhiteSpace(item.CRO_NM) ? item.CRO_NM : "";
                    orec["CSL_ND30"] = !string.IsNullOrWhiteSpace(item.CSL_ND30) ? item.CSL_ND30 : "";
                    orec["CSL_ND75"] = !string.IsNullOrWhiteSpace(item.CSL_ND75) ? item.CSL_ND75 : "";
                    orec["CSL_NM"] = !string.IsNullOrWhiteSpace(item.CSL_NM) ? item.CSL_NM : "";
                    orec["CTT_ND30"] = !string.IsNullOrWhiteSpace(item.CTT_ND30) ? item.CTT_ND30 : "";
                    orec["CTT_NE"] = !string.IsNullOrWhiteSpace(item.CTT_NE) ? item.CTT_NE : "";
                    orec["CTT_NM"] = !string.IsNullOrWhiteSpace(item.CTT_NM) ? item.CTT_NM : "";
                    orec["CTX_ND30"] = !string.IsNullOrWhiteSpace(item.CTX_ND30) ? item.CTX_ND30 : "";
                    orec["CTX_NE"] = !string.IsNullOrWhiteSpace(item.CTX_NE) ? item.CTX_NE : "";
                    orec["CTX_NM"] = !string.IsNullOrWhiteSpace(item.CTX_NM) ? item.CTX_NM : "";
                    orec["CXM_ND30"] = !string.IsNullOrWhiteSpace(item.CXM_ND30) ? item.CXM_ND30 : "";
                    orec["CXM_NE"] = !string.IsNullOrWhiteSpace(item.CXM_NE) ? item.CXM_NE : "";
                    orec["CXM_NM"] = !string.IsNullOrWhiteSpace(item.CXM_NM) ? item.CXM_NM : "";
                    orec["CZA_ND30"] = !string.IsNullOrWhiteSpace(item.CZA_ND30) ? item.CZA_ND30 : "";
                    orec["CZA_NE"] = !string.IsNullOrWhiteSpace(item.CZA_NE) ? item.CZA_NE : "";
                    orec["CZA_NM"] = !string.IsNullOrWhiteSpace(item.CZA_NM) ? item.CZA_NM : "";
                    orec["CZO_ND30"] = !string.IsNullOrWhiteSpace(item.CZO_ND30) ? item.CZO_ND30 : "";
                    orec["CZO_NE"] = !string.IsNullOrWhiteSpace(item.CZO_NE) ? item.CZO_NE : "";
                    orec["CZO_NM"] = !string.IsNullOrWhiteSpace(item.CZO_NM) ? item.CZO_NM : "";
                    orec["CZT_ND30"] = !string.IsNullOrWhiteSpace(item.CZT_ND30) ? item.CZT_ND30 : "";
                    orec["CZT_NE"] = !string.IsNullOrWhiteSpace(item.CZT_NE) ? item.CZT_NE : "";
                    orec["CZT_NM"] = !string.IsNullOrWhiteSpace(item.CZT_NM) ? item.CZT_NM : "";
                    orec["DOR_ND10"] = !string.IsNullOrWhiteSpace(item.DOR_ND10) ? item.DOR_ND10 : "";
                    orec["DOR_NE"] = !string.IsNullOrWhiteSpace(item.DOR_NE) ? item.DOR_NE : "";
                    orec["DOR_NM"] = !string.IsNullOrWhiteSpace(item.DOR_NM) ? item.DOR_NM : "";
                    orec["DOX_ND30"] = !string.IsNullOrWhiteSpace(item.DOX_ND30) ? item.DOX_ND30 : "";
                    orec["DOX_NE"] = !string.IsNullOrWhiteSpace(item.DOX_NE) ? item.DOX_NE : "";
                    orec["DOX_NM"] = !string.IsNullOrWhiteSpace(item.DOX_NM) ? item.DOX_NM : "";
                    orec["ERY_ND15"] = !string.IsNullOrWhiteSpace(item.ERY_ND15) ? item.ERY_ND15 : "";
                    orec["ERY_NE"] = !string.IsNullOrWhiteSpace(item.ERY_NE) ? item.ERY_NE : "";
                    orec["ERY_NM"] = !string.IsNullOrWhiteSpace(item.ERY_NM) ? item.ERY_NM : "";
                    orec["ETP_ND10"] = !string.IsNullOrWhiteSpace(item.ETP_ND10) ? item.ETP_ND10 : "";
                    orec["ETP_NE"] = !string.IsNullOrWhiteSpace(item.ETP_NE) ? item.ETP_NE : "";
                    orec["ETP_NM"] = !string.IsNullOrWhiteSpace(item.ETP_NM) ? item.ETP_NM : "";
                    orec["FEP_ND30"] = !string.IsNullOrWhiteSpace(item.FEP_ND30) ? item.FEP_ND30 : "";
                    orec["FEP_NE"] = !string.IsNullOrWhiteSpace(item.FEP_NE) ? item.FEP_NE : "";
                    orec["FEP_NM"] = !string.IsNullOrWhiteSpace(item.FEP_NM) ? item.FEP_NM : "";
                    orec["FOS_ND200"] = !string.IsNullOrWhiteSpace(item.FOS_ND200) ? item.FOS_ND200 : "";
                    orec["FOS_NE"] = !string.IsNullOrWhiteSpace(item.FOS_NE) ? item.FOS_NE : "";
                    orec["FOS_NM"] = !string.IsNullOrWhiteSpace(item.FOS_NM) ? item.FOS_NM : "";
                    orec["FOX_ND30"] = !string.IsNullOrWhiteSpace(item.FOX_ND30) ? item.FOX_ND30 : "";
                    orec["FOX_NE"] = !string.IsNullOrWhiteSpace(item.FOX_NE) ? item.FOX_NE : "";
                    orec["FOX_NM"] = !string.IsNullOrWhiteSpace(item.FOX_NM) ? item.FOX_NM : "";
                    orec["GEH_ND120"] = !string.IsNullOrWhiteSpace(item.GEH_ND120) ? item.GEH_ND120 : "";
                    orec["GEH_NM"] = !string.IsNullOrWhiteSpace(item.GEH_NM) ? item.GEH_NM : "";
                    orec["GEN_ND10"] = !string.IsNullOrWhiteSpace(item.GEN_ND10) ? item.GEN_ND10 : "";
                    orec["GEN_NE"] = !string.IsNullOrWhiteSpace(item.GEN_NE) ? item.GEN_NE : "";
                    orec["GEN_NM"] = !string.IsNullOrWhiteSpace(item.GEN_NM) ? item.GEN_NM : "";
                    orec["IPM_ND10"] = !string.IsNullOrWhiteSpace(item.IPM_ND10) ? item.IPM_ND10 : "";
                    orec["IPM_NE"] = !string.IsNullOrWhiteSpace(item.IPM_NE) ? item.IPM_NE : "";
                    orec["IPM_NM"] = !string.IsNullOrWhiteSpace(item.IPM_NM) ? item.IPM_NM : "";
                    orec["LNZ_ND30"] = !string.IsNullOrWhiteSpace(item.LNZ_ND30) ? item.LNZ_ND30 : "";
                    orec["LNZ_NE"] = !string.IsNullOrWhiteSpace(item.LNZ_NE) ? item.LNZ_NE : "";
                    orec["LNZ_NM"] = !string.IsNullOrWhiteSpace(item.LNZ_NM) ? item.LNZ_NM : "";
                    orec["LVX_ND5"] = !string.IsNullOrWhiteSpace(item.LVX_ND5) ? item.LVX_ND5 : "";
                    orec["LVX_NE"] = !string.IsNullOrWhiteSpace(item.LVX_NE) ? item.LVX_NE : "";
                    orec["LVX_NM"] = !string.IsNullOrWhiteSpace(item.LVX_NM) ? item.LVX_NM : "";
                    orec["MEM_ND10"] = !string.IsNullOrWhiteSpace(item.MEM_ND10) ? item.MEM_ND10 : "";
                    orec["MEM_NE"] = !string.IsNullOrWhiteSpace(item.MEM_NE) ? item.MEM_NE : "";
                    orec["MEM_NM"] = !string.IsNullOrWhiteSpace(item.MEM_NM) ? item.MEM_NM : "";
                    orec["MFX_ND5"] = !string.IsNullOrWhiteSpace(item.MFX_ND5) ? item.MFX_ND5 : "";
                    orec["MFX_NE"] = !string.IsNullOrWhiteSpace(item.MFX_NE) ? item.MFX_NE : "";
                    orec["MFX_NM"] = !string.IsNullOrWhiteSpace(item.MFX_NM) ? item.MFX_NM : "";
                    orec["MNO_ND30"] = !string.IsNullOrWhiteSpace(item.MNO_ND30) ? item.MNO_ND30 : "";
                    orec["MNO_NE"] = !string.IsNullOrWhiteSpace(item.MNO_NE) ? item.MNO_NE : "";
                    orec["MNO_NM"] = !string.IsNullOrWhiteSpace(item.MNO_NM) ? item.MNO_NM : "";
                    orec["NET_ND30"] = !string.IsNullOrWhiteSpace(item.NET_ND30) ? item.NET_ND30 : "";
                    orec["NET_NE"] = !string.IsNullOrWhiteSpace(item.NET_NE) ? item.NET_NE : "";
                    orec["NET_NM"] = !string.IsNullOrWhiteSpace(item.NET_NM) ? item.NET_NM : "";
                    orec["NIT_ND300"] = !string.IsNullOrWhiteSpace(item.NIT_ND300) ? item.NIT_ND300 : "";
                    orec["NIT_NE"] = !string.IsNullOrWhiteSpace(item.NIT_NE) ? item.NIT_NE : "";
                    orec["NIT_NM"] = !string.IsNullOrWhiteSpace(item.NIT_NM) ? item.NIT_NM : "";
                    orec["OFX_ND5"] = !string.IsNullOrWhiteSpace(item.OFX_ND5) ? item.OFX_ND5 : "";
                    orec["OXA_ND1"] = !string.IsNullOrWhiteSpace(item.OXA_ND1) ? item.OXA_ND1 : "";
                    orec["OXA_NE"] = !string.IsNullOrWhiteSpace(item.OXA_NE) ? item.OXA_NE : "";
                    orec["OXA_NM"] = !string.IsNullOrWhiteSpace(item.OXA_NM) ? item.OXA_NM : "";
                    orec["PEN_ND10"] = !string.IsNullOrWhiteSpace(item.PEN_ND10) ? item.PEN_ND10 : "";
                    orec["PEN_NE"] = !string.IsNullOrWhiteSpace(item.PEN_NE) ? item.PEN_NE : "";
                    orec["PEN_NM"] = !string.IsNullOrWhiteSpace(item.PEN_NM) ? item.PEN_NM : "";
                    orec["PIP_ND100"] = !string.IsNullOrWhiteSpace(item.PIP_ND100) ? item.PIP_ND100 : "";
                    orec["PIP_NE"] = !string.IsNullOrWhiteSpace(item.PIP_NE) ? item.PIP_NE : "";
                    orec["PIP_NM"] = !string.IsNullOrWhiteSpace(item.PIP_NM) ? item.PIP_NM : "";
                    orec["POL_ND300"] = !string.IsNullOrWhiteSpace(item.POL_ND300) ? item.POL_ND300 : "";
                    orec["POL_NE"] = !string.IsNullOrWhiteSpace(item.POL_NE) ? item.POL_NE : "";
                    orec["POL_NM"] = !string.IsNullOrWhiteSpace(item.POL_NM) ? item.POL_NM : "";
                    orec["QDA_ND15"] = !string.IsNullOrWhiteSpace(item.QDA_ND15) ? item.QDA_ND15 : "";
                    orec["QDA_NE"] = !string.IsNullOrWhiteSpace(item.QDA_NE) ? item.QDA_NE : "";
                    orec["QDA_NM"] = !string.IsNullOrWhiteSpace(item.QDA_NM) ? item.QDA_NM : "";
                    orec["RIF_ND5"] = !string.IsNullOrWhiteSpace(item.RIF_ND5) ? item.RIF_ND5 : "";
                    orec["RIF_NE"] = !string.IsNullOrWhiteSpace(item.RIF_NE) ? item.RIF_NE : "";
                    orec["RIF_NM"] = !string.IsNullOrWhiteSpace(item.RIF_NM) ? item.RIF_NM : "";
                    orec["SAM_ND10"] = !string.IsNullOrWhiteSpace(item.SAM_ND10) ? item.SAM_ND10 : "";
                    orec["SAM_NE"] = !string.IsNullOrWhiteSpace(item.SAM_NE) ? item.SAM_NE : "";
                    orec["SAM_NM"] = !string.IsNullOrWhiteSpace(item.SAM_NM) ? item.SAM_NM : "";
                    orec["STH_ND300"] = !string.IsNullOrWhiteSpace(item.STH_ND300) ? item.STH_ND300 : "";
                    orec["STH_NE"] = !string.IsNullOrWhiteSpace(item.STH_NE) ? item.STH_NE : "";
                    orec["STH_NM"] = !string.IsNullOrWhiteSpace(item.STH_NM) ? item.STH_NM : "";
                    orec["STR_ND10"] = !string.IsNullOrWhiteSpace(item.STR_ND10) ? item.STR_ND10 : "";
                    orec["STR_NE"] = !string.IsNullOrWhiteSpace(item.STR_NE) ? item.STR_NE : "";
                    orec["STR_NM"] = !string.IsNullOrWhiteSpace(item.STR_NM) ? item.STR_NM : "";
                    orec["SXT_ND1_2"] = !string.IsNullOrWhiteSpace(item.SXT_ND1_2) ? item.SXT_ND1_2 : "";
                    orec["SXT_NE"] = !string.IsNullOrWhiteSpace(item.SXT_NE) ? item.SXT_NE : "";
                    orec["SXT_NM"] = !string.IsNullOrWhiteSpace(item.SXT_NM) ? item.SXT_NM : "";
                    orec["TCC_ND75"] = !string.IsNullOrWhiteSpace(item.TCC_ND75) ? item.TCC_ND75 : "";
                    orec["TCC_NE"] = !string.IsNullOrWhiteSpace(item.TCC_NE) ? item.TCC_NE : "";
                    orec["TCC_NM"] = !string.IsNullOrWhiteSpace(item.TCC_NM) ? item.TCC_NM : "";
                    orec["TCY_ND30"] = !string.IsNullOrWhiteSpace(item.TCY_ND30) ? item.TCY_ND30 : "";
                    orec["TCY_NE"] = !string.IsNullOrWhiteSpace(item.TCY_NE) ? item.TCY_NE : "";
                    orec["TCY_NM"] = !string.IsNullOrWhiteSpace(item.TCY_NM) ? item.TCY_NM : "";
                    orec["TEC_ND30"] = !string.IsNullOrWhiteSpace(item.TEC_ND30) ? item.TEC_ND30 : "";
                    orec["TEC_NE"] = !string.IsNullOrWhiteSpace(item.TEC_NE) ? item.TEC_NE : "";
                    orec["TEC_NM"] = !string.IsNullOrWhiteSpace(item.TEC_NM) ? item.TEC_NM : "";
                    orec["TGC_ND15"] = !string.IsNullOrWhiteSpace(item.TGC_ND15) ? item.TGC_ND15 : "";
                    orec["TGC_NE"] = !string.IsNullOrWhiteSpace(item.TGC_NE) ? item.TGC_NE : "";
                    orec["TGC_NM"] = !string.IsNullOrWhiteSpace(item.TGC_NM) ? item.TGC_NM : "";
                    orec["TIC_ND75"] = !string.IsNullOrWhiteSpace(item.TIC_ND75) ? item.TIC_ND75 : "";
                    orec["TIC_NE"] = !string.IsNullOrWhiteSpace(item.TIC_NE) ? item.TIC_NE : "";
                    orec["TIC_NM"] = !string.IsNullOrWhiteSpace(item.TIC_NM) ? item.TIC_NM : "";
                    orec["TOB_ND10"] = !string.IsNullOrWhiteSpace(item.TOB_ND10) ? item.TOB_ND10 : "";
                    orec["TOB_NE"] = !string.IsNullOrWhiteSpace(item.TOB_NE) ? item.TOB_NE : "";
                    orec["TOB_NM"] = !string.IsNullOrWhiteSpace(item.TOB_NM) ? item.TOB_NM : "";
                    orec["TZP_ND100"] = !string.IsNullOrWhiteSpace(item.TZP_ND100) ? item.TZP_ND100 : "";
                    orec["TZP_NE"] = !string.IsNullOrWhiteSpace(item.TZP_NE) ? item.TZP_NE : "";
                    orec["TZP_NM"] = !string.IsNullOrWhiteSpace(item.TZP_NM) ? item.TZP_NM : "";
                    orec["VAN_ND30"] = !string.IsNullOrWhiteSpace(item.VAN_ND30) ? item.VAN_ND30 : "";
                    orec["VAN_NE"] = !string.IsNullOrWhiteSpace(item.VAN_NE) ? item.VAN_NE : "";
                    orec["VAN_NM"] = !string.IsNullOrWhiteSpace(item.VAN_NM) ? item.VAN_NM : "";
                    odbf.Write(orec, true);
                }

                #endregion

                odbf.WriteHeader();
                odbf.Close();

                #endregion

                Log4Helper.Info(this.GetType(), $"容错文件生成成功，路径如下\r\n{path}");

                return path;
            }
            catch (Exception ex)
            {
                Log4Helper.Error(this.GetType(), ex);
                throw ex;
            }
        }

        static string GetConnectionString(string dbfFilePath)
        {
            return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbfFilePath};Extended Properties=dBASE IV;";
        }

        /// <summary>
        /// 新容错文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetNewDbfFileName(string path)
        {
            string dbfFileName = System.IO.Path.GetFileNameWithoutExtension(path);
            if (dbfFileName.Length > 8)
            {
                long newDbfFileNameId = Convert.ToInt64(CommonHelper.GuidToLongID.ToString().Substring(0, 7));
                string newDbfFileName = $"T{newDbfFileNameId}";
                string dirPath = System.IO.Path.GetDirectoryName(System.Web.HttpContext.Current.Server.MapPath(path));

                if (new FileInfo(Path.Combine(dirPath, $"{newDbfFileName}.dbf")).Exists)
                {
                    return GetNewDbfFileName(dbfFileName);
                }
                else
                {
                    return newDbfFileName;
                }
            }
            else
            {
                return dbfFileName;
            }
        }

        /// <summary>
        /// 生成新的容错文件的路径
        /// </summary>
        /// <param name="entity">上传的医学数据</param>
        /// <param name="hospital">所属医院</param>
        /// <returns></returns>
        public string GetNewFilePath(MedicalData entity, HospitalTeamList hospitalTeam)
        {
            try
            {
                /**
                 * 
                 * 规则说明：
                 * 时间：2019-2-11 
                 * 读取用户上传选择的年份和季度，然后按照规则商城新的文件名称。
                 * 上半年：W+年份2位+16+医院编码.DBF 
                 * 下半年： W+年份2位+712+医院编码.DBF
                 * 全年：W+年份2位+T+医院编码.DBF   
                 * 
                 * 以2019为例，上半年用16表示(1-6月)，下半年712表示(7-12月)，全年直接用T表示。具体如下：
                 * 上半年：w1916医院编码.dbf
                 * 下半年：w19712医院编码.dbf
                 *  全年：w19t医院编码.dbf
                 *  
                 *  如果没有医院编码，则使用医院名称替换
                 * 
                 * */

                string newFileName = "";
                string code = string.IsNullOrWhiteSpace(hospitalTeam.Code) ? hospitalTeam.Name : hospitalTeam.Code;
                string year = entity.Year.ToString().Substring(2); //年份的后2位
                switch (entity.Quarter)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        return "错误的季度";
                    case 5:
                        //全年：W+年份2位+T+医院编码.DBF   
                       //newFileName = "w" + year + "t" + code + ".dbf";
                       //2021-01-28 胡老师要求去掉"t"
                        newFileName = "w" + year + code.ToLower() + ".dbf";
                        break;
                    case 6:
                        //上半年：W+年份2位+16+医院编码.DBF 
                        newFileName = "w" + year + "16" + code.ToLower() + ".dbf";
                        break;
                    case 7:
                        //下半年： W+年份2位+712+医院编码.DBF
                        newFileName = "w" + year + "712" + code.ToLower() + ".dbf";
                        break;
                    default:
                        newFileName = "";
                        break;
                }

                return entity.UploadFilePath.Substring(0, entity.UploadFilePath.LastIndexOf('/')) + "/" + newFileName.ToLower();
            }
            catch (Exception ex)
            {
                Log4Helper.Error(typeof(MedicalDataItemService), ex);
                throw ex;
            }
        }

        /// <summary>
        /// 使用Microsoft.ACE.OLEDB.12.0 生成dbf对应create table sql
        /// </summary>
        /// <param name="tableName">数据表名称（也是生成文件的文件名称）</param>
        /// <returns></returns>
        private string GetSQLwithCreateDBF(string tableName)
        {
            #region 弃用
            //return $@"CREATE TABLE {tableName} ( COUNTRY_A VARCHAR(48), LABORATORY VARCHAR(48), PATIENT_ID VARCHAR(48), FIRST_NAME NVARCHAR(48), LAST_NAME NVARCHAR(48), SEX VARCHAR(12), AGE VARCHAR(8), DATE_BIRTH VARCHAR(48), WARD VARCHAR(48), WARD_TYPE VARCHAR(48), INSTITUT VARCHAR(48), DEPARTMENT VARCHAR(48), SPEC_NUM VARCHAR(48), SPEC_DATE VARCHAR(48), SPEC_TYPE VARCHAR(48), SPEC_CODE VARCHAR(48), ORGANISM VARCHAR(48), ORG_TYPE VARCHAR(48), ESBL VARCHAR(48), BETA_LACT VARCHAR(48), AMK_ND30 VARCHAR(24), AMC_ND20 VARCHAR(24), AZM_ND15 VARCHAR(24), AMP_ND10 VARCHAR(24), SAM_ND10 VARCHAR(24), ATM_ND30 VARCHAR(24), OXA_ND1 VARCHAR(24), POL_ND300 VARCHAR(24), NIT_ND300 VARCHAR(24), SXT_ND1_2 VARCHAR(24), STH_ND300 VARCHAR(24), GEH_ND120 VARCHAR(24), ERY_ND15 VARCHAR(24), CIP_ND5 VARCHAR(24), CLI_ND2 VARCHAR(24), RIF_ND5 VARCHAR(24), LNZ_ND30 VARCHAR(24), STR_ND10 VARCHAR(24), FOS_ND200 VARCHAR(24), CHL_ND30 VARCHAR(24), MEM_ND10 VARCHAR(24), MNO_ND30 VARCHAR(24), MFX_ND5 VARCHAR(24), PIP_ND100 VARCHAR(24), TZP_ND100 VARCHAR(24), PEN_ND10 VARCHAR(24), GEN_ND10 VARCHAR(24), TCY_ND30 VARCHAR(24), TCC_ND75 VARCHAR(24), TIC_ND75 VARCHAR(24), TEC_ND30 VARCHAR(24), TGC_ND15 VARCHAR(24), FEP_ND30 VARCHAR(24), CXM_ND30 VARCHAR(24), CEC_ND30 VARCHAR(24), CFP_ND75 VARCHAR(24), CSL_ND30 VARCHAR(24), CRO_ND30 VARCHAR(24), CTX_ND30 VARCHAR(24), CAZ_ND30 VARCHAR(24), FOX_ND30 VARCHAR(24), CZO_ND30 VARCHAR(24), TOB_ND10 VARCHAR(24), VAN_ND30 VARCHAR(24), IPM_ND10 VARCHAR(24), LVX_ND5 VARCHAR(24), OFX_ND5 VARCHAR(24), DOX_ND30 VARCHAR(24), AMK_NM VARCHAR(24), AMC_NM VARCHAR(24), AZM_NM VARCHAR(24), AMP_NM VARCHAR(24), SAM_NM VARCHAR(24), ATM_NM VARCHAR(24), OXA_NM VARCHAR(24), POL_NM VARCHAR(24), NIT_NM VARCHAR(24), SXT_NM VARCHAR(24), STH_NM VARCHAR(24), GEH_NM VARCHAR(24), ERY_NM VARCHAR(24), CIP_NM VARCHAR(24), CLI_NM VARCHAR(24), RIF_NM VARCHAR(24), LNZ_NM VARCHAR(24), STR_NM VARCHAR(24), FOS_NM VARCHAR(24), CHL_NM VARCHAR(24), MEM_NM VARCHAR(24), MNO_NM VARCHAR(24), MFX_NM VARCHAR(24), PIP_NM VARCHAR(24), TZP_NM VARCHAR(24), PEN_NM VARCHAR(24), GEN_NM VARCHAR(24), PEN_NE VARCHAR(24), TCY_NM VARCHAR(24), TCC_NM VARCHAR(24), TIC_NM VARCHAR(24), TEC_NM VARCHAR(24), TGC_NM VARCHAR(24), FEP_NM VARCHAR(24), CXM_NM VARCHAR(24), CEC_NM VARCHAR(24), CFP_NM VARCHAR(24), CSL_NM VARCHAR(24), CRO_NM VARCHAR(24), CTX_NM VARCHAR(24), CAZ_NM VARCHAR(24), FOX_NM VARCHAR(24), CZO_NM VARCHAR(24), TOB_NM VARCHAR(24), VAN_NM VARCHAR(24), VAN_NE VARCHAR(24), IPM_NM VARCHAR(24), LVX_NM VARCHAR(24), CTX_NE VARCHAR(24), CSL_ND75 VARCHAR(24), ETP_ND10 VARCHAR(24), ETP_NM VARCHAR(24), CTT_ND30 VARCHAR(24), CTT_NM VARCHAR(24), DOR_ND10 VARCHAR(24), DOR_NM VARCHAR(24), NET_ND30 VARCHAR(24), NET_NM VARCHAR(24), QDA_ND15 VARCHAR(24), QDA_NM VARCHAR(24), CPT_ND30 VARCHAR(24), CPT_NM VARCHAR(24), CPT_NE VARCHAR(24), CZA_ND30 VARCHAR(24), CZA_NM VARCHAR(24), CZA_NE VARCHAR(24), AZA_ND30 VARCHAR(24), AZA_NM VARCHAR(24), AZA_NE VARCHAR(24), CZT_ND30 VARCHAR(24), CZT_NM VARCHAR(24), CZT_NE VARCHAR(24),DOX_NM VARCHAR(24));";//最后一个新增字段
            #endregion

            return $@"CREATE TABLE {tableName} (COUNTRY_A VARCHAR(3),LABORATORY VARCHAR(3),PATIENT_ID VARCHAR(30),FIRST_NAME VARCHAR(20),LAST_NAME VARCHAR(30),SEX VARCHAR(5),AGE VARCHAR(5),DATE_BIRTH VARCHAR(20),WARD VARCHAR(10),WARD_TYPE VARCHAR(6),INSTITUT VARCHAR(3),DEPARTMENT VARCHAR(3),SPEC_NUM VARCHAR(30),SPEC_DATE VARCHAR(20),SPEC_TYPE VARCHAR(2),SPEC_CODE VARCHAR(3),ORGANISM VARCHAR(3),ORG_TYPE VARCHAR(1),ESBL VARCHAR(1),BETA_LACT VARCHAR(1),INDUC_CLI VARCHAR(1),CARBAPENEM VARCHAR(10),COMMENT VARCHAR(30)AMC_ND20 VARCHAR(10),AMC_NE VARCHAR(10),AMC_NM VARCHAR(10),AMK_ND30 VARCHAR(10),AMK_NE VARCHAR(10),AMK_NM VARCHAR(10),AMP_ND10 VARCHAR(10),AMP_NE VARCHAR(10),AMP_NM VARCHAR(10),ATM_ND30 VARCHAR(10),ATM_NE VARCHAR(10),ATM_NM VARCHAR(10),AZA_ND30 VARCHAR(10),AZA_NE VARCHAR(10),AZA_NM VARCHAR(10),AZM_ND15 VARCHAR(10),AZM_NE VARCHAR(10),AZM_NM VARCHAR(10),CAZ_ND30 VARCHAR(10),CAZ_NE VARCHAR(10),CAZ_NM VARCHAR(10),CEC_ND30 VARCHAR(10),CEC_NE VARCHAR(10),CEC_NM VARCHAR(10),CFP_ND75 VARCHAR(10),CFP_NE VARCHAR(10),CFP_NM VARCHAR(10),CHL_ND30 VARCHAR(10),CHL_NE VARCHAR(10),CHL_NM VARCHAR(10),CIP_ND5 VARCHAR(10),CIP_NE VARCHAR(10),CIP_NM VARCHAR(10),CLI_ND2 VARCHAR(10),CLI_NE VARCHAR(10),CLI_NM VARCHAR(10),COL_ND10 VARCHAR(10),COL_NE VARCHAR(10),COL_NM VARCHAR(10),CPT_ND30 VARCHAR(10),CPT_NE VARCHAR(10),CPT_NM VARCHAR(10),CRO_ND30 VARCHAR(10),CRO_NE VARCHAR(10),CRO_NM VARCHAR(10),CSL_ND30 VARCHAR(10),CSL_ND75 VARCHAR(10),CSL_NM VARCHAR(10),CTT_ND30 VARCHAR(10),CTT_NE VARCHAR(10),CTT_NM VARCHAR(10),CTX_ND30 VARCHAR(10),CTX_NE VARCHAR(10),CTX_NM VARCHAR(10),CXM_ND30 VARCHAR(10),CXM_NE VARCHAR(10),CXM_NM VARCHAR(10),CZA_ND30 VARCHAR(10),CZA_NE VARCHAR(10),CZA_NM VARCHAR(10),CZO_ND30 VARCHAR(10),CZO_NE VARCHAR(10),CZO_NM VARCHAR(10),CZT_ND30 VARCHAR(10),CZT_NE VARCHAR(10),CZT_NM VARCHAR(10),DOR_ND10 VARCHAR(10),DOR_NE VARCHAR(10),DOR_NM VARCHAR(10),DOX_ND30 VARCHAR(10),DOX_NE VARCHAR(10),DOX_NM VARCHAR(10),ERY_ND15 VARCHAR(10),ERY_NE VARCHAR(10),ERY_NM VARCHAR(10),ETP_ND10 VARCHAR(10),ETP_NE VARCHAR(10),ETP_NM VARCHAR(10),FEP_ND30 VARCHAR(10),FEP_NE VARCHAR(10),FEP_NM VARCHAR(10),FOS_ND200 VARCHAR(10),FOS_NE VARCHAR(10),FOS_NM VARCHAR(10),FOX_ND30 VARCHAR(10),FOX_NE VARCHAR(10),FOX_NM VARCHAR(10),GEH_ND120 VARCHAR(10),GEH_NM VARCHAR(10),GEN_ND10 VARCHAR(10),GEN_NE VARCHAR(10),GEN_NM VARCHAR(10),IPM_ND10 VARCHAR(10),IPM_NE VARCHAR(10),IPM_NM VARCHAR(10),LNZ_ND30 VARCHAR(10),LNZ_NE VARCHAR(10),LNZ_NM VARCHAR(10),LVX_ND5 VARCHAR(10),LVX_NE VARCHAR(10),LVX_NM VARCHAR(10),MEM_ND10 VARCHAR(10),MEM_NE VARCHAR(10),MEM_NM VARCHAR(10),MFX_ND5 VARCHAR(10),MFX_NE VARCHAR(10),MFX_NM VARCHAR(10),MNO_ND30 VARCHAR(10),MNO_NE VARCHAR(10),MNO_NM VARCHAR(10),NET_ND30 VARCHAR(10),NET_NE VARCHAR(10),NET_NM VARCHAR(10),NIT_ND300 VARCHAR(10),NIT_NE VARCHAR(10),NIT_NM VARCHAR(10),OFX_ND5 VARCHAR(10),OXA_ND1 VARCHAR(10),OXA_NE VARCHAR(10),OXA_NM VARCHAR(10),PEN_ND10 VARCHAR(10),PEN_NE VARCHAR(10),PEN_NM VARCHAR(10),PIP_ND100 VARCHAR(10),PIP_NE VARCHAR(10),PIP_NM VARCHAR(10),POL_ND300 VARCHAR(10),POL_NE VARCHAR(10),POL_NM VARCHAR(10),QDA_ND15 VARCHAR(10),QDA_NE VARCHAR(10),QDA_NM VARCHAR(10),RIF_ND5 VARCHAR(10),RIF_NE VARCHAR(10),RIF_NM VARCHAR(10),SAM_ND10 VARCHAR(10),SAM_NE VARCHAR(10),SAM_NM VARCHAR(10),STH_ND300 VARCHAR(10),STH_NE VARCHAR(10),STH_NM VARCHAR(10),STR_ND10 VARCHAR(10),STR_NE VARCHAR(10),STR_NM VARCHAR(10),SXT_ND1_2 VARCHAR(10),SXT_NE VARCHAR(10),SXT_NM VARCHAR(10),TCC_ND75 VARCHAR(10),TCC_NE VARCHAR(10),TCC_NM VARCHAR(10),TCY_ND30 VARCHAR(10),TCY_NE VARCHAR(10),TCY_NM VARCHAR(10),TEC_ND30 VARCHAR(10),TEC_NE VARCHAR(10),TEC_NM VARCHAR(10),TGC_ND15 VARCHAR(10),TGC_NE VARCHAR(10),TGC_NM VARCHAR(10),TIC_ND75 VARCHAR(10),TIC_NE VARCHAR(10),TIC_NM VARCHAR(10),TOB_ND10 VARCHAR(10),TOB_NE VARCHAR(10),TOB_NM VARCHAR(10),TZP_ND100 VARCHAR(10),TZP_NE VARCHAR(10),TZP_NM VARCHAR(10),VAN_ND30 VARCHAR(10),VAN_NE VARCHAR(10),VAN_NM VARCHAR(10),";
        }

        /// <summary>
        /// 使用Microsoft.ACE.OLEDB.12.0 数据源插入dbf对应的insert into table sql
        /// </summary>
        /// <param name="tableName">dbf数据表名称</param>
        /// <returns></returns>
        private string GetSQLWithInsertDBF(string tableName)
        {
            return $@"INSERT INTO {tableName} VALUES(@COUNTRY_A,@LABORATORY,@PATIENT_ID,@FIRST_NAME,@LAST_NAME,@SEX,@AGE,@DATE_BIRTH,@WARD,@WARD_TYPE,@INSTITUT,@DEPARTMENT,@SPEC_NUM,@SPEC_DATE,@SPEC_TYPE,@SPEC_CODE,@ORGANISM,@ORG_TYPE,@ESBL,@BETA_LACT,@INDUC_CLI,@CARBAPENEM,@COMMENT,@AMC_ND20,@AMC_NE,@AMC_NM,@AMK_ND30,@AMK_NE,@AMK_NM,@AMP_ND10,@AMP_NE,@AMP_NM,@ATM_ND30,@ATM_NE,@ATM_NM,@AZA_ND30,@AZA_NE,@AZA_NM,@AZM_ND15,@AZM_NE,@AZM_NM,@CAZ_ND30,@CAZ_NE,@CAZ_NM,@CEC_ND30,@CEC_NE,@CEC_NM,@CFP_ND75,@CFP_NE,@CFP_NM,@CHL_ND30,@CHL_NE,@CHL_NM,@CIP_ND5,@CIP_NE,@CIP_NM,@CLI_ND2,@CLI_NE,@CLI_NM,@COL_ND10,@COL_NE,@COL_NM,@CPT_ND30,@CPT_NE,@CPT_NM,@CRO_ND30,@CRO_NE,@CRO_NM,@CSL_ND30,@CSL_ND75,@CSL_NM,@CTT_ND30,@CTT_NE,@CTT_NM,@CTX_ND30,@CTX_NE,@CTX_NM,@CXM_ND30,@CXM_NE,@CXM_NM,@CZA_ND30,@CZA_NE,@CZA_NM,@CZO_ND30,@CZO_NE,@CZO_NM,@CZT_ND30,@CZT_NE,@CZT_NM,@DOR_ND10,@DOR_NE,@DOR_NM,@DOX_ND30,@DOX_NE,@DOX_NM,@ERY_ND15,@ERY_NE,@ERY_NM,@ETP_ND10,@ETP_NE,@ETP_NM,@FEP_ND30,@FEP_NE,@FEP_NM,@FOS_ND200,@FOS_NE,@FOS_NM,@FOX_ND30,@FOX_NE,@FOX_NM,@GEH_ND120,@GEH_NM,@GEN_ND10,@GEN_NE,@GEN_NM,@IPM_ND10,@IPM_NE,@IPM_NM,@LNZ_ND30,@LNZ_NE,@LNZ_NM,@LVX_ND5,@LVX_NE,@LVX_NM,@MEM_ND10,@MEM_NE,@MEM_NM,@MFX_ND5,@MFX_NE,@MFX_NM,@MNO_ND30,@MNO_NE,@MNO_NM,@NET_ND30,@NET_NE,@NET_NM,@NIT_ND300,@NIT_NE,@NIT_NM,@OFX_ND5,@OXA_ND1,@OXA_NE,@OXA_NM,@PEN_ND10,@PEN_NE,@PEN_NM,@PIP_ND100,@PIP_NE,@PIP_NM,@POL_ND300,@POL_NE,@POL_NM,@QDA_ND15,@QDA_NE,@QDA_NM,@RIF_ND5,@RIF_NE,@RIF_NM,@SAM_ND10,@SAM_NE,@SAM_NM,@STH_ND300,@STH_NE,@STH_NM,@STR_ND10,@STR_NE,@STR_NM,@SXT_ND1_2,@SXT_NE,@SXT_NM,@TCC_ND75,@TCC_NE,@TCC_NM,@TCY_ND30,@TCY_NE,@TCY_NM,@TEC_ND30,@TEC_NE,@TEC_NM,@TGC_ND15,@TGC_NE,@TGC_NM,@TIC_ND75,@TIC_NE,@TIC_NM,@TOB_ND10,@TOB_NE,@TOB_NM,@TZP_ND100,@TZP_NE,@TZP_NM,@VAN_ND30,@VAN_NE,@VAN_NM)";
            #region 弃用
            //return $@"INSERT INTO {tableName} VALUES (@COUNTRY_A, @LABORATORY, @PATIENT_ID, @FIRST_NAME, @LAST_NAME, @SEX, @AGE, @DATE_BIRTH, @WARD, @WARD_TYPE, @INSTITUT, @DEPARTMENT, @SPEC_NUM, @SPEC_DATE, @SPEC_TYPE, @SPEC_CODE, @ORGANISM, @ORG_TYPE, @ESBL, @BETA_LACT, @AMK_ND30, @AMC_ND20, @AZM_ND15, @AMP_ND10, @SAM_ND10, @ATM_ND30, @OXA_ND1, @POL_ND300, @NIT_ND300, @SXT_ND1_2, @STH_ND300, @GEH_ND120, @ERY_ND15, @CIP_ND5, @CLI_ND2, @RIF_ND5, @LNZ_ND30, @STR_ND10, @FOS_ND200, @CHL_ND30, @MEM_ND10, @MNO_ND30, @MFX_ND5, @PIP_ND100, @TZP_ND100, @PEN_ND10, @GEN_ND10, @TCY_ND30, @TCC_ND75, @TIC_ND75, @TEC_ND30, @TGC_ND15, @FEP_ND30, @CXM_ND30, @CEC_ND30, @CFP_ND75, @CSL_ND30, @CRO_ND30, @CTX_ND30, @CAZ_ND30, @FOX_ND30, @CZO_ND30, @TOB_ND10, @VAN_ND30, @IPM_ND10, @LVX_ND5, @OFX_ND5, @DOX_ND30, @AMK_NM, @AMC_NM, @AZM_NM, @AMP_NM, @SAM_NM, @ATM_NM, @OXA_NM, @POL_NM, @NIT_NM, @SXT_NM, @STH_NM, @GEH_NM, @ERY_NM, @CIP_NM, @CLI_NM, @RIF_NM, @LNZ_NM, @STR_NM, @FOS_NM, @CHL_NM, @MEM_NM, @MNO_NM, @MFX_NM, @PIP_NM, @TZP_NM, @PEN_NM, @GEN_NM, @PEN_NE, @TCY_NM, @TCC_NM, @TIC_NM, @TEC_NM, @TGC_NM, @FEP_NM, @CXM_NM, @CEC_NM, @CFP_NM, @CSL_NM, @CRO_NM, @CTX_NM, @CAZ_NM, @FOX_NM, @CZO_NM, @TOB_NM, @VAN_NM, @VAN_NE, @IPM_NM, @LVX_NM, @CTX_NE, @CSL_ND75, @ETP_ND10, @ETP_NM, @CTT_ND30, @CTT_NM, @DOR_ND10, @DOR_NM, @NET_ND30, @NET_NM, @QDA_ND15, @QDA_NM, @CPT_ND30, @CPT_NM, @CPT_NE, @CZA_ND30, @CZA_NM, @CZA_NE, @AZA_ND30, @AZA_NM, @AZA_NE, @CZT_ND30, @CZT_NM, @CZT_NE, @DOX_NM);";//最后两个新增字段
            #endregion;
        }

    }
}
