using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Products
{
    /// <summary>
    /// 商品状态
    /// </summary>
    public enum ProductStatusEnum
    {
        [Description("下架")]
        Soldout = 1,
        [Description("上架")]
        Putaway = 2
    }

}
