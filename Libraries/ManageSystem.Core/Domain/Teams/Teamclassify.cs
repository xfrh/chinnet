using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Teams
{
    //对应数据表：Teamclassify
    public partial class Teamclassify:BaseEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
      public  long Id {get;set;}
        /// <summary>
        /// 分类名称
        /// </summary>
      public  string classifyname {get;set;}
        /// <summary>
        /// 穿件人
        /// </summary>
      public  long createby_Id {get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
      public  DateTime InsertTime {get;set;}
        /// <summary>
        /// 修改时间
        /// </summary>
      public DateTime UpdateTime {get;set;}
    }
}
