using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.DataInput
{
    public partial  class OLUserproject
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id{get;set;}
        /// <summary>
        /// 项目id
        /// </summary>
      public long projectid{get;set;}
        /// <summary>
        /// 用户id
        /// </summary>
      public long userid{get;set;}
        /// <summary>
        /// 创建者
        /// </summary>
      public long created_byId{get;set;}
       ///
       /// 添加时间
       /// </summary>
      private DateTime insertTime = DateTime.Now;
      public DateTime InsertTime {
            set { insertTime = value; }
            get { return insertTime; }
      }
        
        /// <summary>
        /// 修改时间
        /// </summary>
     private DateTime updatetime = DateTime.Now;
      public DateTime Updatetime {
            set { updatetime = value; }
            get { return updatetime; }
        }
        /// <summary>
        /// 用户绑定项目数量
        /// </summary>
        public int Counts { get; set; }
    }
}
