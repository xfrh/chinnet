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
    /// 操作类 ，数据库表名：DataGermYear 
    /// </summary>
    public partial class DataGermYearService : BaseService<DataGermYear>, IDataGermYearService
    {
        private readonly HttpContextBase httpContext;
        private readonly IAuthenticationService iauthenticationService;

        public DataGermYearService(IRepository<DataGermYear> actionLogRepository,
            HttpContextBase _httpContext,
             IAuthenticationService _iauthenticationService) :
            base(actionLogRepository)
        {
            this.httpContext = _httpContext;
            this.iauthenticationService = _iauthenticationService;
        }


        /// <summary>
        /// 根据细菌名称和年份的名称查询数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="germName">细菌名称</param>
        /// <returns></returns>
        public List<DataGermYear> Query(int year, string germName)
        {
            var data = this.Query().ToList();
            if (year > 0)
                data = data.Where(m => m.Year == year).ToList();

            if (!string.IsNullOrWhiteSpace(germName))
                data = data.Where(m => m.GermName.ToLower().Equals(germName.ToLower())).ToList();

            return data.OrderBy(m => m.Year).ToList();
        }

        /// <summary>
        /// 获取所有的年份
        /// </summary>
        /// <returns></returns>
        public List<string> QueryYear()
        {
            var data = this.Query(m => m.Mark > 0).GroupBy(m => m.Year).Select(m => m.FirstOrDefault()).OrderByDescending(m => m.Year).ToList();

            List<string> result = new List<string>();
            foreach (var item in data)
            {
                if (item.Year <= 0)
                    continue;

                result.Add(item.Year.ToString());
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

        /// <summary>
        /// 根据阶段名称获取数据
        /// </summary>
        /// <param name="stage">阶段名称</param>
        /// <returns></returns>
        public List<DataGermYear> QueryByStage(string stage)
        {
            return this.Query(m => m.Mark > 0 && m.Stage.ToLower().Equals(stage.ToLower())).OrderByDescending(m => m.Year).ToList();
        }


    }
}
