namespace SmithsonianSearch.Models.ResultItems
{
    using SmithsonianSearch.Configuration;

    /// <summary>
    ///     The merchandise.
    /// Model class that represents an item of 'merchandise' content type.
    /// </summary>
    public class Merchandise : PurchasableResultItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Merchandise" /> class.
        /// </summary>
        public Merchandise()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Merchandise"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public Merchandise(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Merchandise"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="redirectUrl">
        /// The redirect url.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public Merchandise(GSPRESR result, string redirectUrl, IConfig config)
            : base(result, redirectUrl, config)
        {
        }

        #endregion
    }
}