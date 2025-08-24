using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.MicdataDistribution
{
    public class ddAntibiotic
    {
        /// <summary>
        /// id
        /// </summary>
       public long antibiotic__id { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
       public long group_id { get; set; }
        /// <summary>
        /// 是否已于数据
        /// </summary>
       public bool  mark { get; set; }
        /// <summary>
        /// 细菌名称
        /// </summary>
       public string  title { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
       public string title_en{ get; set; }
        /// <summary>
        /// 细菌代码
        /// </summary>
       public string code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
       public int sortid { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
       public bool isvalid { get; set; }
        /// <summary>
        /// 是否默认选中
        /// </summary>
        public bool isdefault { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime created { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
       public long created_by { get; set; }
    }
}
