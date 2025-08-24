using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ManageSystem.Core.Domain.Cre
{
   public class Toconfigure
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long? Data_id { get; set; }
        /// <summary>
        /// 医院负责人信息表id
        /// </summary>
        public long? Contact_id { get; set; }
        /// <summary>
        /// 时间表id
        /// </summary>
        public long? Cre_id { get; set; }
        /// <summary>
        /// 细菌表id
        /// </summary>
        public long? Germ_id { get; set; }
        /// <summary>
        /// 细菌名称
        /// </summary>

        public string Germ { get; set; }
        /// <summary>
        /// 细菌检出株数
        /// </summary>     
        public int? Germ_detected { get; set; }
        /// <summary>
        /// 碳青霉烯类耐药检出株数
        /// </summary>

        public int? Cr_detected { get; set; }
        /// <summary>
        /// 痰
        /// </summary>

        public string Specimen_tan { get; set; }
        /// <summary>
        /// 血液
        /// </summary>

        public string Specimen_blood { get; set; }
        /// <summary>
        /// 粪便
        /// </summary>

        public string Specimen_faeces { get; set; }
        /// <summary>
        /// 肺泡灌洗液
        /// </summary>

        public string Specimen_alveolar { get; set; }
        /// <summary>
        /// 伤口脓液
        /// </summary>

        public string Specimen_woundpus { get; set; }
        /// <summary>
        /// 尿道
        /// </summary>

        public string Specimen_urethra { get; set; }
        /// <summary>
        /// 中心静脉导管
        /// </summary>

        public string Specimen_venous { get; set; }
        /// <summary>
        /// 穿刺液
        /// </summary>

        public string Specimen_puncture { get; set; }
        /// <summary>
        /// 其他
        /// </summary>

        public string Specimen_other { get; set; }
        /// <summary>
        /// 血液科
        /// </summary>

        public string Department_bloodsection { get; set; }
        /// <summary>
        /// ICU
        /// </summary>

        public string Department_icu { get; set; }
        /// <summary>
        /// 呼吸科
        /// </summary>

        public string Department_breathing { get; set; }
        /// <summary>
        /// 感染科
        /// </summary>

        public string Department_Infected { get; set; }
        /// <summary>
        /// 移植科
        /// </summary>

        public string Department_transplant { get; set; }
        /// <summary>
        /// 其他
        /// </summary>

        public string Department_other { get; set; }
        /// <summary>
        /// 仅以药敏试验结果判断CRE
        /// </summary>

        public string Sensitive { get; set; }
        /// <summary>
        /// 改良Hodge试验
        /// </summary>

        public string Hodge { get; set; }
        /// <summary>
        /// Carba NP
        /// </summary>

        public string CarbaNP { get; set; }
        /// <summary>
        /// mCIM和eCIM
        /// </summary>

        public string MCIMandeCIM { get; set; }
        /// <summary>
        /// EDTA和APB抑制试验
        /// </summary>

        public string EDTAandAPB { get; set; }
        /// <summary>
        /// 金标免疫快速检测技术
        /// </summary>

        public string Goldlabeled { get; set; }
        /// <summary>
        /// 常规PCR技术
        /// </summary>

        public string PCR { get; set; }
        /// <summary>
        /// GeneXpert
        /// </summary>

        public string GeneXpert { get; set; }
        /// <summary>
        /// 国产碳青霉烯酶基因检测试剂盒
        /// </summary>

        public string Carbapenemase { get; set; }

        /// <summary>
        /// 是否常规报告CRE所产碳青霉烯酶型别
        /// </summary>
        public byte General_report { get; set; }
        /// <summary>
        /// 报告方式
        /// </summary>           
        public string Report_type { get; set; }
        /// <summary>
        /// KPC
        /// </summary>

        public int? Carbapenemase_kpc { get; set; }
        /// <summary>
        /// NDM
        /// </summary>

        public int? Carbapenemase_ndm { get; set; }
        /// <summary>
        /// OXA-48
        /// </summary>

        public int? Carbapenemase_oxa_48 { get; set; }
        /// <summary>
        /// IPM
        /// </summary>

        public int? Carbapenemase_ipm { get; set; }
        /// <summary>
        /// VIM
        /// </summary>

        public int? Carbapenemase_vim { get; set; }
        /// <summary>
        /// 其他
        /// </summary>

        public int? Carbapenemase_other { get; set; }
        /// <summary>
        /// 多黏菌素
        /// </summary>

        public int? Antibiotic_polymyxin { get; set; }
        /// <summary>
        /// 替加环素
        /// </summary>

        public int? Antibiotic_tegafycline { get; set; }
        /// <summary>
        /// 头孢他啶-阿维巴坦
        /// </summary>

        public int? Antibiotic_ceftazidime { get; set; }
        /// <summary>
        /// 磷霉素
        /// </summary>

        public int? Antibiotic_fosfomycin { get; set; }
        /// <summary>
        /// 氯霉素
        /// </summary>

        public int? Antibiotic_chloramphenicol { get; set; }
        /// <summary>
        /// 联合药敏试验
        /// </summary>
        public int? Antibiotic_drugtest { get; set; }


        /// <summary>
        /// 主键id
        /// </summary>
        //public long Cre_id { get; set; }
        /// <summary>
        /// 细菌表id
        /// </summary>
        // public long Germ_id { get; set; }

        /// <summary>
        /// 细菌上传名称
        /// </summary>
        public string Germ_upload_name { get; set; }
        /// <summary>
        /// 碳青霉烯类耐药 名称
        /// </summary>
        public string Cr_name { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public string Data_year { get; set; }
        /// <summary>
        /// 季度
        /// </summary>
        public string Data_season { get; set; }
        /// <summary>
        /// 是否显示当年或当季度数据热图
        /// </summary>
        public byte Allow_report_display { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public byte Isvalid { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        public byte Isaudited { get; set; }
        /// <summary>
        /// 审核者ID
        /// </summary>
        public long? Audited_by { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? Audited_time { get; set; }
        /// <summary>
        /// 数据上传时间
        /// </summary>
        public DateTime? Created { get; set; }
        /// <summary>
        /// 上传者id
        /// </summary>
        public long? Created_by { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        public long? Modified_by { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// 主键id
        /// </summary>
       // public long Contact_id { get; set; }
        /// <summary>
        /// 数据时间断表id
        /// </summary>
       // public long Cre_id { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public long Hospital_id { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string Hospital { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        public long Province_id { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public long City_id { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县id
        /// </summary>
        public long District_id { get; set; }
        /// <summary>
        /// 区县名称
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Detailedaddress { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// kpc
        /// </summary>
        public float ratekpc { get; set; }
        /// <summary>
        /// ndm
        /// </summary>
        public float ratendm { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        public string  Name { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public List<SelectListItem> PercentageList { get; set; }

        /// <summary>
        /// 细菌分类
        /// </summary>
        public List<SelectListItem> BacteriafenlList { get; set; }

        /// <summary>
        /// 年份列表
        /// </summary>
        public List<SelectListItem>YearList { get; set; }

        /// <summary>
        /// 季度列表
        /// </summary>
        public List<SelectListItem> SeasonList { get; set; }
    }
}
