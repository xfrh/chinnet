using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.DataInput
{
    /// <summary>
    /// 项目数据表
    /// </summary>
    public partial class OLDataInput : BaseEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public long projectid { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }
        /// <summary>
        /// 数据模板id
        /// </summary>
        public long templateId { get; set; }
        /// <summary>
        /// 实验时间
        /// </summary>
        public string experimenttime { get; set; }
        /// <summary>
        /// 实验人
        /// </summary>
        public string experimenter { get; set; }
        /// <summary>
        /// 工作编号
        /// </summary>
        public string jobnumber { get; set; }
        /// <summary>
        /// 菌种库号
        /// </summary>
        public string germnumber { get; set; }
        /// <summary>
        /// 细菌名称
        /// </summary>
        public string germname { get; set; }        
        /// <summary>
        /// 审核人
        /// </summary>
        public string auditor { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? auditortime { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string datevalue { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public long created_byId { get; set; }
        /// 添加时间
        /// </summary>
        private DateTime insertTime = DateTime.Now;
        public DateTime InsertTime
        {
            set { insertTime = value; }
            get { return insertTime; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        private DateTime updatetime = DateTime.Now;
        public DateTime Updatetime
        {
            set { updatetime = value; }
            get { return updatetime; }
        }

        /// <summary>
        /// 病原菌数量
        /// </summary>
        public int sums {get;set;}
    }
}
