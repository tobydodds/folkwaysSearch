namespace SmithsonianSearch.Models.ResultItems
{
    using System.Collections.Generic;

    using SmithsonianSearch.Configuration;

    /// <summary>
    ///     The article.
    /// </summary>
    public class Article : GenericResultItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Article" /> class.
        /// </summary>
        public Article()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Article"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public Article(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Article"/> class from GSA results.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public Article(GSPRESR result, IConfig config)
            : base(result, config)
        {
            this.Initialize(result, config);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     Gets or sets the category.
        /// </summary>
        public string Category { get; set; }

        #endregion

        #region Methods

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
            if (result.MT != null && result.MT.Length > 0)
            {
                string key = null;

                IDictionary<string, string> tempCollection = this.GetAttributes(result);

                key = "author";
                if (tempCollection.ContainsKey(key))
                {
                    this.Author = tempCollection[key];
                }

                key = "category";
                if (tempCollection.ContainsKey(key))
                {
                    this.Category = tempCollection[key];
                }
            }
        }

        #endregion
    }
}