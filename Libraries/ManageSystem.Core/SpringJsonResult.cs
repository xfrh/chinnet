using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core
{
    /// <summary>
    /// 通用返回Json
    /// </summary>
    public class SpringJsonResult
    {
        /// <summary>
        /// 获取json返回值
        /// </summary>
        /// <param name="status">结果状态，true表示成功， false表示失败</param>
        /// <param name="message">信息内容</param>
        /// <param name="otherMessage">其他的信息内容</param>
        /// <param name="code">编码，一般是错误编码，返回失败的时候用</param>
        /// <returns></returns>
        public static string Get(bool status = false, string message = "", string otherMessage = "", int code = 0)
        {
            SpringJsonResultModel model = new SpringJsonResultModel()
            {
                Code = code,
                Message = message,
                OtherMessage = otherMessage,
                Status = status
            };

            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// 获取错误json返回值
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="otherMessage">其他的信息内容</param>
        /// <param name="code">编码，一般是错误编码，返回失败的时候用</param>
        /// <returns></returns>
        public static string Error(string message = "", string otherMessage = "", int code = 0)
        {
            SpringJsonResultModel model = new SpringJsonResultModel()
            {
                Code = code,
                Message = message,
                OtherMessage = otherMessage,
                Status = false
            };

            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// 获取成功json返回值
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <param name="otherMessage">其他的信息内容</param>
        /// <param name="data">data 数据</param>
        /// <param name="code">编码，一般是错误编码，返回失败的时候用</param>
        /// <returns></returns>
        public static string Success(string message = "", string otherMessage = "", object data = null, int code = 0)
        {
            SpringJsonResultModel model = new SpringJsonResultModel()
            {
                Code = code,
                Message = message,
                OtherMessage = otherMessage,
                Status = true,
                Data = data
            };

            return JsonConvert.SerializeObject(model);
        }
    }

    /// <summary>
    /// 通用返回Json数据实体类
    /// </summary>
    public class SpringJsonResultModel
    {
        /// <summary>
        /// 结果状态，true表示成功， false表示失败
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 编码，一般是错误编码，返回失败的时候用
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回其他的信息
        /// </summary>
        public string OtherMessage { get; set; }

        /// <summary>
        /// Data数据
        /// </summary>
        public object Data { get; set; }
    }

}