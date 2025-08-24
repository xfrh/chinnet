using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;

namespace ManageSystem.Services.Medicine
{
	/// <summary>
	/// 操作类 ，数据库表名：MedicalDataDisposeLog 
	/// </summary>
	public partial class MedicalDataDisposeLogService :  BaseService<MedicalDataDisposeLog>, IMedicalDataDisposeLogService
	{
		public MedicalDataDisposeLogService(IRepository<MedicalDataDisposeLog> repository): base(repository)
		{
			
		}

	}
}
