using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core;

namespace ManageSystem.Services.Medicine
{
	/// <summary>
	/// 操作类 ，数据库表名：Specimen 
	/// </summary>
	public partial class SpecimenService :  BaseService<Specimen>, ISpecimenService
	{

		public SpecimenService(IRepository<Specimen> repository): base(repository)
		{
			
		}

        public IPagedList<Specimen> QueryPage(string name, string state, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(state))
            {
                int stateTemp = int.Parse(state);
                query = query.Where(m => m.Status == (stateTemp == 1));
            }

            query = query.OrderBy(m => m.Sort);

            var list = new PagedList<Specimen>(query.ToList(), pageIndex, pageSize);

            return list;
        }

    }
}
