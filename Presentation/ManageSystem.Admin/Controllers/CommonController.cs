
using ManageSystem.Admin.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class CommonController : Controller
    {
     
        public CommonController( )
        {
          
        }


        /// <summary>
        ///  404 错误页面
        /// </summary>
        /// <returns></returns>
        [CheckRole(false,false)]
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }

        /// <summary>
        ///  禁止访问的页面
        /// </summary>
        /// <returns></returns>
        [CheckRole(false, false)]
        public ActionResult AccessDenied()
        {
            return View();
        }


        /// <summary>
        ///  错误页面
        /// </summary>
        /// <returns></returns>
        [CheckIP(false)]
        [CheckRole(false,false)]
        public ActionResult Error(string message)
        {
            message = string.IsNullOrWhiteSpace(message) ? "" : message;

            this.ViewBag.Message = message;

            return View();
        }
        


    }
}