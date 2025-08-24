using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Framework.Upload
{
    /// <summary>
    /// 前台上传文件接口
    /// </summary>
    public interface IUpload
    {
        /// <summary>
        /// 上传文件接口
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        UploadResult Upload(UploadParameter parameter);

    }
}
