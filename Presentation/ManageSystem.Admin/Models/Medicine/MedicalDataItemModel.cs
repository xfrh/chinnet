using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Medicine;

namespace ManageSystem.Admin.Models.Medicine
{
	/// <summary>
	/// 模型类 ，数据库表名：MedicalDataItem 
	/// </summary>
	 [Validator(typeof(MedicalDataItemValidator))]
	public partial class MedicalDataItemModel : BaseEntityModel
	{

		/// <summary>
		/// 所属医学数据
		/// <summary>
		[HtmlDisplayAttribute("所属医学数据","所属医学数据")]
		public long MedicalDataId { get; set; }

		/// <summary>
		/// 国家代码
		/// <summary>
		[HtmlDisplayAttribute("国家代码","国家代码")]
		public String COUNTRY_A { get; set; }

		/// <summary>
		/// 实验室代码
		/// <summary>
		[HtmlDisplayAttribute("实验室代码","实验室代码")]
		public String LABORATORY { get; set; }

		/// <summary>
		/// 来源
		/// <summary>
		[HtmlDisplayAttribute("来源","来源")]
		public String ORIGIN  { get; set; }

		/// <summary>
		/// 病历号（住院病历必填）
		/// <summary>
		[HtmlDisplayAttribute("病历号（住院病历必填）","病历号（住院病历必填）")]
		public String PATIENT_ID { get; set; }

		/// <summary>
		/// 用户姓（中国就直接使用该字段）
		/// <summary>
		[HtmlDisplayAttribute("用户姓（中国就直接使用该字段）","用户姓（中国就直接使用该字段）")]
		public String FIRST_NAME { get; set; }

		/// <summary>
		/// 用户名
		/// <summary>
		[HtmlDisplayAttribute("用户名","用户名")]
		public String LAST_NAME { get; set; }

		/// <summary>
		/// 姓名
		/// <summary>
		[HtmlDisplayAttribute("姓名","姓名")]
		public String FULL_NAME { get; set; }

		/// <summary>
		/// 性别
		/// <summary>
		[HtmlDisplayAttribute("性别","性别")]
		public String SEX { get; set; }

		/// <summary>
		/// 年龄
		/// <summary>
		[HtmlDisplayAttribute("年龄","年龄")]
		public Int32 AGE { get; set; }

		/// <summary>
		/// 年龄类型，有枚举
		/// <summary>
		[HtmlDisplayAttribute("年龄类型，有枚举","年龄类型，有枚举")]
		public String PAT_TYPE { get; set; }

		/// <summary>
		/// 生日
		/// <summary>
		[HtmlDisplayAttribute("生日","生日")]
		public String DATE_BIRTH { get; set; }

		/// <summary>
		/// 科室
		/// <summary>
		[HtmlDisplayAttribute("科室","科室")]
		public String WARD { get; set; }

		/// <summary>
		/// 科室类别，关联科室类别表
		/// <summary>
		[HtmlDisplayAttribute("科室类别，关联科室类别表","科室类别，关联科室类别表")]
		public String WARD_TYPE { get; set; }

		/// <summary>
		/// 医院代码
		/// <summary>
		[HtmlDisplayAttribute("医院代码","医院代码")]
		public String INSTITUT { get; set; }

		/// <summary>
		/// 专业类别
		/// <summary>
		[HtmlDisplayAttribute("专业类别","专业类别")]
		public String DEPARTMENT { get; set; }

		/// <summary>
		/// 标本编号
		/// <summary>
		[HtmlDisplayAttribute("标本编号","标本编号")]
		public String SPEC_NUM { get; set; }

		/// <summary>
		/// 标本日期
		/// <summary>
		[HtmlDisplayAttribute("标本日期","标本日期")]
		public DateTime SPEC_DATE { get; set; }

		/// <summary>
		/// 标本种类缩写
		/// <summary>
		[HtmlDisplayAttribute("标本种类缩写","标本种类缩写")]
		public String SPEC_TYPE { get; set; }

		/// <summary>
		/// 标本种类代码
		/// <summary>
		[HtmlDisplayAttribute("标本种类代码","标本种类代码")]
		public String SPEC_CODE { get; set; }

