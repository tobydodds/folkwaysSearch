namespace SmithsonianSearch.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The related queries model.
    /// </summary>
    public class RelatedQueriesModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedQueriesModel"/> class.
        /// </summary>
        public RelatedQueriesModel()
        {
            this.RelatedQueries = new string[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedQueriesModel"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        public RelatedQueriesModel(GSP result)
        {
            this.Initialize(result);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the related queries.
        /// </summary>
        public IEnumerable<string> RelatedQueries { get; set; }

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
            if (result != null && result.Synonyms != null && result.Synonyms.Any())
            {
                this.RelatedQueries = result.Synonyms.Select(s => s.Value).ToList();
            }
            else
            {
                this.RelatedQueries = new string[0];
            }
        }

        #endregion
    }
}