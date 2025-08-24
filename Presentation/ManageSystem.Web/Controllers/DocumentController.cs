using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Documents;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.Articles;
using ManageSystem.Services.Documents;
using ManageSystem.Services.Satellites;
using ManageSystem.Web.App_Start;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Articles;
using ManageSystem.Web.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    /// <summary>
    /// 文档
    /// </summary>
    public class DocumentController : WebBaseController
    {
        private readonly IDocumentDownloadLogService _logService;
        private readonly IDocumentService _documentService;
        private readonly ISatelliteService _satelliteService;

        public DocumentController(
              IDocumentDownloadLogService logService,
              IDocumentService documentService,
              ISatelliteService satelliteService
        )
        {
            this._logService = logService;
            this._documentService = documentService;
            _satelliteService = satelliteService;
        }

        [CheckRole(false)]
        public ActionResult PageJump(IndexSearchModel model)
        {
            var result = this.SetIndexData(model);

            return View(result) ;
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
            model.PageList = this._documentService.Query(r => r.Owners.Contains("2") && r.Status == true).OrderByDescending(r => r.weChattop).ThenBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ToPagedList(searchModel.PageIndex, 20); //获取查询数据

            return model;
        }

        [CheckRole(false)]
        public ActionResult SatelliteIndex(string city,string te)
        {
            var result = this.SeSatelliteIndexData(new IndexSearchModel(), city);

            return View(result);
        }

        private IndexModel SeSatelliteIndexData(IndexSearchModel searchModel, string city)
        {
            long id = this._satelliteService.Query(x => x.RealmName == city).First().Id;
            //分页数据
            IndexModel model = new IndexModel();
            model.SearchModel = searchModel ?? new IndexSearchModel();
            model.PageList = this._documentService.Query(r => r.Satellite == id && r.Status == true).OrderByDescending(r => r.weChattop).ThenBy(r => r.Sort).ThenByDescending(r => r.InsertTime).ToPagedList(searchModel.PageIndex, 20); //获取查询数据

            return model;
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