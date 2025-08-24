using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.Cre
{
   public class Cre_contact
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long? Contact_id { get; set; }
        /// <summary>
        /// 数据时间断表id
        /// </summary>
        public long? Cre_id { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public long Hospital_id { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string Hospital { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        public long Province_id { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public long City_id { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县id
        /// </summary>
        public long District_id { get; set; }
        /// <summary>
        /// 区县名称
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Detailedaddress { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }
}
