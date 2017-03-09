using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard;
using Orchard.Mvc.Filters;

namespace Folkways.Filters
{
    public class LayoutFilter : FilterProvider, IResultFilter
    {
        private readonly IWorkContextAccessor _wca;

        public LayoutFilter(IWorkContextAccessor wca)
        {
            _wca = wca;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var workContext = _wca.GetContext();
            var routeValues = filterContext.RouteData.Values;
            workContext.Layout.Metadata.Alternates.Add(
                BuildShapeName(routeValues, "area"));
            workContext.Layout.Metadata.Alternates.Add(
                BuildShapeName(routeValues, "area", "controller"));
            workContext.Layout.Metadata.Alternates.Add(
                BuildShapeName(routeValues, "area", "controller", "action"));
            //check if this is the login screen, look for layout-logon.cshtml layout
            if ((new string[] { "~/Users/Account/LogOn", "~/Users/Account/AccessDenied" }).Contains(((workContext.HttpContext).Request).AppRelativeCurrentExecutionFilePath))
            {
                workContext.Layout.Metadata.Alternates.Add("Layout__LogOn");
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        private static string BuildShapeName(
            RouteValueDictionary values, params string[] names)
        {

            return "Layout__" +
                string.Join("__",
                    names.Select(s =>
                        ((string)values[s] ?? "").Replace(".", "_")));
        }
    }
}