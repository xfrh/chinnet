using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Weixin;
using System.Linq.Expressions;
using Senparc.Weixin.MP.CommonAPIs;
using ManageSystem.Services.Common;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP;
using ManageSystem.Services.Configuration;

namespace ManageSystem.Services.Weixin
{
    /// <summary>
    /// 操作类 ，数据库表名：WeixinMenu 
    /// </summary>
    public partial class WeixinMenuService : BaseService<WeixinMenu>, IWeixinMenuService
    {
        protected readonly ISettingService SettingService;
        public WeixinMenuService(
            IRepository<WeixinMenu> repository,
            ISettingService settingService
            ) : base(repository)
        {
            this.SettingService = settingService;
        }


        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        public string CreateMenu()
        {
            try
            {
               
                WxJsonResult result = CommonApi.CreateMenu(this.SettingService.QueryValue<string>("weixin.app.id"), this.GetWeixinMenu());
                if (result.errcode == Senparc.Weixin.ReturnCode.请求成功)
                    return "更新微信菜单成功，请注意缓存时间";

                return "更新失败，失败原因："+result.errcode+"   "+result.errmsg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取微信菜单的json数据
        /// </summary>
        /// <returns></returns>
        private ButtonGroup GetWeixinMenu()
        {
            Dictionary<WeixinMenu, List<WeixinMenu>> list = new Dictionary<WeixinMenu, List<WeixinMenu>>();

            var parentList = this.Query(m => m.ParentId == 0 && m.Mark > 0).OrderBy(m=>m.Sort);
            foreach (var item in parentList)
            {
                var nodeList = this.Query(m => m.ParentId == item.Id && m.Mark > 0).OrderBy(m => m.Sort).ToList();

                list.Add(item, nodeList);
            }


            ButtonGroup bg = new ButtonGroup();
            foreach (var item in list.Keys)
            {
                var nodeList = list[item];
                if (nodeList != null && nodeList.Any())
                {
                    var subButton = new SubButton()
                    {
                        name = item.Name
                    };

                    //有子节点
                    foreach (var node in nodeList)
                        subButton.sub_button.Add(this.GetSingleButton(node));

                    bg.button.Add(subButton);
                }
                else
                {
                    //没有子节点
                    bg.button.Add(this.GetMenuItem(item));
                }
            }

            return bg;

        }

        /// <summary>
        /// 一级菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private BaseButton GetMenuItem(WeixinMenu menu)
        {
            BaseButton result = null;

            switch ((WeixinMenuType)menu.Type)
            {
                case WeixinMenuType.click:
                    result =new SingleClickButton()
                    {
                        name = menu.Name,
                        key = menu.Key,
                        type = ButtonType.click.ToString(),
                    };
                    break;
                case WeixinMenuType.view:
                    result = new SingleViewButton()
                    {
                        url = menu.Value,
                        name = menu.Name,
                    };
                    break;
                case WeixinMenuType.text:
                    result = new SingleClickButton()
                    {
                        name = menu.Name,
                        key = menu.Key,
                        type = ButtonType.click.ToString(),
                    };
                    break;
            }

            return result;
        }
        
        /// <summary>
        /// 二级菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private SingleButton GetSingleButton(WeixinMenu menu)
        {
            SingleButton result = null;

            switch ((WeixinMenuType)menu.Type)
            {
                case WeixinMenuType.click:
                    result = new SingleClickButton()
                    {
                        name = menu.Name,
                        key = menu.Key,
                        type = ButtonType.click.ToString(),
                    };
                    break;
                case WeixinMenuType.view:
                    result = new SingleViewButton()
                    {
                        url = menu.Value,
                        name = menu.Name,
                    };
                    break;
                case WeixinMenuType.text:
                    result = new SingleClickButton()
                    {
                        name = menu.Name,
                        key = menu.Key,
                        type = ButtonType.click.ToString(),
                    };
                    break;
            }

            return result;
        }


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <param name="topCount">指定查询的条数，小于等于0表示不使用</param>
        /// <param name="isOrderByDesc">是否倒序排列数据，相当于SQL：ORDER BY DESC  </param>
        /// <param name="orderBy">排序字段，Lambda表达式 </param>
        /// <returns>返回对象集合</returns>
        public override List<WeixinMenu> Query(Expression<Func<WeixinMenu, bool>> where, int topCount = 0, bool isOrderByDesc = false, Expression<Func<WeixinMenu, int?>> orderBy = null)
        {
            if (where == null) where = p => true;
            IQueryable<WeixinMenu> query = this._repository.Table.Where(where).Where(m => m.Mark > 0);

            if (orderBy == null)
            {
                //不启用排序
                query.OrderBy(m => m.Id);
            }
            else
            {
                //启用排序
                if (!isOrderByDesc && orderBy != null) query = query.OrderBy(m => m.ParentId).ThenBy(orderBy);
                else if (isOrderByDesc && orderBy != null) query = query.OrderByDescending(m => m.ParentId).ThenByDescending(orderBy);
            }

            if (topCount > 0) query = query.Take<WeixinMenu>(topCount);

            if (query == null) return null;

            var unsortedCategories = query.ToList();
            //sort categories
            var sortedCategories = unsortedCategories.SortFunctionForTree();

            return sortedCategories.ToList<WeixinMenu>();
        }



        /// <summary>
        /// 根据 菜单key获取菜单的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public WeixinMenu QueryEntity(string key)
        {
            return this.QueryEntity(m => m.Key.Equals(key.ToLower()));
        }


    }
}
