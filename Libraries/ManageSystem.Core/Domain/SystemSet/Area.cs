using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.SystemSet
{
    public class Area : BaseEntity
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 简短名称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 层级位置
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public int Sort { get; set; }
    }
}
