namespace SmithsonianSearch.Models.ResultItems
{
    using System.Collections.Generic;

    using SmithsonianSearch.Configuration;

    /// <summary>
    /// The tool for teaching.
    /// Model class that represents an item of 'tool for teaching' content type.
    /// </summary>
    public class ToolForTeaching : GenericResultItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolForTeaching"/> class.
        /// </summary>
        public ToolForTeaching()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolForTeaching"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public ToolForTeaching(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolForTeaching"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        public ToolForTeaching(GSPRESR result, IConfig config)
            : base(result, config)
        {
            this.Initialize(result, config);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the grades.
        /// </summary>
        public string Grades { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initialize custom properties from GSA results.
        /// Initialize new properties from metadata here.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        private void Initialize(GSPRESR result, IConfig config)
        {
            if (result != null && result.MT != null && result.MT.Length > 0)
            {
                IDictionary<string, string> tempCollection = this.GetAttributes(result);

                string key = "region";
                if (tempCollection.ContainsKey(key))
                {
                    this.Region = tempCollection[key];
                }

                key = "country";
                if (tempCollection.ContainsKey(key))
                {
                    this.Country = tempCollection[key];
                }

                key = "gradelevel";
                if (tempCollection.ContainsKey(key))
                {
                    this.Grades = tempCollection[key];
                }
            }
        }

        #endregion
    }
}