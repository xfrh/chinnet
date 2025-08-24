using ManageSystem.Core.Domain.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Sate
{
    public partial class SatelliteUser :BaseEntity
    {

        private ICollection<Role> _roleList;
        private ICollection<Function> _functionList;
        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String PassWord { get; set; }

        /// <summary>
        /// SatelliteId
        /// </summary>
        public long SatelliteId { get; set; }

        public Int32 State { get; set; }
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
