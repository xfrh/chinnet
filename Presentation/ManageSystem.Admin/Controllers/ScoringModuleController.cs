using ManageSystem.Admin.App_Start;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.ScoringModule;
using ManageSystem.Core.Domain.ScoringModule.Enum;
using ManageSystem.Core.Domain.ScoringModule.Log;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Extensions;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Configuration;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.ScoringModule;
using ManageSystem.Services.SystemSet;
using ManageSystem.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ManageSystem.Admin.Controllers
{
    /// <summary>
    /// 评分模块
    /// </summary>
    public class ScoringModuleController : AdminBaseController
    {
        #region 业务申明
        private readonly IUserinfoService _userinfoService;
        private readonly IHospitalService _hospitalService;
        private readonly IMemberService _memberService;
        private readonly IDataQualityScoreService _scoreService;
        private readonly IDataQualityScoreRecordService _scoreRecordService;
        private readonly IDataQualityScoreDetailService _scoreDetailService;
        private readonly IDataQualityReevaluationApplyService _reevaluationApplyService;
        private readonly IDataQualityJuryService _juryService;
        private readonly IDataQualityHospitalService _dqhospitalService;
        private readonly IDataQualityActionLogService _actionLogService;
        private readonly IValidateCodeService _validateCodeService;

        private readonly HttpClient httpClient = new HttpClient();


        #endregion

        #region 构造函数
        public ScoringModuleController(IUserinfoService userinfoService, IHospitalService hospitalService, IMemberService memberService, IDataQualityScoreService dataQualityScoreService, IDataQualityScoreRecordService dataQualityScoreRecordService, IDataQualityScoreDetailService dataQualityScoreDetailService, IDataQualityReevaluationApplyService dataQualityReevaluationApplyService, IDataQualityJuryService dataQualityJuryService, IDataQualityHospitalService dataQualityHospitalService, IDataQualityActionLogService actionLogService, IValidateCodeService validateCodeService)
        {
            _userinfoService = userinfoService;
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
        }
        #endregion

        #region 评分列表
        [CheckRole(false, false)]
        public ActionResult List()
        {
            var statusEnum = (from DataQuality_Score_Status viewScope in Enum.GetValues(typeof(DataQuality_Score_Status)) select new SelectListItem { Value = ((int)viewScope).ToString(), Text = viewScope.GetDescription() }).ToList();
            statusEnum.Insert(0, new SelectListItem { Value = null, Text = "不限" });
            ViewData["DropSatusEnum"] = statusEnum;

            if (CacheManager.IsSet($"ScoringModule_Create_{LoginUserinfo.Id}"))
            {
                var id = CacheManager.Get<long>($"ScoringModule_Create_{LoginUserinfo.Id}");
                _dqhospitalService.Delete(r => r.DataQuality_Score_Id == id && r.Status == 0);
                CacheManager.Remove($"ScoringModule_Create_{LoginUserinfo.Id}");
            }
            return View();
        }

        [HttpPost, CheckRole(false, false)]
        public JsonResult List(string title, string intro, DataQuality_Score_Status? status, DataSourceRequest command)
        {
            var list = _scoreService.QueryPage(title, intro, status, command.Page - 1, command.PageSize);
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                data.Add(new
                {
                    Id = item.Id.ToString(),
                    InsertTime = item.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                    Title = item.Title,
                    Status = item.Status.GetDescription(),
                    DateTimeRange = new
                    {
                        Begin = item.BeginTime.ToString("yyyy-MM-dd HH:mm"),
                        End = item.EndTime.ToString("yyyy-MM-dd HH:mm")
                    },
                    HospitalTotal = _dqhospitalService.Count(r => r.DataQuality_Score_Id == item.Id && r.Mark > 0 && r.Status == 1),
                    Intro = item.Intro
                });
            }
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 变更评比信息状态
        [HttpPost, CheckRole(false, false)]
        public ActionResult ChangeDataQualityScoreStatus(long id, DataQuality_Score_Status status)
        {
            var entity = _scoreService.QueryEntity(id);
            entity.Status = status;
            entity.UpdateTime = DateTime.Now;

            _scoreService.Update(entity);
            _actionLogService.Insert(new DataQuality_Action_Log
            {
                Action_Log = "变更评比信息状态为：" + status.GetDescription(),
                Action_Log_Detail = entity.SerializeObject(),
                Action_Time = entity.UpdateTime,
                Action_Type = "编辑",
                DataQuality_Score_Id = entity.Id,
                Id = CommonHelper.GuidToLongID,
                InsertTime = DateTime.Now,
                Mark = 1,
                Operator_Id = LoginUserinfo.Id,
                Platform = (int)ActionSource.Admin
            });
            ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, LoginUserinfo.Id, string.IsNullOrWhiteSpace(LoginUserinfo.Name) ? LoginUserinfo.LoginId : LoginUserinfo.Name, "变更评比信息状态为：" + status.GetDescription(), entity.SerializeObject());
            SuccessNotification("评比信息状态变更成功！");
            return RedirectToAction("List");
        }
        #endregion

        #region 新增评分
        [CheckRole(false, false)]
        // GET: ScoringModule
        public ActionResult Create()
        {
            DataQuality_Score model = new DataQuality_Score
            {
                Id = CommonHelper.GuidToLongID
            };
            if (CacheManager.IsSet($"ScoringModule_Create_{LoginUserinfo.Id}"))
            {
                model.Id = CacheManager.Get<long>($"ScoringModule_Create_{LoginUserinfo.Id}");
            }

            if (model.Id == 0)
            {
                model.Id = CommonHelper.GuidToLongID;
            }
            else
            {
                if (_scoreService.Count(r => r.Id == model.Id && r.Mark > 0) > 0)
                {
                    model.Id = CommonHelper.GuidToLongID;
                }
            }
            CacheManager.Set($"ScoringModule_Create_{LoginUserinfo.Id}", model.Id, 24 * 60);
            ViewBag.Flag = true;
            return View(model);
        }

        [HttpPost, ValidateInput(false), CheckRole(false, false)]
        public ActionResult Create(DataQuality_Score model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    ErrorNotification("参数错误");
                    if (CacheManager.IsSet($"ScoringModule_Create_{LoginUserinfo.Id}"))
                    {
                        CacheManager.Remove($"ScoringModule_Create_{LoginUserinfo.Id}");
                    }
                    return RedirectToAction("Create");
                }

                if (_scoreService.Count(r => r.Id == model.Id) > 0)
                {
                    ErrorNotification("请勿重复提交");
                    if (CacheManager.IsSet($"ScoringModule_Create_{LoginUserinfo.Id}"))
                    {
                        CacheManager.Remove($"ScoringModule_Create_{LoginUserinfo.Id}");
                    }
                    return RedirectToAction("Create");
                }

                if (_scoreService.Count(r => r.Title == model.Title && r.Mark > 0) > 0)
                {
                    ErrorNotification("请勿重复提交");
                    if (CacheManager.IsSet($"ScoringModule_Create_{LoginUserinfo.Id}"))
                    {
                        CacheManager.Remove($"ScoringModule_Create_{LoginUserinfo.Id}");
                    }
                    return RedirectToAction("Create");
                }

                var entity = new DataQuality_Score
                {
                    Id = model.Id,
                    Title = model.Title,
                    BeginTime = model.BeginTime,
                    EndTime = model.EndTime,
                    InsertTime = DateTime.Now,
                    Intro = model.Intro,
                    Mark = 1,
                    Status = DataQuality_Score_Status.NotStarted
                };
                var hospitalCount = _dqhospitalService.Count(r => r.DataQuality_Score_Id == entity.Id && r.Mark > 0);
                if (hospitalCount <= 0)
                {
                    ViewBag.Flag = false;
                    ErrorNotification("请选择参与评比的医院");
                    return View(model);
                }

                // 判断医院数量和评委数量
                if (_juryService.Count(r => r.DataQuality_Score_Id == entity.Id && r.Mark > 0) != hospitalCount)
                {
                    ViewBag.Flag = false;
                    ErrorNotification("请为所有参与评比的医院选择评委");
                    return View(model);
                }


                _scoreService.Insert(entity);
                _dqhospitalService.Update(r => r.DataQuality_Score_Id == entity.Id && r.Mark > 0 && r.Status == 0, update => new DataQuality_Hospital { Status = 1 });
                if (CacheManager.IsSet($"ScoringModule_Create_{LoginUserinfo.Id}"))
                {
                    CacheManager.Remove($"ScoringModule_Create_{LoginUserinfo.Id}");
                }

                ActionLogService.Insert(ActionType.Create, ActionSource.Admin, LoginUserinfo.Id, string.IsNullOrWhiteSpace(LoginUserinfo.Name) ? LoginUserinfo.LoginId : LoginUserinfo.Name, "后台新增自动评分信息", entity.SerializeObject());
                _actionLogService.Insert(new DataQuality_Action_Log
                {
                    Action_Log = "新增自动评分",
                    Action_Log_Detail = entity.SerializeObject(),
                    Action_Time = DateTime.Now,
                    Action_Type = "新增",
                    DataQuality_Score_Id = entity.Id,
                    Id = CommonHelper.GuidToLongID,
                    InsertTime = DateTime.Now,
                    Mark = 1,
                    Operator_Id = LoginUserinfo.Id,
                    Platform = (int)ActionSource.Admin
                });

                SuccessNotification("新增自动评分信息成功");
                return this.RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(GetType(), ex);
                ErrorNotification("提交发生异常");
            }
            ViewBag.Flag = false;
            return View(model);
        }
        #endregion

        #region 请求医院列表
        [HttpPost, CheckRole(false, false)]
        public JsonResult CreateWithCheckedHospitalList(long id, DataSourceRequest command)
        {
            var list = _dqhospitalService.QueryPage(id, null, command.Page - 1, command.PageSize);
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var hospital = _hospitalService.QueryEntity(item.Hospital_Id);
                List<string> juryNames = _juryService.GetJuryNames(item.DataQuality_Score_Id, item.Id);

                data.Add(new
                {
                    Id = item.Id.ToString(),
                    HospitalName = hospital.Name,
                    Contacts = hospital.ContactsUser,
                    ContactWay = hospital.ContactsTel,
                    Sort = new
                    {
                        Key = item.Id.ToString(),
                        Value = item.Sort.ToString()
                    },
                    Explain = new
                    {
                        Key = item.Id.ToString(),
                        Value = !string.IsNullOrWhiteSpace(item.Explain) ? item.Explain : ""
                    },
                    Jury = new
                    {
                        Key = item.Id.ToString(),
                        Value = juryNames.Count > 0 ? string.Join("、", juryNames) : "选择"
                    }
                });
            }

            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }

        [HttpPost, CheckRole(false, false)]
        public JsonResult CreateWithHospitalList(string hospitalName, string contacts, int? status, int? isTeam, DataSourceRequest command)
        {
            var list = _hospitalService.QueryPage(hospitalName, status, isTeam, contacts, command.Page - 1, command.PageSize);
            var data = list.Select(item => new
            {
                Id = item.Id.ToString(),
                HospitalName = item.Name,
                Status = item.State ? "正常" : "禁用",
                HospitalAddress = item.Address,
                Contacts = item.ContactsUser,
                ContactWay = item.ContactsTel
            });

            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }

        #endregion

        #region 确认提交选择参与医院
        [HttpPost, CheckRole(false, false)]
        public JsonResult CreateWithConfirmHospital(long id, List<long> hospitalIds)
        {
            try
            {
                List<DataQuality_Hospital> list = new List<DataQuality_Hospital>();
                DateTime now = DateTime.Now;
                foreach (var item in hospitalIds)
                {
                    if (!_dqhospitalService.CheckHospital(id, item))
                    {
                        list.Add(new DataQuality_Hospital
                        {
                            DataQuality_Score_Id = id,
                            Describe = null,
                            Explain = null,
                            Hospital_Id = item,
                            Id = CommonHelper.GuidToLongID,
                            InsertTime = now,
                            Mark = 1,
                            Sort = 1,
                            Status = 0
                        });
                    }
                }
                if (list.Count > 0)
                {
                    _dqhospitalService.Insert(list);
                }
                return Json(new
                {
                    status = true,
                    message = "操作成功"
                });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new
                {
                    status = false,
                    message = "操作失败"
                });
            }
        }

        [HttpPost, CheckRole(false, false)]
        public JsonResult EditWithConfirmHospital(long id, List<long> hospitalIds)
        {
            try
            {
                List<DataQuality_Hospital> list = new List<DataQuality_Hospital>();
                DateTime now = DateTime.Now;
                foreach (var item in hospitalIds)
                {
                    if (!_dqhospitalService.CheckHospital(id, item))
                    {
                        list.Add(new DataQuality_Hospital
                        {
                            DataQuality_Score_Id = id,
                            Describe = null,
                            Explain = null,
                            Hospital_Id = item,
                            Id = CommonHelper.GuidToLongID,
                            InsertTime = now,
                            Mark = 1,
                            Sort = 1,
                            Status = 1
                        });
                    }
                }
                if (list.Count > 0)
                {
                    _dqhospitalService.Insert(list);
                }
                return Json(new
                {
                    status = true,
                    message = "操作成功"
                });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(this.GetType(), ex);
                return Json(new
                {
                    status = false,
                    message = "操作失败"
                });
            }
        }
        #endregion

        #region 取消医院参与
        /// <summary>
        /// 删除医院
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPost, CheckRole(false, false)]
        public JsonResult CreateHospitalWithDelete(long id, long dqsid)
        {
            try
            {
                this._dqhospitalService.Delete(id);
                this._juryService.Delete(r => r.DataQuality_Hospital_Id == id && r.DataQuality_Score_Id == dqsid);
                return Json(new
                {
                    status = true,
                    message = "已删除"
                });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(GetType(), ex);
                return Json(new
                {
                    status = false,
                    message = "删除失败"
                });
            }
        }
        #endregion

        #region 评委列表
        [HttpPost, CheckRole(false, false)]
        public JsonResult CreateWithJuryList(string name, int? status, string hospital, DataSourceRequest command)
        {
            List<long> hospitalIds = new List<long>();
            if (!string.IsNullOrWhiteSpace(hospital))
            {
                hospitalIds = _hospitalService.Query(r => r.Name.Contains(hospital)).Select(r => r.Id).ToList();
            }
            var list = _memberService.QueryPage(name, name, status, hospitalIds, command.Page - 1, command.PageSize);
            var data = list.Select(item => new
            {
                Id = item.Id.ToString(),
                Name = item.Name,
                Mobile = item.Phone,
                Status = ((MemberStatus)item.Status).GetDescription(),
                Hospital = (_hospitalService.QueryEntity(item.HospitalId)?.Name) ?? ""
            });
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 设置评委
        /// <summary>
        /// 设置评委
        /// </summary>
        /// <param name="dqid"></param>
        /// <param name="dqHospitalId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpPost, CheckRole(false, false)]
        public JsonResult CreateHospitalWithSettingJury(long dqid, long dqHospitalId, long memberId)
        {
            try
            {
                List<DataQuality_Jury> list = _juryService.Query(r => r.DataQuality_Score_Id == dqid && r.DataQuality_Hospital_Id == dqHospitalId && r.Mark > 0);

                if (list.Count() == 0)
                {
                    _juryService.Insert(new DataQuality_Jury
                    {
                        DataQuality_Hospital_Id = dqHospitalId,
                        DataQuality_Score_Id = dqid,
                        Id = CommonHelper.GuidToLongID,
                        InsertTime = DateTime.Now,
                        Jury_Id = memberId,
                        Mark = 1
                    });
                }
                else
                {
                    list.ForEach(item =>
                    {
                        item.Jury_Id = memberId;
                        _juryService.Update(item);
                    });
                }
                return Json(new { status = true, message = "操作成功" });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(GetType(), ex);
                return Json(new { status = false, message = "操作异常" });
            }
        }
        #endregion

        #region 设置医院顺序
        [HttpPost, CheckRole(false, false)]
        public JsonResult SettingHospitalSortByCreate(long dqid, long dqHospitalId, long sort)
        {
            try
            {
                if (sort <= 0)
                {
                    return Json(new
                    {
                        status = false,
                        message = "排序值必须是大于0的整数"
                    });
                }

                _dqhospitalService.Update(r => r.DataQuality_Score_Id == dqid && r.Id == dqHospitalId && r.Mark > 0, update => new DataQuality_Hospital { Sort = sort });

                return Json(new
                {
                    status = true,
                    message = "操作成功"
                });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(GetType(), ex);
                return Json(new
                {
                    status = false,
                    message = "操作失败"
                });
            }
        }
        #endregion

        #region 设置医院参与说明
        [HttpPost, CheckRole(false, false)]
        public JsonResult SettingHospitalExplainByCreate(long dqid, long dqHospitalId, string explain)
        {
            try
            {
                _dqhospitalService.Update(r => r.DataQuality_Score_Id == dqid && r.Id == dqHospitalId && r.Mark > 0, update => new DataQuality_Hospital { Explain = explain });

                return Json(new
                {
                    status = true,
                    message = "操作成功"
                });
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(GetType(), ex);
                return Json(new
                {
                    status = false,
                    message = "操作失败"
                });
            }
        }
        #endregion

        #region 编辑评分
        [CheckRole(false, false)]
        // GET: ScoringModule
        public ActionResult Edit(long id)
        {
            var model = _scoreService.QueryEntity(id);

            // 删除未保存的参与医院
            _dqhospitalService.DeleteUnSaveDataQualityHospitalByDataQualityScoreId(model.Id);
            return View(model);
        }

        [HttpPost, ValidateInput(false), CheckRole(false, false)]
        public ActionResult Edit(DataQuality_Score model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    ErrorNotification("参数错误");
                    return RedirectToAction("List");
                }

                if (_scoreService.Count(r => r.Id == model.Id) == 0)
                {
                    ErrorNotification("未找到自动评分信息");
                    return RedirectToAction("List");
                }

                if (_scoreService.Count(r => r.Id != model.Id && r.Title == model.Title && r.Mark > 0) > 0)
                {
                    ErrorNotification("评比名称以及存在");
                    return View(model);
                }

                var entity = _scoreService.QueryEntity(model.Id);
                _scoreService.Update(r => r.Id == model.Id, update => new DataQuality_Score
                {
                    Title = model.Title,
                    BeginTime = model.BeginTime,
                    EndTime = model.EndTime,
                    UpdateTime = DateTime.Now,
                    Mark = 2
                });

                var hospitalCount = _dqhospitalService.Count(r => r.DataQuality_Score_Id == entity.Id && r.Mark > 0);
                if (hospitalCount <= 0)
                {
                    ViewBag.Flag = false;
                    ErrorNotification("请选择参与评比的医院");
                    return View(model);
                }

                // 判断医院数量和评委数量
                if (_juryService.Count(r => r.DataQuality_Score_Id == entity.Id && r.Mark > 0) != hospitalCount)
                {
                    ViewBag.Flag = false;
                    ErrorNotification("请为所有参与评比的医院选择评委");
                    return View(model);
                }

                _dqhospitalService.Update(r => r.DataQuality_Score_Id == entity.Id && r.Mark > 0 && r.Status == 0, update => new DataQuality_Hospital { Status = 1 });


                ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, LoginUserinfo.Id, string.IsNullOrWhiteSpace(LoginUserinfo.Name) ? LoginUserinfo.LoginId : LoginUserinfo.Name, "后台编辑自动评分信息", entity.SerializeObject());
                _actionLogService.Insert(new DataQuality_Action_Log
                {
                    Action_Log = "修改了自动评分信息",
                    Action_Log_Detail = entity.SerializeObject(),
                    Action_Time = DateTime.Now,
                    Action_Type = "编辑",
                    DataQuality_Score_Id = entity.Id,
                    Id = CommonHelper.GuidToLongID,
                    InsertTime = DateTime.Now,
                    Mark = 1,
                    Operator_Id = LoginUserinfo.Id,
                    Platform = (int)ActionSource.Admin
                });
                SuccessNotification("修改自动评分信息成功");
                return this.RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Log4Helper.Debug(GetType(), ex);
                ErrorNotification("提交发生异常");
            }
            return View(model);
        }
        #endregion

        #region 查看评分
        [CheckRole(false, false)]
        // GET: ScoringModule
        public ActionResult Detail(long id)
        {
            var model = _scoreService.QueryEntity(id);

            // 删除未保存的参与医院
            _dqhospitalService.DeleteUnSaveDataQualityHospitalByDataQualityScoreId(model.Id);
            return View(model);
        }
        #endregion

        #region 删除评分
        [CheckRole(false, false)]
        // GET: ScoringModule
        public ActionResult Delete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("List");
            }

            _scoreService.Delete(selectedIds);

            string logContent = "【手动】删除评比信息，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);
            return RedirectToAction("List");
        }
        #endregion

        #region 评分记录--列表
        [HttpPost, CheckRole(false, false)]
        public JsonResult RecordToList(long id, string juryName, string hospital, DateTime? begin, DateTime? end, DataSourceRequest command)
        {
            var list = _scoreRecordService.QueryPage(id, juryName, hospital, begin, end, command.Page - 1, command.PageSize);
            var data = list.Select(item => new
            {
                Id = item.Id.ToString(),
                MarkingTime = item.Evaluate_Time.ToString("yyyy-MM-dd HH:mm"),
                JuryName = _memberService.QueryEntity((_juryService.QueryEntity(item.DataQuality_Jury_Id)?.Jury_Id) ?? 0)?.Name,
                HospitalName = _hospitalService.QueryEntity((_dqhospitalService.QueryEntity(item.DataQuality_Hospital_Id)?.Hospital_Id) ?? 0)?.Name,
                TotalScore = item.Evaluate_Score.ToString("F1"),
                Comment = item.Evaluate_Remark
            });
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 评分详情
        [CheckRole(false, false)]
        public ActionResult RecoreDetail(long id)
        {
            DataQuality_Score_Record recordModel = _scoreRecordService.QueryEntity(id);

            ViewData["MarkingTime"] = recordModel.Evaluate_Time.ToString("yyyy-MM-dd HH:mm");
            ViewData["JuryName"] = _memberService.QueryEntity((_juryService.QueryEntity(recordModel.DataQuality_Jury_Id)?.Jury_Id) ?? 0)?.Name;
            ViewData["HospitalName"] = _hospitalService.QueryEntity((_dqhospitalService.QueryEntity(recordModel.DataQuality_Hospital_Id)?.Hospital_Id) ?? 0)?.Name;
            ViewData["TotalScore"] = recordModel.Evaluate_Score.ToString("F1");
            ViewData["Comment"] = recordModel.Evaluate_Remark;

            List<DataQuality_Score_Detail> list = _scoreDetailService.Query(r => r.DataQuality_Score_Record_Id == id && r.Mark > 0);
            return View(list);
        }
        #endregion

        #region 操作日志--列表
        [HttpPost, CheckRole(false, false)]
        public JsonResult ActionLogToList(long id, string userName, string actionType, string content, DateTime? begin, DateTime? end, DataSourceRequest command)
        {
            var list = _actionLogService.QueryPage(id, userName, actionType, content, begin, end, command.Page - 1, command.PageSize);
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                string _userName = "";
                if (item.Platform == (int)ActionSource.Admin)
                {
                    _userName = _userinfoService.QueryEntity(item.Operator_Id)?.Name ?? "";
                }
                else if (item.Platform == (int)ActionSource.Mobile)
                {
                    _userName = _memberService.QueryEntity(item.Operator_Id)?.Name ?? "";
                }
                data.Add(new
                {
                    ActionTime = item.Action_Time.ToString("yyyy-MM-dd HH:mm"),
                    UserName = _userName,
                    ActionType = item.Action_Type,
                    ActionLog = item.Action_Log
                });
            }
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 重评申请
        [CheckRole(false, false)]
        // GET: ScoringModule
        public ActionResult Reevaluation()
        {
            ViewData["DropSatusEnum"] = new List<SelectListItem>
            {
                new SelectListItem { Text = "全部", Value = "null" },
                new SelectListItem { Text = "待处理", Value = "待处理" },
                new SelectListItem { Text = "同意", Value = "同意" },
                new SelectListItem { Text = "取消", Value = "取消" }
            };
            return View();
        }

        [HttpPost, CheckRole(false, false)]
        public JsonResult Reevaluation(string title, string status, string juryName, string hospital, DataSourceRequest command)
        {
            var list = _reevaluationApplyService.QueryPage(title, status, juryName, hospital, command.Page - 1, command.PageSize);
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var member_id = (_juryService.QueryEntity(item.DataQuality_Jury_Id)?.Jury_Id) ?? 0;
                var member = _memberService.QueryEntity(member_id);
                var member_name = member?.Name;
                member_name = !string.IsNullOrEmpty(member_name) ? member_name : member?.NickName;
                member_name = !string.IsNullOrEmpty(member_name) ? member_name : member?.Phone;

                var hospital_id = (_dqhospitalService.QueryEntity(item.DataQuality_Hospital_Id)?.Hospital_Id) ?? 0;
                var hospitalEntity = _hospitalService.QueryEntity(hospital_id);
                data.Add(new
                {
                    Id = item.Id.ToString(),
                    InsertTime = item.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                    Title = _scoreService.QueryEntity(item.DataQuality_Score_Id)?.Title,
                    Status = item.Status == 1 ? "待处理" : (item.Status == 2 ? "同意" : (item.Status == 3 ? "取消" : "-")),
                    JuryName = member_name,
                    HospitalName = hospitalEntity?.Name,
                    EvaluateScore = item.Evaluate_Score,
                    item.Reason,
                    Handler = _userinfoService.QueryEntity(item.Handler_Id ?? 0)?.Name,
                    More = new
                    {
                        Id = item.Id.ToString(),
                        ShowAgree = item.Status == 1,
                        ShowCancel = item.Status == 1
                    }
                });
            }
            var gridModel = new DataSourceResult
            {
                Data = data,
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }

        [HttpPost, CheckRole(false, false)]
        public ActionResult OnChangeReevaluationStatus(long id, int status, string remark)
        {
            var entity = _reevaluationApplyService.QueryEntity(id);
            entity.Status = status;
            entity.UpdateTime = DateTime.Now;
            entity.Handler_Id = LoginUserinfo.Id;
            entity.Handle_Time = DateTime.Now;
            entity.Handle_Remark = remark;

            _reevaluationApplyService.Update(entity);
            ActionLogService.Insert(ActionType.Edit, ActionSource.Admin, LoginUserinfo.Id, string.IsNullOrWhiteSpace(LoginUserinfo.Name) ? LoginUserinfo.LoginId : LoginUserinfo.Name, $"重评申请状态变更为：{(status == 2 ? "同意" : (status == 3 ? "取消" : ""))}", entity.SerializeObject());
            SuccessNotification("重评申请状态变更成功！");
            return RedirectToAction("Reevaluation");
        }
        #endregion

        #region 重评查看
        public ActionResult ReevaluationDetail(long id)
        {
            var entity = _reevaluationApplyService.QueryEntity(id);

            var memberEntity = _memberService.QueryEntity((_juryService.QueryEntity(entity.DataQuality_Jury_Id)?.Jury_Id) ?? 0);
            string memberName = !string.IsNullOrWhiteSpace(memberEntity?.Name) ? memberEntity?.Name : memberEntity?.NickName;
            memberName = !string.IsNullOrWhiteSpace(memberName) ? memberName : memberEntity?.Phone;

            var hospitalEntity = _hospitalService.QueryEntity((_dqhospitalService.QueryEntity(entity.DataQuality_Hospital_Id)?.Hospital_Id) ?? 0);

            ViewData["Title"] = _scoreService.QueryEntity(entity.DataQuality_Score_Id)?.Title;
            ViewData["JuryName"] = memberName;
            ViewData["Hospital"] = hospitalEntity?.Name;
            ViewData["Status"] = entity.Status == 1 ? "待处理" : entity.Status == 2 ? "同意" : entity.Status == 3 ? "取消" : "";
            ViewData["Score"] = entity.Evaluate_Score;
            ViewData["Reason"] = entity.Reason;
            ViewData["Handler"] = _userinfoService.QueryEntity(entity.Handler_Id ?? 0)?.Name;
            ViewData["HandleTime"] = entity.Handle_Time?.ToString("yyyy-MM-dd HH:mm:ss");
            ViewData["HandleRemark"] = entity.Handle_Remark;
            return View();
        }
        #endregion

        #region 删除重评申请
        [HttpPost, CheckRole(false, false)]
        public ActionResult ReevaluationDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("Reevaluation");
            }

            _reevaluationApplyService.Delete(selectedIds);

            string logContent = "【手动】删除重评申请，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);
            return RedirectToAction("Reevaluation");
        }
        #endregion

        #region 发送通知
        /// <summary>
        /// 批量发送邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost, CheckRole(false, false)]
        public ActionResult SetMessage(long Id)
        {
            int emailcount = 0;
            int phonecount = 0;
            var scoringModule = this._scoreService.QueryEntity(Id);

            var ids = this._juryService.GetJuryIdByDataQualityScoreId(scoringModule.Id);
            var openurl = $"{ConfigHelper.GetConfigString("m.chinet.com").Trim('/')}/ScoringModule/HospitalList/{scoringModule.Id}".ToLowerInvariant();
            string email_template_path = Server.MapPath("/Content/Files/scoringmodule_email_notify_template.html");
            string email_template_content = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(email_template_path))
            {
                email_template_content = sr.ReadToEnd();
            }
            email_template_content = email_template_content
                .Replace("$scoringmodule_title$", scoringModule.Title)
                .Replace("$scoringmodule_link$", openurl);

            foreach (var id in ids)
            {
                Member entity = this._memberService.QueryEntity(id);

                if (entity.Email != "" && entity.Email != null)
                {
                    List<string> hospitalNames = _dqhospitalService.GetHospitalNameByJuryIdWithDataQualityScore(scoringModule.Id, entity.Id);
                    string html = email_template_content;
                    html = html.Replace("$scoringmodule_juryName$", entity.Name);
                    StringBuilder hospitalListBuilder = new StringBuilder();
                    int hospitalIndex = 1;
                    foreach (var hospitalName in hospitalNames)
                    {
                        hospitalListBuilder.Append($"&ensp;{hospitalIndex++}.&ensp;{hospitalName}；<br />");
                    }

                    #region 邮件发送
                    html = html.Replace("$scoringmodule_hospital_list$", System.Text.RegularExpressions.Regex.Replace(hospitalListBuilder.ToString(), "(<br />)$", ""));

                    if (entity != null && entity.Id > 0 && entity.Mark > 0)
                    {
                        MessageEmail email = new MessageEmail()
                        {
                            Id = CommonHelper.GuidToLongID,
                            Content = html,
                            Email = entity.Email,
                            MemberId = entity.Id,
                            MemberName = entity.Name,
                            Remark = "",
                            SceneType = "",
                            SendType = 1,
                            Source = "web",
                            Status = 1,//状态：1、待发送   2：已发送   3：失败
                            Title = scoringModule.Title + "活动通知"
                        };

                        try
                        {
                            //发送邮件
                            string serviceEmail = ConfigHelper.GetConfigString("message.email.email");
                            string servicePassword = ConfigHelper.GetConfigString("message.email.password");
                            int servicePort = ConfigHelper.GetConfigString("message.email.port").GetInt();
                            bool serviceSSL = ConfigHelper.GetConfigString("message.email.ssl").ToBoolean();
                            string serviceSMTP = ConfigHelper.GetConfigString("message.email.smtp");
                            string displayname = ConfigHelper.GetConfigString("message.email.displayname");

                            bool result = new EmailHelper(serviceEmail, servicePassword, serviceSMTP, displayname, servicePort, serviceSSL).WebMailSend(new string[] { entity.Email }, email.Title, html);

                            if (result)
                            {
                                emailcount++;
                                email.Status = 2;
                            }
                            else
                            {
                                email.Status = 3;
                            }

                            var user = this.LoginUserinfo;
                        }
                        catch (Exception ex)
                        {
                            email.Remark = "发送邮件发生异常，异常信息：" + ex.Message + ",发送邮件成功" + emailcount + "条，失败" + (ids.Count() - emailcount) + "条";
                            return this.Content(JsonHelper.GetBaseMessage(false, "发送邮件失败"));
                        }

                        //保存发送邮件记录
                        EngineContext.Current.Resolve<IMessageEmailService>().Insert(email);

                    }
                    #endregion
                }

                if (entity.Phone != "" && entity.Phone != null)
                {
                    #region 短信发送
                    //将短信内容写入到数据库
                    ValidateCode model = new ValidateCode();
                    model.Id = CommonHelper.GuidToLongID;
                    model.Source = (int)ValidateCodeSource.PC;
                    model.Type = (int)ValidateCodeType.FindPasswordPhone;
                    model.Value = entity.Phone;
                    model.Describe = "评比项目通知，手机号码：" + entity.Phone;
                    model.StartTime = DateTime.Now;
                    try
                    {
                        string key = WebSettingService.GetWebSMS();

                        //【CHINET】您的验证码是#code#。如非本人操作，请忽略本短信
                        string tpl_value = HttpUtility.UrlEncode("#name#=" + entity.Name + "&#project#=" + scoringModule.Title + "&#url#=" + openurl);
                        string para = "&mobile=" + entity.Phone + "&tpl_id=210472&tpl_value=" + tpl_value;
                        string url = "http://v.juhe.cn/sms/send?key=" + key + "&dtype=json" + para;

                        System.Net.WebClient wc = new System.Net.WebClient();
                        byte[] b = wc.DownloadData(url);
                        string s1 = Encoding.GetEncoding("utf-8").GetString(b);

                        SmsApiResult result = s1.DeserializeObject<SmsApiResult>();

                        if (result.error_code != 0)
                        {
                            model.Describe = "发送短信发生异常，发送短信成功" + phonecount + "条，失败" + (ids.Count() - phonecount) + "条";
                            return this.Content(JsonHelper.GetBaseMessage(false, "发送短信失败"));
                        }
                        else
                        {
                            phonecount++;
                        }
                        model.Describe += $"；短信发送结果：{result.SerializeObject()}";
                    }
                    catch (Exception ex)
                    {
                        model.Describe = "发送短信发生异常，异常信息：" + ex.Message + ",发送短信成功" + phonecount + "条，失败" + (ids.Count() - phonecount) + "条";
                        return this.Content(JsonHelper.GetBaseMessage(false, "发送短信失败"));
                    }
                    model.OutTime = DateTime.Now.AddMinutes(10);
                    this._validateCodeService.Insert(model);

                    #endregion
                }
            }

            AutoDataQuality_Hospital_PageInit(scoringModule.Id);

            return this.Content(JsonHelper.GetBaseMessage(true, "消息通知成功,发送邮件成功" + emailcount + "条，失败" + (ids.Count() - emailcount) + "条；发送短信成功" + phonecount + "条，失败" + (ids.Count() - phonecount) + "条。"));
        }

        private void AutoDataQuality_Hospital_PageInit(long id)
        {
            try
            {
                string requestUri = $"{ConfigHelper.GetConfigString("innerapi.chinets.com").TrimEnd('/')}/v1/scoremd/page_init";
                Task.Run(() =>
                {
                    httpClient.GetAsync(requestUri);
                });
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}