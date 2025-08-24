using ManageSystem.Core;
using System;

namespace ManageSystem.Core.Domain.Media
{
    /// <summary>
    /// 代表一个下载
    /// </summary>
    public partial class Download : BaseEntity
    {
        /// <summary>
        /// 获取或设置一个GUID
        /// </summary>
        public Guid DownloadGuid { get; set; }

        /// <summary>
        /// 获取或设置一个值,指出是否应该使用DownloadUrl属性
        /// </summary>
        public bool UseDownloadUrl { get; set; }

        /// <summary>
        /// 获取或设置一个下载的URL
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        ///获取或设置下载二进制文件
        /// </summary>
        public byte[] DownloadBinary { get; set; }

        /// <summary>
        /// 下载的mime类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 下载的文件名
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// 获取或设置扩展
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 获取或设置一个值,指出是否下载是新的
        /// </summary>
        public bool IsNew { get; set; }
    }

}
