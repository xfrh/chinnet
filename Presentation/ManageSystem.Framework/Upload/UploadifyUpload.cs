using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ManageSystem.Framework.Upload
{
    /// <summary>
    /// 使用Uploadify 插件上传文件
    /// </summary>
    public class UploadifyUpload : IUpload
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public UploadResult Upload(UploadParameter parameter)
        {
            try
            {
                if (parameter == null) return UploadHelper.GetErrorUploadResult("上传参数不能为空");

                if (parameter.HttpFile == null || parameter.HttpFile.Count <= 0) return UploadHelper.GetErrorUploadResult("请选择上传文件");

                var file = parameter.HttpFile[0];
                if (file == null || file.ContentLength <= 0) return UploadHelper.GetErrorUploadResult("选择的文件大小为0");

                //文件大小检查
                if (parameter.MaxLength > 0)
                {
                    long size = file.ContentLength / 1024;   //结果为KB   1KB=1024 * 1字节
                    if (size > parameter.MaxLength) return UploadHelper.GetErrorUploadResult("请选择上传大于了限制");
                }

                if (!string.IsNullOrWhiteSpace(parameter.Extension))
                {
                    //文件后缀检查
                    string extension = UploadHelper.GetExtensionName(file.FileName);
                    if (string.IsNullOrWhiteSpace(extension)) return UploadHelper.GetErrorUploadResult("文件类型错误");

                    if (!parameter.Extension.Contains(extension)) return UploadHelper.GetErrorUploadResult("文件类型错误");
                }

                //返回参数
                UploadResult model = new UploadResult();
                model.IsSuccess = true;
                model.ErrorMessage = "";
                model.FileName = file.FileName;
                model.ExtensionName = UploadHelper.GetExtensionName(file.FileName);
                model.FileNewName = string.IsNullOrWhiteSpace(parameter.NewFileName) ? file.FileName : (parameter.NewFileName + "." + model.ExtensionName);
                model.FilePath = parameter.FilePath + model.FileNewName;
                model.FileSize = Math.Round(Convert.ToDouble(file.ContentLength / 1024), 2);

                //保存文件
                string serverPath = HttpContext.Current.Server.MapPath(parameter.FilePath) + model.FileNewName;
                if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                   Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
              
                file.SaveAs(serverPath);

                return model;
            }
            catch (Exception ex)
            {
                return UploadHelper.GetErrorUploadResult(ex.Message);
            }
        }

    }


}
