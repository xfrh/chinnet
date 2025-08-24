using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Domain.Sate;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作类 ，数据库表名：UserRole 
	/// </summary>
	public partial class SatelliteMenuService :  BaseService<SatelliteMenu>, ISatelliteMenuService
    {
        private readonly IRepository<SatelliteMenuRole> satelliteMenuRoleRepository;
        public SatelliteMenuService(IRepository<SatelliteMenu> repository, IRepository<SatelliteMenuRole> _satelliteMenuRoleRepository) : base(repository)
		{
            this.satelliteMenuRoleRepository = _satelliteMenuRoleRepository;

        }

        public void getByMenuIds(string menuIds)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据指定用户的id获取该用户所拥有的角色
        /// </summary>
        /// <param name="userinfoId"></param>
        /// <returns></returns>
        public List<SatelliteMenu> GetListBySatelliteUserId(long SatelliteUserId)
        {
            if (SatelliteUserId <= 0) return null;

            var query = this._repository.Table.Where(
                rel => this.satelliteMenuRoleRepository.Table.Where(m => m.SatelliteUserId == SatelliteUserId).Select(t => t.SatelliteMenuId).Contains(rel.Id)
                );

            return query.ToList();
        }

    }
}