		/// <summary>
		/// 细菌
		/// <summary>
		[HtmlDisplayAttribute("细菌","细菌")]
		public String ORGANISM { get; set; }

		/// <summary>
		/// 细菌类型（+：革兰阳性菌、-：革兰阴性菌）
		/// <summary>
		[HtmlDisplayAttribute("细菌类型（+：革兰阳性菌、-：革兰阴性菌）","细菌类型（+：革兰阳性菌、-：革兰阴性菌）")]
		public String ORG_TYPE { get; set; }

		/// <summary>
		/// 数据采样时间
		/// <summary>
		[HtmlDisplayAttribute("数据采样时间","数据采样时间")]
		public DateTime DATE_DATA { get; set; }

		/// <summary>
		/// 超广谱β-内酰胺酶
		/// <summary>
		[HtmlDisplayAttribute("超广谱β-内酰胺酶","超广谱β-内酰胺酶")]
		public String ESBL { get; set; }

		/// <summary>
		/// 超广谱β-内酰胺酶
		/// <summary>
		[HtmlDisplayAttribute("超广谱β-内酰胺酶","超广谱β-内酰胺酶")]
		public String BETA_LACT { get; set; }

		/// <summary>
		/// 耐甲氧西林葡萄球菌检测试验
		/// <summary>
		[HtmlDisplayAttribute("耐甲氧西林葡萄球菌检测试验","耐甲氧西林葡萄球菌检测试验")]
		public String MRSA_SCRN { get; set; }

		/// <summary>
		/// 诱导型克林霉素耐药试验
		/// <summary>
		[HtmlDisplayAttribute("诱导型克林霉素耐药试验","诱导型克林霉素耐药试验")]
		public String INDUC_CLI { get; set; }

		/// <summary>
		/// 霍奇试验
		/// <summary>
		[HtmlDisplayAttribute("霍奇试验","霍奇试验")]
		public String HODGE { get; set; }

		/// <summary>
		/// 碳青酶烯酶
		/// <summary>
		[HtmlDisplayAttribute("碳青酶烯酶","碳青酶烯酶")]
		public String CARBAPENEM { get; set; }

		/// <summary>
		/// 阿米卡星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("阿米卡星(KB法)","阿米卡星(KB法)")]
		public String AMK_ND30 { get; set; }

		/// <summary>
		/// 阿莫西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("阿莫西林(KB法)","阿莫西林(KB法)")]
		public String AMX_ND25 { get; set; }

		/// <summary>
		/// 阿莫西林/克拉维酸(KB法)
		/// <summary>
		[HtmlDisplayAttribute("阿莫西林/克拉维酸(KB法)","阿莫西林/克拉维酸(KB法)")]
		public String AMC_ND20 { get; set; }

		/// <summary>
		/// 阿奇霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("阿奇霉素(KB法)","阿奇霉素(KB法)")]
		public String AZM_ND15 { get; set; }

		/// <summary>
		/// 氨苄西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("氨苄西林(KB法)","氨苄西林(KB法)")]
		public String AMP_ND10 { get; set; }

		/// <summary>
		/// 氨苄西林/舒巴坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("氨苄西林/舒巴坦(KB法)","氨苄西林/舒巴坦(KB法)")]
		public String SAM_ND10 { get; set; }

		/// <summary>
		/// 氨曲南(KB法)
		/// <summary>
		[HtmlDisplayAttribute("氨曲南(KB法)","氨曲南(KB法)")]
		public String ATM_ND30 { get; set; }

		/// <summary>
		/// 苯唑西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("苯唑西林(KB法)","苯唑西林(KB法)")]
		public String OXA_ND1 { get; set; }

		/// <summary>
		/// 多粘菌素B(KB法)
		/// <summary>
		[HtmlDisplayAttribute("多粘菌素B(KB法)","多粘菌素B(KB法)")]
		public String POL_ND300 { get; set; }

		/// <summary>
		/// 呋喃妥因(KB法)
		/// <summary>
		[HtmlDisplayAttribute("呋喃妥因(KB法)","呋喃妥因(KB法)")]
		public String NIT_ND300 { get; set; }

