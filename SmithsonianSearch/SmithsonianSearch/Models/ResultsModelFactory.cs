namespace SmithsonianSearch.Models
{
    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.Enums;
    using SmithsonianSearch.Models.ResultItems;
    using SmithsonianSearch.Models.SearchResultsModels;

    /// <summary>
    ///     The results model factory.
    /// </summary>
    public static class ResultsModelFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Build result item.
        /// In case of a new content type do not forget to add a case inside this method.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="searchPageUrl">
        /// The search page url.
        /// </param>
        /// <returns>
        /// The <see cref="GenericResultItem"/>.
        /// </returns>
        public static GenericResultItem BuildResultItem(GSPRESR item, IConfig config, string searchPageUrl = null)
        {
            if (item == null)
            {
                return new GenericResultItem(config);
            }

            if (item.MT == null || item.MT.Length == 0)
            {
                return new GenericResultItem(item, config);
            }

            string contentType = GenericResultItem.GetContentType(item) ?? string.Empty;

            GenericResultItem builtObject = null;

            switch (contentType.ToLowerInvariant())
            {
                case "album":
                    builtObject = new Album(item, searchPageUrl, config);
                    break;
                case "track":
                    builtObject = new Track(item, searchPageUrl, config);
                    break;
                case "article":
                    builtObject = new Article(item, config);
                    break;
                case "video":
                    builtObject = new Video(item, config);
                    break;
                case "playlist":
                    builtObject = new Playlist(item, config);
                    break;
                case "merchandise":
                    builtObject = new Merchandise(item, searchPageUrl, config);
                    break;
                case "toolforteaching":
                    builtObject = new ToolForTeaching(item, config);
                    break;
                default:
                    builtObject = new GenericResultItem(item, config);
                    break;
            }

            return builtObject;
        }

        /// <summary>
        /// Build result model.
        /// In case of a new content type do not forget to add a case inside this method.
        /// </summary>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="searchModel">
        /// The search model.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="searchPageUrl">
        /// The search page URL.
        /// </param>
        /// <returns>
        /// The <see cref="ISearchResultsModel"/>.
        /// </returns>
        public static ISearchResultsModel BuildResultsModel(
            ContentTypesEnum contentType, 
            GSP results, 
            SearchModel searchModel, 
            IConfig config, 
            string searchPageUrl = null)
        {
            ISearchResultsModel model = null;

            switch (contentType)
            {
                case ContentTypesEnum.Album:
                    model = new SearchResultsModelAlbums(results, searchPageUrl, searchModel, config);
                    break;
                case ContentTypesEnum.Article:
                    model = new SearchResultsModelArticles(results, searchModel, config);
                    break;
                case ContentTypesEnum.Track:
                    model = new SearchResultsModelTracks(results, searchPageUrl, searchModel, config);
                    break;
                case ContentTypesEnum.Merchandise:
                    model = new SearchResultsModelMerchandise(results, searchPageUrl, searchModel, config);
                    break;
                case ContentTypesEnum.Playlist:
                    model = new SearchResultsModelPlaylists(results, searchModel, config);
                    break;
                case ContentTypesEnum.ToolForTeaching:
                    model = new SearchResultsModelToolsForTeaching(results, searchModel, config);
                    break;
                case ContentTypesEnum.Video:
                    model = new SearchResultsModelVideos(results, searchModel, config);
                    break;
                case ContentTypesEnum.PurchasableMediaContent:
                    model = new SearchResultsModelPurchasableMedia(results, searchPageUrl, searchModel, config);
                    break;
                default:
                    model = new SearchResultsModelMixed(results, searchPageUrl, searchModel, config);
                    break;
            }

            return model;
        }

        #endregion
    }
}