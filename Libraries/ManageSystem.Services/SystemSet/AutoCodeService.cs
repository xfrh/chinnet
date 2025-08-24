using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Articles;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Data;
using System.Data.Common;
using ManageSystem.Core.Infrastructure;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace ManageSystem.Services.SystemSet
{
    /// <summary>
    /// 操作类 ，数据库表名：AutoCode 
    /// </summary>
    public partial class AutoCodeService : BaseService<AutoCode>, IAutoCodeService
    {

        public AutoCodeService(IRepository<AutoCode> repository) : base(repository)
        {

        }

        /// <summary>
        /// 根据类型获取一个自动生成的编号
        /// </summary>
        /// <param name="type">编号类型</param>
        /// <returns></returns>
        public string GetCode(AutoCodeType type)
        {
            try
            {
                IDbContext c = EngineContext.Current.Resolve<IDbContext>();
                DbConnection con = ((IObjectContextAdapter)c).ObjectContext.Connection;

                // 多参数写法 var country = "Australia";   var keyWords = "Beach, Sun"; var destinations = context.Database.SqlQuery<DestinationSummary>("dbo.GetDestinationSummary @p0, @p1", country, keyWords);
                var str = c.SqlQuery<string>("[dbo].[sp_CreateCode] @p0", (int)type).ToList();

                string code =  str[0];
                return code;
            }
            catch (Exception ex)
            {

            }

            return "";
        }
    }
}
