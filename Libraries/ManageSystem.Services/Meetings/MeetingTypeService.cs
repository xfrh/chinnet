using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Meetings;
using ManageSystem.Core;

namespace ManageSystem.Services.Meetings
{
	/// <summary>
	/// 操作类 ，数据库表名：MeetingType 
	/// </summary>
	public partial class MeetingTypeService :  BaseService<MeetingType>, IMeetingTypeService
	{

		public MeetingTypeService(IRepository<MeetingType> repository): base(repository)
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

        public IPagedList<MeetingType> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<MeetingType>(query, pageIndex, pageSize);

        }

        public IPagedList<MeetingType> QueryPageSa(string name, string sateid,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            query = query.Where(m => m.Describe.Contains(sateid));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<MeetingType>(query, pageIndex, pageSize);

        }


    }
}
