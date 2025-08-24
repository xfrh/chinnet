using ManageSystem.Core.Domain.DataInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.DataInput
{
    [Serializable]
    public partial class OLProjectModel
    {
        public PagedList<OLProjecItemModel> PageList { get; set; }
    }
    public partial class OLProjecItemModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 字段模板id
        /// </summary>
        public string templateId { get; set; }

        /// <summary>
        /// 字段代码
        /// </summary>
        public string field_code { get; set; }
        
        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }
        /// <summary>
        /// 参与人
        /// </summary>
        public string participants { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endtime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public long created_byId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string InsertTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime updatetime { get; set; }
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

        /// <summary>
        /// 创建者id
        /// </summary>
        public string createdId { get; set; }
    }
}