namespace SmithsonianSearch.Models.ResultItems
{
    using System;
    using System.Collections.Generic;

    using SmithsonianSearch.Configuration;

    /// <summary>
    ///     The track.
    /// Model class that represents an item of 'track' content type.
    /// </summary>
    public class Track : PurchasableMediaContent
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Track" /> class.
        /// </summary>
        public Track()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public Track(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
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
        public Track(GSPRESR result, string redirectUrl, IConfig config)
            : base(result, redirectUrl, config)
        {
            if (result == null)
            {
                return;
            }

            if (result.MT != null && result.MT.Length > 0)
            {
                string key = null;

                IDictionary<string, string> tempCollection = this.GetAttributes(result);

                key = "albumtitle";
                if (tempCollection.ContainsKey(key))
                {
                    this.AlbumTitle = tempCollection[key];
                }

                key = "sampleaudio";
                if (tempCollection.ContainsKey(key))
                {
                    this.SampleAudio = tempCollection[key];
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the album title.
        /// </summary>
        public string AlbumTitle { get; set; }

        /// <summary>
        ///     Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        #endregion
    }
}