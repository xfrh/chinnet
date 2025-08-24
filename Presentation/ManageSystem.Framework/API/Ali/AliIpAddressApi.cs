using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Framework.API.Ali
{
    /// <summary>
    /// 
    /// 功能：调用淘宝的ip地址库获取指定IP的相关信息，比如城市、省份等等
    /// 作者：邹学典
    /// 时间：2015-12-30
    /// 版本：1.0
    /// 备注：
    /// 
    /// </summary>
    public class AliIpAddressApi
    {

        /// <summary>
        /// 通过指定ip地址获取相关ip信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IpAddressModel GetIpAddress(string ip)
        {
            try
            {
                string url = "http://ip.taobao.com/service/getIpInfo.php?ip="+ip;

                var resultJson = AliIpAddressApi.HttpGet(url);
                string value = JsonConvert.DeserializeObject(resultJson).ToString();
                IpAddressModel model = JsonConvert.DeserializeObject<IpAddressModel>(value);

                return model;
            }
            catch (Exception)
            {

            }

            return null;
        }


        /// <summary>
        /// 后台发送GET请求
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <returns></returns>
        private static string HttpGet(string url)
        {
            try
            {
                //创建Get请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                //接受返回来的数据
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                string retString = streamReader.ReadToEnd();

                streamReader.Close();
                stream.Close();
                response.Close();

                return retString;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }

    public class IpAddressModel
    {
        public int code { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string ip { get; set; }
        public string country { get; set; }
        public string area { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string isp { get; set; }
        public string country_id { get; set; }
        public string area_id { get; set; }
        public string region_id { get; set; }
        public string city_id { get; set; }
        public string county_id { get; set; }
        public string isp_id { get; set; }
    }

}
