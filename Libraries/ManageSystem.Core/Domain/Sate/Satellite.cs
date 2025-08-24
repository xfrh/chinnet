using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Sate
{
    public partial class Satellite :BaseEntity
    {
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string PlaceOfWork { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public string Office { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 负责人手机号码
        /// </summary>
        public string ChargePhoneNumber { get; set; }

        /// <summary>
        /// 负责人微信号码
        /// </summary>
        public string ChargeWechatNumber { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ChargeName { get; set; }

        /// <summary>
        /// 负责人email
        /// </summary>
        public string ChargeEmail { get; set; }

        /// <summary>
        /// 卫星网名称
        /// </summary>
        public string SatelliteName { get; set; }

        /// <summary>
        /// 成员单位数量
        /// </summary>
        public string UnitCount { get; set; }

        /// <summary>
        /// 项目负责人姓名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 项目负责人电话
        /// </summary>
        public string ItemPhoneNumber { get; set; }

        /// <summary>
        /// 项目负责人email
        /// </summary>
        public string ItemEmail { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public  string RealmName { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyUser { get; set; }
    }
}
