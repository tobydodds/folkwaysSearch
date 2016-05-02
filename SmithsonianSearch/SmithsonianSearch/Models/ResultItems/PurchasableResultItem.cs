namespace SmithsonianSearch.Models.ResultItems
{
    using System.Collections.Generic;

    using SmithsonianSearch.Configuration;

    /// <summary>
    ///     The PurchasableResultItem class represents an item that can be purchased.
    /// This model class is used as base class for other models that represent items that can be purchased.
    /// </summary>
    public class PurchasableResultItem : GenericResultItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PurchasableResultItem" /> class.
        /// </summary>
        public PurchasableResultItem()
        {
            this.PurchasableOptions = new List<PurchasableItemOption>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchasableResultItem"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public PurchasableResultItem(IConfig config)
            : base(config)
        {
            this.PurchasableOptions = new List<PurchasableItemOption>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchasableResultItem"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="redirectUrl">
        /// The redirect Url.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public PurchasableResultItem(GSPRESR result, string redirectUrl, IConfig config)
            : base(result, config)
        {
            this.PurchasableOptions = new List<PurchasableItemOption>();

            this.Initialize(result, redirectUrl, config);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the purchasable options.
        /// </summary>
        public IList<PurchasableItemOption> PurchasableOptions { get; set; }

        /// <summary>
        ///     Gets or sets URL to be redirected back to after purchase (required by the store logic)
        /// </summary>
        public string RedirectUrl { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The initialize.
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
        private void Initialize(GSPRESR result, string redirectUrl, IConfig config)
        {
            IDictionary<string, string> tempCollection = this.GetAttributes(result);

            this.RedirectUrl = redirectUrl;

            this.ReadPurchasableOptionsFromAttributes(tempCollection);
        }

        /// <summary>
        /// The read purchasable options from attributes.
        /// </summary>
        /// <param name="attributes">
        /// The attributes.
        /// </param>
        private void ReadPurchasableOptionsFromAttributes(IDictionary<string, string> attributes)
        {
            if (this.PurchasableOptions == null)
            {
                this.PurchasableOptions = new List<PurchasableItemOption>();
            }

            string key = null;

            var formats = new[]
                              {
                                  new { Suffix = "trackdownload", Title = "Digital Download" }, 
                                  new { Suffix = "cd", Title = "CD" }, 
                                  new { Suffix = "customcd", Title = "Custom CD" }, 
                                  new { Suffix = "albumdownload", Title = "Digital Download" }, 
                                  new { Suffix = "boxset", Title = "Box Set" }, 
                                  new { Suffix = "lp", Title = "LP" }
                              };

            foreach (var format in formats)
            {
                var purchasableOption = new PurchasableItemOption { Format = format.Title };

                key = "price" + format.Suffix;
                if (attributes.ContainsKey(key))
                {
                    purchasableOption.Price = attributes[key];
                }

                key = "buylink" + format.Suffix;
                if (attributes.ContainsKey(key))
                {
                    purchasableOption.Id = attributes[key];
                }

                if (!string.IsNullOrEmpty(purchasableOption.Id))
                {
                    this.PurchasableOptions.Add(purchasableOption);
                }
            }
        }

        #endregion
    }
}