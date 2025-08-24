using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Services.Articles;
using ManageSystem.Web.Extensions;
using ManageSystem.Web.Models.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Controllers
{
    public class NewsController : WebBaseController
    {
        private readonly IArticleTypeService ArticleTypeService;
        private readonly IArticleService ArticleService;


        public NewsController(IArticleTypeService articleTypeService, IArticleService articleService)
        {
            this.ArticleTypeService = articleTypeService;
            this.ArticleService = articleService;
        }

     
        public ActionResult Index(int pageIndex = 1, string type = "")
        {
            this.ViewBag.ArticleTypeList = this.ArticleTypeService.Query(m => m.Mark > 0).OrderBy(m => m.Sort).Select(x => x.ToModel()).ToList();

            var result = this.ArticleService.QueryPage(type).Select(x=>x.ToModel()).ToPagedList(pageIndex, 5);

            return View(result);
        }

        public ActionResult Detail(long  id)
        {
            try
            {
                var entity = this.ArticleService.QueryEntity(id);
                if (entity == null || entity.State != (int)ArticleState.Normal)
                {
                    base.ErrorNotification("操作提示|文章不存在或不可访问！");
                    return this.RedirectToAction("Index","News");
                }

                //右侧的数据
                Dictionary<ArticleTypeModel, List<ArticleModel>> listData = new Dictionary<ArticleTypeModel, List<ArticleModel>>();
                var typeList = this.ArticleTypeService.Query(m => m.Mark > 0).OrderBy(m => m.Sort).Select(x => x.ToModel()).ToList();
                foreach (var item in typeList)
                {
                    var articleList = this.ArticleService.Query(m => m.ArticleTypeId == item.Id && m.Mark > 0 && m.State == (int)ArticleState.Normal)
                                                                .OrderByDescending(m => m.ReleaseTime).Take(10).Select(m => m.ToModel()).ToList();
                    if (articleList != null && articleList.Any())
                        listData.Add(item, articleList);
                }
                this.ViewBag.RightListData = listData;

                var model = entity.ToModel();

                return View(model);
            }
            catch (Exception ex)
            {
                base.SystemLogService.Insert(ex, SystemLogLevel.Error, this.Request.RawUrl.ToString());
                return this.RedirectToAction("Index");
            }
        }

    }
}