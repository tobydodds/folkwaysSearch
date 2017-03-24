namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    ///     The search results model videos.
    /// </summary>
    public class SearchResultsModelVideos : SearchResultsModelGeneric<Video>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelVideos" /> class.
        /// </summary>
        public SearchResultsModelVideos()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelVideos"/> class.
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
        public SearchResultsModelVideos(GSP results, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => new Video(r, config))
                             : new Video[0];
        }

        #endregion
    }
}