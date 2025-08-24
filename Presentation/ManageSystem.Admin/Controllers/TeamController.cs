using ManageSystem.Admin.Models.Team;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Teams;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Extensions;

namespace ManageSystem.Admin.Controllers
{
    public class TeamController : AdminBaseController
    {
        #region 业务声明
        private readonly ITeamCREContentService _teamCREContentService;

        private readonly ITeamService _teamService;

        private readonly IHospitalService HospitalService;
        #endregion

        #region 构造器
        public TeamController(ITeamCREContentService teamCREContentService, ITeamService teamService, IHospitalService _hospitalService)
        {
            _teamCREContentService = teamCREContentService;
            _teamService = teamService;
            this.HospitalService = _hospitalService;
        }
        #endregion

        #region 不用方法但不可删除
        // GET: Team
        public ActionResult CREContent()
        {
            Team_CREContent contentEntity = _teamCREContentService.Query(r => r.Mark > 0).OrderByDescending(r => r.InsertTime).FirstOrDefault() ?? new Team_CREContent() { Id = CommonHelper.GuidToLongID };
            return View(contentEntity);
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken()]
        public ActionResult CREContent(Team_CREContent model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Content))
                {
                    base.ErrorNotification("请完善CRE成员单位信息");
                    return View(model);
                }

                Team_CREContent contentEntity = _teamCREContentService.QueryEntity(model.Id) ?? new Team_CREContent();
                if (contentEntity.Id <= 0)
                {
                    // 新增
                    contentEntity = new Team_CREContent
                    {
                        Content = model.Content,
                        Describe = null,
                        Id = CommonHelper.GuidToLongID,
                        InsertTime = DateTime.Now,
                        Mark = 1,
                        Version = 1
                    };
                    _teamCREContentService.Insert(contentEntity);
                }
                else
                {
                    // 编辑
                    contentEntity.Content = model.Content;
                    _teamCREContentService.Update(contentEntity);
                }

