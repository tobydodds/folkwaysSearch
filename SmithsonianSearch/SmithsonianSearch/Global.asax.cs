namespace SmithsonianSearch
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using SmithsonianSearch.App_Start;

    using StackExchange.Profiling;

    /// <summary>
    ///     The MVC application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        #region Methods

        /// <summary>
        /// The application_ begin request.
        /// </summary>
        protected void Application_BeginRequest()
        {
            if (this.Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        /// <summary>
        /// The application_ end request.
        /// </summary>
        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        /// <summary>
        ///     The application_ start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        #endregion
    }
}