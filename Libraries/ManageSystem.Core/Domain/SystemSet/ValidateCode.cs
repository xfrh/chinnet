using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.SystemSet
{
    /// <summary>
    /// 实体类 ，数据库表名：ValidateCode 
    /// </summary>
    public partial class ValidateCode : BaseEntity
	{
        /// <summary>
        /// 验证码
        /// <summary>
        public String Code { get; set; }

        /// <summary>
        /// 手机号码或者邮箱地址
        /// <summary>
        public String Value { get; set; }

        /// <summary>
        /// 验证码类型： 1：短信  2：邮件
        /// <summary>
        public Int32 Type { get; set; }

        /// <summary>
        ///数据来源： 1：后台  2：微信
        /// <summary>
        public Int32 Source { get; set; }

        /// <summary>
        /// 开始时间
        /// <summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 过期时间
        /// <summary>
        public DateTime OutTime { get; set; }

    }
}
