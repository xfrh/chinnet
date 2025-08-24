using ManageSystem.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class SharedController : WebBaseController
    {


        /// <summary>
        /// 用户登录信息
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [CheckRole(false)]
        public PartialViewResult _LoginInfo()
        {
            var member = base.LoginUserinfo;

            return this.PartialView(member);
        }


        
    }
}