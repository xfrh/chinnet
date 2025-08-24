using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Utility;
using ManageSystem.Services.CRE;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Areas.CRE.Controllers
{
    public class CreUploadDataController :Controller
    {
        /// <summary>
        /// 用户业务层
        /// </summary>
        private readonly ICreUploatDataService  creUploatDataService;
        /// <summary>
        /// 加解密服务
        /// </summary>
        private readonly IEncryptionService EncryptionService;
        /// <summary>
        /// 用户业务层
        /// </summary>
        private readonly IMemberService MemberService;

        /// <summary>
        /// 医院业务层
        /// </summary>
        private readonly IHospitalService HospitalService;

        private readonly HttpContextBase httpContext;

        public CreUploadDataController(ICreUploatDataService UploatDataService, IEncryptionService _encryptionService, IMemberService _memberService, HttpContextBase _httpContext, IHospitalService _hospitalService) { creUploatDataService = UploatDataService; EncryptionService = _encryptionService; MemberService = _memberService; httpContext = _httpContext; HospitalService = _hospitalService; }
        // GET: CRE/CreUploadData
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dataupload()
        {
            
            ViewBag.Essential = creUploatDataService.Get_Germ_Type();
            long? crem_id = null;
            if (Request.QueryString["creid"] == null)
            {
                crem_id = null;
            }
            else
            {
                crem_id = long.Parse(Request.QueryString["creid"]);
            }
            ViewBag.Essentials = creUploatDataService.Get_Germ_Types(crem_id);
            ViewBag.Germ_id = creUploatDataService.Get_Typeshou().Germ_id;
            return View();
        }

        public string Hospital(string token)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                //if (member_id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                member = MemberService.QueryEntity(member_id);
                //if (member == null || member.Id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                //if (member.Mark != 1 && member.Mark != 2)
                //{
                //    return Json(new { status = false, message = "用户信息状态异常" });
                //}
            }
            catch (Exception)
            {
                throw;
                //return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion  
            var model = HospitalService.QueryEntity(member.HospitalId);
            return model.Name;
        }

        /// <summary>
        /// 查询地区
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public string GetArea(long parentId)
        {
            //EchartscofingController.GetEcharts(null,null);
            string json = JsonConvert.SerializeObject(creUploatDataService.GetareList(parentId));
            return json;
        }

        /// <summary>
        /// 查询基础信息
        /// </summary>
        /// <returns></returns>
        public string Basicinformation(string token)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                //if (member_id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                member = MemberService.QueryEntity(member_id);
                //if (member == null || member.Id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                //if (member.Mark != 1 && member.Mark != 2)
                //{
                //    return Json(new { status = false, message = "用户信息状态异常" });
                //}
            }
            catch (Exception)
            {
                throw;
                //return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion
            string json = JsonConvert.SerializeObject(creUploatDataService.Basicinformation(member.Id));
            return json;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="toconfigure"></param>
        /// <returns></returns>
        public int AddCre_data(Toconfigure toconfigure,string token)
        {

            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                //if (member_id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                member = MemberService.QueryEntity(member_id);
                //if (member == null || member.Id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                //if (member.Mark != 1 && member.Mark != 2)
                //{
                //    return Json(new { status = false, message = "用户信息状态异常" });
                //}
            }
            catch (Exception)
            {
                throw;
                //return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion
            toconfigure.Created_by = member.Id;
            toconfigure.Created = DateTime.Now;
            toconfigure.Hospital_id = member.HospitalId;
            toconfigure.Modified = null;
            toconfigure.Modified_by = null;
            toconfigure.Allow_report_display = 1;
            toconfigure.Isvalid = 1;
            toconfigure.Isaudited = 1;
            toconfigure.Audited_by = 999;
            toconfigure.Audited_time = DateTime.Now;
            toconfigure.Province = null;
            toconfigure.City = null;
            toconfigure.District = null;
            if (toconfigure.Germ_id == null)
            {
                toconfigure.Germ_id = creUploatDataService.Get_Typeshou().Germ_id;
            }
            Cre_data_period data_Period = creUploatDataService.GetPeriod(toconfigure.Germ_id,toconfigure.Data_year, toconfigure.Data_season,toconfigure.Created_by);
            int result = 0;
            if (data_Period == null)
            {
                result = creUploatDataService.AddCre_Data(toconfigure);
                AddCrehetmap("添加数据", "添加数据", member.Name);
            }                    
            else
            {
                toconfigure.Modified = DateTime.Now; ;
                toconfigure.Modified_by = member.Id;
                toconfigure.Cre_id = data_Period.Cre_id;
                result = creUploatDataService.UpdateCre_data(toconfigure);
                AddCrehetmap("修改数据", "修改数据", member.Name);
            }
            return result;
        }

        /// <summary>
        /// 添加cre上传日志日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public void AddCrehetmap(string type, string content, string name)
        {
            CreProject_log cRE_Heatmp = new CreProject_log();
            cRE_Heatmp.BrowserName = httpContext.Request.Browser.Browser;
            cRE_Heatmp.UserinfoId = HttpHelper.GetIp();
            cRE_Heatmp.UserinfoName = name;
            cRE_Heatmp.Content = content;
            cRE_Heatmp.Detail = content;
            cRE_Heatmp.Type = type;
            cRE_Heatmp.InsertTime = DateTime.Now;
            cRE_Heatmp.UpdateTime = DateTime.Now;
            cRE_Heatmp.DeleteTime = null;
            creUploatDataService.AddCreProject_log(cRE_Heatmp);
        }

        /// <summary>
        /// 数据返填
        /// </summary>
        /// <param name="year"></param>
        /// <param name="season"></param>
        /// <param name="Grem_id"></param>
        /// <returns></returns>
        public string Databackfilling(string year, string season, long? Grem_id,string token)
        {
            #region 用户信息
            string tokenText = EncryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                //if (member_id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                member = MemberService.QueryEntity(member_id);
                //if (member == null || member.Id <= 0)
                //{
                //    return Json(new { status = false, message = "未能获取用户信息" });
                //}
                //if (member.Mark != 1 && member.Mark != 2)
                //{
                //    return Json(new { status = false, message = "用户信息状态异常" });
                //}
            }
            catch (Exception)
            {
                throw;
                //return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion
            string Databackfilling = JsonConvert.SerializeObject(creUploatDataService.Getdata_show(year, season, Grem_id, member.Id));
            return Databackfilling;
        }
    }
}