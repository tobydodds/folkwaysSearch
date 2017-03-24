namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    ///     The search results model.
    /// </summary>
    public class SearchResultsModelMixed : SearchResultsModelGeneric<GenericResultItem>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelMixed" /> class.
        /// </summary>
        public SearchResultsModelMixed()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelMixed"/> class.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="searchPageUrl">
        /// The search page url.
        /// </param>
        /// <param name="searchModel">
        /// The search model.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchResultsModelMixed(GSP results, string searchPageUrl, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => ResultsModelFactory.BuildResultItem(r, config, searchPageUrl))
                             : new GenericResultItem[0];
        }

        #endregion
    }
}