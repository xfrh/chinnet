using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Cre
{
   public class Cre_germ_types
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Germ_id { get; set; }
        /// <summary>
        /// 细菌Code
        /// </summary>
        public string Ger_code { get; set; }
        /// <summary>
        /// 细菌名称
        /// </summary>
        public string Germ_name { get; set; }
        /// <summary>
        /// 细菌上传名称
        /// </summary>
        public string Germ_upload_name { get; set; }
        /// <summary>
        /// 碳青霉烯类耐药 名称
        /// </summary>
        public string Cr_name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sortid { get; set; }
        /// <summary>
        /// 是否允许编辑数据
        /// </summary>
        public bool allow_edit { get; set; }
        /// <summary>
        /// 是否允许数据上传
        /// </summary>
        public bool Allow_data_upload { get; set; }
        /// <summary>
        /// 是否允许前端报告显示
        /// </summary>
        public bool Allow_report_display { get; set; }
        /// <summary>
        /// 强制审核
        /// </summary>
        public byte Force_audit { get; set; }
        /// <summary>
        /// 审核者邮箱
        /// </summary>
        public string Auditor_email { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        public long User_id { get; set; }

        /// <summary>
        /// 天加时间
        /// </summary>
        public DateTime? InsrtTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        //public bool State { get; set; }

        public int sort { get; set; }
    }
}
