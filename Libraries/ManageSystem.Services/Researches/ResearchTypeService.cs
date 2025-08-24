using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Researches;

namespace ManageSystem.Services.Researches
{
	/// <summary>
	/// 操作类 ，数据库表名：ResearchType 
	/// </summary>
	public partial class ResearchTypeService :  BaseService<ResearchType>, IResearchTypeService
	{

		public ResearchTypeService(IRepository<ResearchType> repository): base(repository)
		{
			
		}

        /// <summary>
        /// 根据id获取类型名称
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public string GetTypeName(long typeId)
        {
            if (typeId == 0) return "";

            var entity = this.QueryEntity(typeId);
            if (entity == null || entity.Id <= 0) return "";

            return entity.Name;
        }

    }
}
