namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    ///     The search results model tracks.
    /// </summary>
    public class SearchResultsModelTracks : SearchResultsModelGeneric<Track>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelTracks" /> class.
        /// </summary>
        public SearchResultsModelTracks()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelTracks"/> class.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="redirectUrl">
        /// The redirect url.
        /// </param>
        /// <param name="searchModel">
        /// The search model.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchResultsModelTracks(GSP results, string redirectUrl, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => new Track(r, redirectUrl, config))
                             : new Track[0];
        }

        #endregion
    }
}