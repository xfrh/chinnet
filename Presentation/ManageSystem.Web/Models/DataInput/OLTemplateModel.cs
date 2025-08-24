using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.DataInput
{
  
    [Serializable]
    public partial class OLTemplateModel
    {

        public PagedList<OLTemplateItemModel> PageList { get; set; }
    }

    public partial class OLTemplateItemModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string template_name { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public long created_byId { get; set; }

        public string createdname { get; set; }
        /// <summary>
        /// 字段代码
        /// </summary>
        public string field_code { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string InsertTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Updatetime { get; set; }
    }
}