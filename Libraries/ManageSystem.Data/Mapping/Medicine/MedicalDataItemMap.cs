using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ManageSystem.Core.Domain.Medicine;


namespace ManageSystem.Data.Mapping.Medicine
{
    /// <summary>
    /// 数据库操作类 ，数据库表名：MedicalDataItem 
    /// </summary>
    public partial class MedicalDataItemMap : ManageSystemEntityTypeConfiguration<MedicalDataItem>
    {

        public MedicalDataItemMap()
        {
            this.ToTable("MedicalDataItem");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Describe).IsMaxLength();
            this.Property(p => p.OrganismName).HasMaxLength(1000);
            
            this.Property(p => p.AMC_ND20).HasMaxLength(100);
            this.Property(p => p.COUNTRY_A).HasMaxLength(100);
            this.Property(p => p.LABORATORY).HasMaxLength(100);
            this.Property(p => p.ORIGIN).HasMaxLength(100);
            this.Property(p => p.PATIENT_ID).HasMaxLength(100);
            this.Property(p => p.FIRST_NAME).HasMaxLength(100);
            this.Property(p => p.LAST_NAME).HasMaxLength(100);
            this.Property(p => p.FULL_NAME).HasMaxLength(100);
            this.Property(p => p.SEX).HasMaxLength(100);
            this.Property(p => p.AGE).HasMaxLength(100);
            this.Property(p => p.PAT_TYPE).HasMaxLength(100);
            this.Property(p => p.WARD).HasMaxLength(100);
            this.Property(p => p.WARD_TYPE).HasMaxLength(100);
            this.Property(p => p.INSTITUT).HasMaxLength(100);
            this.Property(p => p.DEPARTMENT).HasMaxLength(100);
            this.Property(p => p.SPEC_NUM).HasMaxLength(100);
            this.Property(p => p.SPEC_TYPE).HasMaxLength(100);
            this.Property(p => p.SPEC_CODE).HasMaxLength(100);
            this.Property(p => p.ORGANISM).HasMaxLength(100);
            this.Property(p => p.ORG_TYPE).HasMaxLength(100);
            this.Property(p => p.ESBL).HasMaxLength(100);
            this.Property(p => p.BETA_LACT).HasMaxLength(100);
            this.Property(p => p.AMK_ND30).HasMaxLength(100);
            this.Property(p => p.DATE_BIRTH).HasMaxLength(100);
            
