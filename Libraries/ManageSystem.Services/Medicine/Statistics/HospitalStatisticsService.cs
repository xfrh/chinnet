using ManageSystem.Core.Domain.Medicine.Statistics;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine.Statistics
{

    /// <summary>
    /// 医学数据统计报表 的  医院数据统计  业务处理类
    /// 
    /// </summary>
    public class HospitalStatisticsService
    {
        private readonly IDbContext DbContext = null;

        public HospitalStatisticsService()
        {
            this.DbContext = EngineContext.Current.Resolve<IDbContext>();
        }

        /// <summary>
        /// 按省份统计医院数据
        /// </summary>
        /// <returns></returns>
        public List<HospitalByArea> GetHospitalByAreaData()
        {
            string sql = @"  SELECT Id,Name,(SELECT COUNT(*)  FROM dbo.MedicalData 
                                     WHERE Mark>0 AND AreaId=Area.Id ) AS HospitalCount  
                                     FROM dbo.Area (NOLOCK)WHERE Mark>0 AND ParentId = 0 ORDER BY Sort DESC ";
      
            return this.DbContext.SqlQuery<HospitalByArea>(sql).ToList();
        }

    }
}
