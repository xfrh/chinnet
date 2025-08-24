using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.Model
{
    /// <summary>
    /// 短信API 发送以后返回的对象
    /// https://www.juhe.cn/docs/api/id/54
    /// </summary>
    public class SmsApiResultModel
    {
        /// <summary>
        /// 短信发送成功
        /// </summary>
        public string reason { get; set; }

        /// <summary>
        ///  0 发送成功
        /// </summary>
        public int error_code { get; set; }

        /// <summary>
        /// 结果内容
        /// </summary>
        public Result result { get; set; }
    }

    public class Result
    {
        /// <summary>
        /// 发送数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 扣除条数
        /// </summary>
        public int fee { get; set; }

        /// <summary>
        /// 短信ID
        /// </summary>
        public string sid { get; set; }
    }
}
