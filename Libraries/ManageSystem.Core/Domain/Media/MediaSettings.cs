using ManageSystem.Core.Configuration;

namespace ManageSystem.Core.Domain.Media
{
    public class MediaSettings : ISettings
    {
        public int AvatarPictureSize { get; set; }
        public int ProductThumbPictureSize { get; set; }
        public int ProductDetailsPictureSize { get; set; }
        public int ProductThumbPictureSizeOnProductDetailsPage { get; set; }
        public int AssociatedProductPictureSize { get; set; }
        public int CategoryThumbPictureSize { get; set; }
        public int ManufacturerThumbPictureSize { get; set; }
        public int VendorThumbPictureSize { get; set; }
        public int CartThumbPictureSize { get; set; }
        public int MiniCartThumbPictureSize { get; set; }
        public int AutoCompleteSearchThumbPictureSize { get; set; }

        public bool DefaultPictureZoomEnabled { get; set; }

        public int MaximumImageSize { get; set; }

        /// <summary>
        /// 获取或设置一个默认的用于图像生成质量
        /// </summary>
        public int DefaultImageQuality { get; set; }

        /// <summary>
        /// 木屐或设置一个影响无锡市指示是否单一(/内容/图片/拇指)或多个(/内容/图片/拇指/ 001 /和/内容/图片/拇指/ 002 /)目录将用于图片的拇指
        /// </summary>
        public bool MultipleThumbDirectories { get; set; }

    }
}