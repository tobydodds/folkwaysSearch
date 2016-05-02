namespace Smithsonian.Search.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Routing;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using SmithsonianSearch.Configuration;
    using SmithsonianSearch.Helpers;
    using SmithsonianSearch.Models;
    using SmithsonianSearch.Models.Enums;

    [TestClass]
    public class SearchLinksHelperTests
    {
        #region Constructors and Destructors

        static SearchLinksHelperTests()
        {
            RouteTable.Routes.MapPageRoute("Default", "{controller}/{action}", "~/test.aspx");
        }

        #endregion

        #region Public Properties

        public string JsonSearchModelUrlParamName
        {
            get
            {
                return "jsonsearchmodel";
            }
        }

        #endregion

        #region Properties

        private string BaseUrl
        {
            get
            {
                return "http://test.dev";
            }
        }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void GetClearFiltersUrl_SomeFiltersApplied_NoFiltersAppliedAndOtherFieldsNotChanged()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { "a", 
                                            new List<Filter> { 
                                                new Filter() { Title = "b" },
                                                new Filter() { Title = "c" },
                                                new Filter() { Title = "d" }
                                            } 
                                        },
                                        { "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" }
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             "lorem",
                                             new HashSet<string> { "ipsum", "dolor" }
                                         },
                                         { "sit", new HashSet<string> { "amet" } }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;
            var appliedFiltersToCheck = new Dictionary<string, HashSet<string>>();

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetClearFiltersUrl(this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFiltersToCheck);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void
            GetFilterUrl_FilterToBeExcludedAndNotPreviouslyApplied_FilterAppliedAndStartItemIndexResetAndOtherFieldsNotChanged
            ()
        {
            string filterName = "genre";
            string filterValue = "folk";

            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { filterName, new List<Filter> { new Filter() { Title = filterValue } } }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         { "lorem", new HashSet<string> { "ipsum" } }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;

            // Filter should be excluded from applied filters
            var appliedFiltersToCheck = new Dictionary<string, HashSet<string>>
                                            {
                                                {
                                                    "lorem",
                                                    new HashSet<string> { "ipsum" }
                                                }
                                            };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetFilterUrl(filterName, filterValue, false, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, ContentTypesEnum.Mixed);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFiltersToCheck);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void
            GetFilterUrl_FilterToBeExcludedAndPreviouslyApplied_FilterAppliedAndStartItemIndexResetAndOtherFieldsNotChanged
            ()
        {
            string filterName = "genre";
            string filterValue = "folk";

            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { filterName, new List<Filter> { new Filter() { Title = filterValue } } }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             filterName,
                                             new HashSet<string> { filterValue }
                                         },
                                         {
                                             "lorem", new HashSet<string> { "ipsum" }
                                         }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;

            // Filter should be excluded from applied filters
            var appliedFiltersToCheck = new Dictionary<string, HashSet<string>>
                                            {
                                                {
                                                    "lorem",
                                                    new HashSet<string> { "ipsum" }
                                                }
                                            };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetFilterUrl(filterName, filterValue, false, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, ContentTypesEnum.Mixed);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFiltersToCheck);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void
            GetFilterUrl_FilterToBeIncludedAndNotPreviouslyApplied_FilterAppliedAndStartItemIndexResetAndOtherFieldsNotChanged
            ()
        {
            string filterName = "genre";
            string filterValue = "folk";

            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { filterName, new List<Filter> { new Filter() { Title = filterValue } } }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         { "lorem", new HashSet<string> { "ipsum" } }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;
            var appliedFiltersToCheck = new Dictionary<string, HashSet<string>>
                                            {
                                                {
                                                    filterName,
                                                    new HashSet<string>
                                                        {
                                                            filterValue
                                                        }
                                                },
                                                {
                                                    "lorem",
                                                    new HashSet<string> { "ipsum" }
                                                }
                                            };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetFilterUrl(filterName, filterValue, true, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, ContentTypesEnum.Mixed);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFiltersToCheck);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void
            GetFilterUrl_FilterToBeIncludedAndPreviouslyApplied_FilterAppliedAndStartItemIndexResetAndOtherFieldsNotChanged
            ()
        {
            string filterName = "genre";
            string filterValue = "folk";

            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { filterName, new List<Filter> { new Filter() { Title = filterValue } } }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         { "lorem", new HashSet<string> { "ipsum" } },
                                         {
                                             filterName,
                                             new HashSet<string>
                                                 {
                                                     filterValue,
                                                     "a",
                                                     "b"
                                                 }
                                         }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;
            var appliedFiltersToCheck = new Dictionary<string, HashSet<string>>
                                            {
                                                {
                                                    filterName,
                                                    new HashSet<string>
                                                        {
                                                            filterValue,
                                                            "a",
                                                            "b"
                                                        }
                                                },
                                                {
                                                    "lorem",
                                                    new HashSet<string> { "ipsum" }
                                                }
                                            };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetFilterUrl(filterName, filterValue, true, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, ContentTypesEnum.Mixed);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFiltersToCheck);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetNextPageUrl__StartItemIndexChanged()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "a", 
                                            new List<Filter> { 
                                                new Filter() { Title = "b" },
                                                new Filter() { Title = "c" },
                                                new Filter() { Title = "d" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             "lorem",
                                             new HashSet<string> { "ipsum", "dolor" }
                                         },
                                         { "sit", new HashSet<string> { "amet" } }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 75;

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetNextPageUrl(this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/NextPage?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetRelatedQueryUrl_RelatedQueryWithTags_QueryChangedAndStartItemIndexReset()
        {
            string query = "Katarina Muller";
            string relatedQuery = "Katarina <b><i>Miller<i></b>";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "a", 
                                            new List<Filter> { 
                                                new Filter() { Title = "b" },
                                                new Filter() { Title = "c" },
                                                new Filter() { Title = "d" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             "lorem",
                                             new HashSet<string> { "ipsum", "dolor" }
                                         }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;
            string queryToCheck = "Katarina Miller";

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetRelatedQueryUrl(relatedQuery, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, queryToCheck);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetOriginallySpelledQueryUrl_Query_QueryChangedAndStartItemIndexReset()
        {
            string query = "cuban music";
            string originallySpelledQuery = "cubban music";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "a", 
                                            new List<Filter> { 
                                                new Filter() { Title = "b" },
                                                new Filter() { Title = "c" },
                                                new Filter() { Title = "d" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             "lorem",
                                             new HashSet<string> { "ipsum", "dolor" }
                                         }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;
            string queryToCheck = "cubban music";

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetOriginallySpelledQueryUrl(originallySpelledQuery, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            Assert.IsTrue(deserializedModel.SpellingSuggestionSearchRestricted, "SpellingSuggestionSearchRestricted is false.");
            this.CheckQuery(deserializedModel, queryToCheck);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetSearchModelForNewSearch_SearchModel_StartItemIndexResetSpellingFlagResetFiltersCleared()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>> { { "sit", new HashSet<string> { "amet" } } };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            SearchModel createdSearchModel = model.GetSearchModelForNewSearch();

            Assert.IsNotNull(createdSearchModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(createdSearchModel, query);
            this.CheckContentType(createdSearchModel, contentType);
            this.CheckSelectedView(createdSearchModel, selectedView);
            this.CheckSortingOption(createdSearchModel, sortingOption);
            this.CheckStartItemIndex(createdSearchModel, 0);
            this.CheckResultsPerPage(createdSearchModel, resultsPerPage);
            this.CheckAppliedFilters(createdSearchModel, new Dictionary<string, HashSet<string>>());
            this.CheckAvailableContentTypes(createdSearchModel, new ContentTypesEnum[0]);
            this.CheckSpellingSuggestionRestricted(createdSearchModel, false);
        }

        [TestMethod]
        public void GetRouteValueDictionary_NotResetStartItemIndex_JsonSearchModelAvailable()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>> { { "sit", new HashSet<string> { "amet" } } };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            RouteValueDictionary routeValueDictionary = model.GetRouteValueDictionary();

            Assert.IsNotNull(routeValueDictionary, "Result is null");

            string key = "JsonSearchModel";
            Assert.IsTrue(routeValueDictionary.ContainsKey(key), key + " is not in route value dictionary");
            Assert.IsNotNull(routeValueDictionary[key], "JSON search model is null");

            var deserializedModel = JsonConvert.DeserializeObject<SearchModel>(routeValueDictionary[key] as string);

            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndex);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetRouteValueObject_DoNotResetStartItemIndex_JsonSearchModelIsAvailableAndValid()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>> { { "sit", new HashSet<string> { "amet" } } };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            object routeValueObject = model.GetRouteValueObject();

            Assert.IsNotNull(routeValueObject, "Result is null");

            var routeValueDictionary = new RouteValueDictionary(routeValueObject);

            string key = "JsonSearchModel";
            Assert.IsTrue(routeValueDictionary.ContainsKey(key), key + " is not in route value dictionary");
            Assert.IsNotNull(routeValueDictionary[key], "JSON search model is null");

            var deserializedModel = JsonConvert.DeserializeObject<SearchModel>(routeValueDictionary[key] as string);

            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndex);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetRouteValueObject_ResetStartItemIndex_JsonSearchModelIsAvailableAndValid()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>> { { "sit", new HashSet<string> { "amet" } } };

            int startItemIndexToCheck = 0;

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            object routeValueObject = model.GetRouteValueObject(true);

            Assert.IsNotNull(routeValueObject, "Result is null");

            var routeValueDictionary = new RouteValueDictionary(routeValueObject);

            string key = "JsonSearchModel";
            Assert.IsTrue(routeValueDictionary.ContainsKey(key), key + " is not in route value dictionary");
            Assert.IsNotNull(routeValueDictionary[key], "JSON search model is null");

            var deserializedModel = JsonConvert.DeserializeObject<SearchModel>(routeValueDictionary[key] as string);

            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetSortingOptionUrl_AnotherSortingOption_CorrectSortingOptionInUrlAndStartItemIndexReset()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             "lorem",
                                             new HashSet<string> { "ipsum", "dolor" }
                                         }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;
            var sortingOptionToCheck = SortOptionsEnum.DateAsc;

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetSortingOptionUrl(sortingOptionToCheck, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, contentType);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOptionToCheck);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetTabUrl_AnotherContentType_CorrectContentTypeAndStartItemIndexReset()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" },
                                            } 
                                        }
                                    };

            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             "lorem",
                                             new HashSet<string> { "ipsum", "dolor" }
                                         }
                                     };

            // Start item index should be reset to 0
            int startItemIndexToCheck = 0;
            var contentTypeToCheck = ContentTypesEnum.Playlist;

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            string url = model.GetTabUrl(contentTypeToCheck, this.GetRequestContext());

            SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

            Assert.IsTrue(
                url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                "Controller and action in the URL do not match.");
            Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
            this.CheckQuery(deserializedModel, query);
            this.CheckContentType(deserializedModel, contentTypeToCheck);
            this.CheckSelectedView(deserializedModel, selectedView);
            this.CheckSortingOption(deserializedModel, sortingOption);
            this.CheckStartItemIndex(deserializedModel, startItemIndexToCheck);
            this.CheckResultsPerPage(deserializedModel, resultsPerPage);
            this.CheckAppliedFilters(deserializedModel, appliedFilters);
            this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
            this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
        }

        [TestMethod]
        public void GetViewOptionsUrls__CorrectViewOptionAndPageSize()
        {
            string query = "jazz";
            var contentType = ContentTypesEnum.Album;
            var selectedView = ViewOptionsEnum.Table;
            var sortingOption = SortOptionsEnum.DateDesc;
            int resultsPerPage = 25;
            int startItemIndex = 50;
            bool spellingSuggestionSearchRestricted = true;
            var filterOptions = new Dictionary<string, List<Filter>>
                                    {
                                        { 
                                            "f", 
                                            new List<Filter> { 
                                                new Filter() { Title = "g" },
                                                new Filter() { Title = "h" },
                                                new Filter() { Title = "i" },
                                            } 
                                        },
                                        { 
                                            "a", 
                                            new List<Filter> { 
                                                new Filter() { Title = "b" },
                                                new Filter() { Title = "c" },
                                                new Filter() { Title = "d" },
                                            } 
                                        }
                                    };
            var availableContentTypes = new[] { ContentTypesEnum.Mixed, ContentTypesEnum.Album, ContentTypesEnum.Track };
            var appliedFilters = new Dictionary<string, HashSet<string>>
                                     {
                                         {
                                             "lorem",
                                             new HashSet<string> { "ipsum", "dolor" }
                                         }
                                     };

            SearchModel model = this.GetSearchModel(
                query,
                contentType,
                selectedView,
                sortingOption,
                resultsPerPage,
                startItemIndex,
                spellingSuggestionSearchRestricted,
                filterOptions,
                appliedFilters,
                availableContentTypes);

            IDictionary<ViewOptionsEnum, string> urls = model.GetViewOptionsUrls(this.GetRequestContext());
            Assert.IsNotNull(urls);

            int numberOfViewOptions = Enum.GetValues(typeof(ViewOptionsEnum)).Length;
            Assert.IsTrue(urls.Count == numberOfViewOptions, "URLs are provided not for all view options.");

            foreach (var pair in urls)
            {
                ViewOptionsEnum viewOption = pair.Key;
                string url = pair.Value;
                int resultsPerPageToCheck = viewOption == ViewOptionsEnum.Table
                                                ? Config.Instance.PageSizeTableView
                                                : viewOption == ViewOptionsEnum.Tiles
                                                      ? Config.Instance.PageSizeTilesView
                                                      : resultsPerPage;

                SearchModel deserializedModel = this.GetSearchModelFromUrl(url);

                Assert.IsTrue(
                    url.IndexOf("Search/Search?", 0, StringComparison.OrdinalIgnoreCase) != -1,
                    "Controller and action in the URL do not match.");
                Assert.IsNotNull(deserializedModel, "Search model failed to be deserialized from JSON.");
                this.CheckQuery(deserializedModel, query);
                this.CheckContentType(deserializedModel, contentType);
                this.CheckSelectedView(deserializedModel, viewOption);
                this.CheckSortingOption(deserializedModel, sortingOption);
                this.CheckStartItemIndex(deserializedModel, startItemIndex);
                this.CheckResultsPerPage(deserializedModel, resultsPerPageToCheck);
                this.CheckAppliedFilters(deserializedModel, appliedFilters);
                this.CheckAvailableContentTypes(deserializedModel, availableContentTypes);
                this.CheckSpellingSuggestionRestricted(deserializedModel, spellingSuggestionSearchRestricted);
            }
        }

        #endregion

        #region Methods

        private void CheckAppliedFilter(SearchModel model, string name, string value)
        {
            Assert.IsTrue(
                model.FiltersModel.AppliedFilters.ContainsKey(name),
                string.Format("Filter {0} with value {1} is not applied. Such filter name was not found.", name, value));
            Assert.IsTrue(
                model.FiltersModel.AppliedFilters[name].Contains(value),
                string.Format("Filter {0} with value {1} is not applied. Such filter value was not found.", name, value));
        }

        private void CheckAppliedFilters(SearchModel model, IDictionary<string, HashSet<string>> appliedFilters)
        {
            if (appliedFilters == null || !appliedFilters.Any())
            {
                Assert.IsTrue(model.FiltersModel.AppliedFilters.Any() == false, "There are unexpected applied filters.");
                return;
            }

            Assert.IsTrue(
                appliedFilters.Sum(f => f.Value.Count) == model.FiltersModel.AppliedFilters.Sum(f => f.Value.Count),
                "The number of applied filters doesn't match");

            foreach (var appliedFilter in appliedFilters)
            {
                foreach (string value in appliedFilter.Value)
                {
                    this.CheckAppliedFilter(model, appliedFilter.Key, value);
                }
            }
        }

        private void CheckAvailableContentTypes(SearchModel model, IEnumerable<ContentTypesEnum> availableContentTypes)
        {
            Assert.IsTrue(
                model.FiltersModel.AvailableContentTypes.Count() == availableContentTypes.Count(),
                "Number of available content types does not match.");
            foreach (ContentTypesEnum ct in availableContentTypes)
            {
                Assert.IsTrue(
                    model.FiltersModel.AvailableContentTypes.Contains(ct),
                    string.Format("Content type {0} is not marked as available in the model.", ct));
            }
        }

        private void CheckContentType(SearchModel model, ContentTypesEnum contentType)
        {
            Assert.IsTrue(model.FiltersModel.ContentType == contentType, "Content type does not match");
        }

        private void CheckFilterOptions(SearchModel model, Dictionary<string, List<string>> filterOptions)
        {
            Assert.IsTrue(
                model.FiltersModel.FiltersOptions.Count == filterOptions.Count,
                "The number of available filters doesn't match.");

            foreach (var filter in filterOptions)
            {
                Assert.IsTrue(
                    model.FiltersModel.FiltersOptions.ContainsKey(filter.Key),
                    string.Format("Filter {0} was not found in model.", filter.Key));
                Assert.IsTrue(
                    model.FiltersModel.FiltersOptions[filter.Key].Count == filter.Value.Count,
                    string.Format("Number of values for filter {0} doesn't match.", filter.Key));
                foreach (string value in filter.Value)
                {
                    Assert.IsTrue(
                        model.FiltersModel.FiltersOptions[filter.Key].Select(fo => fo.Title).Contains(value),
                        string.Format("Model doesn't contain value {0} for filter {1}", value, filter.Key));
                }
            }
        }

        private void CheckQuery(SearchModel model, string query)
        {
            Assert.IsTrue(model.Query.Equals(query), "Query does not match.");
        }

        private void CheckResultsPerPage(SearchModel model, int resultsPerPage)
        {
            Assert.IsTrue(
                model.PaginationModel.ResultsPerPage == resultsPerPage,
                "Number of results per page doesn't match");
        }

        private void CheckSelectedView(SearchModel model, ViewOptionsEnum selectedView)
        {
            Assert.IsTrue(model.SelectedView == selectedView, "Selected view does not match");
        }

        private void CheckSpellingSuggestionRestricted(SearchModel model, bool valueToCheck)
        {
            Assert.AreEqual(valueToCheck, model.SpellingSuggestionSearchRestricted);
        }

        private void CheckSortingOption(SearchModel model, SortOptionsEnum sortingOption)
        {
            Assert.IsTrue(model.SortingOption == sortingOption, "Sorting option does not match");
        }

        private void CheckStartItemIndex(SearchModel model, int startItemIndex)
        {
            Assert.IsTrue(model.PaginationModel.StartItemIndex == startItemIndex, "Start item index doesn't match.");
        }

        private RequestContext GetRequestContext()
        {
            var httpRequest = new HttpRequest("~/test.aspx", "http://test.dev/test.aspx", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var httpContextWrapper = new HttpContextWrapper(httpContext);
            var routeData = new RouteData { RouteHandler = new PageRouteHandler("~/test.aspx"), };

            return new RequestContext(httpContextWrapper, routeData);
        }

        private SearchModel GetSearchModel(
            string query,
            ContentTypesEnum contentType = 0,
            ViewOptionsEnum selectedView = 0,
            SortOptionsEnum sortingOption = 0,
            int resultsPerPage = 0,
            int startItemIndex = 0,
            bool searchSpellingSuggestionRestricted = false,
            Dictionary<string, List<Filter>> filterOptions = null,
            Dictionary<string, HashSet<string>> appliedFilters = null,
            IEnumerable<ContentTypesEnum> availableContentTypes = null)
        {
            var paginationModel = new PaginationModel
                                      {
                                          ResultsPerPage = resultsPerPage,
                                          StartItemIndex = startItemIndex,
                                      };

            var filtersModel = new FiltersModel
                                   {
                                       FiltersOptions =
                                           filterOptions ?? new Dictionary<string, List<Filter>>(),
                                       AvailableContentTypes =
                                           availableContentTypes ?? new ContentTypesEnum[0],
                                       AppliedFilters =
                                           appliedFilters ?? new Dictionary<string, HashSet<string>>(),
                                       ContentType = contentType
                                   };

            return new SearchModel
                       {
                           Query = query,
                           SelectedView = selectedView,
                           PaginationModel = paginationModel,
                           FiltersModel = filtersModel,
                           SortingOption = sortingOption,
                           SpellingSuggestionSearchRestricted = searchSpellingSuggestionRestricted
                       };
        }

        private SearchModel GetSearchModelFromUrl(string url)
        {
            var uri = new Uri(this.BaseUrl + url);
            string jsonSearchModel = HttpUtility.ParseQueryString(uri.Query).Get(this.JsonSearchModelUrlParamName);
            var deserializedModel = JsonConvert.DeserializeObject<SearchModel>(jsonSearchModel);

            return deserializedModel;
        }

        #endregion
    }
}