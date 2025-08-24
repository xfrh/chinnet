using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Articles
{
  

    /// <summary>
     /// 文章状态
     /// </summary>
    public enum ArticleState
    {
        [Description("草稿")]
        Draft = 1,
        [Description("正常")]
        Normal = 2,
        [Description("禁用")]
        Disable = 3
    }

}
