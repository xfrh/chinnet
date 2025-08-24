using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Documents
{
    /// <summary>
    /// 资料下载，资料信息
    /// </summary>
    public class Document : BaseEntity
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用  0：禁用  1：启用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序编号，正序排列
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 所属资料<br />
        /// 存储格式如：1,2,3,4
        /// </summary>
        public string Owners { get; set; }

        /// <summary>
        /// 链接地址<br />
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 文件的完整地址
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 链接地址中显示的名称
        /// </summary>
        public string UrlName { get; set; }

        /// <summary>
        /// 链接地址，点击链接跳转地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 置顶标识
        /// </summary>
        public bool weChattop { get; set; }

        /// <summary>
        /// 所属卫星网
        /// </summary>
        public long Satellite { get; set; }
    }
}
