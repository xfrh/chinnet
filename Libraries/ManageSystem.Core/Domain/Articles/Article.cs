using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Articles
{
    /// <summary>
    /// 实体类 ，数据库表名：Article 
    /// </summary>
    public partial class Article : BaseEntity
    {
        /// <summary>
        /// 文章名称
        /// <summary>
        public String Name { get; set; }
        /// <summary>
        /// 文章状态
        /// <summary>
        public Int32 State { get; set; }
        /// <summary>
        /// 文章内容
        /// <summary>
        public String Content { get; set; }

        /// <summary>
        /// 文章简短内容
        /// <summary>
        public String ShortContent { get; set; }

        /// <summary>
        /// 封面图片
        /// <summary>
        public String CoverImage { get; set; }
        /// <summary>
        /// 文章作者
        /// <summary>
        public String Author { get; set; }
        /// <summary>
        /// 发布时间
        /// <summary>
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 文章分类id
        /// </summary>
        public long ArticleTypeId { get; set; }

        /// <summary>
        /// 被赞次数  （同步ArticlePraise表的数据）
        /// </summary>
        public int PraiseCount { get; set; }

        /// <summary>
        ///  打赏总次数（同步ArticleReward表的数据）
        /// </summary>
        public int RewardCount { get; set; }

        /// <summary>
        /// 打赏的总金额（同步ArticleReward表的数据）
        /// </summary>
        public decimal RewardAmount { get; set; }

        /// <summary>
        /// 查看次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 所属用户Id
        /// </summary>
        public long UserinfoId { get; set; }

        /// <summary>
        ///  二维码图片地址
        /// </summary>
        public string CodeImage { get; set; }

        /// <summary>
        /// 是否可以报名
        /// </summary>
        public bool IsApply { get; set; }

        /// <summary>
        /// 是否需要支付，支付以后才能查看文章及附件
        /// </summary>
        public bool IsPay { get; set; }

        /// <summary>
        /// 需要支付最小金额（如果需要支付）
        /// </summary>
        public decimal PayPrice { get; set; }

    }
}