            this.Property(p => p.AMC_ND20).HasMaxLength(100);
            this.Property(p => p.AZM_ND15).HasMaxLength(100);
            this.Property(p => p.AMP_ND10).HasMaxLength(100);
            this.Property(p => p.SAM_ND10).HasMaxLength(100);
            this.Property(p => p.ATM_ND30).HasMaxLength(100);
            this.Property(p => p.OXA_ND1).HasMaxLength(100);
            this.Property(p => p.POL_ND300).HasMaxLength(100);
            this.Property(p => p.NIT_ND300).HasMaxLength(100);
            this.Property(p => p.SXT_ND1_2).HasMaxLength(100);
            this.Property(p => p.STH_ND300).HasMaxLength(100);
            this.Property(p => p.GEH_ND120).HasMaxLength(100);
            this.Property(p => p.ERY_ND15).HasMaxLength(100);
            this.Property(p => p.CIP_ND5).HasMaxLength(100);
            this.Property(p => p.CLI_ND2).HasMaxLength(100);
            this.Property(p => p.RIF_ND5).HasMaxLength(100);
            this.Property(p => p.LNZ_ND30).HasMaxLength(100);
            this.Property(p => p.STR_ND10).HasMaxLength(100);
            this.Property(p => p.FOS_ND200).HasMaxLength(100);
            this.Property(p => p.CHL_ND30).HasMaxLength(100);
            this.Property(p => p.MEM_ND10).HasMaxLength(100);
            this.Property(p => p.MNO_ND30).HasMaxLength(100);
            this.Property(p => p.MFX_ND5).HasMaxLength(100);
            this.Property(p => p.PIP_ND100).HasMaxLength(100);
            this.Property(p => p.TZP_ND100).HasMaxLength(100);
            this.Property(p => p.PEN_ND10).HasMaxLength(100);
            this.Property(p => p.GEN_ND10).HasMaxLength(100);
            this.Property(p => p.TCY_ND30).HasMaxLength(100);
            this.Property(p => p.TCC_ND75).HasMaxLength(100);
            this.Property(p => p.TIC_ND75).HasMaxLength(100);
            this.Property(p => p.TEC_ND30).HasMaxLength(100);
            this.Property(p => p.TGC_ND15).HasMaxLength(100);
            this.Property(p => p.FEP_ND30).HasMaxLength(100);
            this.Property(p => p.CXM_ND30).HasMaxLength(100);
            this.Property(p => p.CEC_ND30).HasMaxLength(100);
            this.Property(p => p.CFP_ND75).HasMaxLength(100);
            this.Property(p => p.CSL_ND30).HasMaxLength(100);
            this.Property(p => p.CRO_ND30).HasMaxLength(100);
            this.Property(p => p.CTX_ND30).HasMaxLength(100);
            this.Property(p => p.CAZ_ND30).HasMaxLength(100);
            this.Property(p => p.FOX_ND30).HasMaxLength(100);
            this.Property(p => p.CZO_ND30).HasMaxLength(100);
            this.Property(p => p.TOB_ND10).HasMaxLength(100);
            this.Property(p => p.VAN_ND30).HasMaxLength(100);
            this.Property(p => p.IPM_ND10).HasMaxLength(100);
            this.Property(p => p.LVX_ND5).HasMaxLength(100);
            this.Property(p => p.OFX_ND5).HasMaxLength(100);
            this.Property(p => p.DOX_ND30).HasMaxLength(100);
            this.Property(p => p.AMK_NM).HasMaxLength(100);
            this.Property(p => p.AMC_NM).HasMaxLength(100);
            this.Property(p => p.AZM_NM).HasMaxLength(100);
            this.Property(p => p.AMP_NM).HasMaxLength(100);
            this.Property(p => p.SAM_NM).HasMaxLength(100);
            this.Property(p => p.ATM_NM).HasMaxLength(100);
            this.Property(p => p.OXA_NM).HasMaxLength(100);
            this.Property(p => p.POL_NM).HasMaxLength(100);
            this.Property(p => p.NIT_NM).HasMaxLength(100);
            this.Property(p => p.SXT_NM).HasMaxLength(100);
            this.Property(p => p.STH_NM).HasMaxLength(100);
            this.Property(p => p.GEH_NM).HasMaxLength(100);
            this.Property(p => p.ERY_NM).HasMaxLength(100);
            this.Property(p => p.CIP_NM).HasMaxLength(100);
            this.Property(p => p.CLI_NM).HasMaxLength(100);
            this.Property(p => p.RIF_NM).HasMaxLength(100);
            this.Property(p => p.LNZ_NM).HasMaxLength(100);
            this.Property(p => p.STR_NM).HasMaxLength(100);
            this.Property(p => p.FOS_NM).HasMaxLength(100);
            this.Property(p => p.CHL_NM).HasMaxLength(100);
            this.Property(p => p.MEM_NM).HasMaxLength(100);
            this.Property(p => p.MNO_NM).HasMaxLength(100);
            this.Property(p => p.MFX_NM).HasMaxLength(100);
            this.Property(p => p.PIP_NM).HasMaxLength(100);
            this.Property(p => p.TZP_NM).HasMaxLength(100);
            this.Property(p => p.PEN_NM).HasMaxLength(100);
            this.Property(p => p.GEN_NM).HasMaxLength(100);
            this.Property(p => p.PEN_NE).HasMaxLength(100);
            this.Property(p => p.TCY_NM).HasMaxLength(100);
            this.Property(p => p.TCC_NM).HasMaxLength(100);
            this.Property(p => p.TIC_NM).HasMaxLength(100);
            this.Property(p => p.TEC_NM).HasMaxLength(100);
            this.Property(p => p.TGC_NM).HasMaxLength(100);
            this.Property(p => p.FEP_NM).HasMaxLength(100);
            this.Property(p => p.CXM_NM).HasMaxLength(100);
            this.Property(p => p.CEC_NM).HasMaxLength(100);
            this.Property(p => p.CFP_NM).HasMaxLength(100);
            this.Property(p => p.CSL_NM).HasMaxLength(100);
            this.Property(p => p.CRO_NM).HasMaxLength(100);
            this.Property(p => p.CTX_NM).HasMaxLength(100);
            this.Property(p => p.CAZ_NM).HasMaxLength(100);
            this.Property(p => p.FOX_NM).HasMaxLength(100);
            this.Property(p => p.CZO_NM).HasMaxLength(100);
            this.Property(p => p.TOB_NM).HasMaxLength(100);
            this.Property(p => p.VAN_NM).HasMaxLength(100);
            this.Property(p => p.VAN_NE).HasMaxLength(100);
            this.Property(p => p.IPM_NM).HasMaxLength(100);
            this.Property(p => p.LVX_NM).HasMaxLength(100);
            this.Property(p => p.CTX_NE).HasMaxLength(100);
            this.Property(p => p.CSL_ND75).HasMaxLength(100);
            this.Property(p => p.ETP_ND10).HasMaxLength(100);
            this.Property(p => p.ETP_NM).HasMaxLength(100);
            this.Property(p => p.CTT_ND30).HasMaxLength(100);
            this.Property(p => p.CTT_NM).HasMaxLength(100);
            this.Property(p => p.DOR_ND10).HasMaxLength(100);
            this.Property(p => p.DOR_NM).HasMaxLength(100);
            this.Property(p => p.NET_ND30).HasMaxLength(100);
            this.Property(p => p.NET_NM).HasMaxLength(100);
            this.Property(p => p.QDA_ND15).HasMaxLength(100);
            this.Property(p => p.QDA_NM).HasMaxLength(100);

