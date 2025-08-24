using QuartzService.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace QuartzService
{
    class Program
    {
        static void Main(string[] args)
        {
      
            string va = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            //加载需要检查的网站列表
            WebService.GetWebListByConfig();

            //加载需要接受短信的用户列表
            WebService.GetUserbListByConfig();

            HostFactory.Run(x =>
            {
                x.UseLog4Net();

                x.Service<ServiceRunner>();

                x.SetDescription("【Chinet】自动化作业服务");
                x.SetDisplayName("Chinet 相关的作业服务");
                x.SetServiceName("Chinet 相关的作业服务");

                x.EnablePauseAndContinue();
            });
        }

    }
}