		/// <summary>
		/// 复方新诺明(KB法)
		/// <summary>
		[HtmlDisplayAttribute("复方新诺明(KB法)","复方新诺明(KB法)")]
		public String SXT_ND1_2 { get; set; }

		/// <summary>
		/// 高浓度链霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("高浓度链霉素(KB法)","高浓度链霉素(KB法)")]
		public String STH_ND300 { get; set; }

		/// <summary>
		/// 高浓度庆大霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("高浓度庆大霉素(KB法)","高浓度庆大霉素(KB法)")]
		public String GEH_ND120 { get; set; }

		/// <summary>
		/// 红霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("红霉素(KB法)","红霉素(KB法)")]
		public String ERY_ND15 { get; set; }

		/// <summary>
		/// 环丙沙星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("环丙沙星(KB法)","环丙沙星(KB法)")]
		public String CIP_ND5 { get; set; }

		/// <summary>
		/// 甲氧西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("甲氧西林(KB法)","甲氧西林(KB法)")]
		public String MET_ND5 { get; set; }

		/// <summary>
		/// 克林霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("克林霉素(KB法)","克林霉素(KB法)")]
		public String CLI_ND2 { get; set; }

		/// <summary>
		/// 利福平(KB法)
		/// <summary>
		[HtmlDisplayAttribute("利福平(KB法)","利福平(KB法)")]
		public String RIF_ND5 { get; set; }

		/// <summary>
		/// 利奈唑胺(KB法)
		/// <summary>
		[HtmlDisplayAttribute("利奈唑胺(KB法)","利奈唑胺(KB法)")]
		public String LNZ_ND30 { get; set; }

		/// <summary>
		/// 链霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("链霉素(KB法)","链霉素(KB法)")]
		public String STR_ND10 { get; set; }

		/// <summary>
		/// 磷霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("磷霉素(KB法)","磷霉素(KB法)")]
		public String FOS_ND200 { get; set; }

		/// <summary>
		/// 氯霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("氯霉素(KB法)","氯霉素(KB法)")]
		public String CHL_ND30 { get; set; }

		/// <summary>
		/// 美洛培南(KB法)
		/// <summary>
		[HtmlDisplayAttribute("美洛培南(KB法)","美洛培南(KB法)")]
		public String MEM_ND10 { get; set; }

		/// <summary>
		/// 米诺环素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("米诺环素(KB法)","米诺环素(KB法)")]
		public String MNO_ND30 { get; set; }

		/// <summary>
		/// 莫西沙星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("莫西沙星(KB法)","莫西沙星(KB法)")]
		public String MFX_ND5 { get; set; }

		/// <summary>
		/// 哌拉西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("哌拉西林(KB法)","哌拉西林(KB法)")]
		public String PIP_ND100 { get; set; }

		/// <summary>
		/// 哌拉西林/他唑巴坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("哌拉西林/他唑巴坦(KB法)","哌拉西林/他唑巴坦(KB法)")]
		public String TZP_ND100 { get; set; }

		/// <summary>
		/// 青霉素G(KB法)
		/// <summary>
		[HtmlDisplayAttribute("青霉素G(KB法)","青霉素G(KB法)")]
		public String PEN_ND10 { get; set; }

		/// <summary>
		/// 庆大霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("庆大霉素(KB法)","庆大霉素(KB法)")]
		public String GEN_ND10 { get; set; }

		/// <summary>
		/// 四环素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("四环素(KB法)","四环素(KB法)")]
		public String TCY_ND30 { get; set; }

		/// <summary>
		/// 替卡西林/克拉维酸(KB法)
		/// <summary>
		[HtmlDisplayAttribute("替卡西林/克拉维酸(KB法)","替卡西林/克拉维酸(KB法)")]
		public String TCC_ND75 { get; set; }

		/// <summary>
		/// 替卡西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("替卡西林(KB法)","替卡西林(KB法)")]
		public String TIC_ND75 { get; set; }

		/// <summary>
		/// 替考拉宁(KB法)
		/// <summary>
		[HtmlDisplayAttribute("替考拉宁(KB法)","替考拉宁(KB法)")]
		public String TEC_ND30 { get; set; }

