using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services
{
    class 修改验证规则和上传记录
    {
        //先备份原有的数据库

        /*
         1、MedicalData 表添加2个字段，AntibioticResultStatue、AntibioticResultTime
         
-- 添加字段 
ALTER TABLE MedicalData ADD AntibioticResultStatue  INT NOT NULL  DEFAULT (0) 
ALTER TABLE MedicalData ADD AntibioticResultTime  DATETIME NOT NULL  DEFAULT ('1900-01-01 00:00') 

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传数据耐药生成结果状态，1：待处理    2：处理中   3：成功  4：失败' , @level0type=N'SCHEMA',@level0name=N'dbo', 
@level1type=N'TABLE',@level1name=N'MedicalData', @level2type=N'COLUMN',@level2name=N'AntibioticResultStatue'  


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上传数据耐药生成结果时间，也就是生成结果状态的处理时间' , @level0type=N'SCHEMA',@level0name=N'dbo', 
@level1type=N'TABLE',@level1name=N'MedicalData', @level2type=N'COLUMN',@level2name=N'AntibioticResultTime' 
         
        UPDATE MedicalData SET AntibioticResultStatue=3,AntibioticResultTime=GETDATE()


ALTER TABLE MedicalDataItem ADD CPT_ND30  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CPT_NM  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CPT_NE  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CZA_ND30  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CZA_NM  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CZA_NE  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD AZA_ND30  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD AZA_NM  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD AZA_NE  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CZT_ND30  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CZT_NM  nvarchar(100) 
ALTER TABLE MedicalDataItem ADD CZT_NE  nvarchar(100) 

UPDATE MedicalDataItem SET  CPT_ND30='',	CPT_NM='',	CPT_NE='',	CZA_ND30='',	CZA_NM='',	CZA_NE='',
	AZA_ND30='',	AZA_NM='',	AZA_NE='',	CZT_ND30='',	CZT_NM='',	CZT_NE=''


        -------------

        ALTER TABLE MedicalAntibioticResult ADD CPT_ND30  int 
ALTER TABLE MedicalAntibioticResult ADD CPT_NM  int 
ALTER TABLE MedicalAntibioticResult ADD CPT_NE  int 
ALTER TABLE MedicalAntibioticResult ADD CZA_ND30  int 
ALTER TABLE MedicalAntibioticResult ADD CZA_NM  int 
ALTER TABLE MedicalAntibioticResult ADD CZA_NE  int 
ALTER TABLE MedicalAntibioticResult ADD AZA_ND30  int 
ALTER TABLE MedicalAntibioticResult ADD AZA_NM  int 
ALTER TABLE MedicalAntibioticResult ADD AZA_NE  int 
ALTER TABLE MedicalAntibioticResult ADD CZT_ND30  int 
ALTER TABLE MedicalAntibioticResult ADD CZT_NM  int 
ALTER TABLE MedicalAntibioticResult ADD CZT_NE  int 

UPDATE MedicalAntibioticResult SET  CPT_ND30='0',	CPT_NM='0',	CPT_NE='0',	CZA_ND30='0',	CZA_NM='0',	CZA_NE='0',
	AZA_ND30='0',	AZA_NM='0',	AZA_NE='0',	CZT_ND30='0',	CZT_NM='0',	CZT_NE='0'


        -- 添加字段的默认值
          ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR INSTITUT
          ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR PAT_TYPE
          ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ORIGIN
          ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FULL_NAME
         ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR COUNTRY_A
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR LABORATORY
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR PATIENT_ID
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FIRST_NAME
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR LAST_NAME
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SEX
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AGE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR DATE_BIRTH
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR WARD
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR WARD_TYPE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR DEPARTMENT
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SPEC_NUM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SPEC_DATE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SPEC_TYPE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SPEC_CODE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ORGANISM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ORG_TYPE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ESBL
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR BETA_LACT
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AMK_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AMC_ND20
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AZM_ND15
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AMP_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SAM_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ATM_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR OXA_ND1
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR POL_ND300
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR NIT_ND300
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SXT_ND1_2
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR STH_ND300
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR GEH_ND120
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ERY_ND15
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CIP_ND5
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CLI_ND2
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR RIF_ND5
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR LNZ_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR STR_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FOS_ND200
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CHL_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR MEM_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR MNO_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR MFX_ND5
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR PIP_ND100
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TZP_ND100
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR PEN_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR GEN_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TCY_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TCC_ND75
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TIC_ND75
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TEC_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TGC_ND15
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FEP_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CXM_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CEC_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CFP_ND75
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CSL_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CRO_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CTX_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CAZ_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FOX_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZO_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TOB_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR VAN_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR IPM_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR LVX_ND5
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR OFX_ND5
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR DOX_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AMK_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AMC_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AZM_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AMP_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SAM_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ATM_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR OXA_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR POL_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR NIT_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR SXT_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR STH_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR GEH_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ERY_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CIP_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CLI_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR RIF_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR LNZ_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR STR_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FOS_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CHL_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR MEM_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR MNO_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR MFX_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR PIP_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TZP_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR PEN_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR GEN_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR PEN_NE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TCY_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TCC_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TIC_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TEC_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TGC_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FEP_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CXM_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CEC_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CFP_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CSL_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CRO_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CTX_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CAZ_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR FOX_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZO_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR TOB_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR VAN_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR VAN_NE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR IPM_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR LVX_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CTX_NE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CSL_ND75
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ETP_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR ETP_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CTT_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CTT_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR DOR_ND10
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR DOR_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR NET_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR NET_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR QDA_ND15
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR QDA_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CPT_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CPT_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CPT_NE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZA_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZA_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZA_NE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AZA_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AZA_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR AZA_NE
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZT_ND30
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZT_NM
 ALTER TABLE [MedicalDataItem] ADD   DEFAULT '' FOR CZT_NE


        -- 处理过的文件地址
          ALTER TABLE MedicalData ADD DisposeFilePath  NVARCHAR(1000) NOT NULL  DEFAULT ('') 
            UPDATE MedicalData SET DisposeFilePath=''

        --
        
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMX_ND30
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR SPEC_REAS
	ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR COMMENT
	ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CCV_NM
	ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CTC_NM
	ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MFX_ND
	ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR DAP_NM
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MEZ_ND75
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR NOR_ND10
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MAN_ND30
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR NOV_ND5
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMX_NM
   ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MET_NM
  ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MET_ND5
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR SSS_ND200
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CEP_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CRB_ND100
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZX_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMX_ND25
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMK_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMC_ND20
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AZM_ND15
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMP_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR SAM_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR ATM_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR OXA_ND1
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR POL_ND300
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR NIT_ND300
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR SXT_ND1_2
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR STH_ND300
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR GEH_ND120
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR ERY_ND15
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CIP_ND5
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CLI_ND2
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR RIF_ND5
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR LNZ_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR STR_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR FOS_ND200
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CHL_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MEM_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MNO_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MFX_ND5
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR PIP_ND100
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TZP_ND100
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR PEN_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR GEN_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TCY_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TCC_ND75
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TIC_ND75
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TEC_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TGC_ND15
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR FEP_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CXM_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CEC_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CFP_ND75
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CSL_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CRO_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CTX_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CAZ_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR FOX_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZO_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TOB_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR VAN_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR IPM_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR LVX_ND5
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR OFX_ND5
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR DOX_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMK_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMC_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AZM_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AMP_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR SAM_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR ATM_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR OXA_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR POL_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR NIT_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR SXT_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR STH_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR GEH_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR ERY_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CIP_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CLI_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR RIF_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR LNZ_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR STR_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR FOS_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CHL_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MEM_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MNO_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR MFX_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR PIP_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TZP_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR PEN_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR GEN_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR PEN_NE
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TCY_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TCC_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TIC_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TEC_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TGC_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR FEP_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CXM_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CEC_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CFP_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CSL_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CRO_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CTX_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CAZ_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR FOX_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZO_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR TOB_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR VAN_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR VAN_NE
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR IPM_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR LVX_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CTX_NE
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CSL_ND75
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR ETP_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR ETP_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CTT_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CTT_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR DOR_ND10
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR DOR_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR NET_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR NET_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR QDA_ND15
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR QDA_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CPT_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CPT_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CPT_NE
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZA_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZA_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZA_NE
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AZA_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AZA_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR AZA_NE
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZT_ND30
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZT_NM
 ALTER TABLE MedicalAntibioticResult ADD   DEFAULT 0 FOR CZT_NE


         */



    }
}
