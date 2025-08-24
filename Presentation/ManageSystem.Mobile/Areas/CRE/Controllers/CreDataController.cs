using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Services.CRE;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Areas.CRE.Controllers
{
    public class CreDataController : MobileBaseController
    {
        private readonly ICreDataService creDataService;

        /// <summary>
        /// 加解密服务
        /// </summary>
        private readonly IEncryptionService EncryptionService;

        /// <summary>
        /// 用户业务层
        /// </summary>
        private readonly IMemberService MemberService;
        public CreDataController(ICreDataService _creDataService,IEncryptionService _encryptionService, IMemberService _memberService)
        {
            creDataService = _creDataService;
            EncryptionService = _encryptionService;
            MemberService = _memberService;
        }
        // GET: CRE/CreData
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult HeatMap()
        {
            ViewBag.tite = creDataService.Get_Type().ToList();
            ViewBag.year = creDataService.Get_Year().ToList();
            ViewBag.datayear = creDataService.Get_Data_Periods();
            if (creDataService.Get_Typeshou() == null)
            {
                ViewBag.Germ_id = null;
            }
            else
            {
                ViewBag.Germ_id = creDataService.Get_Typeshou().Germ_id;
            }
            return View();
        }
        /// <summary>
        /// 热图一级数据显示
        /// </summary>
        /// <param name="year"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public string SHeatMapDataShow(string year, string Germ_id)
        {
            if (string.IsNullOrWhiteSpace(year) && string.IsNullOrWhiteSpace(Germ_id))
            {
                year = creDataService.Get_Data_Periods();
                Germ_id = creDataService.Get_Typeshou().Germ_id.ToString();
            }
            //EchartscofingController.GetEcharts(null,null);
            string json = JsonConvert.SerializeObject(creDataService.SGetdata_Show(year, Germ_id));
            return json;
        }
        /// <summary>
        /// 热图二级数据显示
        /// </summary>
        /// <param name="year"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public string CHeatMapDataShow(string year, string Germ_id)
        {
            if (string.IsNullOrWhiteSpace(year) && string.IsNullOrWhiteSpace(Germ_id))
            {
                year = creDataService.Get_Data_Periods();
                Germ_id = creDataService.Get_Typeshou().Germ_id.ToString();
            }
            //EchartscofingController.GetEcharts(null,null);
            string json = JsonConvert.SerializeObject(creDataService.CGetdata_Show(year, Germ_id));
            return json;
        }

        public string GetEcharts(string year, long? type)
        {
            if (year == null && type == null)
            {
                year= creDataService.Get_Data_Periods();
                type= creDataService.Get_Typeshou().Germ_id;
                Cre_Colormatching color = creDataService.GetColormatchings(year, type);
                if (color == null)
                {
                    return "";
                }
                else
                {
                    return color.Content_value;
                }

            }
            else
            {
                Cre_Colormatching color = creDataService.GetColormatchings(year, type);
                var data = JsonConvert.SerializeObject(color.Content_value);
                return data;
            }

        }
        public ViewResult GermWithBar()
        {
            List<Cre_germ_types> Essential = creDataService.Get_Germ_Type();
            ViewBag.Essential = Essential;
            return View();
        }

        /// <summary>
        /// 大肠埃希菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public string Escherichiacoli(string field, long? Germ_id)
        {
            string Escherichiacoli = JsonConvert.SerializeObject(creDataService.Escherichiacoli(field, Germ_id));
            return Escherichiacoli;
        }
        /// <summary>
        /// 克雷伯菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public string klebsiella(string field, long? Germ_id)
        {
            string klebsiella = JsonConvert.SerializeObject(creDataService.klebsiella(field, Germ_id));
            return klebsiella;
        }
        /// <summary>
        /// 肠杆菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public string Entero(string field, long? Germ_id)
        {
            string Entero = JsonConvert.SerializeObject(creDataService.Enterobacter(field, Germ_id));
            return Entero;
        }
        /// <summary>
        /// 沙雷菌
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public string serratia(string field, long? Germ_id)
        {
            string serratia = JsonConvert.SerializeObject(creDataService.serratia(field, Germ_id));
            return serratia;
        }

        /// <summary>
        /// 柱状图查询(菌株率)
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Strainrate(string field)
        {
            string Strainrate = JsonConvert.SerializeObject(creDataService.Strainrate(field));
            return Strainrate;
        }

        /// <summary>
        /// 抗药物分析
        /// </summary>
        /// <param name="field"></param>
        /// <param name="Germ_id"></param>
        /// <returns></returns>
        public string Drugrate(string field, long? Germ_id)
        {
            string Drugrate = JsonConvert.SerializeObject(creDataService.Drugrate(field, Germ_id));
            return Drugrate;
        }

        public string Getmember(string token)
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
            string Crefillonline = member.Crefillonline;
            return Crefillonline;
        }
    }
}