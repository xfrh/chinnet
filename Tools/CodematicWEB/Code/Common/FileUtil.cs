using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CodematicWEB.Code.Common
{
    public class FileUtil
    {

        /// <summary>
        /// 将内容写入到文件
        /// </summary>
        /// <param name="filePath">文件的绝对地址</param>
        /// <param name="content">文件的内容</param>
        public static void WriteFile(string filePath, string content)
        {
            StreamWriter sw = null;
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate);
                sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(content);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                if (fs != null)
                {

                    fs.Close();
                }
            }
        }
    }
}