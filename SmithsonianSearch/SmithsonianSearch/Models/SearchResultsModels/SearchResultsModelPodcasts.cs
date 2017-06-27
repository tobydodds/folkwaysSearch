namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    ///     The search results model Podcasts.
    /// </summary>
    public class SearchResultsModelPodcasts : SearchResultsModelGeneric<Podcast>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelPodcasts" /> class.
        /// </summary>
        public SearchResultsModelPodcasts()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelPodcasts"/> class.
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
        public SearchResultsModelPodcasts(GSP results, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => new Podcast(r, config))
                             : new Podcast[0];
        }

        #endregion
    }
}