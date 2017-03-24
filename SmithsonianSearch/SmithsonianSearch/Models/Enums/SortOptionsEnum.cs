namespace SmithsonianSearch.Models.Enums
{
    /// <summary>
    /// The sort options offered by GSA.
    /// </summary>
    public enum SortOptionsEnum
    {
        /// <summary>
        /// Sort by relevance.
        /// </summary>
        Relevance = 0, 

        /// <summary>
        /// Sort by date in ascending order.
        /// </summary>
        DateAsc = 1, 

        /// <summary>
        /// Sort by date in descending order.
        /// </summary>
        DateDesc = 2,

        /// <summary>
        /// Sort by artist in ascending order.
        /// </summary>
        ArtistAsc = 3,

        /// <summary>
        /// Sort by artist in descending order.
        /// </summary>
        ArtistDesc = 4,

        /// <summary>
        /// Sort by catalog number in ascending order.
        /// </summary>
        CatalogNumberAsc = 5,

        /// <summary>
        /// Sort by catalog number in descending order.
        /// </summary>
        CatalogNumberDesc = 6,

        /// <summary>
        /// Sort by title in ascending order.
        /// </summary>
        TitleAsc = 7,

        /// <summary>
        /// Sort by title in descending order.
        /// </summary>
        TitleDesc = 8,

        /// <summary>
        /// Sort by year in ascending order.
        /// </summary>
        YearAsc = 9,

        /// <summary>
        /// Sort by year in descending order.
        /// </summary>
        YearDesc = 10
    }
}