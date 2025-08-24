using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{

    /// <summary>
    /// 实体类 ，数据库表名：MedicalDataItem 
    /// </summary>
    public partial class MedicalDataItem : BaseEntity
    {

        /// <summary>
        /// 所属医学数据
        /// <summary>
        public long MedicalDataId { get; set; }

        /// <summary>
        /// 排序字段，相对于每次医学数据，不是全局的排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 该行数据是否有效，目前已知ORGANISM字段不再范围则无效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        ///细菌Id，关联细菌表，只有数据中的细菌编码正确，且数据库中才值
        /// </summary>
        public long OrganismId { get; set; }

        /// <summary>
        /// 细菌名称，关联细菌表，只有数据中的细菌编码正确，且数据库中才值
        /// <summary>
        public String OrganismName { get; set; }


        /// <summary>
        /// 国家代码
        /// <summary>
        public String COUNTRY_A { get; set; }
        /// <summary>
        /// 实验室代码
        /// <summary>
        public String LABORATORY { get; set; }
        /// <summary>
        /// 来源
        /// <summary>
        public String ORIGIN { get; set; }
        /// <summary>
        /// 病历号（住院病历必填）
        /// <summary>
        public String PATIENT_ID { get; set; }
        /// <summary>
        /// 用户姓（中国就直接使用该字段）
        /// <summary>
        public String FIRST_NAME { get; set; }
        /// <summary>
        /// 用户名
        /// <summary>
        public String LAST_NAME { get; set; }
        /// <summary>
        /// 姓名
        /// <summary>
        public String FULL_NAME { get; set; }
        /// <summary>
        /// 性别
        /// <summary>
        public String SEX { get; set; }
        /// <summary>
        /// 年龄
        /// <summary>
        public String AGE { get; set; }
        /// <summary>
        /// 年龄类型，有枚举
        /// <summary>
        public String PAT_TYPE { get; set; }
        /// <summary>
        /// 生日
        /// <summary>
        public String DATE_BIRTH { get; set; }
        /// <summary>
        /// 科室
        /// <summary>
        public String WARD { get; set; }
        /// <summary>
        /// 科室类别，关联科室类别表
        /// <summary>
        public String WARD_TYPE { get; set; }
        /// <summary>
        /// 医院代码
        /// <summary>
        public String INSTITUT { get; set; }
        /// <summary>
        /// 专业类别
        /// <summary>
        public String DEPARTMENT { get; set; }
        /// <summary>
        /// 标本编号
        /// <summary>
        public String SPEC_NUM { get; set; }
        /// <summary>
        /// 标本日期
        /// <summary>
        public String SPEC_DATE { get; set; }
        /// <summary>
        /// 标本种类缩写
        /// <summary>
        public String SPEC_TYPE { get; set; }
        /// <summary>
        /// 标本种类代码
        /// <summary>
        public String SPEC_CODE { get; set; }
        /// <summary>
        /// 细菌
        /// <summary>
        public String ORGANISM { get; set; }
        /// <summary>
        /// 细菌类型（+：革兰阳性菌、-：革兰阴性菌）
        /// <summary>
        public String ORG_TYPE { get; set; }

        /// <summary>
        /// 超广谱β-内酰胺酶
        /// <summary>
        public String ESBL { get; set; }
        /// <summary>
        /// 超广谱β-内酰胺酶
        /// <summary>
        public String BETA_LACT { get; set; }

        /// <summary>
        /// 新增字段未知意思
        /// </summary>
        public String INDUC_CLI { get; set; }

        /// <summary>
        /// 新增字段未知意思
        /// </summary>
        public String CARBAPENEM { get; set; }

        ///// <summary>
        ///// 新增字段未知意思
        ///// </summary>
        //public String COMMENT { get; set; }

        /// <summary>
        /// 阿米卡星(KB法)
        /// <summary>
        public String AMK_ND30 { get; set; }

        /// <summary>
        /// 阿莫西林/克拉维酸(KB法)
        /// <summary>
        public String AMC_ND20 { get; set; }
        /// <summary>
        /// 阿奇霉素(KB法)
        /// <summary>
        public String AZM_ND15 { get; set; }
        /// <summary>
        /// 氨苄西林(KB法)
        /// <summary>
        public String AMP_ND10 { get; set; }
        /// <summary>
        /// 氨苄西林/舒巴坦(KB法)
        /// <summary>
        public String SAM_ND10 { get; set; }
        /// <summary>
        /// 氨曲南(KB法)
        /// <summary>
        public String ATM_ND30 { get; set; }
        /// <summary>
        /// 苯唑西林(KB法)
        /// <summary>
        public String OXA_ND1 { get; set; }
        /// <summary>
        /// 多粘菌素B(KB法)
        /// <summary>
        public String POL_ND300 { get; set; }
        /// <summary>
        /// 呋喃妥因(KB法)
        /// <summary>
        public String NIT_ND300 { get; set; }
        /// <summary>
        /// 复方新诺明(KB法)
        /// <summary>
        public String SXT_ND1_2 { get; set; }
        /// <summary>
        /// 高浓度链霉素(KB法)
        /// <summary>
        public String STH_ND300 { get; set; }
        /// <summary>
        /// 高浓度庆大霉素(KB法)
        /// <summary>
        public String GEH_ND120 { get; set; }
        /// <summary>
        /// 红霉素(KB法)
        /// <summary>
        public String ERY_ND15 { get; set; }
        /// <summary>
        /// 环丙沙星(KB法)
        /// <summary>
        public String CIP_ND5 { get; set; }

        /// <summary>
        /// 克林霉素(KB法)
        /// <summary>
        public String CLI_ND2 { get; set; }
        /// <summary>
        /// 利福平(KB法)
        /// <summary>
        public String RIF_ND5 { get; set; }
        /// <summary>
        /// 利奈唑胺(KB法)
        /// <summary>
        public String LNZ_ND30 { get; set; }
        /// <summary>
        /// 链霉素(KB法)
        /// <summary>
        public String STR_ND10 { get; set; }
        /// <summary>
        /// 磷霉素(KB法)
        /// <summary>
        public String FOS_ND200 { get; set; }
        /// <summary>
        /// 氯霉素(KB法)
        /// <summary>
        public String CHL_ND30 { get; set; }
        /// <summary>
        /// 美洛培南(KB法)
        /// <summary>
        public String MEM_ND10 { get; set; }
        /// <summary>
        /// 米诺环素(KB法)
        /// <summary>
        public String MNO_ND30 { get; set; }
        /// <summary>
        /// 莫西沙星(KB法)
        /// <summary>
        public String MFX_ND5 { get; set; }
        /// <summary>
        /// 哌拉西林(KB法)
        /// <summary>
        public String PIP_ND100 { get; set; }
        /// <summary>
        /// 哌拉西林/他唑巴坦(KB法)
        /// <summary>
        public String TZP_ND100 { get; set; }
        /// <summary>
        /// 青霉素G(KB法)
        /// <summary>
        public String PEN_ND10 { get; set; }
        /// <summary>
        /// 庆大霉素(KB法)
        /// <summary>
        public String GEN_ND10 { get; set; }
        /// <summary>
        /// 四环素(KB法)
        /// <summary>
        public String TCY_ND30 { get; set; }
        /// <summary>
        /// 替卡西林/克拉维酸(KB法)
        /// <summary>
        public String TCC_ND75 { get; set; }
        /// <summary>
        /// 替卡西林(KB法)
        /// <summary>
        public String TIC_ND75 { get; set; }
        /// <summary>
        /// 替考拉宁(KB法)
        /// <summary>
        public String TEC_ND30 { get; set; }
        /// <summary>
        /// 替加环素(KB法)
        /// <summary>
        public String TGC_ND15 { get; set; }
        /// <summary>
        /// 头孢吡肟(KB法)
        /// <summary>
        public String FEP_ND30 { get; set; }
        /// <summary>
        /// 头孢呋辛(KB法)
        /// <summary>
        public String CXM_ND30 { get; set; }
        /// <summary>
        /// 头孢克洛(KB法)
        /// <summary>
        public String CEC_ND30 { get; set; }
        /// <summary>
        /// 头孢哌酮(KB法)
        /// <summary>
        public String CFP_ND75 { get; set; }
        /// <summary>
        /// 头孢哌酮/舒巴坦(KB法)
        /// <summary>
        public String CSL_ND30 { get; set; }
        /// <summary>
        /// 头孢曲松(KB法)
        /// <summary>
        public String CRO_ND30 { get; set; }
        /// <summary>
        /// 头孢噻肟(KB法)
        /// <summary>
        public String CTX_ND30 { get; set; }
        /// <summary>
        /// 头孢他啶(KB法)
        /// <summary>
        public String CAZ_ND30 { get; set; }
        /// <summary>
        /// 头孢西丁(KB法)
        /// <summary>
        public String FOX_ND30 { get; set; }
        /// <summary>
        /// 头孢唑啉(KB法)
        /// <summary>
        public String CZO_ND30 { get; set; }
        /// <summary>
        /// 妥布霉素(KB法)
        /// <summary>
        public String TOB_ND10 { get; set; }
        /// <summary>
        /// 万古霉素(KB法)
        /// <summary>
        public String VAN_ND30 { get; set; }
        /// <summary>
        /// 亚胺培南(KB法)
        /// <summary>
        public String IPM_ND10 { get; set; }
        /// <summary>
        /// 左旋氧氟沙星(KB法)
        /// <summary>
        public String LVX_ND5 { get; set; }

        /// <summary>
        /// 氧氟沙星(KB法)
        /// <summary>
        public String OFX_ND5 { get; set; }

        /// <summary>
        /// 多西环素(KB法)
        /// <summary>
        public String DOX_ND30 { get; set; }
        /// <summary>
        /// 阿米卡星(MIC法)
        /// <summary>
        public String AMK_NM { get; set; }
        /// <summary>
        /// 阿莫西林/克拉维酸(MIC法)
        /// <summary>
        public String AMC_NM { get; set; }
        /// <summary>
        /// 阿奇霉素(MIC法)
        /// <summary>
        public String AZM_NM { get; set; }
        /// <summary>
        /// 氨苄西林(MIC法)
        /// <summary>
        public String AMP_NM { get; set; }
        /// <summary>
        /// 氨苄西林/舒巴坦(MIC法)
        /// <summary>
        public String SAM_NM { get; set; }
        /// <summary>
        /// 氨曲南(MIC法)
        /// <summary>
        public String ATM_NM { get; set; }
        /// <summary>
        /// 苯唑西林(MIC法)
        /// <summary>
        public String OXA_NM { get; set; }
        /// <summary>
        /// 多粘菌素B(MIC法)
        /// <summary>
        public String POL_NM { get; set; }
        /// <summary>
        /// 呋喃妥因(MIC法)
        /// <summary>
        public String NIT_NM { get; set; }
        /// <summary>
        /// 复方新诺明(MIC法)
        /// <summary>
        public String SXT_NM { get; set; }
        /// <summary>
        /// 高浓度链霉素(MIC法)
        /// <summary>
        public String STH_NM { get; set; }
        /// <summary>
        /// 高浓度庆大霉素(MIC法)
        /// <summary>
        public String GEH_NM { get; set; }
        /// <summary>
        /// 红霉素(MIC法)
        /// <summary>
        public String ERY_NM { get; set; }
        /// <summary>
        /// 环丙沙星(MIC法) 
        /// <summary>
        public String CIP_NM { get; set; }

        /// <summary>
        /// 克林霉素(MIC法)
        /// <summary>
        public String CLI_NM { get; set; }
        /// <summary>
        /// 利福平(MIC法)
        /// <summary>
        public String RIF_NM { get; set; }
        /// <summary>
        /// 利奈唑胺(MIC法)
        /// <summary>
        public String LNZ_NM { get; set; }
        /// <summary>
        /// 链霉素(MIC法)
        /// <summary>
        public String STR_NM { get; set; }
        /// <summary>
        /// 磷霉素(MIC法)
        /// <summary>
        public String FOS_NM { get; set; }
        /// <summary>
        /// 氯霉素(MIC法)
        /// <summary>
        public String CHL_NM { get; set; }
        /// <summary>
        /// 美洛培南(MIC法)
        /// <summary>
        public String MEM_NM { get; set; }
        /// <summary>
        /// 米诺环素(MIC法)
        /// <summary>
        public String MNO_NM { get; set; }
        /// <summary>
        /// 莫西沙星(MIC法)
        /// <summary>
        public String MFX_NM { get; set; }
        /// <summary>
        /// 哌拉西林(MIC法)
        /// <summary>
        public String PIP_NM { get; set; }
        /// <summary>
        /// 哌拉西林/他唑巴坦(MIC法)
        /// <summary>
        public String TZP_NM { get; set; }
        /// <summary>
        /// 青霉素G(MIC法)
        /// <summary>
        public String PEN_NM { get; set; }
        /// <summary>
        /// 庆大霉素(MIC法)
        /// <summary>
        public String GEN_NM { get; set; }
        /// <summary>
        /// 青霉素G(Etest法)
        /// <summary>
        public String PEN_NE { get; set; }
        /// <summary>
        /// 四环素(MIC法)
        /// <summary>
        public String TCY_NM { get; set; }
        /// <summary>
        /// 替卡西林/克拉维酸(MIC法)
        /// <summary>
        public String TCC_NM { get; set; }
        /// <summary>
        /// 替卡西林(MIC法)
        /// <summary>
        public String TIC_NM { get; set; }
        /// <summary>
        /// 替考拉宁(MIC法)
        /// <summary>
        public String TEC_NM { get; set; }
        /// <summary>
        /// 替加环素(MIC法)
        /// <summary>
        public String TGC_NM { get; set; }
        /// <summary>
        /// 头孢吡肟(MIC法)
        /// <summary>
        public String FEP_NM { get; set; }
        /// <summary>
        /// 头孢呋辛(MIC法)
        /// <summary>
        public String CXM_NM { get; set; }
        /// <summary>
        /// 头孢克洛(MIC法)
        /// <summary>
        public String CEC_NM { get; set; }
        /// <summary>
        /// 头孢哌酮(MIC法)
        /// <summary>
        public String CFP_NM { get; set; }
        /// <summary>
        /// 头孢哌酮/舒巴坦(MIC法)
        /// <summary>
        public String CSL_NM { get; set; }
        /// <summary>
        /// 头孢曲松(MIC法)
        /// <summary>
        public String CRO_NM { get; set; }
        /// <summary>
        /// 头孢噻肟(MIC法)
        /// <summary>
        public String CTX_NM { get; set; }
        /// <summary>
        /// 头孢他啶(MIC法)
        /// <summary>
        public String CAZ_NM { get; set; }
        /// <summary>
        /// 头孢西丁(MIC法)
        /// <summary>
        public String FOX_NM { get; set; }
        /// <summary>
        /// 头孢唑啉(MIC法)
        /// <summary>
        public String CZO_NM { get; set; }
        /// <summary>
        /// 妥布霉素(MIC法)
        /// <summary>
        public String TOB_NM { get; set; }
        /// <summary>
        /// 万古霉素(MIC法)
        /// <summary>
        public String VAN_NM { get; set; }
        /// <summary>
        /// 万古霉素(Etest法)
        /// <summary>
        public String VAN_NE { get; set; }
        /// <summary>
        /// 亚胺培南(MIC法)
        /// <summary>
        public String IPM_NM { get; set; }
        /// <summary>
        /// 左旋氧氟沙星(MIC法)
        /// <summary>
        public String LVX_NM { get; set; }
        /// <summary>
        /// 头孢噻肟(Etest法)
        /// <summary>
        public String CTX_NE { get; set; }
        /// <summary>
        /// 头孢哌酮/舒巴坦(KB法)
        /// <summary>
        public String CSL_ND75 { get; set; }

        /// <summary>
        /// 厄他培南(KB法)
        /// <summary>
        public String ETP_ND10 { get; set; }
        /// <summary>
        /// 厄他培南(MIC法)
        /// <summary>
        public String ETP_NM { get; set; }
        /// <summary>
        /// 头孢替坦(KB法)
        /// <summary>
        public String CTT_ND30 { get; set; }
        /// <summary>
        /// 头孢替坦(MIC法)
        /// <summary>
        public String CTT_NM { get; set; }
        /// <summary>
        /// 多尼培南(KB法)
        /// <summary>
        public String DOR_ND10 { get; set; }
        /// <summary>
        /// 多尼培南(MIC法)
        /// <summary>
        public String DOR_NM { get; set; }
        /// <summary>
        /// 奈替米星(KB法)
        /// <summary>
        public String NET_ND30 { get; set; }
        /// <summary>
        /// 奈替米星(MIC法)
        /// <summary>
        public String NET_NM { get; set; }
        /// <summary>
        /// 奎奴普丁/达福普汀(KB法)
        /// <summary>
        public String QDA_ND15 { get; set; }
        /// <summary>
        /// 奎奴普丁/达福普汀(MIC法)
        /// <summary>
        public String QDA_NM { get; set; }
        /// <summary>
        /// 该行数据在原始上传文件中的行索引
        /// <summary>
        public Int32 UploadRowIndex { get; set; }
        /// <summary>
        /// SPEC_REAS
        /// <summary>
        public String SPEC_REAS { get; set; }
        /// <summary>
        /// COMMENT<br />
        /// 注释
        /// <summary>
        public String COMMENT { get; set; }

        /// <summary>
        /// CPT_ND30
        /// <summary>
        public String CPT_ND30 { get; set; }

        /// <summary>
        /// CPT_NM
        /// <summary>
        public String CPT_NM { get; set; }

        /// <summary>
        /// CPT_NE
        /// <summary>
        public String CPT_NE { get; set; }

        /// <summary>
        /// CZA_ND30
        /// <summary>
        public String CZA_ND30 { get; set; }

        /// <summary>
        /// CZA_NM
        /// <summary>
        public String CZA_NM { get; set; }

        /// <summary>
        /// CZA_NE
        /// <summary>
        public String CZA_NE { get; set; }

        /// <summary>
        /// AZA_ND30
        /// <summary>
        public String AZA_ND30 { get; set; }

        /// <summary>
        /// AZA_NM
        /// <summary>
        public String AZA_NM { get; set; }

        /// <summary>
        /// AZA_NE
        /// <summary>
        public String AZA_NE { get; set; }

        /// <summary>
        /// CZT_ND30
        /// <summary>
        public String CZT_ND30 { get; set; }

        /// <summary>
        ///CZT_NM
        /// <summary>
        public String CZT_NM { get; set; }

        /// <summary>
        /// CZT_NE
        /// <summary>
        public String CZT_NE { get; set; }

        /// <summary>
        /// 新增字段
        /// TGC_NE
        /// </summary>
        //public String TGC_NE { get; set; }

        /// <summary>
        /// 新增字段
        /// TGC_NE
        /// </summary>
        public String DOX_NM { get; set; }

        public String COL_ND10 { get; set; }
        public String COL_NM { get; set; }
        public String COL_NE { get; set; }
        public String AMC_NE { get; set; }
        public String AMK_NE { get; set; }
        public String AMP_NE { get; set; }
        public String ATM_NE { get; set; }
        public String AZM_NE { get; set; }
        public String CAZ_NE { get; set; }
        public String CEC_NE { get; set; }
        public String CFP_NE { get; set; }
        public String CHL_NE { get; set; }
        public String CIP_NE { get; set; }
        public String CLI_NE { get; set; }
        public String CRO_NE { get; set; }
        public String CTT_NE { get; set; }
        public String CXM_NE { get; set; }
        public String CZO_NE { get; set; }
        public String DOR_NE { get; set; }
        public String DOX_NE { get; set; }
        public String ERY_NE { get; set; }
        public String ETP_NE { get; set; }
        public String FEP_NE { get; set; }
        public String FOS_NE { get; set; }
        public String FOX_NE { get; set; }
        public String GEN_NE { get; set; }
        public String IPM_NE { get; set; }
        public String LNZ_NE { get; set; }
        public String LVX_NE { get; set; }
        public String MEM_NE { get; set; }
        public String MFX_NE { get; set; }
        public String MNO_NE { get; set; }
        public String NET_NE { get; set; }
        public String NIT_NE { get; set; }
        public String OXA_NE { get; set; }
        public String PIP_NE { get; set; }
        public String POL_NE { get; set; }
        public String QDA_NE { get; set; }
        public String RIF_NE { get; set; }
        public String SAM_NE { get; set; }
        public String STH_NE { get; set; }
        public String STR_NE { get; set; }
        public String SXT_NE { get; set; }
        public String TCC_NE { get; set; }
        public String TCY_NE { get; set; }
        public String TEC_NE { get; set; }
        public String TGC_NE { get; set; }
        public String TIC_NE { get; set; }
        public String TOB_NE { get; set; }
        public String TZP_NE { get; set; }

        //2025-04-22 Gerry 新增加字段：依拉环素ERV_NM，ERV_ND20, ERV_NE
        /// <summary>
        /// 依拉环素（MIC法）
        /// </summary>
        public string ERV_NM { get; set; }
        /// <summary>
        /// 依拉环素（KB法）
        /// </summary>
        public string ERV_ND20 { get; set; }
        /// <summary>
        /// 依拉环素（Etest法）
        /// </summary>
        public string ERV_NE { get; set; }

#if false

        //2020-01-14新增加字段

        /// <summary>
        /// 诱导克林霉素耐药试验D试验<br />
        /// +表示阳性，-表示阴性，P、POS替换为+，N、NEG替换为-<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string INDUC_CLI { get; set; }

        /// <summary>
        /// 碳青霉烯酶<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string CARBAPENEM { get; set; }

        /// <summary>
        /// 改良碳青霉烯灭活试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string MCIM { get; set; }

        /// <summary>
        /// EDTA碳青霉烯灭活试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string ECIM { get; set; }

        /// <summary>
        /// 碳青霉烯酶基因型<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string CARBGENE { get; set; }

        /// <summary>
        /// 诊断<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string DIAGNOSIS { get; set; }

        /// <summary>
        /// 头孢美唑纸片法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string CMZ_ND30 { get; set; }

        /// <summary>
        /// 阿莫西林纸片法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string AMX_ND25 { get; set; }

        /// <summary>
        /// 阿莫西林MIC法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string AMX_NM { get; set; }

        /// <summary>
        /// 氧氟沙星MIC法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string OFX_NM { get; set; }

        /// <summary>
        /// 多黏菌素E纸片法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string COL_ND10 { get; set; }

        /// <summary>
        /// 多黏菌素E的MIC法<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string COL_NM { get; set; }

        /// <summary>
        /// 多黏菌素E的E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string COL_NE { get; set; }

        /// <summary>
        /// 头孢曲松E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string CRO_NE { get; set; }

        /// <summary>
        /// 厄他培南E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string ETP_NE { get; set; }

        /// <summary>
        /// 亚胺培南E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string IPM_NE { get; set; }

        /// <summary>
        /// 利奈唑胺E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string LNZ_NE { get; set; }

        /// <summary>
        /// 美罗培南E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string MEM_NE { get; set; }

        /// <summary>
        /// 多黏菌素B的E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string POL_NE { get; set; }

        /// <summary>
        /// 替考拉宁E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string TEC_NE { get; set; }

        /// <summary>
        /// 替加环素E试验<br />
        /// 2020-01-14新增加字段
        /// </summary>
        public string TGC_NE { get; set; }

#endif

    }
}
