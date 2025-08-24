using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageSystem.Core;
using ManageSystem.Core.Domain.MicdataDistribution;

namespace ManageSystem.Services.MicdataDistribution
{
   public interface IddAntibioticService
    {
        /// <summary>
        /// web方法
        /// </summary>
        /// <returns></returns>
        List<ddAntibiotic> GetDdAntibiotics();

        /// <summary>
        /// 后台列表页
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<ddAntibiotic> Antibioticslist(string title,string code,int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int insertAntibiotic(ddAntibiotic model);
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
        ddAntibiotic GetSort(int sort);

        /// <summary>
        /// 根据id查询抗生素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ddAntibiotic QueryEntity(long id);

        /// <summary>
        /// 编辑抗生素
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateAntibiotic(ddAntibiotic model);

        /// <summary>
        ///根据条件删除数据
        /// </summary>
        /// <param name="ids"></param>
        void Delete(string ids);


        /// <summary>
        /// 根据code查询抗生素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ddAntibiotic QueryEntityCode(string code);

        /// <summary>
        /// 修改抗生素是否有数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateddAntibioticMark(string code);


        /// <summary>
        /// 查询默认显示抗生素
        /// </summary>
        /// <returns></returns>
        List<ddAntibiotic> GetdefaultddAntibiotic();

    }
}
