using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Cre
{
   public class Cre_data
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Data_id { get; set; }
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
        /// 医院id
        /// </summary>
        public long? Hospital_id { get; set; }
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
        /// 总数量
        /// </summary>
        public int? sums { get; set; }

        public string xAxis { get; set; }
        /// <summary>
        /// 总检出率
        /// </summary>
        public float Strainrate { get; set; }

        public string Supplier { get; set; }
        public float Drugrate { get; set; }

        public long? Created_by { get; set; }
    }
}
