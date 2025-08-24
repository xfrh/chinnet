using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalSpecType 
    /// </summary>
    public partial class MedicalSpecTypeService :  BaseService<MedicalSpecType>, IMedicalSpecTypeService
    {
		public MedicalSpecTypeService(IRepository<MedicalSpecType> repository): base(repository)
		{
			
		}

    }
}
