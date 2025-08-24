using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Models.Organism
{
    /// <summary>
    /// 细菌查询数据模型
    /// </summary>
    public class BacteriaModel : BaseEntityModel
    {
        /// <summary>
        /// 细菌名称
        /// </summary>
        [HtmlDisplayAttribute("名称", "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 细菌分类标识ID
        /// </summary>
        [HtmlDisplayAttribute("细菌分类标识ID", "细菌分类标识ID")]
        public long MedicalOrganismTypeId { get; set; }

        /// <summary>
        /// 细菌分类
        /// </summary>
        [HtmlDisplayAttribute("细菌分类", "细菌分类")]
        public IList<SelectListItem> MedicalOrganismType { get; set; }

        /// <summary>
        /// 细菌标识ID
        /// </summary>
        [HtmlDisplayAttribute("细菌标识ID", "细菌标识ID")]
        public long MedicalOrganismId { get; set; }

        /// <summary>
        /// 细菌
        /// </summary>
        [HtmlDisplayAttribute("细菌", "细菌")]
        public IList<SelectListItem> MedicalOrganism { get; set; }

        [HtmlDisplayAttribute("数据文件", "数据文件")]
        public HttpPostedFileBase DataFiles { get; set; }


        /// <summary>
        /// 细菌名称
        /// </summary>
        public string OrganismName { get; set; }

    }



}