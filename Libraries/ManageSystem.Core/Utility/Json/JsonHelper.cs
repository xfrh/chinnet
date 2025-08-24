using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace ManageSystem.Core.Utility
{
    public class JsonHelper
    {
        
        /// <summary>
        /// 获取基础的json通知数据， 包含2个属性，status , message
        /// </summary>
        /// <param name="status">结果状态</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public static string GetBaseMessage(bool status = false, string message = "")
        {
            Dictionary<string, string> r = new Dictionary<string, string>();
            r.Add("status", status ? "true" : "false");
            r.Add("message", message);

            return JsonConvert.SerializeObject(r);
        }

        public static T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

    }
}
