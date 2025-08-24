using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Framework.Upload
{
    /// <summary>
    /// 上传成功以后的返回值
    /// </summary>
    public class UploadResult
    {
        /// <summary>
        /// 上传是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 上传失败以后的提示信息
        /// </summary>
        public string ErrorMessage { get; set; }


        /// <summary>
        /// 文件名称（原始）
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件的扩展名
        /// </summary>
        public string ExtensionName { get; set; }

        /// <summary>
        /// 文件名称（新的，保存在服务器的）
        /// </summary>
        public string FileNewName { get; set; }

        /// <summary>
        ///文件在服务器上的相对路径，包括文件夹和文件的名称 ，不带后缀系统会自动拼接
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件类型， 1：文件  2：图片
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 自定义的数据 1，可自由传递数据
        /// </summary>
        public string CustomValue1 { get; set; }

        /// <summary>
        /// 自定义的数据 2，可自由传递数据
        /// </summary>
        public string CustomValue2 { get; set; }

        /// <summary>
        /// 文件大小，单位KB
        /// </summary>
        public double FileSize { get; set; }
    }
}
