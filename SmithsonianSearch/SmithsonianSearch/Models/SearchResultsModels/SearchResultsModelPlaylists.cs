namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    ///     The search results model playlists.
    /// </summary>
    public class SearchResultsModelPlaylists : SearchResultsModelGeneric<Playlist>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelPlaylists" /> class.
        /// </summary>
        public SearchResultsModelPlaylists()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelPlaylists"/> class.
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
        public SearchResultsModelPlaylists(GSP results, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => new Playlist(r, config))
                             : new Playlist[0];
        }

        #endregion
    }
}