using ManageSystem.Core.Domain.Documents;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Services.Documents;
using ManageSystem.Services.Log;
using ManageSystem.Services.Members;
using ManageSystem.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Mobile.Controllers
{
    /// <summary>
    /// 资料下载
    /// </summary>
    public class DocumentController : MobileBaseController
    {
        #region 业务层声明
        /// <summary>
        /// 资料业务层
        /// </summary>
        private readonly IDocumentService DocumentService;
        /// <summary>
        /// 资料下载记录业务层
        /// </summary>
        private readonly IDocumentDownloadLogService DownloadLogService;
        /// <summary>
        /// 用户业务层
        /// </summary>
        private readonly IMemberService MemberService;
        /// <summary>
        /// 加密服务
        /// </summary>
        private readonly IEncryptionService EncryptionService;
        /// <summary>
        /// 系统日志业务层
        /// </summary>
        private readonly ISystemLogService SystemLogService;
        /// <summary>
        /// 操作日志业务层
        /// </summary>
        private readonly IActionLogService ActionLogService;
        #endregion

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="_documentService"></param>
        /// <param name="_memberService"></param>
        /// <param name="_encryptionService"></param>
        /// <param name="_downloadLogService"></param>
        /// <param name="_systemLogService"></param>
        /// <param name="_actionLogService"></param>
        public DocumentController(IDocumentService _documentService, IMemberService _memberService, IEncryptionService _encryptionService, IDocumentDownloadLogService _downloadLogService, ISystemLogService _systemLogService, IActionLogService _actionLogService)
        {
            DocumentService = _documentService;
            DownloadLogService = _downloadLogService;
            MemberService = _memberService;
            EncryptionService = _encryptionService;
            SystemLogService = _systemLogService;
            ActionLogService = _actionLogService;
        }

        #region 资料下载
        /// <summary>
        /// 资料列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 分页获取资料列表
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(int page = 1, int pageSize = 15)
        {
            //var pageList = DocumentService.Query(r => r.UrlName.Equals("《点击下载》")).ToPagedList(page, pageSize); //获取查询数据
            //var pageList = DocumentService.Query(r => r.Owners == null || r.Owners.Trim() == "" || r.Owners.Contains("2")).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ToPagedList(page, pageSize); //获取查询数据
            var pageList = DocumentService.Query(R=>R.Owners.Contains("2")).OrderByDescending(r => r.weChattop).ThenBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ToPagedList(page, pageSize);          
            //List<string> supportExt = new List<string> { ".PDF", ".DOC", ".DOCX", ".XLS", ".XLSX", ".PPT", ".PPTX" };
            List<dynamic> data = new List<dynamic>();
            if (pageList.Any())
            {
                foreach (var item in pageList)
                {
                    var fileExt = !string.IsNullOrWhiteSpace(item.FilePath) ? System.IO.Path.GetExtension(item.FilePath) : "";
                    data.Add(new
                    {
                        Id = item.Id.ToString(),
                        item.Name,
                        item.Count,
                        Date = item.InsertTime.ToString("yyyy-MM-dd"),
                        //FileExt = fileExt,
                        //IsView = supportExt.Contains(fileExt.ToUpperInvariant()),
                        Url = item.Url
                    });
                }
            }

            return Json(new
            {
                status = true,
                data = data,
                nextPage = page + 1,
                existNextPage = pageList.TotalPageCount > page
            });
        }

        /// <summary>
        /// 增加下载记录
        /// </summary>
        /// <param name="documentId">资料id</param>
        /// <param name="token">用户token</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DownloadLog(long documentId, string token)
        {
            if (token == "")
            {
                Document document = DocumentService.QueryEntity(documentId);
                DownloadLogService.Insert(new DocumentDownloadLog()
                {
                    DocumentId = document.Id,
                    DocumentName = document.Name,
                    MemberId =0,
                    MemberName = "匿名",
                    Describe = "移动端下载资料"
                });

                document.Count += 1;

                DocumentService.Update(document);
                try
                {
                    this.ActionLogService.Insert(ActionType.View, ActionSource.Mobile, 0, "匿名", $"匿名下载了在线资料：{document.Name}");
                }
                catch (System.Exception)
                {

                }
                return Json(new { status = true, url = document.Url });
            }
            else
            {
                #region 用户信息
                string tokenText = EncryptionService.DecryptText(token);
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
                    member = MemberService.QueryEntity(member_id);
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

                Document document = DocumentService.QueryEntity(documentId);
                if (document == null || document.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取资料信息" });
                }

                DownloadLogService.Insert(new DocumentDownloadLog()
                {
                    DocumentId = document.Id,
                    DocumentName = document.Name,
                    MemberId = member.Id,
                    MemberName = member.Name,
                    Describe = "移动端下载资料"
                });

                document.Count += 1;

                DocumentService.Update(document);
                try
                {
                    this.ActionLogService.Insert(ActionType.View, ActionSource.Mobile, member.Id, member.Name + "（" + member.LoginId + "）", $"{member.Name}下载了在线资料：{document.Name}");
                }
                catch (System.Exception)
                {

                }
                return Json(new { status = true, url = document.Url });
            }
        }

        //public ActionResult DownloadLog(long documentId, string token)
        //{
        //    #region 用户信息
        //    string tokenText = EncryptionService.DecryptText(token);
        //    long member_id = 0;
        //    Member member = null;
        //    try
        //    {
        //        string[] sArray = Regex.Split(tokenText, "@@");
        //        long.TryParse(sArray[0], out member_id);
        //        if (member_id <= 0)
        //        {
        //            return Json(new { status = false, message = "未能获取用户信息" });
        //        }
        //        member = MemberService.QueryEntity(member_id);
        //        if (member == null || member.Id <= 0)
        //        {
        //            return Json(new { status = false, message = "未能获取用户信息" });
        //        }
        //        if (member.Mark != 1 && member.Mark != 2)
        //        {
        //            return Json(new { status = false, message = "用户信息状态异常" });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { status = false, message = "未能获取用户信息" });
        //    }
        //    #endregion

        //    Document document = DocumentService.QueryEntity(documentId);
        //    if (document == null || document.Id <= 0)
        //    {
        //        return Json(new { status = false, message = "未能获取资料信息" });
        //    }

        //    DownloadLogService.Insert(new DocumentDownloadLog()
        //    {
        //        DocumentId = document.Id,
        //        DocumentName = document.Name,
        //        MemberId = member.Id,
        //        MemberName = member.Name,
        //        Describe = "移动端下载资料"
        //    });

        //    document.Count += 1;

        //    DocumentService.Update(document);
        //    try
        //    {
        //        this.ActionLogService.Insert(ActionType.View, ActionSource.Mobile, member.Id, member.Name + "（" + member.LoginId + "）", $"{member.Name}下载了在线资料：{document.Name}");
        //    }
        //    catch (System.Exception)
        //    {

        //    }

        //    return File(null);
        //}
        #endregion

    }
}