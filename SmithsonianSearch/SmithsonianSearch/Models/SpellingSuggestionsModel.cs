namespace SmithsonianSearch.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The related queries model.
    /// </summary>
    public class SpellingSuggestionsModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellingSuggestionsModel"/> class. 
        ///     Initializes a new instance of the <see cref="RelatedQueriesModel"/> class.
        /// </summary>
        public SpellingSuggestionsModel()
        {
            this.SpellingSuggestions = new string[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellingSuggestionsModel"/> class. 
        /// Initializes a new instance of the <see cref="RelatedQueriesModel"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        public SpellingSuggestionsModel(GSP result)
        {
            this.Initialize(result);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the related queries.
        /// </summary>
        public IEnumerable<string> SpellingSuggestions { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        private void Initialize(GSP result)
        {
            if (result != null && result.Spelling != null && result.Spelling.Any())
            {
                this.SpellingSuggestions = result.Spelling.Select(s => s.Value).ToList();
            }
            else
            {
                this.SpellingSuggestions = new string[0];
            }
        }

        #endregion
    }
}