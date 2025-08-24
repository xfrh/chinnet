using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Admin.Extensions;
using ManageSystem.Framework.Controllers;
using ManageSystem.Framework.Kendoui;
using System.Web;
using System.Web.Mvc;
using ManageSystem.Core;
using ManageSystem.Services.Articles;
using ManageSystem.Admin.Models.Articles;
using ManageSystem.Core.Domain.Articles;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Framework;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Extensions;
using ManageSystem.Admin.App_Start;
using Newtonsoft.Json;

namespace ManageSystem.Admin.Controllers
{

    public class ArticlesController : AdminBaseController
    {

        private readonly IArticleTypeService articleTypeService;
        private readonly IArticleService articleService;
        private readonly IArticleAttachmentService ArticleAttachmentService;
        private readonly IArticleViewService ArticleViewService;

        public ArticlesController(
            IArticleTypeService _articleTypeService,
            IArticleService _articleService,
            IArticleAttachmentService _articleAttachmentService,
    
                IArticleViewService _articleViewService
        )
        {
            this.articleTypeService = _articleTypeService;
            this.articleService = _articleService;
            this.ArticleAttachmentService = _articleAttachmentService;
            this.ArticleViewService = _articleViewService;
        }


        #region 文章分类

        /// <summary>
        /// 文章分类 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleTypeList()
        {
            return View();
        }

        /// <summary>
        /// 文章分类 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleTypeList(DataSourceRequest command, ArticleTypeModel model)
        {

            //获得数据
            var list = this.articleTypeService.QueryPage(model.Name, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        InsertTime = x.InsertTime,
                        Sort = x.Sort
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }


        /// <summary>
        /// 文章分类 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleTypeCreate()
        {
            return View(this.SetArticleTypeCreateData());
        }


        private ArticleTypeModel SetArticleTypeCreateData()
        {
            ArticleTypeModel model = new ArticleTypeModel();
            return model;
        }

        /// <summary>
        /// 文章分类 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleTypeCreate(ArticleTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                this.articleTypeService.Insert(entity);

                string logContent = "添加文章分类，分类名称： " + model.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ArticleTypeCreate");
            }

            return View(this.SetArticleTypeCreateData());
        }


        /// <summary>
        /// 文章分类 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleTypeEdit(long id)
        {
            return this.View(this.SetArticleTypeEditData(id));
        }

        private ArticleTypeModel SetArticleTypeEditData(long id)
        {
            var entity = this.articleTypeService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }

        /// <summary>
        /// 文章分类 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleTypeEdit(ArticleTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = this.articleTypeService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.articleTypeService.Update(entity);