                base.SuccessNotification("编辑CRE单位成员提交成功");
                base.InsetActionLog(ActionType.Edit, "编辑CRE单位成员提交成功", contentEntity.SerializeObject());
                return View(contentEntity);
            }
            catch (Exception)
            {
                base.ErrorNotification("提交出现异常");
                return View(model);
            }
        }
        #endregion

        public ActionResult TeamList()
        {
            TeamModel model = new TeamModel();
            model.classifyList = this._teamService.GetTeamclassifies().Select(x => { return new SelectListItem() { Text = x.classifyname, Value = x.Id.ToString() }; }).ToList();
            model.classifyList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "" });
            return View(model);
        }

        [HttpPost]
        public JsonResult TeamList(DataSourceRequest command, Team model)
        {
            IPagedList<Team> list = _teamService.QueryPage(model.Title, command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    select new
                                    {
                                        Id = x.Id,
                                        hospital_id = x.Hospitalid,
                                        title = x.Title,
                                        People=x.People,
                                        image = x.Images,
                                        Displayimages = x.Displayimages ? "是" : "否",
                                        classifyId = x.classifyId,
                                        Hospital = this.HospitalService.QueryEntity(x.Hospitalid)?.Name,
                                        InsertTime = x.InsertTime.ToString(),
                                        username = _teamService.GetUsers(x.Createby_Id),
                                        project_code=x.project_code
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };

        }

        public TeamModel SetTeamCreateData()
        {
            TeamModel model = new TeamModel();
            model.classifyList = this._teamService.GetTeamclassifies().Select(x => { return new SelectListItem() { Text = x.classifyname, Value = x.Id.ToString() }; }).ToList();
            model.classifyList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "111" });

            model.hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });

            return model;
        }

        public ActionResult TeamCreate()
        {
           
            return View(this.SetTeamCreateData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeamCreate(TeamModel Team)
        {
            try
            {           
                if (Team.PostFile == null || Team.PostFile.ContentLength == 0)
                {
                    var entity = new Team
                    {
                        InsertTime = DateTime.Now,
                        Title = Team.Title,
                        People = Team.People,
                        Hospitalid = Team.Hospitalid,
                        classifyId = Team.classifyId,
                        Images = Team.Images,
                        Displayimages = Team.Displayimages,
                        Createby_Id = LoginUserinfo.Id,
                        project_code=Team.project_code
                    };
                    if (_teamService.AddTeam(entity) > 0)
                    {
                        string logContent = "新增员单位成功，成员单位名称： " + entity.Title;
                        base.InsetActionLog(ActionType.Create, logContent);
                        base.SuccessNotification(logContent);
                        return this.RedirectToAction("TeamList");

                    }
                    else
                    {
                        string logContent = "操作失败" + entity.Title;
                        base.SuccessNotification(logContent);
                        return this.RedirectToAction("TeamCreate");
                    }
                }
                else
                {
                    string result = null;
                    if (Team.PostFile == null || Team.PostFile.ContentLength <= 0)
                    {
                        ModelState.AddModelError("PostFile", "未能获取到文件信息");
                        return null;
                    }

                    string path = "/Content/File/team";
                    UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(Team.PostFile, path, 51200, UpLoadType.Unlimited, System.IO.Path.GetFileNameWithoutExtension(Team.PostFile.FileName));
                    if (uploadResult == null || !uploadResult.State)
                    {
                        ModelState.AddModelError("PostFile", uploadResult.ErrorMessage);
                        return null;
                    }
                    else
                    {
                        result = path + "/" + uploadResult.Name;
                    }
                    Team.Images = result;
                    var entity = new Team
                    {
                        InsertTime = DateTime.Now,
                        Title = Team.Title,
                        People=Team.People,
                        Hospitalid = Team.Hospitalid,
                        classifyId = Team.classifyId,
                        Images = Team.Images,
                        Displayimages = Team.Displayimages,
                        Createby_Id = LoginUserinfo.Id,
                        project_code = Team.project_code
                    };
                    if (_teamService.AddTeam(entity) > 0)
                    {
                        string logContent = "新增员单位成功，成员单位名称： " + entity.Title;
                        base.InsetActionLog(ActionType.Create, logContent);
                        base.SuccessNotification(logContent);
                        return this.RedirectToAction("TeamList");
                    }
                    else
                    {
                        string logContent = "操作失败" + entity.Title;
                        base.SuccessNotification(logContent);
                        return this.RedirectToAction("TeamCreate");

                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
            /// <summary>
            /// 删除成员单位
            /// </summary>
            /// <param name="selectedIds"></param>
            /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeam(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("TeamList");
            }

            this._teamService.deleteTeam(selectedIds);

            string logContent = "【手动】删除成员单位，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("TeamList");

        }

        /// <summary>
        /// 删除成员单位
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSHTeam(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("TeamList");
            }

            this._teamService.deleteTeam(selectedIds);

            string logContent = "【手动】删除医学数据，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SHTeamList");

        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult TeamEdit(long id)
        {
            var model = this.SetTeamEditData(id);
          
            return View(model);
        }

        private TeamModel SetTeamEditData(long id)
        {
            var entity = _teamService.GetTeamsbyid(id);
            var model = entity.ToModel();
            model.classifyList = this._teamService.GetTeamclassifies().Select(x => { return new SelectListItem() { Text = x.classifyname, Value = x.Id.ToString() }; }).ToList();
            model.classifyList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "" });

            model.hospitalList = this.HospitalService.Query(m => m.Mark > 0 && m.State).OrderBy(m => m.Sort).Select(x => { return new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }; }).ToList();
            model.hospitalList.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            model.classifyId = entity.classifyId;
            model.Images = entity.Images;
            model.FilePath = entity.Images;
            model.Hospitalid = entity.Hospitalid;
            model.Title = entity.Title;
            model.Displayimages = entity.Displayimages;
            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeamEdit(TeamModel Team)
        {
            try
            {
                if (Team.PostFile == null || Team.PostFile.ContentLength == 0)
                {
                    var entity = new Team
                    {
                        Id = Team.Id,
                        InsertTime = DateTime.Now,
                        Title = Team.Title,
                        People = Team.People,
                        Hospitalid = Team.Hospitalid,
                        classifyId = Team.classifyId,
                        Images = Team.Images,
                        Displayimages = Team.Displayimages,
                        Createby_Id = LoginUserinfo.Id,
                        UpdateTime = DateTime.Now,
                        project_code=Team.project_code
                    };
                    if (_teamService.UpdateTeam(entity) > 0)
                    {
                        string logContent = "修改员单位成功，成员单位名称： " + entity.Title;
                        base.InsetActionLog(ActionType.Create, logContent);
                        base.SuccessNotification(logContent);
                        return this.RedirectToAction("TeamList");
                    }
                    else
                    {
                        string logContent = "操作失败" + entity.Title;
                        base.SuccessNotification(logContent);
                    }
                }
                else
                {
                    string result = null;
                    if (Team.PostFile == null || Team.PostFile.ContentLength <= 0)
                    {
                        ModelState.AddModelError("PostFile", "未能获取到文件信息");
                        return null;
                    }

                    string path = "/Content/File/team";
                    UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(Team.PostFile, path, 51200, UpLoadType.Unlimited, System.IO.Path.GetFileNameWithoutExtension(Team.PostFile.FileName));
                    if (uploadResult == null || !uploadResult.State)
                    {
                        ModelState.AddModelError("PostFile", uploadResult.ErrorMessage);
                        return null;
                    }
                    else
                    {
                        result = path + "/" + uploadResult.Name;
                    }
                    Team.Images = result;
                    var entity = new Team
                    {
                        Id = Team.Id,
                        InsertTime = DateTime.Now,
                        Title = Team.Title,
                        People = Team.People,
                        Hospitalid = Team.Hospitalid,
                        classifyId = Team.classifyId,
                        Images = Team.Images,
                        Displayimages = Team.Displayimages,
                        Createby_Id = LoginUserinfo.Id,
                        UpdateTime=DateTime.Now,
                        project_code = Team.project_code
                    };
                    if (_teamService.UpdateTeam(entity) > 0)
                    {
                        string logContent = "修改员单位成功，成员单位名称： " + entity.Title;
                        base.InsetActionLog(ActionType.Create, logContent);
                        base.SuccessNotification(logContent);
                        return this.RedirectToAction("TeamList");
                    }
                    else
                    {
                        string logContent = "操作失败" + entity.Title;
                        base.SuccessNotification(logContent);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(this.SetTeamEditData(Team.Id));
        }

        /// <summary>
        /// 根据id查询医院
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost,CheckRole(CheckRole = false)]
        public JsonResult Hospital(string Id)
        {
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = this.HospitalService.QueryEntity(long.Parse(Id))?.Name;
                                    
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };

        }

        /// <summary>
        /// 上海网成员单位列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SHTeamList()
        {
            TeamModel model = new TeamModel();
            model.classifyList = this._teamService.GetTeamclassifies().Select(x => { return new SelectListItem() { Text = x.classifyname, Value = x.Id.ToString() }; }).ToList();
            model.classifyList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "" });
            return View(model);
        }

        /// <summary>
        /// 上海网成员单位列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SHTeamList(DataSourceRequest command, Team model)
        {
            IPagedList<Team> list = _teamService.SHQueryPage(model.Title, command.Page - 1, command.PageSize);
            DataSourceResult dataSourceResult = new DataSourceResult();
            dataSourceResult.Data = from x in list
                                    select new
                                    {
                                        Id = x.Id,
                                        hospital_id = x.Hospitalid,
                                        title = x.Title,
                                        People = x.People,
                                        image = x.Images,
                                        Displayimages = x.Displayimages ? "是" : "否",
                                        classifyId = x.classifyId,
                                        Hospital = this.HospitalService.QueryEntity(x.Hospitalid)?.Name,
                                        InsertTime = x.InsertTime.ToString(),
                                        username = _teamService.GetUsers(x.Createby_Id),
                                        project_code = x.project_code
                                    };
            dataSourceResult.Total = list.TotalCount;
            DataSourceResult gridModel = dataSourceResult;
            return new JsonResult
            {
                Data = gridModel
            };

        }

    }
}