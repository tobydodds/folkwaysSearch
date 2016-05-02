namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    ///     The search results model articles.
    /// </summary>
    public class SearchResultsModelArticles : SearchResultsModelGeneric<Article>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelArticles" /> class.
        /// </summary>
        public SearchResultsModelArticles()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelArticles"/> class.
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
        public SearchResultsModelArticles(GSP results, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => new Article(r, config))
                             : new Article[0];
        }

        #endregion
    }
}