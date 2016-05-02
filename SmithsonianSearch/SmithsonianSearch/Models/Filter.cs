namespace SmithsonianSearch.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Filter attribute
    /// </summary>
    public class Filter
    {
        public string Title { get; set; }

        /// <summary>
        /// Number of search results that have this filter attribute
        /// </summary>
        public int ResultsCount { get; set; }
    }
}