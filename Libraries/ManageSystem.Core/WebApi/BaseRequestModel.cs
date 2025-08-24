using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.WebApi
{
    /// <summary>
    /// Web api 请求数据的基类
    /// </summary>
    public class BaseRequestModel
    {
        /// <summary>
        /// 接口请求数据来源，比如：mobile、ios、android、web 等等
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 用户身份标记，登录用户的标记
        /// </summary>
        public string Token { get; set; }

    }
}
