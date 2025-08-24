using ManageSystem.Core.Domain.Documents;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Services.Documents;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Controllers;
using ManageSystem.Web.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Areas.CR.Controllers
{
    public class CRDocumentController : WebBaseController
    {
        // GET: CRs/CRDocument
        private readonly IDocumentDownloadLogService _logService;
        private readonly IDocumentService _documentService;

        public CRDocumentController(
              IDocumentDownloadLogService logService,
              IDocumentService documentService
        )
        {
            this._logService = logService;
            this._documentService = documentService;
        }

        [CheckRole(false)]
        public ActionResult Index(IndexSearchModel model)
        {
            var result = this.SetIndexData(model);

            return View(result);
        }

        private IndexModel SetIndexData(IndexSearchModel searchModel)
        {
            //分页数据
            IndexModel model = new IndexModel();
            model.SearchModel = searchModel ?? new IndexSearchModel();
            model.PageList = this._documentService.Query(r => r.Owners.Contains("4") && r.Status == true).OrderByDescending(r => r.weChattop).ThenBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ToPagedList(searchModel.PageIndex, 20); //获取查询数据
            return model;
        }

        [CheckRole(false)]
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
        [CheckRole(false)]
        public JsonResult List(int page = 1, int pageSize = 15)
        {
            //var pageList = DocumentService.Query(r => r.UrlName.Equals("《点击下载》")).ToPagedList(page, pageSize); //获取查询数据
            //var pageList = DocumentService.Query(r => r.Owners == null || r.Owners.Trim() == "" || r.Owners.Contains("2")).OrderBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ToPagedList(page, pageSize); //获取查询数据
            var pageList = this._documentService.Query(r => r.Owners.Contains("4") && r.Status == true).OrderByDescending(r => r.weChattop).ThenBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ToPagedList(page, pageSize);
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
                        item.Link,
                        item.UrlName,
                        //FileExt = fileExt,
                        //IsView = supportExt.Contains(fileExt.ToUpperInvariant()),
                        item.Url
                    }); ;
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
        [CheckRole(false)]
        public JsonResult DownloadLog(long documentId, string token)
        {
            if (token == "")
            {
                Document document = _documentService.QueryEntity(documentId);
                _logService.Insert(new DocumentDownloadLog()
                {
                    DocumentId = document.Id,
                    DocumentName = document.Name,
                    MemberId = 0,
                    MemberName = "匿名",
                    Describe = "下载资料"
                });

                document.Count += 1;

                _documentService.Update(document);
                try
                {
                    this.ActionLogService.Insert(ActionType.View, ActionSource.Mobile, 0, "匿名", $"匿名下载了在线资料：{document.Name}");
                }
                catch (System.Exception)
                {

                }
                return Json(new { status = true, url = document.Url, Link=document.Link });
            }
            else
            {
                #region 用户信息
               
                Member member = LoginUserinfo;               
                #endregion

                Document document = _documentService.QueryEntity(documentId);
                if (document == null || document.Id <= 0)
                {
                    return Json(new { status = false, message = "未能获取资料信息" });
                }

                _logService.Insert(new DocumentDownloadLog()
                {
                    DocumentId = document.Id,
                    DocumentName = document.Name,
                    MemberId = member.Id,
                    MemberName = member.Name,
                    Describe = "下载资料"
                });

                document.Count += 1;

                _documentService.Update(document);
                try
                {
                    this.ActionLogService.Insert(ActionType.View, ActionSource.Mobile, member.Id, member.Name + "（" + member.LoginId + "）", $"{member.Name}下载了在线资料：{document.Name}");
                }
                catch (System.Exception)
                {

                }
                return Json(new { status = true, url = document.Url,Link = document.Link });
            }
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="id">id文档id</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(false)]
        public ContentResult Download(long Id)
        {
            var document = this._documentService.QueryEntity(Id);
            if (document == null || document.Id <= 0)
                return this.Content("");

            var member = base.LoginUserinfo;
            if (member == null)
            {
                this._logService.Insert(new DocumentDownloadLog()
                {
                    DocumentId = document.Id,
                    DocumentName = document.Name,
                    MemberName = "匿名",
                    MemberId = 0,
                });
                document.Count += 1;
                this._documentService.Update(document);
                base.InsetActionLog(ActionType.Create, "匿名" + "下载了在线资料：" + document.Name);
                return this.Content("");
            }
            else
            {
                this._logService.Insert(new DocumentDownloadLog()
                {
                    DocumentId = document.Id,
                    DocumentName = document.Name,
                    MemberId = member.Id,
                    MemberName = member.Name,
                });

                document.Count += 1;
                this._documentService.Update(document);

                base.InsetActionLog(ActionType.Create, member.Name + "下载了在线资料：" + document.Name);

                return this.Content("");
            }
        }
    }
}