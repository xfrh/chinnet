using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Sate;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Satellites
{
    public partial class SatelliteUserService : BaseService<SatelliteUser>, ISatelliteUserService 
    {
        public SatelliteUserService(IRepository<SatelliteUser> repository
             ) : base(repository)
        {
            
        }
        
        public SatelliteUser insert(long SatelliteId, string UserName, string Id, string PassWord, Member member)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                con.Open();
                using (var tran = con.BeginTransaction())
                {
                   

                    //申请表里面插入数据
                    SatelliteUser model = new SatelliteUser();
                    model.SatelliteId = SatelliteId;
                    model.UserName = UserName;
                    model.PassWord = PassWord;
                   
                    this.Insert(model);

                    //添加操作日志

                    tran.Commit();

                    return model;
                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SatelliteUser QueryEntity(string id)
        {
           return this.QueryEntity(m => m.Id.Equals(id));
        }

        public SatelliteUser QueryEntityByUserName(string userName)
        {
            return this.QueryEntity(m => m.UserName.Equals(userName));
        }
        
        public IPagedList<SatelliteUser> QueryPage(long SatelliteId, string UserName, int pageIndex, int pageSize)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(SatelliteId+""))
                query = query.Where(m => m.SatelliteId == SatelliteId);

            if (!string.IsNullOrWhiteSpace(UserName))
                query = query.Where(m => m.UserName.Contains(UserName.Trim()));
            //if (!string.IsNullOrWhiteSpace(companyName))
            //    query = query.Where(m => m.PlaceOfWork.Contains(companyName.Trim()));

            query = query.OrderByDescending(m => m.InsertTime);

            return new PagedList<SatelliteUser>(query, pageIndex, pageSize);
        }

        public void update(long SatelliteId, string UserName, string Id, string PassWord)
        {
            SatelliteUser sa=this.QueryEntity(m => m.Id.Equals(Id));
            sa.UserName = UserName;
            sa.PassWord = PassWord;
            this.Update(sa);
        }

        public void updateStatus(long SatelliteId, string Status, string Id)
        {
            SatelliteUser sa = this.QueryEntity(m => m.Id.Equals(Id));
            
            this.Update(sa);
        }
    }
}
