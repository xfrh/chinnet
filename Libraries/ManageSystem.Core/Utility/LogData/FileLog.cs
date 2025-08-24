
using System;
using System.IO;
using System.Text;
using System.Web;


namespace ManageSystem.Core.Utility
{
    public class FileLog
    {
        private static readonly object writeFile = new object();

        /// <summary>
        ///将错误信息写到指定的错误页面上，注意网站根目录下面需要有Debug.htm页面
        /// </summary>
        /// <param name="debugstr"></param>
        public static void WriteToDocument(string debugstr)
        {
            FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Debug.htm"), System.IO.FileMode.Append, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine("<hr style='color:#666'><div width='100%' style='background:#666;color:#ffffff;padding-left:5px;padding-right:5px;padding-top:5px;padding-bottom:5px'>错误页面：" + System.Web.HttpContext.Current.Request.RawUrl + "<br>Beg时间：" + DateTime.Now.ToString() + "<br>Beg内容：" + debugstr + "</div>");
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// 传入exception 对象，写入日志
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteException(Exception exception)
        {
            WriteLog(exception.ToString());
        }

        /// <summary>
        /// 在本地写入错误日志
        /// </summary>
        /// <param name="exception"></param> 错误信息
        public static void WriteLog(string debugstr)
        {
            lock (writeFile)
            {
                FileStream fs = null;
                StreamWriter sw = null;

                try
                {
                    string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    string folder = HttpContext.Current.Server.MapPath("~/App_Data/Log");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    fs = new FileStream(folder + "/" + filename, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine("错误页面：" + System.Web.HttpContext.Current.Request.RawUrl + "\r\n Bug时间：" + DateTime.Now.ToString() + "\r\n Bug内容：" + debugstr + "\r\n");
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Dispose();
                        sw = null;
                    }
                    if (fs != null)
                    {
                        //     fs.Flush();
                        fs.Dispose();
                        fs = null;
                    }
                }
            }
        }





    }
}