
using ManageSystem.Core;
using ManageSystem.Core.Domain.Chart;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.Chart;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Models.Datas;
using ManageSystem.Web.Models.Members;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// CHINET 首页 
    /// </summary>
    public class ChinetController : WebBaseController
    {
        #region 业务声明
        private readonly IMemberService MemberService;
        private readonly IValidateCodeService ValidateCodeService;
        private readonly IAuthenticationService AuthenticationService;
        private readonly IArticleService ArticleService;
        private readonly IEncryptionService EncryptionService;
        private readonly IDataSegmentService _dataSegmentService;
        private readonly IHeatmapService _heatmapService;
        private readonly IHeatmapItemService _heatmapItemService;
        private readonly ISatelliteService _satelliteService;

        private static readonly Dictionary<string, string> _provinceMap = new Dictionary<string, string>
        {
            ["北京"] = "北京市",
            ["天津"] = "天津市",
            ["河北"] = "河北省",
            ["山西"] = "山西省",
            ["内蒙古"] = "内蒙古自治区",
            ["辽宁"] = "辽宁省",
            ["吉林"] = "吉林省",
            ["黑龙江"] = "黑龙江省",
            ["上海"] = "上海市",
            ["江苏"] = "江苏省",
            ["浙江"] = "浙江省",
            ["安徽"] = "安徽省",
            ["福建"] = "福建省",
            ["江西"] = "江西省",
            ["山东"] = "山东省",
            ["河南"] = "河南省",
            ["湖北"] = "湖北省",
            ["湖南"] = "湖南省",
            ["广东"] = "广东省",
            ["广西"] = "广西壮族自治区",
            ["海南"] = "海南省",
            ["重庆"] = "重庆市",
            ["四川"] = "四川省",
            ["贵州"] = "贵州省",
            ["云南"] = "云南省",
            ["西藏"] = "西藏自治区",
            ["陕西"] = "陕西省",
            ["甘肃"] = "甘肃省",
            ["青海"] = "青海省",
            ["宁夏"] = "宁夏回族自治区",
            ["新疆"] = "新疆维吾尔自治区",
            ["台湾"] = "台湾省",
            ["香港"] = "香港特别行政区",
            ["澳门"] = "澳门特别行政区"
        };

        #endregion

        #region 构造器
        public ChinetController(
            IMemberService _memberService,
            IValidateCodeService _validateCodeService,
            IAuthenticationService _authenticationService,
            IArticleService _articleService,
            IEncryptionService _encryptionService,
            IDataSegmentService dataSegmentService,
            IHeatmapService heatmapService,
            IHeatmapItemService heatmapItemService,
              ISatelliteService satelliteService
        )
        {
            this.MemberService = _memberService;
            this.ValidateCodeService = _validateCodeService;
            this.AuthenticationService = _authenticationService;
            this.ArticleService = _articleService;
            this.EncryptionService = _encryptionService;
            this._dataSegmentService = dataSegmentService;
            this._heatmapService = heatmapService;
            this._heatmapItemService = heatmapItemService;
            this._satelliteService = satelliteService;
        }
        #endregion

        [CheckRole(false)]
        public ActionResult Index()
        {
            Expression<Func<Chart_DataSegment, bool>> predicate = r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.Heatmap;

            List<HeatmapDTO> heatmaps = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new HeatmapDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _heatmapService.Query(r => r.Display && r.Mark > 0 && r.DataSegmentId == item.Id).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).Select(r => new HeatmapItemDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Default = r.Default
                }).ToList()
            }).ToList();

            return View(heatmaps);
        }


        [CheckRole(false)]
        public ActionResult IndexMap(string city)
        {
            Expression<Func<Chart_DataSegment, bool>> predicate = r => r.Display && r.Mark > 0 && r.ProjectType == DataSegmentEnum.Heatmap;
            string loginId = this._satelliteService.Query().First(x => x.RealmName == city).Id.ToString();
            List<HeatmapDTO> heatmaps = _dataSegmentService.Query(predicate).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).ThenBy(r => r.Id).Select(item => new HeatmapDTO
            {
                Id = item.Id,
                Name = item.Name,
                DataItems = _heatmapService.Query(r => r.Display && r.Mark > 0 && r.DataSegmentId == item.Id&&r.SatelliteId== loginId).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).Select(r => new HeatmapItemDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Default = r.Default
                }).ToList()
            }).ToList();

            return View(heatmaps);
        }

        [HttpPost, CheckRole(false)]
        public ActionResult IndexData(long id)
        {

            var entity = _heatmapService.QueryEntity(id);
            var _ds = _dataSegmentService.QueryEntity(entity.DataSegmentId);
            var data = _heatmapItemService.Query(r => r.HeatmapId == id && r.Mark > 0).OrderBy(r => r.Sort).ThenBy(r => r.InsertTime).Select(item => new
            {
                name = _provinceMap.FirstOrDefault(r => r.Value == item.Name).Key,
                value = item.Value
            }).ToList();
            return Json(new
            {
                title = $"{_ds.Name} ({entity.Name})",
                data = data
            });
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="token"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult Cipher(string token, string returnUrl)
        {
            long id = 0;
            string tokenText = EncryptionService.DecryptText(token);
            try
            {
                id = Convert.ToInt64(tokenText);
                if (id == 0)
                {
                    return Redirect("/");
                }
            }
            catch (Exception)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Cipher(string token, string password, string confirmPassword, string returnUrl)
        {
            long id = 0;
            string tokenText = EncryptionService.DecryptText(token);
            try
            {
                id = Convert.ToInt64(tokenText);
                if (id == 0)
                {
                    return Redirect("/");
                }
            }
            catch (Exception)
            {
                return Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Trim().Length < 6) { throw new ManageSystemException("密码长度必须大于等于6位"); }
            password = EncryptionService.EncryptText(password.Trim());

            if (string.IsNullOrWhiteSpace(confirmPassword) || confirmPassword.Trim().Length < 6) { throw new ManageSystemException("确认密码长度必须大于等于6位"); }
            confirmPassword = EncryptionService.EncryptText(confirmPassword.Trim());

            if (!confirmPassword.Equals(password)) { throw new ManageSystemException("2次密码输入不一致"); }

            if (!MemberService.UpdatePassword(id, confirmPassword))
            {
                throw new ManageSystemException("密码设置失败");
            }

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }
    }
}