using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.DataInput
{
    public partial class OLGermname
    {
        /// <summary>
        /// /主键id
        /// </summary>
      public int Id{get;set;}
        /// <summary>
        /// 细菌名称
        /// </summary>
      public int Germname{get;set;}
        /// <summary>
        /// 细菌呆么
        /// </summary>
      public int code{get;set;}
        /// <summary>
        /// 创建者
        /// </summary>
      public int Create_byId{get;set;}
        /// <summary>
        /// 添加时间
        /// </summary>
      public int Inserttime{get;set;}
    }
}
