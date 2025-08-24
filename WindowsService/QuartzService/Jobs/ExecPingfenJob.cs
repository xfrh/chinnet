using log4net;
using Quartz;
using QuartzService.Model;
using QuartzService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Jobs
{
    public sealed class ExecPingfenJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ExecPingfenJob));
        public void Execute(IJobExecutionContext context)
        {
            IEnumerable<WebItemModel> _webList = WebService.WebList.Where(r => r.Status && r.Target == 2);
            //检查状态
            foreach (var item in _webList)
            {
                try
                {
                    if (!item.Status) continue;

                    WebRequest myRequest = WebRequest.Create(item.Url);
                    myRequest.Timeout = 1000 * 600; // 600秒

                    WebResponse myResponse = myRequest.GetResponse();
                    myResponse.Close();
                }
                catch
                {

                }
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
