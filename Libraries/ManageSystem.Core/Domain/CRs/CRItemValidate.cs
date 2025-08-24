using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Core.Domain.CRs
{
    public partial class CRItemValidate : BaseEntity
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
