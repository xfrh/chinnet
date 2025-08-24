using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Messages
{
    /// <summary>
    /// 发送邮件记录
    /// </summary>
    public class MessageEmail : BaseEntity
    {
        /// <summary>
        /// 邮箱地址
        /// <summary>
        public string Email { set; get; }

        /// <summary>
        /// 邮件标题
        /// <summary>
        public string Title { set; get; }

        /// <summary>
        /// 邮件内容
        /// <summary>
        public String Content { set; get; }

        /// <summary>
        /// 发送类型： 1、手动   2、自动。手动则需要直接调用接口，自动是系统服务运行
        /// <summary>
        public int SendType { get; set; }

        /// <summary>
        /// 短信发送场景类型：1：登录验证码  2：注册验证码  3：找回密码验证码  4：应用发放提醒
        /// <summary>
        public string SceneType { get; set; }

        /// <summary>
        /// 数据来源： 1：后台  2：微信  3：网站
        /// <summary>
        public string Source { set; get; }

        /// <summary>
        /// 状态：1、待发送   2：已发送   3：失败
        /// <summary>
        public Int32 Status { set; get; }

        /// <summary>
        /// 所属用户Id，Member_Member 表数据
        /// <summary>
        public long MemberId { set; get; }

        /// <summary>
        /// 所属用户姓名，Member_Member 表数据
        /// <summary>
        public String MemberName { set; get; }

        /// <summary>
        /// 备注信息
        /// <summary>
        public String Remark { set; get; }

    }
}
