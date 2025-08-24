using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Cre
{
   public class CreProject_log
    {
        /// <summary>
        /// 日志id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 浏览器名称
        /// </summary>
        public string BrowserName { get; set; }
        /// <summary>
        /// 电脑ip
        /// </summary>
        public string UserinfoId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserinfoName { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 细节
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        //Source 
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 数据id
        /// </summary>
        public long CREdataId { get; set; }
    }
}
