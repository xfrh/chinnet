using FluentValidation.Attributes;
using ManageSystem.Admin.Validators.CRE;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Admin.Models.CRE
{
    [Validator(typeof(ToconfigureValidator))]
    public class ToconfigureModel: BaseEntityModel
    {
       
        [HtmlDisplayAttribute("细菌名称", "细菌名称", true)]            
        public string Germ { get; set; }
        /// <summary>
        /// 细菌检出株数
        /// </summary>   
        [HtmlDisplayAttribute("细菌检出株数", "细菌检出株数", true)]
        public int? Germ_detected { get; set; }
        /// <summary>
        /// 碳青霉烯类耐药检出株数
        /// </summary>
        [HtmlDisplayAttribute("碳青霉烯类耐药检出株数", "碳青霉烯类耐药检出株数", true)]
        public int? Cr_detected { get; set; }
        /// <summary>
        /// 痰
        /// </summary>
        [HtmlDisplayAttribute("痰", "痰", true)]
        public string Specimen_tan { get; set; }
        /// <summary>
        /// 血液
        /// </summary>
        [HtmlDisplayAttribute("血液", "血液", true)]
        public string Specimen_blood { get; set; }
        /// <summary>
        /// 粪便
        /// </summary>
        [HtmlDisplayAttribute("粪便", "粪便", true)]
        public string Specimen_faeces { get; set; }
        /// <summary>
        /// 肺泡灌洗液
        /// </summary>
        [HtmlDisplayAttribute("肺泡灌洗液", "肺泡灌洗液", true)]
        public string Specimen_alveolar { get; set; }
        /// <summary>
        /// 伤口脓液
        /// </summary>
        [HtmlDisplayAttribute("伤口脓液", "伤口脓液", true)]
        public string Specimen_woundpus { get; set; }
        /// <summary>
        /// 尿道
        /// </summary>
        [HtmlDisplayAttribute("尿道", "尿道", true)]
        public string Specimen_urethra { get; set; }
        /// <summary>
        /// 中心静脉导管
        /// </summary>
        [HtmlDisplayAttribute("中心静脉导管", "中心静脉导管", true)]
        public string Specimen_venous { get; set; }
        /// <summary>
        /// 穿刺液
        /// </summary>
        [HtmlDisplayAttribute("穿刺液", "穿刺液", true)]
        public string Specimen_puncture { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        [HtmlDisplayAttribute("其他", "其他", true)]
        public string Specimen_other { get; set; }
        /// <summary>
        /// 血液科
        /// </summary>
        [HtmlDisplayAttribute("血液科", "血液科", true)]
        public string Department_bloodsection { get; set; }
        /// <summary>
        /// ICU
        /// </summary>
        [HtmlDisplayAttribute("ICU", "ICU", true)]
        public string Department_icu { get; set; }
        /// <summary>
        /// 呼吸科
        /// </summary>
        [HtmlDisplayAttribute("呼吸科", "呼吸科", true)]
        public string Department_breathing { get; set; }
        /// <summary>
        /// 感染科
        /// </summary>
        [HtmlDisplayAttribute("感染科", "感染科", true)]
        public string Department_Infected { get; set; }
        /// <summary>
        /// 移植科
        /// </summary>
        [HtmlDisplayAttribute("移植科", "移植科", true)]
        public string Department_transplant { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        [HtmlDisplayAttribute("其他", "其他", true)]
        public string Department_other { get; set; }
        /// <summary>
        /// 仅以药敏试验结果判断CRE
        /// </summary>
        [HtmlDisplayAttribute("仅以药敏试验结果判断CRE", "仅以药敏试验结果判断CRE", true)]
        public string Sensitive { get; set; }
        /// <summary>
        /// 改良Hodge试验
        /// </summary>
        [HtmlDisplayAttribute("改良Hodge试验", "改良Hodge试验", true)]
        public string Hodge { get; set; }
        /// <summary>
        /// Carba NP
        /// </summary>
        [HtmlDisplayAttribute("Carba NP", "Carba NP", true)]
        public string CarbaNP { get; set; }
        /// <summary>
        /// mCIM和eCIM
        /// </summary>
        [HtmlDisplayAttribute("mCIM和eCIM", "mCIM和eCIM", true)]
        public string MCIMandeCIM { get; set; }
        /// <summary>
        /// EDTA和APB抑制试验
        /// </summary>
        [HtmlDisplayAttribute("EDTA和APB抑制试验", "EDTA和APB抑制试验", true)]
        public string EDTAandAPB { get; set; }
        /// <summary>
        /// 金标免疫快速检测技术
        /// </summary>
        [HtmlDisplayAttribute("金标免疫快速检测技术", "金标免疫快速检测技术", true)]
        public string Goldlabeled { get; set; }
        /// <summary>
        /// 常规PCR技术
        /// </summary>
        [HtmlDisplayAttribute("常规PCR技术", "常规PCR技术", true)]
        public string PCR { get; set; }
        /// <summary>
        /// GeneXpert
        /// </summary>
        [HtmlDisplayAttribute("GeneXpert", "GeneXpert", true)]
        public string GeneXpert { get; set; }
        /// <summary>
        /// 国产碳青霉烯酶基因检测试剂盒
        /// </summary>
        [HtmlDisplayAttribute("国产碳青霉烯酶基因检测试剂盒", "国产碳青霉烯酶基因检测试剂盒", true)]
        public string Carbapenemase { get; set; }
        /// <summary>
        /// 是否常规报告CRE所产碳青霉烯酶型别
        /// </summary>
        [HtmlDisplayAttribute("是否常规报告CRE所产碳青霉烯酶型别", "是否常规报告CRE所产碳青霉烯酶型别", true)]
        public byte General_report { get; set; }
        /// <summary>
        /// 报告方式
        /// </summary>    
        [HtmlDisplayAttribute("报告方式", "报告方式", true)]
        public string Report_type { get; set; }
        /// <summary>
        /// KPC
        /// </summary>
        [HtmlDisplayAttribute("KPC", "KPC", true)]
        public int? Carbapenemase_kpc { get; set; }
        /// <summary>
        /// NDM
        /// </summary>
        [HtmlDisplayAttribute("NDM", "NDM", true)]
        public int? Carbapenemase_ndm { get; set; }
        /// <summary>
        /// OXA-48
        /// </summary>
        [HtmlDisplayAttribute("OXA-48", "OXA-48", true)]
        public int? Carbapenemase_oxa_48 { get; set; }
        /// <summary>
        /// IPM
        /// </summary>
        [HtmlDisplayAttribute("IPM", "IPM", true)]
        public int? Carbapenemase_ipm { get; set; }
        /// <summary>
        /// VIM
        /// </summary>
        [HtmlDisplayAttribute("VIM", "VIM", true)]
        public int? Carbapenemase_vim { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        [HtmlDisplayAttribute("其他", "其他", true)]
        public int? Carbapenemase_other { get; set; }
        /// <summary>
        /// 多黏菌素
        /// </summary>
        [HtmlDisplayAttribute("多黏菌素", "多黏菌素", true)]
        public int? Antibiotic_polymyxin { get; set; }
        /// <summary>
        /// 替加环素
        /// </summary>
        [HtmlDisplayAttribute("替加环素", "替加环素", true)]
        public int? Antibiotic_tegafycline { get; set; }
        /// <summary>
        /// 头孢他啶-阿维巴坦
        /// </summary>
        [HtmlDisplayAttribute("头孢他啶-阿维巴坦", "头孢他啶-阿维巴坦", true)]
        public int? Antibiotic_ceftazidime { get; set; }
        /// <summary>
        /// 磷霉素
        /// </summary>
        [HtmlDisplayAttribute("磷霉素", "磷霉素", true)]
        public int? Antibiotic_fosfomycin { get; set; }
        /// <summary>
        /// 氯霉素
        /// </summary>
        [HtmlDisplayAttribute("氯霉素", "氯霉素", true)]
        public int? Antibiotic_chloramphenicol { get; set; }
        /// <summary>
        /// 联合药敏试验
        /// </summary>
        [HtmlDisplayAttribute("联合药敏试验", "联合药敏试验", true)]
        public int? Antibiotic_drugtest { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        [HtmlDisplayAttribute("年度", "年度", true)]
        public string Data_year { get; set; }
        /// <summary>
        /// 季度
        /// </summary>
        [HtmlDisplayAttribute("季度", "季度", true)]
        public string Data_season { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [HtmlDisplayAttribute("是否显示", "是否显示", true)]
        public byte Allow_report_display { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [HtmlDisplayAttribute("是否有效", "是否有效", true)]
        public byte Isvalid { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        [HtmlDisplayAttribute("是否已审核", "是否已审核", true)]
        public byte Isaudited { get; set; }
        /// <summary>
        /// 审核者ID
        /// </summary>
        [HtmlDisplayAttribute("血液", "血液", true)]
        public long? Audited_by { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [HtmlDisplayAttribute("审核时间", "审核时间", true)]
        public DateTime? Audited_time { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        [HtmlDisplayAttribute("上传时间", "上传时间", true)]
        public DateTime? Created { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [HtmlDisplayAttribute("修改时间", "修改时间", true)]
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [HtmlDisplayAttribute("医院名称", "医院名称", true)]
        public string Hospital { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        [HtmlDisplayAttribute("省份id", "省份id", true)]
        public long Province_id { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        [HtmlDisplayAttribute("省份名称", "省份名称", true)]
        public string Province { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        [HtmlDisplayAttribute("城市id", "城市id", true)]
        public long City_id { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [HtmlDisplayAttribute("城市名称", "城市名称", true)]
        public string City { get; set; }
        /// <summary>
        /// 区县id
        /// </summary>
        [HtmlDisplayAttribute("区县id", "区县id", true)]
        public long District_id { get; set; }
        /// <summary>
        /// 区县名称
        /// </summary>
        [HtmlDisplayAttribute("区县名称", "区县名称", true)]
        public string District { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        [HtmlDisplayAttribute("详细地址", "详细地址", true)]
        public string Detailedaddress { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [HtmlDisplayAttribute("负责人", "负责人", true)]
        public string Contact { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [HtmlDisplayAttribute("邮箱地址", "邮箱地址", true)]
        public string Email { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [HtmlDisplayAttribute("联系电话", "联系电话", true)]
        public string Mobile { get; set; }
        /// <summary>
        /// 上传人
        /// </summary>
        [HtmlDisplayAttribute("上传人", "上传人", true)]
        public string Name { get; set; }
    }
}