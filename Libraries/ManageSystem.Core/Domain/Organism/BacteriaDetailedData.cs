using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Organism
{
    /// <summary>
    /// 细菌详情数据
    /// </summary>
    public partial class BacteriaDetailedData : BaseEntity
    {
        /// <summary>
        /// 细菌名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 0.002
        /// </summary>
        public string V0002 { get; set; }

        /// <summary>
        /// 0.004
        /// </summary>
        public string V0004 { get; set; }

        /// <summary>
        /// 0.008
        /// </summary>
        public string V0008 { get; set; }

        /// <summary>
        /// 0.016
        /// </summary>
        public string V0016 { get; set; }

        /// <summary>
        /// 0.032
        /// </summary>
        public string V0032 { get; set; }

        /// <summary>
        /// 0.064
        /// </summary>
        public string V0064 { get; set; }

        /// <summary>
        /// 0.125
        /// </summary>
        public string V0125 { get; set; }

        /// <summary>
        /// 0.25
        /// </summary>
        public string V0025 { get; set; }

        /// <summary>
        /// 0.5
        /// </summary>
        public string V0005 { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        public string V1001 { get; set; }

        /// <summary>
        /// 2
        /// </summary>
        public string V1002 { get; set; }

        /// <summary>
        /// 4
        /// </summary>
        public string V1004 { get; set; }

        /// <summary>
        /// 8
        /// </summary>
        public string V1008 { get; set; }

        /// <summary>
        /// 16
        /// </summary>
        public string V1016 { get; set; }

        /// <summary>
        /// 32
        /// </summary>
        public string V1032 { get; set; }

        /// <summary>
        /// 64
        /// </summary>
        public string V1064 { get; set; }

        /// <summary>
        /// 128
        /// </summary>
        public string V1128 { get; set; }

        /// <summary>
        /// 256
        /// </summary>
        public string V1256 { get; set; }

        /// <summary>
        /// 512
        /// </summary>
        public string V1512 { get; set; }


        /// <summary>
        /// ECOFF
        /// </summary>
        public string Ecoff { get; set; }


        /// <summary>
        /// Distributions
        /// </summary>
        public string Distributions { get; set; }

        /// <summary>
        /// Observations
        /// </summary>
        public string Observations { get; set; }

        /// <summary>
        /// 细菌类型关联ID
        /// </summary>
        public long OrganismTypeId { get; set; }

    }
}
