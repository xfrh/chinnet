using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Services.Common;
using ManageSystem.Core.Infrastructure;

namespace ManageSystem.Services.Products
{
    /// <summary>
    /// 商品的扩展类
    /// </summary>
    public static partial class ProductExtensions
    {
        /// <summary>
        ///  获取商品封面图片的完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public static string GetProductImage(string path,bool isDefault = true)
        {
            if (!string.IsNullOrWhiteSpace(path)) return  path;

            return isDefault ? @"/Content/upload/Product/default.jpg" : "";
        }


    }
}
