using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Cre
{
   public class Cre_data_period
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Cre_id { get; set; }
        /// <summary>
        /// 细菌表id
        /// </summary>
        public long? Germ_id { get; set; }
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

    }
}
