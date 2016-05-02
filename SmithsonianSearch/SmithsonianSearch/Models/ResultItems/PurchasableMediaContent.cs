namespace SmithsonianSearch.Models.ResultItems
{
    using System.Collections.Generic;

    using SmithsonianSearch.Configuration;

    /// <summary>
    ///     The purchasable media content.
    /// This model class is used as base class for other models that represent media content items that can be purchased: tracks, albums etc.
    /// </summary>
    public class PurchasableMediaContent : PurchasableResultItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PurchasableMediaContent" /> class.
        /// </summary>
        public PurchasableMediaContent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchasableMediaContent"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public PurchasableMediaContent(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchasableMediaContent"/> class.
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
        public PurchasableMediaContent(GSPRESR result, string redirectUrl, IConfig config)
            : base(result, redirectUrl, config)
        {
            this.Initialize(result, config);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        ///     Gets or sets the year.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Sample audio for tracks
        /// </summary>
        public string SampleAudio { get; set; }

        /// <summary>
        /// Player model for tracks
        /// </summary>
        public PlayerModel Player
        {
            get
            {
                return !string.IsNullOrEmpty(this.SampleAudio) ? new PlayerModel(this.SampleAudio) : null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        private void Initialize(GSPRESR result, IConfig config)
        {
            IDictionary<string, string> tempCollection = this.GetAttributes(result);

            string key = "artist";
            if (tempCollection.ContainsKey(key))
            {
                this.Artist = tempCollection[key];
            }

            if (string.IsNullOrEmpty(this.Artist))
            {
                this.Artist = "Various Artists";
            }

            key = "year";
            if (tempCollection.ContainsKey(key))
            {
                this.Year = tempCollection[key];
            }

            key = "sampleaudio";
            if (tempCollection.ContainsKey(key))
            {
                this.SampleAudio = tempCollection[key];
            }
        }

        #endregion
    }
}