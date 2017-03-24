namespace SmithsonianSearch
{
    using System.Web.Mvc;

    using StackExchange.Profiling.MVCHelpers;

    /// <summary>
    /// The filter config.
    /// </summary>
    public class FilterConfig
    {
        #region Public Methods and Operators

        /// <summary>
        /// The register global filters.
        /// </summary>
        /// <param name="filters">
        /// The filters.
        /// </param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ProfilingActionFilter());
        }

        #endregion
    }
}