namespace SmithsonianSearch.Models.ResultItems
{
    using System.Collections.Generic;

    using SmithsonianSearch.Configuration;

    /// <summary>
    ///     The album.
    /// Model class that represents an item of 'album' content type.
    /// </summary>
    public class Album : PurchasableMediaContent
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Album" /> class.
        /// </summary>
        public Album()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Album"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public Album(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Album"/> class from GSA results.
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
        public Album(GSPRESR result, string redirectUrl, IConfig config)
            : base(result, redirectUrl, config)
        {
            this.Initialize(result, config);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the catalog number.
        /// </summary>
        public string CatalogNumber { get; set; }

        /// <summary>
        ///     Gets or sets the format.
        /// </summary>
        public string Format { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize custom properties.
        /// Initialize new properties from metadata here.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        private void Initialize(GSPRESR result, IConfig config)
        {
            if (result != null && result.MT != null && result.MT.Length > 0)
            {
                IDictionary<string, string> tempCollection = this.GetAttributes(result);

                string key = "catalognumber";
                if (tempCollection.ContainsKey(key))
                {
                    this.CatalogNumber = tempCollection[key];
                }
            }
        }

        #endregion
    }
}