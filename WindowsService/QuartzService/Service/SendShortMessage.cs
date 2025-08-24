using QuartzService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace QuartzService.Service
{
    public class SendShortMessage
    {
   
        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="code">短信编码</param>
        /// <param name="tel">电话号码</param>
        /// <returns>success 表示成功，其他则返回相对应的错误提示</returns>
        public static string Send(string type, string code, string tel)
        {
            try
            {
                string key = "0e3630dfc4e2b1b7d93d19468a523233";

                string tpl_value = HttpUtility.UrlEncode("#code#=" + code);
                string para = "&mobile=" + tel + "&tpl_id=4991&tpl_value=" + tpl_value;
                string url = "http://v.juhe.cn/sms/send?key=" + key + "&dtype=json" + para;

                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] b = wc.DownloadData(url);
                string s = Encoding.GetEncoding("utf-8").GetString(b);

                SmsApiResultModel result =JsonConvert.DeserializeObject<SmsApiResultModel>(s);
                if (result.error_code == 0) return "success";

                return result.reason;
            }
            catch (Exception ex)
            {
                return "系统错误，请联系管理员";
            }
        }

    }
}
