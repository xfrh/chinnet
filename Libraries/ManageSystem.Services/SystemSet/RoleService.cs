using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core;

namespace ManageSystem.Services.SystemSet
{
	/// <summary>
	/// 操作类 ，数据库表名：Role 
	/// </summary>
	public partial class RoleService :  BaseService<Role>, IRoleService
	{
        private readonly IUserRoleService userRoleService;
        private readonly IRepository<UserRole> userRoleRepository;

        private readonly IRepository<SatelliteRole> satelliteRoleRepository;

        public RoleService(IRepository<Role> repository,
            IUserRoleService _userRoleService,
             IRepository<UserRole> _userRoleRepository,
            IRepository<SatelliteRole> _satelliteRoleRepository
            ) : base(repository)
		{
            this.userRoleService = _userRoleService;
            this.userRoleRepository = _userRoleRepository;
            this.satelliteRoleRepository = _satelliteRoleRepository;
        }

        /// <summary>
        /// 根据指定用户的id获取该用户所拥有的角色
        /// </summary>
        /// <param name="userinfoId"></param>
        /// <returns></returns>
        public List<Role> GetListByUserId(long userinfoId)
        {
            if (userinfoId<=0 ) return null;

            var query = this._repository.Table.Where(
                rel => this.userRoleRepository.Table.Where(
                         m => m.UserinfoId == userinfoId).Select(t => t.RoleId).Contains(rel.Id)
                );

            return query.ToList();
        }

        /// <summary>
        /// 根据指定用户的id获取该用户所拥有的角色
        /// </summary>
        /// <param name="userinfoId"></param>
        /// <returns></returns>
        public List<Role> GetListBySatelliteUserId(long SatelliteUserId)
        {
            if (SatelliteUserId <= 0) return null;

            var query = this._repository.Table.Where(
                rel => this.satelliteRoleRepository.Table.Where(
                         m => m.SatelliteUserId == SatelliteUserId).Select(t => t.RoleId).Contains(rel.Id)
                );

            return query.ToList();
        }
        public IPagedList<Role> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = this._repository.Table.Where(m=>m.Mark>0);

            if (!string.IsNullOrWhiteSpace(name)) query = query.Where(m => m.Name.Contains(name));

            query= query.OrderBy(m => m.Sort);

            return    new PagedList<Role>(query, pageIndex, pageSize);
        }

    }
}