		/// <summary>
		/// 替加环素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("替加环素(KB法)","替加环素(KB法)")]
		public String TGC_ND15 { get; set; }

		/// <summary>
		/// 头孢吡肟(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢吡肟(KB法)","头孢吡肟(KB法)")]
		public String FEP_ND30 { get; set; }

		/// <summary>
		/// 头孢呋辛(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢呋辛(KB法)","头孢呋辛(KB法)")]
		public String CXM_ND30 { get; set; }

		/// <summary>
		/// 头孢克洛(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢克洛(KB法)","头孢克洛(KB法)")]
		public String CEC_ND30 { get; set; }

		/// <summary>
		/// 头孢哌酮(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢哌酮(KB法)","头孢哌酮(KB法)")]
		public String CFP_ND75 { get; set; }

		/// <summary>
		/// 头孢哌酮/舒巴坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢哌酮/舒巴坦(KB法)","头孢哌酮/舒巴坦(KB法)")]
		public String CSL_ND30 { get; set; }

		/// <summary>
		/// 头孢曲松(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢曲松(KB法)","头孢曲松(KB法)")]
		public String CRO_ND30 { get; set; }

		/// <summary>
		/// 头孢噻肟(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢噻肟(KB法)","头孢噻肟(KB法)")]
		public String CTX_ND30 { get; set; }

		/// <summary>
		/// 头孢他啶(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢他啶(KB法)","头孢他啶(KB法)")]
		public String CAZ_ND30 { get; set; }

		/// <summary>
		/// 头孢西丁(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢西丁(KB法)","头孢西丁(KB法)")]
		public String FOX_ND30 { get; set; }

		/// <summary>
		/// 头孢唑啉(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢唑啉(KB法)","头孢唑啉(KB法)")]
		public String CZO_ND30 { get; set; }

		/// <summary>
		/// 妥布霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("妥布霉素(KB法)","妥布霉素(KB法)")]
		public String TOB_ND10 { get; set; }

		/// <summary>
		/// 万古霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("万古霉素(KB法)","万古霉素(KB法)")]
		public String VAN_ND30 { get; set; }

		/// <summary>
		/// 亚胺培南(KB法)
		/// <summary>
		[HtmlDisplayAttribute("亚胺培南(KB法)","亚胺培南(KB法)")]
		public String IPM_ND10 { get; set; }

		/// <summary>
		/// 左旋氧氟沙星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("左旋氧氟沙星(KB法)","左旋氧氟沙星(KB法)")]
		public String LVX_ND5 { get; set; }

		/// <summary>
		/// 磺胺类(KB法)
		/// <summary>
		[HtmlDisplayAttribute("磺胺类(KB法)","磺胺类(KB法)")]
		public String SSS_ND200 { get; set; }

		/// <summary>
		/// 头孢噻吩(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢噻吩(KB法)","头孢噻吩(KB法)")]
		public String CEP_ND30 { get; set; }

		/// <summary>
		/// 羧苄西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("羧苄西林(KB法)","羧苄西林(KB法)")]
		public String CRB_ND100 { get; set; }

		/// <summary>
		/// 氧氟沙星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("氧氟沙星(KB法)","氧氟沙星(KB法)")]
		public String OFX_ND5 { get; set; }

		/// <summary>
		/// 头孢唑肟(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢唑肟(KB法)","头孢唑肟(KB法)")]
		public String CZX_ND30 { get; set; }

		/// <summary>
		/// 美洛西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("美洛西林(KB法)","美洛西林(KB法)")]
		public String MEZ_ND75 { get; set; }

		/// <summary>
		/// 诺氟沙星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("诺氟沙星(KB法)","诺氟沙星(KB法)")]
		public String NOR_ND10 { get; set; }

		/// <summary>
		/// 头孢孟多(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢孟多(KB法)","头孢孟多(KB法)")]
		public String MAN_ND30 { get; set; }

		/// <summary>
		/// 多西环素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("多西环素(KB法)","多西环素(KB法)")]
		public String DOX_ND30 { get; set; }

