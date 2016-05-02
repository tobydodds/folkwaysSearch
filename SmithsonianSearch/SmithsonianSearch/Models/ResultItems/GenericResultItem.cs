namespace SmithsonianSearch.Models.ResultItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Helpers;

    /// <summary>
    ///     The search result item model.
    /// </summary>
    public class GenericResultItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericResultItem" /> class.
        /// </summary>
        public GenericResultItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResultItem"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public GenericResultItem(IConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            this.Config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResultItem"/> class.
        /// </summary>
        /// <param name="result">
        /// The result deserialized from GSA XML.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public GenericResultItem(GSPRESR result, IConfig config)
            : this(config)
        {
            this.Initialize(result, config);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the config.
        /// </summary>
        public IConfig Config { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the image url.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the type (track, album, article, book etc.).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Gets or sets the url.
        /// </summary>
        public Uri Url { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get content type.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetContentType(GSPRESR item)
        {
            if (item.MT == null || item.MT.Length == 0)
            {
                return string.Empty;
            }

            string contentType =
                item.MT.Where(f => f.N.Equals("ContentType", StringComparison.OrdinalIgnoreCase))
                    .Select(f => f.V)
                    .SingleOrDefault() ?? string.Empty;

            return contentType;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get attributes.
        /// </summary>
        /// <param name="res">
        /// The res.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        protected IDictionary<string, string> GetAttributes(GSPRESR res)
        {
            if (res == null || res.MT == null || res.MT.Length == 0)
            {
                return new Dictionary<string, string>();
            }

            var tempCollection = new Dictionary<string, string>();
            string key = null;
            string value = null;

            foreach (GSPRESRMT meta in res.MT)
            {
                key = meta.N.ToLowerInvariant();
                value = meta.V != null ? meta.V.Trim().RemoveHtmlTags() : null;

                if (!tempCollection.ContainsKey(key))
                {
                    tempCollection.Add(key, value);
                }
                else
                {
                    tempCollection[key] = tempCollection[key] + " | " + value;
                }
            }

            return tempCollection;
        }

        /// <summary>
        /// The get string from meta attributes.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="attributeName">
        /// The attribute name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected string GetStringFromMetaAttributes(GSPRESR item, string attributeName)
        {
            if (item.MT == null || item.MT.Length == 0)
            {
                return string.Empty;
            }

            return
                item.MT.Where(f => f.N.Equals(attributeName, StringComparison.OrdinalIgnoreCase))
                    .Select(f => f.V)
                    .SingleOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Ensure the image URL is absolute URL.
        ///     In case if the received URL is relative we add it to a default images server URI and return the absolute URL.
        /// </summary>
        /// <param name="url">
        /// Image URL received from GSA search
        /// </param>
        /// <param name="serverUri">
        /// URI of default images' server
        /// </param>
        /// <returns>
        /// Possibly modified image's URL
        /// </returns>
        private string ComposeFullImageUrl(string url, Uri serverUri)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            Uri imageUri = null;
            bool isAbsoluteUrl = Uri.TryCreate(url, UriKind.Absolute, out imageUri);

            if (!isAbsoluteUrl)
            {
                var ub = new UriBuilder(serverUri.Scheme, serverUri.Host, serverUri.Port, url);
                imageUri = ub.Uri;
            }

            return imageUri.AbsoluteUri;
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Config must not be null
        /// </exception>
        private void Initialize(GSPRESR result, IConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            IDictionary<string, string> tempCollection = this.GetAttributes(result);

            this.Config = config;

            string key = "title";
            if (tempCollection.ContainsKey(key))
            {
                this.Title = tempCollection[key];
            }

            key = "imageurl";
            if (tempCollection.ContainsKey(key))
            {
                string imageUrl = tempCollection[key];
                this.ImageUrl = this.ComposeFullImageUrl(imageUrl, config.ImagesBaseUri);
            }

            key = "infotext";
            if (tempCollection.ContainsKey(key))
            {
                this.Description = tempCollection[key];
            }

            this.Url = new Uri(result.U);

            this.Type = GetContentType(result);
        }

        #endregion
    }
}