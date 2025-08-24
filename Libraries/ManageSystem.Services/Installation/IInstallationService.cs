using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageSystem.Services.Installation
{
    public partial interface IInstallationService
    {
        void InstallData(bool installSampleData = true);
    }
}
