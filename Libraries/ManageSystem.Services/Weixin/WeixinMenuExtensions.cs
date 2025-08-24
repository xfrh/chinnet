using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Weixin
{
  public static class WeixinMenuExtensions
    {

        /// <summary>
        /// 对功能进行排序
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="parentId">Parent category identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        public static IList<WeixinMenu> SortFunctionForTree(this IList<WeixinMenu> source, long parentId = 0, bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var result = new List<WeixinMenu>();

            foreach (var cat in source.Where(c => c.ParentId == parentId).ToList())
            {
                result.Add(cat);
                result.AddRange(SortFunctionForTree(source, cat.Id, true));
            }
            if (!ignoreCategoriesWithoutExistingParent && result.Count != source.Count)
            {
                //find categories without parent in provided category source and insert them into result
                foreach (var cat in source)
                    if (result.FirstOrDefault(x => x.Id == cat.Id) == null)
                        result.Add(cat);
            }
            return result;
        }



        /// <summary>
        /// 把格式化的功能集合
        /// </summary>
        /// <param name="model">Function</param>
        /// <param name="functionService">Function service</param>
        /// <param name="separator">Separator</param>
        /// <returns></returns>
        public static string GetFormattedBreadCrumb(this WeixinMenu model,
           IList<WeixinMenu> allFunction, string separator = ">>")
        {
            string result = string.Empty;

            var breadcrumb = GetFunctionBreadCrumb(model, allFunction);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                string functionName = breadcrumb[i].Name;
                result = String.IsNullOrEmpty(result)
                    ? functionName
                    : string.Format("{0} {1} {2}", result, separator, functionName);
            }

            return result;
        }


        /// <summary>
        /// 获取功能集合
        /// </summary>
        /// <param name="model">Function</param>
        /// <param name="functionService">Function service</param>
        /// <returns></returns>
        public static IList<WeixinMenu> GetFunctionBreadCrumb(this WeixinMenu model,
              IList<WeixinMenu> allFunction)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var result = new List<WeixinMenu>();

            //用于防止循环引用
            var alreadyProcessedCategoryIds = new List<long>();

            while (model != null && //非空的 
                model.Mark >0 && //未删除的
                !alreadyProcessedCategoryIds.Contains(model.Id)) //Available
            {
                result.Add(model);

                alreadyProcessedCategoryIds.Add(model.Id);

                model = (from c in allFunction
                         where c.Id == model.ParentId
                            select c).FirstOrDefault();
            }
            result.Reverse();
            return result;
        }


        /// <summary>
        /// 获取功能面包屑导航的名称
        /// </summary>
        /// <param name="model">Function</param>
        /// <param name="functionService">Function service</param>
        /// <returns></returns>
        public static string GetFormattedBreadCrumb(this WeixinMenu model,
            IWeixinMenuService functionService,
            string separator = ">>", int languageId = 0)
        {
            string result = string.Empty;

            var breadcrumb = GetFunctionBreadCrumb(model, functionService);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var name = breadcrumb[i].Name;
                result = String.IsNullOrEmpty(result)
                    ? name
                    : string.Format("{0} {1} {2}", result, separator, name);
            }

            return result;
        }

        /// <summary>
        /// 获取功能的面包屑集合
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="categoryService">Category service</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns></returns>
        public static IList<WeixinMenu> GetFunctionBreadCrumb(this WeixinMenu model,
           IWeixinMenuService functionService)
        {
            if (model == null)
                throw new ArgumentNullException("WeixinMenu");

            var result = new List<WeixinMenu>();

            //used to prevent circular references
            var alreadyProcessedCategoryIds = new List<long>();

            while (model != null && //not null
                model.Mark>0 && //not deleted
                !alreadyProcessedCategoryIds.Contains(model.Id)) //prevent circular references
            {
                result.Add(model);

                alreadyProcessedCategoryIds.Add(model.Id);

                model = functionService.QueryEntity(model.ParentId);
            }
            result.Reverse();
            return result;
        }

    }
}
