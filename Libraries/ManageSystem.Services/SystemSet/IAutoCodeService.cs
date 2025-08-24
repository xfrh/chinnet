using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.SystemSet;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作接口类 ，数据库表名：AutoCode 
	/// </summary>
	public  partial interface IAutoCodeService : IBaseService<AutoCode>
	{

        /// <summary>
        /// 根据类型获取一个自动生成的编号
        /// </summary>
        /// <param name="type">编号类型</param>
        /// <returns></returns>
        String GetCode(AutoCodeType type);

	}
}
