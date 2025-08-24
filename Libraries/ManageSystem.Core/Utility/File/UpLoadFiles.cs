using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using ManageSystem.Core.Extensions;

namespace ManageSystem.Core.Utility
{
    /// <summary>
    /// 上传文件的类型枚举
    /// </summary>
    public enum UpLoadType
    {
        [Description("jpg | gif | png | jpeg | bmp")]
        Image = 1,
        [Description("jpg | gif | png | txt | doc | xls")]
        File = 2,
        [Description("flv | mp4 | F4v | webm | m3u8")]
        Video = 3,
        [Description("xls | xlsx | dbf ")]
        Other1 = 4, //上传病例文件
        /// <summary>
        /// 不限
        /// </summary>
        Unlimited
    }

    /// <summary>
    /// 封装上传文件的结果
    /// </summary>
    public class UpLoadFileResult
    {
        /// <summary>
        /// 原始上传文件的名称
        /// </summary>
        public string OldName { get; set; }

        /// <summary>
        /// 新文件的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件的后缀，格式：jpg
        /// </summary>
        public string FileSuffix { get; set; }

        /// <summary>
        /// 保存文件路径，格式：/Content/Upload/MedicalData
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 文件路径加文件的名称，格式：/Content/Upload/MedicalData/1.jpg
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 文件长度，单位：KB
        /// </summary>
        public Double Length { get; set; }

        /// <summary>
        /// 上传结果
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// UpLoadFiles 的摘要说明。
    /// </summary>
    public class UpLoadFiles
    {
        public UpLoadFiles()
        {
        }


        /// <summary>
        /// 检查文件类型名称和所选择的格式是否匹配
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CheckFileExtension(string fileName, string upImgType)
        {
            // 判断格式
            string strEx = (FileUtil.GetExName(fileName)).ToLower();

            return !(upImgType.IndexOf(strEx.Replace(".", "")) < 0);
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="picpath">文件路径</param>
        /// <returns>返回值，True为删除成功，False为删除失败</returns>
        public static bool DeleteFile(string picpath)
        {
            bool flag = false; ;
            if (picpath != "" && File.Exists(System.Web.HttpContext.Current.Server.MapPath(picpath)))
            {
                try
                {
                    File.Delete(System.Web.HttpContext.Current.Server.MapPath(picpath));
                    flag = true;
                }
                catch
                {
                }
            }
            return flag;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="httpFile">HTML 控件</param>
        /// <param name="savePath">文件保存路径，相对路径</param>
        /// <param name="maxSize">文件最大尺寸，0：表示不限制</param>
        /// <param name="type">上传文件的类型，UploadType 枚举</param>
        /// <param name="fileName">保存文件的名称，如果是空，系统将自动生成</param>
        /// <returns></returns>
        public static UpLoadFileResult UpLoadFile(HttpPostedFileBase httpFile, string savePath, int maxSize, UpLoadType type, string fileName)
        {
            return UpLoadFile(httpFile, savePath, maxSize, type, fileName, "");
        }


        /// <summary>
        /// 上传文件和删除原始的文件（比如上传了新头像图片删除原始的图片）
        /// </summary>
        /// <param name="httpFile">HTML 控件</param>
        /// <param name="savePath">文件保存路径，相对路径</param>
        /// <param name="maxSize">文件最大尺寸，0：表示不限制</param>
        /// <param name="type">上传文件的类型，UploadType 枚举</param>
        /// <param name="fileName">保存文件的名称，如果是空，系统将自动生成</param>
        /// <param name="deleteFileName">删除文件的名称，文件名称+后缀，必须是和现在上传的文件在同一文件夹</param>
        /// <returns></returns>
        public static UpLoadFileResult UpLoadFile(HttpPostedFileBase httpFile, string savePath, int maxSize, UpLoadType type, string fileName, string deleteFileName)
        {
            UpLoadFileResult resultModel = new UpLoadFileResult();
            resultModel.OldName = httpFile.FileName;

            string tempFileName = "";

            fileName = string.IsNullOrEmpty(fileName) ? DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) : fileName;
            string error = "";
            try
            {
                if (httpFile == null || httpFile.ContentLength <= 0)
                {
                    error = "上传文件为空！";
                    throw new Exception();
                }

                if (maxSize > 0 && httpFile.ContentLength > maxSize * 1024)
                {
                    error = "上传文件不能超过 " + (maxSize / 1024.0).ToString() + " MB，请您重新选择合适的文件！";
                    throw new Exception();
                }

                // 判断文件格式
                string strEx = (FileUtil.GetExName(httpFile.FileName)).ToLower();

                if (type != UpLoadType.Unlimited)
                {
                    string typeValue = EnumExtension.GetEnumDescription(type);
                    if (typeValue.IndexOf(strEx.Replace(".", "")) < 0)
                    {
                        error = "上传文件格式不正确，正确格式为" + typeValue + "，请您重新选择合适的文件！";
                        throw new Exception();
                    }
                }


                //如果不存在就创建file文件夹
                if (Directory.Exists(HttpContext.Current.Server.MapPath(savePath)) == false)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(savePath));

                tempFileName = fileName + strEx;
                string newPath = HttpContext.Current.Server.MapPath(savePath + "/" + tempFileName);              
                if (File.Exists(newPath))
                {
                    string Path = HttpContext.Current.Server.MapPath(savePath + "/" + tempFileName);
                    string tempFileNames = fileName +DateTime.Now.ToString("yyyyMMddHHmmss") + strEx;
                    string Paths = HttpContext.Current.Server.MapPath(savePath + "/" + tempFileNames);
                    File.Move(Path, Paths);
                    //error = "文件名重复！";
                    //throw new Exception("文件名重复！");
                }

                //保存文件
                httpFile.SaveAs(newPath);

                //删除老的文件
                if (!string.IsNullOrEmpty(deleteFileName))
                {
                    try
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(savePath + "/" + deleteFileName));
                    }
                    catch (Exception)
                    {
                    }
                }

                resultModel.Length = Math.Round(Convert.ToDouble(httpFile.ContentLength / 1024), 2);
                resultModel.FileSuffix = strEx;
                resultModel.FullPath = savePath + "/" + tempFileName;
                resultModel.Name = tempFileName;
                resultModel.State = true;
                resultModel.Path = savePath;

            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(error))
                    error = "系统错误，请联系管理员！";
                resultModel.State = false;
            }

            resultModel.ErrorMessage = error;
            return resultModel;
        }

        /// <summary>
        /// 删除上传以后遗留的老图片，
        /// </summary>
        /// <param name="path">图片的相对路径地址，例："~/UploadFile/Ad/1.jpg"</param>
        /// <returns></returns>
        public static bool DeleteOldImage(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return false;
                path = HttpContext.Current.Server.MapPath(path);
                if (!File.Exists(path)) return false;

                File.Delete(path);
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }
    }


}


