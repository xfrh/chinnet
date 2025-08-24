using ManageSystem.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class MicThirdController : WebBaseController
    {
        public MicThirdController()
        { }

        [CheckRole(false)]
        public ActionResult MicSignUpView()
        {

            return View();
        }
    }
}