using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Framework.Upload
{

    /// <summary>
    /// 上传文件的后缀
    /// </summary>
    public class UploadFileExtension
    {
        /// <summary>
        /// 文件的后缀
        /// </summary>
        public static string FileExtension
        {
            get
            {
                return "txt,doc,docx,pdf,xlsx,xls,rar,7z";
            }
        }

        /// <summary>
        /// 图片的后缀
        /// </summary>
        public static string ImageExtension
        {
            get
            {
                return "jpg,jpeg,bmp,gif,png";
            }
        }

    }
}
