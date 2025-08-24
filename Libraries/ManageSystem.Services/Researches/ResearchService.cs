using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Researches;
using ManageSystem.Core;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Domain.Research;

namespace ManageSystem.Services.Researches
{
	/// <summary>
	/// 操作类 ，数据库表名：Research 
	/// </summary>
	public partial class ResearchService :  BaseService<Research>, IResearchService
	{

		public ResearchService(IRepository<Research> repository): base(repository)
		{
			
		}


        /// <summary>
        /// 分页查询  网页
        /// </summary>
        /// <param name="typeId">会议类型</param>
        /// <param name="areaId">所属区域（省份）</param>
        /// <param name="day">天查询条件</param>
        /// <param name="price">收费条件</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IQueryable<Research> Query(long typeId, long areaId, int day,int orderBy)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (typeId > 0)
                query = query.Where(m => m.ResearchTypeId == typeId);

            if (areaId > 0)
                query = query.Where(m => m.AreaProvinceId == areaId);

            if (day > 0)
            {
                DateTime endTime = DateTime.Now;
                switch (day)
                {
                    case 1:
                        //今天
                        DateTime startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
                        query = query.Where(m => m.StartTime >= startTime && m.StartTime <= endTime);

                        break;
                    case 2:
                        //近一周
                        startTime = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00"));
                        query = query.Where(m => m.StartTime >= startTime && m.StartTime <= endTime);

                        break;
                    case 3:
                        //近一月
                        startTime = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd 00:00"));
                        query = query.Where(m => m.StartTime >= startTime && m.StartTime <= endTime);
                        break;
                }
            }

            if (orderBy == 0)
            {
                //按时间
                query = query.OrderByDescending(m => m.InsertTime);
            }
            else
            {
                //按热度
                query = query.OrderByDescending(m => m.InsertTime).OrderByDescending(m => m.ViewCount);
            }
            
            return query;

        }

        public IPagedList<Research> QueryPage(string name, long researchType, string memberName,   string startTime, string endTime, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0 && m.Status == (int)ResearchStatusEnum.Finish);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (researchType > 0)
                query = query.Where(m => m.ResearchTypeId == researchType);

            if (!string.IsNullOrWhiteSpace(memberName))
                query = query.Where(m => m.MemberName.Contains(memberName.Trim()));

            if (!string.IsNullOrWhiteSpace(startTime) && DateHelper.IsDateTime(startTime))
            {
                DateTime startTimeTemp = DateTime.Parse(startTime);
                query = query.Where(m => m.StartTime >= startTimeTemp);
            }

            if (!string.IsNullOrWhiteSpace(endTime) && DateHelper.IsDateTime(endTime))
            {
                DateTime startTimeTemp = DateTime.Parse(endTime);
                query = query.Where(m => m.StartTime <= startTimeTemp);
            }

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Research>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// 会员中心 分页查询  网页  
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        public IQueryable<Research> Query(long memberId)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (memberId > 0)
                query = query.Where(m => m.MemberId == memberId);

            query = query.OrderByDescending(m => m.InsertTime);

            return query;
        }

    }
}
