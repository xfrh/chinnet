using FluentValidation.Attributes;
using ManageSystem.Core.Domain.Organism;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Admin.Models.Organism
{
    /// <summary>
    /// 细菌详情数据
    /// </summary>
    public partial class BacteriaDetailedDataModel: BaseEntityModel
    {
        /// <summary>
        /// 细菌名称
        /// </summary>
        [HtmlDisplayAttribute("名称", "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 0.002
        /// </summary>
        [HtmlDisplayAttribute("0.002", "0.002")]
        public string V0002 { get; set; }

        /// <summary>
        /// 0.004
        /// </summary>
        [HtmlDisplayAttribute("0.004", "0.004")]
        public string V0004 { get; set; }

        /// <summary>
        /// 0.008
        /// </summary>
        [HtmlDisplayAttribute("0.008", "0.008")]
        public string V0008 { get; set; }

        /// <summary>
        /// 0.016
        /// </summary>
        [HtmlDisplayAttribute("0.016", "0.016")]
        public string V0016 { get; set; }

        /// <summary>
        /// 0.032
        /// </summary>
        [HtmlDisplayAttribute("0.032", "0.032")]
        public string V0032 { get; set; }

        /// <summary>
        /// 0.064
        /// </summary>
        [HtmlDisplayAttribute("0.064", "0.064")]
        public string V0064 { get; set; }

        /// <summary>
        /// 0.125
        /// </summary>
        [HtmlDisplayAttribute("0.125", "0.125")]
        public string V0125 { get; set; }

        /// <summary>
        /// 0.25
        /// </summary>
        [HtmlDisplayAttribute("0.25", "0.25")]
        public string V0025 { get; set; }

        /// <summary>
        /// 0.5
        /// </summary>
        [HtmlDisplayAttribute("0.5", "0.5")]
        public string V0005 { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        [HtmlDisplayAttribute("1", "1")]
        public string V1001 { get; set; }

        /// <summary>
        /// 2
        /// </summary>
        [HtmlDisplayAttribute("2", "2")]
        public string V1002 { get; set; }

        /// <summary>
        /// 4
        /// </summary>
        [HtmlDisplayAttribute("4", "4")]
        public string V1004 { get; set; }

        /// <summary>
        /// 8
        /// </summary>
        [HtmlDisplayAttribute("8", "8")]
        public string V1008 { get; set; }

        /// <summary>
        /// 16
        /// </summary>
        [HtmlDisplayAttribute("16", "16")]
        public string V1016 { get; set; }

        /// <summary>
        /// 32
        /// </summary>
        [HtmlDisplayAttribute("32", "32")]
        public string V1032 { get; set; }

        /// <summary>
        /// 64
        /// </summary>
        [HtmlDisplayAttribute("64", "64")]
        public string V1064 { get; set; }

        /// <summary>
        /// 128
        /// </summary>
        [HtmlDisplayAttribute("128", "128")]
        public string V1128 { get; set; }

        /// <summary>
        /// 256
        /// </summary>
        [HtmlDisplayAttribute("256", "256")]
        public string V1256 { get; set; }

        /// <summary>
        /// 512
        /// </summary>
        [HtmlDisplayAttribute("512", "512")]
        public string V1512 { get; set; }


        /// <summary>
        /// ECOFF
        /// </summary>
        [HtmlDisplayAttribute("ECOFF", "ECOFF")]
        public string Ecoff { get; set; }


        /// <summary>
        /// Distributions
        /// </summary>
        [HtmlDisplayAttribute("Distributions", "Distributions")]
        public string Distributions { get; set; }

        /// <summary>
        /// Observations
        /// </summary>
        [HtmlDisplayAttribute("Observations", "Observations")]
        public string Observations { get; set; }

        /// <summary>
        /// 细菌类型关联ID
        /// </summary>
        [HtmlDisplayAttribute("细菌类型", "细菌类型")]
        public long OrganismTypeId { get; set; }

    }
}
