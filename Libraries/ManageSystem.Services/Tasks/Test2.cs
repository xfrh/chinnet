using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ManageSystem.Services.Tasks
{
    public class Test2 : ITask
    {
        private string filePath = @"E:\log.txt";
        public void Execute()
        {
            FileInfo fs = new FileInfo(filePath);
            StreamWriter sw = fs.AppendText();
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "，Test2 运行成功");

            sw.Flush();
            sw.Dispose();
            fs = null;
        }
    }
}
