using System.Web;
using System.Web.Mvc;


namespace ManageSystem.Admin.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckRoleAttribute());
            filters.Add(new CheckIPAttribute());
        }
    }
}