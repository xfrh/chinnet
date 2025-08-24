using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.SystemSet;
using System.Net;
using System.Web;
using ManageSystem.Services.Configuration;
using ManageSystem.Core.Utility;
using ManageSystem.Data;
using ManageSystem.Core.Infrastructure;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;

namespace ManageSystem.Services.SystemSet
{
    /// <summary>
    /// 操作类 ，数据库表名：Area 
    /// </summary>
    public partial class AreaService : BaseService<Area>, IAreaService
    {

        public AreaService(IRepository<Area> repository) : base(repository)
        {

        }

        /// <summary>
        /// 根据区域id获取完整的区域名称，一直向上级查找，使用空格分割
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public string GetFullName(long areaId)
        {
            List<string> list = new List<string>();
            StringBuilder sb = new StringBuilder();
            this.GetFulleNameItem(list, areaId);

            string vlalue = "";
            for (int i = list.Count - 1; i >= 0; i--)
                vlalue += list[i] + " ";

            return vlalue.TrimEnd(' ');
        }

        /// <summary>
        /// 递归获取
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        private void GetFulleNameItem(List<string> list, long areaId)
        {
            var entity = this.QueryEntity(areaId);
            if (entity == null) return;
            list.Add(entity.Name);

            this.GetFulleNameItem(list, entity.ParentId);
        }

        /// <summary>
        /// 根据上级id获取所对应的列表数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Area> QueryByParentId(long parentId)
        {
            return this.Query(m => m.ParentId == parentId).OrderBy(m => m.Sort).ToList();
        }

        /// <summary>
        /// 根据区域id获取的区域名
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public string GetName(long areaId)
        {
            var entity = this.QueryEntity(areaId);

            return (entity == null || entity.Id <= 0) ? "" : entity.Name;
        }

        public int MaxSort()
        {
            IDbContext c = EngineContext.Current.Resolve<IDbContext>();
            DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            int max = c.SqlQuery<int>("SELECT MAX(Sort) FROM dbo.Hospital WHERE [Mark] > 0;").FirstOrDefault();
            con.Close();
            return max;
        }

        public List<Area> GetArea(long parentId)
        {
            using (SqlConnection conn = DapperHelper.GetConnection())
            {
                string sql = string.Format("select * from [dbo].[Area] where Id=@Id");
                List<Area> ateamodel = conn.Query<Area>(sql, new { Id = parentId })?.ToList();
                return ateamodel;
            }
            
        }
    }
}
