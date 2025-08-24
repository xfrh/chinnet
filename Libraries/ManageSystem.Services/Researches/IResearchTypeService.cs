using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Researches;

namespace ManageSystem.Services.Researches
{
	/// <summary>
	/// 操作接口类 ，数据库表名：ResearchType 
	/// </summary>
	public  partial interface IResearchTypeService : IBaseService<ResearchType>
	{
        /// <summary>
        /// 根据id获取类型名称
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        string GetTypeName(long typeId);

    }
}
