using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.MicdataDistribution
{
   public class ddCategory
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int category_id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int sortid  { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime created  { get; set; }
        /// <summary>
        ///  创建人
        /// </summary>
        public int created_by  { get; set; }
    }
}