		/// <summary>
		/// 新生霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("新生霉素(KB法)","新生霉素(KB法)")]
		public String NOV_ND5 { get; set; }

		/// <summary>
		/// 阿米卡星(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("阿米卡星(MIC法)","阿米卡星(MIC法)")]
		public String AMK_NM { get; set; }

		/// <summary>
		/// 阿莫西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("阿莫西林(MIC法)","阿莫西林(MIC法)")]
		public String AMX_NM { get; set; }

		/// <summary>
		/// 阿莫西林/克拉维酸(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("阿莫西林/克拉维酸(MIC法)","阿莫西林/克拉维酸(MIC法)")]
		public String AMC_NM { get; set; }

		/// <summary>
		/// 阿奇霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("阿奇霉素(MIC法)","阿奇霉素(MIC法)")]
		public String AZM_NM { get; set; }

		/// <summary>
		/// 氨苄西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("氨苄西林(MIC法)","氨苄西林(MIC法)")]
		public String AMP_NM { get; set; }

		/// <summary>
		/// 氨苄西林/舒巴坦(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("氨苄西林/舒巴坦(MIC法)","氨苄西林/舒巴坦(MIC法)")]
		public String SAM_NM { get; set; }

		/// <summary>
		/// 氨曲南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("氨曲南(MIC法)","氨曲南(MIC法)")]
		public String ATM_NM { get; set; }

		/// <summary>
		/// 苯唑西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("苯唑西林(MIC法)","苯唑西林(MIC法)")]
		public String OXA_NM { get; set; }

		/// <summary>
		/// 多粘菌素B(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("多粘菌素B(MIC法)","多粘菌素B(MIC法)")]
		public String POL_NM { get; set; }

		/// <summary>
		/// 呋喃妥因(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("呋喃妥因(MIC法)","呋喃妥因(MIC法)")]
		public String NIT_NM { get; set; }

		/// <summary>
		/// 复方新诺明(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("复方新诺明(MIC法)","复方新诺明(MIC法)")]
		public String SXT_NM { get; set; }

		/// <summary>
		/// 高浓度链霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("高浓度链霉素(MIC法)","高浓度链霉素(MIC法)")]
		public String STH_NM { get; set; }

		/// <summary>
		/// 高浓度庆大霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("高浓度庆大霉素(MIC法)","高浓度庆大霉素(MIC法)")]
		public String GEH_NM { get; set; }

		/// <summary>
		/// 红霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("红霉素(MIC法)","红霉素(MIC法)")]
		public String ERY_NM { get; set; }

		/// <summary>
		/// 环丙沙星(MIC法) 
		/// <summary>
		[HtmlDisplayAttribute("环丙沙星(MIC法) ","环丙沙星(MIC法) ")]
		public String CIP_NM { get; set; }

		/// <summary>
		/// 甲氧西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("甲氧西林(MIC法)","甲氧西林(MIC法)")]
		public String MET_NM { get; set; }

		/// <summary>
		/// 克林霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("克林霉素(MIC法)","克林霉素(MIC法)")]
		public String CLI_NM { get; set; }

		/// <summary>
		/// 利福平(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("利福平(MIC法)","利福平(MIC法)")]
		public String RIF_NM { get; set; }

		/// <summary>
		/// 利奈唑胺(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("利奈唑胺(MIC法)","利奈唑胺(MIC法)")]
		public String LNZ_NM { get; set; }

		/// <summary>
		/// 链霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("链霉素(MIC法)","链霉素(MIC法)")]
		public String STR_NM { get; set; }

		/// <summary>
		/// 磷霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("磷霉素(MIC法)","磷霉素(MIC法)")]
		public String FOS_NM { get; set; }

		/// <summary>
		/// 氯霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("氯霉素(MIC法)","氯霉素(MIC法)")]
		public String CHL_NM { get; set; }

		/// <summary>
		/// 美洛培南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("美洛培南(MIC法)","美洛培南(MIC法)")]
		public String MEM_NM { get; set; }

		/// <summary>
		/// 米诺环素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("米诺环素(MIC法)","米诺环素(MIC法)")]
		public String MNO_NM { get; set; }

