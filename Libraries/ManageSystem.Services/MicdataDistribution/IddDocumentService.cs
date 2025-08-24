using ManageSystem.Core;
using ManageSystem.Core.Domain.MicdataDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.MicdataDistribution
{
   public interface IddDocumentService
    {
        /// <summary>
        /// 列表页
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Data_year"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ddDocument> GetddDocument(string filename, long year_id, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        string GetUsers(long userid);

        /// <summary>
        /// 数据添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int ddDocumentCreate(ddDocument model);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        int deleteDocument(string ids);
        /// <summary>
        /// 根据ID查询一个模型
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ddDocument Document(long Id);

    }
}
