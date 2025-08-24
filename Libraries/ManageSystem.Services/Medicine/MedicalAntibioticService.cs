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
	/// 操作类 ，数据库表名：MedicalAntibiotic 
	/// </summary>
	public partial class MedicalAntibioticService :  BaseService<MedicalAntibiotic>, IMedicalAntibioticService
	{

		public MedicalAntibioticService(IRepository<MedicalAntibiotic> repository): base(repository)
		{
			
		}

	}
}
