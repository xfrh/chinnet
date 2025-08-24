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
    /// 操作接口类 ，数据库表名：Area 
    /// </summary>
    public partial interface IAreaService : IBaseService<Area>
    {

        /// <summary>
        /// 根据上级id获取所对应的列表数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<Area> QueryByParentId(long parentId);

        /// <summary>
        /// 根据区域id获取完整的区域名称，一直向上级查找，使用空格分割
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        string GetFullName(long areaId);

        /// <summary>
        /// 根据区域id获取的区域名
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        string GetName(long areaId);

        /// <summary>
        /// Max Sort
        /// </summary>
        /// <returns></returns>
        int MaxSort();

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<Area> GetArea(long parentId);
    }
}
