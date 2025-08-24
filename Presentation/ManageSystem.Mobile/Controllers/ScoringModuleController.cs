using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Mobile.Models.ScoringModule;
using ManageSystem.Services.Configuration;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.ScoringModule;
using ManageSystem.Services.Security;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    /// <summary>
    /// 评分模块
    /// </summary>
    public class ScoringModuleController : MobileBaseController
    {
        #region 业务声明
        private readonly IEncryptionService _encryptionService;
        private readonly IMemberService _memberService;
        private readonly IHospitalService _hospitalService;
        private readonly IDataQualityScoreService _scoreService;
        private readonly IDataQualityScoreRecordService _scoreRecordService;
        private readonly IDataQualityScoreDetailService _scoreDetailService;
        private readonly IDataQualityReevaluationApplyService _reevaluationApplyService;
        private readonly IDataQualityJuryService _juryService;
        private readonly IDataQualityHospitalService _dqhospitalService;
        private readonly IDataQualityActionLogService _actionLogService;
        private readonly IValidateCodeService _validateCodeService;
        private readonly ISettingService _settingService;
        private readonly IMedicalAntibioticResultService _medicalAntibioticResultService;
        #endregion

        #region 构造函数
        public ScoringModuleController(IEncryptionService encryptionService, IHospitalService hospitalService, IMemberService memberService, IDataQualityScoreService dataQualityScoreService, IDataQualityScoreRecordService dataQualityScoreRecordService, IDataQualityScoreDetailService dataQualityScoreDetailService, IDataQualityReevaluationApplyService dataQualityReevaluationApplyService, IDataQualityJuryService dataQualityJuryService, IDataQualityHospitalService dataQualityHospitalService, IDataQualityActionLogService actionLogService, IValidateCodeService validateCodeService, ISettingService settingService, IMedicalAntibioticResultService medicalAntibioticResultService)
        {
            _encryptionService = encryptionService;
            _hospitalService = hospitalService;
            _memberService = memberService;
            _scoreService = dataQualityScoreService;
            _scoreRecordService = dataQualityScoreRecordService;
            _scoreDetailService = dataQualityScoreDetailService;
            _reevaluationApplyService = dataQualityReevaluationApplyService;
            _juryService = dataQualityJuryService;
            _dqhospitalService = dataQualityHospitalService;
            _actionLogService = actionLogService;
            _validateCodeService = validateCodeService;
            _settingService = settingService;
            _medicalAntibioticResultService = medicalAntibioticResultService;
        }
        #endregion

        #region 评分列表
        /// <summary>
        /// 评分列表
        /// </summary>
        /// <returns></returns>
        public ActionResult EvaluateScoringList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EvaluateScoringList(string token, int page, int pageSize)
        {
            #region 用户信息
            string tokenText = _encryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = _memberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            var list = _scoreService.QueryPageWithWap(member.Id, page - 1, pageSize);
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                data.Add(new
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    Intro = item.Intro,
                    Status = item.Status.GetDescription(),
                    DateTime = new
                    {
                        Begin = item.BeginTime.ToString("yyyy/MM/dd HH:ss"),
                        End = item.EndTime.ToString("yyyy/MM/dd HH:ss")
                    },
                    HospitalCount = _dqhospitalService.Count(r => r.DataQuality_Score_Id == item.Id && r.Mark > 0 && r.Status == 1),
                    JuryCount = _juryService.GetJuryCountByDataQualityScoreId(item.Id)
                });
            }

            return Json(new
            {
                status = true,
                data = data,
                nextPage = page + 1,
                existNextPage = list.TotalPages > page
            });
        }
        #endregion

        #region 医院列表
        public ActionResult HospitalList(long id)
        {
            ViewData["ScoreTitle"] = _scoreService.QueryEntity(id)?.Title;
            return View();
        }

        [HttpPost]
        public JsonResult HospitalList(long id, string token, int page, int pageSize)
        {
            #region 用户信息
            string tokenText = _encryptionService.DecryptText(token);
            long member_id = 0;
            Member member = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                member = _memberService.QueryEntity(member_id);
                if (member == null || member.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (member.Mark != 1 && member.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            var entity = _scoreService.QueryEntity(id);

            var list = _dqhospitalService.QueryPage(entity.Id, juryId: member_id, page: page - 1, pageSize: pageSize);

            List<object> data = new List<object>();


            foreach (var item in list)
            {
                var dqJury = _juryService.Query(r => r.DataQuality_Score_Id == id && r.DataQuality_Hospital_Id == item.Id && r.Jury_Id == member.Id && r.Mark > 0).FirstOrDefault();
                bool isScore = false;
                string datetime = "";
                double score = 0.0;

                bool showApply = false;
                bool showAudit = false;
                bool showEvaluate = true;
                List<DataQuality_Score_Record> evaluateDatas = new List<DataQuality_Score_Record>();

                if (dqJury != null && dqJury.Id > 0)
                {
                    evaluateDatas = _scoreRecordService.Query(r => r.DataQuality_Score_Id == item.DataQuality_Score_Id && r.DataQuality_Hospital_Id == item.Id && r.DataQuality_Jury_Id == dqJury.Id && r.Mark > 0);

                    isScore = evaluateDatas != null && evaluateDatas.Count() > 0;

                    var applyEntity = _reevaluationApplyService.Query(r => r.DataQuality_Hospital_Id == item.Id && r.DataQuality_Jury_Id == dqJury.Id && r.DataQuality_Score_Id == item.DataQuality_Score_Id && r.Mark > 0).OrderByDescending(r => r.InsertTime).FirstOrDefault();

                    if (isScore && entity.EndTime.CompareTo(DateTime.Now) > 0 && ((applyEntity?.Status) ?? 0) == 1 && ((applyEntity?.ActionStatus) ?? 0) == 0)
                    {
                        showAudit = true;
                        showApply = false;
                        showEvaluate = false;
                    }
                    else if (isScore && entity.EndTime.CompareTo(DateTime.Now) > 0 && (((applyEntity?.Status) ?? 0) < 2 || ((applyEntity?.Status) ?? 0) == 3) && ((applyEntity?.ActionStatus) ?? 0) == 0)
                    {
                        showApply = true;
                        showAudit = false;
                        showEvaluate = false;
                    }
                    else if (isScore && entity.EndTime.CompareTo(DateTime.Now) > 0 && ((applyEntity?.Status) ?? 0) == 2 && ((applyEntity?.ActionStatus) ?? 0) == 1)
                    {
                        showApply = true;
                        showAudit = false;
                        showEvaluate = false;
                    }
                    else if (isScore && entity.EndTime.CompareTo(DateTime.Now) > 0 && ((applyEntity?.Status) ?? 0) == 2 && ((applyEntity?.ActionStatus) ?? 0) == 0)
                    {
                        showEvaluate = true;
                        showApply = false;
                        showAudit = false;
                    }
                }


                data.Add(new
                {
                    Id = item.Id.ToString(),
                    Hospital = _hospitalService.QueryEntity(item.Hospital_Id)?.Name,
                    Explain = !string.IsNullOrWhiteSpace(item.Explain) ? item.Explain : "暂无说明",
                    EvaluateDatas = evaluateDatas.OrderByDescending(r => r.Evaluate_Time).Take(5).Select(r => new
                    {
                        EvaluateTime = r.Evaluate_Time.ToString("yyyy/MM/dd HH:mm"),
                        EvaluateScore = r.Evaluate_Score
                    }),
                    ShowApply = new
                    {
                        Show = showApply,
                        Url = showApply ? Url.Action("AnewApply", new { id = item.Id }) : null
                    },
                    ShowAudit = new { Show = showAudit },
                    ShowEvaluate = new
                    {
                        Show = showEvaluate,
                        Url = showEvaluate ? Url.Action("EvaluateScoringForm", new { id = item.Id }) : null
                    }
                });
            }

            return Json(new
            {
                status = true,
                data = data,
                nextPage = page + 1,
                existNextPage = list.TotalPages > page
            });
        }
        #endregion

        #region 评分页面
        public ActionResult EvaluateScoringForm(long id)
        {
            #region 标题/医院名称
            var entity = _dqhospitalService.QueryEntity(id);
            if (entity != null)
            {
                ViewData["ScoreTitle"] = _scoreService.QueryEntity(entity.DataQuality_Score_Id)?.Title;
                ViewData["HospitalName"] = _hospitalService.QueryEntity(entity.Hospital_Id)?.Name;
            }
            else
            {
                ViewData["ScoreTitle"] = "";
                ViewData["HospitalName"] = "";
            }
            #endregion

            IEnumerable<DataQuality_Hospital_PageInit> datas = _scoreService.GetHospitalPageInit(entity.DataQuality_Score_Id, entity.Id);

            #region 1. 标本来源

            #region 标本来源 - 门诊患者分离株所占比例

            #endregion

            #region 标本来源 - 血液和脑脊液标本分离株来源占比

            #endregion

            #endregion

            #region 2. 菌株数量

            #region 菌株数量 - 二级医院全年菌株数量

            #endregion

            #region 菌株数量 - 三级医院全年菌株数量

            #endregion

            #endregion

            #region 3. 药敏品种合理性

            var pageinit_3 = datas.Where(r => r.GroupSort == 3).OrderBy(r => r.Item_Sort);

            // 最终得分
            double _3_total_deduction = pageinit_3.Sum(r => (double?)r.Item_Value) ?? 0;

            _3_total_deduction = (30 > _3_total_deduction ? (30 - _3_total_deduction) : 0);

            ViewData["drug_sensitive"] = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = _3_total_deduction,
                item1 = (pageinit_3.Where(r => r.Item_Sort == 1).FirstOrDefault()?.Item_Value) ?? 0,
                item2 = (pageinit_3.Where(r => r.Item_Sort == 2).FirstOrDefault()?.Item_Value) ?? 0,
                item3 = (pageinit_3.Where(r => r.Item_Sort == 3).FirstOrDefault()?.Item_Value) ?? 0,
                item4 = (pageinit_3.Where(r => r.Item_Sort == 4).FirstOrDefault()?.Item_Value) ?? 0,
                item5 = (pageinit_3.Where(r => r.Item_Sort == 5).FirstOrDefault()?.Item_Value) ?? 0,
                item6 = (pageinit_3.Where(r => r.Item_Sort == 6).FirstOrDefault()?.Item_Value) ?? 0,
                item7 = (pageinit_3.Where(r => r.Item_Sort == 7).FirstOrDefault()?.Item_Value) ?? 0
            });

            #endregion

            #region 4. 重点监测耐药菌
            var pageinit_4 = datas.Where(r => r.GroupSort == 4).OrderBy(r => r.Item_Sort);
            double emphasis_score = pageinit_4.Sum(r => (double?)r.Item_Value) ?? 0;
            emphasis_score = emphasis_score >= 50 ? 50 : emphasis_score;
            emphasis_score = emphasis_score <= 0 ? 0 : emphasis_score;

            ViewData["emphasis"] = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                score = emphasis_score,
                total = emphasis_score,
                mrsa_score = (pageinit_4.Where(r => r.Item_Sort == 1).FirstOrDefault()?.Item_Value) ?? 0,
                mrsa_score_value = (pageinit_4.Where(r => r.Item_Sort == 1).FirstOrDefault()?.Item_Value) ?? 0,
                vrefm_score = (pageinit_4.Where(r => r.Item_Sort == 2).FirstOrDefault()?.Item_Value) ?? 0,
                vrefm_score_value = (pageinit_4.Where(r => r.Item_Sort == 2).FirstOrDefault()?.Item_Value) ?? 0,
                crkp_score = (pageinit_4.Where(r => r.Item_Sort == 3).FirstOrDefault()?.Item_Value) ?? 0,
                crkp_score_value = (pageinit_4.Where(r => r.Item_Sort == 3).FirstOrDefault()?.Item_Value) ?? 0,
                crpa_score = (pageinit_4.Where(r => r.Item_Sort == 4).FirstOrDefault()?.Item_Value) ?? 0,
                crpa_score_value = (pageinit_4.Where(r => r.Item_Sort == 4).FirstOrDefault()?.Item_Value) ?? 0,
                crab_score = (pageinit_4.Where(r => r.Item_Sort == 5).FirstOrDefault()?.Item_Value) ?? 0,
                crab_score_value = (pageinit_4.Where(r => r.Item_Sort == 5).FirstOrDefault()?.Item_Value) ?? 0,
                ctx_score = (pageinit_4.Where(r => r.Item_Sort == 6).FirstOrDefault()?.Item_Value) ?? 0,
                ctx_score_value = (pageinit_4.Where(r => r.Item_Sort == 6).FirstOrDefault()?.Item_Value) ?? 0
            });
            #endregion

            string timestamp = DateTime.Now.ToTimestamp().ToString();
            string nonce = _encryptionService.EncryptText(Guid.NewGuid().ToString("N"));
            ViewData["signature"] = _encryptionService.EncryptText($"{entity.Id}@@{entity.DataQuality_Score_Id}$${timestamp}||{nonce}");
            ViewData["timestamp"] = timestamp;
            ViewData["nonce"] = nonce;

            return View();
        }

        private ActionResult EvaluateScoringForm_Bak(long id)
        {
            #region 标题/医院名称
            var entity = _dqhospitalService.QueryEntity(id);
            if (entity != null)
            {
                ViewData["ScoreTitle"] = _scoreService.QueryEntity(entity.DataQuality_Score_Id)?.Title;
                ViewData["HospitalName"] = _hospitalService.QueryEntity(entity.Hospital_Id)?.Name;
            }
            else
            {
                ViewData["ScoreTitle"] = "";
                ViewData["HospitalName"] = "";
            }
            #endregion

            long medicalDataId = _scoreService.GetLastValidMedicalDataId(entity.Hospital_Id);
            List<MedicalAntibioticResult> medicalAntibioticResults = _scoreService.QueryMedicalAntibioticResult(medicalDataId);

            #region 标本来源 - 门诊患者分离株所占比例

            #endregion

            #region 标本来源 - 血液和脑脊液标本分离株来源占比

            #endregion

            #region 菌株数量 - 二级医院全年菌株数量

            #endregion

            #region 菌株数量 - 三级医院全年菌株数量

            #endregion

            #region 药敏品种合理性

            #region 1. 大肠埃希菌/肺炎克雷伯菌
            // 大肠埃希菌 ID = 1093
            // 肺炎克雷伯菌 ID = 535
            var _1093_535_medicalAntibioticResults = medicalAntibioticResults.Where(r => new List<long> { 535, 1093 }.Contains(r.OrganismId) && r.Mark > 0 && r.IsValid);
            int _1_count = _1093_535_medicalAntibioticResults.Count();
            int _1_score = 0;
            /**
             * 氨苄西林、哌拉西林/他唑巴坦、头孢唑林、头孢呋辛、头孢噻肟（或头孢曲松）、头孢他啶、头孢吡肟、阿米卡星
             * 菌株数量 ≥85% 不扣分
             * 菌株数量 ≤84% -1分/药物
             **/
            // 氨苄西林 AMP_NM,AMP_ND10
            double _1_amp_count = _1093_535_medicalAntibioticResults.Where(r => r.AMP_NM == 3 || r.AMP_ND10 == 3).Count();
            if ((_1_amp_count / _1_count) < 85.0) _1_score += 1;
            // 哌拉西林/他唑巴坦 TZP_NM,TZP_ND100
            double _1_tzp_count = _1093_535_medicalAntibioticResults.Where(r => r.TZP_NM == 3 || r.TZP_ND100 == 3).Count();
            if ((_1_tzp_count / _1_count) < 85.0) _1_score += 1;
            // 头孢唑林 CZO_NM,CZO_ND30
            double _1_czo_count = _1093_535_medicalAntibioticResults.Where(r => r.CZO_NM == 3 || r.CZO_ND30 == 3).Count();
            if ((_1_czo_count / _1_count) < 85.0) _1_score += 1;
            // 头孢呋辛 CXM_NM,CXM_ND30
            double _1_cxm_count = _1093_535_medicalAntibioticResults.Where(r => r.CXM_NM == 3 || r.CXM_ND30 == 3).Count();
            if ((_1_cxm_count / _1_count) < 85.0) _1_score += 1;
            // 头孢噻肟（或头孢曲松） CTX_NM,CTX_NE,CTX_ND30
            double _1_ctx_count = _1093_535_medicalAntibioticResults.Where(r => r.CTX_NM == 3 || r.CTX_NE == 3 || r.CTX_ND30 == 3 || r.CRO_NM == 3 || r.CRO_ND30 == 3).Count();
            if ((_1_ctx_count / _1_count) < 85.0) _1_score += 1;
            // 头孢他啶 CAZ_NM,CAZ_ND30
            double _1_caz_count = _1093_535_medicalAntibioticResults.Where(r => r.CAZ_NM == 3 || r.CAZ_ND30 == 3).Count();
            if ((_1_caz_count / _1_count) < 85.0) _1_score += 1;
            // 头孢吡肟 FEP_NM,FEP_ND30
            double _1_fep_count = _1093_535_medicalAntibioticResults.Where(r => r.FEP_NM == 3 || r.FEP_ND30 == 3).Count();
            if ((_1_fep_count / _1_count) < 85.0) _1_score += 1;
            // 阿米卡星 AMK_NM,AMK_ND30
            double _1_amk_count = _1093_535_medicalAntibioticResults.Where(r => r.AMK_NM == 3 || r.AMK_ND30 == 3).Count();
            if ((_1_amk_count / _1_count) < 85) _1_score += 1;

            /**
             * 多黏菌素、替加环素
             * 菌株数量 ≥10% 不扣分
             * 菌株数量 ≤9% -1分/药物
             **/
            // 多黏菌素 POL_NM,POL_ND300
            double _1_pol_count = _1093_535_medicalAntibioticResults.Where(r => r.POL_NM == 3 || r.POL_ND300 == 3).Count();
            if ((_1_pol_count / _1_count) < 10) _1_score += 1;
            // 替加环素 TGC_NM, TGC_ND15
            double _1_tgc_count = _1093_535_medicalAntibioticResults.Where(r => r.TGC_NM == 3 || r.TGC_ND15 == 3).Count();
            if ((_1_tgc_count / _1_count) < 10) _1_score += 1;

            // 单项扣分
            ViewData["drug_sensitive"] = $"{(0 - _1_score)}分";
            #endregion

            #region 2. 铜绿假单胞菌
            // 铜绿假单胞菌 ID = 732
            var _732_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 732 && r.Mark > 0 && r.IsValid);
            int _2_count = _732_medicalAntibioticResults.Count();
            int _2_score = 0;
            /**
             * 哌拉西林/他唑巴坦、头孢他啶、头孢吡肟、阿米卡星、环丙沙星（或左氧氟沙星）
             * 菌株数量 ≥85% 不扣分，
             * 菌株数量 ≤84% -1分/药物
             */
            // 哌拉西林/他唑巴坦 TZP_NM,TZP_ND100
            double _2_tzp_count = _732_medicalAntibioticResults.Where(r => r.TZP_NM == 3 || r.TZP_ND100 == 3).Count();
            if ((_2_tzp_count / _2_count) < 85.0) _2_score += 1;
            // 头孢他啶 CAZ_NM,CAZ_ND30
            double _2_caz_count = _732_medicalAntibioticResults.Where(r => r.CAZ_NM == 3 || r.CAZ_ND30 == 3).Count();
            if ((_2_caz_count / _2_count) < 85.0) _2_score += 1;
            // 头孢吡肟 FEP_NM,FEP_ND30
            double _2_fep_count = _732_medicalAntibioticResults.Where(r => r.FEP_NM == 3 || r.FEP_ND30 == 3).Count();
            if ((_2_fep_count / _2_count) < 85.0) _2_score += 1;
            // 阿米卡星 AMK_NM,AMK_ND30
            double _2_amk_count = _732_medicalAntibioticResults.Where(r => r.AMK_NM == 3 || r.AMK_ND30 == 3).Count();
            if ((_2_amk_count / _2_count) < 85.0) _2_score += 1;
            // 环丙沙星（或左氧氟沙星） CIP_NM, CIP_ND5
            double _2_cip_count = _732_medicalAntibioticResults.Where(r => r.CIP_NM == 3 || r.CIP_ND5 == 3 || r.LVX_NM == 3 || r.LVX_ND5 == 3).Count();
            if ((_2_cip_count / _2_count) < 85.0) _2_score += 1;

            /**
             * 多黏菌素
             * 菌株数量 ≥10% 不扣分
             * 菌株数量 ≤9% -1分/药物
             */
            double _2_pol_count = _732_medicalAntibioticResults.Where(r => r.POL_NM == 3 || r.POL_ND300 == 3).Count();
            if ((_2_pol_count / _2_count) < 10.0) _2_score += 1;

            // 单项扣分
            ViewData["3_2_deduction"] = $"{(0 - _2_score)}分";
            #endregion

            #region 3. 鲍曼不动杆菌
            // 鲍曼不动杆菌 ID = 1007
            var _1007_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 1007 && r.Mark > 0 && r.IsValid);
            int _3_count = _1007_medicalAntibioticResults.Count();
            int _3_score = 0;
            /**
             * 哌拉西林/他唑巴坦、头孢哌酮/舒巴坦、头孢他啶、头孢吡肟、阿米卡星、环丙沙星（或左氧氟沙星）
             * 菌株数量 ≥85% 不扣分，
             * 菌株数量 ≤84% -1分/药物
             */
            // 哌拉西林/他唑巴坦 TZP_NM,TZP_ND100
            double _3_tzp_count = _1007_medicalAntibioticResults.Where(r => r.TZP_NM == 3 || r.TZP_ND100 == 3).Count();
            if ((_3_tzp_count / _3_count) < 85.0) _3_score += 1;
            // 头孢哌酮/舒巴坦 CSL_NM,CSL_ND30,CSL_ND75
            double _3_csl_count = _1007_medicalAntibioticResults.Where(r => r.CSL_NM == 3 || r.CSL_ND30 == 3 || r.CSL_ND75 == 3).Count();
            if ((_3_csl_count / _3_count) < 85.0) _3_score += 1;
            // 头孢他啶 CAZ_NM,CAZ_ND30
            double _3_caz_count = _1007_medicalAntibioticResults.Where(r => r.CAZ_NM == 3 || r.CAZ_ND30 == 3).Count();
            if ((_3_caz_count / _3_count) < 85.0) _3_score += 1;
            // 头孢吡肟 FEP_NM,FEP_ND30
            double _3_fep_count = _1007_medicalAntibioticResults.Where(r => r.FEP_NM == 3 || r.FEP_ND30 == 3).Count();
            if ((_3_fep_count / _3_count) < 85.0) _3_score += 1;
            // 阿米卡星 AMK_NM,AMK_ND30
            double _3_amk_count = _1007_medicalAntibioticResults.Where(r => r.AMK_NM == 3 || r.AMK_ND30 == 3).Count();
            if ((_3_amk_count / _3_count) < 85.0) _3_score += 1;
            // 环丙沙星（或左氧氟沙星） CIP_NM, CIP_ND5
            double _3_cip_count = _1007_medicalAntibioticResults.Where(r => r.CIP_NM == 3 || r.CIP_ND5 == 3 || r.LVX_NM == 3 || r.LVX_ND5 == 3).Count();
            if ((_3_cip_count / _3_count) < 85.0) _3_score += 1;

            /**
             * 多黏菌素、替加环素
             * 菌株数量 ≥10% 不扣分
             * 菌株数量 ≤9% -1分/药物
             */
            // 多黏菌素 POL_NM,POL_ND300
            double _3_pol_count = _1007_medicalAntibioticResults.Where(r => r.POL_NM == 3 || r.POL_ND300 == 3).Count();
            if ((_3_pol_count / _3_count) < 10.0) _3_score += 1;
            // 替加环素 TGC_NM, TGC_ND15
            double _3_tgc_count = _1007_medicalAntibioticResults.Where(r => r.TGC_NM == 3 || r.TGC_ND15 == 3).Count();
            if ((_3_tgc_count / _3_count) < 10.0) _3_score += 1;

            // 单项扣分
            ViewData["3_3_deduction"] = $"{(0 - _3_score)}分";
            #endregion

            #region 4. 金黄色葡萄球菌
            // 金黄色葡萄球菌 ID = 1218
            var _1218_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 1218 && r.Mark > 0 && r.IsValid);
            int _4_count = _1218_medicalAntibioticResults.Count();
            int _4_score = 0;
            /**
             * 青霉素、头孢西丁（或苯唑西林）、红霉素、克林霉素、万古霉素、环丙沙星（或左氧氟沙星）
             * 菌株数量 ≥85% 不扣分，
             * 菌株数量 ≤84% -1分/药物
             */
            // 青霉素 PEN_NM,PEN_NE,PEN_ND10
            double _4_pen_count = _1218_medicalAntibioticResults.Where(r => r.PEN_NM == 3 || r.PEN_NE == 3 || r.PEN_ND10 == 3).Count();
            if ((_4_pen_count / _4_count) < 85.0) _4_score += 1;

            // 头孢西丁（或苯唑西林） FOX_NM,FOX_ND30 苯唑西林 OXA_NM,OXA_ND1
            double _4_fox_count = _1218_medicalAntibioticResults.Where(r => r.FOX_NM == 3 || r.FOX_ND30 == 3 || r.OXA_NM == 3 || r.OXA_ND1 == 3).Count();
            if ((_4_fox_count / _4_count) < 85.0) _4_score += 1;

            // 红霉素 ERY_NM,ERY_ND15
            double _4_ery_count = _1218_medicalAntibioticResults.Where(r => r.ERY_NM == 3 || r.ERY_ND15 == 3).Count();
            if ((_4_ery_count / _4_count) < 85.0) _4_score += 1;

            // 克林霉素 CLI_NM, CLI_ND2
            double _4_cli_count = _1218_medicalAntibioticResults.Where(r => r.CLI_NM == 3 || r.CLI_ND2 == 3).Count();
            if ((_4_cli_count / _4_count) < 85.0) _4_score += 1;

            // 万古霉素 VAN_NM,VAN_NE,VAN_ND30
            double _4_van_count = _1218_medicalAntibioticResults.Where(r => r.VAN_NM == 3 || r.VAN_NE == 3 || r.VAN_ND30 == 3).Count();
            if ((_4_van_count / _4_count) < 85.0) _4_score += 1;

            // 环丙沙星（或左氧氟沙星） CIP_NM, CIP_ND5 左氧氟沙星 LVX_NM,LVX_ND5
            double _4_cip_count = _1218_medicalAntibioticResults.Where(r => r.CIP_NM == 3 || r.CIP_ND5 == 3 || r.LVX_NM == 3 || r.LVX_ND5 == 3).Count();
            if ((_4_cip_count / _4_count) < 85.0) _4_score += 1;

            // 单项扣分
            ViewData["3_4_deduction"] = $"{(0 - _4_score)}分";
            #endregion

            #region 5. 肺炎链球菌
            // 肺炎链球菌 ID = 493
            var _493_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 493 && r.Mark > 0 && r.IsValid);
            int _5_count = _493_medicalAntibioticResults.Count();
            int _5_score = 0;
            /**
             * 头孢曲松（或头孢噻肟）、左旋氧氟沙星（或莫西沙星）、万古霉素、利奈唑胺、青霉素MIC
             * 菌株数量 ≥85% 不扣分，
             * 菌株数量 ≤84% -1分/药物
             */
            // 头孢噻肟（或头孢曲松） CTX_NM,CTX_NE,CTX_ND30
            double _5_ctx_count = _493_medicalAntibioticResults.Where(r => r.CTX_NM == 3 || r.CTX_NE == 3 || r.CTX_ND30 == 3 || r.CRO_NM == 3 || r.CRO_ND30 == 3).Count();
            if ((_5_ctx_count / _5_count) < 85.0) _5_score += 1;

            // 左旋氧氟沙星 LVX_NM,LVX_ND5 莫西沙星 MFX_NM,MFX_ND,MFX_ND5
            double _5_lvx_count = _493_medicalAntibioticResults.Where(r => r.LVX_NM == 3 || r.LVX_ND5 == 3 || r.MFX_NM == 3 || r.MFX_ND == 3 || r.MFX_ND5 == 3).Count();
            if ((_5_lvx_count / _5_count) < 85.0) _5_score += 1;

            // 万古霉素 VAN_NM,VAN_NE,VAN_ND30
            double _5_van_count = _493_medicalAntibioticResults.Where(r => r.VAN_NM == 3 || r.VAN_NE == 3 || r.VAN_ND30 == 3).Count();
            if ((_5_van_count / _5_count) < 85.0) _5_score += 1;

            // 利奈唑胺 LNZ_NM,LNZ_ND30
            double _5_lnz_count = _493_medicalAntibioticResults.Where(r => r.LNZ_NM == 3 || r.LNZ_ND30 == 3).Count();
            if ((_5_lnz_count / _5_count) < 85.0) _5_score += 1;

            // 青霉素 PEN_NM,PEN_NE
            double _5_pen_count = _493_medicalAntibioticResults.Where(r => r.PEN_NM == 3 || r.PEN_NE == 3).Count();
            if ((_5_pen_count / _5_count) < 85.0) _5_score += 1;

            // 单项扣分
            ViewData["3_5_deduction"] = $"{(0 - _5_score)}分";
            #endregion

            #region 6. 粪肠球菌
            // 粪肠球菌 ID = 908
            var _908_medicalAntibioticResults = medicalAntibioticResults.Where(r => r.OrganismId == 908 && r.Mark > 0 && r.IsValid);
            int _6_count = _908_medicalAntibioticResults.Count();
            int _6_score = 0;
            /**
             * 氨苄西林、高浓度庆大霉素/链霉素、万古霉素
             * 菌株数量 ≥85% 不扣分，
             * 菌株数量 ≤84% -1分/药物
             */
            // 氨苄西林 AMP_NM,AMP_ND10
            double _6_amp_count = _908_medicalAntibioticResults.Where(r => r.AMP_NM == 3 || r.AMP_ND10 == 3).Count();
            if ((_6_amp_count / _6_count) < 85.0) _6_score += 1;

            // 高浓度庆大霉素 GEH_NM,GEH_ND120
            double _6_geh_count = _908_medicalAntibioticResults.Where(r => r.GEH_NM == 3 || r.GEH_ND120 == 3).Count();
            if ((_6_geh_count / _6_count) < 85.0) _6_score += 1;

            // 链霉素 STR_NM,STR_ND10
            double _6_str_count = _908_medicalAntibioticResults.Where(r => r.STR_NM == 3 || r.STR_ND10 == 3).Count();
            if ((_6_str_count / _6_count) < 85.0) _6_score += 1;

            // 万古霉素 VAN_NM,VAN_NE,VAN_ND30
            double _6_van_count = _908_medicalAntibioticResults.Where(r => r.VAN_NM == 3 || r.VAN_NE == 3 || r.VAN_ND30 == 3).Count();
            if ((_6_van_count / _6_count) < 85.0) _6_score += 1;

            // 单项扣分

            #endregion

            #region 7. 流感嗜血杆菌和卡他莫拉菌
            // 流感嗜血杆菌 ID = 678
            // 卡他莫拉菌 ID = 560
            var _560_678_medicalAntibioticResults = medicalAntibioticResults.Where(r => (r.OrganismId == 678 || r.OrganismId == 560) && r.Mark > 0 && r.IsValid);
            int _7_count = _560_678_medicalAntibioticResults.Count();
            int _7_score = 0;
            /**
             * β-内酰胺酶
             * 菌株数量 ≥85% 不扣分，
             * 菌株数量 ≤84% -1分/药物
             */
            #endregion

            #region 最终得分
            // 最终得分
            int _3_total_deduction = _1_score + _2_score + _3_score + _4_score + _5_score + _6_score + _7_score;
            _3_total_deduction = (30 > _3_total_deduction ? (30 - _3_total_deduction) : 0);

            ViewData["drug_sensitive"] = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = _3_total_deduction,
                item1 = 0 - _1_score,
                item2 = 0 - _2_score,
                item3 = 0 - _3_score,
                item4 = 0 - _4_score,
                item5 = 0 - _5_score,
                item6 = 0 - _6_score,
                item7 = 0 - _7_score
            });
            #endregion

            #endregion

            #region 重点监测耐药菌
            DateTime now = DateTime.Now;
            var mainMonitor = _scoreService.GetCount(now.Year, entity.Hospital_Id);
            int DataCount = mainMonitor.DataCount;

            #region 甲氧西林耐药金黄色葡萄球菌，MRSA
            // 甲氧西林耐药金黄色葡萄球菌，MRSA
            double t_mrsa_rate = Math.Round((double)mainMonitor.MRSATotal / mainMonitor.DataTotal, 2) * 100;
            double h_mrsa_rate = Math.Round((double)mainMonitor.MRSACount / mainMonitor.DataCount, 2) * 100;
            double mrsa_score = MainScoreCalculation(t_mrsa_rate, h_mrsa_rate);
            double f_mrsa_score = mrsa_score * 0.09;
            #endregion

            #region 万古霉素耐药屎肠球菌，VREfm
            // 万古霉素耐药屎肠球菌，VREfm
            double t_vrefm_rate = Math.Round((double)mainMonitor.VREfmTotal / mainMonitor.DataTotal, 2) * 100;
            double h_vrefm_rate = Math.Round((double)mainMonitor.VREfmCount / mainMonitor.DataCount, 2) * 100;
            double vrefm_score = MainScoreCalculation(t_vrefm_rate, h_vrefm_rate);
            double f_vrefm_score = vrefm_score * 0.04;
            #endregion

            #region 碳青霉烯类耐药肺炎克雷伯菌， CRKP
            // 碳青霉烯类耐药肺炎克雷伯菌， CRKP
            double t_crkp_count = Math.Round((double)mainMonitor.CRKPTotal / mainMonitor.DataTotal, 2) * 100;
            double h_crkp_count = Math.Round((double)mainMonitor.CRKPCount / mainMonitor.DataCount, 2) * 100;
            double crkp_score = MainScoreCalculation(t_crkp_count, h_crkp_count);
            double f_crkp_score = crkp_score * 0.12;
            #endregion

            #region 碳青霉烯类耐药铜绿假单胞菌，CRPA
            // 碳青霉烯类耐药铜绿假单胞菌，CRPA
            double t_crpa_count = Math.Round((double)mainMonitor.CRPATotal / mainMonitor.DataTotal, 2) * 100;
            double h_crpa_count = Math.Round((double)mainMonitor.CRPACount / mainMonitor.DataCount, 2) * 100;
            double crpa_score = MainScoreCalculation(t_crpa_count, h_crpa_count);
            double f_crpa_score = crpa_score * 0.08;
            #endregion

            #region 碳青霉烯类耐药鲍曼不动杆菌，CRAB
            // 碳青霉烯类耐药鲍曼不动杆菌，CRAB
            double t_crab_count = Math.Round((double)mainMonitor.CRABTotal / mainMonitor.DataTotal, 2) * 100;
            double h_crab_count = Math.Round((double)mainMonitor.CRABCount / mainMonitor.DataCount, 2) * 100;
            double crab_score = MainScoreCalculation(t_crab_count, h_crab_count);
            double f_crab_score = crab_score * 0.09;
            #endregion

            #region 头孢噻肟/头孢曲松耐药大肠埃希菌，CTX/CRO-R-eco
            // 头孢噻肟/头孢曲松耐药大肠埃希菌，CTX/CRO-R-eco
            double t_ctx_count = Math.Round((double)mainMonitor.CRABTotal / mainMonitor.DataTotal, 2) * 100;
            double h_ctx_count = Math.Round((double)mainMonitor.CRABCount / mainMonitor.DataCount, 2) * 100;
            double ctx_score = MainScoreCalculation(t_ctx_count, h_ctx_count);
            double f_ctx_score = ctx_score * 0.08;
            #endregion

            double emphasis_score = f_mrsa_score + f_vrefm_score + f_crkp_score + f_crpa_score + f_crab_score + f_ctx_score;
            emphasis_score = emphasis_score >= 50 ? 50 : emphasis_score;
            emphasis_score = emphasis_score <= 0 ? 0 : emphasis_score;

            ViewData["emphasis"] = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                score = emphasis_score,
                total = emphasis_score,
                mrsa_score = f_mrsa_score,
                mrsa_score_value = f_mrsa_score,
                vrefm_score = f_vrefm_score,
                vrefm_score_value = f_vrefm_score,
                crkp_score = f_crkp_score,
                crkp_score_value = f_crkp_score,
                crpa_score = f_crpa_score,
                crpa_score_value = f_crpa_score,
                crab_score = f_crab_score,
                crab_score_value = f_crab_score,
                ctx_score = f_ctx_score,
                ctx_score_value = f_ctx_score
            });
            #endregion

            string timestamp = DateTime.Now.ToTimestamp().ToString();
            string nonce = _encryptionService.EncryptText(Guid.NewGuid().ToString("N"));
            ViewData["signature"] = _encryptionService.EncryptText($"{entity.Id}@@{entity.DataQuality_Score_Id}$${timestamp}||{nonce}");
            ViewData["timestamp"] = timestamp;
            ViewData["nonce"] = nonce;

            return View();
        }

        private static double MainScoreCalculation(double t_mrsa_rate, double h_mrsa_rate)
        {
            double mrsa_score = 0;
            if (t_mrsa_rate != h_mrsa_rate)
            {
                if (t_mrsa_rate > h_mrsa_rate)
                {
                    if ((t_mrsa_rate - h_mrsa_rate) <= 10.0)
                    {
                        mrsa_score += 10;
                    }
                    else if ((t_mrsa_rate - h_mrsa_rate) <= 20.0)
                    {
                        mrsa_score += 20;
                    }
                    else if ((t_mrsa_rate - h_mrsa_rate) <= 30.0)
                    {
                        mrsa_score += 30;
                    }
                    else if ((t_mrsa_rate - h_mrsa_rate) <= 40.0)
                    {
                        mrsa_score += 40;
                    }
                    else
                    {
                        mrsa_score += 50;
                    }
                }
                else if (h_mrsa_rate > t_mrsa_rate)
                {
                    if ((h_mrsa_rate - t_mrsa_rate) <= 10.0)
                    {
                        mrsa_score -= 10;
                    }
                    else if ((h_mrsa_rate - t_mrsa_rate) <= 20.0)
                    {
                        mrsa_score -= 20;
                    }
                    else if ((h_mrsa_rate - t_mrsa_rate) <= 30.0)
                    {
                        mrsa_score -= 30;
                    }
                    else if ((h_mrsa_rate - t_mrsa_rate) <= 40.0)
                    {
                        mrsa_score -= 40;
                    }
                    else
                    {
                        mrsa_score -= 50;
                    }
                }
            }
            else
            {
                mrsa_score = 50;
            }

            return mrsa_score;
        }

        [HttpPost]
        public JsonResult EvaluateScoringForm(EvaluateScoringFormParameter parameter)
        {
            #region 验证
            if (string.IsNullOrWhiteSpace(parameter.token))
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1000]"
                });
            }

            #region 用户信息
            string tokenText = _encryptionService.DecryptText(parameter.token);
            long member_id = 0;
            Member memberEntity = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                memberEntity = _memberService.QueryEntity(member_id);
                if (memberEntity == null || memberEntity.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (memberEntity.Mark != 1 && memberEntity.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            var entity = _dqhospitalService.QueryEntity(parameter.id);
            if (entity == null)
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1001]"
                });
            }

            string _signature = _encryptionService.EncryptText($"{entity.Id}@@{entity.DataQuality_Score_Id}$${parameter.timestamp}||{parameter.nonce}");
            if (!_signature.Equals(parameter.signature))
            {
                return Json(new
                {
                    status = false,
                    message = "无效的请求[code:2000]"
                });
            }

            var juries = _juryService.Query(r => r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.DataQuality_Hospital_Id == entity.Id && r.Mark > 0);
            if (juries == null || juries.Count() == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1002]"
                });
            }

            var juryEntity = juries.Where(r => r.Jury_Id == memberEntity.Id).OrderByDescending(r => r.InsertTime).FirstOrDefault();
            if (juryEntity == null || juryEntity.Id <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1003]"
                });
            }


            // 重复提交直接跳转到结果页面
            if (_scoreRecordService.Count(r => r.DataQuality_Hospital_Id == entity.Id && r.DataQuality_Jury_Id == juryEntity.Id && r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.Mark > 0 && r.Signature == parameter.signature && r.Timestamp == parameter.timestamp && r.Nonce == parameter.nonce) > 0)
            {
                return Json(new
                {
                    status = true,
                    ret_url = Url.Action("EvaluateScoringResult", new { id = entity.Id })
                });
            }

            #endregion

            DateTime now = DateTime.Now;

            DataQuality_Score_Record score_Record = new DataQuality_Score_Record
            {
                Id = CommonHelper.GuidToLongID,
                DataQuality_Hospital_Id = entity.Id,
                DataQuality_Jury_Id = juryEntity.Id,
                DataQuality_Score_Id = entity.DataQuality_Score_Id,
                Evaluate_Remark = null,
                InsertTime = now,
                Evaluate_Time = now,
                UpdateTime = now,
                Mark = 1,
                Signature = parameter.signature,
                Timestamp = parameter.timestamp,
                Nonce = parameter.nonce
            };

            List<DataQuality_Score_Detail> score_Details = new List<DataQuality_Score_Detail>();
            int idx = 1;
            foreach (KeyValuePair<string, List<EvaluateScoringItemFormParameter>> keyValuePair in parameter.scoredata)
            {
                keyValuePair.Value.ForEach(item =>
                {
                    score_Details.Add(new DataQuality_Score_Detail
                    {
                        DataQuality_Hospital_Id = entity.Id,
                        DataQuality_Jury_Id = juryEntity.Id,
                        DataQuality_Score_Group_Index = idx,
                        DataQuality_Score_Group = keyValuePair.Key,
                        DataQuality_Score_Id = entity.DataQuality_Score_Id,
                        DataQuality_Score_Sort = item.sort,
                        DataQuality_Score_Item = item.item,
                        DataQuality_Score_Record_Id = score_Record.Id,
                        DataQuality_Score_Score = item.score,
                        DataQuality_Score_Value = item.value,
                        Id = CommonHelper.GuidToLongID,
                        InsertTime = score_Record.InsertTime,
                        UpdateTime = score_Record.UpdateTime,
                        Mark = 1,
                        Version = 1,
                        Describe = null,
                        DeleteTime = new DateTime(1900, 1, 1, 0, 0, 0, 0)
                    });
                });
                idx += 1;
            }

            score_Record.Evaluate_Score = score_Details.Sum(r => r.DataQuality_Score_Score) + 30;

            if (score_Record.Evaluate_Score > 100.0)
            {
                return Json(new
                {
                    status = false,
                    message = "评分分值错误[code:1003]"
                });
            }

            _scoreRecordService.Insert(score_Record, score_Details);

            // 更新重评申请为已评分
            var applyList = _reevaluationApplyService.Query(r => r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.DataQuality_Hospital_Id == entity.Id && r.DataQuality_Jury_Id == juryEntity.Id && r.Mark > 0 && r.Status == 2 && r.ActionStatus == 0);
            applyList.ForEach(item =>
            {
                item.ActionStatus = 1;
                _reevaluationApplyService.Update(item);
            });

            string jurnName = !string.IsNullOrWhiteSpace(memberEntity.Name) ? memberEntity.Name : (!string.IsNullOrWhiteSpace(memberEntity.NickName) ? memberEntity.NickName : memberEntity.Phone);
            _actionLogService.Insert(new Core.Domain.ScoringModule.Log.DataQuality_Action_Log
            {
                Action_Log = $"{jurnName}对{(_hospitalService.QueryEntity(entity.Hospital_Id)?.Name)}提交了评分，评分：{score_Record.Evaluate_Score}",
                Action_Log_Detail = score_Record.SerializeObject(),
                Action_Time = now,
                Action_Type = "评分",
                DataQuality_Score_Id = score_Record.DataQuality_Score_Id,
                Id = score_Record.Id,
                InsertTime = now,
                Mark = 1,
                Platform = (int)ManageSystem.Core.Domain.Log.ActionSource.Mobile,
                UpdateTime = now,
                Operator_Id = memberEntity.Id
            });
            return Json(new
            {
                status = true,
                ret_url = Url.Action("EvaluateScoringResult", new { id = entity.Id })
            });
        }

        [HttpPost]
        public JsonResult EvaluateScoring_CalculateEmp(List<double> parameter)
        {
            var total = parameter.Sum();

            double score = total > 50 ? 50 : total;
            score = score <= 0 ? 0 : score;

            return Json(new
            {
                status = true,
                score = score.ToString("0.##"),
                total = total.ToString("0.##")
            });
        }
        #endregion

        #region 评分结果页面
        public ActionResult EvaluateScoringResult(long id)
        {
            var entity = _dqhospitalService.QueryEntity(id);
            if (entity != null)
            {
                ViewData["ScoreTitle"] = _scoreService.QueryEntity(entity.DataQuality_Score_Id)?.Title;
                ViewData["HospitalName"] = _hospitalService.QueryEntity(entity.Hospital_Id)?.Name;
            }
            else
            {
                ViewData["ScoreTitle"] = "";
                ViewData["HospitalName"] = "";
            }

            double? score = _scoreRecordService.Query(r => r.DataQuality_Hospital_Id == entity.Id && r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.Mark > 0).OrderByDescending(r => r.InsertTime).FirstOrDefault()?.Evaluate_Score;

            ViewData["Score"] = score ?? 0;

            ViewData["HospitalListUrl"] = Url.Action("HospitalList", new { id = entity.DataQuality_Score_Id });

            return View();
        }
        #endregion

        #region 重评申请页面
        public ActionResult AnewApply(long id)
        {
            var entity = _dqhospitalService.QueryEntity(id);
            if (entity != null)
            {
                ViewData["ScoreTitle"] = _scoreService.QueryEntity(entity.DataQuality_Score_Id)?.Title;
                ViewData["HospitalName"] = _hospitalService.QueryEntity(entity.Hospital_Id)?.Name;
            }
            else
            {
                ViewData["ScoreTitle"] = "";
                ViewData["HospitalName"] = "";
            }

            ViewData["Score"] = _scoreRecordService.QueryEntity(r => r.DataQuality_Hospital_Id == entity.Id && r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.Mark > 0)?.Evaluate_Score;

            return View();
        }

        [HttpPost]
        public JsonResult AnewApply(long id, string token, string reason)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1000]"
                });
            }

            #region 用户信息
            string tokenText = _encryptionService.DecryptText(token);
            long member_id = 0;
            Member memberEntity = null;
            try
            {
                string[] sArray = Regex.Split(tokenText, "@@");
                long.TryParse(sArray[0], out member_id);
                if (member_id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                memberEntity = _memberService.QueryEntity(member_id);
                if (memberEntity == null || memberEntity.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取用户信息" });
                }
                if (memberEntity.Mark != 1 && memberEntity.Mark != 2)
                {
                    return Json(new { status = false, message = "用户信息状态异常" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "未能获取用户信息" });
            }
            #endregion

            var entity = _dqhospitalService.QueryEntity(id);
            if (entity == null)
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1001]"
                });
            }



            var juries = _juryService.Query(r => r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.DataQuality_Hospital_Id == entity.Id && r.Mark > 0);
            if (juries == null || juries.Count() == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1002]"
                });
            }


            var juryEntity = juries.Where(r => r.Jury_Id == memberEntity.Id).OrderByDescending(r => r.InsertTime).FirstOrDefault();
            if (juryEntity == null || juryEntity.Id <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1003]"
                });
            }

            var recordEntity = _scoreRecordService.Query(r => r.DataQuality_Hospital_Id == entity.Id && r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.DataQuality_Jury_Id == juryEntity.Id && r.Mark > 0)
                .OrderByDescending(r => r.Evaluate_Time)
                .ThenByDescending(r => r.InsertTime)
                .ThenByDescending(r => r.Timestamp)
                .ThenBy(r => r.Id)
                .FirstOrDefault();

            if (recordEntity == null || recordEntity.Id <= 0)
            {
                return Json(new
                {
                    status = false,
                    message = "提交参数错误[code:1004]"
                });
            }

            if (_reevaluationApplyService.Count(r => r.DataQuality_Hospital_Id == entity.Id && r.DataQuality_Jury_Id == juryEntity.Id && r.DataQuality_Score_Id == entity.DataQuality_Score_Id && r.DataQuality_Score_Record_Id == recordEntity.Id && r.Mark > 0 && r.Status == 1) > 0)
            {
                return Json(new
                {
                    status = true,
                    ret_url = Url.Action("HospitalList", new { id = entity.DataQuality_Score_Id })
                });
            }
            DataQuality_Reevaluation_Apply apply = new DataQuality_Reevaluation_Apply
            {
                DataQuality_Hospital_Id = entity.Id,
                DataQuality_Jury_Id = juryEntity.Id,
                DataQuality_Score_Id = entity.DataQuality_Score_Id,
                DataQuality_Score_Record_Id = recordEntity.Id,
                Handler_Id = null,
                Handle_Remark = null,
                Handle_Time = null,
                Id = CommonHelper.GuidToLongID,
                InsertTime = DateTime.Now,
                Mark = 1,
                Reason = reason,
                Status = 1,
                ActionStatus = 0,
                Evaluate_Score = recordEntity.Evaluate_Score
            };
            _reevaluationApplyService.Insert(apply);

            // 提交申请后发送邮件通知管理员
            string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
            string servicePassword = ConfigHelper.GetConfigString("message.email.password");
            int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
            bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
            string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
            string displayname = ConfigHelper.GetConfigString("message.email.displayname");

            string noticeEmail = this._settingService.QueryValue<string>("web.notice.email"); //通知邮箱
            string noticePhone = this._settingService.QueryValue<string>("web.notice.phone"); //通知短信

            #region  短信通知管理员（原本）
            //string touser = ConfigHelper.GetConfigString("pingfen.notice.email.admin");
            //System.Text.StringBuilder emailContent = new System.Text.StringBuilder("您有新的重评申请需要审核，请登录CHINET数据云管理后台查看操作。<br />");
            //emailContent.Append($"重评申请信息：<br />");
            //emailContent.Append($"评比名称：{_scoreService.QueryEntity(entity.DataQuality_Score_Id)?.Title}<br />");
            //emailContent.Append($"评比医院：{_hospitalService.QueryEntity(entity.Hospital_Id)?.Name}<br />");
            //if (!string.IsNullOrWhiteSpace(memberEntity.Name))
            //{
            //    if (!string.IsNullOrWhiteSpace(memberEntity.Phone))
            //    {
            //        emailContent.Append($"评&emsp;&emsp;委：{memberEntity.Name}({memberEntity.Phone})<br />");
            //    }
            //    else
            //    {
            //        emailContent.Append($"评&emsp;&emsp;委：{memberEntity.Name}<br />");
            //    }
            //}
            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(memberEntity.Phone))
            //    {
            //        emailContent.Append($"评&emsp;&emsp;委：{memberEntity.LoginId}({memberEntity.Phone})<br />");
            //    }
            //    else
            //    {
            //        emailContent.Append($"评&emsp;&emsp;委：{memberEntity.LoginId}<br />");
            //    }
            //}

            //emailContent.Append($"评&emsp;&emsp;分：{recordEntity.Evaluate_Score}<br />");
            //emailContent.Append($"申请理由：{reason}<br />");
            //emailContent.Append($"登录CHINET数据云管理后台查看更多重评申请！");

            //bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { touser }, "CHINET数据云评分系统重评申请通知", emailContent.ToString());
            #endregion


            #region 通知相关用户（修改）

            #region 邮件发送
            var emails = noticeEmail.Split(',');
            foreach (var email in emails)
            {
                StringBuilder s = new StringBuilder();
                s.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:700px;\">");
                s.Append("<tr style=\"font-size:16px;font-weight: bold;text-indent:33px;\"><td colspan=\"3\" style=\"font-size:16px;font-weight: bold;text-indent:33px;line-height: 50px;text-indent: 0;width:700px;\">尊敬的管理员，" + memberEntity.Name + "评委提交了重评申请，还请及时登录后台审核，谢谢！</td></tr>");
                s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">感谢!</td></tr>");
                s.Append("<tr><td colspan=\"3\" style=\"padding: 20px 0;text-indent: 0;font-weight: bold;\">CHINET数据云</td></tr>");
                s.Append("</table>");
                try
                {
                    string serviceWebUrl = ConfigHelper.GetConfigString("FileWebUrl");
                }
                catch (Exception)
                {

                }

                if (entity != null && entity.Id > 0 && entity.Mark > 0)
                {
                    MessageEmail model = new MessageEmail()
                    {
                        Content = s.ToString(),
                        Email = email,
                        Remark = "",
                        SceneType = "",
                        SendType = 1,
                        Source = "web",
                        Status = 1,//状态：1、待发送   2：已发送   3：失败
                        Title = "重评通知"
                    };

                    try
                    {

                        bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { email }, model.Title, s.ToString());

                        if (result)
                        {
                            model.Status = 2;
                        }
                        else
                            model.Status = 3;
                    }
                    catch (Exception ex)
                    {
                        model.Remark = "发送邮件发生异常，异常信息：" + ex.Message + ",发送邮件失败";
                    }

                    //保存发送邮件记录
                    EngineContext.Current.Resolve<IMessageEmailService>().Insert(model);

                }
            }
            #endregion

            #region 短信发送

            var phones = noticePhone.Split(',');

            foreach (var phone in phones)
            {
                //将短信内容写入到数据库
                ValidateCode model = new ValidateCode();
                model.Id = CommonHelper.GuidToLongID;
                model.Source = (int)ValidateCodeSource.PC;
                model.Type = (int)ValidateCodeType.FindPasswordPhone;
                model.Code = "邮件发送";
                model.StartTime = DateTime.Now;
                model.OutTime = DateTime.Now;
                model.Value = phone;
                model.InsertTime = DateTime.Now;
                model.Describe = "重评活动通知，手机号码：" + phone;
                try
                {
                    string key = WebSettingService.GetWebSMS();

                    //【CHINET】您的验证码是#code#。如非本人操作，请忽略本短信
                    string tpl_value = HttpUtility.UrlEncode("#name#=" + memberEntity.Name);
                    string para = "&mobile=" + phone + "&tpl_id=210473&tpl_value=" + tpl_value;
                    string url = "http://v.juhe.cn/sms/send?key=" + key + "&dtype=json" + para;

                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] b = wc.DownloadData(url);
                    string s1 = Encoding.GetEncoding("utf-8").GetString(b);

                    SmsApiResult result = s1.DeserializeObject<SmsApiResult>();
                    if (result.error_code != 0)
                    {
                        model.Describe = "发送短信发生异常 Code不为0";
                    }
                }
                catch (Exception ex)
                {
                    model.Describe = "发送短信发生异常，异常信息：" + ex.Message;
                }
                try
                {
                    this._validateCodeService.Insert(model);
                }
                catch (Exception ex)
                {

                }


                #endregion

            }
            #endregion

            return Json(new
            {
                status = true,
                ret_url = Url.Action("HospitalList", new
                {
                    id = entity.DataQuality_Score_Id
                })
            });
        }
        #endregion

        #region 我的评分
        public ActionResult MyList()
        {
            return View();
        }
        #endregion
    }
}