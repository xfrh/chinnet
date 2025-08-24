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
using ManageSystem.Services.Weixin;
using ManageSystem.Admin.Models.Weixin;
using ManageSystem.Core.Domain.Weixin;
using System.Linq.Expressions;
using ManageSystem.Services.Log;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Framework;
using ManageSystem.Core.Extensions;
using ManageSystem.Admin.App_Start;
using Senparc.Weixin.MP.CommonAPIs;
using ManageSystem.Services.Common;
using Newtonsoft.Json;
using ManageSystem.Services.Articles;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Services.SystemSet;
using ManageSystem.Admin.Models.SystemSet;
using ManageSystem.Core.Domain.Configuration;
using ManageSystem.Services.Configuration;
using ManageSystem.Admin.Models.Configuration;

namespace ManageSystem.Admin.Controllers
{

    public class WeixinController : AdminBaseController
    {
        private readonly IActionLogService actionLogService;
        private readonly IWeixinMenuService weixinMenuService;
        private readonly IArticleService articleService;
        private readonly ISettingService settingService;
        private readonly IWeixinUserService weixinUserService;


        /// <summary>
        /// 微信关注的配置key
        /// </summary>
        private static string SubscribeConfigKey = "weixin.subscribe";

        public WeixinController(
            IActionLogService _actionLogService,
            IWeixinMenuService _weixinMenuService,
              IArticleService _articleService,
             ISettingService _settingService,
             IWeixinUserService _weixinUserService
        )
        {
            this.actionLogService = _actionLogService;
            this.weixinMenuService = _weixinMenuService;
            this.articleService = _articleService;
            this.settingService = _settingService;
            this.weixinUserService = _weixinUserService;
        }

        #region 微信菜单

        /// <summary>
        /// 微信菜单 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinMenuList()
        {
            //微信规定 菜单只能有2级，顶级3个，二级可以有5个
            List<WeixinMenuModel> list = new List<WeixinMenuModel>();

            var parentList = this.weixinMenuService.QueryPage(m => m.ParentId == 0 && m.Mark != 0).OrderBy(m => m.Sort);
            foreach (var item in parentList)
            {
                WeixinMenuModel m = item.ToModel();
                m.TypeName = ((WeixinMenuType)item.Type).GetDescription();

                m.ChildList = this.weixinMenuService.QueryPage(c => c.ParentId == item.Id && m.Mark != 0).OrderBy(c => c.Sort)
                    .Select(x => x.ToModel()).ToList();

                int index = 0;
                foreach (var article in m.ChildList)
                {
                    //设置文章的集合
                    if (article.Type == (int)WeixinMenuType.click && !string.IsNullOrWhiteSpace(article.Value))
                    {
                        var articleList = this.articleService.Query(article.Value);
                        if (articleList == null || !articleList.Any())
                        {
                            article.ArticleItemList = new List<ArticleItemModel>();
                        }
                        else
                        {
                            article.ArticleItemList = articleList.Select(x =>
                            {
                                return new ArticleItemModel()
                                {
                                    ArticleId = x.Id.ToString(),
                                    Name = x.Name,
                                    Sort = ++index
                                };
                            }).ToList();
                        }

                        article.Value = "";
                    }
                }

                foreach (var node in m.ChildList)
                    node.TypeName = ((WeixinMenuType)node.Type).GetDescription();

                list.Add(m);
            }

            return View(list);
        }

        /// <summary>
        /// 微信菜单 设置创建页面的数据
        /// </summary>
        /// <returns></returns>
        public WeixinMenuModel SetWeixinMenuCreateData()
        {
            WeixinMenuModel model = new WeixinMenuModel();
            model.MenuTypeList = WeixinMenuType.click.ToSelectList(false, false).ToList();
            model.Sort = 100;
            model.Type = (int)WeixinMenuType.click;
            model.ArticleItemList = new List<ArticleItemModel>();

            this.PrepareAllFunctionModel(model);

            string parentId = this.Request.QueryString["parentId"];
            if (!string.IsNullOrWhiteSpace(parentId))
                model.ParentId = long.Parse(parentId);

            return model;
        }

        /// <summary>
        /// 微信菜单 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinMenuCreate()
        {
            return View(this.SetWeixinMenuCreateData());

        }

        /// <summary>
        /// 加载所有的功能集合
        /// </summary>
        /// <param name="model"></param>
        [NonAction]
        protected virtual void PrepareAllFunctionModel(WeixinMenuModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.MenuList.Add(new SelectListItem
            {
                Text = "顶级功能",
                Value = "0"
            });

            var functionList = this.weixinMenuService.Query(null, 0, false, p => p.Sort);

            string parentId = this.Request.QueryString["parentId"];
            parentId = string.IsNullOrWhiteSpace(parentId) ? "" : parentId;

            foreach (var c in functionList)
            {
                model.MenuList.Add(new SelectListItem
                {
                    Text = c.GetFormattedBreadCrumb(functionList),
                    Value = c.Id.ToString(),
                    Selected = (parentId.Equals(c.Id.ToString()))
                });
            }
        }

