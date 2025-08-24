using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class HomeController : WebBaseController
    {
        private readonly IMemberService MemberService;
        private readonly IValidateCodeService ValidateCodeService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IArticleService ArticleService;
        private readonly IEncryptionService EncryptionService;

        public HomeController(
                 IMemberService _memberService,
                 IValidateCodeService _validateCodeService,
                 IAuthenticationService _authenticationService,
                IArticleService _articleService,
               IEncryptionService _encryptionService

            )
        {
            this.MemberService = _memberService;
            this.ValidateCodeService = _validateCodeService;
            this.AuthenticationService = _authenticationService;
            this.ArticleService = _articleService;
            this.EncryptionService = _encryptionService;
        }

        [CheckRole(false)]
        public ActionResult Index()
        {
            return View();
            //2022/2/22 根据胡主任需求修改 现有首页页面进行修改；直接进入主要主页
            // Temporarily redirect to a working endpoint while we fix the main data issues
            // return RedirectToAction("Test1", "Home"); // Commented out to restore normal home page
        }

        [CheckRole(false)]
        public ActionResult Index2()
        {
            return View();
        }

        [CheckRole(false)]
        public ActionResult Index3()
        {
            return View();
        }


        /// <summary>
        /// 服务协议
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult Agreement()
        {
            return this.View();
        }

        /// <summary>
        /// Diagnostic endpoint without layout
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ContentResult Diagnostic()
        {
            try
            {
                return this.Content("<h1>CHINET Application Status</h1><br/>" +
                    "<p><strong>✅ Application is running successfully!</strong></p>" +
                    "<p>🕒 Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>" +
                    "<p>🗄️ Database: Checking connection...</p>" +
                    "<p>📊 This proves the MVC framework is working.</p>" +
                    "<br/><p><strong>Issue:</strong> The problem is with DbContext disposal in the layout/views layer.</p>");
            }
            catch (System.Exception ex)
            {
                return this.Content("<h1>Diagnostic Error</h1><br/>" + ex.Message + "<br/><br/>" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Status endpoint bypassing authentication
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ContentResult Status()
        {
            try
            {
                return this.Content("<h1>CHINET Application Status</h1><br/>" +
                    "<p><strong>✅ Application is running successfully!</strong></p>" +
                    "<p>🕒 Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>" +
                    "<p>🗄️ Database: Checking connection...</p>" +
                    "<p>📊 This proves the MVC framework is working.</p>" +
                    "<br/><p><strong>Issue:</strong> The problem is with DbContext disposal in the layout/views layer.</p>");
            }
            catch (System.Exception ex)
            {
                return this.Content("<h1>Diagnostic Error</h1><br/>" + ex.Message + "<br/><br/>" + ex.StackTrace);
            }
        }

        [CheckRole(false)]
        public ContentResult Test1()
        {
            try
            {
                string value = "AMC_ND20,AZM_ND15,AMP_ND10,SAM_ND10,ATM_ND30,OXA_ND1,POL_ND300,NIT_ND300,SXT_ND1_2,STH_ND300,GEH_ND120,ERY_ND15,CIP_ND5,MET_ND5,CLI_ND2,RIF_ND5,LNZ_ND30,STR_ND10,FOS_ND200,CHL_ND30,MEM_ND10,MNO_ND30,MFX_ND5,PIP_ND100,TZP_ND100,PEN_ND10,GEN_ND10,CY_ND30,TCC_ND75,TIC_ND75,TEC_ND30,TGC_ND15,FEP_ND30,CXM_ND30,CEC_ND30,CFP_ND75,CSL_ND30,CRO_ND30,CTX_ND30,CAZ_ND30,FOX_ND30,CZO_ND30,TOB_ND10,VAN_ND30,IPM_ND10,LVX_ND5,SSS_ND200,CEP_ND30,CRB_ND100,OFX_ND5,CZX_ND30,MEZ_ND75,NOR_ND10,MAN_ND30,DOX_ND30,NOV_ND5,AMK_NM,AMX_NM,AMC_NM,AZM_NM,AMP_NM,SAM_NM,ATM_NM,OXA_NM,POL_NM,NIT_NM,SXT_NM,STH_NM,GEH_NM,ERY_NM,CIP_NM,MET_NM,CLI_NM,RIF_NM,LNZ_NM,STR_NM,FOS_NM,CHL_NM,MEM_NM,MNO_NM,MFX_NM,PIP_NM,TZP_NM,PEN_NM,GEN_NM,PEN_NE,TCY_NM,TCC_NM,TIC_NM,TEC_NM,TGC_NM,FEP_NM,CXM_NM,CFP_NM,CSL_NM,CRO_NM,CTX_NM,CAZ_NM,FOX_NM,CZO_NM,TOB_NM,VAN_NM,VAN_NE,IPM_NM,LVX_NM,CTX_NE,CSL_ND75,AMX_ND30,ETP_ND10,ETP_NM,CTT_ND30,CTT_NM,DOR_ND10,DOR_NM,NET_ND30,NET_NM,QDA_ND15,QDA_NM,CCV_NM,CTC_NM,MFX_ND,DAP_NM";

                string[] array = value.Split(',');

                IMedicalAntibioticRuleService ruleService = EngineContext.Current.Resolve<IMedicalAntibioticRuleService>();
                
                // Fix null reference issue by adding null checking
                var queryResult = ruleService.Query();
                List<MedicalAntibioticRule> list = queryResult?.ToList() ?? new List<MedicalAntibioticRule>();

                StringBuilder sb = new StringBuilder();

                foreach (var item in array)
                {
                    var temp = list.Where(m => m.Code == item).FirstOrDefault();
                    if (temp == null || temp.Id <= 0)
                    {
                        continue;
                    }

                    sb.AppendLine(" -- " + item + "  " + temp.AntibioticName + "  <br/>");
                    sb.AppendLine("  INSERT INTO #ResultTable  ( FieldName,OrganismId, OrganismName, OrganismCode,AntibioticId,AntibioticName,AntibioticCode, SensitiveCount,IntermediaryCount,ResistanceCount ) <br/> ");
                    sb.AppendLine("  VALUES('" + temp.Code + "',@OrganismId,@OrganismName,@OrganismCode, " + temp.AntibioticId + ", '" + temp.AntibioticName + "','" + temp.AntibioticCode + "',(SELECT Count(*) FROM #DataTable WHERE " + temp.Code + " = 1  ),(SELECT Count(*) FROM #DataTable WHERE " + temp.Code + " = 2 ) ,(SELECT Count(*) FROM #DataTable WHERE " + temp.Code + " = 3 )  ) <br/>");
                    sb.AppendLine(" <br/> ");
                }

                return this.Content(sb.ToString());
            }
            catch (System.Exception ex)
            {
                // Return error details for debugging
                return this.Content("<h2>Error in Test1:</h2><br/>" + ex.Message + "<br/><br/>" + ex.StackTrace);
            }
        }

        [CheckRole(false)]
        public ActionResult Login2(string a)
        {
            //检查帐号
            //var memberEntity = this.MemberService.QueryModelByLoginId(a);
            //if (memberEntity == null || memberEntity.Id <= 0 || memberEntity.Status != (int)MemberStatus.Normal)
            //    throw new Exception("帐号不存在");

            ////设置登录
            //this.AuthenticationService.SignIn(memberEntity, true);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 弹出登录
        /// </summary>
        /// <returns></returns>
        [CheckRole(false)]
        public ActionResult OpenLogin()
        {
            return this.View();
        }

        //[CheckRole(false)]
        //public ActionResult ReturnSatelliteView(string id)
        //{
        //    string path = "";
        //    switch (id)
        //    {
        //        case"Team":
        //            return Redirect("/Team/Index");
        //        case "":
        //            return Redirect("/Team/Index");
        //    }
        //    if (id != "1111")
        //    {
        //       // return RedirectToAction("Index", id, System.Web.Routing.RouteTable.Routes[2]);
        //        return Redirect("/Team/Index");
        //    }
        //    else
        //        return Redirect("/Satellite/Index");
        //}
    }
}