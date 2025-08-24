using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SHChart
{
    public class Chart_SHTableData: BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
      public long Id{get;set;}
        /// <summary>
        /// 所属数据段
        /// </summary>
      public long DataSegmentId{get;set;}
        /// <summary>
        /// 报表名称
        /// </summary>
      public long Name{get;set;}
        /// <summary>
        /// 报表标题
        /// </summary>
      public long Title{get;set;}

      public long Sort{get;set;}
      public long Display{get;set;}
      public long Value{get;set;}
    }
}
