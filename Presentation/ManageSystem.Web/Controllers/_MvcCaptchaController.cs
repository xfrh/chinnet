using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.MvcCaptcha;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 图片验证码
    /// </summary>
    public class _MvcCaptchaController : WebBaseController
    {
        [CheckRole(false)]
        public ActionResult MvcCaptchaImage()
        {
            return new MvcCaptchaImageResult();
        }

        [CheckRole(false)]
        public ActionResult MvcCaptchaLoader()
        {
            string prevGuid = Request.ServerVariables["Query_String"];
            if (!string.IsNullOrEmpty(prevGuid))
                HttpContext.Session.Remove(prevGuid);
            var options = new MvcCaptchaOptions();
            var config = MvcCaptchaConfigSection.GetConfig();
            if (config != null)
            {
                options.TextChars = config.TextChars;
                options.TextLength = config.TextLength;
                options.FontWarp = config.FontWarp;
                options.BackgroundNoise = config.BackgroundNoise;
                options.LineNoise = config.LineNoise;
            }

            var image = new MvcCaptchaImage(options);
            HttpContext.Session.Add(image.UniqueId, image);
            HttpContext.Response.Cache.SetNoStore();
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return Content(image.UniqueId);
        }
    }
}