        /// <summary>
        /// 微信菜单 保存数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WeixinMenuCreate(WeixinMenuModel model)
        {
            if (this.weixinMenuService.Count(m => m.Key.Equals(model.Key) && m.Mark > 0) > 0)
                ModelState.AddModelError("Key", "Key 已经存在");

            if (model.ParentId == 0)
            {
                //顶级菜单，只能有3个
                if (this.weixinMenuService.Count(m => m.ParentId == 0 && m.Mark > 0) > 2)
                    ModelState.AddModelError("ParentId", "微信规定顶级菜单只能有3个");
            }
            else
            {
                //二级菜单，只能有5个
                if (this.weixinMenuService.Count(m => m.ParentId == model.ParentId && m.Mark > 0) > 4)
                    ModelState.AddModelError("ParentId", "微信规定二级菜单只能有5个");
            }

            if (ModelState.IsValid)
            {
                this.SetMenuValue(model);
                
                var entity = model.ToEntity();
                this.weixinMenuService.Insert(entity);

                string logContent = "添加微信菜单成功，菜单名称：" + model.Name;
                base.InsetActionLog(ActionType.Create, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("WeixinMenuCreate", new { result = "ok" });
            }

            return View(this.SetWeixinMenuCreateData());
        }

        /// <summary>
        /// 根据菜单的类型设置微信菜单的值
        /// </summary>
        /// <param name="model"></param>
        private void SetMenuValue(WeixinMenuModel model)
        {
            switch ((WeixinMenuType)model.Type)
            {
                case WeixinMenuType.click:
                    if (!string.IsNullOrWhiteSpace(model.Value))
                    {
                        //菜单类型是文章推送并且选择了文章。这里进行排序和获取id集合
                        List<ArticleItemModel> valueList = JsonConvert.DeserializeObject<List<ArticleItemModel>>(model.Value);
                        string temp = "";
                        if (valueList != null && valueList.Any())
                        {
                            var tempList = valueList.OrderBy(m => m.Sort);
                            foreach (var item in tempList)
                                temp += item.ArticleId + ",";
                        }
                        model.Value = temp.TrimEnd(',');
                    }
                    break;
                case WeixinMenuType.view:
                    model.Value = model.Value;
                    break;
                case WeixinMenuType.text:
                    model.Value = model.Value;
                    break;
            }

        }
        /// <summary>
        /// 微信菜单 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public WeixinMenuModel SetWeixinMenuEditData(long id)
        {
            var entity = this.weixinMenuService.QueryEntity(id);

            var model = entity.ToModel();
            model.MenuTypeList = WeixinMenuType.click.ToSelectList(false, false).ToList();

            int index = 0;
            if (model.Type == (int)WeixinMenuType.click && !string.IsNullOrWhiteSpace(model.Value))
            {
                var articleList = this.articleService.Query(model.Value);
                if (articleList == null || !articleList.Any())
                {
                    model.ArticleItemList = new List<ArticleItemModel>();
                }
                else
                {
                    model.ArticleItemList = articleList.Select(x =>
                    {
                        return new ArticleItemModel()
                        {
                            ArticleId = x.Id.ToString(),
                            Name = x.Name,
                            Sort = ++index
                        };
                    }).ToList();
                }

                model.Value = "";
            }
            else
            {
                model.ArticleItemList = new List<ArticleItemModel>();
            }

            this.PrepareAllFunctionModel(model);

            return model;
        }

        /// <summary>
        /// 微信菜单 编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinMenuEdit(long id)
        {
            return this.View(this.SetWeixinMenuEditData(id));
        }

        /// <summary>
        /// 微信菜单 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WeixinMenuEdit(WeixinMenuModel model)
        {
            if (this.weixinMenuService.Count(m => m.Key.Equals(model.Key) && m.Id != model.Id && m.Mark > 0) > 0)
                ModelState.AddModelError("Key", "Key 已经存在");

            if (ModelState.IsValid)
            {
                this.SetMenuValue(model);

                var entity = this.weixinMenuService.QueryEntity(model.Id);
                base.SetDefaultValue(model, entity);
                entity = model.ToEntity(entity);

                this.weixinMenuService.Update(entity);

                string logContent = "修改微信菜单 ";
                base.InsetActionLog(ActionType.Edit, logContent);
                base.SuccessNotification(logContent);

                return this.RedirectToAction("WeixinMenuEdit", new { result = "ok" });
            }
            return this.View(this.SetWeixinMenuEditData(model.Id));
        }



        /// <summary>
        /// 微信菜单 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinMenuView(long id)
        {
            return this.View(this.SetWeixinMenuEditData(id));
        }


        /// <summary>
        /// 微信菜单 删除
        /// </summary>
        /// <param name="selectedIds">需要删除的id集合，使用英文逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeixinMenuDelete(string selectedIds)
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                base.ErrorNotification("请选择需要删除的数据");
                return this.RedirectToAction("WeixinMenuList");
            }

