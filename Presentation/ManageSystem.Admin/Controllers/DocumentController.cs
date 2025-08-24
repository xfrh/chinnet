using ManageSystem.Admin.App_Start;
using ManageSystem.Admin.Models.Document;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Documents;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Utility;
using ManageSystem.Framework.Kendoui;
using ManageSystem.Services.Documents;
using ManageSystem.Services.Members;
using ManageSystem.Services.Satellites;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageSystem.Admin.Controllers
{
    public class DocumentController : AdminBaseController
    {
        private readonly IDocumentDownloadLogService _documentDownloadLog;
        private readonly IDocumentService _documentService;
        private readonly IMemberService _memberService;
        private readonly ISatelliteService _satelliteService;

        public DocumentController(
              IDocumentDownloadLogService documentDownloadLog,
              IDocumentService documentService,
              IMemberService memberService,
              ISatelliteService satelliteService
        )
        {
            _documentDownloadLog = documentDownloadLog;
            _documentService = documentService;
            _memberService = memberService;
            _satelliteService = satelliteService;
        }

        #region 列表
        public ViewResult List()
        {
            return View(new DocumentModel());
        }

        [HttpPost]
        public JsonResult List(DataSourceRequest command, DocumentModel document)
        {
            IPagedList<Document> list = _documentService.QueryPage(document.Title, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Status = x.Status ? "是" : "否",
                        x.Sort,
                        Count = new
                        {
                            Id = x.Id.ToString(),
                            Value = x.Count
                        },
                        Owners = x.Owners.Replace("2", "CHINET").Replace("3", "CRE专项研究").Replace("4","CR替加环素").Replace(",", "/"),
                        DateTime = new
                        {
                            Date = x.InsertTime.ToString("yyyy-MM-dd"),
                            Time = x.InsertTime.ToString("HH:mm"),
                            FullDateTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm")
                        },
                        weCattop=x.weChattop?"1":"0"
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 新增
        public ViewResult Create()
        {

            return View(SetDocumentCreateData());
        }

        [HttpPost]
        public ActionResult Create(DocumentModel document)
        {
            if (document.Owners == null || document.Owners.Count <= 0)
            {
                ModelState.AddModelError("Owners", "请选择资料所属");
            }
            else if (document.Owners.Contains(3)|| document.Owners.Contains(4))
            {
                if (string.IsNullOrWhiteSpace(document.Link) && (document.PostFile == null || document.PostFile.ContentLength == 0))
                {
                    ModelState.AddModelError("PostFile", "请上传文件或选择输入链接地址");
                    ModelState.AddModelError("Link", "请输入链接地址或选择上传文件");
                }
                else if (string.IsNullOrWhiteSpace(document.Link) && (document.PostFile != null && document.PostFile.ContentLength > 0))
                {
                    document.FilePath = UploadDocumentFile(document.PostFile);
                }
                else if (string.IsNullOrWhiteSpace(document.Link))
                {
                    ModelState.AddModelError("Link", "请输入链接地址");
                }
            }
            else
            {
                document.FilePath = UploadDocumentFile(document.PostFile);
            }

            if (ModelState.IsValid)
            {
                var entity = new Document
                {
                    Count = 0,
                    InsertTime = DateTime.Now,
                    Mark = 1,
                    Name = document.Title,
                    Sort = document.Sort,
                    Status = document.Status,
                    Url = document.FilePath,
                    FilePath = document.FilePath,
                    UrlName = document.UrlName,
                    Owners = string.Join(",", document.Owners),
                    Link = document.Link,
                    weChattop = document.weChattop,
                    Satellite = document.Satellite
                };
                if (_documentService.GetSort(entity.Sort) != null)
                {
                    _documentService.UpdateSort(entity.Sort);
                    _documentService.Insert(entity);
                }
                else
                {
                    _documentService.Insert(entity);
                }

                string logContent = "新增资料成功，资料标题： " + entity.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);
            }

            return View(SetDocumentCreateData());
        }

        private DocumentModel SetDocumentCreateData()
        {
            List<SelectListItem> sateName = this._satelliteService.Query().Where(x => x.RealmName != "&" && x.Mark > 0).Select(x => { return new SelectListItem() { Text = x.SatelliteName, Value = x.Id.ToString() }; }).ToList();
            sateName.Insert(0, new SelectListItem() { Text = "没有可不选", Value = "0" });
            return new DocumentModel
            {
                Id = CommonHelper.GuidToLongID,
                //Sort = _documentService.CurrentMaxSort() + 1,
                Sort = 1,
                UrlName = "《点击下载》",
                Status = true,
                weChattop= false,
                BelongSate = sateName
            };
        }

        private string UploadDocumentFile(HttpPostedFileBase postFile)
        {
            string result = null;
            if (postFile == null || postFile.ContentLength <= 0)
            {
                ModelState.AddModelError("PostFile", "未能获取到文件信息");
                return null;
            }

            string path = "/Content/File";
            UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(postFile, path, 51200, UpLoadType.Unlimited, System.IO.Path.GetFileNameWithoutExtension(postFile.FileName));
            if (uploadResult == null || !uploadResult.State)
            {
                ModelState.AddModelError("PostFile", uploadResult.ErrorMessage);
                return null;
            }
            else
            {
                result = path + "/" + uploadResult.Name;
            }
            return result;
        }
        #endregion

        #region 修改
        public ViewResult Edit(long id)
        {
            return View(SetDocumentEditData(id));
        }

        [HttpPost]
        public ActionResult Edit(DocumentModel document)
        {
            if (document.Owners == null || document.Owners.Count <= 0)
            {
                ModelState.AddModelError("Owners", "请选择资料所属");
            }

            document.FilePath = null;
            string link = null;

            var entity = _documentService.QueryEntity(document.Id);
            if (entity == null || entity.Id <= 0)
            {
                base.ErrorNotification("未能获取到实体信息");
                return View(SetDocumentEditData(document.Id));
            }

            if (document.Owners.Contains(3)|| document.Owners.Contains(4))
            {
                if (string.IsNullOrWhiteSpace(entity.FilePath) && string.IsNullOrWhiteSpace(document.Link))
                {
                    ModelState.AddModelError("PostFile", "请上传文件或选择输入链接地址");
                    ModelState.AddModelError("Link", "请输入链接地址或选择上传文件");
                }
                else if (!string.IsNullOrWhiteSpace(entity.FilePath) && string.IsNullOrWhiteSpace(document.Link))
                {
                    if (document.PostFile != null && document.PostFile.ContentLength > 0)
                    {
                        document.FilePath = UploadDocumentFile(document.PostFile);
                    }
                }
                else if (string.IsNullOrWhiteSpace(entity.FilePath) && !string.IsNullOrWhiteSpace(document.Link))
                {
                    link = document.Link;
                }
                else if (!string.IsNullOrWhiteSpace(entity.FilePath) && !string.IsNullOrWhiteSpace(document.Link))
                {
                    link = document.Link;
                }
            }
            else
            {
                if (document.PostFile != null && document.PostFile.ContentLength > 0)
                {
                    document.FilePath = UploadDocumentFile(document.PostFile);
                }
            }

            if (ModelState.IsValid)
            {
                int Sort = SetDocumentEditData(document.Id).Sort;

                entity.Name = document.Title;
                entity.UrlName = document.UrlName;
                entity.Sort = document.Sort;
                entity.Status = document.Status;
                entity.Owners = string.Join(",", document.Owners);
                entity.weChattop = document.weChattop;
                entity.Satellite = document.Satellite;
                if (document.Owners.Contains(3) && !string.IsNullOrWhiteSpace(document.Link))
                {
                    entity.Link = document.Link;
                }

                if (!string.IsNullOrWhiteSpace(document.FilePath))
                {
                    entity.Url = document.FilePath;
                    entity.FilePath = document.FilePath;
                }
                int aaa = SetDocumentEditData(document.Id).Sort;
                if (document.Sort != Sort && _documentService.QueryEntity(R => R.Sort == document.Sort) != null)
                {
                    _documentService.UpdateSort(document.Sort);
                    _documentService.Update(entity);
                }
                else
                {
                    _documentService.Update(entity);
                }

                string logContent = "修改资料成功，资料标题： " + entity.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);
                return RedirectToAction("List");
            }

            return View(SetDocumentEditData(document.Id));
        }

        private DocumentModel SetDocumentEditData(long id)
        {
            var entity = _documentService.QueryEntity(id);
            if (entity == null)
            {
                return SetDocumentCreateData();
            }

            List<int> Owners = new List<int>();
            if (!string.IsNullOrWhiteSpace(entity.Owners))
            {
                entity.Owners.Split(',').ToList().ForEach(item =>
                {
                    if (int.TryParse(item, out int value) && value > 0)
                    {
                        Owners.Add(value);
                    }
                });
            }

            return new DocumentModel
            {
                FilePath = entity.FilePath,
                Id = entity.Id,
                InsertTime = entity.InsertTime,
                PostFile = null,
                Sort = entity.Sort,
                Status = entity.Status,
                Title = entity.Name,
                UrlName = entity.UrlName,
                Owners = Owners,
                Link = entity.Link,
                weChattop=entity.weChattop,
                Satellite = entity.Satellite

            };
        }
        #endregion

        #region 详情
        public ActionResult Detail(long id)
        {
            Document document = new Document();
            document = _documentService.QueryEntity(id);
            //_documentService.QueryEntity(id) ?? new Document()
            return View(document);
        }
        #endregion

        #region 删除
        [HttpPost]
        public ActionResult Delete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("List");
            }

            _documentService.Delete(selectedIds);

            #region 文件处理
            try
            {
                List<long> ids = selectedIds.Split(',').Select(x => { return long.Parse(x); }).ToList();
                var d = _documentService.QueryDelete(ids);
                _documentService.QueryDelete(ids).ForEach(item =>
                {
                    var filePath = Server.MapPath(item.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(item.FilePath);
                        string fileExt = System.IO.Path.GetExtension(item.FilePath);
                        Computer MyComputer = new Computer();
                        MyComputer.FileSystem.RenameFile(filePath, $"{fileName}_DELETE_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{fileExt.TrimStart('.')}");
                    }
                });
            }
            catch (Exception)
            {

            }
            #endregion

            string logContent = "【手动】删除资料，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("List");
        }
        #endregion

        #region 下载记录
        public ViewResult DocumentDownloadList(long documentId)
        {
            return View();
        }

        [HttpPost]
        public JsonResult DocumentDownloadList(long documentId, string name, DataSourceRequest command)
        {
            IPagedList<DocumentDownloadLog> list = _documentDownloadLog.QueryPage(documentId, name, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        DocumentName = x.DocumentName,
                        Member = new
                        {
                            Name = x.MemberName,
                            //Mobile = _memberService.QueryEntity(x.MemberId)?.Phone
                        },
                        DownloadDateTime = new
                        {
                            Date = x.InsertTime.ToString("yyyy-MM-dd"),
                            Time = x.InsertTime.ToString("HH:mm")
                        },
                        x.Describe
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }
        #endregion

        #region 置顶
        /// <summary>
        /// 置顶功能
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [CheckRole(true,false)]
        public ActionResult WeChattop(long Id)
        {
            int result = _documentService.weChattop(Id);
            if (result > 0)
            {
                var entity = _documentService.QueryEntity(Id);
                string logContent = "置顶成功，质料标题：" + entity.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);
            }
            else
            {
                string logContent = "系统故障前联系开发人员";
                base.InsetActionLog(ActionType.Create, logContent);
                base.ErrorNotification(logContent);
            }

            return  RedirectToAction("List", "Document", false);
        }
        #endregion

        #region 取消置顶
        /// <summary>
        /// 取消置顶功能
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ActionResult Cancelceiling(long Id)
        {
            int result = _documentService.Cancelceiling(Id);
            if (result > 0)
            {
                var entity = _documentService.QueryEntity(Id);
                string logContent = "取消置顶成功，质料标题：" + entity.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);
            }
            else
            {
                string logContent = "系统故障前联系开发人员";
                base.InsetActionLog(ActionType.Create, logContent);
                base.ErrorNotification(logContent);
            }

            return RedirectToAction("List", "Document", false);
        }
        #endregion
    }
}