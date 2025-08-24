using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.MicdataDistribution
{
  public  class ddCategoryValue
    {
        /// <summary>
        /// 主键id
        /// </summary>
       public int category_value_id{get;set;}
        /// <summary>
        /// 名称
        /// </summary>
     public string title{get;set;}
        /// <summary>
        /// 代码
        /// </summary>
     public string code{get;set;}
        /// <summary>
        /// 排序
        /// </summary>
     public int sortid{get;set;}
        /// <summary>
        /// 是否有效
        /// </summary>
     public int isvalid{get;set;}
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
