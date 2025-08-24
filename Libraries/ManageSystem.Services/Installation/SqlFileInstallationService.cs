using ManageSystem.Core;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Installation
{
    public partial class SqlFileInstallationService : IInstallationService
    {
        #region Fields


        private readonly IRepository<Userinfo> _customerRepository;
        private readonly IDbContext _dbContext;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public SqlFileInstallationService(
            IRepository<Userinfo> customerRepository,
            IDbContext dbContext,
            IWebHelper webHelper)
        {
            this._customerRepository = customerRepository;
            this._dbContext = dbContext;
            this._webHelper = webHelper;
        }

        #endregion

        #region Utilities


        protected virtual void ExecuteSqlFile(string path)
        {
            var statements = new List<string>();
            
            using (var stream =File.Open(path,FileMode.Open) )
            using (var reader = new StreamReader(stream, System.Text.Encoding.Default))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                    statements.Add(statement);
            }

            foreach (string stmt in statements)
                _dbContext.ExecuteSqlCommand(stmt);
        }

        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();

                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Methods

        public virtual void InstallData( bool installSampleData = true)
        {
            ExecuteSqlFile(_webHelper.MapPath("~/App_Data/Install/create_required_data.sql"));
           
            //if (installSampleData)
            //{
            //    ExecuteSqlFile(_webHelper.MapPath("~/App_Data/Install/create_sample_data.sql"));
            //}
        }

        #endregion
    }
}
