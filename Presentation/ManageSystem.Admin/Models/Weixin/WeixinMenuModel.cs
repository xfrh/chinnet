using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FluentValidation.Attributes;
using ManageSystem.Framework;
using ManageSystem.Framework.Mvc;
using ManageSystem.Admin.Validators.Weixin;
using System.Web.Mvc;

namespace ManageSystem.Admin.Models.Weixin
{
    /// <summary>
    /// 模型类 ，数据库表名：WeixinMenu 
    /// </summary>
    [Validator(typeof(WeixinMenuValidator))]
    public partial class WeixinMenuModel : BaseEntityModel
    {
        public WeixinMenuModel()
        {
            MenuList = new List<SelectListItem>();
        }

        /// <summary>
        /// 菜单名称
        /// <summary>
        [HtmlDisplayAttribute("菜单名称", "菜单名称")]
        public String Name { get; set; }

        /// <summary>
        /// 菜单类型
        /// <summary>
        [HtmlDisplayAttribute("菜单类型", "菜单类型")]
        public Int32 Type { get; set; }

        public string TypeName { get; set; }

        [HtmlDisplayAttribute("菜单类型", "菜单类型")]
        public IList<SelectListItem> MenuTypeList { get; set; }

        /// <summary>
        /// 菜单关键字
        /// <summary>
        [HtmlDisplayAttribute("菜单关键字", "菜单关键字")]
        public String Key { get; set; }

        /// <summary>
        /// 排序编号
        /// <summary>
        [HtmlDisplayAttribute("排序编号", "排序编号")]
        public Int32 Sort { get; set; }


        /// <summary>
        /// 上级id
        /// <summary>
        [HtmlDisplayAttribute("上级菜单", "上级菜单")]
        public long ParentId { get; set; }

        [HtmlDisplayAttribute("上级菜单", "上级菜单")]
        public IList<SelectListItem> MenuList { get; set; }

        /// <summary>
        /// 菜单值
        /// <summary>
        [HtmlDisplayAttribute("菜单值", "菜单值")]
        public String Value { get; set; }

        /// <summary>
        /// 所对应的子菜单集合
        /// </summary>
        public List<WeixinMenuModel> ChildList { get; set; }


        /// <summary>
        /// 对应的文章类型的，只用于添加或者是修改的时候，提交保存的数据
        /// </summary>
        public ICollection<ArticleItemModel> ArticleItemList { get; set; }

        /// <summary>
        /// 对应的文章类型的，只用于添加或者是修改的时候，提交保存的数据
        /// </summary>
        public string ArticleItemListStr { get; set; }

    }


    /// <summary>
    /// 添加或者修改的时候类型如果选择是文章
    /// </summary>
    public class ArticleItemModel
    {

        /// <summary>
        /// 文章名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 对应的排序id
        /// </summary>

        public int Sort { get; set; }


        /// <summary>
        /// 对应的文章id
        /// </summary>
        public string ArticleId { get; set; }
    }


}
