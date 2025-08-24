using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Model
{
    /// <summary>
    /// 网站站点
    /// </summary>
    public class WebItemModel
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 检测地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 网站IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 网站备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 目标<br />
        /// 1: CheckWebStatusJob<br />
        /// 2: ExecPingfenJob
        /// </summary>
        public int Target { get; set; }
    }
}
