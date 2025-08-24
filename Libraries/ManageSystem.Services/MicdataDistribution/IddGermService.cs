using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageSystem.Core;
using ManageSystem.Core.Domain.MicdataDistribution;

namespace ManageSystem.Services.MicdataDistribution
{
    public partial interface IddGermService 
    {
        /// <summary>
        /// web下拉列表
        /// </summary>
        /// <returns></returns>
        List<ddGerm> GetGerms();

        IPagedList<ddGerm> GetddGerms(string title,string code,int pageIndex = 0, int pageSize = 2147483647);

        int insertGerms(ddGerm model);
        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        int UpdateSort(int sort);

        /// <summary>
        /// 查询排序号是存在
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        ddGerm GetSort(int sort);

        /// <summary>
        /// 根据id查询细菌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ddGerm QueryEntity(long id);

        /// <summary>
        /// 编辑细菌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateddGerm(ddGerm model);

        /// <summary>
        ///根据条件删除数据
        /// </summary>
        /// <param name="ids"></param>
        void Delete(string ids);

        /// <summary>
        /// 根据code查询细菌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ddGerm QueryEntityCode(string code);

        /// <summary>
        /// 修改细菌是否有数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateddGermMark(string code);

        /// <summary>
        /// 查询默认显示细菌
        /// </summary>
        /// <returns></returns>
        List<ddGerm> GetdefaultGerms();
    }
}
