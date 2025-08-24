using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Web.Models.Medicine
{
    public class UploadFile
    {
        /// <summary>
        /// 文件上传后保存的服务器路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string Savepath(string path, string fileName)
        {
            string serverPath = HttpContext.Current.Server.MapPath(path) + fileName;
            return serverPath;
        }
    }
}