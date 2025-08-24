//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Hairihan TECH, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;

namespace ManageSystem.Core.Utility
{
    /// <summary>
    ///	FileUtil
    /// 文件帮助类
    /// </author> 
    /// </summary>
    public class FileUtil
    {
    
        /// <summary>
        /// 有善的文件大小现实方式
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <returns>现实方式</returns>
        public static string GetFriendlyFileSize(double fileSize)
        {
            string returnValue = string.Empty;
            if (fileSize < 1024)
            {
                returnValue = fileSize.ToString("F1") + "Byte";
            }
            else
            {
                fileSize = fileSize / 1024;
                if (fileSize < 1024)
                {
                    returnValue = fileSize.ToString("F1") + "KB";
                }
                else
                {
                    fileSize = fileSize / 1024;
                    if (fileSize < 1024)
                    {
                        returnValue = fileSize.ToString("F1") + "M";
                    }
                    else
                    {
                        fileSize = fileSize / 1024;
                        returnValue = fileSize.ToString("F1") + "GB";
                    }
                }
            }
            return returnValue;
        }
        

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>字节</returns>
        public static byte[] GetFile(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] file = binaryReader.ReadBytes(((int)fileStream.Length));
            binaryReader.Close();
            fileStream.Close();
            return file;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="File">文件</param>
        /// <param name="fileName">文件名</param>
        public static void SaveFile(byte[] file, string fileName)
        {
            string directoryName = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            fileStream.Write(file, 0, file.Length);
            fileStream.Close();
        }

        public static byte[] ImageToByte(Image Image)
        {
            MemoryStream memoryStream = new MemoryStream();
            Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
            byte[] file = memoryStream.GetBuffer();
            memoryStream.Close();
            return file;
        }

        public static Image ByteToImage(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream = new System.IO.MemoryStream(buffer);
            Image image = Image.FromStream(memoryStream);
            memoryStream.Close();
            return image;
        }

        /// <summary>
        /// 向创建二进制文件，并向其中写入二进制信息
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="message">文件文本内容</param>
        public static void WriteBinaryFile(string fileName, string message)
        {
            Console.WriteLine("写入二进制文件信息开始。");
            FileStream fileStream = null;
            BinaryWriter binaryWriter = null;
            try
            {
                // 首先判断，文件是否已经存在
                if (System.IO.File.Exists(fileName))
                {
                    // 如果文件已经存在，那么删除掉.
                   System.IO.File.Delete(fileName);
                }
                // 注意第2个参数：
                // FileMode.Create 指定操作系统应创建新文件。如果文件已存在，它将被覆盖。
                fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                binaryWriter = new BinaryWriter(fileStream);

                // 写入测试数据.
                // bw.Write(0x20);
                // bw.Write(1024.567d);
                // bw.Write(1024);

                // 注意，二进制写入 字符串信息的时候
                // 带长度前缀的字符串通过在字符串前面放置一个包含该字符串长度的字节或单词，来表示该字符串的长度。
                // 此方法首先将字符串长度作为一个四字节无符号整数写入，然后将这些字符写入流中。

                // 这里将先写入一个 0x07， 然后再写入 abcdefg
                // bw.Write("abcdefg");
                byte[] binaryBytes = System.Text.Encoding.UTF8.GetBytes(message);
                // binaryWriter.Write(binaryBytes);
                binaryWriter.Write(binaryBytes);

                // 关闭文件.
                binaryWriter.Close();
                fileStream.Close();

                binaryWriter = null;
                fileStream = null;
            }
            catch (Exception ex)
            {
                // Console.WriteLine("在写入文件的过程中，发生了异常！");
                // Console.WriteLine(ex.Message);
                // Console.WriteLine(ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (binaryWriter != null)
                {
                    try
                    {
                        binaryWriter.Close();
                    }
                    catch
                    {
                        // 最后关闭文件，无视 关闭是否会发生错误了.
                    }
                }
                if (fileStream != null)
                {
                    try
                    {
                        fileStream.Close();
                    }
                    catch
                    {
                        // 最后关闭文件，无视 关闭是否会发生错误了.
                    }
                }
            }
            // Console.WriteLine("写入二进制文件信息结束。");
        }
        
        /// <summary>
        /// 测试向从二进制文件中读取数据，并显示到终端上.
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件内容</returns>
        public static string ReadBinaryFile(string fileName)
        {
            string message = string.Empty;
            // Console.WriteLine("读取二进制文件信息开始。");
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                // 首先判断，文件是否已经存在
                if (!System.IO.File.Exists(fileName))
                {
                    // 如果文件不存在，那么提示无法读取！
                    // Console.WriteLine("二进制文件{0}不存在！", fileName);
                    return string.Empty;
                }


                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);

