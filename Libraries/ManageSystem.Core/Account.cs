using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core
{
    /// <summary>
    /// 帐号基类
    /// </summary>
    public class Account: BaseEntity
    {
        
        /// <summary>
        /// 登录帐号
        /// <summary>
        public String LoginId { get; set; }

        /// <summary>
        /// 登录密码
        /// <summary>
        public String Password { get; set; }

        /// <summary>
        /// 姓名
        /// <summary>
        public String Name { get; set; }

    }
}
