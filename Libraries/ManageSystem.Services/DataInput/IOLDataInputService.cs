using ManageSystem.Core;
using ManageSystem.Core.Domain.DataInput;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.DataInput
{
    public partial interface IOLDataInputService /*: IBaseService<OLDataInput>*/
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        int AddOLDataInput(OLDataInput model);

        /// <summary>
        /// 添加数据-鼻梁
        /// </summary>
        /// <param name="modelList"></param>
        /// <returns></returns>
        //int BatchOLDataInput(List<OLDataInput> modelList);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int delete(long Id);
        
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<OLDataInput> QueryList(string[] ids);

      
        /// <summary>
        /// 根据医学数据Id获取成员对应的分页数据
        /// </summary>
        /// <param name="medicalDataId">对应的医学数据Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<OLDataInput> QueryPage(long projectid,long create_byId, string number,string ischeck,int pageIndex = 0, int pageSize = int.MaxValue);

        IPagedList<OLDataInput> QuerPageByCondition(string number,string ischeck, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 管理员数据分页查询
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        IPagedList<OLDataInput> QueryPagelist(long Id, long createdid, string number,string ischeck,int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <returns></returns>
        List<OLFieldmodel> GetOLDataheade(long projectid);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int updateOLDataInput(OLDataInput model);


        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AuditorOLDataInput(OLDataInput model, string Ids);

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="ids">需要导出的项目id集合，为空则导出全部</param>
        /// <param name="ep">excel导出组建</param>
        /// <returns></returns>
        //string Export(string ids, ExcelPackage ep);

        /// <summary>
        /// 配置修改页面数据
        /// </summary>
        /// <returns></returns>
        List<OLDataInput> updateOLDataheade(long dataid);

        /// <summary>
        /// 配置修改页面数据
        /// </summary>
        /// <returns></returns>
        List<OLFieldmodel> GetOLDataheadelist(long dataid);

        /// <summary>
        /// 根据项目id查询最后这个项目最后插入的一条数据
        /// </summary>
        /// <returns></returns>
        OLDataInput GetMaxDatalist(long projectid,long userId);

        //string Export1(string ids, ExcelPackage ep);


        /// <summary>
        /// 导出dbf
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string GetDBF(long Id);


       string GetMoban(long Id, ExcelPackage ep);
    }
}
