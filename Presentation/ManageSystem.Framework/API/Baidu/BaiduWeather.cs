using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace ManageSystem.Framework.API.Baidu
{
    public class BaiduWeather
    {

        /// <summary>
        /// 百度开放平台的key，必须
        /// </summary>
        public string BaiduWeatherKey = "";

        /// <summary>
        /// 百度天气接口的请求地址，必须
        /// </summary>
        public string BiaduWeatherUrl = "";

        public BaiduWeather(string appKey, string apiUrl)
        {
            this.BaiduWeatherKey = appKey;
            this.BiaduWeatherUrl = apiUrl;
        }
        public BaiduWeather(string appKey)
        {
            this.BaiduWeatherKey = appKey;
            
            this.BiaduWeatherUrl = "http://api.map.baidu.com/telematics/v3/weather?output=json&ak={0}&location={1}";
        }

        /// <summary>
        /// 获取天气完整数据
        /// </summary>
        /// <param name="cityNameOrCode">城市名称或者城市编码，具体详见百度接口</param>
        /// <returns></returns>
        public Weather GetWeather(string cityNameOrCode)
        {
            if (string.IsNullOrEmpty(this.BaiduWeatherKey))
                throw new Exception("appKey 不能为空");

            if (string.IsNullOrEmpty(this.BiaduWeatherUrl))
                throw new Exception("Api url 地址不能为空");

            try
            {
                string url = string.Format(this.BiaduWeatherUrl, this.BaiduWeatherKey, cityNameOrCode);

                var resultJson = BaiduWeather.HttpGet(url);
                string value = JsonConvert.DeserializeObject(resultJson).ToString();
                Weather model = JsonConvert.DeserializeObject<Weather>(value);
                for (int i = 0; i < model.results[0].weather_data.Length; i++)
                {
                    if (i <= 0) continue;
                    model.results[0].weather_data[i].date += " " + DateTime.Now.AddDays(i).ToString("MM月dd日");
                }

                return model;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// 获取天气数据，包括时间、温度、风向等，共4天天气（包括今天）
        /// </summary>
        /// <param name="cityNameOrCode">城市名称或者城市编码，具体详见百度接口</param>
        /// <returns></returns>
        public  List<Weather_Data> GetWeatherData(string cityNameOrCode)
        {
            try
            {
                string key = "BaiduWeatherCacheKey";
                List<Weather_Data> model = HttpRuntime.Cache[key] as List<Weather_Data>;
                if (model == null)
                {
                    model = this.GetWeather(cityNameOrCode).results[0].weather_data.ToList();
                    HttpRuntime.Cache.Insert(key, model,null,DateTime.UtcNow.AddMinutes(30), Cache.NoSlidingExpiration);
                }

                return model;
            }
            catch (Exception)
            {

            }

            return null;
        }

        /// <summary>
        /// 获取的指定天的天气数据，包括时间、温度、风向等。不输入表示今天
        /// </summary>
        /// <param name="dayIndex">日期索引，0表示今天，1表示明天，2表示后天，3表示大后天</param>
        /// <returns></returns>
        public  Weather_Data GetWeatherByDay(string cityNameOrCode, int dayIndex = 0)
        {
            try
            {
                dayIndex = (dayIndex < 0 || dayIndex > 3) ? 0 : dayIndex; ;
                return this.GetWeatherData(cityNameOrCode)[dayIndex];
            }
            catch (Exception)
            {

            }

            return null;
        }

        /// <summary>
        /// 获取当日天气的索引提示内容，比如：穿衣、洗车、旅游等等
        /// </summary>
        /// <param name="cityNameOrCode">城市名称或者城市编码，具体详见百度接口</param>
        /// <returns></returns>
        public  List<Index> GetWeatherIndex(string cityNameOrCode)
        {
            try
            {
                Weather model = this.GetWeather(cityNameOrCode);
                return model.results[0].index.ToList();
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
        public static string HttpGet(string url)
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

    #region
    public class Index
    {
        public string title { get; set; }
        public string zs { get; set; }
        public string tipt { get; set; }
        public string des { get; set; }
    }

    public class Result
    {
        public string currentCity { get; set; }
        public string pm25 { get; set; }
        public Index[] index { get; set; }
        public Weather_Data[] weather_data { get; set; }
    }


    public class Weather
    {
        public int error { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public Result[] results { get; set; }

    }

    public class Weather_Data
    {
        public string date { get; set; }
        public string dayPictureUrl { get; set; }
        public string nightPictureUrl { get; set; }
        public string weather { get; set; }
        public string wind { get; set; }
        public string temperature { get; set; }
    }

    #endregion
}
