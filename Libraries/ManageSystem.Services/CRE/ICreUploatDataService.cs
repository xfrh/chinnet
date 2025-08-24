using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.CRE
{
    public interface ICreUploatDataService
    {
        /// <summary>
        /// 查询细菌分类表
        /// </summary>
        /// <returns></returns>
        List<Cre_germ_types> Get_Germ_Type();

        /// <summary>
        /// 根据id查询细菌分类
        /// </summary>
        /// <param name="germ_id"></param>
        /// <returns></returns>
        List<Cre_germ_types> Get_Germ_Types(long? germ_id);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="toconfigure"></param>
        /// <returns></returns>
        int AddCre_Data(Toconfigure toconfigure);

        /// <summary>
        /// 地区下拉框绑定
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<Area> GetareList(long parentId);

        /// <summary>
        /// 查询基础信息
        /// </summary>
        /// <param name="loginid"></param>
        /// <returns></returns>
        List<Toconfigure> Basicinformation(long loginid);

        /// <summary>
        /// 查询细菌名称
        /// </summary>
        /// <param name="ger_id"></param>
        /// <returns></returns>
        string GetCre_germ_types(long? ger_id);

        /// <summary>
        /// 查询排在最前面的细菌
        /// </summary>
        /// <returns></returns>
        Cre_germ_types Get_Typeshou();

        /// <summary>
        /// 数据返填
        /// </summary>
        /// <returns></returns>
        List<Toconfigure> Getdata_show(string year, string season, long? Germ_ids, long? Created_by);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="toconfigure"></param>
        /// <returns></returns>
        int UpdateCre_data(Toconfigure toconfigure);

        /// <summary>
        /// 查询Cre_data_period
        /// </summary>
        /// <returns></returns>
        Cre_data_period GetPeriod(long? Germ_id, string yera, string season, long? memberid);

        /// <summary>
        /// 添加CRE日志
        /// </summary>
        /// <param name="heatmp_Log"></param>
        /// <returns></returns>
        void AddCreProject_log(CreProject_log heatmp_Log);       
    }
}
