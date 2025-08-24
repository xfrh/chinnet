using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.DynamicLinq;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Medicine;
using ManageSystem.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class TestController : Controller
    {
        IMedicalDataItemService _medicalDataItemService;
        public TestController(IMedicalDataItemService medicalDataItemService)
        {
            _medicalDataItemService = medicalDataItemService;
        }
        /// <summary>
        /// Email 模版
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Mailbox()
        {
            return View();
        }
    }
}