using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.CRProjects
{
    /// <summary>
    /// 实体类 ，数据库表名：MedicalDataItemValidate
    /// </summary>
    public partial class CRProjectItemValidate : BaseEntity
	{
        /// <summary>
        /// 该行数据在原始上传文件中的行索引
        /// </summary>
        public int UploadRowIndex { get; set; }

        /// <summary>
        /// 医学数据Id
        /// <summary>
        public long MedicalDataId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 医学数据项Id
        /// <summary>
        public long MedicalDataItemId { get; set; }

        /// <summary>
        /// 字段项名称，医学数据的字段名称
        /// <summary>
        public String KeyName { get; set; }

        /// <summary>
        /// 错误等级 1：提示  2：警告  3：错误
        /// <summary>
        public Int32 Level { get; set; }

        /// <summary>
        /// 错误名称
        /// <summary>
        public string ErrorName { get; set; }

        /// <summary>
        /// 详细内容
        /// <summary>
        public string Content { get; set; }

    }
}
