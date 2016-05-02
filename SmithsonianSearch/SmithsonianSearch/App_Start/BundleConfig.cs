namespace SmithsonianSearch.App_Start
{
    using System.Web.Optimization;

    /// <summary>
    /// The scripts and styles bundle config.
    /// </summary>
    public class BundleConfig
    {
        #region Public Methods and Operators

        /// <summary>
        /// The register bundles.
        /// </summary>
        /// <param name="bundles">
        /// The bundles.
        /// </param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-1.10.2.min.js",
                "~/Scripts/jquery-ui-1.10.2.min.js",
                "~/Scripts/jquery.ui.autocomplete.min",
                "~/Scripts/jquery.history.js",
                "~/Scripts/jquery.mobile.custom.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include("~/Scripts/bootstrap.min.js", "~/Scripts/bootstrap-hover-dropdown.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/folkways").Include("~/Scripts/index.js"));

            bundles.Add(new StyleBundle("~/css/fonts").Include("~/css/fonts.css"));

            bundles.Add(new StyleBundle("~/css/jquery").Include(
                "~/css/jquery-ui.min.css",
                "~/css/jquery.mobile.min.css"));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include("~/css/bootstrap.css"));

        }

        #endregion
    }
}