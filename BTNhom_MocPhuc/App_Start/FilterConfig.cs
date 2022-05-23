using System.Web;
using System.Web.Mvc;

namespace BTNhom_MocPhuc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
