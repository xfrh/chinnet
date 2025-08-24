using ManageSystem.Core;
using ManageSystem.Core.Domain.Organism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Organism
{
    /// <summary>
	/// 操作接口类 ，数据库表名：BacteriaDetailedData 
	/// </summary>
    public partial interface IBacteriaDetailedDataService : IBaseService<BacteriaDetailedData>
    {
        /// <summary>
        /// 查询细菌信息
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        IList<BacteriaDetailedData> Query(string ids);

        /// <summary>
        /// 分页查询 细菌信息
        /// </summary>
        /// <param name="name">细菌名称</param>
        /// <param name="medicalOrganismId">细菌名</param>
        /// <param name="medicalOrganismTypeId">细菌分类</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">总行数</param>
        /// <returns></returns>
        IPagedList<BacteriaDetailedData> PageQuery(string name, long medicalOrganismTypeId, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
