using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Survey;
using ManageSystem.Core.Utility;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using ManageSystem.Services.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    /// <summary>
    /// 调研
    /// </summary>
    public class SurveyController : Controller
    {
        #region 业务声明
        private readonly IEncryptionService _encryptionService;
        private readonly IMemberService _memberService;
        private readonly ISurveySurveyService _surveyService;
        private readonly ISurveySubjectService _surveySubjectService;
        private readonly ISurveySubjectOptionService _surveySubjectOptionService;
        private readonly ISurveyRecordService _surveyRecordService;
        #endregion

        #region 构造函数
        public SurveyController(IEncryptionService encryptionService, IMemberService memberService, ISurveySurveyService surveyService, ISurveySubjectService surveySubjectService, ISurveySubjectOptionService surveySubjectOptionService, ISurveyRecordService surveyRecordService)
        {
            _encryptionService = encryptionService;
            _memberService = memberService;
            _surveyService = surveyService;
            _surveySubjectService = surveySubjectService;
            _surveySubjectOptionService = surveySubjectOptionService;
            _surveyRecordService = surveyRecordService;
        }
        #endregion

        #region 问卷答题

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Survey")]
        public ActionResult Index(long id)
        {
            var data = this._surveyService.QueryEntity(id);
            ViewBag.Name = "问答";
            ViewBag.Title = ViewBag.Name;

            // 不存在
            if (data == null)
            {
                return View("~/Views/Survey/Index.Null.cshtml");
            }

            // 不是启用的状态
            if (data.State != 1)
            {
                return View("~/Views/Survey/Index.Disabled.cshtml");
            }

            // 判断是否过期
            DateTime now = DateTime.Now;
            if (data.StartTime.Subtract(now).TotalSeconds >= 0)
            {
                // 还没开始
                return View("~/Views/Survey/Index.NotStarted.cshtml");
            }

            if (now.Subtract(data.EndTime).TotalSeconds >= 0)
            {
                // 已结束
                return View("~/Views/Survey/Index.Expired.cshtml");
            }

            ViewBag.NeedLogin = data.Rood == 2 ? 1 : 0;

            Survey_Record model = new Survey_Record
            {
                //1、用户回答问卷记录
                Id = CommonHelper.GuidToLongID,
                SurveyId = id,
                Mark = 0
            };

            //this._surveyRecordService.Insert(model);
            ViewBag.RecordId = model.Id;
            ViewBag.Id = data.Id;
            ViewBag.Name = data.Name;
            ViewBag.Remark = data.Remark;
            ViewBag.Title = ViewBag.Name;

            return View();
        }


        /// <summary>
        /// 第1页
        /// </summary>
        /// <param name="itemId">用户提交Id</param>
        /// <returns></returns>
        [Route("Survey/Page1/{id:long}")]
        public ActionResult Page1(long id)
        {
            return View();
        }

        [Route("Survey/Page1-Part2/{id:long}")]
        public ViewResult PagePartial2(long id)
        {
            return View();
        }

        [Route("Survey/Page2/{id:long}")]
        public ActionResult Page2(long id)
        {
            return View();
        }

        [Route("Survey/Page3/{id:long}")]
        public ActionResult Page3(long id)
        {
            return View();
        }

        [Route("Survey/Page4/{id:long}")]
        public ActionResult Page4(long id)
        {
            return View();
        }

        [Route("Survey/Page5/{id:long}")]
        public ActionResult Page5(long id)
        {
            return View();
        }

        [Route("Survey/Page6/{id:long}")]
        public ActionResult Page6(long id)
        {
            return View();
        }

        [Route("Survey/Page7/{id:long}")]
        public ActionResult Page7(long id)
        {
            return View();
        }

        [Route("Survey/Page8/{id:long}")]
        public ActionResult Page8(long id)
        {
            return View();
        }

        [Route("Survey/Page9/{id:long}")]
        public ActionResult Page9(long id)
        {
            return View();
        }

        [Route("Survey/Page10/{id:long}")]
        public ActionResult Page10(long id)
        {
            return View();
        }

        [Route("Survey/Page11/{id:long}")]
        public ActionResult Page11(long id)
        {
            return View();
        }

        [Route("Survey/Page12/{id:long}")]
        public ActionResult Page12(long id)
        {
            return View();
        }

        [Route("Survey/Page13/{id:long}")]
        public ActionResult Page13(long id)
        {
            return View();
        }

        [Route("Survey/Page14/{id:long}")]
        public ActionResult Page14(long id)
        {
            return View();
        }

        [Route("Survey/Page15/{id:long}")]
        public ActionResult Page15(long id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Submit(int page, Survey_Record model)
        {
            var entity = this._surveyRecordService.QueryEntity(model.Id);
            if (entity == null || entity.Id <= 0)
            {
                return Json(new { status = false, message = "提交失败" });
            }

            switch (page)
            {
                case 1:
                    entity.Content1 = model.Content1;
                    entity.Content2 = model.Content2;
                    entity.Content3 = model.Content3;
                    entity.Content4 = model.Content4;
                    break;
                case 2:
                    entity.Content5 = model.Content5;
                    entity.Content6 = model.Content6;
                    entity.Content7 = model.Content7;
                    entity.Content8 = model.Content8;
                    break;
                case 3:
                    entity.Content9 = model.Content9;
                    entity.Content10 = model.Content10;
                    entity.Content11 = model.Content11;
                    entity.Content12 = model.Content12;
                    break;
                case 4:
                    entity.Content13 = model.Content13;
                    entity.Content14 = model.Content14;
                    entity.Content15 = model.Content15;
                    entity.Content16 = model.Content16;
                    break;
                case 5:
                    entity.Content17 = model.Content17;
                    entity.Content18 = model.Content18;
                    entity.Content19 = model.Content19;
                    entity.Content20 = model.Content20;
                    break;
                case 6:
                    entity.Content21 = model.Content21;
                    entity.Content22 = model.Content22;
                    entity.Content23 = model.Content23;
                    entity.Content24 = model.Content24;
                    break;
                case 7:
                    entity.Content25 = model.Content25;
                    entity.Content26 = model.Content26;
                    break;
                case 8:
                    entity.Content26 = model.Content26;
                    entity.Content27 = model.Content27;
                    break;
                default:
                    entity.Content29 = model.Content29;
                    entity.Content30 = model.Content30;
                    entity.Content31 = model.Content31;
                    break;
            }

            this._surveyRecordService.Update(entity);
            return Json(new { status = true, message = "提交成功" });
        }


        [HttpPost]
        public JsonResult Issue_OnInit(long id, string token)
        {
            Survey_Survey surveyEntity = _surveyService.QueryEntity(id);

            if (surveyEntity == null || surveyEntity.Id <= 0)
            {
                return Json(new { status = false, message = "该问答不存在", redirect = false });
            }

            if (2 == surveyEntity.Rood && string.IsNullOrWhiteSpace(token))
            {
                return Json(new { status = false, message = "用户信息状态异常", redirect = true, returl = $"/login.cshtml?returl={System.Web.HttpUtility.UrlEncode($"/survey/{id}")}" });
            }

            if (2 == surveyEntity.Rood && !string.IsNullOrWhiteSpace(token))
            {
                Member member = TokenToMember(token);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "用户信息状态异常", redirect = true, returl = $"/login.cshtml?returl={System.Web.HttpUtility.UrlEncode($"/survey/{id}")}" });
                }

                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常", redirect = false });
                }
            }

            if (surveyEntity.State != 1)
                return Json(new { status = false, message = "该问答未开启", redirect = false });

            if (surveyEntity.StartTime > DateTime.Now || surveyEntity.EndTime < DateTime.Now)
                return Json(new { status = false, message = "该问答不在规定时间", redirect = false });


            return Json(new
            {
                status = true,
                survey = new
                {
                    id = surveyEntity.Id.ToString(),
                    title = surveyEntity.Name,
                    content = surveyEntity.Remark
                }
            });
        }

        [HttpPost]
        public JsonResult Issue_OnSubmit(Dictionary<string, string> parameters)
        {
            try
            {
                long _id = 0;
                if (!long.TryParse(parameters["Survey_Id"], out _id) || _id == 0)
                {
                    return Json(new { status = false, message = "提交参数错误[code:1000]" });
                }

                var surveyEntity = _surveyService.QueryEntity(_id);
                if (surveyEntity == null || surveyEntity.Id <= 0)
                {
                    return Json(new { status = false, message = "提交参数错误[code:1002]" });
                }

                // 非公开问卷，需要登录
                if (2 == surveyEntity.Rood && string.IsNullOrWhiteSpace(parameters["Token"]))
                {
                    return Json(new { status = false, message = "提交参数错误[code:2000]" });
                }

                Survey_Record recordEntity = new Survey_Record();

                if (2 == surveyEntity.Rood && !string.IsNullOrWhiteSpace(parameters["Token"]))
                {
                    Member member = TokenToMember(parameters["Token"]);
                    if (member == null || member.Id <= 0)
                    {
                        return Json(new { status = false, message = "提交参数错误[code:2001]" });
                    }

                    if (member.Mark != 1 && member.Mark != 2)
                    {
                        return Json(new { status = false, message = "提交参数错误[code:2010]" });
                    }

                    recordEntity.MemberId = member.Id;
                    recordEntity.MemberName = member.Name;
                    recordEntity.MemberPhone = member.Phone;
                }
                else if (!string.IsNullOrWhiteSpace(parameters["Token"]))
                {
                    Member member = TokenToMember(parameters["Token"]);
                    if (member != null && member.Id > 0 && member.Mark > 0)
                    {
                        recordEntity.MemberId = member.Id;
                        recordEntity.MemberName = member.Name;
                        recordEntity.MemberPhone = member.Phone;
                    }
                }

                recordEntity.SurveyId = surveyEntity.Id;
                recordEntity.Rood = surveyEntity.Rood;
                recordEntity.InsertTime = DateTime.Now;
                recordEntity.UpdateTime = recordEntity.InsertTime;
                recordEntity.Mark = 1;
                recordEntity.Version = 1;
                recordEntity.Describe = "";
                recordEntity.Content1 = parameters["Content1"] ?? "";
                recordEntity.Content2 = parameters["Content2"] ?? "";
                recordEntity.Content3 = parameters["Content3"] ?? "";
                recordEntity.Content4 = parameters["Content4"] ?? "";
                recordEntity.Content5 = parameters["Content5"] ?? "";
                recordEntity.Content6 = parameters["Content6"] ?? "";
                recordEntity.Content7 = parameters["Content7"] ?? "";
                recordEntity.Content8 = parameters["Content8"] ?? "";
                recordEntity.Content9 = parameters["Content9"] ?? "";
                recordEntity.Content10 = parameters["Content10"] ?? "";
                recordEntity.Content11 = parameters["Content11"] ?? "";
                recordEntity.Content12 = parameters["Content12"] ?? "";
                recordEntity.Content13 = parameters["Content13"] ?? "";
                recordEntity.Content14 = parameters["Content14"] ?? "";
                recordEntity.Content15 = parameters["Content15"] ?? "";
                recordEntity.Content16 = parameters["Content16"] ?? "";
                recordEntity.Content17 = parameters["Content17"] ?? "";
                recordEntity.Content18 = parameters["Content18"] ?? "";
                recordEntity.Content19 = parameters["Content19"] ?? "";
                recordEntity.Content20 = parameters["Content20"] ?? "";
                recordEntity.Content21 = parameters["Content21"] ?? "";
                recordEntity.Content22 = parameters["Content22"] ?? "";
                recordEntity.Content23 = parameters["Content23"] ?? "";
                recordEntity.Content24 = parameters["Content24"] ?? "";
                recordEntity.Content25 = parameters["Content25"] ?? "";
                recordEntity.Content26 = parameters["Content26"] == null ? "" : parameters["Content26"];
                recordEntity.Content27 = parameters["Content27"] == null ? "" : parameters["Content27"];
                recordEntity.Content28 = parameters["Content28"] == null ? "" : parameters["Content28"];
                recordEntity.Content29 = parameters["Content29"] == null ? "" : parameters["Content29"];
                recordEntity.Content30 = parameters["Content30"] == null ? "" : parameters["Content30"];
                recordEntity.Content31 = parameters["Content31"] == null ? "" : parameters["Content31"];

                _surveyRecordService.Insert(recordEntity);
                return Json(new { status = true, message = "提交成功", page = Url.Action("Finish", new { id = recordEntity.Id }).ToLowerInvariant() });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "提交发生错误[code:500]" });
            }
        }

        [Route("Survey/Finish/{id:long}")]
        public ActionResult Finish(long id)
        {
            return View();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private Member TokenToMember(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            string tokenText = _encryptionService.DecryptText(token);
            Member memberEntity = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                if (long.TryParse(sArray[0], out long member_id) && member_id > 0)
                {
                    memberEntity = _memberService.QueryEntity(member_id);
                }
            }
            catch (Exception)
            {
            }
            return memberEntity;
        }

        private Tuple<string, string> RegexSplit(string input, string pattern)
        {
            string value = "";
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, pattern);
            if (match.Success)
            {
                value = match.Value;
            }
            input = input.Replace($":{value}", "");
            return new Tuple<string, string>(input, value);
        }
        #endregion

    }
}