            this.weixinMenuService.Delete(selectedIds);

            string logContent = "【手动】删除微信菜单，删除的id集合:" + selectedIds;
            base.InsetActionLog(ActionType.Delete, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("WeixinMenuCreate");
        }


        /// <summary>
        /// 同步菜单数据到微信微信
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult SyncWeixinMenu()
        {
            try
            {

                string resul = this.weixinMenuService.CreateMenu();

                return this.Content(resul);

            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);
            }

        }




        #endregion

        #region  微信关注推送

        /// <summary>
        /// 微信关注推送
        /// </summary>
        /// <returns></returns>
        public ActionResult SubscribeSet()
        {
            Setting model = this.SettingService.QueryEntity(SubscribeConfigKey);
            if (model == null)
            {
                model = new Setting();
                model.Name = SubscribeConfigKey;
                model.Value = "";
                model.Name = "微信关注推送";
                model.IsAdmin = false;
                model.IsCache = true;
                model.Describe = "";

                this.SettingService.Insert(model);
            }

            int index = 0;
            var articleList = this.articleService.Query(model.Value);
            if (articleList == null || !articleList.Any())
            {
                this.ViewBag.ArticleItemList = new List<ArticleItemModel>();
                model.Value = model.Value;
            }
            else
            {
                this.ViewBag.ArticleItemList = articleList.Select(x =>
                {
                    return new ArticleItemModel()
                    {
                        ArticleId = x.Id.ToString(),
                        Name = x.Name,
                        Sort = ++index
                    };
                }).ToList();
            }

            return View(model.ToModel());
        }