		/// <summary>
		/// 莫西沙星(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("莫西沙星(MIC法)","莫西沙星(MIC法)")]
		public String MFX_NM { get; set; }

		/// <summary>
		/// 哌拉西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("哌拉西林(MIC法)","哌拉西林(MIC法)")]
		public String PIP_NM { get; set; }

		/// <summary>
		/// 哌拉西林/他唑巴坦(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("哌拉西林/他唑巴坦(MIC法)","哌拉西林/他唑巴坦(MIC法)")]
		public String TZP_NM { get; set; }

		/// <summary>
		/// 青霉素G(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("青霉素G(MIC法)","青霉素G(MIC法)")]
		public String PEN_NM { get; set; }

		/// <summary>
		/// 庆大霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("庆大霉素(MIC法)","庆大霉素(MIC法)")]
		public String GEN_NM { get; set; }

		/// <summary>
		/// 青霉素G(Etest法)
		/// <summary>
		[HtmlDisplayAttribute("青霉素G(Etest法)","青霉素G(Etest法)")]
		public String PEN_NE { get; set; }

		/// <summary>
		/// 四环素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("四环素(MIC法)","四环素(MIC法)")]
		public String TCY_NM { get; set; }

		/// <summary>
		/// 替卡西林/克拉维酸(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("替卡西林/克拉维酸(MIC法)","替卡西林/克拉维酸(MIC法)")]
		public String TCC_NM { get; set; }

		/// <summary>
		/// 替卡西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("替卡西林(MIC法)","替卡西林(MIC法)")]
		public String TIC_NM { get; set; }

		/// <summary>
		/// 替考拉宁(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("替考拉宁(MIC法)","替考拉宁(MIC法)")]
		public String TEC_NM { get; set; }

		/// <summary>
		/// 替加环素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("替加环素(MIC法)","替加环素(MIC法)")]
		public String TGC_NM { get; set; }

		/// <summary>
		/// 头孢吡肟(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢吡肟(MIC法)","头孢吡肟(MIC法)")]
		public String FEP_NM { get; set; }

		/// <summary>
		/// 头孢呋辛(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢呋辛(MIC法)","头孢呋辛(MIC法)")]
		public String CXM_NM { get; set; }

		/// <summary>
		/// 头孢哌酮(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢哌酮(MIC法)","头孢哌酮(MIC法)")]
		public String CFP_NM { get; set; }

		/// <summary>
		/// 头孢哌酮/舒巴坦(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢哌酮/舒巴坦(MIC法)","头孢哌酮/舒巴坦(MIC法)")]
		public String CSL_NM { get; set; }

		/// <summary>
		/// 头孢曲松(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢曲松(MIC法)","头孢曲松(MIC法)")]
		public String CRO_NM { get; set; }

		/// <summary>
		/// 头孢噻肟(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢噻肟(MIC法)","头孢噻肟(MIC法)")]
		public String CTX_NM { get; set; }

		/// <summary>
		/// 头孢他啶(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢他啶(MIC法)","头孢他啶(MIC法)")]
		public String CAZ_NM { get; set; }

		/// <summary>
		/// 头孢西丁(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢西丁(MIC法)","头孢西丁(MIC法)")]
		public String FOX_NM { get; set; }

		/// <summary>
		/// 头孢唑啉(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢唑啉(MIC法)","头孢唑啉(MIC法)")]
		public String CZO_NM { get; set; }

		/// <summary>
		/// 妥布霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("妥布霉素(MIC法)","妥布霉素(MIC法)")]
		public String TOB_NM { get; set; }

		/// <summary>
		/// 万古霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("万古霉素(MIC法)","万古霉素(MIC法)")]
		public String VAN_NM { get; set; }

		/// <summary>
		/// 万古霉素(Etest法)
		/// <summary>
		[HtmlDisplayAttribute("万古霉素(Etest法)","万古霉素(Etest法)")]
		public String VAN_NE { get; set; }

		/// <summary>
		/// 亚胺培南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("亚胺培南(MIC法)","亚胺培南(MIC法)")]
		public String IPM_NM { get; set; }

