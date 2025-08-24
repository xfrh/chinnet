using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ManageSystem.Core.Utility
{
    /// <summary>
    /// 邮件相关助手类
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// 服务器邮箱地址， 发送人的邮箱地址
        /// </summary>
        private string _serviceEmail = "";

        /// <summary>
        /// 发送人的邮箱地址对应的密码
        /// </summary>
        private string _servicePassword = "";

        /// <summary>
        /// 是否启用SSL加密
        /// </summary>
        private bool _serviceEnableSsl = false;

        /// <summary>
        /// SMTP服务器的邮件发送端口，默认25
        /// </summary>
        private int _servicePort = 25;

        /// <summary>
        /// 邮件服务器的STMP地址
        /// </summary>
        private string _serviceSMTP = "";

        /// <summary>
        /// 邮件显示的名称
        /// </summary>
        private string _serviceDisplayName = "";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceEmail"></param>
        /// <param name="servicePassword"></param>
        /// <param name="serviceSMTP"></param>
        /// <param name="servicePort"></param>
        /// <param name="dsplayName"></param>
        public EmailHelper(string serviceEmail, string servicePassword, string serviceSMTP, string dsplayName, int servicePort = 25, bool serviceEnableSsl = false)
        {
            this._serviceEmail = serviceEmail;
            this._servicePassword = servicePassword;
            this._serviceSMTP = serviceSMTP;
            this._servicePort = servicePort;
            this._serviceEnableSsl = serviceEnableSsl;
            this._serviceDisplayName = dsplayName;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailArray">接受邮件的邮箱数组，例如：new string[] { "******@qq.com","12345678@qq.com"};</param>
        /// <param name="title">邮件的标题</param>
        /// <param name="content">邮件的内容</param>
        /// <param name="isHtml">发送的内容是否是html</param>
        /// <param name="ccArray">抄送的邮箱数组，例如：new string[] { "******@qq.com","12345678@qq.com"};</param>
        /// <param name="attachmentsPathArray">附件文件的路径集合</param>
        /// <returns></returns>
        public bool Send(string[] mailArray, string title, string content, bool isHtml = true, string[] ccArray = null, string[] attachmentsPathArray = null)
        {
            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(this._serviceEmail, this._serviceDisplayName);
            MailMessage myMail = new MailMessage();

            //向收件人地址集合添加邮件地址
            if (mailArray != null)
            {
                for (int i = 0; i < mailArray.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(mailArray[i])) continue;
                    myMail.To.Add(mailArray[i].ToString());
                }
            }

            //向抄送收件人地址集合添加邮件地址
            if (ccArray != null)
            {
                for (int i = 0; i < ccArray.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(ccArray[i])) continue;
                    myMail.CC.Add(ccArray[i].ToString());
                }
            }

            myMail.From = maddr;  //发件人地址
            myMail.Subject = title;     //电子邮件的标题
            myMail.SubjectEncoding = Encoding.UTF8;    //电子邮件的主题内容使用的编码
            myMail.Body = content;//电子邮件正文
            myMail.BodyEncoding = System.Text.Encoding.UTF8;//电子邮件正文的编码
            myMail.Priority = MailPriority.High;
            myMail.IsBodyHtml = isHtml;//是否是html内容

            //在有附件的情况下添加附件
            try
            {
                if (attachmentsPathArray != null && attachmentsPathArray.Length > 0)
                {
                    Attachment attachFile = null;
                    foreach (string path in attachmentsPathArray)
                    {
                        if (string.IsNullOrWhiteSpace(path)) continue;

                        attachFile = new Attachment(path);
                        myMail.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("在添加附件时有错误:" + err);
            }

            SmtpClient smtp = new SmtpClient(this._serviceSMTP, this._servicePort);
            smtp.Credentials = new System.Net.NetworkCredential(this._serviceEmail, this._servicePassword);  //指定发件人的邮件地址和密码以验证发件人身份
            smtp.EnableSsl = this._serviceEnableSsl;
            //smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                //将邮件发送到SMTP邮件服务器
                smtp.Send(myMail);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送邮件
        /// System.Net.Mail 命名空间下的 SmtpClient 默认是不支持 SSL & 465 端口发送邮件
        /// 使用.NET框架废弃的，在 System.Web.Mail 命名空间下的 MailMessage 类代替
        /// </summary>
        /// <param name="mailArray">接受邮件的邮箱数组，例如：new string[] { "******@qq.com","12345678@qq.com"};</param>
        /// <param name="title">邮件的标题</param>
        /// <param name="content">邮件的内容</param>
        /// <param name="isHtml">发送的内容是否是html</param>
        /// <param name="ccArray">抄送的邮箱数组，例如：new string[] { "******@qq.com","12345678@qq.com"};</param>
        /// <param name="attachmentsPathArray">附件文件的路径集合</param>
        /// <returns></returns>
        public bool WebMailSend(string[] mailArray, string title, string content, bool isHtml = true, string[] ccArray = null, string[] attachmentsPathArray = null)
        {
            System.Web.Mail.MailMessage mm = new System.Web.Mail.MailMessage();
            // MailMessage mm = new MailMessage(); //实例化一个邮件类
            mm.Priority = System.Web.Mail.MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            mm.From = this._serviceEmail;

            //向收件人地址集合添加邮件地址
            if (mailArray != null)
            {
                mm.To = string.Join(";", mailArray);
            }

            //向抄送收件人地址集合添加邮件地址
            if (ccArray != null)
            {
                mm.Cc = string.Join(";", ccArray);
            }

            mm.Subject = title;     //邮件标题   
            mm.BodyFormat = System.Web.Mail.MailFormat.Html;
            mm.BodyEncoding = Encoding.UTF8;    //邮件正文的编码， 设置不正确， 接收者会收到乱码
            mm.Body = content;                            //邮件正文

            //在有附件的情况下添加附件
            try
            {
                if (attachmentsPathArray != null && attachmentsPathArray.Length > 0)
                {
                    Attachment attachFile = null;
                    foreach (string path in attachmentsPathArray)
                    {
                        if (string.IsNullOrWhiteSpace(path)) continue;

                        attachFile = new Attachment(path);
                        mm.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("在添加附件时有错误:" + err);
            }

            mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //身份验证
            mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mm.From); //邮箱登录账号，这里跟前面的发送账号一样就行
            mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", this._servicePassword); //这个密码要注意：如果是一般账号，要用授权码；企业账号用登录密码
            mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", this._servicePort);//端口
            mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", _serviceEnableSsl ? "true" : "false");//SSL加密


            System.Web.Mail.SmtpMail.SmtpServer = this._serviceSMTP;
            try
            {
                System.Web.Mail.SmtpMail.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                Log4Helper.Error($"邮件发送失败，错误原因：{ex}");
                //throw new Exception("邮件发送失败错误原因:" + err);
                return false;
            }

        }
    }
}