        /// <summary>
        /// 微信菜单 保存编辑数据
        /// </summary>
        /// <param name="model">保存对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubscribeSet(SettingModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Value))
            {
                if (model.Describe.Equals("2"))
                {
                    //推送文章
                    string temp = "";

                    //菜单类型是文章推送并且选择了文章。这里进行排序和获取id集合
                    List<ArticleItemModel> valueList = JsonConvert.DeserializeObject<List<ArticleItemModel>>(model.Value);
                    if (valueList != null && valueList.Any())
                    {
                        if (valueList.Count > 10)
                        {
                            base.ErrorNotification("保存失败，微信推送文章个数不能大于10个！");
                            return this.RedirectToAction("SubscribeSet");
                        }

                        var tempList = valueList.OrderBy(m => m.Sort);
                        foreach (var item in tempList)
                            temp += item.ArticleId + ",";
                    }

                    model.Value = temp.TrimEnd(',');
                }
                else
                {
                    //推送文字
                    if (!string.IsNullOrWhiteSpace(model.Value))
                    {
                        model.Value = model.Value.Replace("\"", "'");
                        model.Value = model.Value.Trim();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(model.Value))
                model.Value = "";

            var entity = this.SettingService.QueryEntity(model.Id);

            entity.Value = model.Value;
            entity.Describe = model.Describe;

            base.SetDefaultValue(model, entity);

            this.SettingService.Update(entity);

            string logContent = "修改微信关注推送内容成功";
            base.InsetActionLog(ActionType.Edit, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("SubscribeSet");
        }

        #endregion

        #region 关注用户

        /// <summary>
        /// 关注用户 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinUserList()
        {
            WeixinUserModel model = new WeixinUserModel();
            model.SubscribeList = new List<SelectListItem>() {
                   new SelectListItem() { Text = "全部状态", Value = "" },
                    new SelectListItem() { Text = "关注中", Value = "1" },
                    new SelectListItem() { Text = "取消关注", Value = "0" }
            };

            this.ViewBag.SubscribeCount = this.weixinUserService.Count(m => m.Mark > 0 && m.Subscribe); //总关注人数
            this.ViewBag.UnSubscribeCount = this.weixinUserService.Count(m => m.Mark > 0 && !m.Subscribe); //总取消关注人数

            DateTime startTime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " 00:00");
            DateTime endTime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd") + " 23:59");
            this.ViewBag.NowSubscribeCount = this.weixinUserService.Count(m => m.Mark > 0 && m.Subscribe && m.SubscribeTime >= startTime && m.SubscribeTime <= endTime); //今日关注人数
            this.ViewBag.NowUnSubscribeCount = this.weixinUserService.Count(m => m.Mark > 0 && !m.Subscribe && m.SubscribeTime >= startTime && m.SubscribeTime <= endTime); //今日取消关注人数

            return View(model);
        }

        /// <summary>
        /// 关注用户 获取数据
        /// </summary>
        /// <param name="command">数据源对象，分页等数据</param>
        /// <param name="model">查询数据对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeixinUserList(DataSourceRequest command, WeixinUserModel model)
        {
            //获得数据
            var list = this.weixinUserService.QueryPage(model.Openid, model.SubscribeValue, model.StartTime, model.EndTime, command.Page - 1, command.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = list.Select(x =>
                {
                    return new
                    {
                        Id = x.Id.ToString(),
                        Openid = x.Openid,
                        Subscribe = x.Subscribe ? "关注中" : "取消关注",
                        Sex = ((WeixinUserSex)x.Sex).GetDescription(),
                        Name=x.Name,
                        SubscribeTime = x.SubscribeTime,
                        UnsubscribeTime = x.Subscribe ? "" : x.UnsubscribeTime.ToString("yyyy-MM-dd HH:mm")
                    };
                }),
                Total = list.TotalCount
            };
            return new JsonResult { Data = gridModel };
        }

        private string GetSex(int sex)
        {
            if (sex == 1) return "男性";
            if (sex == 2) return "女性";
            return "未知";
        }


        /// <summary>
        /// 关注用户 设置编辑页面的数据
        /// </summary>
        /// <returns></returns>
        public WeixinUserModel SetWeixinUserEditData(long id)
        {
            var entity = this.weixinUserService.QueryEntity(id);

            var model = entity.ToModel();
            model.SubscribeValue = model.Subscribe ? "关注中" : "取消关注";
            return model;
        }


        /// <summary>
        /// 关注用户 查看
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinUserView(long id)
        {
            return this.View(this.SetWeixinUserEditData(id));
        }


        #endregion

        #region 微信设置

        /// <summary>
        /// 微信设置
        /// </summary>
        /// <returns></returns>
        public ActionResult WeixinSetting()
        {
            return View(this.SetWeixinSettingData());
        }

        private WeixinSettingModel SetWeixinSettingData()
        {
            WeixinSettingModel model = new WeixinSettingModel();
           string temp = WeixinSettingService.GetWeixinAccountType();
            if (!string.IsNullOrWhiteSpace(temp))
            {
                model.AccountType = int.Parse(temp);
                model.AccountTypeList = ((WeixinAccountType)model.AccountType).ToSelectList(true, false).ToList();
            }
            else
            {
                model.AccountTypeList = WeixinAccountType.Enterprise.ToSelectList(false, false).ToList();
            }
           
            model.WeixinToken = WeixinSettingService.GetToken(); //微信Token
            model.WeixinAppId = WeixinSettingService.GetAppId();  //微信AppId
            model.WeixinAppSecret = WeixinSettingService.GetAppSecret();  //微信AppSecret
            model.WeixinPayMchId = WeixinSettingService.GetWeixinPayMchId();   // 微信支付商户id
            model.WeixinPayKey = WeixinSettingService.GetWeixinPayKey(); // 商户支付密钥Key
            model.WeixinPayNotify = WeixinSettingService.GetWeixinPayNotify(); //微信支付通知地址

            return model;

        }

        /// <summary>
        /// 保存微信设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeixinSetting(WeixinSettingModel model)
        {
            this.SettingService.UpdateValue("weixin.app.token", model.WeixinToken);//微信Token
            this.SettingService.UpdateValue("weixin.app.id", model.WeixinAppId);//微信AppId
            this.SettingService.UpdateValue("weixin.app.secret", model.WeixinAppSecret);//微信AppSecret
            this.SettingService.UpdateValue("weixin.account.type", model.AccountType.ToString());//微信帐号的类型

            this.SettingService.UpdateValue("weixin.pay.key", model.WeixinPayKey);// 商户支付密钥Key
            this.SettingService.UpdateValue("weixin.pay.mchid", model.WeixinPayMchId);//微信支付商户id
            this.SettingService.UpdateValue("weixin.pay.notify", model.WeixinPayNotify);//微信支付通知地址

            string logContent = "修改微信设置完成";
            base.InsetActionLog(ActionType.Edit, logContent);
            base.SuccessNotification(logContent);

            return this.RedirectToAction("WeixinSetting");
        }
        #endregion


    }
}
