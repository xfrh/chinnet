using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Data
{
    /// <summary>
    ///  SQL查询，参数对象
    ///  用法：
    ///        SpringSqlParameters par = new SpringSqlParameters();
    ///        sqlWhere += " AND Content LIKE @Content ";
    ///        par.Add("@Content", "%" + content + "%");
    ///           
    ///        sqlWhere += " AND Type = @Type ";
    ///        par.Add("@Type", typeName);
    /// </summary>
    public class SpringSqlParameters : DynamicParameters
    {

    }
}