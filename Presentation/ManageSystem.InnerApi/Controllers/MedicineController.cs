using ManageSystem.Core.Domain.Log;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Core.WebApi;
using ManageSystem.Services.Log;
using ManageSystem.Services.Medicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ManageSystem.InnerApi.Controllers
{
    /// <summary>
    /// 医学数据相关的接口
    /// </summary>
    public class MedicineController : ApiController
    {
        private readonly IMedicalDataService MedicalDataService;
        private readonly IMedicalAntibioticResultService MedicalAntibioticResultService;
        private readonly IActionLogService ActionLogService;

        public MedicineController()
        {
            this.MedicalDataService = EngineContext.Current.Resolve<IMedicalDataService>();
            this.ActionLogService = EngineContext.Current.Resolve<IActionLogService>();
            this.MedicalAntibioticResultService = EngineContext.Current.Resolve<IMedicalAntibioticResultService>();
        }

        /// <summary>
        /// 对数据进行分析，上传完成以后调用
        /// </summary>
        /// <param name="id">上传的医学数据Id</param>
        /// <returns></returns>
        [HttpGet, Route("V1/Medicine/Analyze")]
        public HttpResponseMessage Analyze(long id = 0)
        {
            try
            {
                var result = this.MedicalDataService.AutoDispose(id);

                return ApiResult.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 数据耐药性计算，自动化服务调用
        /// </summary>
        /// <param name="id">医学数据的id，如果传递了则获取指定的数据，没有传递则系统查询所有数据</param>
        /// <returns></returns>
        [HttpGet, Route("V1/Medicine/DrugFast")]
        public HttpResponseMessage DrugFast(long id = 0)
        {
            try
            {
                string result = this.MedicalAntibioticResultService.AutoDispose(null, id);
                return ApiResult.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 生成容错文件
        /// </summary>
        /// <param name="id">医学数据的id，如果传递了则获取指定的数据，没有传递则系统查询所有数据</param>
        /// <returns></returns>
        [HttpGet, Route("V1/Medicine/NewFile")]
        public HttpResponseMessage NewFile(long id = 0)
        {
            try
            {
                string result = this.MedicalDataService.CreateNewFile(id);
                return ApiResult.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// 校验用户上传的数据并生成容错文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("V1/Medicine/ValidData")]
        public HttpResponseMessage ValidData(long id = 0)
        {
            if (id <= 0)
            {
                return ApiResult.Error("未获取医学数据ID");
            }

            try
            {
                //重置文件内容-系统会自动再次生成的
                string result = this.MedicalDataService.OneAutoDispose(id);
                return ApiResult.Success(result);

                //return ApiResult.Success(string.Empty);
            }
            catch (Exception ex)
            {
                return ApiResult.Error(ex.Message);
            }
        }
    }
}
