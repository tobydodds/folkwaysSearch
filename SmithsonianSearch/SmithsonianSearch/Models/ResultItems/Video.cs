namespace SmithsonianSearch.Models.ResultItems
{
    using System.Collections.Generic;

    using SmithsonianSearch.Configuration;

    /// <summary>
    /// The video.
    /// Model class that represents an item of 'video' content type.
    /// </summary>
    public class Video : GenericResultItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        public Video()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public Video(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public Video(GSPRESR result, IConfig config)
            : base(result, config)
        {
            this.Initialize(result, config);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public string Duration { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initialize custom properties from GSA results.
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

                string key = "duration";
                if (tempCollection.ContainsKey(key))
                {
                    this.Duration = tempCollection[key];
                }
            }
        }

        #endregion
    }
}