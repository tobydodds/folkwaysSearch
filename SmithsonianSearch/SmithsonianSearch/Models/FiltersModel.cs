namespace SmithsonianSearch.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Newtonsoft.Json;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Models.Enums;

    /// <summary>
    ///     The filters model.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FiltersModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FiltersModel" /> class.
        /// </summary>
        public FiltersModel()
        {
            this.AppliedFilters = new Dictionary<string, HashSet<string>>();
            this.FiltersOptions = new Dictionary<string, List<Filter>>();
            this.AvailableContentTypes = new List<ContentTypesEnum>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersModel"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="availableContentTypes">
        /// The available content types.
        /// </param>
        public FiltersModel(GSP result, IEnumerable<ContentTypesEnum> availableContentTypes)
            : this()
        {
            if (result == null)
            {
                return;
            }

            this.ParseAppliedFilters(result);

            if (result.RES != null && result.RES.PARM != null)
            {
                this.SetFiltersOptions(result.RES.PARM);
            }

            if (availableContentTypes != null && 
                availableContentTypes.Any() && 
                this.ContentType != ContentTypesEnum.Mixed && 
                Config.Instance.FilteringTabsStrategy != Config.FilteringTabsStrategyEnum.HideOtherTabs)
            {
                this.AvailableContentTypes = availableContentTypes.ToList();
            }
            else if (result.RES != null && result.RES.PARM != null && result.RES.PARM.PMT != null)
            {
                this.SetAvailableContentTypes(result.RES.PARM.PMT);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersModel"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        public FiltersModel(FiltersModel model)
            : this()
        {
            if (model == null)
            {
                return;
            }

            this.ContentType = model.ContentType;

            this.FiltersOptions = new Dictionary<string, List<Filter>>();
            if (model.FiltersOptions != null)
            {
                foreach (var filtersOption in model.FiltersOptions)
                {
                    this.FiltersOptions.Add(filtersOption.Key, new List<Filter>(filtersOption.Value));
                }
            }

            this.AppliedFilters = new Dictionary<string, HashSet<string>>();
            if (model.AppliedFilters != null)
            {
                foreach (var filter in model.AppliedFilters)
                {
                    this.AppliedFilters.Add(filter.Key, new HashSet<string>(filter.Value));
                }
            }

            this.AvailableContentTypes = model.AvailableContentTypes != null
                                             ? model.AvailableContentTypes.ToList()
                                             : new List<ContentTypesEnum>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the filters.
        /// </summary>
        [JsonProperty]
        public IDictionary<string, HashSet<string>> AppliedFilters { get; set; }

        /// <summary>
        ///     Gets or sets available content types
        /// </summary>
        [JsonProperty]
        public IEnumerable<ContentTypesEnum> AvailableContentTypes { get; set; }

        /// <summary>
        ///     Gets or sets the content type.
        /// </summary>
        [JsonProperty]
        public ContentTypesEnum ContentType { get; set; }

        /// <summary>
        ///     Gets the content type parameter name.
        /// </summary>
        public string ContentTypeParamName
        {
            get
            {
                return "ContentType";
            }
        }

        /// <summary>
        ///     Gets the filters join operator.
        /// </summary>
        public char FiltersJoinOperator
        {
            get
            {
                return '.';
            }
        }

        /// <summary>
        ///     Gets or sets the filters options.
        /// </summary>
        public Dictionary<string, List<Filter>> FiltersOptions { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Checks if filter is applied.
        /// </summary>
        /// <param name="filterName">
        /// The filter name.
        /// </param>
        /// <param name="option">
        /// Filter option title
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsFilterApplied(string filterName, string option)
        {
            bool isApplied = false;

            if (this.AppliedFilters.ContainsKey(filterName))
            {
                isApplied = this.AppliedFilters[filterName].Contains(option);
            }

            return isApplied;
        }

        /// <summary>
        ///     The serialize filters to url parameters.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string SerializeFiltersToUrlParameters()
        {
            if (this.AppliedFilters == null)
            {
                return string.Empty;
            }

            List<KeyValuePair<string, HashSet<string>>> parameters = this.AppliedFilters.ToList();
            if (this.ContentType != 0)
            {
                parameters.Add(
                    new KeyValuePair<string, HashSet<string>>(
                        this.ContentTypeParamName, 
                        new HashSet<string> { Enum.GetName(typeof(ContentTypesEnum), this.ContentType) }));
            }

            string[] urlParams =
                parameters.SelectMany(
                    f => f.Value.Select(value => string.Format("{0}:{1}", f.Key, this.EncodeGsaSymbols(value))))
                    .ToArray();

            string result = parameters.Count > 0
                                ? "&requiredfields="
                                  + HttpUtility.UrlEncode(string.Join(this.FiltersJoinOperator.ToString(), urlParams))
                                : string.Empty;

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Replace GSA-meaningful characters that are %-encoded with their %-decoded variants (., -, |)
        /// </summary>
        /// <param name="valueToDecode">
        /// Filter value to decode
        /// </param>
        /// <returns>
        /// Value where %-encoded characters (., -, |) are replaced with their %-decoded variants
        /// </returns>
        private string DecodeGsaSymbols(string valueToDecode)
        {
            return
                valueToDecode.Replace("%2E", ".")
                    .Replace("%2D", "-")
                    .Replace("%7C", "|")
                    .Replace("%28", "(")
                    .Replace("%29", ")");
        }

        /// <summary>
        /// Replace GSA-meaningful characters (., -, |) with their %-encoded variants
        /// </summary>
        /// <param name="valueToEncode">
        /// Filter value to encode
        /// </param>
        /// <returns>
        /// Value where ., -, | are replaced with their %-encoded variants
        /// </returns>
        private string EncodeGsaSymbols(string valueToEncode)
        {
            return
                valueToEncode.Replace(".", "%2E")
                    .Replace("-", "%2D")
                    .Replace("|", "%7C")
                    .Replace("(", "%28")
                    .Replace(")", "%29");
        }

        /// <summary>
        /// The parse applied filters.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        private void ParseAppliedFilters(GSP result)
        {
            if (result.PARAM == null || result.PARAM.Length == 0)
            {
                return;
            }

            string serializedFilters =
                result.PARAM.Where(p => p.name.Equals("requiredfields", StringComparison.OrdinalIgnoreCase))
                    .Select(p => p.value)
                    .SingleOrDefault();

            if (!string.IsNullOrEmpty(serializedFilters))
            {
                foreach (string filter in serializedFilters.Split(new[] { this.FiltersJoinOperator }))
                {
                    string[] parts = filter.Split(new[] { ':' });
                    if (parts.Length > 1)
                    {
                        string name = parts[0];
                        string value = parts[1] ?? string.Empty;
                        value = this.DecodeGsaSymbols(value);

                        if (this.ContentTypeParamName.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            ContentTypesEnum contentType = 0;
                            Enum.TryParse(value, true, out contentType);
                            this.ContentType = contentType;
                        }
                        else if (this.AppliedFilters.ContainsKey(name))
                        {
                            if (!this.AppliedFilters[name].Contains(value))
                            {
                                this.AppliedFilters[name].Add(value);
                            }
                        }
                        else
                        {
                            this.AppliedFilters.Add(name, new HashSet<string> { value });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read and set available content types (excluding those that have no results)
        /// </summary>
        /// <param name="attributes">
        /// Dynamic navigation attributes (filters)
        /// </param>
        private void SetAvailableContentTypes(GSPRESPARMPMT[] attributes)
        {
            if (attributes != null && attributes.Length != 0)
            {
                var availableContentTypes = new List<ContentTypesEnum> { ContentTypesEnum.Mixed };

                GSPRESPARMPMT contentTypeOptions =
                    attributes.SingleOrDefault(p => p.NM.Equals(this.ContentTypeParamName, StringComparison.OrdinalIgnoreCase))
                    ?? new GSPRESPARMPMT();
                foreach (GSPRESPARMPMTPV contentType in contentTypeOptions.PV)
                {
                    if (contentType.C > 0)
                    {
                        ContentTypesEnum enumValue;
                        Enum.TryParse(contentType.V, true, out enumValue);
                        if (enumValue != ContentTypesEnum.Mixed)
                        {
                            availableContentTypes.Add(enumValue);
                        }
                    }
                }

                this.AvailableContentTypes = availableContentTypes;
            }
        }

        /// <summary>
        /// The set filters options.
        /// </summary>
        /// <param name="filters">
        /// The filters.
        /// </param>
        private void SetFiltersOptions(GSPRESPARM filters)
        {
            this.FiltersOptions = new Dictionary<string, List<Filter>>();

            foreach (GSPRESPARMPMT metaField in filters.PMT)
            {
                List<Filter> options =
                    metaField.PV.Select(option => new Filter() { Title = option.V, ResultsCount = option.C })
                        .OrderByDescending(option => this.IsFilterApplied(metaField.NM, option.Title))
                        .ToList();
                this.FiltersOptions.Add(metaField.NM, options);
            }
        }

        #endregion
    }
}