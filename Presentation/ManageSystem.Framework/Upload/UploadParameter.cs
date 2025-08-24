using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ManageSystem.Framework.Upload
{

    /// <summary>
    /// 上传文件的参数
    /// </summary>
    public class UploadParameter
    {
        /// <summary>
        /// 上传文件的http请求
        /// </summary>
        public HttpFileCollectionBase HttpFile { get; set; }
        
        /// <summary>
        /// 上传的类型
        /// </summary>
        public UploadTypeEnum Type { get; set; }

        /// <summary>
        /// 上传文件的后缀限制，没有则为空，多个使用英文逗号分割，例如：jpg,bmp,txt,doc
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 上传到服务器以后新的文件名称，如果为空，则使用原始的名称
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// 文件大小限制，单位为KB，0表示不限制
        /// </summary>
        public long MaxLength { get; set; }

        /// <summary>
        /// 文件保存在服务器的相对路径， 格式：/Content/Upload/Meetings/
        /// </summary>
        public string FilePath { get; set; }


    }
}
