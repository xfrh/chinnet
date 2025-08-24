using Dapper;
using ICSharpCode.SharpZipLib.Zip;
using ManageSystem.Core;
using ManageSystem.Core.Caching;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Domain.Medicine;
using ManageSystem.Core.Domain.Members;
using ManageSystem.Core.Domain.Messages;
using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Utility.Excel;
using ManageSystem.Data;
using ManageSystem.Services.Log;
using ManageSystem.Services.Members;
using ManageSystem.Services.Messages;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ManageSystem.Services.Medicine
{
    /// <summary>
    /// 操作类 ，数据库表名：MedicalData 
    /// </summary>
    public partial class MedicalDataProjectService : BaseService<MedicalDataProject>, IMedicalDataProjectService
    {

        public MedicalDataProjectService(IRepository<MedicalDataProject> repository) : base(repository)
        {
        }

        public IPagedList<MedicalDataProject> QueryPage(string name, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            string sql = " select * from  MedicalDataProject  ";
            string where = " AND  Mark > 0  ";
            SpringSqlParameters par = new SpringSqlParameters();
            var query = this._repository.Table.Where(m => m.Mark > 0);


            if (!string.IsNullOrWhiteSpace(name))
            {
                where += " AND Name = @Name ";
                par.Add("Name", name);
            }


            return new DapperPageHelper().QueryPage<MedicalDataProject>(sql, where, " ORDER BY Sort ASC, InsertTime ASC, Id DESC ", "  MedicalDataProject ", par, pageIndex, pageSize);
        }

        public string GetProjectItemName(string projectStr)
        {
            projectStr = projectStr.Replace("[", "(");
            projectStr = projectStr.Replace("]", ")");

            if (!string.IsNullOrWhiteSpace(projectStr) && projectStr != "()")
            {
                string sql = " select * from  MedicalDataProject  ";
                string where = " AND  Mark > 0  ";
                SpringSqlParameters par = new SpringSqlParameters();
                var query = this._repository.Table.Where(m => m.Mark > 0);


                where += " AND Id in " + projectStr;


                List<MedicalDataProject> list = new DapperPageHelper().QueryPage<MedicalDataProject>(sql, where, " ORDER BY Sort ASC, InsertTime ASC, Id DESC ", "  MedicalDataProject ", par).AsList();
                return string.Join(",", list.Select(x => x.Name).ToArray());
            }
            else
                return "";

        }
    }
}
