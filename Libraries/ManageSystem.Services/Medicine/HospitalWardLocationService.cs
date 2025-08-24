using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Medicine
{
    public partial class HospitalWardLocationService : BaseService<HospitalWardLocation>, IHospitalWardLocationService
    {
        public HospitalWardLocationService(IRepository<HospitalWardLocation> repository

            ) : base(repository)
        {

        }

        /// <summary>
        /// 查询医院科室列表
        /// </summary>
        /// <param name="HospitalId"></param>
        /// <returns></returns>
        public IQueryable<HospitalWardLocation> Query(long HospitalId)
        {
            var data = base._repository.Table.Where(m => m.Mark > 0 && m.HospitalId == HospitalId);
            return data.OrderBy(m=>m.Sort);
        }

        /// <summary>
        /// 根据id查询科室
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HospitalWardLocation QueryEntity(long? Id)
        {
            HospitalWardLocation t = this._repository.GetById(Id);
            if (t == null) return null;

            return t;
        }

        /// <summary>
        /// 根基id删除单个，或多个数据
        /// </summary>
        /// <param name="ids"></param>
        public override void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return;
            ids = ids.TrimEnd(',');

            string[] cities = ids.Split(',');

            foreach (var item in cities)
            {
                long temp = long.Parse(item);
                this.Delete(temp);
            }
        }

        public IPagedList<dynamic> QueryPage(string name, int page, int pageSize)
        {
            string sql = "SELECT HospitalId, [Name], COUNT(Ward) AS WardCount, MAX(InsertTime) AS LastDateTime FROM dbo.HospitalWardLocation WHERE Mark > 0 AND HospitalId > 0 GROUP BY HospitalId, [Name]";
            Dapper.DynamicParameters parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(name) && !name.Trim().ToUpperInvariant().Equals("NULL"))
            {
                sql += " HAVING [Name] LIKE @Name ";
                parameters.Add("@Name", $"%{name}%");
            }
            sql += " ORDER BY [LastDateTime] DESC, [Name] ;";

            var data = DapperHelper.GetConnection().Query<dynamic>(sql, parameters).ToList();

            return new PagedList<dynamic>(data, page, pageSize);
        }

        /// <summary>
        /// 重新更新指定的上传文件科室配置信息
        /// </summary>
        /// <param name="medicalId">所属上传id</param>
        /// <returns></returns>
        public string UpdateItem(long medicalId)
        {
            IMedicalDataService medicalDataService = EngineContext.Current.Resolve<IMedicalDataService>();
            IMedicalDataItemService medicalDataItemService = EngineContext.Current.Resolve<IMedicalDataItemService>();

            //查询基础数据
            var entity = medicalDataService.QueryEntity(m => m.Id == medicalId && m.Mark > 0);
            if (entity == null || entity.Id <= 0)
                throw new Exception("Id：" + medicalId + " 数据不存在");

            var itemList = medicalDataItemService.Query(m => m.MedicalDataId == entity.Id).ToList();
            if (itemList == null || !itemList.Any())
                throw new Exception("未查询到明细数据");

            var locationList = this.Query(m => m.HospitalId == entity.HospitalId).ToList();
            if (locationList == null || !locationList.Any())
                throw new Exception("未查询到科室设置数据");

            foreach (var item in itemList)
            {
                var node = locationList.Where(m => m.Ward.ToLower().Equals(item.WARD.ToLower())).FirstOrDefault();
                if (node == null) continue;

                //如果医院重新设置了对应的编码，则需要重新赋值
                item.WARD = node.Location;
                item.DEPARTMENT = node.Department_EN;
                item.WARD_TYPE = node.Location_Type;

                medicalDataItemService.Update(item);
            }

            return "处理完成";
        }
        /// <summary>
        /// 修改科室配置
        /// </summary>
        /// <param name="hospital"></param>
        public void UpdateDepartment(HospitalWardLocation hospital)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("UPDATE HospitalWardLocation SET Name='{0}',Ward='{1}',Department_CN='{2}',Department_EN='{3}',Location='{4}',Location_Type='{5}',UpdateTime='{6}',Mark='{7}' WHERE Id='{8}'",hospital.Name,hospital.Ward,hospital.Ward,hospital.Department_EN,hospital.Location,hospital.Location_Type,hospital.UpdateTime,hospital.Mark,hospital.Id);
                conn.Execute(sql);              
            }
        }
        /// <summary>
        /// 根据条件查询科室配置
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ward"></param>
        /// <returns></returns>
        public HospitalWardLocation QueryWard(long? Id, string ward)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[HospitalWardLocation] where HospitalId='{0}' and Ward='{1}'",Id,ward);
                HospitalWardLocation wards = conn.Query(sql).FirstOrDefault();
                return wards;
            }
        }

        public void DeleDepartment(long hospitalId)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("delete from  HospitalWardLocation  where HospitalId='{0}'", hospitalId);
                conn.Execute(sql);
            }
        }
    }
}
