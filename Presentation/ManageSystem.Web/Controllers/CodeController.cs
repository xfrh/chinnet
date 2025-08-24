using ManageSystem.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{

    public class CodeController : Controller
    {

        /// <summary>
        /// 控制一切扫二维码进入的数据
        /// </summary>
        /// <param name="type">区分不同的数据类型那个</param>
        /// <param name="data">数据</param>
        /// <param name="dataId">传入的自定义数据的id</param>
        /// <returns></returns>
        public ActionResult Index(string type, string dataId, string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(type))    throw new Exception("type 不能为空");
                if (string.IsNullOrWhiteSpace(dataId)) throw new Exception("数据不能为空");

                switch (type.ToLower())
                {
                    case "questionnaire":
                        //问卷调查的二维码，扫描后跳转到填写问卷的页面
                        return this.RedirectToAction("Item", "Survey", new { id = dataId });
                    case "medical":
                        //医学信息的二维码，扫描后跳转到查看医学信息的页面
                        return this.RedirectToAction("Detail", "Medicine", new { id = dataId });
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log4Helper.Error("调用 web 二维码接口数据错误，接口地址：ManageSystem.Web.Controllers.CodeController ，数据type=" + type + " data=" + data + "   dataId=" + dataId + "  " + ex.ToString());
                return this.RedirectToAction("PageNotFound", "Common");
            }

            return View();
        }






    }
}