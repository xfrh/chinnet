using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ManageSystem.Core.Data;
using ManageSystem.Core.Domain.Log;
using System.Web;
using ManageSystem.Services.Authentication;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core;
using ManageSystem.Core.Utility;
using ManageSystem.Core.Domain.Datas;

namespace ManageSystem.Services.Datas
{
    /// <summary>
    /// 操作类 ，数据库表名：DataAntibioticDrugFast 
    /// </summary>
    public partial class DataAntibioticDrugFastService : BaseService<DataAntibioticDrugFast>, IDataAntibioticDrugFastService
    {
        private readonly HttpContextBase httpContext;
        private readonly IAuthenticationService iauthenticationService;

        public DataAntibioticDrugFastService(IRepository<DataAntibioticDrugFast> actionLogRepository,
            HttpContextBase _httpContext,
             IAuthenticationService _iauthenticationService) :
            base(actionLogRepository)
        {
            this.httpContext = _httpContext;
            this.iauthenticationService = _iauthenticationService;
        }


        /// <summary>
        /// 根据细菌名称和抗生素的名称查询数据
        /// </summary>
        /// <param name="antibioticName">抗生素名称</param>
        /// <param name="germName">细菌名称</param>
        /// <returns></returns>
        public List<DataAntibioticDrugFast> Query(string antibioticName, string germName)
        {
            var data = this.Query().ToList();
            if (!string.IsNullOrWhiteSpace(antibioticName))
                data = data.Where(m => m.AntibioticName.ToLower().Equals(antibioticName.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(germName))
                data = data.Where(m => m.GermName.ToLower().Equals(germName.ToLower())).ToList();

            return data.OrderBy(m => m.Sort).ToList();
        }

        /// <summary>
        /// 获取所有的抗生素名称
        /// </summary>
        /// <returns></returns>
        public List<string> QueryAntibiotic()
        {
            var data = this.Query(m => m.Mark > 0).GroupBy(m => m.AntibioticName).Select(m=>m.FirstOrDefault()).ToList();

            List<string> result = new List<string>();
            foreach (var item in data)
            {
                if (string.IsNullOrWhiteSpace(item.AntibioticName))
                    continue;

                result.Add(item.AntibioticName);
            }

            return result;
        }

        /// <summary>
        /// 获取所有的细菌名称
        /// </summary>
        /// <returns></returns>
        public List<string> QueryGerm()
        {
            var data = this.Query(m => m.Mark > 0).GroupBy(m => m.GermName).Select(m => m.FirstOrDefault()).ToList();

            List<string> result = new List<string>();
            foreach (var item in data)
            {
                if (string.IsNullOrWhiteSpace(item.GermName))
                    continue;

                result.Add(item.GermName);
            }

            return result;
        }



    }
}
