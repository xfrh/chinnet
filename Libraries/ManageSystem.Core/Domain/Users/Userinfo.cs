using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Core.Domain.Users
{
    /// <summary>
    /// 实体类 ，数据库表名：Userinfo 
    /// </summary>
    public partial class Userinfo : Account
    {
        private ICollection<Role> _roleList;
        private ICollection<Function> _functionList;

      
        /// <summary>
        /// 用户昵称
        /// <summary>
        public String NickName { get; set; }
        /// <summary>
        /// 手机号码
        /// <summary>
        public String Phone { get; set; }
        /// <summary>
        /// 邮箱地址
        /// <summary>
        public String Email { get; set; }
        /// <summary>
        /// 出生日期
        /// <summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 状态
        /// <summary>
        public Int32 State { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserinfoState UserinfoState
        {
            get
            {
                return (UserinfoState)this.State;
            }
            set
            {
                this.State = (int)value;
            }
        }

        /// <summary>
        /// 性别
        /// <summary>
        public Int32 Sex { get; set; }

        /// <summary>
        /// 默认跳转路径
        /// </summary>
        public String Url { get; set; }
        /// <summary>
        /// 最后登录日期
        /// </summary>
        public DateTime LastLoginDate { get; set; }
        
        /// <summary>
        /// 用户所属角色集合，一个用户可以拥有多个角色
        /// </summary>
        public virtual ICollection<Role> RoleList
        {
            get { return _roleList ?? (_roleList = new List<Role>()); }
            protected set { _roleList = value; }
        }

        /// <summary>
		/// 用户可用功能集合，在登录的时候填充数据
		/// <summary>
        public ICollection<Function> FunctionList
        {
            get { return _functionList ?? (_functionList = new List<Function>()); }
            set { _functionList = value; }
        }
    }
}
