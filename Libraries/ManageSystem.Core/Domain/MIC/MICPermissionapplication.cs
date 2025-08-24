using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.MIC
{
    public partial class MICPermissionapplication:BaseEntity
    {
   
        /// <summary>
        /// 关联用户表id
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 省市
        /// </summary>
        public string Provincesandcities { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime Applicationtime { get; set; }

        /// <summary>
        /// 申请状态 1:已同意 0:不同意
        /// </summary>
        public int Applicationstate { get; set; }
    }

}
