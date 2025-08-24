using ManageSystem.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace ManageSystem.Web.Models.Researches
{
    public class ResearchApplyModel: BaseEntityModel
    {

        /// <summary>
        /// 报名用户的id
        /// <summary>
        public long MemberId { get; set; }
        /// <summary>
        /// 报名用户的姓名
        /// <summary>
        public String MemberName { get; set; }
        /// <summary>
        /// 科研合作的id
        /// <summary>
        public long ResearchId { get; set; }
        /// <summary>
        /// 科研合作的名称
        /// <summary>
        public String ResearchName { get; set; }
        /// <summary>
        /// 报名姓名
        /// <summary>
        public String Name { get; set; }
        /// <summary>
        /// 邮箱地址
        /// <summary>
        public String Email { get; set; }
        /// <summary>
        /// 手机号码
        /// <summary>
        public String Phone { get; set; }
        /// <summary>
        /// 用户备注
        /// <summary>
        public String Remark { get; set; }
        /// <summary>
        /// 报名人数
        /// <summary>
        public Int32 Count { get; set; }
        /// <summary>
        /// 申请状态
        /// <summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 申请状态
        /// <summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 状态变更时间
        /// <summary>
        public DateTime StatusTime { get; set; }

    }

    /// <summary>
    /// 会员中心 科研合作管理  科研的报名申请记录
    /// </summary>
    [Serializable]
    public class ResearchDetailApplyModel
    {
        /// <summary>
        /// 搜索状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        public IList<SelectListItem> StatusList { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public String SearchKey { get; set; }

        /// <summary>
        /// 信息动态数据
        /// </summary>
        public ResearchModel Research { get; set; }

        /// <summary>
        /// 分页查询的数据
        /// </summary>
        public PagedList<ResearchApplyModel> ApplyPageList { get; set; }


    }
}