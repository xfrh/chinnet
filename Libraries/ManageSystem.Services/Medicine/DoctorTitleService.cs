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
	/// 操作类 ，数据库表名：DoctorTitle 
	/// </summary>
	public partial class DoctorTitleService :  BaseService<DoctorTitle>, IDoctorTitleService
	{

		public DoctorTitleService(IRepository<DoctorTitle> repository): base(repository)
		{
			
		}

        public List<DoctorTitle> QueryByParentId(long parentId)
        {
            return this.Query(m => m.ParentId == parentId).OrderBy(m => m.Sort).ToList();
        }

        public IPagedList<DoctorTitle> QueryPage(string name, string stateValue, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            
            var query = base._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(stateValue))
            {
                int stateTemp = int.Parse(stateValue);
                query = query.Where(m => m.Status == (stateTemp == 1));
            }

            query = query.OrderBy(m => m.Sort);

            var list = new PagedList<DoctorTitle>(query.ToList(), pageIndex, pageSize);

            return list;
        }
    }
}
