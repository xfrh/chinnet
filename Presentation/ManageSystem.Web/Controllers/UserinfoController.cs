using ManageSystem.Core.Domain.Users;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class UserinfoController : Controller
    {
        private readonly IUserinfoService userinfoService;
        private readonly IAuthenticationService authenticationService;

        public UserinfoController(IUserinfoService _userinfoService, IAuthenticationService _authenticationService)
        {
            this.userinfoService = _userinfoService;
            this.authenticationService = _authenticationService;
        }

   


    // GET: Customer
    public ActionResult List()
    {
        //Customer entity = this._authenticationService.GetAuthenticatedCustomer();
        //CustomerModel model = entity.ToModel();


            //房源审核通过了，解冻对应的收益

            /*
             
            情况一：
             
             
             */


        return View();
    }



    }
}