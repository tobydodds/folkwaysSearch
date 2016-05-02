namespace SmithsonianSearch.Configuration
{
    using System;

    using SmithsonianSearch.Models.Enums;

    /// <summary>
    ///     The Config interface.
    /// </summary>
    public interface IConfig
    {
        #region Public Properties

        /// <summary>
        ///     Gets the Buy base uri.
        /// </summary>
        Uri BuyBaseUri { get; }

        /// <summary>
        ///     Gets the GSA search base uri.
        /// </summary>
        Uri GsaSearchBaseUri { get; }

        /// <summary>
        ///     Gets the GSA suggest base uri.
        /// </summary>
        Uri GsaSuggestBaseUri { get; }

        /// <summary>
        ///     Gets the images base uri.
        /// </summary>
        Uri ImagesBaseUri { get; }

        /// <summary>
        /// Gets the item field max length.
        /// </summary>
        int ItemFieldMaxLength { get; }

        /// <summary>
        ///     Gets max length of result item's title
        /// </summary>
        int ItemTitleMaxLength { get; }

        /// <summary>
        ///     Gets max length of result item's description
        /// </summary>
        int ItemDescriptionMaxLength { get; }

        /// <summary>
        /// Gets the page size table view.
        /// </summary>
        int PageSizeTableView { get; }

        /// <summary>
        /// Gets the page size tiles view.
        /// </summary>
        int PageSizeTilesView { get; }

        /// <summary>
        /// Gets the configuration setting that defines how to treat spelling suggestion: search it instead of original query or not.
        /// </summary>
        bool SearchSpellingSuggestion { get; }

        Config.FilteringTabsStrategyEnum FilteringTabsStrategy { get; }

        string DefaultSearchPhrase { get; }

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
        string GetSortParameterValue(SortOptionsEnum sortingOption);

        /// <summary>
        /// The get sorting option from parameter.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="SortOptionsEnum"/>.
        /// </returns>
        SortOptionsEnum GetSortingOptionFromParameter(string value);

        #endregion
    }
}