                // int a = br.ReadInt32();
                // double b = br.ReadDouble();
                // int c = br.ReadInt32();
                // byte len = br.ReadByte();
                // char[] d = br.ReadChars(len);

                // Console.WriteLine("第一个数据:{0}", a);
                // Console.WriteLine("第二个数据:{0}", b);
                // Console.WriteLine("第三个数据:{0}", c);
                // Console.WriteLine("第四个数据 (长度为{0}):", len);
                // foreach (char ch in d)
                // {
                //    Console.Write(ch);
                // }
                // Console.WriteLine();
                //完整的读取文件类容需要获取文件的长度
                int count = (int)fs.Length;
                byte[] buffer = new byte[count];
                br.Read(buffer, 0, buffer.Length);
                message = Encoding.Default.GetString(buffer);
               // message = br.ReadString();
                
                // 读取完毕，关闭.
                br.Close();
                fs.Close();

                br = null;
                fs = null;
            }
            catch (Exception ex)
            {
                // Console.WriteLine("在读取文件的过程中，发生了异常！");
                // Console.WriteLine(ex.Message);
                // Console.WriteLine(ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (br != null)
                {
                    try
                    {
                        br.Close();
                    }
                    catch
                    {
                        // 最后关闭文件，无视 关闭是否会发生错误了.
                    }
                }

                if (fs != null)
                {
                    try
                    {
                        fs.Close();
                    }
                    catch
                    {
                        // 最后关闭文件，无视 关闭是否会发生错误了.
                    }
                }
            }
            // Console.WriteLine("读取二进制文件信息结束。");
            return message;
        }

        /// <summary>
        /// 删除文件 
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <returns>bool 是否删除成功</returns>
        public static bool DeleteFile(string fileName)
        {
            if (File.Exists(fileName) == true)
            {
                if (File.GetAttributes(fileName) == FileAttributes.Normal)
                {
                    File.Delete(fileName);
                }
                else
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }
                return true;
            }
            else//文件不存在
            {
                return false;
            }
        }

        /// <summary>
        /// 根据文件名，得到文件的大小，单位分别是GB/MB/KB
        /// </summary>
        /// <param name="FileFullPath">文件名</param>
        /// <returns>返回文件大小</returns>
        public static string GetFileSize(string fileName)
        {
            if (File.Exists(fileName) == true)
            {
                FileInfo fileinfo = new FileInfo(fileName);
                long fl = fileinfo.Length;
                if (fl > 1024 * 1024 * 1024)
                {
                    return Convert.ToString(Math.Round((fl + 0.00) / (1024 * 1024 * 1024), 2)) + " GB";
                }
                else if (fl > 1024 * 1024)
                {
                    return Convert.ToString(Math.Round((fl + 0.00) / (1024 * 1024), 2)) + " MB";
                }
                else
                {
                    return Convert.ToString(Math.Round((fl + 0.00) / 1024, 2)) + " KB";
                }
            }
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// 读取文件文件内容
        /// 文本格式必须为utf-8
        /// (ansi格式容易产生乱码)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetTextFileContent(string fileName)
        {
            StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("utf-8"));
            string message = sr.ReadToEnd();

            return message;
        }

        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExName(string fileName)
        {
            int dotPos = fileName.LastIndexOf(".");
            return fileName.Substring(dotPos);
        }

        /// <summary>
        /// Web 文件下载
        /// </summary>
        /// <param name="FullFileName"></param>
        public static void FileDownload(string fullFileName)
        {
            #region  调用代码示例
            /*
            * string path = "/UploadFile/ProviderLicense/text.txt" ;
            * Files.FileDownload(this.Server.MapPath(path));
              */
            #endregion

            FileInfo DownloadFile = new FileInfo(fullFileName); //设置要下载的文件
            HttpContext.Current.Response.Clear(); //清除缓冲区流中的所有内容输出
            HttpContext.Current.Response.ClearHeaders(); //清除缓冲区流中的所有头
            HttpContext.Current.Response.Buffer = false; //设置缓冲输出为false
            //设置输出流的 HTTP MIME 类型为application/octet-stream
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            //将 HTTP 头添加到输出流
            string s = HttpUtility.UrlEncode(System.Text.UTF8Encoding.UTF8.GetBytes(DownloadFile.Name));
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + s);// HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.UTF8));

            HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());

            //将指定的文件直接写入 HTTP 内容输出流。
            HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
            HttpContext.Current.Response.Flush(); //向客户端发送当前所有缓冲的输出
            HttpContext.Current.Response.End(); //将当前所有缓冲的输出发送到客户端

        }

    }
}