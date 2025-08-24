using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.MicdataDistribution
{
   public partial class ddYear
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long year_id{get;set;}
        /// <summary>
        /// 名称
        /// </summary>
      public string title{get;set;}
        /// <summary>
        /// 排序
        /// </summary>
      public int sortid{get;set;}
        /// <summary>
        /// 是否有效
        /// </summary>
      public bool isvalid{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
      public DateTime created{get;set;}
        /// <summary>
        /// 创建人
        /// </summary>
      public long created_by{get;set;}
    }
}
