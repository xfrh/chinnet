using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.WebApi;
using ManageSystem.Services.Medicine;
using ManageSystem.Services.ScoringModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ManageSystem.InnerApi.Controllers
{
    public class ScoringModuleController : ApiController
    {
        private readonly IDataQualityScoreService _scoreService;
        public ScoringModuleController()
        {
            _scoreService = EngineContext.Current.Resolve<IDataQualityScoreService>();
        }

        [HttpGet, Route("v1/scoremd/page_init")]
        public HttpResponseMessage Page_Init(long id = 0)
        {
            try
            {
                _scoreService.HospitalAutoPageInit(id);
                return ApiResult.Success("完成");
            }
            catch (Exception ex)
            {
                Core.Utility.Log4Helper.Debug(GetType(), ex);
                return ApiResult.Error(ex.Message);
            }
        }
    }
}
