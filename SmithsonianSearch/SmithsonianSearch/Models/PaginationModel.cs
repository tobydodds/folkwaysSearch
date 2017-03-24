namespace SmithsonianSearch.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using SmithsonianSearch.Configuration;

    /// <summary>
    ///     The pagination model.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PaginationModel
    {
        #region Fields

        /// <summary>
        ///     The default number of pagination links.
        /// </summary>
        private readonly int defaultNumberOfPaginationLinks = 3;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginationModel" /> class.
        /// </summary>
        public PaginationModel()
        {
            this.NumberOfPaginationLinks = this.defaultNumberOfPaginationLinks;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public PaginationModel(IConfig config)
            : this()
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            this.Config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationModel"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public PaginationModel(GSP result, IConfig config)
            : this(config)
        {
            if (result == null)
            {
                return;
            }

            this.EstimatedNumberOfResults = result.RES != null ? (int)result.RES.M : 0;

            this.PaginationUrl = this.Config.GsaSearchBaseUri.AbsoluteUri;

            IDictionary<string, string> parameters = this.GetParameters(result);

            string paramName = "num";
            if (parameters.ContainsKey(paramName))
            {
                int resultsPerPage;
                int.TryParse(parameters[paramName], out resultsPerPage);
                if (resultsPerPage > 0)
                {
                    this.ResultsPerPage = resultsPerPage;
                }
            }

            paramName = "start";
            if (parameters.ContainsKey(paramName))
            {
                int startItemIndex;
                int.TryParse(parameters[paramName], out startItemIndex);
                if (startItemIndex >= 0)
                {
                    this.StartItemIndex = startItemIndex;
                }
            }

            if (result.RES != null && result.RES.NB != null)
            {
                this.NextPageGsaUrl = result.RES.NB.NU;
                this.PrevPageGsaUrl = result.RES.NB.PU;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationModel"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public PaginationModel(PaginationModel model, IConfig config)
            : this(config)
        {
            if (model == null)
            {
                return;
            }

            this.EstimatedNumberOfResults = model.EstimatedNumberOfResults;
            this.ResultsPerPage = model.ResultsPerPage;
            this.StartItemIndex = model.StartItemIndex;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the config.
        /// </summary>
        public IConfig Config { get; set; }

        /// <summary>
        ///     Gets current page index
        /// </summary>
        public int CurrentPageIndex
        {
            get
            {
                int index = 1;

                if (this.ResultsPerPage > 0)
                {
                    index = (this.StartItemIndex / this.ResultsPerPage) + 1;
                }

                return index;
            }
        }

        /// <summary>
        ///     Gets the estimated number of pages.
        /// </summary>
        public int EstimatedNumberOfPages
        {
            get
            {
                return this.ResultsPerPage != 0 ? this.EstimatedNumberOfResults / this.ResultsPerPage : 0;
            }
        }

        /// <summary>
        ///     Gets or sets the estimated number of results.
        /// </summary>
        public int EstimatedNumberOfResults { get; set; }

        /// <summary>
        ///     Gets a value indicating whether is next page link available.
        /// </summary>
        public bool IsNextPageLinkAvailable
        {
            get
            {
                return !string.IsNullOrEmpty(this.NextPageGsaUrl);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether is previous page link available.
        /// </summary>
        public bool IsPrevPageLinkAvailable
        {
            get
            {
                return !string.IsNullOrEmpty(this.PrevPageGsaUrl);
            }
        }

        /// <summary>
        ///     Gets or sets the next page GSA URL.
        /// </summary>
        public string NextPageGsaUrl { get; set; }

        /// <summary>
        ///     Gets or sets the number of pagination links.
        /// </summary>
        public int NumberOfPaginationLinks { get; set; }

        /// <summary>
        ///     Gets or sets the previous page GSA URL.
        /// </summary>
        public string PrevPageGsaUrl { get; set; }

        /// <summary>
        ///     Gets or sets the number of results per page.
        /// </summary>
        [JsonProperty]
        public int ResultsPerPage { get; set; }

        /// <summary>
        ///     Gets or sets the start item index.
        /// </summary>
        [JsonProperty]
        public int StartItemIndex { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the base navigation URL.
        /// </summary>
        private string PaginationUrl { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get page numbers to display.
        /// </summary>
        /// <param name="maxNumOfPaginationLinks">
        /// The max number of pagination links.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<int> GetPageNumbersToDisplay(int maxNumOfPaginationLinks)
        {
            if (this.EstimatedNumberOfPages == 0)
            {
                yield return 0;
            }
            else
            {
                int currentPage = this.CurrentPageIndex;

                int startFromPage = currentPage > maxNumOfPaginationLinks
                                        ? currentPage - maxNumOfPaginationLinks + 1
                                        : 1;

                for (int pageNum = startFromPage;
                     pageNum <= startFromPage + Math.Min(maxNumOfPaginationLinks, this.EstimatedNumberOfPages);
                     pageNum++)
                {
                    yield return pageNum;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get parameters.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <returns>
        /// The <see cref="IDictionary"/>.
        /// </returns>
        private IDictionary<string, string> GetParameters(GSP results)
        {
            var parameters = new Dictionary<string, string>();

            if (results != null && results.PARAM != null && results.PARAM.Length > 0)
            {
                foreach (GSPPARAM parameter in results.PARAM)
                {
                    string key = parameter.name.ToLowerInvariant();

                    if (!parameters.ContainsKey(key))
                    {
                        parameters.Add(key, parameter.value);
                    }
                }
            }

            return parameters;
        }

        #endregion
    }
}