using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.CRs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.CRs
{
    public partial class CRItemService :BaseService<CRItem>, ICRItemService
    {
        public CRItemService(IRepository<CRItem> repository) : base(repository)
        {

        }
        public IPagedList<CRItem> QueryPage(long projectId, int pageIndex = 0, int pageSize = int.MaxValue)
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

            var list = new PagedList<CRItem>(query, pageIndex, pageSize);

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
