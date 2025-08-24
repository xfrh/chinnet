using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.DataInput
{
    /// <summary>
    /// 模板表
    /// </summary>
   public partial class OLTemplate : BaseEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
      public long  Id {get;set;} 
        /// <summary>
        /// 模板名称
        /// </summary>
      public string  template_name {get;set;} 
        /// <summary>
        /// 创建人
        /// </summary>
      public long  created_byId {get;set;} 
        /// <summary>
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
        /// 配置字段
        /// </summary>
        public int sums { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Name { get; set; }
    }
}
