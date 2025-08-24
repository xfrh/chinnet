using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Domain.Members;

namespace ManageSystem.Core.Domain.Medicine
{
    /// <summary>
    /// 实体类 ，数据库表名：MedicalDataProject 
    /// </summary>
    public partial class MedicalDataProject : BaseEntity
    {

        public int Sort { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
    }

}
