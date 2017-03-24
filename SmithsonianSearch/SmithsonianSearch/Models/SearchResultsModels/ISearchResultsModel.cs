namespace SmithsonianSearch.Models.SearchResultsModels
{
    /// <summary>
    /// The SearchResultsModel interface.
    /// </summary>
    public interface ISearchResultsModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the search model.
        /// </summary>
        SearchModel SearchModel { get; set; }

        /// <summary>
        /// Gets number of results in this set.
        /// </summary>
        int ResultsCount { get; }

        /// <summary>
        /// Gets or sets related queries model.
        /// </summary>
        RelatedQueriesModel RelatedQueries { get; set; }

        /// <summary>
        /// Gets or sets related spelling suggestions model.
        /// </summary>
        SpellingSuggestionsModel SpellingSuggestions { get; set; }

        /// <summary>
        /// Gets or sets a flag that indicates if the results are for the spelling suggestion
        /// </summary>
        bool IsSpellingSuggestionSearch { get; set; }

        /// <summary>
        /// Gets or sets original serach query
        /// </summary>
        string OriginallySpelledQuery { get; set; }

        #endregion
    }
}