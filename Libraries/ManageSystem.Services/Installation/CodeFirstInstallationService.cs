using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Media;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Installation
{

    /// <summary>
    /// 该类只是个demo，不做使用，通过sql脚本来初始化数据
    /// </summary>
    public partial class CodeFirstInstallationService : IInstallationService
    {
        private readonly IRepository<Userinfo> userinfoRepository;

        public CodeFirstInstallationService(IRepository<Userinfo> _userinfoRepository)
        {
            this.userinfoRepository = _userinfoRepository;
        }

        /// <summary>
        /// 初始化用户数据
        /// </summary>
        protected virtual void InstallUserinfo()
        {
          
        }

        /// <summary>
        /// 初始化设置
        /// </summary>
        protected virtual void InstallSettings()
        {
          
        }

        public void InstallData(bool installSampleData = true)
        {
            //this.InstallUserinfo();
            //this.InstallSettings();
        }
    }
}
