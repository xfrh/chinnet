using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using ManageSystem.Framework.Controllers;
using ManageSystem.Services.Configuration;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Services.Members;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Web.Controllers;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 支付
    /// </summary>
    public class PayController : WebBaseController
    {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="sn">订单号</param>
        /// <returns></returns>
        public ActionResult PayIndex(string sn)
        {
            return View();
        }


    }
}