namespace SmithsonianSearch.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Web;

    using Elmah;

    using Newtonsoft.Json;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.Enums;

    /// <summary>
    ///     The search model. It contains all user choices about search parameters (query, filters, tab, sorting etc.).
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SearchModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchModel" /> class.
        /// </summary>
        public SearchModel()
        {
            this.FiltersModel = new FiltersModel();
            this.PaginationModel = new PaginationModel();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModel"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchModel(IConfig config)
            : this()
        {
            this.PaginationModel = new PaginationModel(config);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModel"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchModel(SearchModel model, IConfig config)
        {
            this.Query = model.Query;
            this.FiltersModel = new FiltersModel(model.FiltersModel);
            this.PaginationModel = new PaginationModel(model.PaginationModel, config);
            this.SelectedView = model.SelectedView;
            this.SortingOption = model.SortingOption;
            this.SpellingSuggestionSearchRestricted = model.SpellingSuggestionSearchRestricted;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModel"/> class.
        /// Copies data from GSA results to know what search parameters were used for these search results.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="selectedView">
        /// The selected view.
        /// </param>
        /// <param name="availableContentTypes">
        /// The available content types.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchModel(
            GSP results, 
            ViewOptionsEnum selectedView, 
            IEnumerable<ContentTypesEnum> availableContentTypes,
            bool searchSpellingSuggestionRestricted,
            IConfig config) : this(config)
        {
            if (results != null)
            {
                this.Query = results.Q;
                this.FiltersModel = new FiltersModel(results, availableContentTypes);
                this.PaginationModel = new PaginationModel(results, config);
                this.SortingOption = this.GetSortOption(results, config);
                this.SpellingSuggestionSearchRestricted = searchSpellingSuggestionRestricted;
            }
            
            this.SelectedView = selectedView;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the filters.
        /// </summary>
        [DataMember]
        [JsonProperty]
        public FiltersModel FiltersModel { get; set; }

        /// <summary>
        ///     Gets or sets pagination model
        /// </summary>
        [JsonProperty]
        public PaginationModel PaginationModel { get; set; }

        /// <summary>
        ///     Gets or sets the search query.
        /// </summary>
        [Required]
        [JsonProperty]
        public string Query { get; set; }

        /// <summary>
        ///     Gets or sets the selected view.
        /// </summary>
        [JsonProperty]
        public ViewOptionsEnum SelectedView { get; set; }

        /// <summary>
        ///     Gets or sets the sorting option.
        /// </summary>
        [JsonProperty]
        public SortOptionsEnum SortingOption { get; set; }

        /// <summary>
        /// Gets or sets a flag that restricts searching spelling suggestion instead of original query
        /// </summary>
        [JsonProperty]
        public bool SpellingSuggestionSearchRestricted { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Deserializes search model from JSON. In case of exception an empty model is returned.
        /// </summary>
        /// <param name="json">
        /// The JSON.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <returns>
        /// The <see cref="SearchModel"/>.
        /// </returns>
        public static SearchModel FromJson(string json, IConfig config)
        {
            SearchModel model = null;

            if (string.IsNullOrEmpty(json))
            {
                return model;
            }

            try
            {
                model = JsonConvert.DeserializeObject<SearchModel>(HttpUtility.UrlDecode(json)) ?? new SearchModel();
            }
            catch (Exception e)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(e));
                model = new SearchModel(config);
            }

            model.PaginationModel.Config = config;

            return model;
        }

        /// <summary>
        /// The compose search URL for GSA.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string ComposeGsaSearchUrl(IConfig config)
        {
            string gsaUrl = config.GsaSearchBaseUri.AbsoluteUri;

            string filters = this.FiltersModel != null
                                 ? this.FiltersModel.SerializeFiltersToUrlParameters()
                                 : string.Empty;

            string sortParameter = this.SortingOption != SortOptionsEnum.Relevance
                                       ? "&sort="
                                         + HttpUtility.UrlEncode(
                                             Config.Instance.GetSortParameterValue(this.SortingOption))
                                       : string.Empty;

            string url = string.Format(
                "{0}&q={1}&num={2}&start={3}{4}{5}", 
                gsaUrl, 
                HttpUtility.UrlEncode(this.Query), 
                this.PaginationModel.ResultsPerPage, 
                this.PaginationModel.StartItemIndex, 
                filters, 
                sortParameter);

            return url;
        }

        /// <summary>
        ///     Serialize search model to JSON.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string ToJson()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.None);

            return json;
        }

        public IEnumerable<SortingOption> GetAvailableSortingOptions()
        {
            var sortOptions = new List<SortingOption>()
                                  {
                                      new SortingOption() { Label = "Sort by Relevance", Value = SortOptionsEnum.Relevance },
                                      new SortingOption() { Label = "Publish Date (Oldest)", Value = SortOptionsEnum.DateAsc },
                                      new SortingOption() { Label = "Publish Date (Newest)", Value = SortOptionsEnum.DateDesc },
                                      new SortingOption() { Label = "Sort by Title (A-Z)", Value = SortOptionsEnum.TitleAsc },
                                      new SortingOption() { Label = "Sort by Title (Z-A)", Value = SortOptionsEnum.TitleDesc }
                                  };

            var sortByCatalogNumberAsc = new SortingOption() { Label = "Sort by Catalog Number (A-Z)", Value = SortOptionsEnum.CatalogNumberAsc };
            var sortByCatalogNumberDesc = new SortingOption() { Label = "Sort by Catalog Number (Z-A)", Value = SortOptionsEnum.CatalogNumberDesc };
            var sortByArtistAsc = new SortingOption() { Label = "Sort by Artist (A-Z)", Value = SortOptionsEnum.ArtistAsc };
            var sortByArtistDesc = new SortingOption() { Label = "Sort by Artist (Z-A)", Value = SortOptionsEnum.ArtistDesc };
            //var sortByYearAsc = new SortingOption() { Label = "Release Year (Oldest)", Value = SortOptionsEnum.YearAsc };
            //var sortByYearDesc = new SortingOption() { Label = "Release Year (Newest)", Value = SortOptionsEnum.YearDesc };

            switch (this.FiltersModel.ContentType)
            {
                case ContentTypesEnum.Album:
                    sortOptions.Add(sortByArtistAsc);
                    sortOptions.Add(sortByArtistDesc);
                    sortOptions.Add(sortByCatalogNumberAsc);
                    sortOptions.Add(sortByCatalogNumberDesc);
                    //sortOptions.Add(sortByYearAsc);
                    //sortOptions.Add(sortByYearDesc);
                    break;
                case ContentTypesEnum.Track:
                    sortOptions.Add(sortByArtistAsc);
                    sortOptions.Add(sortByArtistDesc);
                    //sortOptions.Add(sortByYearAsc);
                    //sortOptions.Add(sortByYearDesc);
                    break;
                case ContentTypesEnum.Mixed:
                    sortOptions.Add(sortByArtistAsc);
                    sortOptions.Add(sortByArtistDesc);
                    sortOptions.Add(sortByCatalogNumberAsc);
                    sortOptions.Add(sortByCatalogNumberDesc);
                    //sortOptions.Add(sortByYearDesc);
                    //sortOptions.Add(sortByYearAsc);
                    break;
                default:
                    break;
            }

            return sortOptions;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get applied sorting option from GSA results.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <returns>
        /// The <see cref="SortOptionsEnum"/>.
        /// </returns>
        private SortOptionsEnum GetSortOption(GSP result, IConfig config)
        {
            GSPPARAM valueSerialized = result != null && result.PARAM != null ? 
                                                    result.PARAM.FirstOrDefault(p => p.name.Equals("sort", StringComparison.OrdinalIgnoreCase)) : 
                                                    null;

            return valueSerialized != null
                       ? config.GetSortingOptionFromParameter(valueSerialized.value)
                       : SortOptionsEnum.Relevance;
        }

        /// <summary>
        /// Method to call after deserialization to ensure some properties are initialized
        /// </summary>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (this.FiltersModel == null)
            {
                this.FiltersModel = new FiltersModel();
            }

            if (this.PaginationModel == null)
            {
                this.PaginationModel = new PaginationModel();
            }
        }

        #endregion
    }
}