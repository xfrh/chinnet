using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.DataInput
{
    /// <summary>
    /// 项目表
    /// </summary>
    public partial class OLProject : BaseEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public  long Id{get;set;}
        /// <summary>
        /// 字段模板id
        /// </summary>
      public  long templateId{get;set;}
        /// <summary>
        /// 项目名称
        /// </summary>
      public  string project_name{get;set;}
        /// <summary>
        /// 开始时间
        /// </summary>
      public  DateTime starttime{get;set;}
        /// <summary>
        /// 结束时间
        /// </summary>
      public DateTime endtime {get;set;}
        /// <summary>
        /// 状态
        /// </summary>
      public string state {get;set;}
        /// <summary>
        /// 创建人id
        /// </summary>
      public  long created_byId{get;set;}
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
        /// 创建人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 复合的数量
        /// </summary>
       public string auditorsums { get; set; }
        /// <summary>
        /// 阅读的数量
        /// </summary>
       public string projectsums { get; set; }
    }
}
