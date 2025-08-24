using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
    /// 实体类 ，数据库表名：MedicalAntibioticResult 
    /// </summary>
    public partial class MedicalAntibioticResult : BaseEntity
    {

        /// <summary>
        /// 所属医学数据Id，关联MedicalData表
        /// <summary>
        public long MedicalDataId { get; set; }

        /// <summary>
        /// 医学数据项Id，关联MedicalDataItem表
        /// <summary>
        public long MedicalDataItemId { get; set; }

        /// <summary>
        /// 细菌Id，关联细菌表
        /// <summary>
        public long OrganismId { get; set; }
        /// <summary>
        /// 细菌名称，关联细菌表
        /// <summary>
        public String OrganismName { get; set; }
        /// <summary>
        /// 细菌编码，关联细菌表
        /// <summary>
        public String OrganismCode { get; set; }

        /// <summary>
        /// 所属会员Id
        /// <summary>
        public long MemberId { get; set; }

        /// <summary>
        /// 排序字段，相对于每次医学数据，不是全局的排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 该行数据是否有效，目前已知ORGANISM字段不再范围则无效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 细菌类型（+：革兰阳性菌、-：革兰阴性菌）
        /// <summary>
        public String  ORG_TYPE { get; set; }

        /// <summary>
        /// 阿米卡星(KB法)  （0：无效值  1：敏感   2：中介   3：耐药） 下面所有的抗生素都一样
        /// <summary>
        public int AMK_ND30 { get; set; }
        /// <summary>
        /// 阿莫西林(KB法)
        /// <summary>
        public int AMX_ND25 { get; set; }
        /// <summary>
        /// 阿莫西林/克拉维酸(KB法)
        /// <summary>
        public int AMC_ND20 { get; set; }
        /// <summary>
        /// 阿奇霉素(KB法)
        /// <summary>
        public int AZM_ND15 { get; set; }
        /// <summary>
        /// 氨苄西林(KB法)
        /// <summary>
        public int AMP_ND10 { get; set; }
        /// <summary>
        /// 氨苄西林/舒巴坦(KB法)
        /// <summary>
        public int SAM_ND10 { get; set; }
        /// <summary>
        /// 氨曲南(KB法)
        /// <summary>
        public int ATM_ND30 { get; set; }
        /// <summary>
        /// 苯唑西林(KB法)
        /// <summary>
        public int OXA_ND1 { get; set; }
        /// <summary>
        /// 多粘菌素B(KB法)
        /// <summary>
        public int POL_ND300 { get; set; }
        /// <summary>
        /// 呋喃妥因(KB法)
        /// <summary>
        public int NIT_ND300 { get; set; }
        /// <summary>
        /// 复方新诺明(KB法)
        /// <summary>
        public int SXT_ND1_2 { get; set; }
        /// <summary>
        /// 高浓度链霉素(KB法)
        /// <summary>
        public int STH_ND300 { get; set; }
        /// <summary>
        /// 高浓度庆大霉素(KB法)
        /// <summary>
        public int GEH_ND120 { get; set; }
        /// <summary>
        /// 红霉素(KB法)
        /// <summary>
        public int ERY_ND15 { get; set; }
        /// <summary>
        /// 环丙沙星(KB法)
        /// <summary>
        public int CIP_ND5 { get; set; }
        /// <summary>
        /// 甲氧西林(KB法)
        /// <summary>
        public int MET_ND5 { get; set; }
        /// <summary>
        /// 克林霉素(KB法)
        /// <summary>
        public int CLI_ND2 { get; set; }
        /// <summary>
        /// 利福平(KB法)
        /// <summary>
        public int RIF_ND5 { get; set; }
        /// <summary>
        /// 利奈唑胺(KB法)
        /// <summary>
        public int LNZ_ND30 { get; set; }
        /// <summary>
        /// 链霉素(KB法)
        /// <summary>
        public int STR_ND10 { get; set; }
        /// <summary>
        /// 磷霉素(KB法)
        /// <summary>
        public int FOS_ND200 { get; set; }
        /// <summary>
        /// 氯霉素(KB法)
        /// <summary>
        public int CHL_ND30 { get; set; }
        /// <summary>
        /// 美洛培南(KB法)
        /// <summary>
        public int MEM_ND10 { get; set; }
        /// <summary>
        /// 米诺环素(KB法)
        /// <summary>
        public int MNO_ND30 { get; set; }
        /// <summary>
        /// 莫西沙星(KB法)
        /// <summary>
        public int MFX_ND5 { get; set; }
        /// <summary>
        /// 哌拉西林(KB法)
        /// <summary>
        public int PIP_ND100 { get; set; }
        /// <summary>
        /// 哌拉西林/他唑巴坦(KB法)
        /// <summary>
        public int TZP_ND100 { get; set; }
        /// <summary>
        /// 青霉素G(KB法)
        /// <summary>
        public int PEN_ND10 { get; set; }
        /// <summary>
        /// 庆大霉素(KB法)
        /// <summary>
        public int GEN_ND10 { get; set; }
        /// <summary>
        /// 四环素(KB法)
        /// <summary>
        public int TCY_ND30 { get; set; }
        /// <summary>
        /// 替卡西林/克拉维酸(KB法)
        /// <summary>
        public int TCC_ND75 { get; set; }
        /// <summary>
        /// 替卡西林(KB法)
        /// <summary>
        public int TIC_ND75 { get; set; }
        /// <summary>
        /// 替考拉宁(KB法)
        /// <summary>
        public int TEC_ND30 { get; set; }
        /// <summary>
        /// 替加环素(KB法)
        /// <summary>
        public int TGC_ND15 { get; set; }
        /// <summary>
        /// 头孢吡肟(KB法)
        /// <summary>
        public int FEP_ND30 { get; set; }
        /// <summary>
        /// 头孢呋辛(KB法)
        /// <summary>
        public int CXM_ND30 { get; set; }
        /// <summary>
        /// 头孢克洛(KB法)
        /// <summary>
        public int CEC_ND30 { get; set; }
        /// <summary>
        /// 头孢哌酮(KB法)
        /// <summary>
        public int CFP_ND75 { get; set; }
        /// <summary>
        /// 头孢哌酮/舒巴坦(KB法)
        /// <summary>
        public int CSL_ND30 { get; set; }
        /// <summary>
        /// 头孢曲松(KB法)
        /// <summary>
        public int CRO_ND30 { get; set; }
        /// <summary>
        /// 头孢噻肟(KB法)
        /// <summary>
        public int CTX_ND30 { get; set; }
        /// <summary>
        /// 头孢他啶(KB法)
        /// <summary>
        public int CAZ_ND30 { get; set; }
        /// <summary>
        /// 头孢西丁(KB法)
        /// <summary>
        public int FOX_ND30 { get; set; }
        /// <summary>
        /// 头孢唑啉(KB法)
        /// <summary>
        public int CZO_ND30 { get; set; }
        /// <summary>
        /// 妥布霉素(KB法)
        /// <summary>
        public int TOB_ND10 { get; set; }
        /// <summary>
        /// 万古霉素(KB法)
        /// <summary>
        public int VAN_ND30 { get; set; }
        /// <summary>
        /// 亚胺培南(KB法)
        /// <summary>
        public int IPM_ND10 { get; set; }
        /// <summary>
        /// 左旋氧氟沙星(KB法)
        /// <summary>
        public int LVX_ND5 { get; set; }
        /// <summary>
        /// 磺胺类(KB法)
        /// <summary>
        public int SSS_ND200 { get; set; }
        /// <summary>
        /// 头孢噻吩(KB法)
        /// <summary>
        public int CEP_ND30 { get; set; }
        /// <summary>
        /// 羧苄西林(KB法)
        /// <summary>
        public int CRB_ND100 { get; set; }
        /// <summary>
        /// 氧氟沙星(KB法)
        /// <summary>
        public int OFX_ND5 { get; set; }
        /// <summary>
        /// 头孢唑肟(KB法)
        /// <summary>
        public int CZX_ND30 { get; set; }
        /// <summary>
        /// 美洛西林(KB法)
        /// <summary>
        public int MEZ_ND75 { get; set; }
        /// <summary>
        /// 诺氟沙星(KB法)
        /// <summary>
        public int NOR_ND10 { get; set; }
        /// <summary>
        /// 头孢孟多(KB法)
        /// <summary>
        public int MAN_ND30 { get; set; }
        /// <summary>
        /// 多西环素(KB法)
        /// <summary>
        public int DOX_ND30 { get; set; }
        /// <summary>
        /// 新生霉素(KB法)
        /// <summary>
        public int NOV_ND5 { get; set; }
        /// <summary>
        /// 阿米卡星(MIC法)
        /// <summary>
        public int AMK_NM { get; set; }
        /// <summary>
        /// 阿莫西林(MIC法)
        /// <summary>
        public int AMX_NM { get; set; }
        /// <summary>
        /// 阿莫西林/克拉维酸(MIC法)
        /// <summary>
        public int AMC_NM { get; set; }
        /// <summary>
        /// 阿奇霉素(MIC法)
        /// <summary>
        public int AZM_NM { get; set; }
        /// <summary>
        /// 氨苄西林(MIC法)
        /// <summary>
        public int AMP_NM { get; set; }
        /// <summary>
        /// 氨苄西林/舒巴坦(MIC法)
        /// <summary>
        public int SAM_NM { get; set; }
        /// <summary>
        /// 氨曲南(MIC法)
        /// <summary>
        public int ATM_NM { get; set; }
        /// <summary>
        /// 苯唑西林(MIC法)
        /// <summary>
        public int OXA_NM { get; set; }
        /// <summary>
        /// 多粘菌素B(MIC法)
        /// <summary>
        public int POL_NM { get; set; }
        /// <summary>
        /// 呋喃妥因(MIC法)
        /// <summary>
        public int NIT_NM { get; set; }
        /// <summary>
        /// 复方新诺明(MIC法)
        /// <summary>
        public int SXT_NM { get; set; }
        /// <summary>
        /// 高浓度链霉素(MIC法)
        /// <summary>
        public int STH_NM { get; set; }
        /// <summary>
        /// 高浓度庆大霉素(MIC法)
        /// <summary>
        public int GEH_NM { get; set; }
        /// <summary>
        /// 红霉素(MIC法)
        /// <summary>
        public int ERY_NM { get; set; }
        /// <summary>
        /// 环丙沙星(MIC法) 
        /// <summary>
        public int CIP_NM { get; set; }
        /// <summary>
        /// 甲氧西林(MIC法)
        /// <summary>
        public int MET_NM { get; set; }
        /// <summary>
        /// 克林霉素(MIC法)
        /// <summary>
        public int CLI_NM { get; set; }
        /// <summary>
        /// 利福平(MIC法)
        /// <summary>
        public int RIF_NM { get; set; }
        /// <summary>
        /// 利奈唑胺(MIC法)
        /// <summary>
        public int LNZ_NM { get; set; }
        /// <summary>
        /// 链霉素(MIC法)
        /// <summary>
        public int STR_NM { get; set; }
        /// <summary>
        /// 磷霉素(MIC法)
        /// <summary>
        public int FOS_NM { get; set; }
        /// <summary>
        /// 氯霉素(MIC法)
        /// <summary>
        public int CHL_NM { get; set; }
        /// <summary>
        /// 美洛培南(MIC法)
        /// <summary>
        public int MEM_NM { get; set; }
        /// <summary>
        /// 米诺环素(MIC法)
        /// <summary>
        public int MNO_NM { get; set; }
        /// <summary>
        /// 莫西沙星(MIC法)
        /// <summary>
        public int MFX_NM { get; set; }
        /// <summary>
        /// 哌拉西林(MIC法)
        /// <summary>
        public int PIP_NM { get; set; }
        /// <summary>
        /// 哌拉西林/他唑巴坦(MIC法)
        /// <summary>
        public int TZP_NM { get; set; }
        /// <summary>
        /// 青霉素G(MIC法)
        /// <summary>
        public int PEN_NM { get; set; }
        /// <summary>
        /// 庆大霉素(MIC法)
        /// <summary>
        public int GEN_NM { get; set; }
        /// <summary>
        /// 青霉素G(Etest法)
        /// <summary>
        public int PEN_NE { get; set; }
        /// <summary>
        /// 四环素(MIC法)
        /// <summary>
        public int TCY_NM { get; set; }
        /// <summary>
        /// 替卡西林/克拉维酸(MIC法)
        /// <summary>
        public int TCC_NM { get; set; }
        /// <summary>
        /// 替卡西林(MIC法)
        /// <summary>
        public int TIC_NM { get; set; }
        /// <summary>
        /// 替考拉宁(MIC法)
        /// <summary>
        public int TEC_NM { get; set; }
        /// <summary>
        /// 替加环素(MIC法)
        /// <summary>
        public int TGC_NM { get; set; }
        /// <summary>
        /// 头孢吡肟(MIC法)
        /// <summary>
        public int FEP_NM { get; set; }
        /// <summary>
        /// 头孢呋辛(MIC法)
        /// <summary>
        public int CXM_NM { get; set; }
        /// <summary>
        /// 头孢克洛(MIC法)
        /// <summary>
        public int CEC_NM { get; set; }
        /// <summary>
        /// 头孢哌酮(MIC法)
        /// <summary>
        public int CFP_NM { get; set; }
        /// <summary>
        /// 头孢哌酮/舒巴坦(MIC法)
        /// <summary>
        public int CSL_NM { get; set; }
        /// <summary>
        /// 头孢曲松(MIC法)
        /// <summary>
        public int CRO_NM { get; set; }
        /// <summary>
        /// 头孢噻肟(MIC法)
        /// <summary>
        public int CTX_NM { get; set; }
        /// <summary>
        /// 头孢他啶(MIC法)
        /// <summary>
        public int CAZ_NM { get; set; }
        /// <summary>
        /// 头孢西丁(MIC法)
        /// <summary>
        public int FOX_NM { get; set; }
        /// <summary>
        /// 头孢唑啉(MIC法)
        /// <summary>
        public int CZO_NM { get; set; }
        /// <summary>
        /// 妥布霉素(MIC法)
        /// <summary>
        public int TOB_NM { get; set; }
        /// <summary>
        /// 万古霉素(MIC法)
        /// <summary>
        public int VAN_NM { get; set; }
        /// <summary>
        /// 万古霉素(Etest法)
        /// <summary>
        public int VAN_NE { get; set; }
        /// <summary>
        /// 亚胺培南(MIC法)
        /// <summary>
        public int IPM_NM { get; set; }
        /// <summary>
        /// 左旋氧氟沙星(MIC法)
        /// <summary>
        public int LVX_NM { get; set; }
        /// <summary>
        /// 头孢噻肟(Etest法)
        /// <summary>
        public int CTX_NE { get; set; }
        /// <summary>
        /// 头孢哌酮/舒巴坦(KB法)
        /// <summary>
        public int CSL_ND75 { get; set; }
        /// <summary>
        /// 阿莫西林(KB法)
        /// <summary>
        public int AMX_ND30 { get; set; }
        /// <summary>
        /// 厄他培南(KB法)
        /// <summary>
        public int ETP_ND10 { get; set; }
        /// <summary>
        /// 厄他培南(MIC法)
        /// <summary>
        public int ETP_NM { get; set; }
        /// <summary>
        /// 头孢替坦(KB法)
        /// <summary>
        public int CTT_ND30 { get; set; }
        /// <summary>
        /// 头孢替坦(MIC法)
        /// <summary>
        public int CTT_NM { get; set; }
        /// <summary>
        /// 多尼培南(KB法)
        /// <summary>
        public int DOR_ND10 { get; set; }
        /// <summary>
        /// 多尼培南(MIC法)
        /// <summary>
        public int DOR_NM { get; set; }
        /// <summary>
        /// 奈替米星(KB法)
        /// <summary>
        public int NET_ND30 { get; set; }
        /// <summary>
        /// 奈替米星(MIC法)
        /// <summary>
        public int NET_NM { get; set; }
        /// <summary>
        /// 奎奴普丁/达福普汀(KB法)
        /// <summary>
        public int QDA_ND15 { get; set; }
        /// <summary>
        /// 奎奴普丁/达福普汀(MIC法)
        /// <summary>
        public int QDA_NM { get; set; }
        /// <summary>
        /// 该行数据在原始上传文件中的行索引
        /// <summary>
        public Int32 UploadRowIndex { get; set; }
        /// <summary>
        /// SPEC_REAS
        /// <summary>
        public int SPEC_REAS { get; set; }
        /// <summary>
        /// COMMENT
        /// <summary>
        public int COMMENT { get; set; }
        /// <summary>
        /// CCV_NM
        /// <summary>
        public int CCV_NM { get; set; }
        /// <summary>
        /// CTC_NM
        /// <summary>
        public int CTC_NM { get; set; }
        /// <summary>
        /// MFX_ND
        /// <summary>
        public int MFX_ND { get; set; }
        /// <summary>
        /// DAP_NM
        /// <summary>
        public int DAP_NM { get; set; }

        /// <summary>
        /// CPT_ND30
        /// <summary>
        public int CPT_ND30 { get; set; }

        /// <summary>
        /// CPT_NM
        /// <summary>
        public int CPT_NM { get; set; }

        /// <summary>
        /// CPT_NE
        /// <summary>
        public int CPT_NE { get; set; }

        /// <summary>
        /// CZA_ND30
        /// <summary>
        public int CZA_ND30 { get; set; }

        /// <summary>
        /// CZA_NM
        /// <summary>
        public int CZA_NM { get; set; }

        /// <summary>
        /// CZA_NE
        /// <summary>
        public int CZA_NE { get; set; }

        /// <summary>
        /// AZA_ND30
        /// <summary>
        public int AZA_ND30 { get; set; }

        /// <summary>
        /// AZA_NM
        /// <summary>
        public int AZA_NM { get; set; }

        /// <summary>
        /// AZA_NE
        /// <summary>
        public int AZA_NE { get; set; }

        /// <summary>
        /// CZT_ND30
        /// <summary>
        public int CZT_ND30 { get; set; }

        /// <summary>
        ///CZT_NM
        /// <summary>
        public int CZT_NM { get; set; }

        /// <summary>
        /// CZT_NE
        /// <summary>
        public int CZT_NE { get; set; }

        /// <summary>
        /// 新增字段
        /// TGC_NE
        /// </summary>
       // public int TGC_NE { get; set; }

        /// <summary>
        /// 新增字段
        /// DOX_NM
        /// </summary>
        public int DOX_NM { get; set; }
        /// <summary>
        /// 新增字段
        /// CARBAPENEM
        /// </summary>
        public String CARBAPENEM { get; set; }
        /// <summary>
        /// 新增字段
        /// INDUC_CLI
        /// </summary>
        public String INDUC_CLI { get; set; }

        public int  COL_ND10 { get; set; }
        public int  COL_NM { get; set; }
        public int  COL_NE { get; set; }
        public int  AMC_NE { get; set; }
        public int  AMK_NE { get; set; }
        public int  AMP_NE { get; set; }
        public int  ATM_NE { get; set; }
        public int  AZM_NE { get; set; }
        public int  CAZ_NE { get; set; }
        public int  CEC_NE { get; set; }
        public int  CFP_NE { get; set; }
        public int  CHL_NE { get; set; }
        public int  CIP_NE { get; set; }
        public int  CLI_NE { get; set; }
        public int  CRO_NE { get; set; }
        public int  CTT_NE { get; set; }
        public int  CXM_NE { get; set; }
        public int  CZO_NE { get; set; }
        public int  DOR_NE { get; set; }
        public int  DOX_NE { get; set; }
        public int  ERY_NE { get; set; }
        public int  ETP_NE { get; set; }
        public int  FEP_NE { get; set; }
        public int  FOS_NE { get; set; }
        public int  FOX_NE { get; set; }
        public int  GEN_NE { get; set; }
        public int  IPM_NE { get; set; }
        public int  LNZ_NE { get; set; }
        public int  LVX_NE { get; set; }
        public int  MEM_NE { get; set; }
        public int  MFX_NE { get; set; }
        public int  MNO_NE { get; set; }
        public int  NET_NE { get; set; }
        public int  NIT_NE { get; set; }
        public int  OXA_NE { get; set; }
        public int  PIP_NE { get; set; }
        public int  POL_NE { get; set; }
        public int  QDA_NE { get; set; }
        public int  RIF_NE { get; set; }
        public int  SAM_NE { get; set; }
        public int  STH_NE { get; set; }
        public int  STR_NE { get; set; }
        public int  SXT_NE { get; set; }
        public int  TCC_NE { get; set; }
        public int  TCY_NE { get; set; }
        public int  TEC_NE { get; set; }
        public int  TGC_NE { get; set; }
        public int  TIC_NE { get; set; }
        public int  TOB_NE { get; set; }
        public int  TZP_NE { get; set; }

        private int _ERV_NM = 0;
        private int _ERV_ND20 = 0;
        private int _ERV_NE = 0;

        /// <summary>
        /// 依拉环素（MIC法）
        /// </summary>
        public int ERV_NM { get{ return _ERV_NM; } set{ _ERV_NM = value; } }
        /// <summary>
        /// 依拉环素（KB法）
        /// </summary>
        public int ERV_ND20 { get{ return _ERV_ND20; } set{ _ERV_ND20 = value; } }
        /// <summary>
        /// 依拉环素（Etest法）
        /// </summary>
        public int ERV_NE { get{ return _ERV_NE; } set{ _ERV_NE = value; } }

#if false
        /// <summary>
        /// 诱导克林霉素耐药试验D试验<br />
        /// +表示阳性，-表示阴性<br />
        /// P、POS替换为+，N、NEG替换为-<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public int  INDUC_CLI { get; set; }
        /// <summary>
        /// 碳青霉烯酶<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public int  CARBAPENEM { get; set; }

        /// <summary>
        /// 改良碳青霉烯灭活试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public int  MCIM { get; set; }

        /// <summary>
        /// EDTA碳青霉烯灭活试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public int  ECIM { get; set; }

        /// <summary>
        /// 碳青霉烯酶基因型<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public int  CARBGENE { get; set; }

        /// <summary>
        /// 诊断<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public int  DIAGNOSIS { get; set; }

        /// <summary>
        /// 头孢美唑纸片法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum CMZ_ND30 { get; set; }

        /// <summary>
        /// 氧氟沙星MIC法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum OFX_NM { get; set; }

        /// <summary>
        /// 多黏菌素E纸片法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum COL_ND10 { get; set; }

        /// <summary>
        /// 多黏菌素E的MIC法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum COL_NM { get; set; }

        /// <summary>
        /// 多黏菌素E的E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum COL_NE { get; set; }

        /// <summary>
        /// 头孢曲松E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum CRO_NE { get; set; }

        /// <summary>
        /// 厄他培南E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum ETP_NE { get; set; }

        /// <summary>
        /// 亚胺培南E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum IPM_NE { get; set; }

        /// <summary>
        /// 利奈唑胺E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum LNZ_NE { get; set; }

        /// <summary>
        /// 美罗培南E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum MEM_NE { get; set; }

        /// <summary>
        /// 多黏菌素B的E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum POL_NE { get; set; }

        /// <summary>
        /// 替考拉宁E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum TEC_NE { get; set; }

        /// <summary>
        /// 替加环素E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public MedicalAntibioticResultValueEnum TGC_NE { get; set; }
#endif
    }
}
