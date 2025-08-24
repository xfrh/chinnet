using ManageSystem.Core.Domain.Cre;
using ManageSystem.Core.Domain.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.CRE
{
   public interface ICreDataService
    {
        /// <summary>
        /// 第一级数据显示
        /// </summary>
        /// <returns></returns>
        List<Toconfigure> SGetdata_Show(string year, string Germ_id);
        /// <summary>
        /// 第二级数据显示
        /// </summary>
        /// <returns></returns>
        List<Toconfigure> CGetdata_Show(string year, string Germ_id);
        /// <summary>
        /// 热图颜色
        /// </summary>
        /// <param name="year"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Cre_Colormatching GetColormatchings(string year, long? type);

        /// <summary>
        /// 首页分类
        /// </summary>
        /// <returns></returns>
        List<Cre_germ_types> Get_Type();
        /// <summary>
        /// 查询年份
        /// </summary>
        /// <returns></returns>
        List<Cre_data_period> Get_Year();

        string Get_Data_Periods();

        /// <summary>
        /// 查询排在最前面的细菌
        /// </summary>
        /// <returns></returns>
        Cre_germ_types Get_Typeshou();

        /// <summary>
        /// 柱状图查询 大肠埃希菌
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        List<string> Escherichiacoli(string field, long? Germ_id);
        /// <summary>
        /// 柱状图查询 肺炎克雷伯菌
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        List<string> klebsiella(string field, long? Germ_id);
        /// <summary>
        /// 柱状图查询  阴沟肠杆菌
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        List<string> Enterobacter(string field, long? Germ_id);
        /// <summary>
        /// 柱状图查询 黏质沙雷菌
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        List<string> serratia(string field, long? Germ_id);

        /// <summary>
        /// 查询细菌分类表
        /// </summary>
        /// <returns></returns>
        List<Cre_germ_types> Get_Germ_Type();

        /// <summary>
        /// 柱状图查询(菌株率‘点击药查询菌’)
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        List<string> Strainrate(string field);

        /// <summary>
        /// 柱状图查询(菌株率，‘点击菌查询要’)
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        List<string> Drugrate(string field, long? Germ_id);

        List<Member> GetMember(long? id);
    }
}
