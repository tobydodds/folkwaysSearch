namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    ///     The search results model tools for teaching.
    /// </summary>
    public class SearchResultsModelToolsForTeaching : SearchResultsModelGeneric<ToolForTeaching>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelToolsForTeaching" /> class.
        /// </summary>
        public SearchResultsModelToolsForTeaching()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelToolsForTeaching"/> class.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="searchModel">
        /// The search model.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchResultsModelToolsForTeaching(GSP results, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => new ToolForTeaching(r, config))
                             : new ToolForTeaching[0];
        }

        #endregion
    }
}