                string logContent = "修改文章分类 ，分类名称：" + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ArticleTypeList");
            }

            return this.View(this.SetArticleTypeEditData(model.Id));
        }


        /// <summary>
        /// 文章分类 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleTypeView(long id)
        {
            return this.View(this.SetArticleTypeEditData(id));
        }


        /// <summary>
        /// 文章分类 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleTypeDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ArticleTypeList");
            }

            this.articleTypeService.Delete(selectedIds);

            string logContent = "【手动】删除文章分类，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ArticleTypeList");
        }


        #endregion

        #region 文章

        /// <summary>
        /// 文章 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleList()
        {
            ArticleModel model = this.SetArticleList();
            return View(model);
        }

        /// <summary>
        /// 文章 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleList(DataSourceRequest command, ArticleModel model)
        {
            //获得数据
            var list = this.articleService.QueryPage(model.Name, model.ArticleTypeId, model.State, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        State = ((ArticleState)x.State).GetDescription(),
                        Author = x.Author,
                        ReleaseTime = x.ReleaseTime.ToString("yyyy-MM-dd"),
                        TypeName = this.articleTypeService.GetTypeName(x.ArticleTypeId)
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }

        /// <summary>
        /// 文章 列表页面
        /// </summary>
        /// <returns></returns>
        [CheckRoleAttribute(true, false)]
        public ActionResult ArticleSelectList()
        {
            ArticleModel model = this.SetArticleList();

            return View(model);
        }

        /// <summary>
        /// 获取列表页面数据
        /// </summary>
        /// <returns></returns>
        public ArticleModel SetArticleList()
        {
            ArticleModel model = new ArticleModel();

            model.StateList = ArticleState.Disable.ToSelectList().ToList();
            model.ArticleTypeList = this.articleTypeService.Query().OrderBy(m => m.Sort).
            Select(x =>
            { return new SelectListItem { Text = x.Name, Value = x.Id.ToString() }; }
            ).ToList();

            model.ArticleTypeList.Insert(0, new SelectListItem() { Text = "全部分类", Value = "" });

            return model;
        }


        /// <summary>
        /// 文章 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public ArticleModel SetArticleCreateData()
        {
            ArticleModel model = new ArticleModel();
            model.StateList = ArticleState.Disable.ToSelectList(false, false).ToList();
            model.ReleaseTime = DateTime.Now;
            model.Author = base.LoginUserinfo.Name;
            model.Id = CommonHelper.GuidToLongID;
            model.ArticleAttachmentList = new List<ArticleAttachmentModel>();

            model.ArticleTypeList = this.articleTypeService.Query().OrderBy(m => m.Sort).
                Select(x =>
                { return new SelectListItem { Text = x.Name, Value = x.Id.ToString() }; }
                ).ToList();

            return model;
        }

        /// <summary>
        /// 文章 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleCreate()
        {

            return View(this.SetArticleCreateData());

        }

        /// <summary>
        /// 文章 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ArticleCreate(ArticleModel model, HttpPostedFileBase coverImage)
        {
            model.CoverImage = this.UploadCoverImage(coverImage);

            if (string.IsNullOrWhiteSpace(model.ShortContent)) model.ShortContent = "";

         
            if (ModelState.IsValid)
            {
                var entity = model.ToEntity();
                entity.CodeImage = this.CreateTwoDimensionCode(model.Id);
                entity.PayPrice = entity.IsPay ? entity.PayPrice : 0;
                entity.UserinfoId = base.LoginUserinfo.Id;

                this.articleService.Insert(entity);

                string logContent = "添加文章成功，文章标题： " + model.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ArticleCreate");
            }

            return View(this.SetArticleCreateData());
        }

        /// <summary>
        /// 上传文章封面图片
        /// </summary>
        /// <param name="coverImage"></param>
        /// <returns></returns>
        private string UploadCoverImage(HttpPostedFileBase coverImage)
        {
            string result = "";

            //上传封面图片
            if (coverImage != null)
            {
                if (coverImage.ContentLength <= 0)
                {
                    ModelState.AddModelError("CoverImage", "封面图片格式或其他原因不正确");
                    return "";
                }

                string path = "/Content/upload/article";
                UpLoadFileResult uploadResult = UpLoadFiles.UpLoadFile(coverImage, path, 0, UpLoadType.Image, "");
                if (uploadResult == null || !uploadResult.State)
                {
                    ModelState.AddModelError("CoverImage", "封面图片格式或其他原因不正确");
                }
                else
                {
                    result = path + "/" + uploadResult.Name;
                }
            }

            return result;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CreateTwoDimensionCode(long id)
        {
            string codeImageFileName = "articles-" + Guid.NewGuid().ToString().Replace("-", "");
            string path = "/Content/Upload/Code/"; //二维码的图片地址
            string codeValue = base.SettingService.QueryValue<string>("web.web.url") + "/Code/Index?type=questionnaire&data=&dataId=" + id;
            if (!string.IsNullOrWhiteSpace(TwoDimensionCode.CreateAndSaveCode(codeValue, this.Server.MapPath(path), ref codeImageFileName, 10)))
                return path + codeImageFileName;

            return "";
        }

        /// <summary>
        /// 文章 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ArticleModel SetArticleEditData(long id)
        {
            var entity = this.articleService.QueryEntity(id);

            var model = entity.ToModel();
            model.StateList = ((ArticleState)model.State).ToSelectList(true, false).ToList();

            model.ArticleTypeList = this.articleTypeService.Query().OrderBy(m => m.Sort).
            Select(x =>
            { return new SelectListItem { Text = x.Name, Value = x.Id.ToString() }; }
            ).ToList();

            model.ArticleAttachmentList = this.ArticleAttachmentService.Query(m => m.ArticleId == entity.Id && m.Mark > 0).OrderBy(m => m.InsertTime).Select(x => x.ToModel()).ToList();

            return model;
        }

        /// <summary>
        /// 文章 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleEdit(long id)
        {
            return this.View(this.SetArticleEditData(id));
        }

        /// <summary>
        /// 文章 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ArticleEdit(ArticleModel model, HttpPostedFileBase coverImage)
        {
            var image = this.UploadCoverImage(coverImage);
            if (!string.IsNullOrWhiteSpace(image))
                model.CoverImage = image;

            if (ModelState.IsValid)
            {
                var entity = this.articleService.QueryEntity(model.Id);
                if (string.IsNullOrWhiteSpace(model.CoverImage))
                    model.CoverImage = entity.CoverImage;

                model.CodeImage = entity.CodeImage;
                model.ViewCount = entity.ViewCount;
                model.UserinfoId = entity.UserinfoId;

                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.articleService.Update(entity);

                string logContent = "修改文章成功，文章标题： " + model.Name;
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("ArticleList");
            }
            return this.View(this.SetArticleEditData(model.Id));
        }


        /// <summary>
        /// 文章 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleView(long id)
        {
            return this.View(this.SetArticleEditData(id));
        }


        /// <summary>
        /// 文章 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ArticleList");
            }

            this.articleService.Delete(selectedIds);

            string logContent = "【手动】删除文章，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ArticleList");
        }


        /// <summary>
        /// 根据文章id或者ids来查询数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(true, false)]
        public ContentResult GetArticleListByIds(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return this.Content("");

            var list = this.articleService.Query(ids);
            if (list == null || !list.Any()) return this.Content("");

            var modelList = list.Select(x =>
            {
                return new
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    State = ((ArticleState)x.State).GetDescription(),
                    Author = x.Author,
                    ReleaseTime = x.ReleaseTime.ToString("yyyy-MM-dd"),
                    TypeName = this.articleTypeService.GetTypeName(x.ArticleTypeId)
                };
            });

            return this.Content(JsonConvert.SerializeObject(modelList));
        }

        /// <summary>
        /// 上传文章附件
        /// </summary>
        /// <param name="articleId">文章id</param>
        /// <returns></returns>
        [CheckRole(true, false)]
        public ContentResult UploadArticleFile(long articleId)
        {
            string message = "";
            string fileName = "";
            string filePath = "";
            string attachmentId = "";

            bool status = false;

            try
            {
                var file = this.Request.Files;
                if (file != null && file.Count > 0)
                {
                    fileName = file[0].FileName;
                    string fileExt = System.IO.Path.GetExtension(file[0].FileName).ToLower();
                    filePath = "/Content/Upload/ArticleAttachment/" + DateTime.Now.ToString("yyyyMMddhhmmsss") + fileExt;

                    ArticleAttachmentType attachmentType = ArticleExtensions.GetAttachmentType(fileExt.TrimEnd('.').TrimStart('.'));

                    //保存文件
                    file[0].SaveAs(Server.MapPath(filePath));

                    //保存文件到数据库
                    ArticleAttachment model = new ArticleAttachment()
                    {
                        Id = CommonHelper.GuidToLongID,
                        ArticleId = articleId,
                        Describe = "",
                        Name = fileName,
                        Path = filePath,
                        Type = (int)attachmentType
                    };

                    this.ArticleAttachmentService.Insert(model);

                    attachmentId = model.Id.ToString();
                    message = fileName;
                    status = true;

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            Dictionary<string, string> r = new Dictionary<string, string>();
            r.Add("status", status ? "true" : "false");
            r.Add("message", message);
            r.Add("fileName", fileName);
            r.Add("filePath", filePath);
            r.Add("attachmentId", attachmentId);

            return this.Content(JsonConvert.SerializeObject(r));
        }



        #endregion


        #region 文章附件
        /// <summary>
        /// 文章附件 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        [CheckRole(true, false)]
        public ContentResult ArticleAttachmentDelete(long articlesId, long attachmentId)
        {
            string message = "";
            bool status = false;

            try
            {
                if (articlesId <= 0 || attachmentId <= 0) throw new Exception("数据错误");

                var entity = this.ArticleAttachmentService.QueryEntity(attachmentId);
                if (entity == null || entity.ArticleId <= 0) throw new Exception("数据错误");

                if (entity.ArticleId != articlesId) throw new Exception("无权删除其他文章附件");

                this.ArticleAttachmentService.Delete(attachmentId);

                string logContent = "【手动】删除文章附件，删除的id集合:" + attachmentId;
                base.InsetActionLog(ActionType.Delete, logContent);

                message = "";
                status = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return this.Content(JsonHelper.GetBaseMessage(status, message));
        }

        #endregion

 

        #region 文章查看管理

        /// <summary>
        /// 文章查看管理 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleViewList()
        {
            return View();
        }

        /// <summary>
        /// 文章查看管理 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleViewList(DataSourceRequest command, ArticleViewModel model)
        {
            //获得数据
            var list = this.ArticleViewService.QueryPage(model.MemberName, model.ArticleId, model.ArticleName, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        InsertTime = x.InsertTime.ToString("yyyy-MM-dd HH:mm"),
                        Describe = x.Describe,
                        ArticleName = this.articleService.GetArticleName(x.ArticleId),
                        ArticleId = x.ArticleId.ToString(),
                        MemberId = x.MemberId.ToString(),
                        MemberName = x.MemberName
                    };
                }),
                Total = list.TotalCount
            };

            return new JsonResult { Data = gridModel };
        }




        /// <summary>
        /// 文章查看管理 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public ArticleViewModel SetArticleViewEditData(long id)
        {
            var entity = this.ArticleViewService.QueryEntity(id);

            var model = entity.ToModel();

            return model;
        }


        /// <summary>
        /// 文章查看管理 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleViewView(long id)
        {
            return this.View(this.SetArticleViewEditData(id));
        }


        /// <summary>
        /// 文章查看管理 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleViewDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("ArticleViewList");
            }

            this.ArticleViewService.Delete(selectedIds);

            string logContent = "【手动】删除文章查看管理，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent, selectedIds.SerializeObject());
            base.SuccessNotification(logContent);

            return this.RedirectToAction("ArticleViewList");
        }


        #endregion
    }
}
