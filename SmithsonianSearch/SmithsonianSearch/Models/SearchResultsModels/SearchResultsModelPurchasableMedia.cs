namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.Enums;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    /// The search results model purchasable media.
    /// </summary>
    public class SearchResultsModelPurchasableMedia : SearchResultsModelGeneric<PurchasableMediaContent>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelPurchasableMedia"/> class.
        /// </summary>
        public SearchResultsModelPurchasableMedia()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelPurchasableMedia"/> class.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="selectedView">
        /// The selected view.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchResultsModelPurchasableMedia(GSP results, string redirectUrl, SearchModel searchModel, IConfig config)
            : base(results, searchModel, config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = (results.RES != null && results.RES.R != null)
                             ? results.RES.R.Select(r => new PurchasableMediaContent(r, redirectUrl, config))
                             : new PurchasableMediaContent[0];
        }

        #endregion
    }
}