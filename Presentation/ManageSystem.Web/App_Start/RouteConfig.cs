using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManageSystem.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapMvcAttributeRoutes();

            //加载重写
            // routes.Add(new MyRoute());

            //http://localhost:56249/
            routes.MapRoute(
             "Route_Index",
             "",
             new { area = "", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "ManageSystem.Web.Controllers" }
         );


            routes.MapRoute(
       "SatelliteMap",
       "{id}",
       new { controller = "Satellite", action = "ReturnSatelliteView", id = "" },
       new[] { "ManageSystem.Web.Controllers" }
         );

            routes.MapRoute(
           "pageDefault",
           "{controller}/{action}/{id}",
           new { controller = "Home", action = "Index", id = "" },
           new[] { "ManageSystem.Web.Controllers" });


            routes.MapRoute(
             "Default",
             "{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = "" },
             new[] { "ManageSystem.Web.Controllers" }
         );





        }
    }

    public class MyRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return null;

            //var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径
            //var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            //data.Values.Add("controller", "Category");
            //data.Values.Add("action", "ShowCategory");
            //data.Values.Add("id", 1);

            //return null;//返回这个路由值将调用CategoryController.ShowCategory(category.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (values.ContainsKey("id"))
            {
                if (values["id"].ToString() != "henan")
                    return null;
               
            }
            else return null;

            string controller = values["controller"].ToString();
            string action = values["action"].ToString();

            //处理Html.Action传递多参数
            //<a href="@Url.Action("About","home",new{articleId= "asldjfsjd", phone = "13588877234" , address = "武侯大道" ,pageIndex=1,pageSize=10})">关于2</a>
            StringBuilder sb = new StringBuilder();
            foreach (var item in values.Keys)
            {
                if (item.Equals("controller") || item.Equals("action"))
                {
                    continue;
                }
                var _kvalue = values[item];
                sb.Append("&" + item + "=" + _kvalue);
            }
            //string url = controller + "/" + action + "?uck=" + token + sb.ToString();
            string url = controller + "/" + action;

            return new VirtualPathData(this, url);
        }
    }
}
