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
    /// 操作类 ，数据库表名：MedicalDataItemError 
    /// </summary>
    public partial class MedicalDataItemValidateService :  BaseService<MedicalDataItemValidate>, IMedicalDataItemValidateService
    {

		public MedicalDataItemValidateService(IRepository<MedicalDataItemValidate> repository): base(repository)
		{
			
		}


        /// <summary>
        /// 根据医学数据Id获取对应的错误分页数据
        /// </summary>
        /// <param name="medicalDataId">对应的医学数据Id</param>
        /// <param name="level">0：表示全部  1：提示  2：警告  3：错误</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<MedicalDataItemValidate> QueryPage(long medicalDataId, int level, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (medicalDataId > 0)
                query = query.Where(m => m.MedicalDataId == medicalDataId);

            if (level > 0)
                query = query.Where(m => m.Level == level);

            query = query.OrderBy(m => m.UploadRowIndex);
            var list = new PagedList<MedicalDataItemValidate>(query, pageIndex, pageSize);

            return list;
        }


    }
}
