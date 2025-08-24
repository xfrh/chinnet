using ManageSystem.Core.Domain.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class WebBaseCRController : WebBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Member member = LoginUserinfo;
            if (member == null || member.Id <= 0 || !member.LoginId.StartsWith("CRFW"))
            {
                base.TransferError("只有CR用户才能使用此功能");
                filterContext.RequestContext.HttpContext.Response.End();
                filterContext.Result = new EmptyResult();
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}