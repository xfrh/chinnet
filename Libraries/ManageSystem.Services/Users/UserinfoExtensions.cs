using ManageSystem.Core.Domain.SystemSet;
using ManageSystem.Core.Domain.Users;
using ManageSystem.Core.Infrastructure;
using ManageSystem.Services.Authentication;
using ManageSystem.Services.SystemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ManageSystem.Services.Users
{
    public class UserinfoExtensions
    {
        /// <summary>
        /// 根据页面地址和当前登录用户检查url是否可以访问
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckFunction(string url)
        {
            try
            {
                //当前登录用户
                IAuthenticationService authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
                var userEntity = authenticationService.GetAuthenticatedUser();
                Userinfo userModel = userEntity as Userinfo;

                //检查登录
                if (userModel == null) return false;

                //检查登录
                if (userModel.FunctionList == null || userModel.FunctionList.Count() <= 0) return false;

                //检查页面是否有权限访问
                var functionService = EngineContext.Current.Resolve<IFunctionService>();
                var httpRequestBase = EngineContext.Current.Resolve<HttpRequestBase>();

                if (httpRequestBase.Url.AbsolutePath.Equals("/")) return true;

                //当前访问的功能
                Function functionModel = functionService.QueryEntity(m => m.Mark > 0 && url.Contains(m.Url.ToLower()));
                if (functionModel == null || string.IsNullOrEmpty(functionModel.Name)) return false;

                //检查用户是否可以访问该功能
                var userFunctionList = userModel.FunctionList.Where(m => m.Id == functionModel.Id);
                if (userFunctionList == null || !userFunctionList.Any()) return false;

                return true;

            }
            catch (Exception)
            {

            }

            return false;
        }

    }
}
