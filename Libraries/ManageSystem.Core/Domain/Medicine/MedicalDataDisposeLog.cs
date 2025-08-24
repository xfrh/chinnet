using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;


namespace ManageSystem.Core.Domain.Medicine
{
	/// <summary>
	/// 实体类 ，数据库表名：MedicalDataDisposeLog 
	/// </summary>
	public partial class MedicalDataDisposeLog : BaseEntity
	{

		/// <summary>
		/// 对应的医学数据Id
		/// <summary>
		public long MedicalDataId { get; set; }
		/// <summary>
		/// 处理的数据
		/// <summary>
		public String Data { get; set; }
		/// <summary>
		/// 状态：1  待处理   2：处理失败   3：处理成功
		/// <summary>
		public Int32 Status { get; set; }
		/// <summary>
		/// 处理时间
		/// <summary>
		public DateTime DisposeTime { get; set; }
		/// <summary>
		/// 处理结果
		/// <summary>
		public String Result { get; set; }


	}
}
