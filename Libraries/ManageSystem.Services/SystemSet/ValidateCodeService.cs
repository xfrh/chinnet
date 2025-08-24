using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.SystemSet;
using System.Net;
using System.Web;
using ManageSystem.Services.Configuration;
using ManageSystem.Core.Utility;

namespace ManageSystem.Services.SystemSet
{
    /// <summary>
    /// 操作类 ，数据库表名：ValidateCode 
    /// </summary>
    public partial class ValidateCodeService : BaseService<ValidateCode>, IValidateCodeService
    {

        public ValidateCodeService(IRepository<ValidateCode> repository) : base(repository)
        {

        }



        /// <summary>
        /// 获取一个随机的验证码
        /// </summary>
        /// <param name="type">验证码类型</param>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        public string GetCode(ValidateCodeType type, int length = 6)
        {
            String result = "";
            String strTable = "";
            switch (type)
            {
                case ValidateCodeType.Phone:
                    strTable = "1234567890";
                    break;
                case ValidateCodeType.Email:
                    strTable = "1234567890abcdefghijkmnpqrstuvwxyz";
                    break;
                case ValidateCodeType.RegistPhone:
                    strTable = "1234567890";
                    break;
            }

            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, strTable.Length);
                result += strTable[index];
            }

            return result;
        }


        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="code">短信编码</param>
        /// <param name="tel">电话号码</param>
        /// <returns>success 表示成功，其他则返回相对应的错误提示</returns>
        public string SendPhoneMessage(string type, string code, string tel)
        {
            try
            {
                string key = WebSettingService.GetWebSMS();

                //【CHINET】您的验证码是#code#。如非本人操作，请忽略本短信
                string tpl_value = HttpUtility.UrlEncode("#code#=" + code);
                string para = "&mobile=" + tel + "&tpl_id=210096&tpl_value=" + tpl_value;
                string url = "http://v.juhe.cn/sms/send?key=" + key + "&dtype=json" + para;

                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] b = wc.DownloadData(url);
                string s = Encoding.GetEncoding("utf-8").GetString(b);

                SmsApiResult result = s.DeserializeObject<SmsApiResult>();
                if (result.error_code == 0) return "success";

                return result.reason;
            }
            catch (Exception ex)
            {
                return "系统错误，请联系管理员";
            }
        }


        /// <summary>
        /// 发送验证码至邮箱
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public string SendEmailMessage(string email, string code)
        {
            try
            {
                //发送邮件
                string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
                string servicePassword = ConfigHelper.GetConfigString("message.email.password");
                int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
                bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
                string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
                string displayname = ConfigHelper.GetConfigString("message.email.displayname");

                string body = $@"<div style='width: 800px;margin: 0px auto;'>
                      <h2 style='width:100%;margin:0 auto;font-family:微软雅黑;font-size:12px;line-height:24px;margin-top:9px;color:#333'>亲爱的用户：</h2>
                      <p style='width:100%;font-family:微软雅黑;font-size:12px;line-height:24px;margin-top:9px;color:#333'>您正在申请找回CHINET数据云会员登录密码，请将下面的验证码输入到找回密码的步骤中。账户安全很重要，请小心保管您的密码喔！</p>
                      <h2 style='width:100%;margin:0 auto;font-family:微软雅黑;font-size:20px;line-height:24px;margin-top:9px;color:#333'>{code}</h2>
                     <p style='width:100%;margin:0 auto;font-family:微软雅黑;font-size:12px;line-height:24px;margin-top:9px;color:#333'>如果您没有申请找回登录密码，请忽略此邮件。祝您使用愉快，谢谢。</p>
             </div>";

                bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { email }, "CHINET数据云找回密码服务", body, true);

                if (result) return "success";

                return "邮件发送失败";
            }
            catch (Exception)
            {
                return "系统错误";
            }
        }
    }
}
