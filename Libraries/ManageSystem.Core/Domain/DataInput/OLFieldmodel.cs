using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.DataInput
{
    /// <summary>
    /// 模板字段表
    /// </summary>
    public partial class OLFieldmodel : BaseEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
      public long  Id{get;set;}
        /// <summary>
        /// 模板id
        /// </summary>
      public long templateId {get;set;}

        /// <summary>
        /// 模板id
        /// </summary>
      public string template_name { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
      public string  field_name{get;set;}
        /// <summary>
        /// 字段代码
        /// </summary>
      public string  field_code{get;set;}
        /// <summary>
        /// 默认值
        /// </summary>
      public string  default_value{get;set;}
        /// <summary>
        /// 创建人
        /// </summary>
      public long created_byId {get;set;}

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

       public string project_name { get; set; }
        /// <summary>
        /// 是否必填字段
        /// </summary>
        public string sfbt { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string order_px { get; set; }
    }
}
