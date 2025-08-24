using ManageSystem.Core.Domain.Organism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManageSystem.Core.Data;
using ManageSystem.Core;
using ManageSystem.Services.Medicine;
using ManageSystem.Core.Domain.Medicine;

namespace ManageSystem.Services.Organism
{
    /// <summary>
    /// 操作类 ，数据库表名：BacteriaDetailedData 
    /// </summary>
    public partial class BacteriaDetailedDataService : BaseService<BacteriaDetailedData>, IBacteriaDetailedDataService
    {
        private IMedicalOrganismService medicalOrganismService;
        private IMedicalOrganismTypeService medicalOrganismTypeService;
        public BacteriaDetailedDataService(IRepository<BacteriaDetailedData> repository,
             IMedicalOrganismService _medicalOrganismService,
             IMedicalOrganismTypeService _medicalOrganismTypeService) : base(repository)
        {
            this.medicalOrganismService = _medicalOrganismService;
            this.medicalOrganismTypeService = _medicalOrganismTypeService;
        }
        /// <summary>
        /// 查询细菌信息
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        public IList<BacteriaDetailedData> Query(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return null;

            var query = base._repository.Table.Where(m => m.Mark > 0);
            string[] array = ids.Split(','); //id集合，格式： 1，3，4,5,6
            query = query.Where(m => array.Contains(m.Id.ToString()));

            IList<BacteriaDetailedData> list = query.ToList();
            if (list == null || !list.Any()) return null;

            //对数据进行排序
            IList<BacteriaDetailedData> newList = new List<BacteriaDetailedData>();
            for (int i = 0; i < list.Count; i++)
            {
                var temp = list.Where(m => m.Id == long.Parse(array[i])).FirstOrDefault();
                if (temp == null) continue;

                newList.Add(temp);
            }

            return newList;
        }
        /// <summary>
        /// 分页查询 细菌信息
        /// </summary>
        /// <param name="medicalOrganismId">细菌</param>
        /// <param name="medicalOrganismTypeId">细菌分类</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">总行数</param>
        /// <returns></returns>
        public IPagedList<BacteriaDetailedData> PageQuery(string name, long medicalOrganismTypeId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            

            var query = base._repository.Table.Where(c => c.Mark > 0); ;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (medicalOrganismTypeId>0)
            {
                query = query.Where(c => c.OrganismTypeId==medicalOrganismTypeId );
            }


            query = query.OrderByDescending(c => c.InsertTime);

            var list = new PagedList<BacteriaDetailedData>(query.ToList(), pageIndex, pageSize);

            return list;

        }
    }
}
