using ManageSystem.Core;
using ManageSystem.Core.Domain.MicdataDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.MicdataDistribution
{
    public interface IddDocumentItemService
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int ddDocumentItemCreate(ddDocumentItem model);

      
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="gremcode"></param>
        /// <param name="yeraid"></param>
        /// <param name="antibiotics"></param>
        /// <returns></returns>
        List<ddDocumentItem> GetDdDocuments(string [] gremcode, string  yeraid, string  antibiotics);

        /// <summary>
        /// 查询tile
        /// </summary>
        /// <returns></returns>
        string GetCategoryValues();
        /// <summary>
        /// 根据医学数据Id获取对应的分页数据
        /// </summary>
        /// <param name="medicalDataId">对应的医学数据Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ddDocumentItem> QueryPage(long medicalDataId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        int deleteDocumentItem(string ids);
    }
}
