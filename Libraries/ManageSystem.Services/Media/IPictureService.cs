using System.Collections.Generic;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Media;

namespace ManageSystem.Services.Media
{
    /// <summary>
    /// Picture service interface
    /// </summary>
    public partial interface IPictureService
    {
        /// <summary>
        /// 得到了加载图片二进制根据图片存储设置
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <returns>Picture binary</returns>
        byte[] LoadPictureBinary(Picture picture);


        /// <summary>
        /// 获取默认图片URL
        /// </summary>
        /// <param name="targetSize">目标图像大小(最长)</param>
        /// <param name="defaultPictureType">默认的图片类型</param>
        /// <param name="storeLocation">存储位置URL;零使用自动确定当前存储位置</param>
        /// <returns>Picture URL</returns>
        string GetDefaultPictureUrl(int targetSize = 0, 
            PictureType defaultPictureType = PictureType.Entity,
            string storeLocation = null);

        /// <summary>
        /// 得到一个图片的URL
        /// </summary>
        /// <param name="pictureId">图像标识符</param>
        /// <param name="targetSize">目标图像大小(最长)</param>
        /// <param name="showDefaultPicture">一个值指示是否显示默认图片</param>
        /// <param name="storeLocation">存储位置URL;零使用自动确定当前存储位置</param>
        /// <param name="defaultPictureType">默认的图片类型</param>
        /// <returns>Picture URL</returns>
        string GetPictureUrl(long pictureId, 
            int targetSize = 0,
            bool showDefaultPicture = true, 
            string storeLocation = null, 
            PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// 得到一个图片的URL
        /// </summary>
        /// <param name="picture">实例图片</param>
        /// <param name="targetSize">目标图像大小(最长)</param>
        /// <param name="showDefaultPicture">一个值指示是否显示默认图片</param>
        /// <param name="storeLocation">存储位置URL;零使用自动确定当前存储位置</param>
        /// <param name="defaultPictureType">默认的图片类型</param>
        /// <returns>Picture URL</returns>
        string GetPictureUrl(Picture picture, 
            int targetSize = 0,
            bool showDefaultPicture = true, 
            string storeLocation = null, 
            PictureType defaultPictureType = PictureType.Entity);

        /// <summary>
        /// 拍局部路径
        /// </summary>
        /// <param name="picture">实例图片</param>
        /// <param name="targetSize">目标图像大小(最长)</param>
        /// <param name="showDefaultPicture">一个值指示是否显示默认图片</param>
        /// <returns></returns>
        string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true);

        /// <summary>
        ///得到一张照片
        /// </summary>
        /// <param name="pictureId">得到一张照片</param>
        /// <returns>Picture</returns>
        Picture GetPictureById(long pictureId);

        /// <summary>
        /// 删除照片
        /// </summary>
        /// <param name="picture">Picture</param>
        void DeletePicture(Picture picture);

        /// <summary>
        /// 得到图片的集合
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of pictures</returns>
        IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 插入一个图片
        /// </summary>
        /// <param name="pictureBinary">图片的二进制</param>
        /// <param name="mimeType">MIME类型的图片</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <param name="altAttribute">“img”“alt”属性的HTML元素</param>
        /// <param name="titleAttribute">“img”“标题”属性的HTML元素</param>
        /// <param name="isNew">一个值指示是否图片是新的</param>
        /// <param name="validateBinary">一个值指示是否验证提供二进制图片</param>
        /// <returns>Picture</returns>
        Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename, 
            string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true);

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="pictureId">图像标识符</param>
        /// <param name="pictureBinary">图片的二进制</param>
        /// <param name="mimeType">MIME类型的图片</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <param name="altAttribute">“img”“alt”属性的HTML元素</param>
        /// <param name="titleAttribute">“img”“标题”属性的HTML元素</param>
        /// <param name="isNew">一个值指示是否图片是新的</param>
        /// <param name="validateBinary">一个值指示是否验证提供二进制图片</param>
        /// <returns>Picture</returns>
        Picture UpdatePicture(long pictureId, byte[] pictureBinary, string mimeType,
            string seoFilename, string altAttribute = null, string titleAttribute = null,
            bool isNew = true, bool validateBinary = true);

        /// <summary>
        /// 更新一个SEO文件名的一幅画
        /// </summary>
        /// <param name="pictureId">图像标识符</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <returns>Picture</returns>
        Picture SetSeoFilename(long pictureId, string seoFilename);

        /// <summary>
        ///验证输入图片尺寸
        /// </summary>
        /// <param name="pictureBinary">二进制图片</param>
        /// <param name="mimeType">MIME类型</param>
        /// <returns>二进制图片或者抛出一个异常</returns>
        byte[] ValidatePicture(byte[] pictureBinary, string mimeType);

        /// <summary>
        /// 获取或设置一个值表示的图像是否应该存储在数据库中。
        /// </summary>
        bool StoreInDb { get; set; }
    }
}
