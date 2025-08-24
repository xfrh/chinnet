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
using Newtonsoft.Json;

namespace QuartzService.Jobs
{
    public sealed class CheckWebStatusJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CheckWebStatusJob));

        public void Execute(IJobExecutionContext context)
        {
            List<WebItemModel> ErrorList = new List<WebItemModel>();
            IEnumerable<WebItemModel> _webList = WebService.WebList.Where(r => r.Status && r.Target == 1);
            //检查状态
            foreach (var item in _webList)
            {
                try
                {
                    if (!item.Status) continue;

                    WebRequest myRequest = WebRequest.Create(item.Url);
                    myRequest.Timeout = 1000 * 60;//60秒

                    WebResponse myResponse = myRequest.GetResponse();
                    myResponse.Close();
                }
                catch
                {

                }
                System.Threading.Thread.Sleep(3000);
            }

            if (ErrorList != null && ErrorList.Any())
            {
                //发送短信
                StringBuilder sb = new StringBuilder();
                sb.Append("【Chinet】，上传的医学数据耐药性检查，执行自动化任务失败");
                foreach (var item in ErrorList)
                    sb.AppendFormat("名称：{0}，IP：{1}，地址：{2}；", item.Name, item.IP, item.Url);

                foreach (var user in WebService.UserList)
                {
                    if (!user.Status) continue;

                    try
                    {
                        SendShortMessage.Send("Register", sb.ToString(), user.Phone);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("上传的医学数据耐药性检查异常，发送的内容：" + user.Phone + "  " + sb.ToString() + " 错误信息：" + ex.ToString());
                    }
                }

                _logger.InfoFormat("上传的医学数据耐药性检查，任务执行成功，错误数据：" + JsonConvert.SerializeObject(ErrorList));
            }
        }
    }
}
