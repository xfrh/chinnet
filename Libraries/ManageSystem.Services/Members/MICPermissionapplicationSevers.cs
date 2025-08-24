using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.MIC;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Members
{
    public partial class MICPermissionapplicationSevers : BaseService<MICPermissionapplication>, IMICPermissionapplication
    {
        public MICPermissionapplicationSevers(IRepository<MICPermissionapplication> repository) : base(repository)
        {

        }


        public IPagedList<MICPermissionapplication> QueryPage(string name, string phone, string companyName, int pageIndex, int pageSize)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.Phone.Contains(phone.Trim()));

            if (!string.IsNullOrWhiteSpace(companyName))
                query = query.Where(m => m.CompanyName.Contains(companyName.Trim()));

            query = query.OrderByDescending(m => m.Applicationtime);

            return new PagedList<MICPermissionapplication>(query, pageIndex, pageSize);
        }

        //public MICPermissionapplication QueryEntityMID(long mid)
        //{
        //    MICPermissionapplication t = this._repository.GetByMId(mid);
        //    if (t == null) return null;

        //    return t;
        //}

    }
}
