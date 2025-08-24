using ManageSystem.Web.App_Start;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 错误相关的控制器
    /// </summary>
    public class ErrorController : Controller
    {

        /// <summary>
        /// 页面错误提示，转发，页面不跳转
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult PageError(string value = "")
        {
            this.ViewBag.ErrorMessage = value;
            return this.View();
        }

        /// <summary>
        /// 页面错误提示，转发，页面不跳转
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Transfer(string value = "")
        {
            this.ViewBag.ErrorMessage = value;

            return this.View();
        }

        /// <summary>
        /// 通用错误页面
        /// </summary>
        /// <param name="value"></param>
        /// <param name="returnUrl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Message(string value = "", string returnUrl = "", string type = "")
        {
            this.ViewBag.ErrorMessage = value;
            return this.View();
        }

        /// <summary>
        /// 404错误页面
        /// </summary>
        /// <param name="value"></param>
        /// <param name="returnUrl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Error404()
        {
            return this.View();
        }

    }
}