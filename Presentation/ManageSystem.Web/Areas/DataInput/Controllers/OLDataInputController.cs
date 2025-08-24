using ManageSystem.Services.DataInput;
using ManageSystem.Web.Controllers;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.DataInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Newtonsoft.Json;
using ManageSystem.Core.Domain.DataInput;
using ManageSystem.Web.Models.Members;
using ManageSystem.Services.Medicine;
using System.Data;
using ManageSystem.Core.Domain.Log;
using OfficeOpenXml;
using System.Web;
using System.IO;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Core;

namespace ManageSystem.Web.Areas.DataInput.Controllers
{
    public class OLDataInputController : WebBaseController
    {
        public readonly IOLDataInputService OLDataInputService;
        public readonly IOLFieldmodelService OLFieldmodelService;
        public readonly IOLProjectService OLProjectService;
        public readonly IOLTemplateService OLTemplateService;
        public readonly IOLMemberService memberService;
        private readonly IHospitalService HospitalService;

        public OLDataInputController(IOLDataInputService _OLDataInputService, IOLFieldmodelService _OLFieldmodelService, IOLProjectService _OLProjectService, IOLTemplateService _OLTemplateService, IOLMemberService _memberService, IHospitalService _hospitalService)
        {
            OLDataInputService = _OLDataInputService;
            OLFieldmodelService = _OLFieldmodelService;
            OLProjectService = _OLProjectService;
            OLTemplateService = _OLTemplateService;
            memberService = _memberService;
            this.HospitalService = _hospitalService;
        }
        // GET: DataInput/OLDataInput
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 字段模板列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult OLtemplate(int pageIndex = 1)
        {
            OLTemplateModel model = new OLTemplateModel();
            var member = LoginUserinfo;
            var list = this.OLTemplateService.Querylist();
            model.PageList = list.Select(x => new OLTemplateItemModel()
            {
                Id = x.Id.ToString(),
                template_name = x.template_name,
                InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm:ss"),
                createdname = x.Name,
                field_code = OLFieldmodelService.GetLFieldfield_code(x.Id).field_code
            }).ToPagedList<OLTemplateItemModel>(pageIndex, MvcPagerExtensions.PageSize);
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            return View(model);
        }
        /// <summary>
        /// 字段模板添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateOLtemplate(long? Id)
        {
            var member = base.LoginUserinfo;
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            if (Id == null || Id <= 0)
            {
                ViewBag.title = "折点-添加字段模板";
                ViewBag.temp = true;
            }
            else
            {
                ViewBag.title = "折点-修改字段模板";
                ViewBag.temp = false;
            }
            return View();
        }
        /// <summary>
        /// 添加模板字段
        /// </summary>
        /// <returns></returns>
        public ContentResult Addtemplate(string[] fieldname, string[] fieldcode, string[] fieldvalue, string template_name, string[] sfbt, string[] order_px)
        {
            var member = base.LoginUserinfo;
            OLTemplate model = new OLTemplate();
            model.Id = CommonHelper.GuidToLongID;
            model.template_name = template_name;
            model.created_byId = member.Id;
            try
            {
                int result = OLTemplateService.AddTemplate(model);
                if (result > 0)
                {
                    OLFieldmodel mode = new OLFieldmodel();
                    for (int i = 0; i < fieldname.Length; i++)
                    {
                        mode.Id = CommonHelper.GuidToLongID;
                        mode.templateId = model.Id;
                        mode.created_byId = member.Id;
                        mode.field_name = fieldname[i].ToString();
                        mode.field_code = fieldcode[i].ToString();
                        mode.default_value = fieldvalue[i].ToString();
                        mode.sfbt = sfbt[i].ToString();
                        mode.order_px = order_px[i].ToString();
                        OLFieldmodelService.AddFieldmodel(mode);
                    }
                    base.InsetActionLog(ActionType.Create, "添加字段模板成功,模板名称：" + template_name);
                }
                return this.Content(JsonHelper.GetBaseMessage(true, ""));

            }
            catch (Exception ex)
            {
                OLFieldmodelService.delete(model.Id);
                OLTemplateService.delete(model.Id);
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        /// <summary>
        /// 字段模板返填
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string UpdattemplateList(long Id)
        {
            string result = JsonConvert.SerializeObject(OLFieldmodelService.GetLFieldmodels(Id));
            return result;
        }
        /// <summary>
        /// 修改字段模板
        /// </summary>
        /// <param name="fieldname"></param>
        /// <param name="fieldcode"></param>
        /// <param name="fieldvalue"></param>
        /// <param name="template_name"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public ContentResult Updatetemplate(string[] fieldname, string[] fieldcode, string[] fieldvalue, string template_name, long templateId, string[] sfbt, string[] order_px)
        {
            var member = base.LoginUserinfo;
            OLTemplate model = new OLTemplate();
            model.Id = templateId;
            model.template_name = template_name;
            model.created_byId = member.Id;
            model.Updatetime = DateTime.Now;
            try
            {
                int result = OLTemplateService.updateOLTemplate(model);
                if (result > 0)
                {
                    OLFieldmodelService.delete(templateId);
                    OLFieldmodel mode = new OLFieldmodel();
                    for (int i = 0; i < fieldname.Length; i++)
                    {
                        mode.templateId = templateId;
                        mode.created_byId = member.Id;
                        mode.InsertTime = DateTime.Now;
                        mode.field_name = fieldname[i].ToString();
                        mode.field_code = fieldcode[i].ToString();
                        mode.default_value = fieldvalue[i].ToString();
                        mode.sfbt = sfbt[i].ToString();
                        mode.order_px = order_px[i].ToString();
                        OLFieldmodelService.AddFieldmodel(mode);
                    }
                    base.InsetActionLog(ActionType.Edit, "修改字段模板成功,模板名称：" + template_name);
                }
                return this.Content(JsonHelper.GetBaseMessage(true, ""));

            }
            catch (Exception ex)
            {
                //OLFieldmodelService.delete(model.Id);
                //OLTemplateService.delete(model.Id);
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        /// <summary>
        /// 字段模板删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="template_name"></param>
        /// <returns></returns>
        public ContentResult deletetemplate(string ids, string template_name)
        {
            //int result=  OLProjectService.delete(ids);
            //return result;
            ids = ids.TrimEnd(',');
            string newid = "";
            try
            {
                if (ids == "") return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                string[] id = ids.Split(',');
                string[] template = template_name.Split(',');
                for (int i = 0; i < id.Length; i++)
                {
                    int sum = OLTemplateService.GetLProjects(long.Parse(id[i]));
                    if (sum > 0)
                    {
                        return this.Content(JsonHelper.GetBaseMessage(false, "模板【" + template[i] + "】存在数据不允许删除"));
                    }
                    else
                    {
                        OLFieldmodelService.delete(long.Parse(id[i]));
                        OLTemplateService.delete(long.Parse(id[i]));
                        newid += id[i] + ",";
                    }
                }
                base.InsetActionLog(ActionType.Delete, "【手动】删除OL项目", "数据id集合：" + newid.TrimEnd(','));

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }
        /// <summary>
        /// 项目列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OLproject(int pageIndex = 1)
        {
            OLProjectModel model = new OLProjectModel();
            var user = LoginUserinfo;
            List<OLProject> list = new List<OLProject>();
            if (user.Median.Contains("1"))
            {
                list = OLProjectService.GetOLProjects(0);
            }
            else if (user.Median.Contains("2"))
            {
                list = OLProjectService.GetLProjects(user.Id, 0);
            }
            model.PageList = list.Select(x => new OLProjecItemModel()
            {
                Id = x.Id.ToString(),
                project_name = x.project_name,
                templateId = OLTemplateService.GetLTemplate(x.templateId) == null ? "" : OLTemplateService.GetLTemplate(x.templateId).template_name,
                field_code = OLFieldmodelService.GetLFieldfield_code(x.templateId) == null ? "" : OLFieldmodelService.GetLFieldfield_code(x.templateId).field_code,
                starttime = x.starttime.ToString("yyyy-MM-dd"),
                endtime = x.endtime.ToString("yyyy-MM-dd"),
                InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Name = x.Name,
                state = x.endtime > DateTime.Now ? (x.state == "True" ? "已开启" : "未开启") : "已结束"
            }).ToPagedList<OLProjecItemModel>(pageIndex, MvcPagerExtensions.PageSize);
            if (user.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            return View(model);
        }

        /// <summary>
        /// 绑定项目下拉列表
        /// </summary>
        /// <returns></returns>
        public string OLprojectlist(long projecId)
        {
            var user = LoginUserinfo;
            List<OLProject> list = new List<OLProject>();
            if (!user.Median.Contains("1"))
            {
                list = OLProjectService.GetLProjects(user.Id, projecId);
            }
            else
            {
                list = OLProjectService.GetOLProjects(projecId);
            }

            List<OLProjecItemModel> list1 = new List<OLProjecItemModel>();
            for (int i = 0; i < list.Count; i++)
            {
                OLProjecItemModel model = new OLProjecItemModel();
                model.Id = list[i].Id.ToString();
                model.templateId = list[i].templateId.ToString();
                model.project_name = list[i].project_name;
                list1.Add(model);
            }
            var result = JsonConvert.SerializeObject(list1);
            return result;
        }
        /// <summary>
        /// 根据id删除项目
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ContentResult deleteOLproject(string ids, string project_name)
        {
            ids = ids.TrimEnd(',');
            string newid = "";
            try
            {
                if (ids == "") return this.Content(JsonHelper.GetBaseMessage(false, "数据不能为空"));
                string[] id = ids.Split(',');
                string[] project = project_name.Split(',');
                List<OLDataInput> list = null;
                for (int i = 0; i < id.Length; i++)
                {
                    list = OLProjectService.GetOLDataInput(long.Parse(id[i]));
                    if (list.Count > 0)
                    {
                        return this.Content(JsonHelper.GetBaseMessage(false, "项目【" + project[i] + "】存在数据不允许删除"));
                    }
                    else
                    {
                        OLProjectService.delete(long.Parse(id[i]));
                        newid += id[i] + ",";
                    }
                }
                base.InsetActionLog(ActionType.Delete, "【手动】删除OL项目", "数据id集合：" + newid.TrimEnd(','));

                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        /// <summary>
        /// 用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberList(string LoginId, string Phone, string Hospital, long? Id, int pageIndex = 1)
        {
            var member = base.LoginUserinfo;
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            List<MemberHospital> list = new List<MemberHospital>();
            MemberModel model = new MemberModel();
            if (LoginId == null && Phone == null && Hospital == null || LoginId == "" && Phone == "" && Hospital == "")
            {
                List<MemberHospital> list1 = memberService.Queryuserlist1(Id);
                List<MemberHospital> list2 = memberService.Queryuserlist2(Id);
                list = list1.Concat(list2).ToList();
                model.PageList = list.Select(x => new MemberItemModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    HospitalId = x.HospitalId,
                    LoginId = x.LoginId,
                    Phone = x.Phone,
                    NickName = x.NickName,
                    HospitalName = x.HospitalName,
                    Median = x.Median,
                }).ToPagedList<MemberItemModel>(pageIndex, MvcPagerExtensions.PageSize);
            }
            else
            {
                var lis = this.memberService.Querylist(LoginId, Phone, Hospital).ToList();
                model.PageList = lis.Select(x => new MemberItemModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    HospitalId = x.HospitalId,
                    LoginId = x.LoginId,
                    Phone = x.Phone,
                    NickName = x.NickName,
                    HospitalName = x.HospitalName,
                    Median = x.Median
                }).ToPagedList<MemberItemModel>(pageIndex, MvcPagerExtensions.PageSize);
            }
            List<bool> state = new List<bool>();
            for (int i = 0; i < list.Count; i++)
            {
                if (memberService.GetOLuserproject(Id, list[i].Id) > 0)
                {
                    state.Add(true);
                }
                else
                {
                    state.Add(false);
                }

            }
            ViewBag.strate = state;

            return View(model);
        }
        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ContentResult UpdatProjectMember(long userid, long ProjectId, bool state)
        {
            var user = LoginUserinfo;
            if (state == true)
            {
                OLUserproject model = new OLUserproject();
                model.Id = CommonHelper.GuidToLongID;
                model.userid = userid;
                model.projectid = ProjectId;
                model.created_byId = user.Id;
                try
                {
                    int result = OLProjectService.CreateProjectMember(model);
                    List<string> SelectMedian = new List<string>();
                    if (userid == user.Id)
                    {
                        SelectMedian.Add("1");
                        SelectMedian.Add("2");
                    }
                    else
                    {
                        SelectMedian.Add("2");
                    }
                    string privil = JsonConvert.SerializeObject(SelectMedian);
                    if (OLProjectService.UpdateMember(userid, privil) > 0)
                    {
                        return this.Content(JsonHelper.GetBaseMessage(true, "绑定成功"));
                    }
                    else
                    {
                        return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                    }
                }
                catch (Exception ex)
                {

                    return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                }

            }
            else
            {
                try
                {
                    int result = OLProjectService.CountProjectMember(userid);
                    if (result > 1)
                    {
                        OLProjectService.deleteProjectMember(ProjectId, userid);
                    }
                    else
                    {
                        OLProjectService.deleteProjectMember(ProjectId, userid);
                        if (userid == user.Id && user.Median.Contains("1"))
                        {
                            List<string> SelectMedian = new List<string>();
                            SelectMedian.Add("1");
                            string privil = JsonConvert.SerializeObject(SelectMedian);
                            OLProjectService.UpdateMember(userid, privil);
                        }
                        else
                        {
                            OLProjectService.UpdateMember(userid, "[]");
                        }
                    }
                    return this.Content(JsonHelper.GetBaseMessage(true, "解绑成功"));
                }
                catch (Exception ex)
                {

                    return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                }

            }
        }
        /// <summary>
        /// 项目添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateOLproject(long? Id)
        {
            var member = base.LoginUserinfo;
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            var result = OLProjectService.GetProjecttemData(Id);
            if (result == null || result.templateId <= 0)
            {
                ViewBag.late = false;
            }
            else
            {
                ViewBag.late = true;
            }
            if (Id == null || Id <= 0)
            {
                ViewBag.title = "折点-项目添加";
            }
            else
            {
                ViewBag.title = "折点-项目修改";
            }
            return View();
        }
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ContentResult AddOLproject(string project_name, DateTime starttime, DateTime endtime, string state, long templateId)
        {
            OLProject project = new OLProject();
            var user = LoginUserinfo;
            project.project_name = project_name;
            project.starttime = starttime;
            project.endtime = endtime;
            project.state = state;
            project.templateId = templateId;
            project.Id = CommonHelper.GuidToLongID;
            project.created_byId = user.Id;
            project.InsertTime = DateTime.Now;

            //project.updatetime = DateTime.Now;
            try
            {
                int result = OLProjectService.AddOLProject(project);
                if (result > 0)
                {
                    base.InsetActionLog(ActionType.Create, "添加OL项目成功,项目名称：" + project.project_name);
                }
                return this.Content(JsonHelper.GetBaseMessage(true, ""));

            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }
        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ActionResult UpdateOLproject(long Id, string project_name, DateTime starttime, DateTime endtime, string state, long templateId)
        {
            OLProject project = new OLProject();
            var user = LoginUserinfo;
            project.project_name = project_name;
            project.starttime = starttime;
            project.endtime = endtime;
            project.state = state;
            project.templateId = templateId;
            project.Id = Id;
            project.created_byId = user.Id;
            try
            {
                int result = OLProjectService.updateOLProject(project);
                if (result > 0)
                {
                    base.InsetActionLog(ActionType.Create, "修改OL项目成功,项目名称：" + project.project_name);
                }
                return this.Content(JsonHelper.GetBaseMessage(true, ""));

            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        #region 弃用方法
        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <returns></returns>
        //public ActionResult Exportexcel(string ids)
        //{
        //    try
        //    {
        //        ExcelPackage ep = new ExcelPackage();
        //        //string path = this.OLDataInputService.Export(ids, ep);
        //        string path = this.OLDataInputService.Export1(ids, ep);
        //        if (string.IsNullOrWhiteSpace(path)) return this.Content("导出文件失败");

        //        var user = this.LoginUserinfo;
        //        this.ActionLogService.Insert(ActionType.Export, ActionSource.Admin, user.Id, user.Name, "导出Excel成功", "导出Excel成功");

        //        Response.Clear();
        //        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        //        Response.AddHeader("content-disposition", "attachment;filename=" + path);
        //        Response.ContentType = "application/vnd.open";
        //        ep.SaveAs(Response.OutputStream);
        //        Response.Flush();
        //        Response.End();
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.SystemLogService.Insert(ex, SystemLogLevel.Error);
        //        return this.Content(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 项目添加页面字段模板下拉列表
        /// </summary>
        /// <returns></returns>
        public string GettemplateList()
        {
            var list = OLTemplateService.GetLTemplatelist();
            List<OLTemplateItemModel> list1 = new List<OLTemplateItemModel>();
            for (int i = 0; i < list.Count; i++)
            {
                OLTemplateItemModel model = new OLTemplateItemModel();
                model.Id = list[i].Id.ToString();
                model.template_name = list[i].template_name;
                list1.Add(model);
            }
            var result = JsonConvert.SerializeObject(list1);
            return result;
        }

        /// <summary>
        /// 项目修改返填
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string UPOLProjectsList(long Id)
        {
            var list = OLProjectService.UPOLProjects(Id);
            List<OLProjecItemModel> list1 = new List<OLProjecItemModel>();
            for (int i = 0; i < list.Count; i++)
            {
                OLProjecItemModel model = new OLProjecItemModel();
                model.Id = list[i].Id.ToString();
                model.project_name = list[i].project_name;
                model.templateId = list[i].templateId.ToString();
                model.starttime = list[i].starttime.ToString("yyyy-MM-dd");
                model.endtime = list[i].endtime.ToString("yyyy-MM-dd");
                model.state = list[i].state;
                list1.Add(model);
            }
            var result = JsonConvert.SerializeObject(list1);
            return result;
        }
        /// <summary>
        /// 项目统计页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectStatistics()
        {
            var member = base.LoginUserinfo;
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            return View();
        }
        /// <summary>
        /// 项目统计审核和阅读的数据量
        /// </summary>
        /// <returns></returns>
        public string Projectlist()
        {
            var list1 = OLProjectService.Project();
            //var list2 = OLProjectService.Projectauditor();
            List<OLProjecItemModel> List = new List<OLProjecItemModel>();

            for (int i = 0; i < list1.Count; i++)
            {
                OLProjecItemModel model = new OLProjecItemModel();
                model.Id = list1[i].Id.ToString();
                model.projectsums = list1[i].projectsums;
                model.project_name = list1[i].project_name;
                model.Name = list1[i].Name;
                model.createdId = list1[i].created_byId.ToString();
                if (OLProjectService.Projectauditor(list1[i].Id, list1[i].created_byId).Count == 0)
                {
                    model.auditorsums = "0";
                }
                else
                {
                    model.auditorsums = OLProjectService.Projectauditor(list1[i].Id, list1[i].created_byId).ToList().FirstOrDefault().auditorsums;
                }
                List.Add(model);
            }
            string result = JsonConvert.SerializeObject(List);
            return result;
        }
        /// <summary>
        /// 查询病原菌
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public string Getgerm(long projectid, string createdId)
        {
            string result = JsonConvert.SerializeObject(OLProjectService.Getgermname(projectid, long.Parse(createdId)));
            return result;
        }

        public ContentResult GetProjecttems(long Id)
        {
            var result = OLProjectService.GetProjecttem(Id);
            if (result.state == "False")
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "该项目暂未开启"));
            }
            else
            {
                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
        }
        /// <summary>
        /// 根据id查询项目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ContentResult GetProject(long Id)
        {
            var result = OLProjectService.GetProjecttem(Id);
            if (result.endtime < DateTime.Now)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "该项目已结束"));
            }
            else
            {
                return this.Content(JsonHelper.GetBaseMessage(true, ""));
            }
        }

        /// <summary>
        /// 数据列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult OLDataInput()
        {
            var member = base.LoginUserinfo;
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            return View();
        }
        /// <summary>
        /// 数据列表表头
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public string GetOLDataheade(long projectid)
        {
            string result = JsonConvert.SerializeObject(OLDataInputService.GetOLDataheade(projectid));
            return result;
        }
        /// <summary>
        /// 数据列表页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public JsonResult GetDataList(long projectid, long createdid, string number, string ischeck, int Page, int PageSize)
        {

            var user = LoginUserinfo;
            if (!user.Median.Contains("1"))
            {
                var IntegraIItemList = this.OLDataInputService.QueryPage(projectid, user.Id, number, ischeck, Page - 1, PageSize);
                List<string> list = new List<string>();
                for (int i = 0; i < IntegraIItemList.Count; i++)
                {
                    list.Add(IntegraIItemList[i].Id.ToString());
                }
                return Json(new { total = IntegraIItemList.TotalPages, rows = IntegraIItemList, ids = list });
            }
            else
            {

                var IntegraIItemList = this.OLDataInputService.QueryPagelist(projectid, createdid, number, ischeck, Page - 1, PageSize);

                List<string> list = new List<string>();
                for (int i = 0; i < IntegraIItemList.Count; i++)
                {
                    list.Add(IntegraIItemList[i].Id.ToString());
                }
                return Json(new { total = IntegraIItemList.TotalPages, rows = IntegraIItemList, ids = list });
            }
        }

        /// <summary>
        /// 删除检查
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ContentResult deleteCheck(string Id)
        {
            Id = Id.TrimEnd(',');
            string[] ids = Id.Split(',');

            var userInfo = LoginUserinfo;

            //  OLDataInput oLDataInput = OLDataInputService.QueryList(ids).First(x=>x.created_byId != userInfo.Id);

            if (OLDataInputService.QueryList(ids).Where(x => x.created_byId != userInfo.Id).Count() > 0)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，所选数据中包含非您上传的数据！"));
            }
            else
                return this.Content(JsonHelper.GetBaseMessage(true, ""));
        }

        /// <summary>
        /// 根据id删除项目数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ContentResult deleteData(long Id)
        {
            try
            {
                if (OLDataInputService.delete(Id) > 0)
                {
                    base.InsetActionLog(ActionType.Delete, "【手动】删除折点项目数据成功", "数据id：" + Id);

                    return this.Content(JsonHelper.GetBaseMessage(true, ""));
                }
                else
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                }
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }
        /// <summary>
        /// 根据id删除项目数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ContentResult deleteDatainput(string Id)
        {
            try
            {
                Id = Id.TrimEnd(',');
                string[] ids = Id.Split(',');
                int n = 1;
                for (int i = 0; i < ids.Length; i++)
                {
                    OLDataInputService.delete(long.Parse(ids[i]));
                    n++;
                }
                if (n > 0)
                {
                    base.InsetActionLog(ActionType.Delete, "【手动】删除折点项目数据成功", "数据id：" + Id);

                    return this.Content(JsonHelper.GetBaseMessage(true, ""));
                }
                else
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                }
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }

        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ContentResult Examine(string Id)
        {
            var user = LoginUserinfo;
            OLDataInput model = new OLDataInput();
            model.auditor = user.Id.ToString();
            model.auditortime = DateTime.Now;
            try
            {
                int result = OLDataInputService.AuditorOLDataInput(model, Id);

                if (result > 0)
                {
                    return this.Content(JsonHelper.GetBaseMessage(true, ""));
                }
                else
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                }
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }
        }
        /// <summary>
        /// 数据添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateOLDataInput(long Id)
        {
            Session["experimenttime"] = "";
            var experimenttime = Session["experimenttime"].ToString();
            if (experimenttime == "")
            {
                ViewBag.experimenttime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.experimenttime = experimenttime;
            }

            var member = base.LoginUserinfo;
            var data = OLDataInputService.GetMaxDatalist(Id, LoginUserinfo.Id);
            if (data == null)
            {
                int i = 1;
                var Code = HospitalService.QueryEntity(m => m.Id == member.HospitalId).Code;
                ViewBag.jobnumber = Code + i.ToString().PadLeft(4, '0').ToString();
            }
            else
            {
                var jobnumber = data.jobnumber.Split('-');
                var num = int.Parse(jobnumber[1]) + 1;
                if (num < 9)
                {
                    ViewBag.jobnumber = jobnumber[0] + "-" + num.ToString().PadLeft(4, '0').ToString();
                }
                else if (num > 9 && num < 100)
                {
                    ViewBag.jobnumber = jobnumber[0] + "-" + num.ToString().PadLeft(3, '0').ToString();
                }
                else if (num > 100 && num < 1000)
                {
                    ViewBag.jobnumber = jobnumber[0] + "-" + num.ToString().PadLeft(2, '0').ToString();
                }
                else
                {
                    ViewBag.jobnumber = jobnumber[0] + "-" + num;
                }
            }
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            return View();
        }

        /// <summary>
        /// 数据编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateOLDataInput()
        {
            var member = base.LoginUserinfo;
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            return View();
        }

        /// <summary>
        /// 数据添加页面表单
        /// </summary>
        /// <returns></returns>
        public string AddDataview(long Id)
        {
            var result = JsonConvert.SerializeObject(OLFieldmodelService.GetLFieldmodellist(Id));
            return result;
        }

        /// <summary>
        /// 单条数据编辑返填
        /// </summary>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public string updateOLDataheade(string Id)
        {
            var result = JsonConvert.SerializeObject(OLDataInputService.updateOLDataheade(long.Parse(Id)));
            return result;
        }
        /// <summary>
        /// 配置编辑列表
        /// </summary>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public string GetOLDataheadelist(string Id)
        {
            var result = JsonConvert.SerializeObject(OLDataInputService.GetOLDataheadelist(long.Parse(Id)));
            return result;
        }
        /// <summary>
        /// 根添加项目数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ContentResult CreateData(long projectid, string jobnumber, string germnumber, string germname, string experimenttime, string datevalue)
        {

            var user = LoginUserinfo;
            OLDataInput model = new OLDataInput();

            model.created_byId = user.Id;
            model.experimenter = user.Name;
            model.experimenttime = experimenttime;
            model.jobnumber = jobnumber;
            model.germnumber = germnumber;
            model.germname = germname;
            model.datevalue = datevalue;
            model.auditor = "";
            model.projectid = projectid;
            model.Id = CommonHelper.GuidToLongID;
            model.templateId = OLProjectService.UPOLProjects(projectid).FirstOrDefault().templateId;
            Session["experimenttime"] = experimenttime;
            try
            {
                if (OLDataInputService.AddOLDataInput(model) > 0)
                {

                    return this.Content(JsonHelper.GetBaseMessage(true, ""));
                }
                else
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                }
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }

        }

        /// <summary>
        /// 根据项目id查询最后这个项目最后插入的一条数据
        /// </summary>
        /// <returns></returns>
        public string GetMaxDatalist(long Id)
        {
            var data = OLDataInputService.GetMaxDatalist(Id, LoginUserinfo.Id);
            var result = JsonConvert.SerializeObject(data);
            return result;
        }

        /// <summary>
        /// 单条数据编辑
        /// </summary>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public ContentResult updateOLData(long Id, string jobnumber, string germnumber, string germname, string experimenttime, string datevalue)
        {
            var user = LoginUserinfo;
            OLDataInput model = new OLDataInput();

            model.created_byId = user.Id;
            model.experimenter = user.Name;
            model.experimenttime = experimenttime;
            model.jobnumber = jobnumber;
            model.germnumber = germnumber;
            model.germname = germname;
            model.datevalue = datevalue;
            model.Id = Id;
            try
            {
                if (OLDataInputService.updateOLDataInput(model) > 0)
                {

                    return this.Content(JsonHelper.GetBaseMessage(true, ""));
                }
                else
                {
                    return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                }
            }
            catch (Exception ex)
            {
                return this.Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
            }

        }



        /// <summary>
        /// 导出dbf文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DownloadNewData(string id)
        {
            try
            {
                string filePath = OLDataInputService.GetDBF(long.Parse(id));
                string path = this.Server.MapPath(filePath);
                //string ex = Path.GetExtension(path);
                //string name = "data-adjust-" + id + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + ex;
                string name = Path.GetFileName(filePath);
                return File(path, "application/octet-stream", Url.Encode(name));

            }
            catch (Exception ex)
            {
                base.ErrorNotification(ex);

                return this.Content(JsonHelper.GetBaseMessage(false, ex.Message));
                //return this.RedirectToAction("DownloadNewData", new { id = id.ToString() });
            }
        }
        /// <summary>
        /// 批量上传页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreatebatchData()
        {
            var member = base.LoginUserinfo;
            if (member.Median.Contains("1"))
            {
                ViewBag.Median = true;
            }
            else
            {
                ViewBag.Median = false;
            }
            return View();
        }
        /// <summary>
        /// 批量上传查询 字段模板
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetTemplate(long? Id)
        {

            var Project = OLProjectService.GetProjecttem(Id);
            var result = "";
            if (Project != null)
            {
                var Template = OLTemplateService.GetLTemplate(Project.templateId);
                var data = new
                {
                    Id = Template.Id.ToString(),
                    Name = Template.template_name
                };
                result = JsonConvert.SerializeObject(data);
            }
            return result;

        }

        /// <summary>
        /// 批量上传插入数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="templateId"></param>
        /// <param name="PostedFile"></param>
        /// <returns></returns>
        public ContentResult AddbatchData(long projectId, long templateId, HttpPostedFileBase PostFile)
        {

            var member = base.LoginUserinfo;
            if (PostFile != null && PostFile.ContentLength > 0)
            {
                //上传文件
                string path = "/Areas/DataInput/Content/File";
                var Hospital = HospitalService.QueryEntity(member.HospitalId);
                UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(PostFile, path, 0, UpLoadType.Other1, Hospital.Name + "_" + PostFile.FileName);
                try
                {
                    DataTable table = null;
                    switch (Path.GetExtension(PostFile.FileName))
                    {
                        case "xls":
                        case ".xls":
                            table = ImportDataTable.ExcelToDataTable(PostFile.InputStream, ExcelEnum.Excel2003, true);
                            break;
                        case "xlsx":
                        case ".xlsx":
                            ExcelPackage excelPackage = new ExcelPackage(PostFile.InputStream);
                            if (excelPackage == null || excelPackage.Workbook == null || excelPackage.Workbook.Worksheets == null || excelPackage.Workbook.Worksheets.Count == 0)
                            {
                                return Content(JsonHelper.GetBaseMessage(false, "未能读取数据文件"));
                            }
                            table = EPPlusHelper.WorksheetToTable(worksheet: excelPackage.Workbook.Worksheets[1]);
                            break;
                        default:
                            return Content(JsonHelper.GetBaseMessage(false, "上传文件格式不支持。"));
                    }
                    var OLFieldmodel = OLFieldmodelService.GetLFieldmodellist(projectId);
                    List<string> OLField = new List<string>();
                    List<string> Columns = new List<string>();
                    foreach (var item in OLFieldmodel)
                    {
                        OLField.Add(item.field_name);
                    }
                    for (int n = 5; n < table.Columns.Count; n++)
                    {
                        Columns.Add(table.Columns[n].ColumnName);
                    }
                    if (!Columns.All(OLField.Contains) && Columns.Count == OLField.Count)
                    {
                        return Content(JsonHelper.GetBaseMessage(false, "上传模板字段与项目绑定模板字段不一致，请核后重新上传！"));
                    }
                    List<OLDataInput> oLDatas = new List<OLDataInput>();
                    var rulen = "0.015,0.03,0.06,0.125,0.25,0.5,1,2,4,8,16,32,64,128,256,<=0.015,<=0.03,<=0.06,<=0.125,<=0.25,<=0.5,<=1,<=2,<=4,<=8,<=16,<=32,<=64,<=128,<=256,>0.015,>0.03,>0.06,>0.125,>0.25,>0.5,>1,>2,>4,>8,>16,>32,>64,>128,>256";
                    int i = 0;
                    foreach (DataRow dr in table.Rows)
                    {
                        OLDataInput oLData = new OLDataInput();
                        i++;
                        oLData.Id = CommonHelper.GuidToLongID;
                        oLData.projectid = projectId;
                        oLData.templateId = templateId;
                        oLData.experimenttime = DateTime.Now.ToString("yyyy-MM-dd");
                        oLData.experimenter = LoginUserinfo.Name;
                        oLData.jobnumber = dr["工作编号"].ToString();
                        if (string.IsNullOrEmpty(oLData.jobnumber))
                        {
                            return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行工作编号为空，请按标准格式字母加数字自增长编写，如【HSH-0001】"));
                        }
                        if (!oLData.jobnumber.Contains("-"))
                        {
                            return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行工作编号格式错误，请按标准格式字母加数字自增长编写，如【HSH-0001】"));
                        }
                        oLData.germnumber = "";
                        oLData.germname = dr["细菌名称"].ToString();
                        if (string.IsNullOrEmpty(oLData.germname))
                        {
                            return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行细菌为空"));
                        }
                        oLData.auditor = null;
                        oLData.auditortime = null;
                        for (int j = 2; j < table.Columns.Count; j++)
                        {
                            OLFieldmodel oLFieldmodel = OLFieldmodelService.GetLFieldmodel(templateId, table.Columns[j].ColumnName);
                            var rulename = oLFieldmodel.default_value;
                            var sfbt = oLFieldmodel.sfbt;
                            //if (dr[j].ToString().Contains('>'))
                            //{
                            //    return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行" + table.Columns[j].ColumnName + "的值【" + dr[j].ToString() + "】不允许有【>】或者【>.】"));
                            //}
                            string oldValue = dr[j].ToString();
                            string newValue = "";

                            if (sfbt == "1" || dr[j].ToString() != "")
                            {
                                if (dr[j].ToString().Substring(0, 1) == ".")
                                {
                                    newValue = dr[j].ToString().Replace(".", "0.");
                                }
                                else
                                {
                                    newValue = dr[j].ToString().Replace("<=.", "0.").Replace("<=", "").Replace("<.", "0.").Replace("<", "").Replace(">", "").Replace(">.", "");//取到的单元格数据;
                                }
                                if (string.IsNullOrEmpty(newValue))
                                {
                                    return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行" + table.Columns[j].ColumnName + "的值【" + dr[j].ToString() + "】不能为空"));
                                }
                            }
                            if (rulename.ToUpper() == "MIC" && dr[j].ToString() != "")
                            {
                                if (!rulen.Split(',').Contains(newValue))
                                {
                                    return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行" + table.Columns[j].ColumnName + "的值【" + dr[j].ToString() + "】不是有效的MIC值"));
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(oLData.datevalue))
                                    {
                                        oLData.datevalue = oldValue;
                                    }
                                    else
                                    {
                                        oLData.datevalue += "," + oldValue;
                                    }
                                }
                            }
                            if (rulename.ToUpper() == "ZJ" && dr[j].ToString() != "")
                            {
                                double data1 = Convert.ToDouble(oldValue);
                                if (data1 % 1 == 0 && 6 <= int.Parse(oldValue) && int.Parse(oldValue) < 50)
                                {
                                    if (string.IsNullOrEmpty(oLData.datevalue))
                                    {
                                        oLData.datevalue = oldValue;
                                    }
                                    else
                                    {
                                        oLData.datevalue += "," + oldValue;
                                    }
                                }
                                else
                                {
                                    return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行" + table.Columns[j].ColumnName + "的值【" + dr[j].ToString() + "】只能是整数且大于等于6小于50"));
                                }
                            }
                            if (rulename.ToUpper() == "MM" && dr[j].ToString() != "")
                            {
                                double data1 = Convert.ToDouble(oldValue);
                                if (data1 % 1 == 0 && 6 <= int.Parse(oldValue) && int.Parse(oldValue) < 50)
                                {
                                    if (string.IsNullOrEmpty(oLData.datevalue))
                                    {
                                        oLData.datevalue = oldValue;
                                    }
                                    else
                                    {
                                        oLData.datevalue += "," + oldValue;
                                    }
                                }
                                else
                                {
                                    return Content(JsonHelper.GetBaseMessage(false, "第【" + i + "】行" + table.Columns[j].ColumnName + "的值【" + dr[j].ToString() + "】只能是整数且大于等于6小于50"));
                                }
                            }
                            if (dr[j].ToString() == "")
                            {
                                if (j == 2)
                                    oLData.datevalue += "&";
                                else
                                    oLData.datevalue += ",&";
                            }
                        }
                        oLData.datevalue = oLData.datevalue.Replace("&", "");
                        oLData.created_byId = member.Id;
                        oLDatas.Add(oLData);
                    }
                    int OL = 0;
                    foreach (var item in oLDatas)
                    {
                        OL += OLDataInputService.AddOLDataInput(item);
                    }
                    if (OL > 0)
                    {
                        return Content(JsonHelper.GetBaseMessage(true, "导入成功"));
                    }
                    else
                    {
                        return Content(JsonHelper.GetBaseMessage(false, "操作失败，请重试！"));
                    }

                }
                catch (Exception ex)
                {
                    Log4Helper.Error(ex.Message);
                    this.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.Url.ToString());
                    return this.Content(SpringJsonResult.Error(ex.Message));
                }

            }
            else
            {
                return Content(JsonHelper.GetBaseMessage(false, "上传文件不能为空"));
            }

        }

        public ActionResult Exportexcel(long Id)
        {
            try
            {
                ExcelPackage ep = new ExcelPackage();
                string path = this.OLDataInputService.GetMoban(Id, ep);
                if (string.IsNullOrWhiteSpace(path)) return this.Content("导出文件失败");

                var user = this.LoginUserinfo;
                this.ActionLogService.Insert(ActionType.Export, ActionSource.Admin, user.Id, user.Name, "导出Excel成功", "导出Excel成功");

                Response.Clear();
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                Response.AddHeader("content-disposition", "attachment;filename=" + path);
                Response.ContentType = "application/vnd.open";
                ep.SaveAs(Response.OutputStream);
                Response.Flush();
                Response.End();
                return View();
            }
            catch (Exception ex)
            {
                this.SystemLogService.Insert(ex, SystemLogLevel.Error);
                return this.Content(ex.Message);
            }
        }
    }
}