            this.Property(p => p.SPEC_REAS).HasMaxLength(100);
            this.Property(p => p.COMMENT).HasMaxLength(100);

            this.Property(p => p.CPT_ND30).HasMaxLength(100);
            this.Property(p => p.CPT_NM).HasMaxLength(100);
            this.Property(p => p.CPT_NE).HasMaxLength(100);
            this.Property(p => p.CZA_ND30).HasMaxLength(100);
            this.Property(p => p.CZA_NM).HasMaxLength(100);
            this.Property(p => p.CZA_NE).HasMaxLength(100);
            this.Property(p => p.AZA_ND30).HasMaxLength(100);
            this.Property(p => p.AZA_NM).HasMaxLength(100);
            this.Property(p => p.AZA_NE).HasMaxLength(100);
            this.Property(p => p.CZT_ND30).HasMaxLength(100);
            this.Property(p => p.CZT_NM).HasMaxLength(100);
            this.Property(p => p.CZT_NE).HasMaxLength(100);
            //this.Property(p => p.TGC_NE).HasMaxLength(100);//新增字段
            this.Property(p => p.DOX_NM).HasMaxLength(100);//新增字段
            this.Property(p => p.INDUC_CLI).HasMaxLength(100);//新增字段
            this.Property(p => p.CARBAPENEM).HasMaxLength(100);//新增字段
            this.Property(p => p.COL_ND10).HasMaxLength(100);
            this.Property(p => p.COL_NM).HasMaxLength(100);
            this.Property(p => p.COL_NE).HasMaxLength(100);
            this.Property(p => p.AMC_NE).HasMaxLength(100);
            this.Property(p => p.AMK_NE).HasMaxLength(100);
            this.Property(p => p.AMP_NE).HasMaxLength(100);
            this.Property(p => p.ATM_NE).HasMaxLength(100);
            this.Property(p => p.AZM_NE).HasMaxLength(100);
            this.Property(p => p.CAZ_NE).HasMaxLength(100);
            this.Property(p => p.CEC_NE).HasMaxLength(100);
            this.Property(p => p.CFP_NE).HasMaxLength(100);
            this.Property(p => p.CHL_NE).HasMaxLength(100);
            this.Property(p => p.CIP_NE).HasMaxLength(100);
            this.Property(p => p.CLI_NE).HasMaxLength(100);
            this.Property(p => p.CRO_NE).HasMaxLength(100);
            this.Property(p => p.CTT_NE).HasMaxLength(100);
            this.Property(p => p.CXM_NE).HasMaxLength(100);
            this.Property(p => p.CZO_NE).HasMaxLength(100);
            this.Property(p => p.DOR_NE).HasMaxLength(100);
            this.Property(p => p.DOX_NE).HasMaxLength(100);
            this.Property(p => p.ERY_NE).HasMaxLength(100);
            this.Property(p => p.ETP_NE).HasMaxLength(100);
            this.Property(p => p.FEP_NE).HasMaxLength(100);
            this.Property(p => p.FOS_NE).HasMaxLength(100);
            this.Property(p => p.FOX_NE).HasMaxLength(100);
            this.Property(p => p.GEN_NE).HasMaxLength(100);
            this.Property(p => p.IPM_NE).HasMaxLength(100);
            this.Property(p => p.LNZ_NE).HasMaxLength(100);
            this.Property(p => p.LVX_NE).HasMaxLength(100);
            this.Property(p => p.MEM_NE).HasMaxLength(100);
            this.Property(p => p.MFX_NE).HasMaxLength(100);
            this.Property(p => p.MNO_NE).HasMaxLength(100);
            this.Property(p => p.NET_NE).HasMaxLength(100);
            this.Property(p => p.NIT_NE).HasMaxLength(100);
            this.Property(p => p.OXA_NE).HasMaxLength(100);
            this.Property(p => p.PIP_NE).HasMaxLength(100);
            this.Property(p => p.POL_NE).HasMaxLength(100);
            this.Property(p => p.QDA_NE).HasMaxLength(100);
            this.Property(p => p.RIF_NE).HasMaxLength(100);
            this.Property(p => p.SAM_NE).HasMaxLength(100);
            this.Property(p => p.STH_NE).HasMaxLength(100);
            this.Property(p => p.STR_NE).HasMaxLength(100);
            this.Property(p => p.SXT_NE).HasMaxLength(100);
            this.Property(p => p.TCC_NE).HasMaxLength(100);
            this.Property(p => p.TCY_NE).HasMaxLength(100);
            this.Property(p => p.TEC_NE).HasMaxLength(100);
            this.Property(p => p.TGC_NE).HasMaxLength(100);
            this.Property(p => p.TIC_NE).HasMaxLength(100);
            this.Property(p => p.TOB_NE).HasMaxLength(100);
            this.Property(p => p.TZP_NE).HasMaxLength(100);

        }

    }
}
