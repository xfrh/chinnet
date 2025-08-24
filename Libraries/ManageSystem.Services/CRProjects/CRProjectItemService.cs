using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ManageSystem.Core.Utility;
using System.IO;
using System.Web;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Domain.CRProjects;

namespace ManageSystem.Services.CRProjects
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalDataItem 
    /// </summary>
    public partial class CRProjectItemService : BaseService<CRProjectItem>, ICRProjectItemService
    {

        public CRProjectItemService(IRepository<CRProjectItem> repository) : base(repository)
        {

        }
        public IPagedList<CRProjectItem> QueryPage(long projectId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);


            if (projectId > 0)
                query = query.Where(m => m.ProjectId == projectId);

            //if (hospitalId > 0)
            //    query = query.Where(m => m.HospitalId == hospitalId);

            //if (year > 0)
            //    query = query.Where(m => m.Year == year);

            //if (quarter > 0)
            //    query = query.Where(m => m.Quarter == quarter);

            //if (projectType > 0)
            //{
            //    query = query.Where(r => r.ProjectType == projectType);
            //}
            query = query.OrderByDescending(m => m.InsertTime);

            var list = new PagedList<CRProjectItem>(query, pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 根据CR项目Id删除数据
        /// </summary>
        /// <param name="projectId">CR项目Id</param>
        public void DeleteByProject(long projectId)
        {
            this.Delete(m => m.ProjectId == projectId);
        }


    }
}
