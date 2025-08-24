using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Cre
{
   public class Cre_Colormatching
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public string Data_year { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Data_type { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Content_value { get; set; }

        /// <summary>
        /// 查询配色存才字段
        /// </summary>
        public int Sums { get; set; }
    }
}
