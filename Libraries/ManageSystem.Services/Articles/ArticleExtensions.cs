using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Services.Common;
using ManageSystem.Core.Infrastructure;

namespace ManageSystem.Services.Articles
{
    /// <summary>
    /// 操作类 ，数据库表名：Article 
    /// </summary>
    public static partial class ArticleExtensions
    {


        /// <summary>
        /// 获取文章封面图片的完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetCoverImage(string path)
        {
            if (!string.IsNullOrWhiteSpace(path)) return WebConfigService.ImageUrl + path;

            return WebConfigService.ImageUrl + @"Content\upload\article\default.jpg";

        }


        /// <summary>
        /// 根据文件的后缀返回文章附件的类型，非规定的后缀将抛出异常
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static ArticleAttachmentType GetAttachmentType(string ext)
        {
            if (string.IsNullOrWhiteSpace(ext)) throw new Exception("后缀不能为空");

            switch (ext.ToLower())
            {
                case "jpg":
                case "png":
                case "gif":
                case "bmp":
                case "jpeg":
                    return ArticleAttachmentType.Image;
                case "mp4":
                case "flv":
                    return ArticleAttachmentType.Video;
                case "mp3":
                    return ArticleAttachmentType.Audio;
                case "txt":
                case "doc":
                case "docx":
                case "xlsx":
                case "xls":
                case "rar":
                case "zip":
                case "7z":
                    return ArticleAttachmentType.Other;
            }

            throw new Exception("不支持文件后缀："+ ext.ToLower());
        }


    }
}
