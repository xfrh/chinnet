using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core;
using ManageSystem.Core.Domain.Documents;

namespace ManageSystem.Services.Documents
{
    /// <summary>
    /// 操作接口类 ，数据库表名：Document 
    /// </summary>
    public partial interface IDocumentService : IBaseService<Document>
    {

        /// <summary>
        /// 分页查询  网页
        /// </summary>
        /// <returns></returns>
        IQueryable<Document> Query();

        /// <summary>
        /// 分页查询数据 后台
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Document> QueryPage(string name, int page, int pageSize);

        /// <summary>
        /// 当前Sort最大值
        /// </summary>
        /// <returns></returns>
        int CurrentMaxSort();

        /// <summary>
        /// 查询已删除的资料
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<Document> QueryDelete(List<long> ids);

        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int UpdateSort(int sort);

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int weChattop(long Id);

        /// <summary>
        /// 查询排序号是存在
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Document GetSort(int sort);

        /// <summary>
        /// 取消置顶
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int Cancelceiling(long Id);


    }
}
