namespace SmithsonianSearch.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using SmithsonianSearch.Models.Enums;

    /// <summary>
    ///     The config.
    /// </summary>
    public class Config : IConfig
    {
        #region Static Fields

        /// <summary>
        ///     The lock object.
        /// </summary>
        private static readonly object LockObject;

        /// <summary>
        ///     The instance.
        /// </summary>
        private static volatile Config instance;

        #endregion

        #region Fields

        /// <summary>
        ///     The Buy base URI.
        /// </summary>
        private readonly Uri buyBaseUri;

        /// <summary>
        ///     The GSA search base URI.
        /// </summary>
        private readonly Uri gsaSearchBaseUri;

        /// <summary>
        ///     The GSA suggest base URI.
        /// </summary>
        private readonly Uri gsaSuggestBaseUri;

        /// <summary>
        ///     The images base URI.
        /// </summary>
        private readonly Uri imagesBaseUri;

        /// <summary>
        ///     The number of results per page in grid view. Should be even not to break table rows bg colors alternation.
        /// </summary>
        private readonly int pageSizeInTableView = 36;

        /// <summary>
        ///     The number of results per page in tiles view
        /// </summary>
        private readonly int pageSizeInTilesView = 24;

        /// <summary>
        ///     Sorting parameter value for GSA URL: by date ASC
        /// </summary>
        private readonly string sortByDateAsc = "date:A:S:d1";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by date DESC
        /// </summary>
        private readonly string sortByDateDesc = "date:D:S:d1";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by artist ASC
        /// </summary>
        private readonly string sortByArtistAsc = "meta:Artist:A::::";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by artist DESC
        /// </summary>
        private readonly string sortByArtistDesc = "meta:Artist:D::::";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by catalog number ASC
        /// </summary>
        private readonly string sortByCatalogNumberAsc = "meta:CatalogNumber:A::::";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by catalog number DESC
        /// </summary>
        private readonly string sortByCatalogNumberDesc = "meta:CatalogNumber:D::::";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by title ASC
        /// </summary>
        private readonly string sortByTitleAsc = "meta:Title:A::::";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by title DESC
        /// </summary>
        private readonly string sortByTitleDesc = "meta:Title:D::::";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by year ASC
        /// </summary>
        private readonly string sortByYearAsc = "meta:Year:A::::Y";

        /// <summary>
        ///     Sorting parameter value for GSA URL: by year DESC
        /// </summary>
        private readonly string sortByYearDesc = "meta:Year:D::::Y";

        private readonly bool searchSpellingSuggestion = false;

        private readonly FilteringTabsStrategyEnum filteringTabsStrategy;

        private readonly string defaultSearchPhrase;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Config" /> class.
        /// </summary>
        static Config()
        {
            LockObject = new object();
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="Config" /> class from being created.
        /// </summary>
        private Config()
        {
            string searchBaseUrlKey = "GsaUrl";
            string suggestBaseUriKey = "GsaSuggestUrl";
            string imagesBaseUrlKey = "ImagesBaseUrl";
            string buyBaseUrlKey = "BuyBaseUrl";
            string searchSpellingSuggestionKey = "SearchSpellingSuggestion";
            string filteringTabsStrategyKey = "FilteringTabsStrategy";
            string defaultSearchPhraseKey = "DefaultSearchPhrase";

            string searchBaseUrl = ConfigurationManager.AppSettings[searchBaseUrlKey];
            string suggestBaseUrl = ConfigurationManager.AppSettings[suggestBaseUriKey];
            string imagesBaseUrl = ConfigurationManager.AppSettings[imagesBaseUrlKey];
            string buyBaseUrl = ConfigurationManager.AppSettings[buyBaseUrlKey];

            this.gsaSearchBaseUri = !string.IsNullOrEmpty(searchBaseUrl) ? new Uri(searchBaseUrl) : null;
            this.gsaSuggestBaseUri = !string.IsNullOrEmpty(suggestBaseUrl) ? new Uri(suggestBaseUrl) : null;
            this.imagesBaseUri = !string.IsNullOrEmpty(imagesBaseUrl) ? new Uri(imagesBaseUrl) : null;
            this.buyBaseUri = !string.IsNullOrEmpty(buyBaseUrl) ? new Uri(buyBaseUrl) : null;

            bool.TryParse(ConfigurationManager.AppSettings[searchSpellingSuggestionKey] ?? "", out this.searchSpellingSuggestion);
            this.defaultSearchPhrase = ConfigurationManager.AppSettings[defaultSearchPhraseKey];

            var notSetParametersCollection = new List<string>();
            if (this.GsaSearchBaseUri == null)
            {
                notSetParametersCollection.Add(searchBaseUrlKey);
            }

            if (this.GsaSuggestBaseUri == null)
            {
                notSetParametersCollection.Add(suggestBaseUriKey);
            }

            if (this.ImagesBaseUri == null)
            {
                notSetParametersCollection.Add(imagesBaseUrlKey);
            }

            switch (ConfigurationManager.AppSettings[filteringTabsStrategyKey])
            {
                case "A": this.filteringTabsStrategy = FilteringTabsStrategyEnum.RevertToAllTab;
                    break;
                case "B": this.filteringTabsStrategy = FilteringTabsStrategyEnum.HideOtherTabs;
                    break;
                case "C": this.filteringTabsStrategy = FilteringTabsStrategyEnum.DisplayNoResultsMessage;
                    break;
                default: this.filteringTabsStrategy = FilteringTabsStrategyEnum.RevertToAllTab;
                    break;
            }

            if (notSetParametersCollection.Any())
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        "Some required parameters have not been set in app config: {0}", 
                        string.Join(", ", notSetParametersCollection)));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static IConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (LockObject)
                    {
                        if (instance == null)
                        {
                            instance = new Config();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        ///     Gets the Buy base URI.
        /// </summary>
        public Uri BuyBaseUri
        {
            get
            {
                return this.buyBaseUri;
            }
        }

        /// <summary>
        ///     Gets the GSA search base URI.
        /// </summary>
        public Uri GsaSearchBaseUri
        {
            get
            {
                return this.gsaSearchBaseUri;
            }
        }

        /// <summary>
        ///     Gets the GSA suggest base URI.
        /// </summary>
        public Uri GsaSuggestBaseUri
        {
            get
            {
                return this.gsaSuggestBaseUri;
            }
        }

        /// <summary>
        ///     Gets the images base URI.
        /// </summary>
        public Uri ImagesBaseUri
        {
            get
            {
                return this.imagesBaseUri;
            }
        }

        /// <summary>
        ///     Gets max length of result item's field
        /// </summary>
        public int ItemFieldMaxLength
        {
            get
            {
                return 60;
            }
        }

        /// <summary>
        ///     Gets max length of result item's title
        /// </summary>
        public int ItemTitleMaxLength
        {
            get
            {
                return 45;
            }
        }

        /// <summary>
        ///     Gets max length of result item's description
        /// </summary>
        public int ItemDescriptionMaxLength {
            get
            {
                return 120;
            }
        }

        /// <summary>
        /// Gets the page size table view.
        /// </summary>
        public int PageSizeTableView
        {
            get
            {
                return this.pageSizeInTableView;
            }
        }

        /// <summary>
        /// Gets the page size tiles view.
        /// </summary>
        public int PageSizeTilesView
        {
            get
            {
                return this.pageSizeInTilesView;
            }
        }

        /// <summary>
        /// Gets the configuration setting that defines how to treat spelling suggestion: search it instead of original query or not.
        /// </summary>
        public bool SearchSpellingSuggestion
        {
            get
            {
                return this.searchSpellingSuggestion;
            }
        }

        public FilteringTabsStrategyEnum FilteringTabsStrategy
        {
            get
            {
                return this.filteringTabsStrategy;
            }
        }

        public string DefaultSearchPhrase
        {
            get
            {
                return this.defaultSearchPhrase;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get sort parameter value.
        /// </summary>
        /// <param name="sortingOption">
        /// The sorting option.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetSortParameterValue(SortOptionsEnum sortingOption)
        {
            string value = string.Empty;

            switch (sortingOption)
            {
                case SortOptionsEnum.DateAsc:
                    value = this.sortByDateAsc;
                    break;
                case SortOptionsEnum.DateDesc:
                    value = this.sortByDateDesc;
                    break;
                case SortOptionsEnum.ArtistAsc:
                    value = this.sortByArtistAsc;
                    break;
                case SortOptionsEnum.ArtistDesc:
                    value = this.sortByArtistDesc;
                    break;
                case SortOptionsEnum.CatalogNumberAsc:
                    value = this.sortByCatalogNumberAsc;
                    break;
                case SortOptionsEnum.CatalogNumberDesc:
                    value = this.sortByCatalogNumberDesc;
                    break;
                case SortOptionsEnum.TitleAsc:
                    value = this.sortByTitleAsc;
                    break;
                case SortOptionsEnum.TitleDesc:
                    value = this.sortByTitleDesc;
                    break;
                case SortOptionsEnum.YearAsc:
                    value = this.sortByYearAsc;
                    break;
                case SortOptionsEnum.YearDesc:
                    value = this.sortByYearDesc;
                    break;
                default:
                    break;
            }

            return value;
        }

        /// <summary>
        /// Parse 'sort' parameter's value returned by GSA and return relevant <see cref="SortOptionsEnum"/> value
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="SortOptionsEnum"/>.
        /// </returns>
        public SortOptionsEnum GetSortingOptionFromParameter(string value)
        {
            var sortingOption = SortOptionsEnum.Relevance;

            if (value.Equals(this.sortByDateAsc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.DateAsc;
            }
            else if (value.Equals(this.sortByDateDesc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.DateDesc;
            }
            else if (value.Equals(this.sortByArtistAsc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.ArtistAsc;
            }
            else if (value.Equals(this.sortByArtistDesc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.ArtistDesc;
            }
            else if (value.Equals(this.sortByCatalogNumberAsc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.CatalogNumberAsc;
            }
            else if (value.Equals(this.sortByCatalogNumberDesc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.CatalogNumberDesc;
            }
            else if (value.Equals(this.sortByTitleAsc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.TitleAsc;
            }
            else if (value.Equals(this.sortByTitleDesc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.TitleDesc;
            }
            else if (value.Equals(this.sortByYearAsc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.YearAsc;
            }
            else if (value.Equals(this.sortByYearDesc, StringComparison.OrdinalIgnoreCase))
            {
                sortingOption = SortOptionsEnum.YearDesc;
            }

            return sortingOption;
        }

        #endregion

        #region Nested types

        public enum FilteringTabsStrategyEnum
        {
            /// <summary>
            /// Applying a filter reverts to ALL tab
            /// </summary>
            RevertToAllTab = 0,

            /// <summary>
            /// Applying a filter hides all tabs except the current one and ALL
            /// </summary>
            HideOtherTabs = 1,

            /// <summary>
            /// Applying a filter does not affect the set of tabs. Tabs that have no results inside will show "No results" message.
            /// </summary>
            DisplayNoResultsMessage = 2
        }

        #endregion
    }
}