		/// <summary>
		/// 左旋氧氟沙星(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("左旋氧氟沙星(MIC法)","左旋氧氟沙星(MIC法)")]
		public String LVX_NM { get; set; }

		/// <summary>
		/// 头孢噻肟(Etest法)
		/// <summary>
		[HtmlDisplayAttribute("头孢噻肟(Etest法)","头孢噻肟(Etest法)")]
		public String CTX_NE { get; set; }

		/// <summary>
		/// 头孢哌酮/舒巴坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢哌酮/舒巴坦(KB法)","头孢哌酮/舒巴坦(KB法)")]
		public String CSL_ND75 { get; set; }

		/// <summary>
		/// 阿莫西林(KB法)
		/// <summary>
		[HtmlDisplayAttribute("阿莫西林(KB法)","阿莫西林(KB法)")]
		public String AMX_ND30 { get; set; }

		/// <summary>
		/// 厄他培南(KB法)
		/// <summary>
		[HtmlDisplayAttribute("厄他培南(KB法)","厄他培南(KB法)")]
		public String ETP_ND10 { get; set; }

		/// <summary>
		/// 厄他培南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("厄他培南(MIC法)","厄他培南(MIC法)")]
		public String ETP_NM { get; set; }

		/// <summary>
		/// 头孢替坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢替坦(KB法)","头孢替坦(KB法)")]
		public String CTT_ND30 { get; set; }

		/// <summary>
		/// 头孢替坦(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢替坦(MIC法)","头孢替坦(MIC法)")]
		public String CTT_NM { get; set; }

		/// <summary>
		/// 多尼培南(KB法)
		/// <summary>
		[HtmlDisplayAttribute("多尼培南(KB法)","多尼培南(KB法)")]
		public String DOR_ND10 { get; set; }

		/// <summary>
		/// 多尼培南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("多尼培南(MIC法)","多尼培南(MIC法)")]
		public String DOR_NM { get; set; }

		/// <summary>
		/// 奈替米星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("奈替米星(KB法)","奈替米星(KB法)")]
		public String NET_ND30 { get; set; }

		/// <summary>
		/// 奈替米星(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("奈替米星(MIC法)","奈替米星(MIC法)")]
		public String NET_NM { get; set; }

		/// <summary>
		/// 奎奴普丁/达福普汀(KB法)
		/// <summary>
		[HtmlDisplayAttribute("奎奴普丁/达福普汀(KB法)","奎奴普丁/达福普汀(KB法)")]
		public String QDA_ND15 { get; set; }

		/// <summary>
		/// 奎奴普丁/达福普汀(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("奎奴普丁/达福普汀(MIC法)","奎奴普丁/达福普汀(MIC法)")]
		public String QDA_NM { get; set; }

		/// <summary>
		/// 该行数据在原始上传文件中的行索引
		/// <summary>
		[HtmlDisplayAttribute("该行数据在原始上传文件中的行索引","该行数据在原始上传文件中的行索引")]
		public Int32 UploadRowIndex { get; set; }

		/// <summary>
		/// SPEC_REAS
		/// <summary>
		[HtmlDisplayAttribute("SPEC_REAS","SPEC_REAS")]
		public String SPEC_REAS { get; set; }

		/// <summary>
		/// COMMENT
		/// <summary>
		[HtmlDisplayAttribute("COMMENT","COMMENT")]
		public String COMMENT { get; set; }

		/// <summary>
		/// CCV_NM
		/// <summary>
		[HtmlDisplayAttribute("CCV_NM","CCV_NM")]
		public String CCV_NM { get; set; }

		/// <summary>
		/// CTC_NM
		/// <summary>
		[HtmlDisplayAttribute("CTC_NM","CTC_NM")]
		public String CTC_NM { get; set; }

		/// <summary>
		/// MFX_ND
		/// <summary>
		[HtmlDisplayAttribute("MFX_ND","MFX_ND")]
		public String MFX_ND { get; set; }

		/// <summary>
		/// DAP_NM
		/// <summary>
		[HtmlDisplayAttribute("DAP_NM","DAP_NM")]
		public String DAP_NM { get; set; }



	}
}
