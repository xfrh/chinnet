using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core;
using System.Linq.Expressions;

namespace ManageSystem.Services.SystemSet
{
    /// <summary>
    /// 操作类 ，数据库表名：Function 
    /// </summary>
    public partial class FunctionService : BaseService<Function>, IFunctionService
    {
        private readonly IRepository<RoleFunction> roleFunctionRepository;
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<UserRole> userRoleRepository;
        private readonly IRepository<SatelliteMenuRole> satelliteMenuRoleRepository;
        private readonly IRepository<SatelliteMenu> satelliteMenuRepository;

        public FunctionService(IRepository<Function> repository,
            IRepository<RoleFunction> _roleFunctionRepository,
            IRepository<Role> _roleRepository,
            IRepository<UserRole> _userRoleRepository,
            IRepository<SatelliteMenuRole> _satelliteMenuRoleRepository,
            IRepository<SatelliteMenu> _satelliteMenuRepository
            ) : base(repository)
        {
            this.roleFunctionRepository = _roleFunctionRepository;
            this.roleRepository = _roleRepository;
            this.userRoleRepository = _userRoleRepository;
            this.satelliteMenuRoleRepository = _satelliteMenuRoleRepository;
            this.satelliteMenuRepository = _satelliteMenuRepository;
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageSize">查询条件</param>
        /// <returns></returns>
        public override IPagedList<Function> QueryPage(int pageIndex = 0, int pageSize = int.MaxValue, Expression<Func<Function, bool>> where = null)
        {
            List<Function> query = this.Query(where,0,false,m=>m.Sort);
            var list = new PagedList<Function>(query, pageIndex, pageSize);

            return list;
        }

        public  IPagedList<Function> QueryPage(string name, long parentFunctionId, int type,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = base._repository.Table.Where(m => m.Mark > 0); ;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));

            if (parentFunctionId >0 )
                query = query.Where(m => m.FunctionId == parentFunctionId);

            if (type > 0)
                query = query.Where(m => m.Type == type);

            query = query.OrderBy(m => m.FunctionId).ThenBy(m=>m.Sort);

            var unsortedCategories = query.ToList();
            //sort categories
            var sortedCategories = unsortedCategories.SortFunctionForTree();

            var list = new PagedList<Function>(sortedCategories, pageIndex, pageSize);

            return list;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="where">查询条件，Lambda表达式，如果没有条件请输入 null</param>
        /// <param name="topCount">指定查询的条数，小于等于0表示不使用</param>
        /// <param name="isOrderByDesc">是否倒序排列数据，相当于SQL：ORDER BY DESC  </param>
        /// <param name="orderBy">排序字段，Lambda表达式 </param>
        /// <returns>返回对象集合</returns>
        public override List<Function> Query(Expression<Func<Function, bool>> where, int topCount = 0, bool isOrderByDesc = false, Expression<Func<Function, int?>> orderBy = null)
        {
            if (where == null) where = p => true;
            IQueryable<Function> query = this._repository.Table.Where(where).Where(m => m.Mark > 0);

            if (orderBy == null)
            {
                //不启用排序
                query.OrderBy(m => m.Id);
            }
            else
            {
                //启用排序
                if (!isOrderByDesc && orderBy != null)  query = query.OrderBy(m => m.FunctionId).ThenBy(orderBy);
                else if (isOrderByDesc && orderBy != null) query = query.OrderByDescending(m => m.FunctionId).ThenByDescending(orderBy);
            }

            if (topCount > 0) query = query.Take<Function>(topCount);

            if (query == null) return null;

            var unsortedCategories = query.ToList();
            //sort categories
            var sortedCategories = unsortedCategories.SortFunctionForTree();

            return sortedCategories.ToList<Function>();
        }

        /// <summary>
        /// 根据角色获取功能集合
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public List<Function> QueryByRoleId(long roleId)
        {
            if (roleId <= 0) return null;

            var query = this._repository.Table.Where(
                rel => this.roleFunctionRepository.Table.
                    Where( f => f.RoleId == roleId).
                               Select(t => t.FunctionId).
                              Contains(rel.Id));

            return query.ToList();
        }

        /// <summary>
        /// 根据用户id获取可以访问的功能集合
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public List<Function> QueryByUserId(long userId)
        {
            if (userId <= 0) return null;

            var query = this._repository.Table.Where(
                 f=> f.Mark > 0 &&  this.roleFunctionRepository.Table.Where(
                      fr => fr.Mark > 0 && this.userRoleRepository.Table.Where(
                           ur=>ur.UserinfoId== userId && ur.Mark>0
                          ).Select(ur=>ur.RoleId).Contains(fr.RoleId)
                     ).Select(fr=>fr.FunctionId).Contains(f.Id)
                ).OrderBy(f=>f.FunctionId).ThenBy(f=>f.Sort);

            return query.ToList();
        }

    }
}
