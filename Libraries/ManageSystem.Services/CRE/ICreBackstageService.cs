using ManageSystem.Core;
using ManageSystem.Core.Domain.Cre;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.CRE
{
   public interface ICreBackstageService
    {
        /// <summary>
        /// 列表页数据
        /// </summary>
        /// <param name="Hospital"></param>
        /// <param name="Data_year"></param>
        /// <param name="Contact"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Toconfigure>GetToconfigures(string Hospital, string Data_year, string Contact,  int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// 详情页数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Toconfigure GetToconfigure(long? id);

        /// <summary>
        /// 查询百分比
        /// </summary>
        /// <returns></returns>
        List<Cre_data_percentage> GetData_Percentages();

        /// <summary>
        /// 查询细菌列表
        /// </summary>
        /// <returns></returns>
        List<Cre_germ_types> Getgerm_types();

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ids">需要导出的项目id集合，为空则导出全部</param>
        /// <param name="ep">excel导出组建</param>
        /// <returns></returns>
        string Export(List<long> ids, ExcelPackage ep);

        /// <summary>
        ///根据条件删除数据
        /// </summary>
        /// <param name="ids"></param>
        void Delete(string ids);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="toconfigure"></param>
        /// <returns></returns>
        int UpdateCre_data(Toconfigure toconfigure);

        /// <summary>
        /// 细菌列表
        /// </summary>
        /// <param name="toconfigure"></param>
        /// <returns></returns>
        IPagedList<Cre_germ_types> GetBacteriaList(string Germ_name,int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// 查询细菌最大排序号
        /// </summary>
        /// <returns></returns>
        int GetMaxsort();

        /// <summary>
        /// 添加细菌
        /// </summary>
        int CreateBacteria(Cre_germ_types germ_Types);

        /// <summary>
        /// 细菌编辑返填/详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Cre_germ_types GetCre_germ_types(long? id);

        /// <summary>
        /// 删除细菌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void DeleteBacteria(string ids);

        /// <summary>
        /// 修改细菌
        /// </summary>
        /// <param name="germ_Types"></param>
        /// <returns></returns>
        int UpdateBacteria(Cre_germ_types  germ_Types);

        /// <summary>
        /// 查询是否有相同排序号
        /// </summary>
        /// <param name="germ_Types"></param>
        /// <returns></returns>
        List<Cre_germ_types> GetSort (int Sort);

        /// <summary>
        /// 修改排序号
        /// </summary>
        /// <param name="Sort"></param>
        /// <returns></returns>
        int UpdateSort(int Sort);


    }
}
