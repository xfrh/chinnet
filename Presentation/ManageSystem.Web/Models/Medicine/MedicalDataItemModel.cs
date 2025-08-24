using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Web.Validators.Medicine;

namespace ManageSystem.Web.Models.Medicine
{
	/// <summary>
	/// 模型类 ，数据库表名：MedicalDataItem 
	/// </summary>
	 [Validator(typeof(MedicalDataItemValidator))]
	public partial class MedicalDataItemModel : BaseEntityModel
	{
        /// 所属医学数据
        /// <summary>
        public long MedicalDataId { get; set; }


        /// <summary>
        /// 系统排序字段
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 国家
        /// <summary>
        [HtmlDisplayAttribute("国家","国家")]
		public String COUNTRY_A { get; set; }

		/// <summary>
		/// 实验室
		/// <summary>
		[HtmlDisplayAttribute("实验室","实验室")]
		public String LABORATORY { get; set; }

		/// <summary>
		/// 病人Id
		/// <summary>
		[HtmlDisplayAttribute("病人Id","病人Id")]
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
        /// 完整姓名
        /// <summary>
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
		public String AGE { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 生日
        /// <summary>
        [HtmlDisplayAttribute("生日","生日")]
		public String DATE_BIRTH { get; set; }

		/// <summary>
		/// 病房
		/// <summary>
		[HtmlDisplayAttribute("病房","病房")]
		public String WARD { get; set; }

		/// <summary>
		/// 标本编号
		/// <summary>
		[HtmlDisplayAttribute("标本编号","标本编号")]
		public String SPEC_NUM { get; set; }

		/// <summary>
		/// 标本日期
		/// <summary>
		[HtmlDisplayAttribute("标本日期","标本日期")]
		public String SPEC_DATE { get; set; }

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
		/// SPEC_REAS
		/// <summary>
		[HtmlDisplayAttribute("SPEC_REAS","SPEC_REAS")]
		public String SPEC_REAS { get; set; }

		/// <summary>
		/// 细菌
		/// <summary>
		[HtmlDisplayAttribute("细菌","细菌")]
		public String ORGANISM { get; set; }

		/// <summary>
		/// 细菌类型
		/// <summary>
		[HtmlDisplayAttribute("细菌类型","细菌类型")]
		public String ORG_TYPE { get; set; }

		/// <summary>
		/// BETA_LACT
		/// <summary>
		[HtmlDisplayAttribute("BETA_LACT","BETA_LACT")]
		public String BETA_LACT { get; set; }

		/// <summary>
		/// COMMENT
		/// <summary>
		[HtmlDisplayAttribute("COMMENT","COMMENT")]
		public String COMMENT { get; set; }

		/// <summary>
		/// 年龄类型
		/// <summary>
		[HtmlDisplayAttribute("年龄类型","年龄类型")]
		public String PAT_TYPE { get; set; }

		/// <summary>
		/// 头孢呋辛(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢呋辛(KB法)","头孢呋辛(KB法)")]
		public String CAZ_ND30 { get; set; }

		/// <summary>
		/// 头孢哌酮(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢哌酮(KB法)","头孢哌酮(KB法)")]
		public String CFP_ND75 { get; set; }

		/// <summary>
		/// 克林霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("克林霉素(KB法)","克林霉素(KB法)")]
		public String CLI_ND2 { get; set; }

		/// <summary>
		/// 头孢曲松(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢曲松(KB法)","头孢曲松(KB法)")]
		public String CRO_ND30 { get; set; }

		/// <summary>
		/// 头孢哌酮/舒巴坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢哌酮/舒巴坦(KB法)","头孢哌酮/舒巴坦(KB法)")]
		public String CSL_ND30 { get; set; }

		/// <summary>
		/// 头孢噻肟(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢噻肟(KB法)","头孢噻肟(KB法)")]
		public String CTX_ND30 { get; set; }

		/// <summary>
		/// 头孢唑啉(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢唑啉(KB法)","头孢唑啉(KB法)")]
		public String CZO_ND30 { get; set; }

		/// <summary>
		/// 红霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("红霉素(KB法)","红霉素(KB法)")]
		public String ERY_ND15 { get; set; }

		/// <summary>
		/// 磷霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("磷霉素(KB法)","磷霉素(KB法)")]
		public String FOS_ND200 { get; set; }

		/// <summary>
		/// 头孢西丁(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢西丁(KB法)","头孢西丁(KB法)")]
		public String FOX_ND30 { get; set; }

		/// <summary>
		/// 高浓度庆大霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("高浓度庆大霉素(KB法)","高浓度庆大霉素(KB法)")]
		public String GEH_ND120 { get; set; }

		/// <summary>
		/// 亚胺培南(KB法)
		/// <summary>
		[HtmlDisplayAttribute("亚胺培南(KB法)","亚胺培南(KB法)")]
		public String IPM_ND10 { get; set; }

		/// <summary>
		/// 利奈唑胺(KB法)
		/// <summary>
		[HtmlDisplayAttribute("利奈唑胺(KB法)","利奈唑胺(KB法)")]
		public String LNZ_ND30 { get; set; }

		/// <summary>
		/// 左旋氧氟沙星(KB法)
		/// <summary>
		[HtmlDisplayAttribute("左旋氧氟沙星(KB法)","左旋氧氟沙星(KB法)")]
		public String LVX_ND5 { get; set; }

		/// <summary>
		/// 美洛培南(KB法)
		/// <summary>
		[HtmlDisplayAttribute("美洛培南(KB法)","美洛培南(KB法)")]
		public String MEM_ND10 { get; set; }

		/// <summary>
		/// 青霉素G(KB法)
		/// <summary>
		[HtmlDisplayAttribute("青霉素G(KB法)","青霉素G(KB法)")]
		public String PEN_ND10 { get; set; }

		/// <summary>
		/// 氨苄西林/舒巴坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("氨苄西林/舒巴坦(KB法)","氨苄西林/舒巴坦(KB法)")]
		public String SAM_ND10 { get; set; }

		/// <summary>
		/// 替考拉宁(KB法)
		/// <summary>
		[HtmlDisplayAttribute("替考拉宁(KB法)","替考拉宁(KB法)")]
		public String TEC_ND30 { get; set; }

		/// <summary>
		/// 哌拉西林/他唑巴坦(KB法)
		/// <summary>
		[HtmlDisplayAttribute("哌拉西林/他唑巴坦(KB法)","哌拉西林/他唑巴坦(KB法)")]
		public String TZP_ND100 { get; set; }

		/// <summary>
		/// 万古霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("万古霉素(KB法)","万古霉素(KB法)")]
		public String VAN_ND30 { get; set; }

		/// <summary>
		/// 阿米卡星(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("阿米卡星(MIC法)","阿米卡星(MIC法)")]
		public String AMK_NM { get; set; }

		/// <summary>
		/// 氨苄西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("氨苄西林(MIC法)","氨苄西林(MIC法)")]
		public String AMP_NM { get; set; }

		/// <summary>
		/// 氨曲南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("氨曲南(MIC法)","氨曲南(MIC法)")]
		public String ATM_NM { get; set; }

		/// <summary>
		/// 头孢他啶(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢他啶(MIC法)","头孢他啶(MIC法)")]
		public String CAZ_NM { get; set; }

		/// <summary>
		/// 环丙沙星(MIC法) 
		/// <summary>
		[HtmlDisplayAttribute("环丙沙星(MIC法) ","环丙沙星(MIC法) ")]
		public String CIP_NM { get; set; }

		/// <summary>
		/// 克林霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("克林霉素(MIC法)","克林霉素(MIC法)")]
		public String CLI_NM { get; set; }

		/// <summary>
		/// 头孢呋辛(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢呋辛(MIC法)","头孢呋辛(MIC法)")]
		public String CXM_NM { get; set; }

		/// <summary>
		/// 红霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("红霉素(MIC法)","红霉素(MIC法)")]
		public String ERY_NM { get; set; }

		/// <summary>
		/// 头孢吡肟(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢吡肟(MIC法)","头孢吡肟(MIC法)")]
		public String FEP_NM { get; set; }

		/// <summary>
		/// 庆大霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("庆大霉素(MIC法)","庆大霉素(MIC法)")]
		public String GEN_NM { get; set; }

		/// <summary>
		/// 青霉素G(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("青霉素G(MIC法)","青霉素G(MIC法)")]
		public String PEN_NM { get; set; }

		/// <summary>
		/// 呋喃妥因(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("呋喃妥因(MIC法)","呋喃妥因(MIC法)")]
		public String NIT_NM { get; set; }

		/// <summary>
		/// 利奈唑胺(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("利奈唑胺(MIC法)","利奈唑胺(MIC法)")]
		public String LNZ_NM { get; set; }

		/// <summary>
		/// 左旋氧氟沙星(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("左旋氧氟沙星(MIC法)","左旋氧氟沙星(MIC法)")]
		public String LVX_NM { get; set; }

		/// <summary>
		/// 哌拉西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("哌拉西林(MIC法)","哌拉西林(MIC法)")]
		public String PIP_NM { get; set; }

		/// <summary>
		/// 利福平(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("利福平(MIC法)","利福平(MIC法)")]
		public String RIF_NM { get; set; }

		/// <summary>
		/// 氨苄西林/舒巴坦(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("氨苄西林/舒巴坦(MIC法)","氨苄西林/舒巴坦(MIC法)")]
		public String SAM_NM { get; set; }

		/// <summary>
		/// 复方新诺明(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("复方新诺明(MIC法)","复方新诺明(MIC法)")]
		public String SXT_NM { get; set; }

		/// <summary>
		/// 哌拉西林/他唑巴坦(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("哌拉西林/他唑巴坦(MIC法)","哌拉西林/他唑巴坦(MIC法)")]
		public String TZP_NM { get; set; }

		/// <summary>
		/// 万古霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("万古霉素(MIC法)","万古霉素(MIC法)")]
		public String VAN_NM { get; set; }

		/// <summary>
		/// MNO_ND
		/// <summary>
		[HtmlDisplayAttribute("MNO_ND","MNO_ND")]
		public String MNO_ND { get; set; }

		/// <summary>
		/// 阿莫西林/克拉维酸(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("阿莫西林/克拉维酸(MIC法)","阿莫西林/克拉维酸(MIC法)")]
		public String AMC_NM { get; set; }

		/// <summary>
		/// 头孢曲松(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢曲松(MIC法)","头孢曲松(MIC法)")]
		public String CRO_NM { get; set; }

		/// <summary>
		/// CCV_NM
		/// <summary>
		[HtmlDisplayAttribute("CCV_NM","CCV_NM")]
		public String CCV_NM { get; set; }

		/// <summary>
		/// 头孢噻肟(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢噻肟(MIC法)","头孢噻肟(MIC法)")]
		public String CTX_NM { get; set; }

		/// <summary>
		/// CTC_NM
		/// <summary>
		[HtmlDisplayAttribute("CTC_NM","CTC_NM")]
		public String CTC_NM { get; set; }

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
		/// 厄他培南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("厄他培南(MIC法)","厄他培南(MIC法)")]
		public String ETP_NM { get; set; }

		/// <summary>
		/// 美洛培南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("美洛培南(MIC法)","美洛培南(MIC法)")]
		public String MEM_NM { get; set; }

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
		/// 妥布霉素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("妥布霉素(MIC法)","妥布霉素(MIC法)")]
		public String TOB_NM { get; set; }

		/// <summary>
		/// 亚胺培南(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("亚胺培南(MIC法)","亚胺培南(MIC法)")]
		public String IPM_NM { get; set; }

		/// <summary>
		/// DAP_NM
		/// <summary>
		[HtmlDisplayAttribute("DAP_NM","DAP_NM")]
		public String DAP_NM { get; set; }

		/// <summary>
		/// 莫西沙星(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("莫西沙星(MIC法)","莫西沙星(MIC法)")]
		public String MFX_NM { get; set; }

		/// <summary>
		/// 苯唑西林(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("苯唑西林(MIC法)","苯唑西林(MIC法)")]
		public String OXA_NM { get; set; }

		/// <summary>
		/// 奎奴普丁/达福普汀(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("奎奴普丁/达福普汀(MIC法)","奎奴普丁/达福普汀(MIC法)")]
		public String QDA_NM { get; set; }

		/// <summary>
		/// 庆大霉素(KB法)
		/// <summary>
		[HtmlDisplayAttribute("庆大霉素(KB法)","庆大霉素(KB法)")]
		public String GEN_ND10 { get; set; }

		/// <summary>
		/// 替加环素(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("替加环素(MIC法)","替加环素(MIC法)")]
		public String TGC_NM { get; set; }

		/// <summary>
		/// 头孢呋辛(KB法)
		/// <summary>
		[HtmlDisplayAttribute("头孢呋辛(KB法)","头孢呋辛(KB法)")]
		public String CXM_ND30 { get; set; }
        

		/// <summary>
		/// 头孢替坦(MIC法)
		/// <summary>
		[HtmlDisplayAttribute("头孢替坦(MIC法)","头孢替坦(MIC法)")]
		public String CTT_NM { get; set; }

		/// <summary>
		/// MFX_ND
		/// <summary>
		[HtmlDisplayAttribute("MFX_ND","MFX_ND")]
		public String MFX_ND { get; set; }



	}
}
