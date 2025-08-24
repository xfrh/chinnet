using Dapper;
using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Satellites
{
    public partial class SatelliteService : BaseService<Satellite>, ISatelliteService
    {
        public SatelliteService(IRepository<Satellite> repository) : base(repository)
        {

        }

        public IPagedList<Satellite> QueryPage(string name, string phone, int pageIndex, int pageSize)
        {
            var query = this._repository.Table.Where(m => m.RealmName != "&");

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.ChargeName.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.ChargePhoneNumber.Contains(phone.Trim()));

            //if (!string.IsNullOrWhiteSpace(companyName))
            //    query = query.Where(m => m.PlaceOfWork.Contains(companyName.Trim()));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Satellite>(query, pageIndex, pageSize);
        }

        public IPagedList<Satellite> QueryPageSa(string name, long said,string phone, int pageIndex, int pageSize)
        {
            var query = this._repository.Table.Where(m => m.RealmName != "&");

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.ChargeName.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.ChargePhoneNumber.Contains(phone.Trim()));

            //if (!string.IsNullOrWhiteSpace(companyName))
            //    query = query.Where(m => m.PlaceOfWork.Contains(companyName.Trim()));
            query = query.Where(m => m.Id == said);

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Satellite>(query, pageIndex, pageSize);
        }

        public IPagedList<Satellite> QueryPageSa(string name, string phone, int pageIndex, int pageSize)
        {
            var query = this._repository.Table.Where(m => m.RealmName != "&");

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.ChargeName.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(m => m.ChargePhoneNumber.Contains(phone.Trim()));

            //if (!string.IsNullOrWhiteSpace(companyName))
            //    query = query.Where(m => m.PlaceOfWork.Contains(companyName.Trim()));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<Satellite>(query, pageIndex, pageSize);
        }

        public bool IsExist(string id)
        {
            Satellite satellite = this._repository.Table.SingleOrDefault(m => m.RealmName == id);
            if (satellite != null)
                return true;
            else
                return false;
        }

        public IList<Satellite> GetLoginList()
        {
            return this._repository.Table.Where(m => m.Mark > 0 && m.RealmName != "&").ToList();
        }

        public int QueryApplyUser(string userId)
        {
            return this._repository.Table.Count(m => m.Mark > 0 && m.RealmName != "&" && m.ApplyUser == userId);
        }

        public int CreateMeetingType(string sateName)
        {
            try
            {
                int result = 0;
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format("insert into MeetingType (Id,Name,Sort,MeetingTypeId,InsertTime,UpdateTime,DeleteTime,Version,Mark,Describe) values("+DateTime.Now.ToString("yyyyMMddHHmmssss")+",'"+ sateName+"',100,0,'"+DateTime.Now.ToString("G")+"','"+ DateTime.Now.ToString("G")+ "','1900-01-01 00:00:00.000',1,1,NULL)");
                    result = conn.Execute(sql);
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int CreateMedicalProjectType(string sateName)
        {
            try
            {
                int result = 0;
                using (SqlConnection conn = DapperHelper.GetConnection())
                {
                    string sql = string.Format("insert into MedicalDataProject (Id,Name,Sort,Path,InsertTime,UpdateTime,DeleteTime,Version,Mark,Describe,Tag,Remark) values(" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ",'" + sateName + "',100,'Sate','" + DateTime.Now.ToString("G") + "','" + DateTime.Now.ToString("G") + "','1900-01-01 00:00:00.000',1,1,NULL,'','')");
                    result = conn.Execute(sql);
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
