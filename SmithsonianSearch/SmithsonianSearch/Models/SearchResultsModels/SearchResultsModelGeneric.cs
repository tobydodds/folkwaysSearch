namespace SmithsonianSearch.Models.SearchResultsModels
{
    using System.Collections.Generic;
    using System.Linq;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.ResultItems;

    /// <summary>
    /// The search results model generic.
    /// The model defines basic properties and costructors for other search results models.
    /// </summary>
    /// <typeparam name="T">
    /// T must be of type <see cref="GenericResultItem"/> or derived from <see cref="GenericResultItem"/>
    /// </typeparam>
    public class SearchResultsModelGeneric<T> : ISearchResultsModel
        where T : GenericResultItem, new()
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchResultsModelGeneric{T}" /> class.
        /// </summary>
        public SearchResultsModelGeneric()
        {
            this.Items = new T[0];
            this.SearchModel = new SearchModel();
            this.RelatedQueries = new RelatedQueriesModel();
            this.SpellingSuggestions = new SpellingSuggestionsModel();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelGeneric{T}"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchResultsModelGeneric(IConfig config)
        {
            this.Items = new T[0];
            this.SearchModel = new SearchModel(config);
            this.RelatedQueries = new RelatedQueriesModel();
            this.SpellingSuggestions = new SpellingSuggestionsModel();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResultsModelGeneric{T}"/> class.
        /// </summary>
        /// <param name="results">
        /// The results.
        /// </param>
        /// <param name="searchModel">
        /// The search model.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public SearchResultsModelGeneric(GSP results, SearchModel searchModel, IConfig config)
        {
            if (results == null)
            {
                return;
            }

            this.Items = new T[0];
            this.SearchModel = new SearchModel(
                results, 
                searchModel.SelectedView, 
                searchModel.FiltersModel.AvailableContentTypes,
                searchModel.SpellingSuggestionSearchRestricted,
                config);
            this.RelatedQueries = new RelatedQueriesModel(results);
            this.SpellingSuggestions = new SpellingSuggestionsModel(results);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        ///     Gets or sets the related queries.
        /// </summary>
        public RelatedQueriesModel RelatedQueries { get; set; }

        /// <summary>
        ///     Gets the results count.
        /// </summary>
        public int ResultsCount
        {
            get
            {
                return this.Items != null ? this.Items.Count() : 0;
            }
        }

        /// <summary>
        ///     Gets or sets the search model.
        /// </summary>
        public SearchModel SearchModel { get; set; }

        /// <summary>
        ///     Gets or sets related spelling suggestions model.
        /// </summary>
        public SpellingSuggestionsModel SpellingSuggestions { get; set; }

        /// <summary>
        /// Gets or sets a flag that indicates if the results are for the spelling suggestion
        /// </summary>
        public bool IsSpellingSuggestionSearch { get; set; }

        /// <summary>
        /// Gets or sets original serach query
        /// </summary>
        public string OriginallySpelledQuery { get; set; }

        #endregion
    }
}