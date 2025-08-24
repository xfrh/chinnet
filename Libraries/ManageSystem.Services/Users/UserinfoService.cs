using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core;
using ManageSystem.Services.Security;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Services.SystemSet;

namespace ManageSystem.Services.Users
{
    /// <summary>
    /// 操作类 ，数据库表名：Userinfo 
    /// </summary>
    public partial class UserinfoService : BaseService<Userinfo>, IUserinfoService
    {
        private readonly IEncryptionService encryptionService;

        public UserinfoService(IRepository<Userinfo> repository,
             IEncryptionService _encryptionService
            ) : base(repository)
        {
            this.encryptionService = _encryptionService;
        }

        public override void Delete(long id)
        {
            Userinfo entity = this.QueryEntity(id);

            entity.Mark = 0;
            entity.DeleteTime = DateTime.Now;
            entity.LoginId = entity.LoginId + "_DELETE";
            if (!string.IsNullOrWhiteSpace(entity.Phone))
            {
                entity.Phone = entity.Phone + "_DELETE";
            }

            this._repository.Update(entity);

        }

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


        public override void Insert(Userinfo entity)
        {
            entity.Password = this.encryptionService.EncryptText(entity.Password);

            base.Insert(entity);
        }


        /// <summary>
        /// 根据登录帐号获取一个用户
        /// </summary>
        /// <param name="loginId">登录帐号</param>
        /// <returns></returns>
        public Userinfo QueryModelByLoginId(string loginId)
        {
            return this.QueryEntity(m => m.LoginId.Equals(loginId));
        }



        public IPagedList<Userinfo> QueryPage(string loginId, string name, int state, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m => m.Mark > 0);

            if (!string.IsNullOrWhiteSpace(loginId))
                query = query.Where(m => m.LoginId.Contains(loginId));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (state > 0)
                query = query.Where(m => m.State == state);

            query = query.OrderBy(m => m.Id);

            return new PagedList<Userinfo>(query, pageIndex, pageSize);

        }
    }
}
