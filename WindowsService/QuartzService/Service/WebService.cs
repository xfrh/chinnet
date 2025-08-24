using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using QuartzService.Model;
using System.Xml;

namespace QuartzService.Service
{
    /// <summary>
    /// 服务类
    /// </summary>
    public class WebService
    {
        /// <summary>
        /// 存取需要检查的网站列表
        /// </summary>
        public static List<WebItemModel> WebList = new List<WebItemModel>();

        /// <summary>
        /// 存取需要接受短信的用户列表
        /// </summary>
        public static List<UserItemModel> UserList = new List<UserItemModel>();

        /// <summary>
        /// 读取配置文件来获取网站的列表
        /// </summary>
        public static void GetWebListByConfig()
        {
            try
            {
                string configFilePath = AppDomain.CurrentDomain.BaseDirectory + "web_list.xml";
                if (!File.Exists(configFilePath)) throw new FileNotFoundException("web_list.xml 文件不存在");

                XmlDocument docment = new XmlDocument();
                try
                {
                    docment.Load(configFilePath);
                }
                catch (Exception)
                {

                    throw new Exception($"docment.Load Exception: {configFilePath}");
                }

                // 得到根节点的所有子节点
                XmlNodeList xnl = docment.SelectNodes("webList/webItem");
                foreach (XmlElement item in xnl)
                {
                    XmlElement xe = item;
                    XmlNodeList xnl0 = xe.ChildNodes;

                    WebItemModel model = new WebItemModel();
                    model.Name = xnl0.Item(0).InnerText;
                    model.Url = xnl0.Item(1).InnerText;
                    model.IP = xnl0.Item(2).InnerText;
                    model.Status = xnl0.Item(3).InnerText.ToLower().Equals("true");
                    model.Remark = xnl0.Item(4).InnerText;
                    model.Target = Convert.ToInt32(xnl0.Item(5).InnerText);

                    WebList.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 读取配置文件来获取接受短信的用户列表
        /// </summary>
        public static void GetUserbListByConfig()
        {
            try
            {
                string configFilePath = AppDomain.CurrentDomain.BaseDirectory + "user_list.xml";
                if (!File.Exists(configFilePath)) throw new FileNotFoundException("user_list.xml 文件不存在");

                XmlDocument docment = new XmlDocument();
                docment.Load(configFilePath);

                // 得到根节点 webList
                XmlNode xn = docment.SelectSingleNode("userList");
                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;

                foreach (var item in xnl)
                {
                    XmlElement xe = (XmlElement)item;
                    XmlNodeList xnl0 = xe.ChildNodes;

                    UserItemModel model = new UserItemModel();
                    model.Name = xnl0.Item(0).InnerText;
                    model.Phone = xnl0.Item(1).InnerText;
                    model.Status = xnl0.Item(2).InnerText.ToLower().Equals("true");
                    model.Remark = xnl0.Item(3).InnerText;

                    UserList.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
