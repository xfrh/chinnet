using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Framework.Upload
{
   public class UploadHelper
    {

        /// <summary>
        /// 获取错误的返回对象
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static UploadResult GetErrorUploadResult(string errorMessage)
        {
            UploadResult model = new UploadResult() {
                ErrorMessage = errorMessage,
                 IsSuccess=false
            };

            return model;
        }

        /// <summary>
        /// 获取名称的文件后缀名
        /// </summary>
        /// <param name="name"></param>
        /// <returns>错误将返回空字符串，不包含最后的.，返回的是小写形式</returns>
        public static string GetExtensionName(string name)
        {
            try
            {
                string ext = name.Substring(name.LastIndexOf(".")+1, name.Length - (name.LastIndexOf(".")+1));

                return ext.ToLower();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
