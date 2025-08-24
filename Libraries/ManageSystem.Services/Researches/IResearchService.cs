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
	/// 操作接口类 ，数据库表名：Research 
	/// </summary>
	public  partial interface IResearchService : IBaseService<Research>
	{
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
        IQueryable<Research> Query(long typeId, long areaId, int day, int orderBy);

        /// <summary>
        /// 分页查询  后台
        /// </summary>
        /// <param name="name">科研主题</param>
        /// <param name="researchType">所属分类</param>
        /// <param name="memberName">发布人</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Research> QueryPage(string name, long researchType, string memberName, string startTime, string endTime, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// 会员中心 分页查询  网页  
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <returns></returns>
        IQueryable<Research> Query(long memberId);

    }
}
