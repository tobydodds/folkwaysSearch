namespace SmithsonianSearch.Models.ResultItems
{
    using SmithsonianSearch.Configuration;

    /// <summary>
    /// The purchasable item option.
    /// Model class that represents a buying option for an item (format-price).
    /// The class is able to build a buy link URL.
    /// </summary>
    public class PurchasableItemOption
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public string Price { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The compose buy url.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string ComposeBuyUrl(IConfig config)
        {
            return !string.IsNullOrEmpty(this.Id) ? config.BuyBaseUri.AbsoluteUri + "&buyid=" + this.Id : string.Empty;
        }

        #endregion
    }
}