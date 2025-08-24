namespace ManageSystem.Core.Domain.Media
{
    /// <summary>
    /// 代表了一个图片
    /// </summary>
    public partial class Picture : BaseEntity
    {
        /// <summary>
        ///获取或设置图片的二进制
        /// </summary>
        public byte[] PictureBinary { get; set; }

        /// <summary>
        /// 获取或设置mime类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        ///获取或设置SEO friednly图片的文件名
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// 获取或设置为“img”“alt”属性的HTML元素。如果为空,则将使用默认的规则(如产品名称)
        /// </summary>
        public string AltAttribute { get; set; }

        /// <summary>
        ///获取或设置“标题”属性为“img”HTML元素。如果为空,则将使用默认的规则(如产品名称)
        /// </summary>
        public string TitleAttribute { get; set; }

        /// <summary>
        /// 获取或设置一个值,指出是否图片是新的
        /// </summary>
        public bool IsNew { get; set; }
    }
}
